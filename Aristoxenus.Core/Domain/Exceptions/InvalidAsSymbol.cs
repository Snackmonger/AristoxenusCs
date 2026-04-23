namespace Aristoxenus.Core.Domain.Exceptions;
using Aristoxenus.Core.SharedKernel.Bases;


public class InvalidAsSymbol(string value, string symbolType) : 
    DomainException($"String '{value}' could not be recognized as a '{symbolType}'");
