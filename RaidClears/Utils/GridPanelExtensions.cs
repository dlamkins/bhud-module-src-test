using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Utils
{
	public static class GridPanelExtensions
	{
		public static void BackgroundColorChange(this GridPanel panel, SettingEntry<float> opacity, SettingEntry<string> bgColor)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			GridPanel panel2 = panel;
			SettingEntry<string> bgColor2 = bgColor;
			SettingEntry<float> opacity2 = opacity;
			opacity2.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object _, ValueChangedEventArgs<float> e)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				((Control)panel2).set_BackgroundColor(AddAlphaToColor(bgColor2.get_Value().HexToXnaColor(), e.get_NewValue()));
			});
			bgColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				((Control)panel2).set_BackgroundColor(AddAlphaToColor(e.get_NewValue().HexToXnaColor(), opacity2.get_Value()));
			});
			((Control)panel2).set_BackgroundColor(AddAlphaToColor(bgColor2.get_Value().HexToXnaColor(), opacity2.get_Value()));
		}

		private static Color AddAlphaToColor(Color color, float opacity)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return new Color(color, (int)(255f * opacity));
		}
	}
}
