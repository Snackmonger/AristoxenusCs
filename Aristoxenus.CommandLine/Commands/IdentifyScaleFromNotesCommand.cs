
using Aristoxenus.Core.Shared.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;


namespace Aristoxenus.CommandLine.Commands;

public class IdentifyScaleFromNotesCommand : Command<IdentifyScaleFromNotesSettings>
{

    protected override int Execute(CommandContext context, IdentifyScaleFromNotesSettings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine($"Identifying scale from notes: [green]{string.Join(", ", settings.Notes)}[/]");
        List<(int, int)> noteNames = [..settings.Notes.Select(SymbolHelper.ParseNoteNameSymbol)];
        AnsiConsole.MarkupLine($"Parsed note names: [green]{string.Join(", ", noteNames)}[/]");
        (int, int) root = noteNames[0];
        AnsiConsole.MarkupLine($"Root note: [green]{root}[/]");
        List<(int, int)> intervals = [..noteNames.Select(it => NomenclatureHelper.GetIntervalNameBetweenNoteNames(root, it))];
        AnsiConsole.MarkupLine($"Calculated intervals: [green]{string.Join(", ", intervals)}[/]");
        if (PatternHelper.CanIdentifyScale(intervals
            .Select(NomenclatureHelper.GetIntervalValueFromName), 
            out string? name, 
            out int? mode)
        )
        {
            AnsiConsole.MarkupLine($"The scale is [green]{name}[/] in mode [green]{mode}[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]The scale could not be identified.[/]");
            return 1;
        }

    }

}
