namespace Aristoxenus.Domain.Models
{
    internal class Chord
    {
        public readonly ChordName Name;
        public readonly ChordVoicing Voicing;

        private Chord(ChordName name, ChordVoicing voicing)
        {
            Name = name;
            Voicing = voicing;
        }

        public Chord CreateNew(ChordName name, ChordVoicing voicing)
        {
            return new(name, voicing);
        }
    }
}
