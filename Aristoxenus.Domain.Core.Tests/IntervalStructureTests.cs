using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Enums;
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Tests
{
    [TestClass]
    public class IntervalStructureTests
    {
        private readonly IntervalStructure _diatonic = IntervalStructure.CreateAsScale(RawScalePatterns.Diatonic);

        [TestMethod]
        [DataRow(new string[] { "1", "3", "5" }, ChordStructure.Tertial, ChordNumberOfNotes.Triad)]
        [DataRow(new string[] { "1", "3", "5", "7" }, ChordStructure.Tertial, ChordNumberOfNotes.Tetrad)]
        [DataRow(new string[] { "1", "3", "5", "7", "9" }, ChordStructure.Tertial, ChordNumberOfNotes.Pentad)]
        [DataRow(new string[] { "1", "3", "5", "7", "9", "11" }, ChordStructure.Tertial, ChordNumberOfNotes.Hexad)]
        [DataRow(new string[] { "1", "3", "5", "7", "9", "11", "13" }, ChordStructure.Tertial, ChordNumberOfNotes.Heptad)]

        [DataRow(new string[] { "1", "4", "7" }, ChordStructure.Quartal, ChordNumberOfNotes.Triad)]
        [DataRow(new string[] { "1", "4", "7", "10" }, ChordStructure.Quartal, ChordNumberOfNotes.Tetrad)]
        [DataRow(new string[] { "1", "4", "7", "10", "13" }, ChordStructure.Quartal, ChordNumberOfNotes.Pentad)]
        [DataRow(new string[] { "1", "4", "7", "10", "13", "16" }, ChordStructure.Quartal, ChordNumberOfNotes.Hexad)]
        [DataRow(new string[] { "1", "4", "7", "10", "13", "16", "19" }, ChordStructure.Quartal, ChordNumberOfNotes.Heptad)]

        [DataRow(new string[] { "1", "5", "9" }, ChordStructure.Quintal, ChordNumberOfNotes.Triad)]
        [DataRow(new string[] { "1", "5", "9", "13" }, ChordStructure.Quintal, ChordNumberOfNotes.Tetrad)]
        [DataRow(new string[] { "1", "5", "9", "13", "17" }, ChordStructure.Quintal, ChordNumberOfNotes.Pentad)]
        [DataRow(new string[] { "1", "5", "9", "13", "17", "21" }, ChordStructure.Quintal, ChordNumberOfNotes.Hexad)]
        [DataRow(new string[] { "1", "5", "9", "13", "17", "21", "25" }, ChordStructure.Quintal, ChordNumberOfNotes.Heptad)]
        public void Chordify_DiatonicScale_ReturnsCorrectIntervals(string[] expectedIntervals, ChordStructure structure, ChordNumberOfNotes numberOfNotes)
        {

            var chordIntervals = _diatonic.Chordify(new ChordScaleDerivation(structure, numberOfNotes));

            CollectionAssert.AreEqual(expectedIntervals, chordIntervals.IntervalSymbols);
        }
    }
}
