using Aristoxenus.Core.Domains.Theory.Interfaces;

using Aristoxenus.Core.Shared.Bases;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

public class Rest : ValueObject, IHasDuration
{
    public Duration Duration { get; init; }

    public Rest(Duration duration)
    {
        Duration = duration;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Duration.Value;
    }

}


