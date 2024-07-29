using System.Threading;
using System.Threading.Tasks;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// Patches are run during system start-up (after bootstrap patches).
/// Patches run only once (assuming they're applied successfully).
/// </summary>
public interface IPatch
{
    Task ApplyPatchAsync(CancellationToken cancellationToken);
}
