using System.Collections.Generic;
using System.Linq;

namespace SmartParkAPI.Contracts.Common
{
    public class ServiceResult
    {
        public bool IsValid => ValidationErrors == null || !ValidationErrors.Any();

        protected ServiceResult()
        {
            ValidationErrors = new List<string>();
        }
        protected ServiceResult(List<string> data, bool success = false)
        {
            if (success)
            {
                SuccessNotifications = data;
            }
            else
            {
                ValidationErrors = data;
            }
            
        }

        public static ServiceResult Success(params string[] successNotifications)
        {
            var success = new List<string>();
            success.AddRange(successNotifications);
            return new ServiceResult(success, true);
        }

        public static ServiceResult Failure(params string[] validationErrors)
        {
            var errors = new List<string>();
            errors.AddRange(validationErrors);
            return new ServiceResult(errors, false);
        }

        public static ServiceResult Failure(List<string> validationErrors)
        {
            return new ServiceResult(validationErrors, false);
        }

        public List<string> ValidationErrors { get; set; }
        public List<string> SuccessNotifications { get; set; }
    }


    public class ServiceResult<T> : ServiceResult
    {
        protected ServiceResult(T result)
        {
            Result = result;
            ValidationErrors = new List<string>();
        }

        protected ServiceResult(List<string> validationErrors) : base(validationErrors)
        {

        }

        public new static ServiceResult<T> Failure(params string[] validationErrors)
        {
            var errors = new List<string>();
            errors.AddRange(validationErrors);
            return new ServiceResult<T>(errors);
        }

        public new static ServiceResult<T> Failure(List<string> validationErrors)
        {
            return new ServiceResult<T>(validationErrors);
        }

        public static ServiceResult<T> Success(T result)
        {
            return new ServiceResult<T>(result);
        }

        public T Result { get; set; }
    }


    public class ServiceResult<T, T2> : ServiceResult<T>
    {

        protected ServiceResult(T result, T2 secondResult) : base(result)
        {
            SecondResult = secondResult;
            ValidationErrors = new List<string>();
        }

        protected ServiceResult(List<string> validationErrors) : base(validationErrors)
        {

        }

        public static ServiceResult<T, T2> Success(T result, T2 secondResult)
        {
            return new ServiceResult<T, T2>(result, secondResult);
        }

        public new static ServiceResult<T, T2> Failure(params string[] validationErrors)
        {
            var errors = new List<string>();
            errors.AddRange(validationErrors);
            return new ServiceResult<T, T2>(errors);
        }

        public new static ServiceResult<T, T2> Failure(List<string> validationErrors)
        {
            return new ServiceResult<T, T2>(validationErrors);
        }


        public T2 SecondResult { get; set; }
    }
}
