using System;
using System.Diagnostics;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using SongbookOfTyria.UI.Controls.Notation;

namespace SongbookOfTyria.UI.Views
{
	public class AboutView : View
	{
		private const int LeftPadding = 70;

		private const int DefaultSpacing = 8;

		private const int SectionSpacing = 20;

		private Panel _scrollPanel;

		private FlowPanel _aboutPanel;

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(true);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			((Control)val).set_Parent(buildPanel);
			_scrollPanel = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Control)val2).set_Width(((Container)_scrollPanel).get_ContentRegion().Width - 70 - 50);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Location(new Point(70, 0));
			((Control)val2).set_Parent((Container)(object)_scrollPanel);
			val2.set_ControlPadding(new Vector2(0f, 8f));
			val2.set_OuterControlPadding(new Vector2(0f, 8f));
			_aboutPanel = val2;
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)OnBuildPanelResized);
			AddSectionHeader((Container)(object)_aboutPanel, "Welcome to Songbook of Tyria");
			AddParagraphWithLinks((Container)(object)_aboutPanel, "This songbook is intended for manual music playing, and is directly linked to [OPUS] Divinity's Philharmonic Orchestra's guild ", "songbook", "https://www.gw2opus.com/songbook/", " and displays our publicly available solo and band tabs.");
			AddSpacer((Container)(object)_aboutPanel, 20);
			AddSectionHeader((Container)(object)_aboutPanel, "How to Read Our Notation");
			AddSubHeader((Container)(object)_aboutPanel, "Notes");
			AddCodeBlock((Container)(object)_aboutPanel, "1  2  3  4  5  6  7  8");
			AddParagraph((Container)(object)_aboutPanel, "Regular numbers are notes from your skill bar. You can create a separate keybind profile for playing music without hindering your ability to play the game.");
			AddCodeBlock((Container)(object)_aboutPanel, "①  ②  ③  ④  ⑤", useLargerFont: true);
			AddParagraph((Container)(object)_aboutPanel, "Circled numbers are F1, F2, F3, F4 and F5 keys from piano, which are sharps/black keys on a real piano.");
			AddNote((Container)(object)_aboutPanel, "Note: If you are used to different sharps notation, you can directly change these to your preferences.");
			AddSubHeader((Container)(object)_aboutPanel, "Octaves");
			AddCodeBlock((Container)(object)_aboutPanel, "[ 1 2 3 ]   1 2 3   ( 1 2 3 )");
			AddParagraph((Container)(object)_aboutPanel, "Square brackets [ ] represent low octave, no brackets is medium octave, and round brackets ( ) are high octave.");
			AddSubHeader((Container)(object)_aboutPanel, "Bars");
			AddCodeBlock((Container)(object)_aboutPanel, "| 1 2 3 4 |");
			AddParagraph((Container)(object)_aboutPanel, "Vertical lines on the sides show boundaries of a single bar (4 beats in 4/4 time signature).");
			AddSubHeader((Container)(object)_aboutPanel, "Lines");
			AddCodeBlock((Container)(object)_aboutPanel, "| 1 2 3 4 | 1 2 3 4 | 1 2 3 4 | 1 2 3 4 |");
			AddParagraph((Container)(object)_aboutPanel, "One line consists of several bars.");
			AddSubHeader((Container)(object)_aboutPanel, "Sections");
			AddCodeBlock((Container)(object)_aboutPanel, "A\n| 1 2 3 4 | 1 2 3 4 | 1 2 3 4 | 1 2 3 4 |\nB\n| 5 6 7 8 | 5 6 7 8 | 5 6 7 8 | 5 6 7 8 |\nC\n| (1 2 3 4) | (1 2 3 4) | (1 2 3 4) |");
			AddParagraph((Container)(object)_aboutPanel, "Letters (A, B, C, D) represent different parts of the song, such as introduction, verse, chorus, and usually just separate groups of lines for better readability. One section consists of several lines of bars.");
			AddSpacer((Container)(object)_aboutPanel, 20);
			AddSectionHeader((Container)(object)_aboutPanel, "Notation Symbols & Spacing");
			AddParagraph((Container)(object)_aboutPanel, "Often the notation contains more than just 4 simple groups of notes. In this case you would have to adjust tempo accordingly. Additionally, there are symbols that represent empty notes, breaks, pauses and arpeggios (notes intended to be rolled quickly). Notes are often grouped together and spaced from each other, representing the tempo of a song.");
			AddSubHeader((Container)(object)_aboutPanel, "Example");
			AddCodeBlock((Container)(object)_aboutPanel, "| 1 2 -3 45 -6 | 12 3/4/5 – 78 | ~123 -45 – 888 |");
			AddSubHeader((Container)(object)_aboutPanel, "Symbol Reference");
			AddSymbolRow((Container)(object)_aboutPanel, "1/3/5", "Slashes between numbers represent a chord - these notes are meant to be pressed together.");
			AddSymbolRow((Container)(object)_aboutPanel, "[3/5]/3/5/(3/5)", "Multi-octave chord. Roll from low to high as quickly as possible, similar to an arpeggio.");
			AddSymbolRow((Container)(object)_aboutPanel, "-3", "A minus sign followed by a note shows an offbeat note, played later than the beat.");
			AddSymbolRow((Container)(object)_aboutPanel, "–", "An empty note/full beat rest. Example: | 1 – 3 – | shows every other beat being empty.");
			AddSymbolRow((Container)(object)_aboutPanel, "~123", "Arpeggio - three notes rolled quickly together, almost like a chord but slower than a triplet. Also written as 1.2.3 or ~123~");
			AddNote((Container)(object)_aboutPanel, "Some tabs have highlighted notation for easier visibility of triplets and arpeggios.");
			AddSpacer((Container)(object)_aboutPanel, 20);
			AddSectionHeader((Container)(object)_aboutPanel, "Need More Help?");
			AddParagraphWithLinks((Container)(object)_aboutPanel, "For more tips and a detailed ", "'How to Play'", "https://www.gw2opus.com/how-to-play/", " guide, you can visit our ", "website", "https://www.gw2opus.com/", " or feel free to reach out to our guild on Discord.");
		}

		private void OnBuildPanelResized(object sender, ResizedEventArgs e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (_scrollPanel == null)
			{
				return;
			}
			Container container = (Container)((sender is Container) ? sender : null);
			if (container != null)
			{
				((Control)_scrollPanel).set_Width(container.get_ContentRegion().Width);
				((Control)_scrollPanel).set_Height(container.get_ContentRegion().Height);
				if (_aboutPanel != null)
				{
					((Control)_aboutPanel).set_Width(((Container)_scrollPanel).get_ContentRegion().Width - 70 - 50);
				}
			}
		}

		protected override void Unload()
		{
			Panel scrollPanel = _scrollPanel;
			if (((scrollPanel != null) ? ((Control)scrollPanel).get_Parent() : null) != null)
			{
				((Control)((Control)_scrollPanel).get_Parent()).remove_Resized((EventHandler<ResizedEventArgs>)OnBuildPanelResized);
			}
			((View<IPresenter>)this).Unload();
		}

		private static void AddSectionHeader(Container parent, string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0));
			val.set_TextColor(new Color(255, 200, 100));
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Parent(parent);
		}

		private static void AddSubHeader(Container parent, string text)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			AddSpacer(parent, 3);
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(new Color(180, 220, 255));
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Parent(parent);
		}

		private static void AddParagraph(Container parent, string text, Color? color = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor((Color)(((_003F?)color) ?? Color.get_White()));
			val.set_AutoSizeWidth(false);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			((Control)val).set_Width(Math.Max(100, ((Control)parent).get_Width() - 20));
			((Control)val).set_Parent(parent);
			Label label = val;
			((Control)parent).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)label).set_Width(Math.Max(100, ((Control)parent).get_Width() - 20));
			});
		}

		private static void AddParagraphWithLinks(Container parent, params object[] segments)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)0);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			val.set_ControlPadding(new Vector2(0f, 0f));
			((Control)val).set_Parent(parent);
			FlowPanel flowPanel = val;
			BitmapFont font = GameService.Content.get_DefaultFont16();
			Color linkColor = default(Color);
			((Color)(ref linkColor))._002Ector(100, 200, 255);
			int i = 0;
			while (i < segments.Length)
			{
				string text = segments[i] as string;
				if (text != null && i + 2 < segments.Length)
				{
					string linkText = segments[i + 1] as string;
					if (linkText != null)
					{
						string url = segments[i + 2] as string;
						if (url != null && url.StartsWith("http"))
						{
							AddInlineWords((Container)(object)flowPanel, text, font, Color.get_White());
							AddInlineLink((Container)(object)flowPanel, linkText, font, linkColor, url);
							i += 3;
							continue;
						}
					}
				}
				if (text != null)
				{
					AddInlineWords((Container)(object)flowPanel, text, font, Color.get_White());
					i++;
				}
				else
				{
					i++;
				}
			}
		}

		private static void AddInlineWords(Container parent, string text, BitmapFont font, Color color)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] words = text.Split(' ');
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Length != 0 || i <= 0)
				{
					string word = words[i];
					if (i < words.Length - 1)
					{
						word += " ";
					}
					Label val = new Label();
					val.set_Text(word);
					val.set_Font(font);
					val.set_TextColor(color);
					val.set_AutoSizeWidth(true);
					val.set_AutoSizeHeight(true);
					((Control)val).set_Parent(parent);
				}
			}
		}

		private static void AddInlineLink(Container parent, string text, BitmapFont font, Color color, string url)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(font);
			val.set_TextColor(color);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_BasicTooltipText(url);
			((Control)val).set_Parent(parent);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = url,
						UseShellExecute = true
					});
				}
				catch (Exception ex)
				{
					Logger.GetLogger<AboutView>().Warn(ex, "Failed to open URL: {Url}", new object[1] { url });
				}
			});
		}

		private static void AddCodeBlock(Container parent, string text, bool useLargerFont = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_BackgroundColor(new Color(20, 20, 25));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Parent(parent);
			Panel codePanel = val;
			BitmapFont font = NotationRenderer.GetFont(useLargerFont ? 30 : 22) ?? NotationRenderer.GetFont(22);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			val2.set_OuterControlPadding(new Vector2(15f, 6f));
			((Control)val2).set_Parent((Container)(object)codePanel);
			FlowPanel verticalFlowPanel = val2;
			string[] array = text.Split('\n');
			foreach (string line in array)
			{
				FlowPanel val3 = new FlowPanel();
				val3.set_FlowDirection((ControlFlowDirection)2);
				((Container)val3).set_HeightSizingMode((SizingMode)1);
				((Container)val3).set_WidthSizingMode((SizingMode)2);
				((Control)val3).set_Parent((Container)(object)verticalFlowPanel);
				RenderColorizedLine((Container)val3, line, font);
			}
		}

		private static void AddSymbolRow(Container parent, string symbol, string description)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			val.set_ControlPadding(new Vector2(10f, 0f));
			((Control)val).set_Parent(parent);
			FlowPanel rowPanel = val;
			BitmapFont font = NotationRenderer.GetFont(22);
			Panel val2 = new Panel();
			((Control)val2).set_BackgroundColor(new Color(20, 20, 25));
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(200);
			((Control)val2).set_Parent((Container)(object)rowPanel);
			Panel symbolPanel = val2;
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			val3.set_OuterControlPadding(new Vector2(10f, 4f));
			((Control)val3).set_Parent((Container)(object)symbolPanel);
			RenderColorizedLine((Container)val3, symbol, font);
			Label val4 = new Label();
			val4.set_Text(description);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_TextColor(Color.get_White());
			val4.set_AutoSizeWidth(false);
			val4.set_AutoSizeHeight(true);
			val4.set_WrapText(true);
			((Control)val4).set_Width(Math.Max(100, ((Control)parent).get_Width() - 240));
			((Control)val4).set_Parent((Container)(object)rowPanel);
			Label descLabel = val4;
			((Control)rowPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)descLabel).set_Width(Math.Max(100, ((Control)rowPanel).get_Width() - 220));
			});
		}

		private static void RenderColorizedLine(Container parent, string line, BitmapFont font)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			Color pipeColor = default(Color);
			((Color)(ref pipeColor))._002Ector(107, 255, 107);
			Color lowOctaveColor = default(Color);
			((Color)(ref lowOctaveColor))._002Ector(107, 181, 255);
			Color highOctaveColor = default(Color);
			((Color)(ref highOctaveColor))._002Ector(255, 107, 107);
			Color defaultColor = default(Color);
			((Color)(ref defaultColor))._002Ector(240, 240, 240);
			bool inLowOctave = false;
			bool inHighOctave = false;
			StringBuilder currentText = new StringBuilder();
			Color currentColor = defaultColor;
			foreach (char c in line)
			{
				Color charColor;
				switch (c)
				{
				case '|':
					charColor = pipeColor;
					break;
				case '[':
					inLowOctave = true;
					charColor = lowOctaveColor;
					break;
				case ']':
					charColor = lowOctaveColor;
					inLowOctave = false;
					break;
				case '(':
					inHighOctave = true;
					charColor = highOctaveColor;
					break;
				case ')':
					charColor = highOctaveColor;
					inHighOctave = false;
					break;
				default:
					charColor = ((!inLowOctave) ? ((!inHighOctave) ? defaultColor : highOctaveColor) : lowOctaveColor);
					break;
				}
				if (charColor != currentColor && currentText.Length > 0)
				{
					Label val = new Label();
					val.set_Text(currentText.ToString());
					val.set_Font(font);
					val.set_TextColor(currentColor);
					val.set_AutoSizeWidth(true);
					val.set_AutoSizeHeight(true);
					((Control)val).set_Parent(parent);
					currentText.Clear();
				}
				currentColor = charColor;
				currentText.Append(c);
			}
			if (currentText.Length > 0)
			{
				Label val2 = new Label();
				val2.set_Text(currentText.ToString());
				val2.set_Font(font);
				val2.set_TextColor(currentColor);
				val2.set_AutoSizeWidth(true);
				val2.set_AutoSizeHeight(true);
				((Control)val2).set_Parent(parent);
			}
		}

		private static void AddNote(Container parent, string text)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Parent(parent);
			Panel notePanel = val;
			Label val2 = new Label();
			val2.set_Text("*" + text);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_White());
			val2.set_AutoSizeWidth(false);
			val2.set_AutoSizeHeight(true);
			val2.set_WrapText(true);
			((Control)val2).set_Width(Math.Max(100, ((Control)parent).get_Width() - 100));
			((Control)val2).set_Location(new Point(20, 0));
			((Control)val2).set_Parent((Container)(object)notePanel);
			Label noteLabel = val2;
			((Control)notePanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)noteLabel).set_Width(Math.Max(100, ((Control)notePanel).get_Width() - 40));
			});
		}

		private static void AddSpacer(Container parent, int height)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Height(height);
			((Control)val).set_Width(1);
			((Control)val).set_Parent(parent);
		}

		public AboutView()
			: this()
		{
		}
	}
}
