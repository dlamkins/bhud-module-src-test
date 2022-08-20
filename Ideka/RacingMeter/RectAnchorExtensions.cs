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
			ref T reference = ref ra;
			reference.Update = (Action<RectAnchor>)Delegate.Combine(reference.Update, (Action<RectAnchor>)delegate
			{
				update(ra);
			});
			return ra;
		}
	}
}
