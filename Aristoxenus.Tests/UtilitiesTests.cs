
using Aristoxenus.Core.Shared.Utilities;

namespace Aristoxenus.Tests;

[TestClass]
public sealed class UtilitiesTests
{
    [TestMethod]
    [DataRow(5, 2, 6, 0)]
    [DataRow(1, -1, 1, -1)]
    [DataRow(2, 1, 3, 0)]
    [DataRow(6, -1, 6, -1)]
    [DataRow(3, 1, 3, 1)]
    [DataRow(3, -1, 2, 0)]
    [DataRow(2, -1, 2, -1)]
    [DataRow(6, 1, 0, 0)]
    public void GetSimplifiedName_(int inputIndex, int inputAccidentals, int expectedIndex, int expectedAccidentals)
    {
        
        (int, int) name = NomenclatureHelper.GetSimplifiedName(inputIndex, inputAccidentals);
        Assert.AreEqual((expectedIndex, expectedAccidentals), name);

    }
}
