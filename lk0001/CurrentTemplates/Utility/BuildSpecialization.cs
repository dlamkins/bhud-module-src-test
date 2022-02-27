namespace lk0001.CurrentTemplates.Utility
{
	internal class BuildSpecialization
	{
		public Constants.Specialization Id;

		public int[] BuildTraits;

		public int[] Traits;

		public BuildSpecialization(Constants.Specialization id, int[] traits)
		{
			Id = id;
			BuildTraits = traits;
			Traits = new int[3];
			for (int i = 0; i < 3; i++)
			{
				Traits[i] = Constants.TraitLines[id].Columns[i].Traits[traits[i]];
			}
		}
	}
}
