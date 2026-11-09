using Aristoxenus.Application.Dtos;
using Aristoxenus.Application.UseCases.GetChordScale;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Enums;
using MediatR;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Aristoxenus.Presentation.CommandLine.SpectreCommands;

public class GetChordScaleCommand(IMediator mediator) : AsyncCommand<GetChordScaleCommand.ProfileScaleCommandSettings>
{
    public class ProfileScaleCommandSettings : CommandSettings
    {
        [CommandOption("-n|--note_name <note_name>")]
        [Description($"The note name to use as the tonic of the scale.")]
        [DefaultValue("C")]
        public string NoteName { get; set; } = "C";

        [CommandOption("-s|--scale_name <scale_name>")]
        [Description($"The canonical name of the parent scale.")]
        [DefaultValue("diatonic")]
        public ScaleName ScaleName { get; set; } = ScaleName.Diatonic;

        [CommandOption("-m|--mode_number <mode_number>")]
        [Description("The mode you want to profile (1 = parent scale)")]
        [DefaultValue(1)]
        public int ModeNumber { get; set; } = 1;

        [CommandOption("-c|--chord_tones <chord_tones>")]
        [Description("The number of chord tones in each chord of the chord-scale (3-7).")]
        [DefaultValue(ChordNumberOfNotes.Triad)]
        public ChordNumberOfNotes NumberOfChordTones { get; set; } = ChordNumberOfNotes.Triad;

        [CommandOption("-t|--structure <chord_structure>")]
        [Description("The structure that derives the chord (tertial=3, quartal=4, quintal=5).")]
        [DefaultValue(ChordStructure.Tertial)]
        public ChordStructure Structure { get; set; } = ChordStructure.Tertial;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, ProfileScaleCommandSettings settings, CancellationToken cancellationToken)
    {
        var request = new GetChordScaleRequest(settings.NoteName, settings.ScaleName, settings.ModeNumber, settings.NumberOfChordTones, settings.Structure);
        var response = await mediator.Send(request, cancellationToken);
        int exitCode;
        if (response.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[green]Scale Note Names[/]: {string.Join(' ', response.Value.ScaleNoteNames)}");
            AnsiConsole.MarkupLine($"[green]Scale Intervals[/]: {string.Join(' ', response.Value.ScaleIntervalNames)}");
            foreach (ChordNameDto chord in response.Value.ChordScale)
            {
                AnsiConsole.MarkupLine($"[blue]{chord.ChordSymbol}[/]: {string.Join(' ', chord.NoteNames)}, {string.Join(' ', chord.IntervalNames)}");
            }
            exitCode = 0;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Failed:[/] {string.Join(' ', response.Errors.Select(it => it.Message))}");
            exitCode = 1;
        }
        return exitCode;
    }
}
