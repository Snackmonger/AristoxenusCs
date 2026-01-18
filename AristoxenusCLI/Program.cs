namespace AristoxenusCLI;
using Aristoxenus.Domain.Models;
using Aristoxenus.Constants;

class Program
{

    static int Main(string[] args)
    {
        IntervalStructure x = IntervalStructure.CreateNew(["1", "2", "3", "5", "9", "14"]);
        IntervalStructure y = IntervalStructure.FromChordSymbol(x.ToChordSymbol());
        Console.WriteLine(x.ToChordSymbol());
        Console.WriteLine(y.ToChordSymbol());
        Console.WriteLine(x.Equals(y));
        return 0;
    }
}