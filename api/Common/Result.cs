namespace api.Common
{
    public enum ResultError
    {
        None = 0,
        Unauthorized,
        Forbidden,
        NotFound,
        Validation,
        Conflict
    }

    public class Result
    {
        public bool Ok { get; }
        public ResultError Error { get; }
        public string? Message { get; }

        protected Result(bool ok, ResultError error, string? message)
        {
            Ok = ok;
            Error = error;
            Message = message;
        }

        public static Result Success() => new(true, ResultError.None, null);

        public static Result Fail(ResultError error, string? message = null) =>
            new(false, error, message);
    }

    public sealed class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool ok, ResultError error, string? message, T? value)
            : base(ok, error, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(true, ResultError.None, null, value);

        public static new Result<T> Fail(ResultError error, string? message = null) =>
            new(false, error, message, default);

        public static Result<T> Fail(Result failure) =>
            new(false, failure.Error, failure.Message, default);
    }
}
