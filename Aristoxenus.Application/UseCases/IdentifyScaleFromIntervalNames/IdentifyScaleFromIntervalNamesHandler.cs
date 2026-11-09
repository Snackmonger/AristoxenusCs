using FluentResults;

using MediatR;
using Aristoxenus.Domain.Core.Errors;
using Aristoxenus.Domain.Core.Models;
using Aristoxenus.Domain.Core.Enums;

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
                    .CanIdentifyScale(out ScaleName? name, out int? mode)
            )
                response = Result.Ok(new IdentifyScaleFromIntervalNamesResponse(request.IntervalNameSymbols, (ScaleName)name!, (int)mode!));

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
