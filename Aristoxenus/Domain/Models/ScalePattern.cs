using Aristoxenus.Constants;


namespace Aristoxenus.Domain.Models
{
    public record ScalePattern
    {
        private IEnumerable<int> Value;

        private ScalePattern(IEnumerable<int> value)
        {
            Value = value;
        }

        public static ScalePattern Diatonic => new ScalePattern(RawScalePatterns.Diatonic);
        public static ScalePattern Altered => new ScalePattern(RawScalePatterns.Altered);
    }
}
