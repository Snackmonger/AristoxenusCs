using Aristoxenus.Domain.Models;
namespace Aristoxenus.Domain.Services

{

    public class Relative
    {
        //public static Pitch PitchFrom(Pitch root, Interval interval)
        //{
        //    if (root.Value + interval.Value > Constants.MaxOctaveNumber * Constants.SemitonesInOctave)
        //    {

        //    }
        //}

        public static Interval IntervalFrom(Pitch pitch1, Pitch pitch2)
        {
            return pitch1.Frequency > pitch2.Frequency
                ? pitch2.Name.IntervalAbove(pitch1.Name)
                : pitch1.Name.IntervalAbove(pitch2.Name);
        }




    }
}
