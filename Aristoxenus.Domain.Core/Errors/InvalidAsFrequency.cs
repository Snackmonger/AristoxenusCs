using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Errors;

public class InvalidAsFrequency(string Message) : DomainException(Message);
