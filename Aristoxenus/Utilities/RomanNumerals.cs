using System.Text;
namespace Aristoxenus.Utilities;

internal class RomanNumerals
{
    public static string Encode(int number)
    {
        if (number <= 0 || number >= 4000)
        {
            throw new ArgumentException("Number must be between 1 and 3999.");
        }
        var romanNumerals = new[]
        {
                new { Value = 1000, Numeral = "M" },
                new { Value = 900, Numeral = "CM" },
                new { Value = 500, Numeral = "D" },
                new { Value = 400, Numeral = "CD" },
                new { Value = 100, Numeral = "C" },
                new { Value = 90, Numeral = "XC" },
                new { Value = 50, Numeral = "L" },
                new { Value = 40, Numeral = "XL" },
                new { Value = 10, Numeral = "X" },
                new { Value = 9, Numeral = "IX" },
                new { Value = 5, Numeral = "V" },
                new { Value = 4, Numeral = "IV" },
                new { Value = 1, Numeral = "I" }
            };
        var result = new StringBuilder();
        foreach (var item in romanNumerals)
        {
            while (number >= item.Value)
            {
                result.Append(item.Numeral);
                number -= item.Value;
            }
        }
        return result.ToString();
    }

    public static int Decode(string romanNumeral)
    {
        var romanToIntegerMap = new Dictionary<string, int>
            {
                {"M", 1000},
                {"D", 500},
                {"C", 100},
                {"L", 50},
                {"X", 10},
                {"V", 5},
                {"I", 1}
            };
        int total = 0;
        int prevValue = 0;
        for (int i = romanNumeral.Length - 1; i >= 0; i--)
        {
            string currentChar = romanNumeral[i].ToString();
            if (!romanToIntegerMap.TryGetValue(currentChar, out int currentValue))
            {
                throw new ArgumentException($"Invalid Roman numeral character: {currentChar}");
            }
            if (currentValue < prevValue)
            {
                total -= currentValue;
            }
            else
            {
                total += currentValue;
            }
            prevValue = currentValue;
        }
        return total;
    }
}
