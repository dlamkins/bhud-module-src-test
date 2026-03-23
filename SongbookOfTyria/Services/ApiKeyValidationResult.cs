namespace SongbookOfTyria.Services
{
	public class ApiKeyValidationResult
	{
		public bool IsValid { get; }

		public string ErrorMessage { get; }

		public ApiKeyValidationResult(bool isValid, string errorMessage)
		{
			IsValid = isValid;
			ErrorMessage = errorMessage;
		}
	}
}
