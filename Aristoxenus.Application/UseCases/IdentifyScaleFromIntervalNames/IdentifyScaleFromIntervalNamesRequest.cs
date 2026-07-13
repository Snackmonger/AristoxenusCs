using FluentResults;
using MediatR;

namespace Aristoxenus.Application.UseCases.IdentifyScaleFromIntervalNames
{
    public record IdentifyScaleFromIntervalNamesRequest(IEnumerable<string> IntervalNameSymbols) : IRequest<Result<IdentifyScaleFromIntervalNamesResponse>>;
}
