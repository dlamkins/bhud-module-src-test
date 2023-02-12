namespace RaidClears.Features.Raids.Services
{
	public class WeeklyWings
	{
		public int Emboldened { get; }

		public int CallOfTheMist { get; }

		public WeeklyWings(int emboldened, int callOfTheMist)
		{
			Emboldened = emboldened;
			CallOfTheMist = callOfTheMist;
		}
	}
}
