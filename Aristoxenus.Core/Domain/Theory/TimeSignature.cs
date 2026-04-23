
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Theory
{
    public  class TimeSignature : ValueObject
    {
        public readonly int Beats;
        public readonly int BeatType;

        private TimeSignature(int beats, int beatType)
        {
            Beats = beats;
            BeatType = beatType;
        }

        public static TimeSignature CreateNew(int beats, int beatType)
            => new(beats, beatType);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Beats;
            yield return BeatType;
        }
    }


}
