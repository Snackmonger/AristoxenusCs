using FluentResults;

using MediatR;

using Aristoxenus.Core.Errors;
using Aristoxenus.Core.Models;

namespace Aristoxenus.Application.UseCases.IdentifyScaleFromIntervalNames;

public class IdentifyScaleFromIntervalNamesHandler : IRequestHandler<IdentifyScaleFromIntervalNamesRequest, Result<IdentifyScaleFromIntervalNamesResponse>>
{
    public async Task<Result<IdentifyScaleFromIntervalNamesResponse>> Handle(IdentifyScaleFromIntervalNamesRequest request, CancellationToken cancellationToken)
    {
        Result<IdentifyScaleFromIntervalNamesResponse> response;
        try
        {
            if (IntervalStructure
                    .FromScaleDegreeSymbols(request.IntervalNameSymbols)
                    .CanIdentifyScale(out string? name, out int? mode)
            )
                response = Result.Ok(new IdentifyScaleFromIntervalNamesResponse(request.IntervalNameSymbols, name!, (int)mode!));

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
