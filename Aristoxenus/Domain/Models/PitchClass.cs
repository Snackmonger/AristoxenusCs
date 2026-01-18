using Aristoxenus.Constants;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;


namespace Aristoxenus.Domain.Models;

/// <summary>
/// Represents the alphabetic part of a note name, which may also be thought
/// of as a position in the diatonic scale.
/// </summary>
public class PitchClass : ValueObject, ISymbolSerializable<PitchClass>
{
    public readonly int Value;

    private PitchClass(int value)
    {
        Value = value;
    }

    public static PitchClass CreateNew(int value)
        => value is < 0 or > 6
            ? throw new ArgumentException(
                "Pitch class must be between 0 and 6.")
            : new PitchClass(value);


    public static PitchClass CreateNew(string value)
    => Symbols.NaturalNoteNames.Contains(value)
        ? CreateNew(Symbols.NaturalNoteNames.IndexOf(value))
        : throw new ArgumentException(
            $"Pitch class must be one of {Symbols.NaturalNoteNames}, got {value}");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static PitchClass FromSymbol(string symbol)
    {
        throw new NotImplementedException();
    }

    public string Symbol => Symbols.NaturalNoteNames[Value].ToString();


}
