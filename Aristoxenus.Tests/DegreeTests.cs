using Aristoxenus.Core.Domain.Theory;
using Aristoxenus.Core.SharedKernel.Bases;
namespace Aristoxenus.Tests
{
    [TestClass]
    public sealed class DegreeTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(16)]
        public void CreateNew_IllegalNumber_ThrowsOnIllegalValue(int value)
        {
            Assert.Throws<DomainException>(() => Degree.CreateNew(value));
        }

        [TestMethod]
        [DataRow("-1")]
        [DataRow("0")]
        [DataRow("16")]
        public void FromSymbol_IllegalString_ThrowsOnIllegalValue(string value)
        {
            Assert.Throws<DomainException>(() => Degree.FromSymbol(value));
        }

        [TestMethod]
        [DataRow("-1")]
        [DataRow("0")]
        [DataRow("16")]
        public void CreateNew_IllegalString_ThrowsOnIllegalValue(string value)
        {
            Assert.Throws<DomainException>(() => Degree.CreateNew(value));
        }

        [TestMethod]
        [DataRow(3, 3)]
        [DataRow(10, 3)]
        [DataRow(6, 6)]
        [DataRow(13, 6)]
        public void FirstOctaveValue_Property_GeneratesCorrectValue(int degreeValue, int expectedFirstOctave)
        {
            Assert.AreEqual(expectedFirstOctave, Degree.CreateNew(degreeValue).FirstOctaveValue);
        }

        [TestMethod]
        [DataRow(6, 13)]
        [DataRow(13, 13)]
        [DataRow(3, 10)]
        [DataRow(10, 10)]
        public void SecondOctaveValue_Property_GeneratesCorrectValue(int degreeValue, int expectedSecondOctave)
        {
            Assert.AreEqual(expectedSecondOctave, Degree.CreateNew(degreeValue).SecondOctaveValue);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(10, "10")]
        [DataRow(6, "6")]
        [DataRow(13, "13")]
        public void Symbol_Property_GeneratesCorrectValue(int degreeValue, string expectedSymbol)
        {
            Assert.AreEqual(expectedSymbol, Degree.CreateNew(degreeValue).ToSymbol());
        }

        [TestMethod]
        [DataRow(1, "I")]
        [DataRow(4, "IV")]
        [DataRow(7, "VII")]
        public void Roman_Property_GeneratesCorrectValue(int degreeValue, string expectedRoman)
        {
            Assert.AreEqual(expectedRoman, Degree.CreateNew(degreeValue).Roman);
        }

        [TestMethod]
        [DataRow(1, "one")]
        [DataRow(4, "four")]
        [DataRow(7, "seven")]
        public void Cardinal_Property_GeneratesCorrectValue(int degreeValue, string expectedCardinal)
        {
            Assert.AreEqual(expectedCardinal, Degree.CreateNew(degreeValue).Cardinal);
        }

        [TestMethod]
        [DataRow(1, "first")]
        [DataRow(4, "fourth")]
        [DataRow(7, "seventh")]
        public void Ordinal_Property_GeneratesCorrectValue(int degreeValue, string expectedOrdinal)
        {
            Assert.AreEqual(expectedOrdinal, Degree.CreateNew(degreeValue).Ordinal);
        }
    }
}