using Aristoxenus.Core.Bases;
using Aristoxenus.Core.Data;
using Aristoxenus.Core.Models;

namespace Aristoxenus.Core.Errors;

public class PitchValueRepresentationMismatch(NoteName noteName, int expectedValue)
    : DomainException($"Note name {noteName.ToSymbol()} suggests a value of {noteName.AbsoluteValue} + (n * 12), " +
        $"but the given expected value is {expectedValue}, which is " +
        $"{expectedValue % Numbers.SemitonesInOctave} + ({expectedValue / Numbers.SemitonesInOctave} * 12)");