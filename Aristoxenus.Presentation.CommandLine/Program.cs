using Microsoft.Extensions.DependencyInjection;

using Spectre.Console.Cli;

using Aristoxenus.Application;

using Aristoxenus.Presentation.CommandLine.SpectreBridge;
using Aristoxenus.Presentation.CommandLine.SpectreCommands;

var services = new ServiceCollection();
services.AddLogging();
services.AddMediatR(configuration =>
    configuration
        .RegisterServicesFromAssembly(typeof(Api).Assembly));

var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);
app.Configure(configuration =>
{
    configuration
        .AddCommand<IdentifyScaleCommand>("scale.id")
        .WithDescription("Identify the scale and mode for the given collection of note names or interval names.");
    configuration
        .AddCommand<GetChordScaleCommand>("scale.chords")
        .WithDescription("Get the chord scale of the given scale form");
    configuration
        .AddCommand<OpenDebugMenuCommand>("debug")
        .WithDescription("Access the debug menu");
});

return await app.RunAsync(args);
