using System.Globalization;
using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Taskmaster.UI
{
	public static class TaskmasterTheme
	{
		public static readonly Color CreamWhite = new Color(240, 230, 210);

		public static readonly Color MutedCream = new Color(200, 191, 169);

		public static readonly Color DimText = new Color(128, 122, 108);

		public static readonly Color DoneText = new Color(106, 106, 98);

		public static readonly Color Gold = new Color(212, 166, 86);

		public static readonly Color DueSoon = new Color(224, 168, 74);

		public static readonly Color Danger = new Color(201, 106, 90);

		public static readonly Color Success = new Color(143, 191, 106);

		public static readonly Color RowHover = Color.get_White() * 0.07f;

		public static readonly Color RowSelected = Gold * 0.18f;

		public static readonly Color SubtleBorder = Color.get_White() * 0.1f;

		public static readonly Color TabActiveFill = new Color(46, 40, 41);

		public static readonly Color TabActiveBorder = new Color(82, 70, 68);

		public static readonly Color TabHoverFill = Color.get_White() * 0.055f;

		public static readonly Color TabBadgeFill = new Color(29, 28, 34);

		public static readonly Color TabBadgeActiveFill = new Color(31, 29, 31);

		public static readonly Color TabBadgeBorder = Color.get_White() * 0.09f;

		public static readonly Color TabInactiveText = new Color(138, 138, 128);

		public static readonly Color ChipFill = new Color(38, 38, 44);

		public static readonly Color ChipBorder = new Color(74, 74, 82);

		public static readonly Color ActionBarFill = new Color(25, 24, 29, 235);

		public static readonly Color EditorFill = new Color(29, 28, 34, 245);

		public static readonly Color EditorBorder = new Color(83, 70, 57);

		public static readonly Color IconGlyph = new Color(57, 50, 38);

		public static readonly Color ToggleActiveFill = new Color(46, 34, 28);

		public static readonly Color ToggleActiveGlyph = CreamWhite;

		public static readonly (string Name, Color Value)[] TabAccentPresets = new(string, Color)[8]
		{
			("Gold", Gold),
			("Rose", new Color(201, 112, 122)),
			("Coral", new Color(217, 125, 90)),
			("Teal", new Color(90, 166, 160)),
			("Sky", new Color(90, 143, 201)),
			("Violet", new Color(143, 122, 201)),
			("Sage", new Color(127, 166, 90)),
			("Slate", new Color(138, 138, 148))
		};

		private static readonly Color BgTop = new Color(24, 22, 30, 255);

		private static readonly Color BgBottom = new Color(36, 30, 26, 255);

		private const int BACKGROUND_X_OFFSET = 1;

		private const int BACKGROUND_Y_OFFSET = 13;

		public static string ToHex(Color c)
		{
			return $"{((Color)(ref c)).get_R():X2}{((Color)(ref c)).get_G():X2}{((Color)(ref c)).get_B():X2}";
		}

		public static Color? ParseAccentHex(string hex)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(hex) || hex.Length != 6)
			{
				return null;
			}
			if (!byte.TryParse(hex.Substring(0, 2), NumberStyles.HexNumber, null, out var r))
			{
				return null;
			}
			if (!byte.TryParse(hex.Substring(2, 2), NumberStyles.HexNumber, null, out var g))
			{
				return null;
			}
			if (!byte.TryParse(hex.Substring(4, 2), NumberStyles.HexNumber, null, out var b))
			{
				return null;
			}
			return new Color((int)r, (int)g, (int)b);
		}

		public static Color WithAlpha(Color c, int a)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Color((int)((Color)(ref c)).get_R(), (int)((Color)(ref c)).get_G(), (int)((Color)(ref c)).get_B(), a);
		}

		public static Texture2D CreateWindowBackground(int windowWidth, int windowHeight)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			int width = windowWidth - 1;
			int height = windowHeight - 13;
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D texture = new Texture2D(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), width, height);
				Color[] data = (Color[])(object)new Color[width * height];
				for (int y = 0; y < height; y++)
				{
					float t = ((height > 1) ? ((float)y / (float)(height - 1)) : 0f);
					Color row = Color.Lerp(BgTop, BgBottom, t);
					for (int x = 0; x < width; x++)
					{
						data[y * width + x] = row;
					}
				}
				texture.SetData<Color>(data);
				return texture;
			}
			finally
			{
				((GraphicsDeviceContext)(ref context)).Dispose();
			}
		}
	}
}
