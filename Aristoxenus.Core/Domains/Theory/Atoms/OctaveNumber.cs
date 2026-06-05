using Aristoxenus.Core.Shared.Bases;
using Aristoxenus.Core.Shared.Interfaces;
using Aristoxenus.Core.Shared.Data;

namespace Aristoxenus.Core.Domains.Theory.Atoms
{
    /// <summary>
    /// Representation of a region of tonal space, as divided into octaves.
    /// </summary>
    public class OctaveNumber : ValueObject, ISymbolSerializable<OctaveNumber>
    {
        public readonly int Value;


        // Constructors, Factories ////////////////////////////////////////////
        public OctaveNumber(int octaveNumber)
        {
            if (octaveNumber is >= Numbers.MinOctaveNumber and < Numbers.MaxOctaveNumber)
                Value = octaveNumber;
            else 
                throw new ArgumentException("Octave number must be between 1 and 8");
        }

        // ISymbolSerializable

        public string ToSymbol() => Value.ToString();

        public static OctaveNumber FromSymbol(string octaveNumber)
        {
            int v = int.Parse(octaveNumber);
            return new(v);

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
