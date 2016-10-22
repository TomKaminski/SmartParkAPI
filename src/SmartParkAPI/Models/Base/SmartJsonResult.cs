using System.Collections.Generic;
using System.Linq;

namespace SmartParkAPI.Models.Base
{
    public class SmartJsonResult
    {
        public bool IsValid => ValidationErrors == null || !ValidationErrors.Any();

        protected SmartJsonResult()
        {

        }

        protected SmartJsonResult(IEnumerable<string> successNotifications, bool success)
        {
            SuccessNotifications = successNotifications;
        }

        protected SmartJsonResult(IEnumerable<string> validationErrors)
        {
            ValidationErrors = validationErrors;
        }

        public static SmartJsonResult Failure(params string[] validationErrors)
        {
            var errors = new List<string>();
            errors.AddRange(validationErrors);
            return new SmartJsonResult(errors);
        }

        public static SmartJsonResult Success(params string[] successNotifications)
        {
            var success = new List<string>();
            success.AddRange(successNotifications);
            return new SmartJsonResult(success, true);
        }

        public static SmartJsonResult Failure(IEnumerable<string> validationErrors)
        {
            return new SmartJsonResult(validationErrors);
        }

        public IEnumerable<string> SuccessNotifications { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; }
    }


    public class SmartJsonResult<T> : SmartJsonResult
    {
        protected SmartJsonResult(T result)
        {
            Result = result;
        }

        protected SmartJsonResult(IEnumerable<string> validationErrors) : base(validationErrors)
        {
        }

        protected SmartJsonResult(T result, IEnumerable<string> validationErrors) : base(validationErrors)
        {
            Result = result;
        }

        protected SmartJsonResult(T result, IEnumerable<string> successNotifications, bool success) : base(successNotifications, success)
        {
            Result = result;
        }

        public new static SmartJsonResult<T> Failure(params string[] validationErrors)
        {
            var errors = new List<string>();
            errors.AddRange(validationErrors);
            return new SmartJsonResult<T>(errors);
        }

        public static SmartJsonResult<T> Failure(IEnumerable<string> validationErrors, T result)
        {
            return new SmartJsonResult<T>(result, validationErrors);
        }

        public static SmartJsonResult<T> Failure(T result)
        {
            return new SmartJsonResult<T>(result);
        }

        public new static SmartJsonResult<T> Failure(IEnumerable<string> validationErrors)
        {
            return new SmartJsonResult<T>(validationErrors);
        }

        public static SmartJsonResult<T> Success(T result, params string[] successNotifications)
        {
            var success = new List<string>();
            success.AddRange(successNotifications);
            return new SmartJsonResult<T>(result, success, true);
        }

        public T Result { get; set; }
    }


    public class SmartJsonResult<T, T2> : SmartJsonResult<T>
    {

        protected SmartJsonResult(T result, T2 secondResult) : base(result)
        {
            SecondResult = secondResult;
        }

        protected SmartJsonResult(T result, T2 secondResult, IEnumerable<string> successNotifications, bool success) : base(result, successNotifications, success)
        {
            SecondResult = secondResult;
        }

        public static SmartJsonResult<T, T2> Success(T result, T2 secondResult, params string[] successNotifications)
        {
            var success = new List<string>();
            success.AddRange(successNotifications);
            return new SmartJsonResult<T, T2>(result, secondResult, success, true);
        }

        public T2 SecondResult { get; set; }
    }
}
