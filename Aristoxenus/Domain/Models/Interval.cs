
using Aristoxenus.Constants;
using Aristoxenus.Domain.Exceptions;
using Aristoxenus.SharedKernel.Bases;

namespace Aristoxenus.Domain.Models
{
    public class Interval : ValueObject
    {
        public readonly IntervalName Name;
        public readonly int Value;

        private Interval(IntervalName name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// The raw value of the interval degree.
        /// </summary>
        public int Degree 
            => Name.Degree.Value;

        /// <summary>
        /// The raw value of the interval accidental.
        /// </summary>
        public int Accidental 
            => Name.Accidental.Value;

        /// <summary>
        /// The raw value of the interval, if it were forced into the first
        /// octave. If the interval is already within a single octave,
        /// </summary>
        public int SingleOctaveValue
            => Name.SingleOctaveValue;

        /// <summary>
        /// The raw value of the interval, if it were forced into the second
        /// octave.
        /// </summary>
        public int DoubleOctaveValue
            => Name.DoubleOctaveValue;

        /// <summary>
        /// The quality of the interval, expressed as a formal
        /// name, e.g. "perfect", "augmented", etc.
        /// </summary>
        public string Quality
        {
            get
            {
#pragma warning disable CS8509
                // Not all paths covered, but we know that our
                // system will not generate other paths.
                string quality = Name.Degree.Value switch
                {
                    1 => Value switch
                    {
                        0 => Keywords.Perfect,
                        1 => Keywords.Augmented,
                        -1 => Keywords.Diminished,
                    },
                    2 => Value switch
                    {
                        1 => Keywords.Diminished,
                        2 => Keywords.Major,
                        3 => Keywords.Augmented,
                        4 => string.Join(Symbols.Whitespace, [Keywords.Double, Keywords.Augmented])
                    },
                    3 => Value switch
                    {
                        2 => Keywords.Diminished,
                        3 => Keywords.Minor,
                        4 => Keywords.Major,
                        5 => Keywords.Augmented,
                    },
                    4 => Value switch
                    {
                        3 => string.Join(Symbols.Whitespace, [Keywords.Double, Keywords.Diminished]),
                        4 => Keywords.Diminished,
                        5 => Keywords.Perfect,
                        6 => Keywords.Augmented,
                        7 => string.Join(Symbols.Whitespace, [Keywords.Double, Keywords.Augmented])
                    },
                    5 => Value switch
                    {
                        5 => string.Join(Symbols.Whitespace, [Keywords.Double, Keywords.Diminished]),
                        6 => Keywords.Diminished,
                        7 => Keywords.Perfect,
                        8 => Keywords.Augmented,
                        9 => string.Join(Symbols.Whitespace, [Keywords.Double, Keywords.Augmented])
                    },
                    6 => Value switch
                    {
                        7 => Keywords.Diminished,
                        8 => Keywords.Minor,
                        9 => Keywords.Major,
                        10 => Keywords.Augmented,
                    },
                    7 => Value switch
                    {
                        9 => Keywords.Diminished,
                        10 => Keywords.Minor,
                        11 => Keywords.Major,
                    }
                };
#pragma warning restore CS8509
                return quality ?? throw new InvalidOperationException(
                    $"Unrecognized interval quality: name={Name.Degree.Value}, value={Value}"
                );
            }
        }
        /// <summary>
        /// Create an interval from a name and a value, which may be a 
        /// multiple of an octave different from the name's implied value.
        /// </summary>
        public static Interval CreateNew(IntervalName name, int value)
        {
            if (name.SingleOctaveValue != (value % Numbers.SemitonesInOctave))
            {
                throw new IntegerIntervalNotConsistentWithString(value, name.Value, name.Symbol);
            }
            return new Interval(name, value);
        }


        /// <summary>
        /// Create an interval from raw primitives for the name, with an 
        /// inferred value.
        /// </summary>
        public static Interval CreateNew(int degree, int accidental)
        {
            var name = IntervalName.CreateNew(degree, accidental);
            return CreateNew(name, name.Value);
        }

        /// <summary>
        /// Create an interval from an interval name, with an inferred value.
        /// </summary>
        public static Interval CreateNew(IntervalName name)
        {
            return CreateNew(name, name.Value);
        }

        /// <summary>
        /// Create an interval that will have the given degree, plus as many
        /// accidentals are necessary to reach the given value (limited to
        /// two octaves).
        /// </summary>
        public static Interval FromExpectedValue(int degree, int value)
        {
            var name = IntervalName.FromExpectedValue(degree, value);
            return CreateNew(name, name.Value);
        }

        /// <summary>
        /// Create an interval from the given interval name symbol, with 
        /// an inferred value.
        /// </summary>
        public static Interval FromSymbol(string intervalSymbol)
        {
            var name = IntervalName.FromSymbol(intervalSymbol);
            return CreateNew(name, name.Value);

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name.Symbol;
            yield return Value;
        }
    }
}
