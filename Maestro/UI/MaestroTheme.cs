using Blish_HUD;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI
{
	public static class MaestroTheme
	{
		public static readonly Color AmberGold = new Color(212, 166, 86);

		public static readonly Color DeepAmber = new Color(232, 184, 74);

		public static readonly Color WarmBronze = new Color(139, 105, 20);

		public static readonly Color DarkCharcoal = new Color(26, 26, 26);

		public static readonly Color SlateGray = new Color(45, 45, 45);

		public static readonly Color MediumGray = new Color(64, 64, 64);

		public static readonly Color CreamWhite = new Color(240, 230, 210);

		public static readonly Color MutedCream = new Color(200, 191, 169);

		public static readonly Color LightGray = new Color(128, 128, 128);

		public static readonly Color Playing = new Color(76, 175, 80);

		public static readonly Color Paused = new Color(255, 193, 7);

		public static readonly Color Error = new Color(244, 67, 54);

		public static readonly Color Disabled = new Color(85, 85, 85);

		public static readonly Color Piano = new Color(126, 200, 227);

		public static readonly Color Harp = new Color(184, 212, 168);

		public static readonly Color Lute = new Color(232, 193, 112);

		public static readonly Color Bass = new Color(212, 132, 140);

		public static readonly Color PanelBackground = new Color(45, 45, 45, 180);

		public static readonly Color PanelHover = new Color(64, 64, 64, 200);

		public static readonly Color PanelSelected = new Color(51, 51, 51, 220);

		public static readonly Color DrawerHeader = new Color(38, 42, 48);

		public static readonly Color DrawerAccent = new Color(85, 95, 110);

		public static readonly Color DrawerBackground = new Color(25, 28, 32);

		public static readonly Color PianoWhiteKey = new Color(250, 250, 245);

		public static readonly Color PianoWhiteKeyHover = new Color(230, 230, 220);

		public static readonly Color PianoWhiteKeyPressed = new Color(200, 180, 140);

		public static readonly Color PianoBlackKey = new Color(30, 30, 30);

		public static readonly Color PianoBlackKeyHover = new Color(50, 50, 50);

		public static readonly Color PianoBlackKeyPressed = new Color(80, 70, 50);

		public static readonly Color ButtonBackground = new Color(60, 60, 60);

		public static readonly Color ButtonBackgroundHover = new Color(80, 80, 80);

		public static readonly Color ChipRest = new Color(80, 80, 90);

		public static readonly Color ChipLowerOctave = new Color(96, 94, 156);

		public static readonly Color ChipMiddleOctave = new Color(86, 144, 99);

		public static readonly Color ChipUpperOctave = new Color(170, 73, 72);

		public const int ActionButtonWidth = 90;

		public const int ActionButtonHeight = 26;

		public const int InputSpacing = 7;

		public const int PaddingContentBottom = 20;

		public const int PaddingContentTop = 2;

		public const int WindowContentTopPadding = 20;

		private static readonly Color WindowBackground = new Color(30, 30, 30, 255);

		private const int BACKGROUND_X_OFFSET = 1;

		private const int BACKGROUND_Y_OFFSET = 13;

		public static Color WithAlpha(Color color, int alpha)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Color((int)((Color)(ref color)).get_R(), (int)((Color)(ref color)).get_G(), (int)((Color)(ref color)).get_B(), alpha);
		}

		public static Color Darken(Color color, float amount)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return new Color((int)((float)(int)((Color)(ref color)).get_R() * amount), (int)((float)(int)((Color)(ref color)).get_G() * amount), (int)((float)(int)((Color)(ref color)).get_B() * amount), (int)((Color)(ref color)).get_A());
		}

		public static Texture2D CreateWindowBackground(int windowWidth, int windowHeight)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			int width = windowWidth - 1;
			int height = windowHeight - 13;
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D texture = new Texture2D(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), width, height);
				Color[] data = (Color[])(object)new Color[width * height];
				for (int i = 0; i < data.Length; i++)
				{
					data[i] = WindowBackground;
				}
				texture.SetData<Color>(data);
				return texture;
			}
			finally
			{
				((GraphicsDeviceContext)(ref context)).Dispose();
			}
		}

		public static Texture2D CreateDrawerBackground(int windowWidth, int windowHeight)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			int width = windowWidth - 1;
			int height = windowHeight - 13;
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D texture = new Texture2D(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), width, height);
				Color[] data = (Color[])(object)new Color[width * height];
				for (int i = 0; i < data.Length; i++)
				{
					data[i] = DrawerBackground;
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
