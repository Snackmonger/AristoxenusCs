using Aristoxenus.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aristoxenus.Domain.Core.Tests
{
    [TestClass]
    public class PitchTests
    {
        private static readonly Pitch C4 = new(new(0, 0), 4);
        private static readonly Pitch B4 = new(new(6, 0), 4);


        [TestMethod(DisplayName = "Calculates the correct pitch above C4 based on a scale degree symbol.")]
        [DataRow("C4", "1")]
        [DataRow("D4", "2")]
        [DataRow("E4", "3")]
        [DataRow("F4", "4")]
        [DataRow("G4", "5")]
        [DataRow("A4", "6")]
        [DataRow("B4", "7")]
        [DataRow("C5", "8")]
        [DataRow("D5", "9")]
        [DataRow("E5", "10")]
        [DataRow("F5", "11")]
        [DataRow("G5", "12")]
        [DataRow("A5", "13")]
        [DataRow("B5", "14")]
        [DataRow("C6", "15")]
        [DataRow("D6", "16")]
        [DataRow("E6", "17")]
        [DataRow("F6", "18")]
        [DataRow("G6", "19")]
        [DataRow("A6", "20")]
        [DataRow("B6", "21")]
        [DataRow("C7", "22")]
        public void GetPitchAbove_C4InBass_ReturnsCorrectPitch(string expectedPitch, string scaleDegree)
        {
            Pitch p = C4.GetPitchAbove(Interval.FromSymbol(scaleDegree));

            Assert.AreEqual(expectedPitch, p.ToSymbol());
        }


        [TestMethod(DisplayName = "Calculates the correct pitch above B4 based on a scale degree symbol.")]
        [DataRow("B4", "1")]
        [DataRow("C#5", "2")]
        [DataRow("D#5", "3")]
        [DataRow("E5", "4")]
        [DataRow("F#5", "5")]
        [DataRow("G#5", "6")]
        [DataRow("A#5", "7")]
        [DataRow("B5", "8")]
        [DataRow("C#6", "9")]
        [DataRow("D#6", "10")]
        [DataRow("E6", "11")]
        [DataRow("F#6", "12")]
        [DataRow("G#6", "13")]
        [DataRow("A#6", "14")]
        [DataRow("B6", "15")]
        [DataRow("C#7", "16")]
        [DataRow("D#7", "17")]
        [DataRow("E7", "18")]
        [DataRow("F#7", "19")]
        [DataRow("G#7", "20")]
        [DataRow("A#7", "21")]
        [DataRow("B7", "22")]
        public void GetPitchAbove_B4InBass_ReturnsCorrectPitch(string expectedPitch, string scaleDegree)
        {
            Pitch p = B4.GetPitchAbove(Interval.FromSymbol(scaleDegree));

            Assert.AreEqual(expectedPitch, p.ToSymbol());
        }
    }
}
