using Ideka.BHUDCommon;

namespace Ideka.RacingMeter
{
	public class WorldCircle : WorldPrimitive
	{
		public override Primitive Primitive { get; }

		public WorldCircle()
		{
			Primitive = Primitive.HorizontalCircle(0.5f, 128);
		}
	}
}