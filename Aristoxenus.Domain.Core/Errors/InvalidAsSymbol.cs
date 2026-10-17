using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Errors;

public class InvalidAsSymbol(string value, string symbolType) : 
    AristoxenusException($"String '{value}' could not be recognized as a '{symbolType}'");
