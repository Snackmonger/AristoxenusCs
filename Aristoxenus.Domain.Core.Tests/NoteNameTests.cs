
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Tests;

[TestClass]
public sealed class NoteNameTests
{
    private readonly NoteName NoteName_GNatural = new(4, 0);
    private readonly NoteName NoteName_GSharp = new(4, 1); // TODO: missing `IntervalAbove` test data 
    private readonly NoteName NoteName_ANatural = new(5, 0);
    private readonly NoteName NoteName_FNatural = new(3, 0); // TODO: missing `IntervalAbove` test data

    [TestMethod]
    [DataRow(0, 0, 7, 0)]
    [DataRow(0, 0, 0, 12)]
    [DataRow(3, -1, -3, -1)]
    [DataRow(4, 2, 11, 2)]
    [DataRow(1, -2, 8, -2)]
    [DataRow(2, 0, -9, 0)]
    [DataRow(0, 0, 0, -12)]
    [DataRow(0, 0, 14, 24)]
    [DataRow(1, 7, 8, 7)]
    [DataRow(1, 3, 22, 15)]
    public void Constructor_ParameterValuesOutOfRange_InitializesWithNormalizedValues(int expectedPitchClass, int expectedAccidental, int pitchClass, int accidental)
    {
        NoteName noteName = new(pitchClass, accidental);

        (int, int) expectedValues = (expectedPitchClass, expectedAccidental);
        (int, int) actualValues = (noteName.PitchClass, noteName.Accidental);

        Assert.AreEqual(expectedValues, actualValues);
    }


    [TestMethod]
    [DataRow("Ab", 5, -1)]
    [DataRow("A", 5, 0)]
    [DataRow("G#", 4, 1)]
    [DataRow("Bb", 6, -1)]
    [DataRow("C#", 0, 1)]
    [DataRow("Cb", 0, -1)]
    [DataRow("B#", 6, 1)]
    [DataRow("F", 3, 0)]
    [DataRow("F#", 3, 1)]
    [DataRow("Fb", 3, -1)]
    [DataRow("Fbb", 3, -2)]
    [DataRow("F##", 3, 2)]
    [DataRow("Cbb", 0, -2)]
    [DataRow("C##", 0, 2)]
    [DataRow("B##", 6, 2)]
    public void ToSymbol_AnyNoteName_ReturnsCorrectStringSymbol(string expectedNoteNameSymbol, int pitchClass, int accidental)
    {
        NoteName noteName = new(pitchClass, accidental);

        string symbol = noteName.ToSymbol();

        Assert.AreEqual(expectedNoteNameSymbol, symbol);
    }

    [TestMethod]
    [DataRow(5, -1, "Ab")]
    [DataRow(5, 1, "A#")]
    [DataRow(5, 0, "A")]
    [DataRow(6, 1, "B#")]
    [DataRow(3, 1, "F#")]
    [DataRow(0, -1, "Cb")]
    [DataRow(2, -1, "Eb")]
    [DataRow(1, -1, "Db")]
    [DataRow(1, 1, "D#")]
    [DataRow(2, 1, "E#")]
    [DataRow(0, 1, "C#")]
    [DataRow(0, 2, "C##")]
    [DataRow(2, -2, "Ebb")]
    [DataRow(3, 2, "F##")]
    [DataRow(6, 2, "B##")]
    [DataRow(6, -2, "Bbb")]
    [DataRow(6, -7, "Bbbbbbbb")]
    [DataRow(2, -3, "Ebbbbbbbbbbbbbbb")]
    [DataRow(6, -2, "Bbbbbbbbbbbbbbb")]
    [DataRow(3, -1, "Fbbbbbbbbbbbbb")]
    public void FromSymbol_AnyNoteName_ReturnsObjectWithCorrectValues(int expectedPitchClass, int expectedAccidental, string noteNameSymbol)
    {
        NoteName noteName = NoteName.FromSymbol(noteNameSymbol);
        (int, int) expected = (expectedPitchClass, expectedAccidental);
        (int, int) actual = (noteName.PitchClass, noteName.Accidental);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("b2", "Ab")]
    [DataRow("2", "A")]
    [DataRow("#2", "A#")]
    [DataRow("##2", "A##")]
    [DataRow("###2", "A###")]
    [DataRow("####2", "A####")]
    [DataRow("#####2", "A#####")]
    [DataRow("######2", "A######")]
    [DataRow("#######2", "A#######")]
    [DataRow("########2", "A########")]
    [DataRow("b3", "Bb")]
    [DataRow("3", "B")]
    [DataRow("b4", "Cb")]
    [DataRow("4", "C")]
    [DataRow("#4", "C#")]
    [DataRow("b5", "Db")]
    [DataRow("bb5", "Dbb")]
    [DataRow("bbb5", "Dbbb")]
    [DataRow("bbbb5", "Dbbbb")]
    [DataRow("bbbbb5", "Dbbbbb")]
    [DataRow("bbbbbb5", "Dbbbbbb")]
    [DataRow("bbbbbbb5", "Dbbbbbbb")]
    [DataRow("5", "D")]
    [DataRow("#5", "D#")]
    [DataRow("##5", "D##")]
    [DataRow("b6", "Eb")]
    [DataRow("bb6", "Ebb")]
    [DataRow("bbb6", "Ebbb")]
    [DataRow("bbbb6", "Ebbbb")]
    [DataRow("bbbbb6", "Ebbbbb")]
    [DataRow("bbbbbb6", "Ebbbbbb")]
    [DataRow("bbbbbbb6", "Ebbbbbbb")]
    [DataRow("6", "E")]
    [DataRow("#6", "E#")]
    [DataRow("bb7", "Fb")]
    [DataRow("bbbbbbbb7", "Fbbbbbbb")]
    [DataRow("bbbbbbbbb7", "Fbbbbbbbb")]
    [DataRow("bbbbbbbbbb7", "Fbbbbbbbbb")]
    [DataRow("b7", "F")]
    [DataRow("7", "F#")]
    public void IntervalAbove_GNaturalInBass_ReturnsCorrectInterval(string expectedScaleDegreeSymbol, string noteNameAboveGNatural)
    {
        Interval expectedInterval = Interval.FromScaleDegreeSymbol(expectedScaleDegreeSymbol);
        NoteName inputNoteName = NoteName.FromSymbol(noteNameAboveGNatural);

        Interval actualInterval = NoteName_GNatural.IntervalAbove(inputNoteName);

        Assert.AreEqual(expectedInterval, actualInterval);
    }

    [TestMethod]
    [DataRow("#1", "A#")]
    [DataRow("b2", "Bb")]
    [DataRow("2", "B")]
    [DataRow("bb3", "Cb")]
    [DataRow("b3", "C")]
    [DataRow("3", "C#")]
    [DataRow("#3", "C##")]
    [DataRow("b4", "Db")]
    [DataRow("4", "D")]
    [DataRow("#4", "D#")]
    [DataRow("b5", "Eb")]
    [DataRow("5", "E")]
    [DataRow("#5", "E#")]
    [DataRow("bb6", "Fb")]
    [DataRow("b6", "F")]
    [DataRow("6", "F#")]
    [DataRow("#6", "F##")]
    [DataRow("bb7", "Gb")]
    [DataRow("b7", "G")]
    [DataRow("7", "G#")]
    public void IntervalAbove_ANaturalInBass_ReturnsCorrectInterval(string expectedScaleDegreeSymbol, string noteNameAboveANatural)
    {
        Interval expectedInterval = Interval.FromScaleDegreeSymbol(expectedScaleDegreeSymbol);
        NoteName inputNoteName = NoteName.FromSymbol(noteNameAboveANatural);

        Interval actualInterval = NoteName_ANatural.IntervalAbove(inputNoteName);

        Assert.AreEqual(expectedInterval, actualInterval);
    }

    [TestMethod]
    [DataRow("B", "A##")]
    [DataRow("B", "Cb")]
    [DataRow("F", "E#")]
    [DataRow("F#", "F#")]
    [DataRow("E", "Fb")]
    [DataRow("C", "B#")]
    [DataRow("D", "Ebb")]
    [DataRow("Db", "Db")]
    [DataRow("Bb", "Bb")]
    [DataRow("A#", "A#")]
    public void SimplifiedName_AnyNoteName_ReturnsSimplestFormOfNoteName(string expectedNoteNameSymbol, string noteNameSymbol)
    {
        NoteName expectedNoteName = NoteName.FromSymbol(expectedNoteNameSymbol);
        NoteName inputNoteName = NoteName.FromSymbol(noteNameSymbol);

        NoteName actualNoteName = inputNoteName.SimplifiedName;

        Assert.AreEqual(expectedNoteName, actualNoteName);
    }

    [TestMethod]
    [DataRow("Ab", "b2")]
    [DataRow("A", "2")]
    [DataRow("A#", "#2")]
    [DataRow("A##", "##2")]
    [DataRow("A###", "###2")]
    [DataRow("A####", "####2")]
    [DataRow("A#####", "#####2")]
    [DataRow("A######", "######2")]
    [DataRow("A#######", "#######2")]
    [DataRow("A########", "########2")]
    [DataRow("Bbb", "bb3")]
    [DataRow("Bb", "b3")]
    [DataRow("B", "3")]
    [DataRow("B#", "#3")]
    [DataRow("B#####", "#####3")]
    [DataRow("B######", "######3")]
    [DataRow("B#######", "#######3")]
    [DataRow("Cbb", "bb4")]
    [DataRow("Cb", "b4")]
    [DataRow("C", "4")]
    [DataRow("C#", "#4")]
    [DataRow("C##", "##4")]
    [DataRow("Dbb", "bb5")]
    [DataRow("Db", "b5")]
    [DataRow("D", "5")]
    [DataRow("D#", "#5")]
    [DataRow("D##", "##5")]
    [DataRow("Ebb", "bb6")]
    [DataRow("Ebbb", "bbb6")]
    [DataRow("Ebbbb", "bbbb6")]
    [DataRow("Ebbbbb", "bbbbb6")]
    [DataRow("Ebbbbbb", "bbbbbb6")]
    [DataRow("Ebbbbbbb", "bbbbbbb6")]
    [DataRow("Ebbbbbbbb", "bbbbbbbb6")]
    [DataRow("Eb", "b6")]
    [DataRow("E", "6")]
    [DataRow("E#", "#6")]
    [DataRow("Fb", "bb7")]
    [DataRow("F", "b7")]
    [DataRow("F#", "7")]
    public void NoteNameAbove_GNaturalInBass_ReturnsCorrectNoteNameAbove(string expectedNoteNameSymbol, string intervalSymbol)
    {
        NoteName expectedNoteName = NoteName.FromSymbol(expectedNoteNameSymbol);
        Interval inputInterval = Interval.FromScaleDegreeSymbol(intervalSymbol);

        NoteName actualNoteName = NoteName_GNatural.NoteNameAbove(inputInterval);

        Assert.AreEqual(expectedNoteName, actualNoteName);
    }


    [TestMethod]
    [DataRow("A", "b2")]
    [DataRow("A#", "2")]
    [DataRow("A##", "#2")]
    [DataRow("A###", "##2")]
    [DataRow("Bb", "bb3")]
    [DataRow("B", "b3")]
    [DataRow("B#", "3")]
    [DataRow("C", "b4")]
    [DataRow("C#", "4")]
    [DataRow("C##", "#4")]
    [DataRow("Db", "bb5")]
    [DataRow("D", "b5")]
    [DataRow("D#", "5")]
    [DataRow("D##", "#5")]
    [DataRow("E", "b6")]
    [DataRow("E#", "6")]
    [DataRow("E##", "#6")]
    [DataRow("F", "bb7")]
    [DataRow("F#", "b7")]
    [DataRow("F##", "7")]
    public void NoteNameAbove_GSharpInBass_ReturnsCorrectNoteNameAbove(string expectedNoteNameSymbol, string intervalSymbol)
    {
        NoteName expectedNoteName = NoteName.FromSymbol(expectedNoteNameSymbol);
        Interval inputInterval = Interval.FromScaleDegreeSymbol(intervalSymbol);

        NoteName actualNoteName = NoteName_GSharp.NoteNameAbove(inputInterval);

        Assert.AreEqual(expectedNoteName, actualNoteName);
    }

    [TestMethod]
    [DataRow("Bb", "b2")]
    [DataRow("B", "2")]
    [DataRow("B#", "#2")]
    [DataRow("C", "b3")]
    [DataRow("C#", "3")]
    [DataRow("Db", "b4")]
    [DataRow("D", "4")]
    [DataRow("D#", "#4")]
    [DataRow("D##", "##4")]
    [DataRow("E", "5")]
    [DataRow("Eb", "b5")]
    [DataRow("E##", "##5")]
    [DataRow("E#", "#5")]
    [DataRow("Fb", "bb6")]
    [DataRow("F", "b6")]
    [DataRow("F#", "6")]
    [DataRow("F##", "#6")]
    [DataRow("Gb", "bb7")]
    [DataRow("G", "b7")]
    [DataRow("G#", "7")]
    public void NoteNameAbove_ANaturalInBass_ReturnsCorrectNoteNameAbove(string expectedNoteNameSymbol, string intervalSymbol)
    {
        NoteName expectedNoteName = NoteName.FromSymbol(expectedNoteNameSymbol);
        Interval inputInterval = Interval.FromScaleDegreeSymbol(intervalSymbol);

        NoteName actualNoteName = NoteName_ANatural.NoteNameAbove(inputInterval);

        Assert.AreEqual(expectedNoteName, actualNoteName);
    }


    [TestMethod]
    [DataRow("Gb", "b2")]
    [DataRow("G", "2")]
    [DataRow("G#", "#2")]
    [DataRow("G##", "##2")]
    [DataRow("Ab", "b3")]
    [DataRow("Abb", "bb3")]
    [DataRow("A", "3")]
    [DataRow("A#", "#3")]
    [DataRow("A##", "##3")]
    [DataRow("Bbb", "b4")]
    [DataRow("Bb", "4")]
    [DataRow("B", "#4")]
    [DataRow("B#", "##4")]
    [DataRow("Cbb", "bb5")]
    [DataRow("Cb", "b5")]
    [DataRow("C", "5")]
    [DataRow("C#", "#5")]
    [DataRow("C##", "##5")]
    [DataRow("Dbb", "bb6")]
    [DataRow("Db", "b6")]
    [DataRow("D", "6")]
    [DataRow("D#", "#6")]
    [DataRow("Ebb", "bb7")]
    [DataRow("Eb", "b7")]
    [DataRow("E", "7")]
    public void NoteNameAbove_FNaturalInBass_ReturnsCorrectNoteNameAbove(string expectedNoteNameSymbol, string intervalSymbol)
    {
        NoteName expectedNoteName = NoteName.FromSymbol(expectedNoteNameSymbol);
        Interval inputInterval = Interval.FromScaleDegreeSymbol(intervalSymbol);

        NoteName actualNoteName = NoteName_FNatural.NoteNameAbove(inputInterval);

        Assert.AreEqual(expectedNoteName, actualNoteName);
    }

    [TestMethod]
    [DataRow(true, "E#", "F")]
    [DataRow(true, "D#", "Eb")]
    [DataRow(true, "E", "Fb")]
    [DataRow(true, "E##", "F#")]
    [DataRow(true, "Bb", "A#")]
    [DataRow(true, "Gb", "F#")]
    [DataRow(true, "Db", "C#")]
    [DataRow(true, "C", "B#")]
    [DataRow(true, "Cb", "B")]
    [DataRow(true, "A", "Bbb")]
    [DataRow(true, "A######", "Abbbbbb")]
    [DataRow(true, "C", "G#####")]
    [DataRow(true, "C#######", "G")]
    [DataRow(true, "E#######", "Ebbbbb")]
    [DataRow(true, "Fbbbbbbbb", "A")]
    public void IsEnharmonicallyEquivalent_AnyNoteName_ReturnsAccurateAssessment(bool expectedBoolean, string noteName1, string noteName2)
    {
        NoteName n1 = NoteName.FromSymbol(noteName1);
        NoteName n2 = NoteName.FromSymbol(noteName2);

        bool result = n1.IsEnharmonicallyEquivalent(n2);

        Assert.AreEqual(expectedBoolean, result);
    }


    [TestMethod]
    [DataRow(true, "C##")]
    [DataRow(false, "C#")]
    [DataRow(true, "C")]
    [DataRow(true, "Cb")]
    [DataRow(false, "Cbb")]
    [DataRow(false, "E##")]
    [DataRow(true, "E#")]
    [DataRow(true, "E")]
    [DataRow(true, "Fb")]
    [DataRow(true, "Ebb")]
    [DataRow(true, "B")]
    [DataRow(true, "B#")]
    [DataRow(false, "Ab")]
    [DataRow(true, "Abb")]
    [DataRow(true, "Bbb")]
    [DataRow(true, "Bbbbbbbb")]
    [DataRow(true, "C#####")]
    [DataRow(true, "A########")]
    [DataRow(true, "Gbbbbbbbbbb")]
    [DataRow(true, "Dbbbbbbbbb")]
    public void IsEnharmonicallyNatural_AnyNoteName_ReturnsAccurateAssessment(bool expectedBoolean, string noteName)
    {
        NoteName name = NoteName.FromSymbol(noteName);

        bool result = name.IsEnharmonicallyNatural;

        Assert.AreEqual(expectedBoolean, result);
    }

    [TestMethod]
    [DataRow("A#", "Bb")]
    [DataRow("B#", "C")]
    [DataRow("C", "B#")]
    [DataRow("Bb", "A#")]
    [DataRow("G", "G")]
    [DataRow("G#", "Ab")]
    [DataRow("Gb", "F#")]
    [DataRow("Eb", "D#")]
    [DataRow("E#", "F")]
    [DataRow("F", "E#")]
    public void FlipBinomial_AnyNoteNameWithOneOrZeroAccidental_ReturnsCorrectNoteName(string expectedNoteName, string noteName)
    {
        NoteName expected = NoteName.FromSymbol(expectedNoteName);
        NoteName name = NoteName.FromSymbol(noteName);

        NoteName result = name.FlipBinomial();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("F##")]
    [DataRow("Cbbbb")]
    [DataRow("B##")]
    [DataRow("Ebb")]
    [DataRow("Abbb")]
    public void FlipBinomial_AnyNoteNameWithMoreThanOneAccidental_ThrowsInvalidOperationException(string noteName)
    {
        NoteName name = NoteName.FromSymbol(noteName);

        Assert.Throws<InvalidOperationException>(name.FlipBinomial);
    }
}
