namespace Exchangerat.Services.Models.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Result
    {
        private readonly List<string> errors;

        internal Result(bool succeeded, List<string> errors)
        {
            this.Succeeded = succeeded;
            this.errors = errors;
        }

        public bool Succeeded { get; }

        public List<string> Errors
            => this.Succeeded
                ? new List<string>()
                : this.errors;

        public static Result Success
            => new Result(true, new List<string>());

        public static Result Failure(IEnumerable<string> errors)
            => new Result(false, errors.ToList());

        public static Result Failure(string error) 
            => Failure(new List<string>() { error });
    }

    public class Result<TData> : Result
    {
        private readonly TData data;

        private Result(bool succeeded, TData data, List<string> errors) 
            : base(succeeded, errors)
        {
            this.data = data;
        }

        private Result(bool succeeded, List<string> errors)
            :base(succeeded, errors) { }

        public TData Data
            => this.Succeeded
                ? this.data
                : throw new InvalidOperationException(
                    $"{nameof(this.Data)} is not available with failed result. Use {this.Errors} instead.");


        public static Result<TData> SuccessWith(TData data)
            => new Result<TData>(true, data, new List<string>());

        public new static Result<TData> Failure(string error)
            => Result<TData>.Failure(new List<string>() {error});

        public new static Result<TData> Failure(IEnumerable<string> errors)
            => new Result<TData>(false, errors.ToList());
    }
}
