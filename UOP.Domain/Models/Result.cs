using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOP.Domain.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }
        public List<string>? Errors { get; private set; } = [];

        private Result(bool isSuccess, T value, List<string>? errors)
        {
            IsSuccess = isSuccess;
            Value = value;
            Errors = errors ?? [];
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(params string[] errors) => new(false, default, [.. errors]);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public List<string> Errors { get; }

        protected Result(bool isSuccess, List<string>? errors)
        {
            if (isSuccess && errors is not null && errors.Count is not 0) throw new InvalidOperationException();
            if (!isSuccess && (errors is null || errors.Count == 0)) throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Errors = errors ?? [];
        }

        public static Result Success() => new(true, null);
        public static Result Failure(List<string> errors) => new(false, errors);
        public static Result Failure(params string[] errors) => new(false, [.. errors]);
    }
}
