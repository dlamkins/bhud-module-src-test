using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	internal class IntTrackBarProvider : ControlProvider<int>
	{
		internal override Control CreateControl(SettingEntry<int> settingEntry, Func<SettingEntry<int>, int, bool> validationFunction, int width, int heigth, int x, int y)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			(int, int)? range = settingEntry?.GetRange() ?? null;
			TrackBar val = new TrackBar();
			((Control)val).set_Width(width);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Enabled(!((SettingEntry)(object)settingEntry).IsDisabled());
			val.set_MinValue((float)(range.HasValue ? range.Value.Item1 : 0));
			val.set_MaxValue((float)(range.HasValue ? range.Value.Item2 : 100));
			val.set_Value((float)(settingEntry?.GetValue() ?? 50));
			TrackBar trackBar = val;
			if (settingEntry != null)
			{
				trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
				{
					if (validationFunction?.Invoke(settingEntry, (int)e.get_Value()) ?? false)
					{
						settingEntry.set_Value((int)e.get_Value());
					}
					else
					{
						trackBar.set_Value((float)settingEntry.get_Value());
					}
				});
			}
			return (Control)(object)trackBar;
		}
	}
}
