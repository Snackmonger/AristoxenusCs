namespace Aristoxenus.Domain.Core.Bases;

public abstract class DomainException : AristoxenusException
{
    protected DomainException()
    {
    }

    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
