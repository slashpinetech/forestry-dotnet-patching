using System;
using System.Threading.Tasks;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// Provides a hook for recording audit log entries of patch application.
///
/// Note that the <see cref="IPatchRecorder"/> will handle keeping track of
/// patches that were applied successfully. This interface is only for
/// applications that want an immutable log of patch attempts.
/// </summary>
public interface IPatchAuditor
{
    /// <summary>
    /// Invoked after a patch is successfully applied.
    /// </summary>
    /// <param name="patchName">The name of the patch that was applied.</param>
    Task RecordSuccessfulPatchAsync(string patchName);

    /// <summary>
    /// Invoked after a patch failed.
    /// </summary>
    /// <param name="patchName">The name of the patch that failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    Task RecordFailedPatchAsync(string patchName, Exception exception);
}
