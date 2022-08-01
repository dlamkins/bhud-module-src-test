namespace falcon.cmtracker.Persistance
{
	public class Token
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Icon { get; set; }

		public BossType bossType { get; set; }

		public SettingValue setting { get; set; }
	}
}
