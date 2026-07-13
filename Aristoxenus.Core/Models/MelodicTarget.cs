namespace Aristoxenus.Core.Models;

/// <summary>
/// The target note towards which a melodic segment develops.
/// </summary>
public class MelodicTarget
{
    public Note Note;
    public double Time;
    public MelodicTarget(Note note, double time)
    {
        if (!(time >= 0))
            throw new Exception("Must be non-negative value");
        Note = note;
        Time = time;
    }

}
