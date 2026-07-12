using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
		private const int MinWidth = 270;

		private const int MaxWidth = 390;

		private const int Padding = 7;

		private const int TitleHeight = 23;

		private const int DescriptionGap = 2;

		private const int DescriptionLineHeight = 22;

		private const int DescriptionMaxLines = 12;

		private const int WrapMeasurePadding = 14;

		private static readonly Color TooltipBackground = new Color(7, 10, 12, 190);

		private static readonly Color TitleColor = new Color(255, 194, 55);

		private static readonly Color DescriptionColor = new Color(238, 238, 238);

		private readonly string _title;

		private readonly string _description;

		public ProfileTooltipView(string title, string description, string fallbackTitle = "Profile")
			: this()
		{
			_title = ((!string.IsNullOrWhiteSpace(title)) ? title.Trim() : (fallbackTitle?.Trim() ?? string.Empty));
			_description = description?.Trim() ?? string.Empty;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			bool hasTitle = !string.IsNullOrWhiteSpace(_title);
			bool num = !string.IsNullOrWhiteSpace(_description);
			int tooltipWidth = GetTooltipWidth(_title);
			int contentWidth = tooltipWidth - 14;
			List<string> descriptionLines = (num ? WrapTextLines(_description, contentWidth - 14, GameService.Content.get_DefaultFont16()).Take(12).ToList() : new List<string>());
			int descriptionHeight = descriptionLines.Count * 22;
			int height = 7;
			if (hasTitle)
			{
				height += 23;
			}
			if (num)
			{
				if (hasTitle)
				{
					height += 2;
				}
				height += descriptionHeight;
			}
			height += 7;
			((Control)buildPanel).set_Size(new Point(tooltipWidth, height));
			((Control)buildPanel).set_BackgroundColor(TooltipBackground);
			Panel tooltipPanel = (Panel)(object)((buildPanel is Panel) ? buildPanel : null);
			if (tooltipPanel != null)
			{
				tooltipPanel.set_ShowBorder(true);
			}
			int y = 7;
			if (hasTitle)
			{
				Label val = new Label();
				val.set_Text(_title);
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_TextColor(TitleColor);
				val.set_StrokeText(false);
				val.set_WrapText(false);
				val.set_ShowShadow(true);
				((Control)val).set_Location(new Point(7, y));
				((Control)val).set_Size(new Point(contentWidth, 23));
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_ZIndex(1);
				y += 23;
			}
			if (!num)
			{
				return;
			}
			if (hasTitle)
			{
				y += 2;
			}
			foreach (string line in descriptionLines)
			{
				Label val2 = new Label();
				val2.set_Text(line);
				val2.set_Font(GameService.Content.get_DefaultFont14());
				val2.set_TextColor(DescriptionColor);
				val2.set_WrapText(false);
				val2.set_ShowShadow(true);
				((Control)val2).set_Location(new Point(7, y));
				((Control)val2).set_Size(new Point(contentWidth, 22));
				((Control)val2).set_Parent(buildPanel);
				((Control)val2).set_ZIndex(1);
				y += 22;
			}
		}

		private static int GetTooltipWidth(string title)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X;
			int maxWidth = Math.Max(270, Math.Min(390, screenWidth - 16));
			int titleWidth = (int)Math.Ceiling(GameService.Content.get_DefaultFont16().MeasureString(title ?? string.Empty).Width) + 14;
			return Math.Max(270, Math.Min(maxWidth, titleWidth));
		}

		[IteratorStateMachine(typeof(_003CWrapTextLines_003Ed__16))]
		private static IEnumerable<string> WrapTextLines(string text, float maxWidth, BitmapFont font)
		{
			return new _003CWrapTextLines_003Ed__16(-2)
			{
				_003C_003E3__text = text,
				_003C_003E3__maxWidth = maxWidth,
				_003C_003E3__font = font
			};
		}
	}
}
