namespace Kenedia.Modules.Core.Utility
{
	public static class Tweening
	{
		public static class Quartic
		{
			public static float EaseIn(float t, float b, float c, float d)
			{
				return c * (t /= d) * t * t * t + b;
			}

			public static float EaseOut(float t, float b, float c, float d)
			{
				return (0f - c) * ((t = t / d - 1f) * t * t * t - 1f) + b;
			}

			public static float EaseInOut(float t, float b, float c, float d)
			{
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * t * t * t * t + b;
				}
				return (0f - c) / 2f * ((t -= 2f) * t * t * t - 2f) + b;
			}

			public static float EaseOutBounce(float currentTime, float startValue, float changeInValue, float totalTime)
			{
				float magic1 = 7.5625f;
				float magic2 = 2.75f;
				float magic3 = 1.5f;
				float magic4 = 2.25f;
				float magic5 = 2.625f;
				float magic6 = 0.75f;
				float magic7 = 0.9375f;
				float magic8 = 63f / 64f;
				if ((currentTime /= totalTime) < 1f / magic2)
				{
					return changeInValue * (magic1 * currentTime * currentTime) + startValue;
				}
				if (currentTime < 2f / magic2)
				{
					return changeInValue * (magic1 * (currentTime -= magic3 / magic2) * currentTime + magic6) + startValue;
				}
				if ((double)currentTime < 2.5 / (double)magic2)
				{
					return changeInValue * (magic1 * (currentTime -= magic4 / magic2) * currentTime + magic7) + startValue;
				}
				return changeInValue * (magic1 * (currentTime -= magic5 / magic2) * currentTime + magic8) + startValue;
			}
		}
	}
}
