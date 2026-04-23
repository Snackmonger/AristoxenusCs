using Aristoxenus.Core.SharedKernel.Bases;

namespace Aristoxenus.Core.Domain.Exceptions;

public class IntegerOutOfRange(int low, int high, string paramName, int value) : 
    DomainException($"Integer {paramName} must be >= {low} and < {high} (got {value})");
