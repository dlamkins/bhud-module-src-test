using System;
using System.IO;
using Blish_HUD;
using Ideka.NetCommon;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	internal class FontAssets : WeakRefDict<(string, int), BitmapFont>
	{
		private static readonly Logger Logger = Logger.GetLogger<FontAssets>();

		public BitmapFont Get()
		{
			return Get(null, null);
		}

		public BitmapFont Get(string? name, int? size)
		{
			return Get((name ?? CTextModule.Settings.FontName.Value, size ?? CTextModule.Settings.FontSize.Value));
		}

		public override BitmapFont Load((string name, int size) key)
		{
			BitmapFont font = null;
			Exception exception = null;
			try
			{
				font = ((key.name == "") ? CTextModule.ContentsManager.GetSpriteFont(CTextModule.DefaultFontPath, key.size) : FontUtils.GetSpriteFont(Path.Combine(CTextModule.BasePath, CTextModule.FontPath, key.name), key.size))?.ToBitmapFont();
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			if (font == null)
			{
				Logger.Warn($"Font load failed: {key.name} {key.size}\n{exception}");
			}
			return font ?? GameService.Content.get_DefaultFont32();
		}
	}
}
