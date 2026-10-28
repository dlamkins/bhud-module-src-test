namespace DeimosOilSummary.Models
{
	public sealed class OilFirstTouch
	{
		public int OilNumber { get; set; }

		public string CharacterName { get; set; }

		public string AccountName { get; set; }

		public long TimeMs { get; set; }

		public string DisplayName => OilNameFormat.Display(CharacterName, AccountName);
	}
}
