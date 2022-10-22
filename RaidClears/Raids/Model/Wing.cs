using RaidClears.Raids.Controls;

namespace RaidClears.Raids.Model
{
	public class Wing
	{
		public string name;

		public int index;

		public string shortName;

		public Encounter[] encounters;

		public bool isEmboldened;

		public bool isCallOfTheMist;

		private WingPanel _wingPanel;

		public Wing(string name, int index, string shortName, Encounter[] encounters)
		{
			this.name = name;
			this.index = index;
			this.shortName = shortName;
			this.encounters = encounters;
			isEmboldened = false;
			isCallOfTheMist = false;
		}

		public void SetWingPanelReference(WingPanel panel)
		{
			_wingPanel = panel;
		}

		public WingPanel GetWingPanelReference()
		{
			return _wingPanel;
		}

		public void setEmboldened(bool embolden)
		{
			isEmboldened = embolden;
		}

		public void setCallOfTheMist(bool call)
		{
			isCallOfTheMist = call;
		}

		public static Wing[] GetWingMetaData()
		{
			return new Wing[7]
			{
				new Wing("Spirit Vale", 1, "SV", new Encounter[4]
				{
					new Encounter("vale_guardian", "Vale Guardian", "VG"),
					new Encounter("spirit_woods", "Spirit Run", "SR"),
					new Encounter("gorseval", "Gorseval", "G"),
					new Encounter("sabetha", "Sabetha", "S")
				}),
				new Wing("Salvation Pass", 2, "SP", new Encounter[3]
				{
					new Encounter("slothasor", "Slothasor", "S"),
					new Encounter("bandit_trio", "Bandit Trio", "B3"),
					new Encounter("matthias", "Matthias Gabrel", "M")
				}),
				new Wing("Stronghold of the Faithful", 3, "SotF", new Encounter[4]
				{
					new Encounter("escort", "Escort", "E"),
					new Encounter("keep_construct", "Keep Construct", "KC"),
					new Encounter("twisted_castle", "Twisted Castel", "TC"),
					new Encounter("xera", "Xera", "X")
				}),
				new Wing("Bastion of the Penitent", 4, "BotP", new Encounter[4]
				{
					new Encounter("cairn", "Cairn the Indominable", "C"),
					new Encounter("mursaat_overseer", "Mursaat Overseer", "MO"),
					new Encounter("samarog", "Samarog", "S"),
					new Encounter("deimos", "Deimos", "D")
				}),
				new Wing("Hall of Chains", 5, "HoC", new Encounter[4]
				{
					new Encounter("soulless_horror", "Soulless Horror", "SH"),
					new Encounter("river_of_souls", "River of Souls", "R"),
					new Encounter("statues_of_grenth", "Statues of Grenth", "S"),
					new Encounter("voice_in_the_void", "Dhuum", "D")
				}),
				new Wing("Mythwright Gambit", 6, "MG", new Encounter[3]
				{
					new Encounter("conjured_amalgamate", "Conjured Amalgamate", "CA"),
					new Encounter("twin_largos", "Twin Largos", "TL"),
					new Encounter("qadim", "Qadim", "Q1")
				}),
				new Wing("The Key of Ahdashim", 7, "TKoA", new Encounter[4]
				{
					new Encounter("gate", "Gate", "G"),
					new Encounter("adina", "Cardinal Adina", "A"),
					new Encounter("sabir", "Cardinal Sabir", "S"),
					new Encounter("qadim_the_peerless", "Qadim the Peerless", "Q2")
				})
			};
		}
	}
}
