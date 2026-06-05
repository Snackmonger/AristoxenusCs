namespace Aristoxenus.Core.Shared.Errors;
using Aristoxenus.Core.Shared.Bases;


public class InvalidAsSymbol(string value, string symbolType) : 
    DomainException($"String '{value}' could not be recognized as a '{symbolType}'");
