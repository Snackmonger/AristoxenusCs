namespace Aristoxenus.Core.Constants
{
    /// <summary>
    /// Symbols that are used as elements to constuct other symbols.
    /// </summary>
    public class Symbols
    {
        public const string Whitespace = " ";
        public const string Sharp = "#";
        public const string Flat = "b";
        public const string NaturalNoteNames = "CDEFGAB";
        public const string CentralNoteName = "A4";
        public const string Diminished = "dim";
        public const string O = "o";
        public const string Augmented = "aug";
        public const string Major = "maj";
        public const string Delta = "Δ";
        public const string Oe = "ø";
        public const string Minor = "min";
        public const string UpperM = "M";
        public const string LowerM = "m";
        public const string Plus = "+";
        public const string Minus = "-";
        public const string Sus = "sus";
        public const string Add = "add";
        public const string No = "no";
        public const string One = "1";
        public const string Two = "2";
        public const string Three = "3";
        public const string Four = "4";
        public const string Five = "5";
        public const string Six = "6";
        public const string Seven = "7";
        public const string Nine = "9";
        public const string Eleven = "11";
        public const string Thirteen = "13";
        public const string FlatTwo = Flat + Two;
        public const string SharpTwo = Sharp + Two;
        public const string FlatFour = Flat + Four;
        public const string SharpFour = Sharp + Four;
        public const string DoubleFlatThree = Flat + Flat + Three;
        public const string FlatThree = Flat + Three;
        public const string SharpThree = Sharp + Three;
        public const string FlatFive = Flat + Five;
        public const string DoubleFlatFive = Flat + Flat + Five;
        public const string SharpFive = Sharp + Five;
        public const string DoubleSharpFive = Sharp + Sharp + Five;
        public const string DoubleFlatSeven = Flat + Flat + Seven;
        public const string FlatSeven = Flat + Seven;
        public static readonly string[] RecognizedMajorSymbols = [
            Major,
            UpperM,
            Delta
        ];
        public static readonly string[] RecognizedMinorSymbols = [
            Minor,
            LowerM,
            Minus
        ];
        public static readonly string[] RecognizedDiminishedSymbols = [
            Diminished,
            O
        ];
        public static readonly string[] RecognizedAugmentedSymbols = [
            Augmented,
            Plus
        ];
        public static readonly string[] RecognizedHalfDiminishedSymbols = [
            Oe
        ];
    }
}
