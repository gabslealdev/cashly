namespace Cashly.Application.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public ApplicationError Error { get; private set; } = null!;

        protected Result(bool isSuccess, ApplicationError error)
        {
            if (isSuccess && error != ApplicationError.None)
                throw new InvalidOperationException("A successful result cannot have an error.");

            if (!isSuccess && error == ApplicationError.None)
                throw new InvalidOperationException("A failed result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
            => new(true, ApplicationError.None);

        public static Result Failure(ApplicationError error)
            => new(false, error);
    }

    public sealed class Result<T> : Result
    {
        private readonly T? _value;

        private Result(T value) : base(true, ApplicationError.None)
            => _value = value;

        private Result(ApplicationError error) : base(false, error)
            => _value = default;

        public T Value
            => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access the value of a failed result.");

        public static Result<T> Success(T value)
            => new(value);

        public new static Result<T> Failure(ApplicationError error)
            => new(error);
    }
}
