using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Maestro.Models;
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

		public static readonly Color GhostButtonBackground = new Color(60, 55, 70);

		public static readonly Color GhostButtonBorder = new Color(80, 75, 90);

		public static readonly Color GhostButtonHover = new Color(80, 75, 95);

		public static readonly Color GhostButtonText = CreamWhite;

		public static readonly Color SubtleBorder = new Color(255, 255, 255, 25);

		public static readonly Color InputLabelColor = new Color(170, 158, 135);

		public static readonly Color OctaveLabelColor = new Color(200, 210, 220);

		public static readonly Color HintTextColor = new Color(110, 105, 120);

		public static readonly Color ChipRest = new Color(58, 53, 64);

		public static readonly Color ChipLowerOctave = new Color(106, 90, 156);

		public static readonly Color ChipMiddleOctave = new Color(90, 138, 99);

		public static readonly Color ChipUpperOctave = new Color(154, 74, 82);

		public static readonly Color ChipLowerOctaveSharp = new Color(60, 50, 95);

		public static readonly Color ChipMiddleOctaveSharp = new Color(48, 88, 56);

		public static readonly Color ChipUpperOctaveSharp = new Color(100, 42, 48);

		public const int ActionButtonWidth = 90;

		public const int ActionButtonHeight = 26;

		public const int InputSpacing = 7;

		public const int PaddingContentBottom = 20;

		public const int PaddingContentTop = 2;

		public const int WindowContentTopPadding = 20;

		private static readonly Color WindowBackground = new Color(30, 24, 40, 255);

		private const int BACKGROUND_X_OFFSET = 1;

		private const int BACKGROUND_Y_OFFSET = 13;

		public const int CornerRadius = 4;

		private static Texture2D _cornerMask;

		public static readonly Color PianoDark = new Color(90, 176, 208);

		public static readonly Color HarpDark = new Color(140, 196, 144);

		public static readonly Color LuteDark = new Color(212, 166, 86);

		public static readonly Color BassDark = new Color(192, 112, 120);

		public static Texture2D GetCornerMask()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (_cornerMask != null)
			{
				return _cornerMask;
			}
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				int r = 4;
				_cornerMask = new Texture2D(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), r, r);
				Color[] data = (Color[])(object)new Color[r * r];
				for (int y = 0; y < r; y++)
				{
					for (int x = 0; x < r; x++)
					{
						float num = r - 1 - x;
						float dy = r - 1 - y;
						float dist = (float)Math.Sqrt(num * num + dy * dy);
						data[y * r + x] = ((dist <= (float)r - 0.5f) ? Color.get_White() : Color.get_Transparent());
					}
				}
				_cornerMask.SetData<Color>(data);
				return _cornerMask;
			}
			finally
			{
				((GraphicsDeviceContext)(ref context)).Dispose();
			}
		}

		public static void DrawRoundedRect(SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, Color color)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			Texture2D corner = GetCornerMask();
			int r = 4;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, pixel, new Rectangle(r, 0, bounds.Width - r * 2, bounds.Height), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, pixel, new Rectangle(0, r, r, bounds.Height - r * 2), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, pixel, new Rectangle(bounds.Width - r, r, r, bounds.Height - r * 2), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, corner, new Rectangle(0, 0, r, r), (Rectangle?)null, color, 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, corner, new Rectangle(bounds.Width - r, 0, r, r), (Rectangle?)null, color, 0f, Vector2.get_Zero(), (SpriteEffects)1);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, corner, new Rectangle(0, bounds.Height - r, r, r), (Rectangle?)null, color, 0f, Vector2.get_Zero(), (SpriteEffects)2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, corner, new Rectangle(bounds.Width - r, bounds.Height - r, r, r), (Rectangle?)null, color, 0f, Vector2.get_Zero(), (SpriteEffects)3);
		}

		public static void DrawBottomRoundedRect(SpriteBatch spriteBatch, Control ctrl, Rectangle bounds, Color color)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			Texture2D corner = GetCornerMask();
			int r = 4;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, pixel, new Rectangle(0, 0, bounds.Width, bounds.Height - r), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, pixel, new Rectangle(r, bounds.Height - r, bounds.Width - r * 2, r), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, corner, new Rectangle(0, bounds.Height - r, r, r), (Rectangle?)null, color, 0f, Vector2.get_Zero(), (SpriteEffects)2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, corner, new Rectangle(bounds.Width - r, bounds.Height - r, r, r), (Rectangle?)null, color, 0f, Vector2.get_Zero(), (SpriteEffects)3);
		}

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

		public static Color Brighten(Color color, int amount = 30)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			return new Color(Math.Min(((Color)(ref color)).get_R() + amount, 255), Math.Min(((Color)(ref color)).get_G() + amount, 255), Math.Min(((Color)(ref color)).get_B() + amount, 255), 255);
		}

		public static Color GetInstrumentAccent(InstrumentType instrument)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(instrument switch
			{
				InstrumentType.Piano => Piano, 
				InstrumentType.Harp => Harp, 
				InstrumentType.Lute => Lute, 
				InstrumentType.Bass => Bass, 
				_ => AmberGold, 
			});
		}

		public static Color GetInstrumentAccentDark(InstrumentType instrument)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(instrument switch
			{
				InstrumentType.Piano => PianoDark, 
				InstrumentType.Harp => HarpDark, 
				InstrumentType.Lute => LuteDark, 
				InstrumentType.Bass => BassDark, 
				_ => WarmBronze, 
			});
		}

		public static Color AccentTint(Color accent, float opacity)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			return new Color((int)((Color)(ref accent)).get_R(), (int)((Color)(ref accent)).get_G(), (int)((Color)(ref accent)).get_B(), (int)(255f * opacity));
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
