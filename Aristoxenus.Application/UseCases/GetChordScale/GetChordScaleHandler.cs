using FluentResults;

using MediatR;

using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Models;
using Aristoxenus.Application.Dtos;


namespace Aristoxenus.Application.UseCases.GetChordScale;

public class GetChordScaleHandler : IRequestHandler<GetChordScaleRequest, Result<GetChordScaleResponse>>
{
    public async Task<Result<GetChordScaleResponse>> Handle(GetChordScaleRequest request, CancellationToken cancellationToken)
    {
        Result<GetChordScaleResponse> result;
        if (RawScalePatterns.AllScales.TryGetValue(request.ScaleName, out int[]? scaleIntervals))
        {
            var intervals = IntervalStructure.CreateAsScale(scaleIntervals).Rotate(request.ModeNumber);
            var tonicName = NoteName.FromSymbol(request.TonicNoteName);
            var scaleNoteNames = intervals.Select(tonicName.NoteNameAbove).ToList();

            List<ChordName> chordNames = [];
            for (int i = 1; i < scaleNoteNames.Count + Numbers.IndexOffset; i++)
            {
                chordNames.Add(new(scaleNoteNames[i], intervals.Rotate(i).Chordify(new(request.Structure, request.NumberOfNotes))));
            }

            List<string> noteNames = [.. scaleNoteNames];
            List<ChordNameDto> chords = [.. chordNames.Select(it => new ChordNameDto(it, [.. it.GetNoteNames()], it.Intervals.IntervalSymbols))];
            result = new GetChordScaleResponse(noteNames, intervals.IntervalSymbols, chords);
            return result;

        }

        return Result.Fail("Unable to identify scale.");

    }
}
