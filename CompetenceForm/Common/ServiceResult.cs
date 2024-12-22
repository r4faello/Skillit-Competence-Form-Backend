namespace CompetenceForm.Common
{
    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        protected ServiceResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static ServiceResult Success()
        {
            return new ServiceResult(true, string.Empty);
        }

        public static ServiceResult Failure(string message)
        {
            return new ServiceResult(false, message);
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; }

        private ServiceResult(bool isSuccess, string message, T? data = default)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>(true, string.Empty, data);
        }

        public static ServiceResult<T> Failure(string message)
        {
            return new ServiceResult<T>(false, message);
        }
    }



}
