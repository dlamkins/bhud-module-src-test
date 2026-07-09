using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace rp.spark.UI.Views
{
	internal class ProfileTooltipView : View, ITooltipView, IView
	{
		private const int MinWidth = 220;

		private const int MaxWidth = 520;

		private const int Padding = 2;

		private const int TitleHeight = 28;

		private const int TitleGap = 2;

		private const int DescriptionLineHeight = 22;

		private const int DescriptionMaxLines = 6;

		private static readonly Color TooltipBackground = new Color(8, 12, 14, 245);

		private static readonly Color TitleColor = new Color(235, 186, 108);

		private static readonly Color DescriptionColor = new Color(235, 235, 235);

		private readonly string _title;

		private readonly string _description;

		public ProfileTooltipView(string title, string description, string fallbackTitle = "Profile")
			: this()
		{
			_title = (string.IsNullOrWhiteSpace(title) ? fallbackTitle : title.Trim());
			_description = (string.IsNullOrWhiteSpace(description) ? "No description set." : description.Trim());
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			int tooltipWidth = GetTooltipWidth(_title);
			int contentWidth = tooltipWidth - 4;
			int descriptionHeight = GetWrappedTextHeight(_description, contentWidth, GameService.Content.get_DefaultFont16(), 22, 6);
			((Control)buildPanel).set_Size(new Point(tooltipWidth, 34 + descriptionHeight));
			((Control)buildPanel).set_BackgroundColor(TooltipBackground);
			Label val = new Label();
			val.set_Text(_title);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(TitleColor);
			val.set_StrokeText(true);
			val.set_WrapText(false);
			val.set_ShowShadow(true);
			((Control)val).set_Location(new Point(2, 2));
			((Control)val).set_Size(new Point(contentWidth, 28));
			((Control)val).set_Parent(buildPanel);
			Label val2 = new Label();
			val2.set_Text(_description);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(DescriptionColor);
			val2.set_WrapText(true);
			((Control)val2).set_Location(new Point(2, 32));
			((Control)val2).set_Size(new Point(contentWidth, descriptionHeight));
			((Control)val2).set_Parent(buildPanel);
		}

		private static int GetTooltipWidth(string title)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X;
			int maxWidth = Math.Min(520, screenWidth - 16);
			int titleWidth = (int)Math.Ceiling(GameService.Content.get_DefaultFont18().MeasureString(title).Width);
			return Math.Max(220, Math.Min(maxWidth, titleWidth + 4));
		}

		private static int GetWrappedTextHeight(string text, float maxWidth, BitmapFont font, int lineHeight, int maxLines)
		{
			int lineCount = (text ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n').Split('\n')
				.Sum((string line) => CountWrappedLines(line, maxWidth, font));
			return Math.Max(1, Math.Min(maxLines, lineCount)) * lineHeight;
		}

		private static int CountWrappedLines(string line, float maxWidth, BitmapFont font)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(line))
			{
				return 1;
			}
			int lineCount = 1;
			string currentLine = string.Empty;
			string[] array = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string word in array)
			{
				string candidate = (string.IsNullOrEmpty(currentLine) ? word : (currentLine + " " + word));
				if (font.MeasureString(candidate).Width <= maxWidth || string.IsNullOrEmpty(currentLine))
				{
					currentLine = candidate;
					continue;
				}
				lineCount++;
				currentLine = word;
			}
			return lineCount;
		}
	}
}
