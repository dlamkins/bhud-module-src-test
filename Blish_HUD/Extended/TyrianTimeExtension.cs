namespace Blish_HUD.Extended
{
	public static class TyrianTimeExtension
	{
		public static TyrianTime Resolve(this TyrianTime time)
		{
			switch (time)
			{
			case TyrianTime.DAY:
			case TyrianTime.DUSK:
				return TyrianTime.DAY;
			case TyrianTime.DAWN:
			case TyrianTime.NIGHT:
				return TyrianTime.NIGHT;
			default:
				return TyrianTime.NONE;
			}
		}

		public static bool ContextEquals(this TyrianTime source, TyrianTime other)
		{
			return source.Resolve() == other.Resolve();
		}
	}
}
