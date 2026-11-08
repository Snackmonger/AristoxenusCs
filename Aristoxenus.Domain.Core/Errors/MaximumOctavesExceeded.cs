using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;

namespace Aristoxenus.Domain.Core.Errors
{
    public class MaximumOctavesExceeded() : DomainException($"Cannot widen an interval beyond the maximum number of octaves (max={Numbers.MaxOctaveNumber})");
}
