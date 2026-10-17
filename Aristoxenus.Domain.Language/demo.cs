

using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Language;

public class Melody
{
    internal void Clear()
    {
        throw new NotImplementedException();
    }

    internal void Add(Pitch p)
    {
        throw new NotImplementedException();
    }
}

public class Thing
{
	public static int Run(Scale scale)
	{

		Melody melody = new();
		var config = new ChordConfiguration(ChordStructure.Tertial, ChordNumberOfNotes.Tetrad);
		foreach (Chord chord in scale.GetChordScale(config, startInOctave: 4))
			foreach (Pitch p in chord.AscendingArpeggio(octaves: 1))
				melody.Add(p);

		return 0;

	}
}