using Aristoxenus.Core.Data;
using Aristoxenus.Core.Utilities;

namespace Aristoxenus.Core.Tests;

[TestClass]
public class PatternHelperTests
{

    [TestMethod]
    [DataRow(3, new[] { 0, 1, 3, 5, 7, 8, 10 })]
    [DataRow(4, new[] { 0, 2, 4, 6, 7, 9, 11 })]
    [DataRow(6, new[] { 0, 2, 3, 5, 7, 8, 10 })]
    [DataRow(7, new[] { 0, 1, 3, 5, 6, 8, 10 })]
    [DataRow(2, new[] { 0, 2, 3, 5, 7, 9, 10 })]
    public void Mode_SingleNumber_ReturnsCorrectRotation(
        int modeIndex,
        IEnumerable<int> expectedPattern)
    {
        var result = PatternHelper.GetModeOfIntervalPattern(RawScalePatterns.Diatonic, modeIndex);
        Assert.IsTrue(Enumerable.SequenceEqual(expectedPattern, result));
    }

    [TestMethod]
    [DataRow(10, new[] { 0, 1, 3, 5, 7, 8, 10 })]
    [DataRow(11, new[] { 0, 2, 4, 6, 7, 9, 11 })]
    [DataRow(13, new[] { 0, 2, 3, 5, 7, 8, 10 })]
    [DataRow(14, new[] { 0, 1, 3, 5, 6, 8, 10 })]
    [DataRow(9, new[] { 0, 2, 3, 5, 7, 9, 10 })]
    public void Mode_OutOfRangeSingleNumber_ReturnsCorrectedRotation(
    int modeIndex,
    IEnumerable<int> expectedPattern)
    {
        var result = PatternHelper.GetModeOfIntervalPattern(RawScalePatterns.Diatonic, modeIndex);
        Assert.IsTrue(Enumerable.SequenceEqual(expectedPattern, result));
    }
}
