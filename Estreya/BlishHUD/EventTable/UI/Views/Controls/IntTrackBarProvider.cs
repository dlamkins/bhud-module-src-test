using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class IntTrackBarProvider : ControlProvider<int, int>
	{
		public override Control CreateControl(BoxedValue<int> value, Func<int, bool> isEnabled, Func<int, bool> isValid, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			TrackBar trackBar = val;
			((Control)trackBar).set_Enabled(isEnabled?.Invoke((int)trackBar.get_Value()) ?? true);
			trackBar.set_MinValue(range.HasValue ? range.Value.Min : 0f);
			trackBar.set_MaxValue(range.HasValue ? range.Value.Max : 100f);
			trackBar.set_Value((float)(value?.Value ?? 50));
			if (value != null)
			{
				trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
				{
					if (isValid?.Invoke((int)e.get_Value()) ?? true)
					{
						value.Value = (int)e.get_Value();
					}
					else
					{
						trackBar.set_Value((float)value.Value);
					}
				});
			}
			return (Control)(object)trackBar;
		}
	}
}
