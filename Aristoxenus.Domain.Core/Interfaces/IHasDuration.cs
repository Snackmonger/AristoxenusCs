using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Interfaces;

public interface IHasDuration 
{
    public Duration Duration { get; }

    public IEnumerable<IHasDuration> SubdivideEqully(int subdivisions);

    public IHasDuration Extend(int multiplier);
}
