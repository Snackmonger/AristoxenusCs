namespace Aristoxenus.Core.Errors;

using Aristoxenus.Core.Bases;


public class IntervalStructureHasDuplicates(IEnumerable<string> intervals) : 
    DomainException($"Interval structure cannot contain multiple intervals with the same value (the following elements were found multiple times: {string.Join(", ", intervals)})");
