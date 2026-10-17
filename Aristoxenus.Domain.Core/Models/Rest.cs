using Aristoxenus.Domain.Core.Interfaces;
using Fractions;

namespace Aristoxenus.Domain.Core.Models;

public class Rest(Duration duration) : IHasDuration
{
    public Duration Duration { get; init; } = duration;

    public IHasDuration Extend(int multiplier)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IHasDuration> SubdivideEqully(int subdivisions)
    {
        Fraction subduration = Duration.Value / subdivisions;
        for (int i = 0; i < subdivisions; i++)
        {
            yield return new Rest(new Duration(subduration));
        }
    }
}


