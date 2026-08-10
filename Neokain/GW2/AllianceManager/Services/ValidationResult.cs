namespace Neokain.GW2.AllianceManager.Services
{
	public class ValidationResult
	{
		public ValidationStatus Status { get; set; }

		public string Message { get; set; }

		public int MinLength { get; set; }

		public int MaxLength { get; set; }
	}
}
