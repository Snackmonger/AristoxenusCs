

using Aristoxenus.Domain.Core.Interfaces;
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Language;

public class DslDemo
{

    public static int Run(Scale scale, IMelody melody)
    {

        melody.WithDefaultDuration(Duration.Quarter);
        var config = new ChordScaleDerivation(ChordStructure.Tertial, ChordNumberOfNotes.Tetrad);
        
        var chordScale = scale.GetChordScale(config, startInOctave: 4);
        bool even = false;

        // Ascending arpeggios, derived from the passed scale, but every
        // other chord in the scale will voiced drop-2, first inversion.
        // The API is meant to make it easy to experiment with different
        // voicings and inversions.
        foreach (Chord chord in chordScale)
        {
            if (even)
                // Use a special voicing
                foreach (Pitch p in chord.WithVoicing(ChordVoicing.Drop2).WithInversion(1).AscendingArpeggio)
                    // Use an explicit duration
                    melody.Add(p, Duration.Eighth);
            else
                // Use the root position close voicing
                foreach (Pitch p in chord.AscendingArpeggio)
                    // Use default duration
                    melody.Add(p);
            even = !even;
        }

        // We don't have to use a scale to get chords. We can also just
        // specify chord names and get their pitches directly. 
        //
        // Most objects in the system have string representations and you
        // don't need to `new` a bunch of objects explicitly.
        foreach (ChordName name in new[] { "Cmaj7", "Emin7", "Bb7", "G7b9" })
        {
            // Reify a chord name by situating it in pitch space, then access
            // the `Chord` API directly. 
            foreach (Pitch p in name.Reify(4).WithInversion(2).AscendingArpeggio)
                melody.Add(p, Duration.Eighth);
        }
        // Naturally, we can add pitches and durations as strings too.
        melody.Add("C4", "1/8");

        // Adding a duration without a pitch means adding a rest.
        melody.Add(Duration.Eighth);

        return 0;

    }
}