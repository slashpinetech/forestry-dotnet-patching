namespace SlashPineTech.Forestry.Patching;

public interface IPatchBuilder
{
    /// <summary>
    /// Adds a bootstrap patch to the DI services. Patches will run in the
    /// order they are added.
    /// </summary>
    /// <typeparam name="TType">The type of patch to add.</typeparam>
    /// <returns>This builder.</returns>
    IPatchBuilder AddBootstrapPatch<TType>() where TType : class, IBootstrapPatch;

    /// <summary>
    /// Adds a standard patch to the DI services. Patches will run in the
    /// order they are added.
    /// </summary>
    /// <typeparam name="TType">The type of patch to add.</typeparam>
    /// <returns>This builder.</returns>
    IPatchBuilder AddPatch<TType>() where TType : class, IPatch;
}
