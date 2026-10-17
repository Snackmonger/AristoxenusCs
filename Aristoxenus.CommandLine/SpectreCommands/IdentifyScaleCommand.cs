using System.ComponentModel;

using MediatR;

using Spectre.Console;
using Spectre.Console.Cli;

using Aristoxenus.Application.UseCases.IdentifyScaleFromIntervalNames;
using Aristoxenus.Application.UseCases.IdentifyScaleFromNoteNames;

namespace Aristoxenus.CommandLine.SpectreCommands;

public class IdentifyScaleCommand(IMediator mediator) : AsyncCommand<IdentifyScaleCommand.IdentifyScaleCommandSettings>
{
    public class IdentifyScaleCommandSettings : CommandSettings
    {
        [CommandArgument(0, "<symbols>")]
        [Description("The symbols (note names or interval names) that make up the scale you want to identify.")]
        public string[] Symbols { get; set; } = [];
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, IdentifyScaleCommandSettings settings, CancellationToken cancellationToken)
    {
        int exitCode;
        AnsiConsole.MarkupLine($"Beginning operation with inputs: [blue]{string.Join(", ", settings.Symbols)}[/]");
        var noteNamesRequest = new IdentifyScaleFromNoteNamesRequest(settings.Symbols);
        var noteNamesResponse = await mediator.Send(noteNamesRequest, cancellationToken);
        if (noteNamesResponse.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[green]SUCCESS[/]: Scale was identified as [green]{noteNamesResponse.Value.ScaleName}[/] in mode [green]{noteNamesResponse.Value.Mode}[/]");
            exitCode = 0;
        }
        else
        { 
            var intervalNamesRequest = new IdentifyScaleFromIntervalNamesRequest(settings.Symbols);
            var intervalNamesResponse = await mediator.Send(intervalNamesRequest, cancellationToken);
            if (intervalNamesResponse.IsSuccess)
            {
                AnsiConsole.MarkupLine($"[green]SUCCESS[/]: Scale was identified as [green]{intervalNamesResponse.Value.ScaleName}[/] in mode [green]{intervalNamesResponse.Value.Mode}[/]");
                exitCode = 0;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]FAILURE[/]");
                foreach (var error in noteNamesResponse.Errors)
                    AnsiConsole.MarkupLine(error.ToString() ?? "Unknown error.");
                exitCode = 1;
            }
        }
        return exitCode;
    }
}
