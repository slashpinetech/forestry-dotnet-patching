using Microsoft.Extensions.DependencyInjection;

namespace SlashPineTech.Forestry.Patching;

public class PatchBuilder : IPatchBuilder
{
    private readonly IServiceCollection _services;

    public PatchBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public IPatchBuilder AddBootstrapPatch<TType>() where TType : class, IBootstrapPatch
    {
        _services.AddTransient<IBootstrapPatch, TType>();
        return this;
    }

    public IPatchBuilder AddPatch<TType>() where TType : class, IPatch
    {
        _services.AddTransient<IPatch, TType>();
        return this;
    }
}
