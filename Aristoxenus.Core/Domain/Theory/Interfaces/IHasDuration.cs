using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Theory.Interfaces
{
    public interface IHasDuration
    {
        public NonNegative<double> Duration { get; }
    }
}
