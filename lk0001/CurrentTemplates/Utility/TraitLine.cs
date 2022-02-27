namespace lk0001.CurrentTemplates.Utility
{
	internal class TraitLine
	{
		public string Name;

		public TraitColumn[] Columns;

		public bool Elite;

		public TraitLine(string name, int[][] traits, bool elite)
		{
			Name = name;
			Elite = elite;
			Columns = new TraitColumn[3];
			for (int i = 0; i < 3; i++)
			{
				Columns[i] = new TraitColumn(traits[i]);
			}
		}

		public TraitLine(string name, int[][] traits)
			: this(name, traits, elite: false)
		{
		}
	}
}
