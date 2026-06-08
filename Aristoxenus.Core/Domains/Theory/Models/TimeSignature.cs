
namespace Aristoxenus.Core.Domains.Theory.Models;

public class TimeSignature 
{
    public readonly int Beats;
    public readonly Duration BeatType;

    public TimeSignature(int beats, Duration beatType)
    {
        Beats = beats;
        BeatType = beatType;
    }
}
