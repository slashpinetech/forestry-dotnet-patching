using Microsoft.Extensions.Options;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// A Microsoft SQL Server implementation of an <see cref="IPatchRecorder"/>.
/// </summary>
public class SqlServerPatchRecorder : PatchRecorderBase
{
    public SqlServerPatchRecorder(IOptions<PatchOptions> options) : base(options)
    {
    }

    protected override string GetCreateTableSql(string tableName)
    {
        return $@"create table [{tableName}] (
  [Id] int not null identity primary key,
  [Name] nvarchar(255) not null,
  [AppliedAt] datetime2 not null default GETDATE()
);";
    }

    protected override string GetInsertAppliedPatchSql(string tableName)
    {
        return $"insert into [{tableName}] ([Name]) values (@Name);";
    }

    protected override string GetAppliedPatchesSql(string tableName)
    {
        return $"select [Name] from [{tableName}];";
    }
}
