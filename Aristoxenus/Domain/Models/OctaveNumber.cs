using Aristoxenus.Constants;
using Aristoxenus.SharedKernel.Bases;
using Aristoxenus.SharedKernel.Interfaces;

namespace Aristoxenus.Domain.Models
{
    /// <summary>
    /// Representation of a region of tonal space, as divided into octaves.
    /// </summary>
    public class OctaveNumber : ValueObject, ISymbolSerializable<OctaveNumber>
    {
        public readonly int Value;
        private OctaveNumber(int octaveNumber)
        {
            Value = octaveNumber;
        }

        public string Symbol => Value.ToString();

        public static OctaveNumber CreateNew(int octaveNumber)
        {
            if (octaveNumber is >= Numbers.MinOctaveNumber and < Numbers.MaxOctaveNumber)
            {
                return new OctaveNumber(octaveNumber);
            }
            throw new ArgumentException("Octave number must be between 1 and 8");
        }

        public static OctaveNumber FromSymbol(string octaveNumber)
        {
            int v = int.Parse(octaveNumber);
            return CreateNew(v);

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
