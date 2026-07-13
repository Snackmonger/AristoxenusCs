using Aristoxenus.Core.Bases;

namespace Aristoxenus.Core.Errors;

public class InvalidAsFrequency(string Message) : DomainException(Message);
