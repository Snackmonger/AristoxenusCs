namespace Aristoxenus.Core.Shared.Bases;

public class AristoxenusException : Exception
{
    public AristoxenusException() : base() { }

    public AristoxenusException(string message) : base(message) { }

    public AristoxenusException(string message, Exception innerException) : base(message, innerException) { }
}
