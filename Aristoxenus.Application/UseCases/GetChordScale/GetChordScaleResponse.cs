using Aristoxenus.Application.Dtos;

namespace Aristoxenus.Application.UseCases.GetChordScale;

public record GetChordScaleResponse(
    List<string> ScaleNoteNames, 
    List<string> ScaleIntervalNames, 
    List<ChordNameDto> ChordScale
);