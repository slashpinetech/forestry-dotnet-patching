using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SlashPineTech.Forestry.Lifecycle;

namespace SlashPineTech.Forestry.Patching;

public static class PatchServiceCollectionExtensions
{
    /// <summary>
    /// Adds support for the patching system to the DI services.
    /// </summary>
    /// <param name="services">The DI services collection.</param>
    /// <param name="configure">A configuration lambda for setting patch options.</param>
    /// <returns>A patch builder used for registering patches to run.</returns>
    public static IPatchBuilder AddPatching(this IServiceCollection services, Action<PatchOptions> configure)
    {
        services.AddLifecycleActions();

        services.Configure(configure);
        services.AddTransient<IStartupAction, PatchRunner>();
        services.TryAddTransient<IPatchRecorder, SqlServerPatchRecorder>();
        services.TryAddTransient<IPatchAuditor, NoopPatchAuditor>();

        return new PatchBuilder(services);
    }
}
