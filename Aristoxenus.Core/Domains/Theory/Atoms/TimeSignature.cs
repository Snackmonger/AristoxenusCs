using Aristoxenus.Core.Shared.Bases;

namespace Aristoxenus.Core.Domains.Theory.Atoms;

public class TimeSignature : ValueObject
{
    public readonly int Beats;
    public readonly Duration BeatType;

    public TimeSignature(int beats, Duration beatType)
    {
        Beats = beats;
        BeatType = beatType;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Beats;
        yield return BeatType;
    }
}
