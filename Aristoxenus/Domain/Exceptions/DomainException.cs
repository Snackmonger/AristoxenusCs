
namespace Aristoxenus.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException() : base() { }

    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class IntervalStructureHasDuplicates(IEnumerable<string> intervals) : DomainException($"Interval structure cannot contain multiple intervals with the same value (the following elements were found multiple times {string.Join(", ", intervals)})");
public class AccidentalOutOfRange(int accidentals) : IntegerOutOfRange(0, 11, nameof(accidentals), accidentals);
public class InvalidAsSymbol(string value, string symbolType) : DomainException($"String '{value}' could not be recognized as a '{symbolType}'");
public class IntegerIntervalNotConsistentWithString(int value, int expectedValue, string intervalName) : DomainException($"Integer {value} is not an octave of the interval {intervalName} (={expectedValue})");
public class IntegerOutOfRange(int low, int high, string paramName, int value) : DomainException($"Integer {paramName} must be >= {low} and < {high} (got {value})");
