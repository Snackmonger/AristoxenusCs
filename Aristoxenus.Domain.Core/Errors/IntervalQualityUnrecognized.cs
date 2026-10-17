using Aristoxenus.Domain.Core.Bases;

namespace Aristoxenus.Domain.Core.Errors;

public class IntervalQualityUnrecognized(int Degree, int Value) : DomainException($"Unrecognized interval quality: degree={Degree}, value={Value}");
