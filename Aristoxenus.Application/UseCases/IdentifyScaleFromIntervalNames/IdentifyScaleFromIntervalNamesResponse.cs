namespace Aristoxenus.Application.UseCases.IdentifyScaleFromIntervalNames
{
    public record IdentifyScaleFromIntervalNamesResponse(IEnumerable<string> RequestedIntervals, string ScaleName, int Mode);
}
