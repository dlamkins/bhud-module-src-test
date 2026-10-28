namespace DeimosOilSummary.Models
{
	public sealed class PlayerOilCount
	{
		public string CharacterName { get; set; }

		public string AccountName { get; set; }

		public int HitCount { get; set; }

		public string DisplayName => OilNameFormat.Display(CharacterName, AccountName);
	}
}
