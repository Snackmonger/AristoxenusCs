using Aristoxenus.Core.Bases;

namespace Aristoxenus.Core.Errors;

public class IntervalValueRepresentationMismatch(int value, int expectedValue, string intervalName) : 
    DomainException($"The interval value {value} is not consistent with the interval symbol '{intervalName}' (={expectedValue})");
