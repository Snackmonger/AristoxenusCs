using Aristoxenus.Core.Shared.Bases;

namespace Aristoxenus.Core.Domains.Theory.Models;

public class Enclosure
{
    public readonly string Label;
    public readonly List<int> Pattern;

    private Enclosure(string label, List<int> pattern)
    {
        Label = label;
        Pattern = pattern;
    }

    public Enclosure CreateNew(string label, List<int> pattern)
    {
        return new(label, pattern);
    }

    public List<Pitch> Apply(Pitch target)
    {
        throw new NotImplementedException();
    }
}

