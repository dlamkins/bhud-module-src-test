using RaidClears.Features.Fractals.Services;

namespace RaidClears.Features.Fractals.Models
{
	public class CMInterface
	{
		public FractalMap Map { get; set; }

		public int Scale { get; set; }

		public int DayOfyear { get; set; }

		public CMInterface(FractalMap map, int scale, int dayOfYear)
		{
			Map = map;
			Scale = scale;
			DayOfyear = dayOfYear;
		}
	}
}
