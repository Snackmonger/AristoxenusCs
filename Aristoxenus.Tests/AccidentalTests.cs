using Aristoxenus.Core.Domain.Theory;
using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Tests;

[TestClass]
public class AccidentalTests
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(-1)]
    public void CreateNew_SingleNumber_DifferentInstancesTestEqualWithSameValues(int value)
    {
        Assert.AreEqual(Accidental.CreateNew(value), Accidental.CreateNew(value));
    }

    [TestMethod]
    [DataRow(12)]
    [DataRow(-16)]
    [DataRow(23)]
    [DataRow(-343)]
    public void CreateNew_IllegalNumber_ThrowsOnIllegalValue(int value)
    {
        Assert.Throws<DomainException>(() => Accidental.CreateNew(value));
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "#")]
    [DataRow(-1, "b")]
    [DataRow(-2, "bb")]
    [DataRow(2, "##")]
    public void CreateNew_ValidStringOrInteger_DifferentInstancesTestEqualWithEquivalentValuesOfDifferentTypes(int value, string symbol)
    {
        Assert.AreEqual(Accidental.CreateNew(value), Accidental.FromSymbol(symbol));
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(1, "#")]
    [DataRow(-1, "b")]
    [DataRow(-2, "bb")]
    [DataRow(2, "##")]
    public void Symbol_Property_GeneratesCorrectSymbolForValue(int value, string expectedSymbol)
    {
        Assert.AreEqual(expectedSymbol, Accidental.CreateNew(value).ToSymbol());
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
        Assert.AreEqual(expectedName, Accidental.CreateNew(value).Name);
    }
}