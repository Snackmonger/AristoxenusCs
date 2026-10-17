

namespace Demo;

public class Thing
{
	public static int Run(Scale scale)
	{

		Melody m;
		foreach ((NoteName n, ChordName c) in scale.GetChordScale(structure: ChordStructure.Triad, notes: 4))
		{
			m.Clear();
			m.AddNotes(c.NoteNames.Ascending.Select(it => );
			foreach (Pitch chordNote in c.Arpeggio(octave: 4))
			{
				m.AddNote(chordNote.GetChromaticStep(-2), Duration.Eighth);
				m.AddNote(chordNote.GetChromaticStep(-1), Duration.Eighth);
				m.AddNote(chordNote);
			}
		}

	}
}