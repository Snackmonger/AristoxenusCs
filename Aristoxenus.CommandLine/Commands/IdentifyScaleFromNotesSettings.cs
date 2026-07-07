using Spectre.Console.Cli;
using System.ComponentModel;

namespace Aristoxenus.CommandLine.Commands;

public class IdentifyScaleFromNotesSettings : CommandSettings
{
    [CommandArgument(0, "<notes>")]
    [Description("The notes to identify the scale from.")]
    public string[] Notes { get; set; } = [];
}
