using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Settings;
using MonoGame.Extended.BitmapFonts;

namespace GatheringTools.LogoutOverlay
{
	public class FontService
	{
		public static readonly List<BitmapFont> Fonts = new List<BitmapFont>
		{
			GetFont((FontSize)11),
			GetFont((FontSize)12),
			GetFont((FontSize)14),
			GetFont((FontSize)16),
			GetFont((FontSize)18),
			GetFont((FontSize)20),
			GetFont((FontSize)22),
			GetFont((FontSize)24),
			GetFont((FontSize)32),
			GetFont((FontSize)34),
			GetFont((FontSize)36)
		};

		public static SettingEntry<int> CreateFontSizeIndexSetting(SettingCollection settings)
		{
			SettingEntry<int> obj = settings.DefineSetting<int>("font size index (logout overlay)", Fonts.Count - 1, (Func<string>)(() => "font size"), (Func<string>)(() => "Change font size of the reminder text"));
			SettingComplianceExtensions.SetRange(obj, 0, Fonts.Count - 1);
			return obj;
		}

		private static BitmapFont GetFont(FontSize fontSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
		}
	}
}
