using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlashPineTech.Forestry.Lifecycle;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// Applies bootstrap and standard patches during system start-up.
/// </summary>
/// <seealso cref="IBootstrapPatch"/>
/// <seealso cref="IPatch"/>
public class PatchRunner : IStartupAction
{
    private readonly IEnumerable<IBootstrapPatch> _bootstrapPatches;
    private readonly IEnumerable<IPatch> _patches;
    private readonly ILogger<PatchRunner> _logger;
    private readonly IPatchAuditor _auditor;
    private readonly IPatchRecorder _recorder;
    private readonly string _tableName;

    public PatchRunner(
        IEnumerable<IBootstrapPatch> bootstrapPatches,
        IEnumerable<IPatch> patches,
        ILogger<PatchRunner> logger,
        IPatchAuditor auditor,
        IPatchRecorder recorder,
        IOptions<PatchOptions> options
    )
    {
        _bootstrapPatches = bootstrapPatches;
        _patches = patches;
        _logger = logger;
        _auditor = auditor;
        _recorder = recorder;
        _tableName = options.Value.TableName;
    }

    public async Task OnStartupAsync(CancellationToken cancellationToken)
    {
        if (_bootstrapPatches.Any())
        {
            _logger.LogDebug("Executing bootstrap patches.");
            foreach (var bootstrapPatch in _bootstrapPatches)
            {
                try
                {
                    var patchName = bootstrapPatch.GetType().Name;

                    _logger.LogInformation("Executing bootstrap patch {patchName}.", patchName);
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    await bootstrapPatch.ApplyPatchAsync(cancellationToken);
                    stopwatch.Stop();
                    _logger.LogInformation("Finished executing bootstrap patch {patchName} in {elapsedTime}ms.", patchName, stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Encountered an exception applying patches.");
                    throw;
                }
            }
        }
        else
        {
            _logger.LogDebug("There are no bootstrap patches registered. Skipping.");
        }

        await _recorder.EnsureAppliedPatchTableExistsAsync(_tableName, cancellationToken);

        if (_patches.Any())
        {
            var appliedPatches = await _recorder.GetAppliedPatchNamesAsync(_tableName, cancellationToken);

            _logger.LogDebug("Executing patches.");
            foreach (var patch in _patches)
            {
                var patchName = patch.GetType().Name;

                if (appliedPatches.Contains(patchName))
                {
                    _logger.LogInformation("Patch '{patchName}' is already applied. Skipping.", patchName);
                    continue;
                }

                try
                {
                    _logger.LogInformation("Executing patch {patchName}.", patchName);
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    await patch.ApplyPatchAsync(cancellationToken);
                    stopwatch.Stop();
                    _logger.LogInformation("Finished executing patch {patchName} in {elapsedTime}ms.", patchName, stopwatch.ElapsedMilliseconds);

                    await _recorder.RecordAppliedPatchAsync(_tableName, patchName, cancellationToken);
                    await _auditor.RecordSuccessfulPatchAsync(patchName);
                }
                catch (Exception ex)
                {
                    await _auditor.RecordFailedPatchAsync(patchName, ex);
                    _logger.LogCritical(ex, "Encountered an exception applying patches.");

                    // patches build on each other, so do not continue if
                    // one failed.
                    throw;
                }
            }
        }
        else
        {
            _logger.LogDebug("There are no patches registered. Skipping.");
        }
    }
}
