
using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Models;

namespace Aristoxenus.Guitar.Models
{
    public class Fret : ValueObject
    {
        public Pitch String {  get; init; }
        public int FretNumber { get; init; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
    public class Fretboard
    {
        public List<Pitch> Strings { get; private set; }
        public int NumberOfFrets {get; private set;}

        public HashSet<Fret> FindNotes(IEnumerable<NoteName> noteNames)
        {
            throw new NotImplementedException();
        }

        public HashSet<Fret> FindNotes(IEnumerable<string> noteNames)
        {
            throw new NotImplementedException();
        }

        public HashSet<Fret> FindNotes(ChordName chordName)
        {
            throw new NotImplementedException();
        }

        public HashSet<Fret> FindNotes(Scale scale)
        {
            throw new NotImplementedException();
        }

    }
}
