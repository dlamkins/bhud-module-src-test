using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace SongbookOfTyria.UI.Utilities
{
	public static class BitmapFontLoader
	{
		private class FontData
		{
			public string Face { get; set; } = string.Empty;


			public int Size { get; set; }

			public int LineHeight { get; set; }

			public int Base { get; set; }

			public int ScaleW { get; set; }

			public int ScaleH { get; set; }

			public List<CharData> Characters { get; } = new List<CharData>();

		}

		private class CharData
		{
			public int Id { get; set; }

			public int X { get; set; }

			public int Y { get; set; }

			public int Width { get; set; }

			public int Height { get; set; }

			public int XOffset { get; set; }

			public int YOffset { get; set; }

			public int XAdvance { get; set; }

			public int Page { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(BitmapFontLoader));

		private static readonly ConcurrentDictionary<string, BitmapFont> _loadedFonts = new ConcurrentDictionary<string, BitmapFont>();

		public static BitmapFont Load(ContentsManager contentsManager, string fontPath, string texturePath, int letterSpacing = 0)
		{
			if (_loadedFonts.TryGetValue(fontPath, out var cachedFont))
			{
				return cachedFont;
			}
			try
			{
				Texture2D fontTexture;
				using (Stream textureStream = contentsManager.GetFileStream(texturePath))
				{
					fontTexture = TextureUtil.FromStreamPremultiplied(textureStream);
				}
				string fontContent;
				using (Stream fontStream = contentsManager.GetFileStream(fontPath))
				{
					using StreamReader reader = new StreamReader(fontStream);
					fontContent = reader.ReadToEnd();
				}
				BitmapFont font = CreateBitmapFont(ParseFontFile(fontContent), fontTexture);
				font.set_LetterSpacing(letterSpacing);
				_loadedFonts.TryAdd(fontPath, font);
				Logger.Debug("Loaded and cached bitmap font: {FontPath}", new object[1] { fontPath });
				return font;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load bitmap font from {Path}", new object[1] { fontPath });
				throw;
			}
		}

		public static void ClearCache()
		{
			_loadedFonts.Clear();
			Logger.Debug("Bitmap font cache cleared.");
		}

		private static FontData ParseFontFile(string content)
		{
			FontData fontData = new FontData();
			string[] array = content.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in array)
			{
				if (line.StartsWith("info "))
				{
					fontData.Face = GetStringValue(line, "face");
					fontData.Size = GetIntValue(line, "size");
				}
				else if (line.StartsWith("common "))
				{
					fontData.LineHeight = GetIntValue(line, "lineHeight");
					fontData.Base = GetIntValue(line, "base");
					fontData.ScaleW = GetIntValue(line, "scaleW");
					fontData.ScaleH = GetIntValue(line, "scaleH");
				}
				else if (line.StartsWith("char "))
				{
					CharData charData = new CharData
					{
						Id = GetIntValue(line, "id"),
						X = GetIntValue(line, "x"),
						Y = GetIntValue(line, "y"),
						Width = GetIntValue(line, "width"),
						Height = GetIntValue(line, "height"),
						XOffset = GetIntValue(line, "xoffset"),
						YOffset = GetIntValue(line, "yoffset"),
						XAdvance = GetIntValue(line, "xadvance"),
						Page = GetIntValue(line, "page")
					};
					fontData.Characters.Add(charData);
				}
			}
			return fontData;
		}

		private static BitmapFont CreateBitmapFont(FontData fontData, Texture2D texture)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			List<BitmapFontRegion> regions = new List<BitmapFontRegion>();
			Rectangle bounds = default(Rectangle);
			foreach (CharData charData in fontData.Characters)
			{
				((Rectangle)(ref bounds))._002Ector(charData.X, charData.Y, charData.Width, charData.Height);
				BitmapFontRegion region = new BitmapFontRegion(new TextureRegion2D(texture, bounds), charData.Id, charData.XOffset, charData.YOffset, charData.XAdvance);
				regions.Add(region);
			}
			return new BitmapFont(fontData.Face, (IEnumerable<BitmapFontRegion>)regions, fontData.LineHeight);
		}

		private static int GetIntValue(string line, string key)
		{
			string pattern = key + "=(-?\\d+)";
			Match match = Regex.Match(line, pattern);
			int result = default(int);
			if (match.Success && InvariantUtil.TryParseInt(match.Groups[1].Value, ref result))
			{
				return result;
			}
			return 0;
		}

		private static string GetStringValue(string line, string key)
		{
			string pattern = key + "=\"([^\"]*)\"";
			Match match = Regex.Match(line, pattern);
			if (!match.Success)
			{
				return string.Empty;
			}
			return match.Groups[1].Value;
		}
	}
}
