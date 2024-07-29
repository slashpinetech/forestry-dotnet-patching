using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// A base implementation for RDBMS-specific implementations.
/// </summary>
public abstract class PatchRecorderBase : IPatchRecorder
{
    private readonly string _connectionString;

    protected PatchRecorderBase(IOptions<PatchOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    public async Task EnsureAppliedPatchTableExistsAsync(string tableName, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var table = connection.GetSchema("TABLES", new[] { null, null, tableName });

        if (table.Rows.Count == 0)
        {
            var createTable = new SqlCommand(GetCreateTableSql(tableName), connection);
            await createTable.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task RecordAppliedPatchAsync(string tableName, string patchName, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        var insert = new SqlCommand(GetInsertAppliedPatchSql(tableName), connection);
        insert.Parameters.Add("@Name", SqlDbType.NVarChar).Value = patchName;
        await insert.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<ISet<string>> GetAppliedPatchNamesAsync(string tableName, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        var query = new SqlCommand(GetAppliedPatchesSql(tableName), connection);
        await using var sdr = await query.ExecuteReaderAsync(cancellationToken);

        var patchNames = new HashSet<string>();
        while (await sdr.ReadAsync(cancellationToken))
        {
            var patchName = sdr["Name"].ToString();
            if (!string.IsNullOrEmpty(patchName))
            {
                patchNames.Add(patchName);
            }
        }

        return patchNames;
    }

    protected abstract string GetCreateTableSql(string tableName);

    protected abstract string GetInsertAppliedPatchSql(string tableName);

    protected abstract string GetAppliedPatchesSql(string tableName);
}
