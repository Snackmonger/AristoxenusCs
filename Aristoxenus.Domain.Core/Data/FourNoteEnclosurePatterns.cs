namespace Aristoxenus.Domain.Core.Data;






public static class FourNoteEnclosurePatterns
{
    // -2, -1, 0, 1, 2
    //  A   B     C  D

    public static readonly int[] ABCB = [-2, -1, 1, -1];
    public static readonly int[] ABDB = [-2, -1, 2, -1];
    public static readonly int[] ABDC = [-2, -1, 2, 1];
    public static readonly int[] ADCB = [-2, 2, 1, -1];
    public static readonly int[] ADBA = [-2, 2, 1, -2];
    public static readonly int[] ADAB = [-2, 2, -2, -1];

    public static readonly int[] BDCB = [-1, 2, 1, -1];
    public static readonly int[] CABC = [1, -2, -1, 1];

    public static readonly int[] DABD = [2, -2, -1, 2];
    public static readonly int[] DABC = [2, -2, -1, 1];
    public static readonly int[] DADC = [2, -2, 2, 1];
    public static readonly int[] DBDC = [2, -1, 2, 1];
    public static readonly int[] DCAB = [2, 1, -2, -1];
    public static readonly int[] DCAC = [2, 1, -2, 1];
    public static readonly int[] DCBC = [2, 1, -1, 1];
}



