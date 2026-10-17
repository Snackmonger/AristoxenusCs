using Spectre.Console;
using Spectre.Console.Cli;

namespace Aristoxenus.Presentation.CommandLine.SpectreCommands;

/// <summary>
/// A special menu for debugging. 
/// </summary>
public class OpenDebugMenuCommand : AsyncCommand<OpenDebugMenuCommand.OpenDebugMenuCommandSettings>
{

    public class OpenDebugMenuCommandSettings : CommandSettings
    {

    }

    protected override async Task<int> ExecuteAsync(CommandContext context, OpenDebugMenuCommandSettings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine("Welcome to the debug menu.");


        return 0;
    }

}
