using BhModule.Community.Pathing.State.UserResources.Population;

namespace BhModule.Community.Pathing.State.UserResources
{
	public class PopulationDefaults
	{
		public const string FILENAME = "populate.yaml";

		public MarkerPopulationDefaults MarkerPopulationDefaults { get; set; } = new MarkerPopulationDefaults();


		public TrailPopulationDefaults TrailPopulationDefaults { get; set; } = new TrailPopulationDefaults();

	}
}
