using Aristoxenus.Core.Data;
using Aristoxenus.Core.Errors;

namespace Aristoxenus.Core.Utilities;


public static class FrequencyHelper
{
    public static int GetSemitonesBetwenFrequencies(double frequency1, double frequency2)
        => (int)Math.Round(Numbers.SemitonesInOctave
            * Math.Log2(frequency2 / frequency1));

    public static int GetSemitonesFromCentralFrequency(double frequency)
        => GetSemitonesBetwenFrequencies(frequency, Numbers.CentralFrequency);


    public static double GetFrequencyFromSemitones(int steps, double frequency)
        => Math.Pow(Numbers.OctaveEquvalenceFactor, (steps / Numbers.SemitonesInOctave) * frequency);


    public static double GetFrequencyFromSemitonesFromCentralFrequency(int steps)
        => GetFrequencyFromSemitones(steps, Numbers.CentralFrequency);

    public static double GetOctavesBetweenFrequencies(double frequency1, double frequency2)
        => frequency1 is < 0 || frequency2 is < 0
            ? throw new InvalidAsFrequency("Frequencies must be positive values")
            : Math.Log2(frequency2 / frequency1);

    public static double GetCentsBetweenFrequencies(double frequency1, double frequency2)
        => Math.Round((Numbers.SemitonesInOctave * 1000) * Math.Log2(frequency2 / frequency1));

    public static int GetMidiNumberFromFrequency(double frequency)
        => (int)Math.Round(Numbers.SemitonesInOctave
            * Math.Log2(frequency / Numbers.CentralFrequency)
            + Numbers.CentralMidiNumber);

    public static double GetFrequencyFromMidiNumber(int midiNumber)
        => Numbers.CentralFrequency
            * Math.Pow(
                Numbers.OctaveEquvalenceFactor,
                (double)(midiNumber - Numbers.CentralMidiNumber)
            / Numbers.SemitonesInOctave);

}
