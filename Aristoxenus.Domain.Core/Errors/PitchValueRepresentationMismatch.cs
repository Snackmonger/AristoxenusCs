using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Bases;
using Aristoxenus.Domain.Core.Models;

namespace Aristoxenus.Domain.Core.Errors;

public class PitchValueRepresentationMismatch(NoteName noteName, int expectedValue)
    : DomainException($"Note name {noteName.ToSymbol()} suggests a value of {noteName.AbsoluteValue} + (n * 12), " +
        $"but the given expected value is {expectedValue}, which is " +
        $"{expectedValue % Numbers.SemitonesInOctave} + ({expectedValue / Numbers.SemitonesInOctave} * 12)");