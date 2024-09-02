namespace FarmingTracker
{
	public class FloatPoint
	{
		public float X { get; set; }

		public float Y { get; set; }

		public FloatPoint(float x, float y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"x: {X} y: {Y}";
		}
	}
}
