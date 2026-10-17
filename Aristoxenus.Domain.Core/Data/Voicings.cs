namespace Aristoxenus.Domain.Core.Data
{
    /// <summary>
    /// Collections of integers representing the transformation to apply to a 
    /// root position chord in order to derive their respecive voicings.
    /// </summary>
    public class LiftVoicings
    {
        public static readonly int[] Drop2 = [1];
        public static readonly int[] Drop2And4 = [1, 3];
        public static readonly int[] Drop2And3 = [2];
        public static readonly int[] Drop3 = [1, 2];
    }
}
