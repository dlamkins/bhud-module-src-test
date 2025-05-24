using System;
using System.IO;
using Blish_HUD.Modules.Managers;
using FontStashSharp;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	internal static class FontUtils
	{
		public static Size2 MeasureStringFixed(this BitmapFont font, string str)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			if (!str.EndsWith(" "))
			{
				return font.MeasureString(str);
			}
			string trimmed = str.Trim(' ');
			int spaceCount = str.Length - trimmed.Length;
			Size2 size = font.MeasureString(trimmed);
			if (spaceCount == 0)
			{
				return size;
			}
			float spaceWidth = font.MeasureString(" ").Width;
			float aWidth = font.MeasureString("a").Width;
			spaceWidth = Math.Max(spaceWidth, font.MeasureString("a ").Width - aWidth);
			spaceWidth = Math.Max(spaceWidth, font.MeasureString(" a").Width - aWidth);
			size.Width += spaceWidth * (float)spaceCount;
			return size;
		}

		public static FontSystem GetSpriteFontBase(this ContentsManager manager, string fontPath)
		{
			using Stream file = manager.GetFileStream(fontPath);
			return GetSpriteFontBase(file);
		}

		public static FontSystem GetSpriteFontBase(string fontPath)
		{
			using FileStream file = File.OpenRead(fontPath);
			return GetSpriteFontBase(file);
		}

		public static FontSystem GetSpriteFontBase(Stream file)
		{
			FontSystem fontSystem = new FontSystem();
			byte[] fontData = new byte[file.Length];
			file.Read(fontData, 0, fontData.Length);
			fontSystem.AddFont(fontData);
			return fontSystem;
		}
	}
}
