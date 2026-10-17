using Libraries.Arithmology;
using Libraries.RomanNumerals;

using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Utilities;
using Aristoxenus.Domain.Core.Interfaces;

namespace Aristoxenus.Domain.Core.Models;

public sealed class ScaleDegree(int degree, int accidental) : ValueObject, ISymbolSerializable<ScaleDegree>
{
    /// <summary>
    /// The base scale degree.
    /// </summary>
    public readonly int Degree = NormalizationHelper.NormalizeDegree(degree);

    /// <summary>
    /// The positive or negative amount by which the normal value of the 
    /// degree is skewed higher or lower, respectively.
    /// 
    /// Invariant: The accidental is always normalized to be within the range of -11 to 11, inclusive.
    /// </summary>
    public readonly int Accidental = NormalizationHelper.NormalizeAccidental(accidental);


    public static ScaleDegree FromSymbol(string symbol)
    {
        (int degree, int accidental) = SymbolHelper.ParseScaleDegreeSymbol(symbol);
        return new(degree, accidental);
    }

    /// <summary>
    /// The Roman numeral symbol for this scale degree (e.g. "#II")
    /// </summary>
    public string RomanName
        => $"{SymbolHelper.CreateAccidentalSymbol(Accidental)}{RomanNumerals.Encode(Degree % (Numbers.NotesInDoubleOctave))}";

    /// <summary>
    /// The cardinal symbol for this scale degree (e.g. "sharp two")
    /// </summary>
    public string CardinalName
        => $"{SymbolHelper.CreateAccidentalName(Accidental)} {Arithmology.Cardinal(Degree)}";

    /// <summary>
    /// The ordinal symbol for this scale degree (e.g. "sharp second")
    /// </summary>
    public string OrdinalName
        => $"{SymbolHelper.CreateAccidentalName(Accidental)} {Arithmology.Ordinal(Degree)}";

    public string ToSymbol()
        => SymbolHelper.CreateScaleDegreeSymbol((Degree, Accidental));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Degree; 
        yield return Accidental;
    }

    public static implicit operator (int, int)(ScaleDegree scaleDegree)
    => (scaleDegree.Degree, scaleDegree.Accidental);

    public static implicit operator ScaleDegree((int degree, int accidental) scaleDegree)
        => new(scaleDegree.degree, scaleDegree.accidental);

    public static implicit operator string(ScaleDegree scaleDegree)
        => SymbolHelper.CreateScaleDegreeSymbol(scaleDegree);

    public static implicit operator ScaleDegree(string scaleDegree)
        => SymbolHelper.ParseScaleDegreeSymbol(scaleDegree);
}

