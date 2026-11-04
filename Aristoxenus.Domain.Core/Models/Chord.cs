using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Models
{
    public class Chord(ChordName name, ChordVoicing voicing, int octave) : ValueObject
    {
        /// <summary>
        /// The name of the chord, which provides its basic root position 
        /// structure.
        /// </summary>
        public readonly ChordName Name = name;

        /// <summary>
        /// The voicing of the chord, which describes a transformation applied
        /// to the root position structure.
        /// </summary>
        public readonly ChordVoicing Voicing = voicing;

        /// <summary>
        /// The octave in which this voicing is situated, which refers to
        /// the octave of the root note of the chord, even if that note 
        /// is not the lowest note in the voicing.
        /// </summary>
        public readonly int OctaveNumber = octave;

        public NoteName RootName => Name.Root;


        //public List<Pitch> AscendingArpeggio()
        //{
        //    Pitch rootPitch = new(RootName, OctaveNumber);
        //    IEnumerable<Interval> intervals = Voicing.Apply(Name.Intervals);
        //    List<Pitch> pitches = [];
        //    foreach (Interval interval in intervals)
        //    {
        //        pitches.Add(rootPitch.GetPitchAbove(interval));
        //    }
        //    return pitches;
        //}

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Voicing;
            yield return OctaveNumber;
        }
    }
}
