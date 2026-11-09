using Aristoxenus.Domain.Core.Enums;

namespace Aristoxenus.Application.UseCases.IdentifyScaleFromNoteNames
{
    public record IdentifyScaleFromNoteNamesResponse(IEnumerable<string> RequestedNotes, ScaleName ScaleName, int Mode);
}
