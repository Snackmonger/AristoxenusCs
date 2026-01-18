
namespace Aristoxenus.Utilities;

/// <summary>
/// Provides methods for encoding and decoding arithmological terms, such 
/// as polyads, cardinals, ordinals, and related number-based nomenclature.
/// </summary>
public class Arithmology
{
    static readonly List<List<string>> Table =
    [
        ["polyad", "tonal", "basal", "cardinal", "ordinal", "uple"],
        ["monad", "monotonic", "primal", "one", "first", "single"],
        ["dyad", "ditonic", "secundal", "two", "second", "double"],
        ["triad", "tritonic", "tertial", "three", "third", "triple"],
        ["tetrad", "tetratonic", "quartal", "four", "fourth", "quadruple"],
        ["pentad", "pentatonic", "quintal", "five", "fifth", "quintuple"],
        ["hexad", "hexatonic", "sextal", "six", "sixth", "sextuple"],
        ["heptad", "heptatonic", "septimal", "seven", "seventh", "septuple"],
        ["octad", "octatonic", "octonal", "eight", "eighth", "octuple"],
        ["ennead", "enneatonic", "nonal", "nine", "ninth", "nonuple"],
        ["decad", "decatonic", "decimal", "ten", "tenth", "decuple"],
        ["hendecad", "hendecatonic", "undecimal", "eleven", "eleventh", "hendecuple"],
        ["duodecad", "duodecatonic", "duodecimal", "twelve", "twelfth", "duodecuple"],
        ["triskaidecad", "triskaidecatonic", "tredecimal", "thirteen", "thirteenth", "tredecuple"],
        ["tettarakaidecad", "tettarakaidecatonic", "quattuordecimal", "fourteen", "fourteenth", "quattuordecuple"],
        ["pentekaidecad", "pentekaidecatonic", "quindecimal", "fifteen", "fifteenth", "quindecuple"]
    ];
    static List<string> Columns()
    {
        return [.. Table[0]];
    }
    static List<string> Rows()
    {
        return [.. Table.Skip(1).Select(row => row[0])];
    }

    public static int Decode(string keyword)
    {
        foreach (var rowData in Rows().Select((value, index) => new { Index = index, Value = value }))
        {
            if (rowData.Value.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
            {
                return rowData.Index;
            }
        }
        throw new ArgumentException($"Unknown keyword: {keyword}");
    }

    public static string Encode(string category, int number)
    {
        if (!Columns().Contains(category))
        {
            throw new ArgumentException($"Unknown category: {category}");
        }
        if (!Enumerable.Range(1, Table.Count).Contains(number))
        {
            throw new ArgumentException($"Cannot encode the number {number}");
        }
        int i = Columns().IndexOf(category);
        return Table[number][i];
    }

    public static string Polyad(int number)
    {
        return Encode("polyad", number);
    }
    public static string Tonal(int number)
    {
        return Encode("tonal", number);
    }
    public static string Basal(int number)
    {
        return Encode("basal", number);
    }
    public static string Cardinal(int number)
    {
        return Encode("cardinal", number);
    }
    public static string Ordinal(int number)
    {
        return Encode("ordinal", number);
    }
    public static string Uple(int number)
    {
        return Encode("uple", number);
    }
}
