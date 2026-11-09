using Aristoxenus.Domain.Core.Enums;

namespace Aristoxenus.Application.UseCases.IdentifyScaleFromIntervalNames
{
    public record IdentifyScaleFromIntervalNamesResponse(IEnumerable<string> RequestedIntervals, ScaleName ScaleName, int Mode);
}
