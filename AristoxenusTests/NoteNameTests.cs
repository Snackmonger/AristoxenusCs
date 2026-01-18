using Aristoxenus.Domain.Models;
namespace AristoxenusTests
{
    [TestClass]
    public sealed class NoteNameTests
    {
        private readonly NoteName NoteNameG = NoteName.FromSymbol("G");

        [TestMethod]
        [DataRow("b2", "Ab")]
        [DataRow("2", "A")]
        [DataRow("#2", "A#")]
        [DataRow("b3", "Bb")]
        [DataRow("3", "B")]
        [DataRow("b4", "Cb")]
        [DataRow("4", "C")]
        [DataRow("#4", "C#")]
        [DataRow("b5", "Db")]
        [DataRow("5", "D")]
        [DataRow("#5", "D#")]
        [DataRow("b6", "Eb")]
        [DataRow("6", "E")]
        [DataRow("#6", "E#")]
        [DataRow("bb7", "Fb")]
        [DataRow("b7", "F")]
        [DataRow("7", "F#")]
        public void ReturnsCorrectNoteNameAbove(string intervalName, string expectedNoteName)
        {
            Assert.AreEqual(expectedNoteName, NoteNameG.NoteNameAbove(intervalName).Symbol);
        }

        [TestMethod]
        [DataRow("b2", "Ab")]
        [DataRow("2", "A")]
        [DataRow("#2", "A#")]
        [DataRow("b3", "Bb")]
        [DataRow("3", "B")]
        [DataRow("b4", "Cb")]
        [DataRow("4", "C")]
        [DataRow("#4", "C#")]
        [DataRow("b5", "Db")]
        [DataRow("5", "D")]
        [DataRow("#5", "D#")]
        [DataRow("b6", "Eb")]
        [DataRow("6", "E")]
        [DataRow("#6", "E#")]
        [DataRow("bb7", "Fb")]
        [DataRow("b7", "F")]
        [DataRow("7", "F#")]
        public void ReturnsCorrectIntervalAbove(string expectedInterval, string noteName)
        {
            // Assuming this should test IntervalAbove method
            Assert.AreEqual(expectedInterval, NoteNameG.IntervalAbove(noteName).Name.Symbol);
        }
    }
}