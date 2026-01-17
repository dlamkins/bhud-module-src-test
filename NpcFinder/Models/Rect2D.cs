namespace NpcFinder.Models
{
	public struct Rect2D
	{
		public double X1;

		public double Y1;

		public double X2;

		public double Y2;

		public Rect2D(double x1, double y1, double x2, double y2)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}
	}
}
