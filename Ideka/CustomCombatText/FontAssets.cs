using System;
using System.IO;
using Blish_HUD;
using FontStashSharp;
using Ideka.NetCommon;

namespace Ideka.CustomCombatText
{
	internal class FontAssets : WeakRefDict<(bool, string), FontSystem>
	{
		private static readonly Logger Logger = Logger.GetLogger<FontAssets>();

		public FontSystem Get(string? name = null)
		{
			return Get(@internal: false, name);
		}

		public FontSystem Get(bool @internal, string? name)
		{
			return Get((@internal, name ?? CTextModule.Settings.FontName.Value));
		}

		public override FontSystem Load((bool @internal, string name) key)
		{
			FontSystem font = null;
			Exception exception = null;
			try
			{
				var (flag, name) = key;
				FontSystem spriteFontBase = default(FontSystem);
				if (!(name == ""))
				{
					if (!flag)
					{
						if (name == null)
						{
							goto IL_007a;
						}
						spriteFontBase = FontUtils.GetSpriteFontBase(Path.Combine(CTextModule.BasePath, CTextModule.FontPath, key.name));
					}
					else
					{
						if (name == null)
						{
							goto IL_007a;
						}
						spriteFontBase = CTextModule.ContentsManager.GetSpriteFontBase(Path.Combine(CTextModule.FontPath, key.name));
					}
				}
				else
				{
					spriteFontBase = CTextModule.ContentsManager.GetSpriteFontBase(CTextModule.DefaultFontPath);
				}
				goto IL_007f;
				IL_007f:
				font = spriteFontBase;
				goto end_IL_0004;
				IL_007a:
				_003CPrivateImplementationDetails_003E.ThrowInvalidOperationException();
				goto IL_007f;
				end_IL_0004:;
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			if (font == null)
			{
				Logger.Warn($"Font load failed: {key.name}\n{exception}");
			}
			return font;
		}
	}
}
