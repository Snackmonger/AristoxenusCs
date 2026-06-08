

using Aristoxenus.Core.Domains.Theory.Models;

namespace Aristoxenus.Core.Shared.Data;

public static class RawIntervals
{
    public static readonly (int, int) DiminishedThird = (3, -2);
    public static readonly (int, int) MinorThird = (3, -1);
    public static readonly (int, int) MajorThird = (3, 0);
    public static readonly (int, int) AugmentedThird = (3, 1);
    public static readonly (int, int) PerfectFifth = (5, 0);
    public static readonly (int, int) DiminishedFifth = (5, -1);
    public static readonly (int, int) AugmentedFifth = (5, 1);
    public static readonly (int, int) MinorSeventh = (7, -1);
    public static readonly (int, int) DiminishedSeventh = (7, -2);
    public static readonly (int, int) MajorSeventh = (7, 0);
}
