using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Aristoxenus.Presentation.CommandLine.SpectreBridge;

// For the premise of this bridge, see:
// https://github.com/spectreconsole/examples/tree/main/examples/Cli/Injection/Infrastructure


public sealed class TypeRegistrar(IServiceCollection builder) : ITypeRegistrar
{
    public void Register(Type service, Type implementation) 
        => builder.AddSingleton(service, implementation);
    public void RegisterInstance(Type service, object implementation) 
        => builder.AddSingleton(service, implementation);
    public void RegisterLazy(Type service, Func<object> factory) 
        => builder.AddSingleton(service, _ => factory());

    public ITypeResolver Build() 
        => new TypeResolver(builder.BuildServiceProvider());
}
