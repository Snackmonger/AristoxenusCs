using Spectre.Console.Cli;

namespace Aristoxenus.Presentation.CommandLine.SpectreBridge;

public sealed class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider) 
        => _provider = provider;

    public object? Resolve(Type? type) 
        => type != null ? _provider.GetService(type) : null;
}
