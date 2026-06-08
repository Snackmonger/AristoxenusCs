namespace Aristoxenus.Core.Shared.Types
{
    public class Result<E, T>
    {
        private T? _value;
        private E? _error;
        public bool IsSuccess { get; }

        public T Value
        {
            get => IsSuccess ? _value! : throw new InvalidOperationException("Result is not successful");
            private set => _value = value;
        }

        public E Error
        {
            get => !IsSuccess ? _error! : throw new InvalidOperationException("Result is successful");
            private set => _error = value;
        }

        private Result(bool isSuccess, T? value, E? error) =>
            (IsSuccess, _value, _error) = (isSuccess, value, error);

        public static Result<E, T> Success(T value)
            => new(true, value, default);
        public static Result<E, T> Failure(E error)
            => new(false, default, error);

        public Result<E, T> Map(Func<T, T> map)
        {
            if (IsSuccess)
            {
                return Success(map(Value));
            }
            return this;
        }

        public Result<E, T> MapError(Func<E, E> map)
        {
            if (!IsSuccess)
            {
                return Failure(map(Error));
            }
            return this;
        }

        public void Match(Action<T> onSuccess, Action<E> onFailure)
        {
            if (IsSuccess)
                onSuccess(Value);
            else
                onFailure(Error);
        }
    }
}
