using Aristoxenus.Domain.Exceptions;
using Aristoxenus.Domain.Models;
namespace AristoxenusTests;

[TestClass]
public class AccidentalTests
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(-1)]
    public void EquivalentAccidentalsTestPositiveForEquality(int value)
    {
        Assert.AreEqual(Accidental.CreateNew(value), Accidental.CreateNew(value));
    }

    [TestMethod]
    [DataRow(12)]
    [DataRow(-16)]
    [DataRow(23)]
    [DataRow(-343)]
    public void AccidentalThrowsWithInvalidData(int value)
    {
        Assert.Throws<DomainException>(() => Accidental.CreateNew(value));
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "#")]
    [DataRow(-1, "b")]
    [DataRow(-2, "bb")]
    [DataRow(2, "##")]
    public void AccidentalCreatedFromIntegerAndFromSymbolAreEquivalent(int value, string symbol)
    {
        Assert.AreEqual(Accidental.CreateNew(value), Accidental.FromSymbol(symbol));
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "#")]
    [DataRow(-1, "b")]
    [DataRow(-2, "bb")]
    [DataRow(2, "##")]
    public void ProducesCorrectSymbolForData(int value, string expectedSymbol)
    {
        Assert.AreEqual(expectedSymbol, Accidental.CreateNew(value).Symbol);
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "sharp")]
    [DataRow(-1, "flat")]
    [DataRow(-2, "double flat")]
    [DataRow(2, "double sharp")]
    [DataRow(-3, "triple flat")]
    [DataRow(3, "triple sharp")]
    [DataRow(-7, "septuple flat")]
    [DataRow(7, "septuple sharp")]
    public void ProducesCorrectNameForData(int value, string expectedName)
    {
        Assert.AreEqual(expectedName, Accidental.CreateNew(value).Name);
    }
}