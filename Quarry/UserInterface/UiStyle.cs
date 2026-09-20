using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Quarry.Models;

namespace Quarry.UserInterface
{
	public static class UiStyle
	{
		public static class Guidance
		{
			public static Color Tagged => GuidanceStyle.Tagged;

			public static Color Coordinate => GuidanceStyle.Coordinate;

			public static Color Route => GuidanceStyle.Route;

			public static Color Area => GuidanceStyle.Area;
		}

		private static BitmapFont numeralFont;

		private static BitmapFont sectionFont;

		public static readonly Color TextPrimary = Color.get_White();

		public static readonly Color ShadowColor = Color.get_Black() * 0.8f;

		public static readonly Color TextSecondary = new Color(200, 200, 200);

		public static readonly Color TextMuted = new Color(150, 150, 150);

		public static readonly Color Accent = Colors.ColonialWhite;

		public static readonly Color ProgressFill = new Color(120, 170, 220);

		public static readonly Color NearDone = new Color(212, 175, 55);

		public static readonly Color Complete = new Color(120, 200, 120) * 0.35f;

		public static readonly Color CardBackground = Color.get_Black() * 0.4f;

		public static readonly Color CardBackgroundHover = Color.get_Black() * 0.28f;

		public static readonly Color CardBorder = new Color(238, 233, 217) * 0.12f;

		public static readonly Color WindowBody = new Color(12, 11, 9) * 0.84f;

		public static readonly Color Rank = new Color(143, 138, 124);

		public static readonly Color PipTodo = new Color(238, 233, 217) * 0.16f;

		public static readonly Color ManualDone = new Color(238, 233, 217);

		public const int Gutter = 8;

		public const int CardPadding = 8;

		public static BitmapFont TitleFont => GameService.Content.get_DefaultFont16();

		public static BitmapFont NumeralFont => numeralFont ?? (numeralFont = GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));

		public static BitmapFont BodyFont => GameService.Content.get_DefaultFont14();

		public static BitmapFont HeaderFont => GameService.Content.get_DefaultFont18();

		public static BitmapFont SectionFont => sectionFont ?? (sectionFont = GameService.Content.GetFont((FontFace)0, (FontSize)14, (FontStyle)2));

		public static void ApplyTextPrimary(Label label)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			label.set_TextColor(TextPrimary);
			label.set_ShowShadow(true);
			label.set_ShadowColor(ShadowColor);
		}

		public static void ApplyShadow(Label label, Color textColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			label.set_TextColor(textColor);
			label.set_ShowShadow(true);
			label.set_ShadowColor(ShadowColor);
		}
	}
}
