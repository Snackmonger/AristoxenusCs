namespace Aristoxenus.Core.Models
{
    public class Chord(ChordName name, ChordVoicing voicing, int octave)
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


        public Pitch RootPitch => new(Name.Root, OctaveNumber);

        public List<Pitch> AscendingArpeggio(int octaves)
        {
            List<Pitch> pitches = [];
            Pitch rootPitch = RootPitch;
            for (int i = 0; i < octaves; i++)
            {
                // TODO
            }
            return pitches;
        }
    }
}
