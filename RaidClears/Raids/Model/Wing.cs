using RaidClears.Raids.Controls;

namespace RaidClears.Raids.Model
{
	public class Wing
	{
		public string name;

		public int index;

		public string shortName;

		public Encounter[] encounters;

		private WingPanel _wingPanel;

		public Wing(string name, int index, string shortName, Encounter[] encounters)
		{
			this.name = name;
			this.index = index;
			this.shortName = shortName;
			this.encounters = encounters;
		}

		public void SetWingPanelReference(WingPanel panel)
		{
			_wingPanel = panel;
		}

		public WingPanel GetWingPanelReference()
		{
			return _wingPanel;
		}
	}
}
