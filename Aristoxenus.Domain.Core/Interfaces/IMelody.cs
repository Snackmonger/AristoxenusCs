using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Interfaces
{
    public interface IMelody : ICollection<IHasDuration>
    {
        double CurrentTime { get; }

        void Add(Duration rest);
        void Add(IEnumerable<Pitch> pitches);
        void Add(IEnumerable<Pitch> pitches, Duration duration);
        void Add(Pitch pitch);
        void Add(Pitch pitch, Duration duration);
        IHasDuration? CheckBeginsAtTime(double time);
        IHasDuration? CheckIntersectTime(double time);
        bool TryExtendAtTime(double time, int multiplier);
        bool TrySubdivideAtTime(double time, int subdivisions);
        IMelody With(Duration rest);
        IMelody With(IEnumerable<Pitch> pitches);
        IMelody With(IEnumerable<Pitch> pitches, Duration duration);
        IMelody With(IHasDuration note);
        IMelody With(Pitch pitch);
        IMelody WithDefaultDuration(Duration duration);
    }
}