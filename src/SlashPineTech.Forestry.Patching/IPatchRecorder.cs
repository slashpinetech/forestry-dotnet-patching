using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// Records when a patch is applied.
/// </summary>
public interface IPatchRecorder
{
    /// <summary>
    /// Called to ensure that the applied patches table exists and creates
    /// the table if it does not exist.
    /// </summary>
    /// <param name="tableName">The name of the applied patches table. The
    /// name is not escaped and implementations of the recorder should
    /// escape the name using the database-specific syntax.</param>
    Task EnsureAppliedPatchTableExistsAsync(
        string tableName,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Records when a patch was applied successfully.
    /// </summary>
    /// <param name="tableName">The name of the applied patches table. The
    /// name is not escaped and implementations of the recorder should
    /// escape the name using the database-specific syntax.</param>
    /// <param name="patchName">The name of the patch.</param>
    Task RecordAppliedPatchAsync(
        string tableName,
        string patchName,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Returns the set of names of patches that have been applied and
    /// should not be applied again.
    /// </summary>
    /// <param name="tableName">The name of the applied patches table. The
    /// name is not escaped and implementations of the recorder should
    /// escape the name using the database-specific syntax.</param>
    Task<ISet<string>> GetAppliedPatchNamesAsync(
        string tableName,
        CancellationToken cancellationToken
    );
}
