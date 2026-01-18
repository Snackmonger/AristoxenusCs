
namespace Aristoxenus.Utilities;

using Aristoxenus.Constants;
public class FrequencyEquations
{
    public static int GetSemitonesBetwenFrequencies(
        double frequency1,
        double frequency2
    )
    {
        return (int)Math.Round(
            Numbers.SemitonesInOctave * Math.Log2(frequency1 / frequency2));
    }

    public static int GetSemitonesFromCentralFrequency(double frequency)
    {
        return GetSemitonesBetwenFrequencies(frequency, Numbers.CentralFrequency);
    }

    public static double GetFrequencyFromSemitones(
               int steps,
               double frequency
           )
    {
        return Math.Pow(
            Numbers.OctaveEquvalenceFactor,
            (steps / Numbers.SemitonesInOctave) * frequency
            );
    }

    public static double GetFrequencyFromSemitonesFromCentralFrequency(
        int steps
    )
    {
        return GetFrequencyFromSemitones(steps, Numbers.CentralFrequency);
    }


    public static double GetOctavesBetweenFrequencies(
        double frequency1,
        double frequency2
    )
    {
        if (frequency1 is < 0 || frequency2 is < 0)
        {
            throw new Exception("Frequencies must be positive values");
        }

        return Math.Log2(frequency1 / frequency2);
    }

    public static double GetCentsBetweenFrequencies(double frequency1, double frequency2)
    {
        return Math.Round(
            (Numbers.SemitonesInOctave * 1000) *
            Math.Log2(frequency1 / frequency2)
        );
    }

    public static int GetMidiNumberFromFrequency(double frequency)
    {
        return (int)Math.Round(
            Numbers.SemitonesInOctave
            * Math.Log2(frequency / Numbers.CentralFrequency)
            + Numbers.CentralMidiNumber
        );
    }

    public static double GetFrequencyFromMidiNumber(int midiNumber)
    {
        return Numbers.CentralFrequency * Math.Pow(
                Numbers.OctaveEquvalenceFactor,
                (double)(midiNumber - Numbers.CentralMidiNumber)
                / Numbers.SemitonesInOctave
        );
    }
}
