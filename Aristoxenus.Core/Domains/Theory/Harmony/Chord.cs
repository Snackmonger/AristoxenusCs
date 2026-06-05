using Aristoxenus.Core.Domains.Theory.Atoms;

namespace Aristoxenus.Core.Domains.Theory.Harmony
{
    internal class Chord
    {
        /// <summary>
        /// The name of the chord, which provides its basic root position 
        /// structure.
        /// </summary>
        public readonly ChordName Name;

        /// <summary>
        /// The voicing of the chord, which defines a transformation applied
        /// to the root position structure.
        /// </summary>
        public readonly ChordVoicing Voicing;

        /// <summary>
        /// The octave in which this voicing is situated.
        /// </summary>
        public readonly OctaveNumber OctaveNumber;

        public Chord(ChordName name, ChordVoicing voicing, OctaveNumber octave)
        {
            Name = name;
            Voicing = voicing;
            OctaveNumber = octave;
        }


    }
}
