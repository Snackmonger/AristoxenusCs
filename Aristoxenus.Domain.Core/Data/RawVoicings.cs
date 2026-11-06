namespace Aristoxenus.Domain.Core.Data
{
    /// <summary>
    /// Collections of integers representing the transformation to apply to a 
    /// root position chord in order to derive their respecive voicings.
    /// </summary>
    public class RawVoicings
    {
        // Although these are called 'drop' voicings, the numbers represent
        // the indices of notes that will be displaced *upwards*, which we do
        // because it makes it easier to find the right voicing for each inversion.
        // Invert, then apply the voicing. 
        public static readonly int[] Drop2 = [1]; // 1357 -> 1573 : c e g b -> c g b e
        public static readonly int[] Drop2And4 = [1, 3]; // 1357 -> 1537 : c e g b -> c g e b
        public static readonly int[] Drop2And3 = [2]; // 1357 -> 1375 : c e g b -> c e b g
        public static readonly int[] Drop3 = [1, 2]; // 1357 -> 1735 : c e g b -> c b e g
        public static readonly int[] SpreadTriad = Drop2; // 135 -> 153  : c e g  -> c g e
    }
}




