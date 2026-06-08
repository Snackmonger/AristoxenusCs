using Aristoxenus.Core.Shared.Bases;

namespace Aristoxenus.Core.Shared.Errors;

public class InvalidAsSymbol(string value, string symbolType) : 
    AristoxenusException($"String '{value}' could not be recognized as a '{symbolType}'");
