using Aristoxenus.Domain.Core.Errors;
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Tests
{
    [TestClass]
    public sealed class IntervalTests
    {
        [TestMethod]
        [DataRow("1", 0, "1")]
        [DataRow("2", 0, "2")]
        [DataRow("b2", 0, "b2")]
        [DataRow("#2", 0, "#2")]
        [DataRow("3", 0, "3")]
        [DataRow("8", 1, "1")]
        [DataRow("9", 1, "2")]
        [DataRow("b9", 1, "b2")]
        [DataRow("#9", 1, "#2")]
        [DataRow("10", 1, "3")]
        [DataRow("b10", 1, "b3")]
        [DataRow("11", 1, "4")]
        [DataRow("b11", 1, "b4")]
        [DataRow("#11", 1, "#4")]
        [DataRow("12", 1, "5")]
        [DataRow("b12", 1, "b5")]
        [DataRow("13", 1, "6")]
        [DataRow("b13", 1, "b6")]
        [DataRow("14", 1, "7")]
        [DataRow("15", 2, "1")]
        [DataRow("16", 2, "2")]
        [DataRow("b16", 2, "b2")]
        [DataRow("#16", 2, "#2")]
        [DataRow("17", 2, "3")]
        [DataRow("b17", 2, "b3")]
        [DataRow("18", 2, "4")]
        [DataRow("b18", 2, "b4")]
        [DataRow("#18", 2, "#4")]
        [DataRow("19", 2, "5")]
        [DataRow("b19", 2, "b5")]
        [DataRow("20", 2, "6")]
        [DataRow("b20", 2, "b6")]
        [DataRow("21", 2, "7")]
        [DataRow("22", 3, "1")]
        [DataRow("23", 3, "2")]
        [DataRow("24", 3, "3")]
        [DataRow("25", 3, "4")]
        [DataRow("26", 3, "5")]
        [DataRow("27", 3, "6")]
        [DataRow("28", 3, "7")]
        public void Widen_LegalInterval_By_LegalNumberOfOctaves_ReturnsCorrectInterval(string expectedInterval, int octaves, string interval)
        {
            Interval i = Interval.FromSymbol(interval);
            
            Interval widened = i.Widen(octaves);

            Assert.AreEqual(expectedInterval, widened.ToSymbol());

        }


        [TestMethod]
        [DataRow("1", 0, "1")]
        [DataRow("2", 0, "2")]
        [DataRow("b2", 0, "b2")]
        [DataRow("#2", 0, "#2")]
        [DataRow("3", 0, "3")]
        [DataRow("1", 1, "8")]
        [DataRow("2", 1, "9")]
        [DataRow("b2", 1, "b9")]
        [DataRow("#2", 1, "#9")]
        [DataRow("3", 1, "10")]
        [DataRow("b3", 1, "b10")]
        [DataRow("4", 1, "11")]
        [DataRow("b4", 1, "b11")]
        [DataRow("#4", 1, "#11")]
        [DataRow("5", 1, "12")]
        [DataRow("b5", 1, "b12")]
        [DataRow("6", 1, "13")]
        [DataRow("b6", 1, "b13")]
        [DataRow("7", 1, "14")]
        [DataRow("1", 2, "15")]
        [DataRow("2", 2, "16")]
        [DataRow("b2", 2, "b16")]
        [DataRow("#2", 2, "#16")]
        [DataRow("3", 2, "17")]
        [DataRow("b3", 2, "b17")]
        [DataRow("4", 2, "18")]
        [DataRow("b4", 2, "b18")]
        [DataRow("#4", 2, "#18")]
        [DataRow("5", 2, "19")]
        [DataRow("b5", 2, "b19")]
        [DataRow("6", 2, "20")]
        [DataRow("b6", 2, "b20")]
        [DataRow("7", 2, "21")]
        [DataRow("1", 3, "22")]
        [DataRow("2", 3, "23")]
        [DataRow("b2", 3, "b23")]
        [DataRow("#2", 3, "#23")]
        [DataRow("3", 3, "24")]
        [DataRow("b3", 3, "b24")]
        [DataRow("4", 3, "25")]
        [DataRow("b4", 3, "b25")]
        [DataRow("#4", 3, "#25")]
        [DataRow("5", 3, "26")]
        [DataRow("b5", 3, "b26")]
        [DataRow("6", 3, "27")]
        [DataRow("b6", 3, "b27")]
        [DataRow("7", 3, "28")]
        public void Narrow_LegalInterval_By_LegalNumberOfOctaves_ReturnsCorrectInterval(string expectedInterval, int octaves, string interval)
        {
            Interval i = Interval.FromSymbol(interval);

            Interval narrowed = i.Narrow(octaves);

            Assert.AreEqual(expectedInterval, narrowed.ToSymbol());
        }

        [TestMethod]
        public void Widen_LegalInterval_By_ExcessiveNumberOfOctaves_ThrowsException()
        {
            Interval i = Interval.FromSymbol("1");

            Assert.Throws<MaximumOctavesExceeded>(() => i.Widen(9));
        }

        [TestMethod]
        public void Widen_LegalInterval_By_NegativeNumberOfOctaves_ReturnsCorrectedInterval()
        {
            Interval i = Interval.FromSymbol("1");

            Assert.AreEqual("8", i.Widen(-1).ToSymbol());
        }
    }
}
