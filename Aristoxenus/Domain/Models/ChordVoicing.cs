using Aristoxenus.SharedKernel.Bases;

namespace Aristoxenus.Domain.Models;

public class ChordVoicing : ValueObject
{
    public readonly List<int> Derivation;
    public readonly int Inversion;

    private ChordVoicing(List<int> derivation, int inversion)
    {
        Derivation = derivation;
        Inversion = inversion;
    }

    public ChordVoicing CreateNew(IEnumerable<int> derivation, int inversion)
    {
        if (inversion < 0)
        {
            throw new Exception();
        }
        return new ChordVoicing([.. Derivation], inversion);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (int d in Derivation)
        {
            yield return d;
        }
        yield return Inversion;
    }
}
