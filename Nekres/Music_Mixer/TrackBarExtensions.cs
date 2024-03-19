using System;
using Blish_HUD.Controls;

namespace Nekres.Music_Mixer
{
	internal static class TrackBarExtensions
	{
		public static void RefreshValue(this TrackBar volumeTrackBar, float value)
		{
			volumeTrackBar.set_MinValue(Math.Min(volumeTrackBar.get_MinValue(), value));
			volumeTrackBar.set_MaxValue(Math.Max(volumeTrackBar.get_MaxValue(), value));
			volumeTrackBar.set_Value(value);
		}
	}
}
