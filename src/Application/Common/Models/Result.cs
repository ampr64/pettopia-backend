using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Models
{
    public class Result<T>
    {
        [MemberNotNullWhen(true, nameof(Data))]
        public bool Succeeded => Errors is { Count: 0 };

        public IReadOnlyList<string> Errors { get; } = new List<string>();

        public T? Data { get; }

        private Result(IEnumerable<string> errors, T? data)
        {
            Errors = errors.ToList();
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            return new Result<T>(Enumerable.Empty<string>(), data);
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(errors, default);
        }
    }
}