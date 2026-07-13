using Aristoxenus.Core.Bases;

namespace Aristoxenus.Core.Errors;

public class InvalidAsSymbol(string value, string symbolType) : 
    AristoxenusException($"String '{value}' could not be recognized as a '{symbolType}'");
