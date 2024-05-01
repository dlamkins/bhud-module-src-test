using System;
using System.IO;
using Blish_HUD;
using Ideka.NetCommon;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	internal class FontAssets : WeakRefDict<(bool, string, float), BitmapFont>
	{
		private static readonly Logger Logger = Logger.GetLogger<FontAssets>();

		public BitmapFont Get()
		{
			return Get(null, null);
		}

		public BitmapFont Get(string? name, float? size)
		{
			return Get(@internal: false, name, size);
		}

		public BitmapFont Get(bool @internal, string? name, float? size)
		{
			return Get((@internal, name ?? CTextModule.Settings.FontName.Value, size ?? ((float)CTextModule.Settings.FontSize.Value)));
		}

		public override BitmapFont Load((bool @internal, string name, float size) key)
		{
			BitmapFont font = null;
			Exception exception = null;
			try
			{
				var (flag, name, _) = key;
				SpriteFont spriteFont = default(SpriteFont);
				if (!(name == ""))
				{
					if (!flag)
					{
						if (name == null)
						{
							goto IL_009b;
						}
						spriteFont = FontUtils.GetSpriteFont(Path.Combine(CTextModule.BasePath, CTextModule.FontPath, key.name), key.size);
					}
					else
					{
						if (name == null)
						{
							goto IL_009b;
						}
						spriteFont = CTextModule.ContentsManager.GetSpriteFont(Path.Combine(CTextModule.FontPath, key.name), key.size);
					}
				}
				else
				{
					spriteFont = CTextModule.ContentsManager.GetSpriteFont(CTextModule.DefaultFontPath, key.size);
				}
				goto IL_00a0;
				IL_00a0:
				font = spriteFont?.ToBitmapFont();
				goto end_IL_0004;
				IL_009b:
				_003CPrivateImplementationDetails_003E.ThrowInvalidOperationException();
				goto IL_00a0;
				end_IL_0004:;
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
