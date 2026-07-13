using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Aristoxenus.CommandLine.SpectreBridge;

// For the premise of this bridge, see:
// https://github.com/spectreconsole/examples/tree/main/examples/Cli/Injection/Infrastructure


public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;

    public TypeRegistrar(IServiceCollection builder) 
        => _builder = builder;

    public void Register(Type service, Type implementation) 
        => _builder.AddSingleton(service, implementation);
    public void RegisterInstance(Type service, object implementation) 
        => _builder.AddSingleton(service, implementation);
    public void RegisterLazy(Type service, Func<object> factory) 
        => _builder.AddSingleton(service, _ => factory());

    public ITypeResolver Build() 
        => new TypeResolver(_builder.BuildServiceProvider());
}
