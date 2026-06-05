
using Aristoxenus.Core.Domains.Theory.Atoms;
using Aristoxenus.Core.Domains.Theory.Melody;

public static class Program
{
    public static void Main()
    {
        var noteName = NoteName.FromSymbol("A##");
        var interval = Interval.FromSymbol("#5");
        var result = noteName.NoteNameAbove(interval);
    }
    public static void Something()
    {
        var melody = new MelodicLine<int>(0, new TimeSignature(4, Duration.Quarter),
            [
                // mes 1: Row, row, row your boat, gently down the stream
                new Note("G4", Duration.Quarter.Dotted),
                new Note("G4", Duration.Quarter.Dotted),
                // mes 2
                new Note("G4", Duration.Quarter),
                new Note("A4", Duration.Eighth),
                new Note("B4", Duration.Quarter.Dotted),
                // mes 3
                new Note("B4", Duration.Quarter),
                new Note("A4", Duration.Eighth),
                new Note("B4", Duration.Quarter),
                new Note("C5", Duration.Eighth),
                // mes 4
                new Note("D5", Duration.Half.Dotted),

                // mes 5: Merrily, merrily, merrily, merrily
                new Note("G5", Duration.Quarter.Triplet),
                new Note("G5", Duration.Quarter.Triplet),
                new Note("G5", Duration.Quarter.Triplet),
                new Note("D5", Duration.Quarter.Triplet),
                new Note("D5", Duration.Quarter.Triplet),
                new Note("D5", Duration.Quarter.Triplet),
                // mes 6
                new Note("B4", Duration.Quarter.Triplet),
                new Note("B4", Duration.Quarter.Triplet),
                new Note("B4", Duration.Quarter.Triplet),
                new Note("G4", Duration.Quarter.Triplet),
                new Note("G4", Duration.Quarter.Triplet),
                new Note("G4", Duration.Quarter.Triplet),

                // mes 7: Life is but a dream
                new Note("D5", Duration.Quarter),
                new Note("C5", Duration.Eighth),
                new Note("B4", Duration.Quarter),
                new Note("A4", Duration.Eighth),
                // mes 8
                new Note("G4", Duration.Half.Dotted)
            ]
        );

        // CurrentTime (absolute time) is always measured in 4/4
        Console.WriteLine(melody.CurrentTime * 4); // 7 (8 measures, 0-indexed)

        // Then we use the time signature to parse the "correct" 
        // groupings of the notes.
        Console.WriteLine(melody.CurrentMeasure);

    }
}