using Aristoxenus.Core.Constants;
using Aristoxenus.Core.SharedKernel.Bases;
using Aristoxenus.Core.SharedKernel.Interfaces;

namespace Aristoxenus.Core.Domain.Theory
{
    /// <summary>
    /// Representation of a region of tonal space, as divided into octaves.
    /// </summary>
    public class OctaveNumber : ValueObject, ISymbolSerializable<OctaveNumber>
    {
        public readonly int Value;


        // Constructors, Factories ////////////////////////////////////////////
        private OctaveNumber(int octaveNumber)
        {
            Value = octaveNumber;
        }

        public static OctaveNumber CreateNew(int octaveNumber)
        {
            if (octaveNumber is >= Numbers.MinOctaveNumber and < Numbers.MaxOctaveNumber)
            {
                return new OctaveNumber(octaveNumber);
            }
            throw new ArgumentException("Octave number must be between 1 and 8");
        }

        // ISymbolSerializable

        public string ToSymbol() => Value.ToString();

        public static OctaveNumber FromSymbol(string octaveNumber)
        {
            int v = int.Parse(octaveNumber);
            return CreateNew(v);

        }

        static OctaveNumber ISymbolSerializable<OctaveNumber>.FromSymbol(string symbol)
            => FromSymbol(symbol);
        string ISymbolSerializable<OctaveNumber>.ToSymbol()
            => ToSymbol();

        // ValueObject

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
