namespace Blish_HUD.Extended
{
	public static class TyrianTimeExtension
	{
		public static TyrianTime Resolve(this TyrianTime time)
		{
			switch (time)
			{
			case TyrianTime.Day:
			case TyrianTime.Dusk:
				return TyrianTime.Day;
			case TyrianTime.Dawn:
			case TyrianTime.Night:
				return TyrianTime.Night;
			default:
				return TyrianTime.None;
			}
		}

		public static bool ContextEquals(this TyrianTime source, TyrianTime other)
		{
			return source.Resolve() == other.Resolve();
		}
	}
}
