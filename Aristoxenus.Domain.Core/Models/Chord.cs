using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Models
{
    public class Chord(ChordName name, ChordVoicing? voicing, int? inversion, int? octave) : ValueObject
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
        public readonly ChordVoicing Voicing = voicing ?? ChordVoicing.RootPosition;

        /// <summary>
        /// The inversion of the chord, which describes a rotation of the interval set.
        /// </summary>
        public readonly int Inversion = inversion ?? 0;

        /// <summary>
        /// The octave in which this voicing is situated, which refers to
        /// the octave of the root note of the chord, even if that note 
        /// is not the lowest note in the voicing.
        /// </summary>
        public readonly int OctaveNumber = octave ?? 4;

        public NoteName RootName => Name.Root;


        public List<Pitch> AscendingArpeggio()
        {
            Pitch rootPitch = new(RootName, OctaveNumber);
            List<Pitch> pitches = [..Name.Intervals.Select(rootPitch.GetPitchAbove)];
            int i = Inversion;
            while (i > 0)
            {
                Pitch lowestPitch = pitches[0];
                pitches.RemoveAt(0);
                pitches.Add(lowestPitch.GetPitchAbove(Interval.Octave));
                i--;
            }

            foreach (int displacedIndex in Voicing.DisplacedIntervalIndices)
            {
                if (displacedIndex <= 0 || displacedIndex >= pitches.Count())
                {
                    throw new ArgumentOutOfRangeException($"Displaced interval index {displacedIndex} is out of range for the provided pitches.");
                }
                Pitch displacedPitch = pitches[displacedIndex];
                pitches.RemoveAt(displacedIndex);
                pitches.Add(displacedPitch.GetPitchAbove(Interval.Octave));
            }

            return pitches;
        }


        public Chord InVoicing(ChordVoicing voicing)
        {
            return new Chord(Name, voicing, Inversion, OctaveNumber);
        }

        public Chord InInversion(int inversion)
        {
            return new Chord(Name, Voicing, inversion, OctaveNumber);
        }

        public Chord InOctave(int octave)
        {
            return new Chord(Name, Voicing, Inversion, octave);
        }   


        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Inversion;
            yield return OctaveNumber;
            yield return Voicing;
        }
    }
}
