namespace rp.spark.Models.Api
{
	public class RollGroupSettingsRequest
	{
		public bool JoinLocked { get; set; }

		public string NewPassword { get; set; } = string.Empty;


		public bool ClearPassword { get; set; }
	}
}
