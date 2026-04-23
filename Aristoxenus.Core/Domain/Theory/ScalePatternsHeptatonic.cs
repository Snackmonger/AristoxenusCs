namespace Aristoxenus.Core.Domain.Theory;

using Aristoxenus.Core.Constants;

public sealed class ScalePatternsHeptatonic
{

    internal static readonly Dictionary<string, IEnumerable<int>> ScaleMap = new() {
        { Keywords.Diatonic, RawScalePatterns.Diatonic},
        { Keywords.Altered, RawScalePatterns.Altered },
        { Keywords.Hemitonic, RawScalePatterns.Hemitonic },
        { Keywords.Hemiolic, RawScalePatterns.Hemiolic },
        { Keywords.Diminished, RawScalePatterns.Diminished },
        { Keywords.Augmented, RawScalePatterns.Augmented },
        { Keywords.Harmonic, RawScalePatterns.Harmonic },
        { Keywords.Biseptimal, RawScalePatterns.Biseptimal },
        { Keywords.Paleochromatic, RawScalePatterns.Paleochromatic },
        { Keywords.Enigmatic, RawScalePatterns.Enigmatic },
        { Keywords.DoubleHarmonic, RawScalePatterns.DoubleHarmonic },
        { Keywords.Neapolitan, RawScalePatterns.Neapolitan },
        { Keywords.Hungarian, RawScalePatterns.Hungarian },
        { Keywords.Persian, RawScalePatterns.Persian },
        { Keywords.Romanian, RawScalePatterns.Romanian}
    };

    public static readonly ScalePattern Diatonic = new(RawScalePatterns.Diatonic);
    public static readonly ScalePattern Altered = new(RawScalePatterns.Altered);
    public static readonly ScalePattern Hemitonic = new(RawScalePatterns.Hemitonic);
    public static readonly ScalePattern Hemiolic = new(RawScalePatterns.Hemiolic);
    public static readonly ScalePattern Diminished = new(RawScalePatterns.Diminished);
    public static readonly ScalePattern Augmented = new(RawScalePatterns.Augmented);
    public static readonly ScalePattern Biseptimal = new(RawScalePatterns.Biseptimal);
    public static readonly ScalePattern Paleochromatic = new(RawScalePatterns.Paleochromatic);
    public static readonly ScalePattern Enigmatic = new(RawScalePatterns.Enigmatic);
    public static readonly ScalePattern Persian = new(RawScalePatterns.Persian);
    public static readonly ScalePattern Hungarian = new(RawScalePatterns.Hungarian);
    public static readonly ScalePattern DoubleHarmonic = new(RawScalePatterns.DoubleHarmonic);
    public static readonly ScalePattern Neapolitan = new(RawScalePatterns.Neapolitan);
    public static readonly ScalePattern Romanian = new(RawScalePatterns.Romanian);
}
