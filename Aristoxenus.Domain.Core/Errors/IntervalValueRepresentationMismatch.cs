using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Errors;

public class IntervalValueRepresentationMismatch(int value, int expectedValue, string intervalName) : 
    DomainException($"The interval value {value} is not consistent with the interval symbol '{intervalName}' (={expectedValue})");
