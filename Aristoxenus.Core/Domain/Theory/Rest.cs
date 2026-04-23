using Aristoxenus.Core.Domain.Theory.Interfaces;
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Theory;

public class Rest : ValueObject, IHasDuration
{
    public NonNegative<double> Duration { get; init; }

    public Rest(NonNegative<double> duration)
    {
        Duration = duration;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Duration.Value;
    }

}


