using System.Collections;
using Aristoxenus.Domain.Core.Interfaces;

namespace Aristoxenus.Domain.Core.Models
{
    public class Melody() : ICollection<IHasDuration>
    {
        private readonly List<IHasDuration> _durationHavingObjects = [];
        public Duration DefaultDuration = Duration.Quarter;
        public double CurrentTime => _durationHavingObjects.Sum((note) => note.Duration.Value.ToDouble());

        public int Count => _durationHavingObjects.Count;

        public bool IsReadOnly => false;

        public Melody WithDefaultDuration(Duration duration)
        {
            DefaultDuration = duration;
            return this;
        }

        public IHasDuration? CheckIntersectTime(double time)
        {
            double currentTime = 0;
            foreach (var note in _durationHavingObjects)
            {
                currentTime += note.Duration.Value.ToDouble();
                if (currentTime >= time)
                {
                    return note;
                }
            }
            return null;
        }

        public IHasDuration? CheckBeginsAtTime(double time)
        {
            for (int i = 0; i < _durationHavingObjects.Count; i++)
            {
                double currentTime = _durationHavingObjects.Take(i).Sum((note) => note.Duration.Value.ToDouble());
                if (currentTime == time)
                {
                    return _durationHavingObjects[i];
                }
            }
            return null;
        }

        public bool TrySubdivideAtTime(double time, int subdivisions)
        {
            IHasDuration? hasDuration = CheckIntersectTime(time);
            if (hasDuration == null)
            {
                return false;
            }
            IEnumerable<IHasDuration> subdividedNotes = hasDuration.SubdivideEqully(subdivisions);
            int index = _durationHavingObjects.IndexOf(hasDuration);
            _durationHavingObjects.RemoveAt(index);
            _durationHavingObjects.InsertRange(index, subdividedNotes);
            return true;
        }

        public bool TryExtendAtTime(double time, int multiplier)
        {
            IHasDuration? hasDuration = CheckBeginsAtTime(time);
            if (hasDuration == null)
            {
                return false;
            }
            IHasDuration extendedNote = hasDuration.Extend(multiplier);
            int index = _durationHavingObjects.IndexOf(hasDuration);
            _durationHavingObjects[index] = extendedNote;
            return true;
        }


        public void Add(IEnumerable<Pitch> pitches, Duration duration)
        {
            foreach (var pitch in pitches)
            {
                var note = new Note(pitch, duration);
                _durationHavingObjects.Add(note);
            }
        }

        public void Add(IEnumerable<Pitch> pitches)
        {
            foreach (var pitch in pitches)
            {
                var note = new Note(pitch, DefaultDuration);
                _durationHavingObjects.Add(note);
            }
        }

        public void Add(Pitch pitch)
        {
            var note = new Note(pitch, DefaultDuration);
            _durationHavingObjects.Add(note);
        }

        public void Add(Duration rest)
        {
            var restNote = new Rest(rest);
            _durationHavingObjects.Add(restNote);
        }

        public void Add(Pitch pitch, Duration duration)
        {
            var note = new Note(pitch, duration);
            _durationHavingObjects.Add(note);
        }


        public void Add(IHasDuration note)
        {
            _durationHavingObjects.Add(note);
        }

        public Melody With(IEnumerable<Pitch> pitches)
        {
            Add(pitches);
            return this;
        }

        public Melody With(IEnumerable<Pitch> pitches, Duration duration)
        {
            Add(pitches, duration);
            return this;
        }

        public Melody With(Pitch pitch)
        {
            Add(pitch, DefaultDuration);
            return this;
        }

        public Melody With(Duration rest)
        {
            Add(rest);
            return this;
        }

        public Melody With(IHasDuration note)
        {
            Add(note);
            return this;
        }

        public void Clear()
        {
            _durationHavingObjects.Clear();
        }

        public bool Contains(IHasDuration item)
        {
            return _durationHavingObjects.Contains(item);
        }

        public void CopyTo(IHasDuration[] array, int arrayIndex)
        {
            _durationHavingObjects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IHasDuration> GetEnumerator()
        {
            return _durationHavingObjects.GetEnumerator();
        }

        public bool Remove(IHasDuration item)
        {
            return _durationHavingObjects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
