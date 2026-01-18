
namespace Aristoxenus.Utilities
{

    internal class IntervalSortHelper
    {
        public static List<string> SortIntervalNames(IEnumerable<string> intervals)
        {
            var dict = new Dictionary<string, double> {
            { "1", 0 },
            { "b2", 1 },
            { "2", 2 },
            { "bb3", 3 },
            { "#2", 4 },
            { "b3", 5 },
            { "3", 6 },
            { "b4", 7 },
            { "#3", 8 },
            { "4", 9},
            { "##3", 9.5},
            { "bb5", 10},
            { "#4", 11},
            { "b5", 12 },
            { "5", 13 },
            { "#5", 14 },
            { "b6", 15 },
            { "##5", 16 },
            { "6", 17 },
            { "bb7", 18 },
            { "#6", 19},
            { "b7", 20 },
            { "7", 21 },
            { "8", 21.5 },
            { "b9", 22 },
            { "9", 23 },
            { "bb10", 23.1 },
            { "#9", 24 },
            { "b10", 24.1 },
            { "10", 24.5 },
            { "#10", 24.6 },
            { "b11", 24.7 },
            { "##10", 24.8 },
            { "11", 25 },
            { "bb12", 25.1},
            { "#11", 26},
            { "b12", 26.1 },
            { "12", 26.2 },
            { "#12", 26.3 },
            { "b13", 27},
            { "##12", 27.1 },
            { "13", 28 },
            { "bb14", 29 },
            { "#13", 30},
            { "b14", 31 },
            { "14", 32 },
            { "15", 33 }
        };

            var i = intervals.ToList();
            i.Sort((x, y) => dict[x].CompareTo(dict[y]));
            return [.. i];
        }

    }
}
