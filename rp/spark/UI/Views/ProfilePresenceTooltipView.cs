using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	internal sealed class ProfilePresenceTooltipView : View, ITooltipView, IView
	{
		private sealed class TooltipSection
		{
			public string Title { get; }

			public List<string> Lines { get; }

			public bool WasTrimmed { get; }

			public TooltipSection(string title, List<string> lines, bool wasTrimmed)
			{
				Title = title ?? string.Empty;
				Lines = lines ?? new List<string>();
				WasTrimmed = wasTrimmed;
			}
		}

		private sealed class TooltipSectionHeader : Control
		{
			private readonly string _text;

			public TooltipSectionHeader(string text)
				: this()
			{
				_text = text ?? string.Empty;
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				BitmapFont font = GameService.Content.get_DefaultFont14();
				int textWidth = (int)Math.Ceiling(font.MeasureString(_text).Width);
				int textX = Math.Max(8, (((Control)this).get_Width() - textWidth) / 2);
				int lineY = ((Control)this).get_Height() / 2 + 1;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, lineY, Math.Max(0, textX - 8), 1), SectionLineColor);
				int rightLineX = textX + textWidth + 8;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(rightLineX, lineY, Math.Max(0, ((Control)this).get_Width() - rightLineX), 1), SectionLineColor);
				Rectangle textBounds = default(Rectangle);
				((Rectangle)(ref textBounds))._002Ector(textX, 0, textWidth + 4, ((Control)this).get_Height());
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, font, new Rectangle(textBounds.X + 1, textBounds.Y + 1, textBounds.Width, textBounds.Height), StandardColors.get_Shadow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, font, textBounds, SectionColor * 0.9f, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		private const int MinTooltipWidth = 300;

		private const int PreferredBodyWidth = 430;

		private const int MaxTooltipWidth = 570;

		private const int ScreenEdgePadding = 32;

		private const int WidthMeasurePadding = 20;

		private const int Padding = 8;

		private const int WrapSafetyPadding = 8;

		private const int TitleHeight = 25;

		private const int LineHeight = 21;

		private const int SectionHeaderHeight = 22;

		private const int SectionHeaderSideInset = 8;

		private const int SectionGap = 5;

		private static readonly Color TooltipBackground = new Color(7, 10, 12, 210);

		private static readonly Color TitleColor = new Color(255, 194, 55);

		private static readonly Color BodyColor = new Color(238, 238, 238);

		private static readonly Color SectionColor = new Color(255, 233, 180);

		private static readonly Color SectionLineColor = new Color(255, 233, 180) * 0.35f;

		private static readonly Color MoreInformationColor = new Color(255, 213, 120);

		private readonly string _characterName;

		private readonly string _characterDetails;

		private readonly string _status;

		private readonly string _location;

		private readonly string _knownFor;

		private readonly string _currently;

		private readonly string _outOfCharacter;

		private readonly bool _showKnownFor;

		private readonly bool _showCurrently;

		private readonly bool _showOutOfCharacter;

		private readonly bool _trimLongSections;

		private readonly int _maximumLinesPerSection;

		private readonly IReadOnlyList<string> _additionalDetailLines;

		private readonly string _additionalSectionTitle;

		private readonly string _additionalSectionText;

		public ProfilePresenceTooltipView(string characterName, string characterDetails, string status, string location, string knownFor, string currently, string outOfCharacter, bool showKnownFor, bool showCurrently, bool showOutOfCharacter, bool trimLongSections, int maximumLinesPerSection, IEnumerable<string> additionalDetailLines = null, string additionalSectionTitle = null, string additionalSectionText = null)
			: this()
		{
			_characterName = Clean(characterName);
			_characterDetails = Clean(characterDetails);
			_status = Clean(status);
			_location = Clean(location);
			_knownFor = Clean(knownFor);
			_currently = Clean(currently);
			_outOfCharacter = Clean(outOfCharacter);
			_showKnownFor = showKnownFor;
			_showCurrently = showCurrently;
			_showOutOfCharacter = showOutOfCharacter;
			_trimLongSections = trimLongSections;
			_maximumLinesPerSection = SparkSettings.ProfileTooltipLimit(maximumLinesPerSection);
			_additionalDetailLines = (from line in (additionalDetailLines ?? Enumerable.Empty<string>()).Select(Clean)
				where !string.IsNullOrWhiteSpace(line)
				select line).ToList();
			_additionalSectionTitle = Clean(additionalSectionTitle);
			_additionalSectionText = Clean(additionalSectionText);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			int tooltipWidth = GetTooltipWidth();
			int contentWidth = tooltipWidth - 16;
			int wrapWidth = contentWidth - 8;
			BitmapFont bodyFont = GameService.Content.get_DefaultFont14();
			List<string> detailLines = BuildDetailLines().SelectMany((string line) => TooltipTextLayout.WrapLines(line, wrapWidth, bodyFont)).ToList();
			TooltipSection knownForSection = BuildSection("Known For", _showKnownFor ? _knownFor : string.Empty, wrapWidth, bodyFont);
			TooltipSection currentlySection = BuildSection("Currently", _showCurrently ? _currently : string.Empty, wrapWidth, bodyFont);
			TooltipSection outOfCharacterSection = BuildSection("Out of Character", _showOutOfCharacter ? _outOfCharacter : string.Empty, wrapWidth, bodyFont);
			TooltipSection additionalSection = BuildSection(_additionalSectionTitle, _additionalSectionText, wrapWidth, bodyFont);
			TooltipSection[] sections = new TooltipSection[4] { knownForSection, currentlySection, outOfCharacterSection, additionalSection };
			int height = 8;
			if (!string.IsNullOrWhiteSpace(_characterName))
			{
				height += 25;
			}
			height += detailLines.Count * 21;
			TooltipSection[] array = sections;
			foreach (TooltipSection section in array)
			{
				height += GetSectionHeight(section);
			}
			height += 8;
			((Control)buildPanel).set_Size(new Point(tooltipWidth, height));
			((Control)buildPanel).set_BackgroundColor(TooltipBackground);
			Panel tooltipPanel = (Panel)(object)((buildPanel is Panel) ? buildPanel : null);
			if (tooltipPanel != null)
			{
				tooltipPanel.set_ShowBorder(true);
			}
			int y = 8;
			if (!string.IsNullOrWhiteSpace(_characterName))
			{
				Label val = new Label();
				val.set_Text(_characterName);
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_TextColor(TitleColor);
				val.set_StrokeText(false);
				val.set_WrapText(false);
				val.set_ShowShadow(true);
				((Control)val).set_Location(new Point(8, y));
				((Control)val).set_Size(new Point(contentWidth, 25));
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_ZIndex(1);
				y += 25;
			}
			y = AddLines(buildPanel, detailLines, y, contentWidth);
			array = sections;
			foreach (TooltipSection section2 in array)
			{
				y = AddSection(buildPanel, section2, y, contentWidth);
			}
		}

		private int GetTooltipWidth()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X;
			int availableWidth = Math.Max(220, screenWidth - 32);
			int maximumWidth = Math.Min(570, availableWidth);
			int minimumWidth = Math.Min(300, maximumWidth);
			BitmapFont titleFont = GameService.Content.get_DefaultFont16();
			BitmapFont bodyFont = GameService.Content.get_DefaultFont14();
			float measuredWidth = 0f;
			if (!string.IsNullOrWhiteSpace(_characterName))
			{
				measuredWidth = Math.Max(measuredWidth, titleFont.MeasureString(_characterName).Width);
			}
			foreach (string detailLine in BuildDetailLines())
			{
				measuredWidth = Math.Max(measuredWidth, bodyFont.MeasureString(detailLine).Width);
			}
			bool num = (_showKnownFor && !string.IsNullOrWhiteSpace(_knownFor)) || (_showCurrently && !string.IsNullOrWhiteSpace(_currently)) || (_showOutOfCharacter && !string.IsNullOrWhiteSpace(_outOfCharacter));
			int desiredWidth = (int)Math.Ceiling(measuredWidth) + 16 + 20;
			if (num)
			{
				desiredWidth = Math.Max(desiredWidth, 430);
			}
			return Math.Max(minimumWidth, Math.Min(maximumWidth, desiredWidth));
		}

		[IteratorStateMachine(typeof(_003CBuildDetailLines_003Ed__36))]
		private IEnumerable<string> BuildDetailLines()
		{
			return new _003CBuildDetailLines_003Ed__36(-2)
			{
				_003C_003E4__this = this
			};
		}

		private TooltipSection BuildSection(string title, string text, float wrapWidth, BitmapFont font)
		{
			List<string> allLines = TooltipTextLayout.WrapLines(text, wrapWidth, font).ToList();
			bool wasTrimmed;
			List<string> visibleLines = TrimSection(allLines, out wasTrimmed);
			return new TooltipSection(title, visibleLines, wasTrimmed);
		}

		private List<string> TrimSection(List<string> lines, out bool wasTrimmed)
		{
			wasTrimmed = _trimLongSections && lines.Count > _maximumLinesPerSection;
			if (!wasTrimmed)
			{
				return lines;
			}
			return lines.Take(_maximumLinesPerSection).ToList();
		}

		private static int GetSectionHeight(TooltipSection section)
		{
			if (section == null || section.Lines.Count == 0)
			{
				return 0;
			}
			int height = 27 + section.Lines.Count * 21;
			if (section.WasTrimmed)
			{
				height += 21;
			}
			return height;
		}

		private static int AddSection(Container parent, TooltipSection section, int y, int width)
		{
			if (section == null || section.Lines.Count == 0)
			{
				return y;
			}
			y += 5;
			AddSectionHeader(parent, section.Title, y, width);
			y += 22;
			y = AddLines(parent, section.Lines, y, width);
			if (section.WasTrimmed)
			{
				y = AddMoreInformationLine(parent, y, width);
			}
			return y;
		}

		private static int AddMoreInformationLine(Container parent, int y, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("(Exceeded max tooltip lines - open profile to read all)");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(MoreInformationColor);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			val.set_WrapText(false);
			val.set_ShowShadow(true);
			((Control)val).set_Location(new Point(8, y));
			((Control)val).set_Size(new Point(width, 21));
			((Control)val).set_Parent(parent);
			((Control)val).set_ZIndex(1);
			return y + 21;
		}

		private static int AddLines(Container parent, IEnumerable<string> lines, int y, int width)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			foreach (string line in lines)
			{
				Label val = new Label();
				val.set_Text(line);
				val.set_Font(GameService.Content.get_DefaultFont14());
				val.set_TextColor(BodyColor);
				val.set_WrapText(false);
				val.set_ShowShadow(true);
				((Control)val).set_Location(new Point(8, y));
				((Control)val).set_Size(new Point(width, 21));
				((Control)val).set_Parent(parent);
				((Control)val).set_ZIndex(1);
				y += 21;
			}
			return y;
		}

		private static void AddSectionHeader(Container parent, string text, int y, int width)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			TooltipSectionHeader tooltipSectionHeader = new TooltipSectionHeader(text);
			((Control)tooltipSectionHeader).set_Location(new Point(16, y));
			((Control)tooltipSectionHeader).set_Size(new Point(Math.Max(0, width - 16), 22));
			((Control)tooltipSectionHeader).set_Parent(parent);
			((Control)tooltipSectionHeader).set_ZIndex(1);
		}

		private static string Clean(string text)
		{
			return text?.Trim() ?? string.Empty;
		}
	}
}
