namespace Aristoxenus.Language.Errors.Runtime;

public class UnknownIdentifier : Exception
{
    public readonly string Name;

    public UnknownIdentifier(string name) : base($"Undefined variable '{name}'")
    {
        Name = name;
    }
}