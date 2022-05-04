using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class FloatTrackBarProvider : ControlProvider<float, float>
	{
		public override Control CreateControl(BoxedValue<float> value, Func<float, bool> isEnabled, Func<float, bool> isValid, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			val.set_SmallStep(true);
			TrackBar trackBar = val;
			((Control)trackBar).set_Enabled(isEnabled?.Invoke(trackBar.get_Value()) ?? true);
			trackBar.set_MinValue(range.HasValue ? range.Value.Min : 0f);
			trackBar.set_MaxValue(range.HasValue ? range.Value.Max : 100f);
			trackBar.set_Value(value?.Value ?? 50f);
			if (value != null)
			{
				trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
				{
					if (isValid?.Invoke(e.get_Value()) ?? true)
					{
						value.Value = e.get_Value();
					}
					else
					{
						trackBar.set_Value(value.Value);
					}
				});
			}
			return (Control)(object)trackBar;
		}
	}
}
