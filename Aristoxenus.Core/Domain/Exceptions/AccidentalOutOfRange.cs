namespace Aristoxenus.Core.Domain.Exceptions;

public class AccidentalOutOfRange(int accidentals) : 
    IntegerOutOfRange(0, 11, nameof(accidentals), accidentals);
