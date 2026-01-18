namespace Aristoxenus.Domain.Models
{
    public class FretboardDiagram
    {
        public readonly int NumberOfStrings;
        public readonly int NumberOfFrets;
        public readonly int? FirstFret;
        public List<FretboardNote> Notes { get; private set; }
    }
}
