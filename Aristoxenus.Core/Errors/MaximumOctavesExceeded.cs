using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;

namespace Aristoxenus.Core.Errors
{
    public class MaximumOctavesExceeded() : DomainException($"Cannot widen an interval beyond the maximum number of octaves (max={Numbers.MaxOctaveNumber})");
}
