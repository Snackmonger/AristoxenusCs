using Aristoxenus.Core.Bases;

namespace Aristoxenus.Core.Errors;

public class IntervalQualityUnrecognized(int Degree, int Value) : DomainException($"Unrecognized interval quality: degree={Degree}, value={Value}");
