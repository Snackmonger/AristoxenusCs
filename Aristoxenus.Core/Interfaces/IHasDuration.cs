using Aristoxenus.Core.Models;

namespace Aristoxenus.Core.Interfaces;

public interface IHasDuration 
{
    public Duration Duration { get; }

    public IEnumerable<IHasDuration> SubdivideEqully(int subdivisions);

    public IHasDuration Extend(int multiplier);
}
