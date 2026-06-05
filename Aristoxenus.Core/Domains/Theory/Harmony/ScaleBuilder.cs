using Aristoxenus.Core.Domains.Theory.Atoms;

namespace Aristoxenus.Core.Domains.Theory.Harmony
{
    public class ScaleBuilder
    {
        private List<NoteName> _notes;
        public ScaleBuilder AddNote(NoteName note)
        {
            if (_notes
                    .Select(n => n.AbsoluteValue)
                    .Contains(note.AbsoluteValue)) 
                return this;
            _notes.Add(note);
            return this;
        }

        public Scale Build()
        {
            throw new NotImplementedException();
        }
    }
}
