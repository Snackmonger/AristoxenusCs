using Aristoxenus.Core.Domains.Theory.Atoms;

namespace Aristoxenus.Tests;

[TestClass]
public class AccidentalTests
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(-1)]
    public void NewObject_SingleNumber_DifferentInstancesTestEqualWithSameValues(int value)
    {
        Assert.AreEqual<Accidental>(new(value), new(value));
    }

    [TestMethod]
    [DataRow(12)]
    [DataRow(-16)]
    [DataRow(23)]
    [DataRow(-343)]
    public void NewObject_IllegalNumber_ThrowsOnIllegalValue(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Accidental(value));
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "#")]
    [DataRow(-1, "b")]
    [DataRow(-2, "bb")]
    [DataRow(2, "##")]
    public void FromSymbol_ValidStringOrInteger_DifferentInstancesTestEqualWithEquivalentValuesOfDifferentTypes(int value, string symbol)
    {
        Assert.AreEqual(new(value), Accidental.FromSymbol(symbol));
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "#")]
    [DataRow(-1, "b")]
    [DataRow(-2, "bb")]
    [DataRow(2, "##")]
    public void ToSymbol_ValidState_GeneratesCorrectSymbolForValue(int value, string expectedSymbol)
    {
        Assert.AreEqual(expectedSymbol, new Accidental(value).ToSymbol());
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
    public void Name_Property_GeneratesCorrectNameForValue(int value, string expectedName)
    {
        Assert.AreEqual(expectedName, new Accidental(value).Name);
    }
}