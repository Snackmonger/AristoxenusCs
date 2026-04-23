namespace Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;

public class IntegerIntervalNotConsistentWithString(int value, int expectedValue, string intervalName) : 
    DomainException($"Integer {value} is not an octave of the interval {intervalName} (={expectedValue})");
