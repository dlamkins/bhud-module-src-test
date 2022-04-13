using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	internal class FloatTrackBarProvider : ControlProvider<float>
	{
		internal override Control CreateControl(SettingEntry<float> settingEntry, Func<SettingEntry<float>, float, bool> validationFunction, int width, int heigth, int x, int y)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			(float, float)? range = settingEntry?.GetRange() ?? null;
			TrackBar val = new TrackBar();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Enabled(!((SettingEntry)(object)settingEntry).IsDisabled());
			val.set_MinValue(range.HasValue ? range.Value.Item1 : 0f);
			val.set_MaxValue(range.HasValue ? range.Value.Item2 : 100f);
			val.set_Value(settingEntry?.GetValue() ?? 50f);
			TrackBar trackBar = val;
			if (settingEntry != null)
			{
				trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
				{
					if (validationFunction?.Invoke(settingEntry, e.get_Value()) ?? false)
					{
						settingEntry.set_Value((float)(int)e.get_Value());
					}
					else
					{
						trackBar.set_Value(settingEntry.get_Value());
					}
				});
			}
			return (Control)(object)trackBar;
		}
	}
}
