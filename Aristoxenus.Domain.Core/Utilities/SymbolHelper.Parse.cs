using System.Text.RegularExpressions;

using Libraries.RomanNumerals;

using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Errors;

namespace Aristoxenus.Domain.Core.Utilities;

public static partial class SymbolHelper
{

    public static ((int pitchClassIndex, int accidental), int octave) ParsePitchSymbol(string pitchSymbol)
    {
        var data = RegularExpressions.PitchSymbol.Match(pitchSymbol);
        if (!data.Success)
        {
            throw new InvalidAsSymbol(pitchSymbol, nameof(pitchSymbol));
        }
        int name = ParsePitchClassSymbol(data.Groups[1].Value);
        int accidental = ParseAccidentalSymbol(data.Groups[2].Value);
        int octave = int.Parse(data.Groups[3].Value);
        return ((name, accidental), octave);
    }


    public static (int, int) ParseNoteNameSymbol(string noteNameSymbol)
    {
        int accidental;
        var data = RegularExpressions.NoteName.Match(noteNameSymbol);
        if (!data.Success)
        {
            throw new InvalidAsSymbol(noteNameSymbol, nameof(noteNameSymbol));
        }
        GroupCollection groups = data.Groups;
        string naturalNoteName = groups[1].Value;
        if (groups[2].Value != null)
        {
            int sharps = RegularExpressions.SharpSymbol.Count(groups[2].Value);
            int flats = RegularExpressions.FlatSymbol.Count(groups[2].Value);
            accidental = sharps - flats;
        }
        else
        {
            accidental = Numbers.Zero;
        }

        return new(ParsePitchClassSymbol(naturalNoteName), accidental);
    }

    public static int ParsePitchClassSymbol(string pitchClassSymbol)
    {
        pitchClassSymbol = pitchClassSymbol.ToUpper();
        if (Symbols.NaturalNoteNames.Contains(pitchClassSymbol))
            return Symbols.NaturalNoteNames.IndexOf(pitchClassSymbol);
        throw new InvalidAsSymbol(pitchClassSymbol, nameof(pitchClassSymbol));
    }

    public static (int, int) ParseScaleDegreeSymbol(string scaleDegreeSymbol)
    {
        Match data = RegularExpressions.ScaleDegreeSymbol.Match(scaleDegreeSymbol);
        if (!data.Success)
            throw new InvalidAsSymbol(scaleDegreeSymbol, nameof(scaleDegreeSymbol));

        GroupCollection groups = data.Groups;
        int accidentalIdx = 1;
        int nameIdx = 2;
        int degree;
        string name = groups[nameIdx].Value;
        if (name.All(char.IsDigit))
        {
            degree = int.Parse(name);
        }
        else
        {
            try
            {
                degree = RomanNumerals.Decode(name.ToUpper());
            }
            catch (ArgumentException)
            {
                throw new InvalidAsSymbol(scaleDegreeSymbol, nameof(scaleDegreeSymbol));
            }
        }
        int accidental = Numbers.Zero;
        if (groups[accidentalIdx] != null)
        {
            // The actual string will only ever have one or the other, per the regex.
            int sharps = RegularExpressions.SharpSymbol.Count(groups[accidentalIdx].Value);
            int flats = RegularExpressions.FlatSymbol.Count(groups[accidentalIdx].Value);
            accidental = sharps - flats;
        }
        return (degree, accidental);
    }

    public static int ParseAccidentalSymbol(string accidentalSymbol)
        => (
            RegularExpressions.SharpSymbol.Count(accidentalSymbol)
            - RegularExpressions.FlatSymbol.Count(accidentalSymbol)
        )
        % Numbers.MaxAccidentals;

}
