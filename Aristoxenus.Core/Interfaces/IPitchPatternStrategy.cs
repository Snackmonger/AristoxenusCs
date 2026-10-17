using Aristoxenus.Core.Models;

namespace Aristoxenus.Core.Interfaces;

/// <summary>
/// A strategy for generating a sequence of pitches based around a
/// target pitch that will be provided at runtime.
/// </summary>
public interface IPitchPatternStrategy
{
    /// <summary>
    /// Return a list of <see cref="Pitch"/> objects representing a sequence
    /// relative to the given <paramref name="targetPitch"/>.
    /// </summary>
    List<Pitch> Execute(Pitch targetPitch);
}



