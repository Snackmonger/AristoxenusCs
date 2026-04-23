using Aristoxenus.Core.Constants;
using Aristoxenus.Core.Domain.Theory;
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Tests
{
    [TestClass]
    public sealed class IntervalStructureTests
    {
        public Interval[] IllegalIntervalSet => [
            Interval.CreateNew(2, 1),
            Interval.CreateNew(3, -1),
            Interval.CreateNew(4, 0)
        ];

        public string[] DiatonicSymbols => [
            Symbols.One,
            Symbols.Two,
            Symbols.Three,
            Symbols.Four,
            Symbols.Five,
            Symbols.Six,
            Symbols.Seven];

        public Interval[] DiatonicScale => [
            Interval.CreateNew(1, 0),
            Interval.CreateNew(2, 0),
            Interval.CreateNew(3, 0),
            Interval.CreateNew(4, 0),
            Interval.CreateNew(5, 0),
            Interval.CreateNew(6, 0),
            Interval.CreateNew(7, 0)];

        [TestMethod]
        public void CreateNew_ValidIntervalArrayOrStringArray_ObjectsCreatedAreEquivalent()
        {
            var structure_1 = IntervalStructure.CreateNew(DiatonicSymbols);
            var structure_2 = IntervalStructure.CreateNew(DiatonicScale);
            Assert.AreEqual(structure_1, structure_2);
        }

        [TestMethod]
        public void CreateNew_IllegalParams_ThrowsOnIllegalValue()
        {
            Assert.Throws<DomainException>(
                () => IntervalStructure.CreateNew(IllegalIntervalSet)
            );

            Assert.Throws<DomainException>(
                () => IntervalStructure.CreateNew(IllegalIntervalSet.Select(i => i.Name.ToSymbol()))
            );
        }

        [TestMethod]
        [DataRow("Cmaj", new[] { "1", "3", "5" })]
        [DataRow("Cmaj7", new[] { "1", "3", "5", "7" })]
        [DataRow("CM7", new[] { "1", "3", "5", "7" })]
        [DataRow("Cm7", new[] { "1", "b3", "5", "b7" })]
        [DataRow("Cmin7", new[] { "1", "b3", "5", "b7" })]
        [DataRow("CmΔ7", new[] { "1", "b3", "5", "7" })]
        [DataRow("CmM7", new[] { "1", "b3", "5", "7" })]
        [DataRow("Cminmaj7", new[] { "1", "b3", "5", "7" })]
        [DataRow("CminM7", new[] { "1", "b3", "5", "7" })]
        [DataRow("C7aug", new[] { "1", "3", "#5", "b7" })]
        [DataRow("Caug7", new[] { "1", "3", "#5", "b7" })]
        [DataRow("C+7", new[] { "1", "3", "#5", "b7" })]
        [DataRow("C7+", new[] { "1", "3", "#5", "b7" })]
        [DataRow("Cmin11b5", new[] { "1", "b3", "b5", "b7", "9", "11" })]
        [DataRow("Co7", new[] { "1", "b3", "b5", "bb7" })]
        [DataRow("Cmin7b5add11", new[] { "1", "b3", "b5", "b7", "11" })]
        [DataRow("Cmaj7#5", new[] { "1", "3", "#5", "7" })]
        [DataRow("Cmaj7sus2", new[] { "1", "2", "5", "7" })]
        [DataRow("Csus4", new[] { "1", "4", "5" })]
        [DataRow("Csus2", new[] { "1", "2", "5" })]
        [DataRow("C7sus4", new[] { "1", "4", "5", "b7" })]
        [DataRow("Csusbb3bb7", new[] { "1", "bb3", "5", "bb7" })]
        [DataRow("Cmin11", new[] { "1", "b3", "5", "b7", "9", "11" })]
        [DataRow("Cdim11nobb7", new[] { "1", "b3", "b5", "9", "11" })]
        [DataRow("Cminno5bb7", new[] { "1", "b3", "bb7" })]
        [DataRow("C9no3", new[] { "1", "5", "b7", "9" })]
        public void FromChordSymbol_ValidString_ReturnsCorrectImplicitStructure(string chordSymbol, string[] expectedIntervals)
        {
            CollectionAssert.AreEqual(expectedIntervals, IntervalStructure.FromChordSymbol(chordSymbol).IntervalSymbols);
        }

        [TestMethod]
        [DataRow(new[] { "1", "3", "5" }, "maj")]
        [DataRow(new[] { "1", "3", "5", "7" }, "maj7")]
        [DataRow(new[] { "1", "3", "5", "7", "9" }, "maj9")]
        [DataRow(new[] { "1", "3", "5", "9" }, "majadd9")]
        [DataRow(new[] { "1", "3", "5", "7", "9", "11" }, "maj11")]
        [DataRow(new[] { "1", "3", "5", "7", "9", "11", "13" }, "maj13")]
        [DataRow(new[] { "1", "3", "5", "7", "11" }, "maj7add11")]
        [DataRow(new[] { "1", "3", "5", "7", "9", "13" }, "maj9add13")]
        [DataRow(new[] { "1", "3", "5", "b7" }, "7")]
        [DataRow(new[] { "1", "3", "5", "b7", "9" }, "9")]
        [DataRow(new[] { "1", "3", "5", "b7", "9", "13" }, "9add13")]
        [DataRow(new[] { "1", "b3", "5" }, "min")]
        [DataRow(new[] { "1", "b3", "5", "b7", "9" }, "min9")]
        [DataRow(new[] { "1", "b3", "5", "9" }, "minadd9")]
        [DataRow(new[] { "1", "b3", "5", "7" }, "minmaj7")]
        [DataRow(new[] { "1", "b3", "5", "7", "9" }, "minmaj9")]
        [DataRow(new[] { "1", "b3", "5", "7", "11" }, "minmaj7add11")]
        [DataRow(new[] { "1", "b3", "5", "7", "9", "11", "13" }, "minmaj13")]
        [DataRow(new[] { "1", "b3", "b5", "b7" }, "min7b5")]
        [DataRow(new[] { "1", "b3", "b5", "b7", "9" }, "min9b5")]
        [DataRow(new[] { "1", "b3", "b5", "b7", "9", "11", "13" }, "min13b5")]
        [DataRow(new[] { "1", "b3", "b5", "bb7" }, "dim7")]
        [DataRow(new[] { "1", "b3", "b5", "bb7", "9" }, "dim9")]
        [DataRow(new[] { "1", "b3", "b5", "bb7", "9", "11" }, "dim11")]
        [DataRow(new[] { "1", "3", "7", "9" }, "maj9no5")]
        [DataRow(new[] { "1", "2", "5" }, "sus2")]
        [DataRow(new[] { "1", "bb3", "5" }, "susbb3")]
        [DataRow(new[] { "1", "#3", "5" }, "sus#3")]
        [DataRow(new[] { "1", "4", "5" }, "sus4")]
        [DataRow(new[] { "1", "2", "5", "7" }, "maj7sus2")]
        [DataRow(new[] { "1", "2", "5", "b7" }, "7sus2")]
        [DataRow(new[] { "1", "2", "5", "bb7" }, "sus2bb7")]
        [DataRow(new[] { "1", "3", "5", "bb7" }, "majbb7")]
        [DataRow(new[] { "1", "bb3", "#5", "7" }, "maj7susbb3#5")]
        [DataRow(new[] { "1", "bb3", "b5", "bb7", "9" }, "susbb3b5bb7add9")]
        [DataRow(new[] { "1", "5", "bb7", "9" }, "no3bb7add9")]
        [DataRow(new[] { "1", "3", "##5", "7" }, "maj7##5")]
        [DataRow(new[] { "1", "3", "bb5", "7" }, "maj7bb5")]
        [DataRow(new[] { "1", "3", "#5", "bb7" }, "maj#5bb7")]
        [DataRow(new[] { "1", "4", "5", "b7" }, "7sus4")]
        public void ToChordSymbol_DefaultStyling_ReturnsCorrectSymbol(string[] intervals, string expectedChordSymbol)
        {
            Assert.AreEqual(expectedChordSymbol, IntervalStructure.CreateNew(intervals).ToChordSymbol());
        }

        [TestMethod]
        [DataRow(new[] { "1", "3", "5", "b7" }, true)]
        [DataRow(new[] { "1", "3", "5", "b7", "9", "13" }, true)]
        [DataRow(new[] { "1", "3", "b5", "b7" }, true)]
        [DataRow(new[] { "1", "3", "#5", "b7", "#11" }, true)]
        [DataRow(new[] { "1", "3", "5", "b7", "b9" }, true)]
        public void IsDominant_DominantStructure_ReturnsTrue(string[] intervals, bool expectedIsDominant)
        {
            Assert.IsTrue(IntervalStructure.CreateNew(intervals).IsDominant);
        }

        [TestMethod]
        [DataRow(["1", "b3", "5", "b7"])]
        [DataRow(["1", "3", "5", "7", "9", "13"])]
        [DataRow(["1", "3", "b5", "bb7"])]
        [DataRow(["1", "#3", "#5", "b7", "#11"])]
        [DataRow(["1", "3", "5", "7", "b9"])]
        public void IsDominant_NonDominantStructure_ReturnsFalse(string[] intervals)
        {
            Assert.IsFalse(IntervalStructure.CreateNew(intervals).IsDominant);
        }

        [TestMethod]
        [DataRow(["1", "b3", "b5", "bb7"])]
        [DataRow(["1", "b3", "b5", "bb7", "9", "13"])]
        [DataRow(["1", "b3", "b5", "bb7", "#11"])]
        [DataRow(["1", "b3", "b5", "bb7", "b9"])]
        public void IsDiminished_DiminishedStructure_ReturnsTrue(string[] intervals)
        {
            Assert.IsTrue(IntervalStructure.CreateNew(intervals).IsDiminished);
        }

        [TestMethod]
        [DataRow(["1", "3", "b5", "bb7"])]
        [DataRow(["1", "b3", "5", "bb7", "9", "13"])]
        [DataRow(["1", "b3", "b5", "b7"])]
        [DataRow(["1", "3", "5", "bb7", "#11"])]
        [DataRow(["1", "b3", "5", "bb7", "b9"])]
        public void IsDiminished_NonDiminishedStructure_ReturnsFalse(string[] intervals)
        {
            Assert.IsFalse(IntervalStructure.CreateNew(intervals).IsDiminished);
        }

        [TestMethod]
        public void IsSupersetOf_ValidSuperset_ReturnsTrue()
        {
            Assert.IsTrue(IntervalStructure.CreateNew(["1", "3", "5", "b7", "9", "13"]).IsSupersetOf(["1", "3", "5"]));
        }

        [TestMethod]
        public void IsSubsetOf_ValidSubset_ReturnsTrue()
        {
            Assert.IsTrue(IntervalStructure.CreateNew(["1", "3", "5"]).IsSubsetOf(["1", "3", "5", "b7", "9", "13"]));
        }

        [TestMethod]
        [DataRow(["1", "3", "5"])]
        [DataRow(["1", "b3", "5"])]
        public void HasPerfectFifth_PerfectFifthStructure_ReturnsTrue(string[] intervals)
        {
            Assert.IsTrue(IntervalStructure.CreateNew(intervals).HasPerfectFifth);
        }

        [TestMethod]
        [DataRow(["1", "3", "b5"])]
        [DataRow(["1", "3", "#5"])]
        public void HasPerfectFifth_NonPerfectFifthStructure_ReturnsFalse(string[] intervals)
        {
            Assert.IsFalse(IntervalStructure.CreateNew(intervals).HasPerfectFifth);
        }

        [TestMethod]
        [DataRow(["1", "3", "b5"])]
        [DataRow(["1", "3", "#5"])]
        public void HasAlteredFifth_AlteredFifthStructure_ReturnsTrue(string[] intervals)
        {
            Assert.IsTrue(IntervalStructure.CreateNew(intervals).HasAlteredFifth);
        }

        [TestMethod]
        [DataRow(["1", "3", "5"])]
        [DataRow(["1", "b3", "5"])]
        public void HasAlteredFifth_NonAlteredFifthStructure_ReturnsFalse(string[] intervals)
        {
            Assert.IsFalse(IntervalStructure.CreateNew(intervals).HasAlteredFifth);
        }

        [TestMethod]
        [DataRow(new[] { 0, 2, 4, 6, 8, 9 })]
        [DataRow(new[] { 0, 2, 4, 6, 6, 9, 11 })]
        public void CreateAsHeptatonic_NonHeptatonicScale_ThrowsErrorWithIllegalValue(int[] intervals)
        {
            Assert.Throws<DomainException>(() => IntervalStructure.CreateAsHeptatonic(intervals));
        }
    }
}