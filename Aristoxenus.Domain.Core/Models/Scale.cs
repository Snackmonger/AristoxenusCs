using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Utilities;

namespace Aristoxenus.Domain.Core.Models;

/// <summary>
/// Representation of a scaleform, with an underlying interval pattern 
/// and a degree of modal rotation, centred on a particular tonic note.
/// </summary>
public class Scale : ValueObject
{
    public readonly NoteName Tonic;
    public readonly List<int> ScalePattern;
    public readonly int ModeNumber;
    public readonly IntervalStructure Intervals;
    public readonly List<NoteName> Notes;

    public Scale(NoteName tonic, List<int> scalePattern, int modeNumber = 1)
    {
        Tonic = tonic;
        ScalePattern = scalePattern;
        ModeNumber =
            modeNumber > 0
            && modeNumber <= ScalePattern.Count
                ? modeNumber
                : 1;
        Intervals = IntervalStructure.CreateAsScale(PatternHelper.GetModeOfIntervalPattern(ScalePattern, ModeNumber));
        Notes = [..Intervals.Select(Tonic.NoteNameAbove)];
        
    }

    public IEnumerable<Chord> GetChordScale(ChordConfiguration config, int startInOctave)
    {
        throw new NotImplementedException();
    }

    public List<Pitch> AscendingPitches(int startInOctave)
    {
        throw new NotImplementedException();
    }

    public List<ChordName> GetChordScaleNames(ChordConfiguration config)
    {
        throw new NotImplementedException();
    }

    public List<ChordName> GetChordScaleNames(ChordStructure structure, ChordNumberOfNotes numberOfNotes)
        => GetChordScaleNames(new(structure, numberOfNotes));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Tonic;
        yield return ModeNumber;
        foreach (var i in ScalePattern) yield return i;
    }
}


