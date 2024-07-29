using System.Threading;
using System.Threading.Tasks;

namespace SlashPineTech.Forestry.Patching;

/// <summary>
/// Bootstrap patches are run each time the application starts up.
/// </summary>
public interface IBootstrapPatch
{
    Task ApplyPatchAsync(CancellationToken cancellationToken);
}
