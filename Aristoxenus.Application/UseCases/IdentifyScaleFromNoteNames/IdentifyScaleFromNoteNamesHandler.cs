using FluentResults;

using MediatR;

using Aristoxenus.Domain.Core.Errors;
using Aristoxenus.Domain.Core.Models;
using Aristoxenus.Domain.Core.Enums;

namespace Aristoxenus.Application.UseCases.IdentifyScaleFromNoteNames;


public class IdentifyScaleFromNoteNamesHandler : IRequestHandler<IdentifyScaleFromNoteNamesRequest, Result<IdentifyScaleFromNoteNamesResponse>>
{
    public async Task<Result<IdentifyScaleFromNoteNamesResponse>> Handle(IdentifyScaleFromNoteNamesRequest request, CancellationToken cancellationToken)
    {
        Result<IdentifyScaleFromNoteNamesResponse> response;
        try
        {
            List<NoteName> noteNames = [.. NoteName.FromSymbols(request.NoteNameSymbols)];
            var root = noteNames[0];
            IntervalStructure intervals = new([.. noteNames.Select(root.IntervalAbove)]);
            if (intervals.CanIdentifyScale(out ScaleName? name, out int? mode))
                response = Result.Ok(new IdentifyScaleFromNoteNamesResponse(request.NoteNameSymbols, (ScaleName)name!, (int)mode!));

            else
                response = Result.Fail($"Unable to identify scale.");
        }
        catch (Exception ex) when (ex is InvalidAsSymbol or IntervalStructureHasDuplicates)
        {
            response = Result.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            response = Result.Fail($"An unknown exception occurred: {ex.Message}");
        }
        return response;
    }
}
