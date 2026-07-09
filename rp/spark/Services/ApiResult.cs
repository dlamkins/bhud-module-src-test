using System.Net;

namespace rp.spark.Services
{
	public class ApiResult<T>
	{
		public bool Succeeded { get; }

		public T Value { get; }

		public HttpStatusCode? StatusCode { get; }

		public string ErrorMessage { get; }

		public ApiFailure FailureKind { get; }

		private ApiResult(bool succeeded, T value, HttpStatusCode? statusCode, string errorMessage, ApiFailure failureKind)
		{
			Succeeded = succeeded;
			Value = value;
			StatusCode = statusCode;
			ErrorMessage = errorMessage ?? string.Empty;
			FailureKind = failureKind;
		}

		public static ApiResult<T> Success(T value, HttpStatusCode? statusCode = null)
		{
			return new ApiResult<T>(succeeded: true, value, statusCode, string.Empty, ApiFailure.None);
		}

		public static ApiResult<T> Failure(string errorMessage, HttpStatusCode? statusCode = null, ApiFailure failureKind = ApiFailure.None)
		{
			return new ApiResult<T>(succeeded: false, default(T), statusCode, errorMessage, failureKind);
		}
	}
}
