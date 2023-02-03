using System;

namespace Ideka.RacingMeter
{
	public static class RectAnchorExtensions
	{
		public static T With<T>(this T ra, Action<T> act) where T : RectAnchor
		{
			act(ra);
			return ra;
		}

		public static T WithUpdate<T>(this T ra, Action<T> update) where T : RectAnchor
		{
			Action<T> update2 = update;
			T ra2 = ra;
			ref T reference = ref ra2;
			reference.Update = (Action<RectAnchor>)Delegate.Combine(reference.Update, (Action<RectAnchor>)delegate
			{
				update2(ra2);
			});
			return ra2;
		}
	}
}
