using System;
using System.Threading.Tasks;

namespace SlashPineTech.Forestry.Patching;

public class NoopPatchAuditor : IPatchAuditor
{
    public Task RecordSuccessfulPatchAsync(string patchName) => Task.CompletedTask;

    public Task RecordFailedPatchAsync(string patchName, Exception exception) => Task.CompletedTask;
}
