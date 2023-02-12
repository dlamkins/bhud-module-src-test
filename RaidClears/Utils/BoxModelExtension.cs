using System;
using Blish_HUD;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Utils
{
	internal static class BoxModelExtension
	{
		public static void WatchColorSettings(this BoxModel box, SettingEntry<string> clearedColor, SettingEntry<string> notClearedColor)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			BoxModel box2 = box;
			SettingEntry<string> notClearedColor2 = notClearedColor;
			SettingEntry<string> clearedColor2 = clearedColor;
			box2.SetClearColors(clearedColor2.get_Value().HexToXnaColor(), notClearedColor2.get_Value().HexToXnaColor());
			clearedColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				box2.SetClearColors(e.get_NewValue().HexToXnaColor(), notClearedColor2.get_Value().HexToXnaColor());
			});
			notClearedColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				box2.SetClearColors(clearedColor2.get_Value().HexToXnaColor(), e.get_NewValue().HexToXnaColor());
			});
		}
	}
}
