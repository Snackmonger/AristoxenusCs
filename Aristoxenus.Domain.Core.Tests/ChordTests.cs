

using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Tests
{
    [TestClass]
    public class ChordTests
    {

        [TestMethod]
        [DataRow(new[] { "C4", "E4", "G4" }, "Cmaj", 0)]
        [DataRow(new[] { "E4", "G4", "C5" }, "Cmaj", 1)]
        [DataRow(new[] { "G4", "C5", "E5" }, "Cmaj", 2)]
        [DataRow(new[] { "C4", "E4", "G4" }, "Cmaj", 3)]
        [DataRow(new[] { "E4", "G4", "C5" }, "Cmaj", 4)]
        [DataRow(new[] { "G4", "C5", "E5" }, "Cmaj", 5)]
        [DataRow(new[] { "B4", "D#5", "F#5" }, "Bmaj", 0)]
        [DataRow(new[] { "C4", "E4", "G4", "B4" }, "Cmaj7", 0)]
        [DataRow(new[] { "E4", "G4", "B4", "C5" }, "Cmaj7", 1)]
        [DataRow(new[] { "G4", "B4", "C5", "E5" }, "Cmaj7", 2)]
        [DataRow(new[] { "B4", "C5", "E5", "G5" }, "Cmaj7", 3)]
        [DataRow(new[] { "C4", "E4", "G4", "B4" }, "Cmaj7", 4)]
        [DataRow(new[] { "E4", "G4", "B4", "C5" }, "Cmaj7", 5)]
        [DataRow(new[] { "B4", "D#5", "F#5", "A#5" }, "Bmaj7", 0)]
        [DataRow(new[] { "C4", "E4", "G4", "B4", "D5" }, "Cmaj9", 0)]
        [DataRow(new[] { "E4", "G4", "B4", "D5", "C6" }, "Cmaj9", 1)]
        [DataRow(new[] { "G4", "B4", "D5", "C6", "E6" }, "Cmaj9", 2)]
        [DataRow(new[] { "B4", "D5", "C6", "E6", "G6" }, "Cmaj9", 3)]
        [DataRow(new[] { "D5", "C6", "E6", "G6", "B6" }, "Cmaj9", 4)]
        [DataRow(new[] { "C4", "E4", "G4", "B4", "D5", "F5" }, "Cmaj11", 0)]
        [DataRow(new[] { "C4", "E4", "G4", "B4", "D5", "F5", "A5" }, "Cmaj13", 0)]

        public void AscendingArpeggio_Tertial_CloseVoicing_Octave4_ReturnsCorrectPitches(string[] expectedPitches, string chordSymbol, int inversion)
        {
            
            var chord = ChordName.FromSymbol(chordSymbol).Reify(inversion, 4);
            
            var arpeggio = chord.AscendingArpeggio();
            List<Pitch> pitches = [..expectedPitches.Select(Pitch.FromSymbol)];
            CollectionAssert.AreEqual(pitches, arpeggio);
        }
    }
}
