namespace Aristoxenus.Domain.Core.Models;

public class TimeSignature(int beats, Duration beatType)
{
    public readonly int Beats = beats;
    public readonly Duration BeatType = beatType;
}
