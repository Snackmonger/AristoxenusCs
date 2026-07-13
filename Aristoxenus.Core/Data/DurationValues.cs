using Fractions;

namespace Aristoxenus.Core.Data;

public class DurationValues
{
    public static readonly Fraction Longa = 4;
    public static readonly Fraction Breve = 2;
    public static readonly Fraction Whole = 1;
    public static readonly Fraction Semibreve = 1;
    public static readonly Fraction Minim = new(1, 2);
    public static readonly Fraction Half = new(1, 2);
    public static readonly Fraction Crotchet = new(1, 4);
    public static readonly Fraction Quarter = new(1, 4);
    public static readonly Fraction Quaver = new(1, 8);
    public static readonly Fraction Eighth = new(1, 8);
    public static readonly Fraction Semiquaver = new(1, 16);
    public static readonly Fraction Sixteenth = new(1, 16);
    public static readonly Fraction Demisemiquaver = new(1, 32);
    public static readonly Fraction ThirtySecond = new(1, 32);
    public static readonly Fraction Hemidemisemiquaver = new(1, 64);
    public static readonly Fraction SixtyFourth = new(1, 64);
    public static readonly Fraction Semihemidemisemiquaver = new(1, 128);
    public static readonly Fraction HundredTwentyEighth = new(1, 128);
}
