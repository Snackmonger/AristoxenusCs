namespace Aristoxenus.Application.UseCases.IdentifyScaleFromNoteNames
{
    public record IdentifyScaleFromNoteNamesResponse(IEnumerable<string> RequestedNotes, string ScaleName, int Mode);
}
