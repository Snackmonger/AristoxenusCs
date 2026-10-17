using System.ComponentModel;

using MediatR;

using Spectre.Console.Cli;

using Aristoxenus.Application.UseCases.GetChordScale;
using Aristoxenus.Core.Data;

namespace Aristoxenus.CommandLine.SpectreCommands;

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
        public string ScaleName { get; set; } = Keywords.Diatonic;

        [CommandOption("-m|--mode_number <mode_number>")]
        [Description("The mode you want to profile (1 = parent scale)")]
        [DefaultValue(1)]
        public int ModeNumber { get; set; } = 1;

        [CommandOption("-c|--chord_tones <chord_tones>")]
        [Description("The number of chord tones in each chord of the chord-scale.")]
        [DefaultValue(4)]
        public int NumberOfChordTones { get; set; } = 4;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, ProfileScaleCommandSettings settings, CancellationToken cancellationToken)
    {
        var request = new GetChordScaleRequest(settings.NoteName, settings.ScaleName, settings.ModeNumber, settings.NumberOfChordTones);
        var response = await mediator.Send(request, cancellationToken);
        int exitCode;
        if (response.IsSuccess)
        {
            exitCode = 0;
        }
        else
        {
            exitCode = 1;
        }
        return exitCode;
    }
}
