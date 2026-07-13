using FluentResults;

using MediatR;

using Aristoxenus.Core.Errors;
using Aristoxenus.Core.Models;

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
            if (intervals.CanIdentifyScale(out string? name, out int? mode))
                response = Result.Ok(new IdentifyScaleFromNoteNamesResponse(request.NoteNameSymbols, name!, (int)mode!));

            else
                response = Result.Fail($"Unable to identify scale.");
        }
        catch (InvalidAsSymbol ex)
        {
            response = Result.Fail(ex.Message);
        }
        catch (IntervalStructureHasDuplicates ex)
        {
            response = Result.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN][{DateTime.UtcNow}] An unknown exception occurred: {ex.Message}");
            response = Result.Fail("Request failed");
        }
        return response;
    }
}
