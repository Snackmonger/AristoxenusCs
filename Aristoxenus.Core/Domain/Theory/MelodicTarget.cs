
namespace Aristoxenus.Core.Domain.Theory;

/// <summary>
/// The target moment towards which a melodic segment develops.
/// </summary>
public class MelodicTarget
{
    public readonly Pitch Pitch;
    public readonly double Time;
    private MelodicTarget(Pitch pitch, double time)
    {
        Pitch = pitch;
        Time = time;
    }

    public static MelodicTarget CreateNew(Pitch pitch, double time)
    {
        if (!(time >= 0))
            throw new Exception("Must be non-negative value");
        return new(pitch, time);
    }
}
