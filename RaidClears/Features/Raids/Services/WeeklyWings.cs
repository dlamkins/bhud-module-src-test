namespace RaidClears.Features.Raids.Services
{
	public class WeeklyWings
	{
		public int Emboldened { get; }

		public int CallOfTheMist { get; }

		public int LatestRelease { get; }

		public WeeklyWings(int emboldened, int callOfTheMist, int latestReleaes)
		{
			Emboldened = emboldened;
			CallOfTheMist = callOfTheMist;
			LatestRelease = latestReleaes;
		}
	}
}
