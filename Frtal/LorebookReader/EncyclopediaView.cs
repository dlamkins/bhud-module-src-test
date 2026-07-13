using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Frtal.LorebookReader
{
	public class EncyclopediaView : View
	{
		private enum DetailMode
		{
			Empty,
			Preview,
			Edit
		}

		private readonly LorebookReaderModule _module;

		private readonly LorebookCatalog _catalog;

		private readonly TextRenderer _textRenderer;

		private readonly Texture2D _parchment;

		private Container _root;

		private TextBox _searchBox;

		private Dropdown _sortDropdown;

		private Dropdown _colorFilter;

		private Dropdown _expansionFilter;

		private FlowPanel _listPanel;

		private StandardButton _exportBtn;

		private StandardButton _importBtn;

		private Panel _detailPanel;

		private LorebookEntry _selected;

		private float _textFontSize = 18f;

		private DetailMode _mode;

		private static readonly string[] SortItems = new string[5] { "Newest first", "Oldest first", "Title A–Z", "Title Z–A", "Color" };

		private const string SoftBreak = "\u200b\n";

		public EncyclopediaView(LorebookReaderModule module, Texture2D parchment)
			: this()
		{
			_module = module;
			_catalog = module.Catalog;
			_textRenderer = module.SharedTextRenderer;
			_parchment = parchment;
		}

		protected override void Build(Container buildPanel)
		{
			_root = buildPanel;
			BuildLayout();
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				BuildLayout();
			});
		}

		private void BuildLayout()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Expected O, but got Unknown
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Expected O, but got Unknown
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Expected O, but got Unknown
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Expected O, but got Unknown
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Expected O, but got Unknown
			_root.ClearChildren();
			int totalW = _root.get_ContentRegion().Width;
			int totalH = _root.get_ContentRegion().Height;
			int leftW = Math.Max(280, (int)((float)totalW * 0.4f));
			TextBox val = new TextBox();
			((Control)val).set_Parent(_root);
			((Control)val).set_Location(new Point(8, 6));
			((Control)val).set_Width(leftW - 16);
			((TextInputBase)val).set_PlaceholderText("Search title, text, metadata…");
			_searchBox = val;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				RefreshList();
			});
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent(_root);
			((Control)val2).set_Location(new Point(8, 38));
			((Control)val2).set_Width(leftW - 16);
			_sortDropdown = val2;
			string[] sortItems = SortItems;
			foreach (string item in sortItems)
			{
				_sortDropdown.get_Items().Add(item);
			}
			_sortDropdown.set_SelectedItem(SortItems[0]);
			_sortDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RefreshList();
			});
			int halfW = (leftW - 24) / 2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Parent(_root);
			((Control)val3).set_Location(new Point(8, 70));
			((Control)val3).set_Width(halfW);
			_colorFilter = val3;
			_colorFilter.get_Items().Add("All");
			(string, int, int, int)[] colors = Palette.Colors;
			for (int i = 0; i < colors.Length; i++)
			{
				(string, int, int, int) c = colors[i];
				if (c.Item1 != "None")
				{
					_colorFilter.get_Items().Add(c.Item1);
				}
			}
			_colorFilter.get_Items().Add("None");
			_colorFilter.set_SelectedItem("All");
			_colorFilter.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RefreshList();
			});
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent(_root);
			((Control)val4).set_Location(new Point(8 + halfW + 8, 70));
			((Control)val4).set_Width(halfW);
			_expansionFilter = val4;
			RebuildExpansionFilter();
			_expansionFilter.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RefreshList();
			});
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent(_root);
			((Control)val5).set_Location(new Point(8, 102));
			((Control)val5).set_Width(leftW - 16);
			((Control)val5).set_Height(totalH - 150);
			val5.set_FlowDirection((ControlFlowDirection)3);
			val5.set_ControlPadding(new Vector2(0f, 3f));
			((Panel)val5).set_CanScroll(true);
			((Panel)val5).set_ShowBorder(true);
			_listPanel = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent(_root);
			((Control)val6).set_Location(new Point(8, totalH - 42));
			((Control)val6).set_Width(halfW);
			val6.set_Text("Export…");
			_exportBtn = val6;
			((Control)_exportBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.ExportCatalogDialog();
			});
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent(_root);
			((Control)val7).set_Location(new Point(8 + halfW + 8, totalH - 42));
			((Control)val7).set_Width(halfW);
			val7.set_Text("Import…");
			_importBtn = val7;
			((Control)_importBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.ImportCatalogDialog();
			});
			Panel val8 = new Panel();
			((Control)val8).set_Parent(_root);
			((Control)val8).set_Location(new Point(leftW + 8, 6));
			((Control)val8).set_Width(totalW - leftW - 16);
			((Control)val8).set_Height(totalH - 14);
			val8.set_ShowBorder(true);
			_detailPanel = val8;
			RefreshList();
			if (_selected != null && _mode == DetailMode.Edit)
			{
				ShowEditor(_selected);
			}
			else if (_selected != null)
			{
				ShowPreview(_selected);
			}
			else
			{
				ShowEmpty();
			}
		}

		public void RebuildExpansionFilter()
		{
			if (_expansionFilter == null)
			{
				return;
			}
			string previous = _expansionFilter.get_SelectedItem();
			_expansionFilter.get_Items().Clear();
			_expansionFilter.get_Items().Add("All");
			foreach (string exp in _catalog.DistinctExpansions())
			{
				_expansionFilter.get_Items().Add(exp);
			}
			_expansionFilter.set_SelectedItem(_expansionFilter.get_Items().Contains(previous ?? "All") ? previous : "All");
		}

		private SortMode CurrentSort()
		{
			return _sortDropdown.get_SelectedItem() switch
			{
				"Oldest first" => SortMode.OldestFirst, 
				"Title A–Z" => SortMode.TitleAZ, 
				"Title Z–A" => SortMode.TitleZA, 
				"Color" => SortMode.ColorTag, 
				_ => SortMode.NewestFirst, 
			};
		}

		public void RefreshList()
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (_listPanel == null)
			{
				return;
			}
			((Container)_listPanel).ClearChildren();
			LorebookCatalog catalog = _catalog;
			TextBox searchBox = _searchBox;
			string search = ((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null);
			SortMode sort = CurrentSort();
			Dropdown colorFilter = _colorFilter;
			string colorFilter2 = ((colorFilter != null) ? colorFilter.get_SelectedItem() : null);
			Dropdown expansionFilter = _expansionFilter;
			List<LorebookEntry> results = catalog.Query(search, sort, colorFilter2, (expansionFilter != null) ? expansionFilter.get_SelectedItem() : null);
			if (results.Count == 0)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_listPanel);
				val.set_Text("No lorebooks match. Read a book in-game to add it.");
				val.set_AutoSizeHeight(true);
				((Control)val).set_Width(((Control)_listPanel).get_Width() - 20);
				return;
			}
			foreach (LorebookEntry entry in results)
			{
				AddListRow(entry);
			}
		}

		public void RefreshFromCatalog()
		{
			RebuildExpansionFilter();
			RefreshList();
			if (_mode == DetailMode.Edit || _selected == null)
			{
				return;
			}
			LorebookEntry fresh = null;
			foreach (LorebookEntry e in _catalog.All)
			{
				if (e.Id == _selected.Id)
				{
					fresh = e;
					break;
				}
			}
			if (fresh != null)
			{
				_selected = fresh;
				ShowPreview(fresh);
			}
			else
			{
				_selected = null;
				ShowEmpty();
			}
		}

		private void AddListRow(LorebookEntry entry)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_listPanel);
			((Control)val).set_Width(((Control)_listPanel).get_Width() - 20);
			((Control)val).set_Height(46);
			val.set_ShowBorder(false);
			((Control)val).set_BackgroundColor((Color)((_selected != null && _selected.Id == entry.Id) ? new Color(60, 70, 90) : Color.get_Transparent()));
			Panel row = val;
			(int R, int G, int B) tuple = Palette.Resolve(entry.ColorTag);
			int cr = tuple.R;
			int cg = tuple.G;
			int cb = tuple.B;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(5);
			((Control)val2).set_Height(46);
			((Control)val2).set_BackgroundColor(new Color(cr, cg, cb));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			((Control)val3).set_Location(new Point(12, 4));
			((Control)val3).set_Width(((Control)row).get_Width() - 20);
			((Control)val3).set_Height(22);
			val3.set_Text(entry.DisplayTitle);
			val3.set_Font(GameService.Content.get_DefaultFont16());
			string meta = entry.MetadataLine;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			((Control)val4).set_Location(new Point(12, 26));
			((Control)val4).set_Width(((Control)row).get_Width() - 20);
			((Control)val4).set_Height(16);
			val4.set_Text(string.IsNullOrEmpty(meta) ? entry.TimestampLocal.ToString("g") : meta);
			val4.set_TextColor(new Color(160, 160, 160));
			((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_selected = entry;
				_mode = DetailMode.Preview;
				RefreshList();
				ShowPreview(entry);
			});
		}

		private void ShowEmpty()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			_mode = DetailMode.Empty;
			((Container)_detailPanel).ClearChildren();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_detailPanel);
			((Control)val).set_Location(new Point(0, ((Control)_detailPanel).get_Height() / 2 - 20));
			((Control)val).set_Width(((Control)_detailPanel).get_Width());
			((Control)val).set_Height(40);
			val.set_Text("Select a lorebook from the list");
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			val.set_TextColor(new Color(150, 150, 150));
		}

		private void ShowPreview(LorebookEntry entry)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Expected O, but got Unknown
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			_mode = DetailMode.Preview;
			((Container)_detailPanel).ClearChildren();
			int w = ((Control)_detailPanel).get_Width();
			int h = ((Control)_detailPanel).get_Height();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_detailPanel);
			((Control)val).set_Location(new Point(12, 10));
			((Control)val).set_Width(w - 24);
			((Control)val).set_Height(28);
			val.set_Text(entry.DisplayTitle);
			val.set_Font(GameService.Content.get_DefaultFont18());
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)_detailPanel);
			((Control)val2).set_Location(new Point(12, 44));
			((Control)val2).set_Width(80);
			val2.set_Text("▶ Play");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.PlayFromCatalog(entry);
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_detailPanel);
			((Control)val3).set_Location(new Point(96, 44));
			((Control)val3).set_Width(72);
			val3.set_Text("■ Stop");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StopSpeaking();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_detailPanel);
			((Control)val4).set_Location(new Point(172, 44));
			((Control)val4).set_Width(80);
			val4.set_Text("Edit");
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowEditor(entry);
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_detailPanel);
			((Control)val5).set_Location(new Point(w - 92, 44));
			((Control)val5).set_Width(80);
			val5.set_Text("Delete");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_catalog.Remove(entry.Id);
				_selected = null;
				RefreshList();
				ShowEmpty();
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_detailPanel);
			((Control)val6).set_Location(new Point(w - 92, 78));
			((Control)val6).set_Width(36);
			val6.set_Text("A−");
			StandardButton fontMinus = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_detailPanel);
			((Control)val7).set_Location(new Point(w - 52, 78));
			((Control)val7).set_Width(36);
			val7.set_Text("A+");
			StandardButton fontPlus = val7;
			string meta = entry.MetadataLine;
			if (!string.IsNullOrEmpty(meta))
			{
				Label val8 = new Label();
				((Control)val8).set_Parent((Container)(object)_detailPanel);
				((Control)val8).set_Location(new Point(12, 82));
				((Control)val8).set_Width(w - 120);
				((Control)val8).set_Height(18);
				val8.set_Text(meta);
				val8.set_TextColor(new Color(190, 190, 190));
			}
			string body = entry.Text;
			if (!string.IsNullOrEmpty(entry.TranslatedText))
			{
				body = body + "\n\n———  " + (entry.TranslatedLang ?? "translation") + "  ———\n\n" + entry.TranslatedText;
			}
			ParchmentTextPanel parchmentTextPanel = new ParchmentTextPanel(_textRenderer, _parchment);
			((Control)parchmentTextPanel).set_Parent((Container)(object)_detailPanel);
			((Control)parchmentTextPanel).set_Location(new Point(12, 106));
			((Control)parchmentTextPanel).set_Width(w - 24);
			((Control)parchmentTextPanel).set_Height(h - 118);
			parchmentTextPanel.FontSize = _textFontSize;
			ParchmentTextPanel parchment = parchmentTextPanel;
			parchment.Text = body;
			parchment.ApplyWrap();
			((Control)fontMinus).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_textFontSize = Math.Max(12f, _textFontSize - 2f);
				parchment.FontSize = _textFontSize;
			});
			((Control)fontPlus).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_textFontSize = Math.Min(40f, _textFontSize + 2f);
				parchment.FontSize = _textFontSize;
			});
		}

		private void ShowEditor(LorebookEntry entry)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Expected O, but got Unknown
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Expected O, but got Unknown
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Expected O, but got Unknown
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			_mode = DetailMode.Edit;
			((Container)_detailPanel).ClearChildren();
			int w = ((Control)_detailPanel).get_Width();
			int h = ((Control)_detailPanel).get_Height();
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)_detailPanel);
			((Control)val).set_Location(new Point(12, 10));
			((Control)val).set_Width(90);
			val.set_Text("‹ Back");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowPreview(entry);
			});
			int formW = Math.Min(280, w / 2 - 16);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)_detailPanel);
			((Control)val2).set_Location(new Point(12, 46));
			((Control)val2).set_Width(formW);
			((Control)val2).set_Height(h - 58);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(0f, 5f));
			((Panel)val2).set_CanScroll(true);
			FlowPanel form = val2;
			AddLabel((Container)(object)form, "Title");
			TextBox titleBox = AddBox((Container)(object)form, entry.Title, formW - 4);
			((TextInputBase)titleBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Title = ((TextInputBase)titleBox).get_Text();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Color tag");
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Parent((Container)(object)form);
			((Control)val3).set_Width(formW - 4);
			Dropdown colorDd = val3;
			(string, int, int, int)[] colors = Palette.Colors;
			for (int i = 0; i < colors.Length; i++)
			{
				(string, int, int, int) c = colors[i];
				colorDd.get_Items().Add(c.Item1);
			}
			colorDd.set_SelectedItem(string.IsNullOrEmpty(entry.ColorTag) ? "None" : entry.ColorTag);
			colorDd.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				entry.ColorTag = colorDd.get_SelectedItem();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Expansion");
			TextBox expBox = AddBox((Container)(object)form, entry.Expansion, formW - 4);
			((TextInputBase)expBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Expansion = ((TextInputBase)expBox).get_Text();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Theme");
			TextBox themeBox = AddBox((Container)(object)form, entry.Theme, formW - 4);
			((TextInputBase)themeBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Theme = ((TextInputBase)themeBox).get_Text();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Location acquired");
			TextBox locBox = AddBox((Container)(object)form, entry.Location, formW - 4);
			((TextInputBase)locBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Location = ((TextInputBase)locBox).get_Text();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Notes");
			TextBox notesBox = AddBox((Container)(object)form, entry.Notes, formW - 4);
			((TextInputBase)notesBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Notes = ((TextInputBase)notesBox).get_Text();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Translate & save");
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)form);
			((Control)val4).set_Width(formW - 4);
			Dropdown langDd = val4;
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int i = 0; i < targetLanguages.Length; i++)
			{
				string name = targetLanguages[i].Item2;
				langDd.get_Items().Add(name);
			}
			langDd.set_SelectedItem(NameForCode(string.IsNullOrEmpty(entry.TranslatedLang) ? _module.TranslateTargetSetting.get_Value() : entry.TranslatedLang));
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)form);
			((Control)val5).set_Width(formW - 4);
			((Control)val5).set_Height(22);
			val5.set_Text("");
			val5.set_TextColor(new Color(190, 190, 190));
			Label status = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)form);
			((Control)val6).set_Width(150);
			val6.set_Text("Translate now");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				string target = CodeForName(langDd.get_SelectedItem());
				status.set_Text("Translating…");
				Task.Run(async delegate
				{
					try
					{
						string tr = await TranslationService.TranslateAsync(entry.Text, target);
						entry.TranslatedText = tr;
						entry.TranslatedLang = target;
						Save(entry);
						status.set_Text("Saved.");
					}
					catch (Exception ex)
					{
						status.set_Text("Translation failed.");
						Logger.GetLogger<EncyclopediaView>().Warn(ex, "Translate failed.");
					}
				});
			});
			int editX = 12 + formW + 12;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_detailPanel);
			((Control)val7).set_Location(new Point(editX, 46));
			((Control)val7).set_Width(w - editX - 12);
			((Control)val7).set_Height(20);
			val7.set_Text("Book text (fix OCR errors, add page 2…)");
			val7.set_TextColor(new Color(200, 200, 200));
			int editW = w - editX - 12;
			MultilineTextBox val8 = new MultilineTextBox();
			((Control)val8).set_Parent((Container)(object)_detailPanel);
			((Control)val8).set_Location(new Point(editX, 70));
			((Control)val8).set_Width(editW);
			((Control)val8).set_Height(h - 130);
			((TextInputBase)val8).set_Text(WrapForEdit(entry.Text, editW));
			MultilineTextBox textEdit = val8;
			((TextInputBase)textEdit).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Text = UnwrapFromEdit(((TextInputBase)textEdit).get_Text());
				Save(entry);
			});
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)_detailPanel);
			((Control)val9).set_Location(new Point(editX, h - 56));
			((Control)val9).set_Width(w - editX - 12);
			((Control)val9).set_Height(40);
			val9.set_Text("Tip: editor uses a basic font without accents, but the preview shows full diacritics.");
			val9.set_TextColor(new Color(150, 150, 150));
		}

		private void Save(LorebookEntry entry)
		{
			_catalog.Update(entry);
			RefreshList();
		}

		private static void AddLabel(Container parent, string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent(parent);
			val.set_Text(text);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Width(280);
			val.set_TextColor(new Color(200, 200, 200));
		}

		private static TextBox AddBox(Container parent, string value, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(width);
			((TextInputBase)val).set_Text(value ?? "");
			return val;
		}

		private static string NameForCode(string code)
		{
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int j = 0; j < targetLanguages.Length; j++)
			{
				var (c, i) = targetLanguages[j];
				if (c == code)
				{
					return i;
				}
			}
			return TranslationService.TargetLanguages[0].Name;
		}

		private static string CodeForName(string name)
		{
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int i = 0; i < targetLanguages.Length; i++)
			{
				(string, string) tuple = targetLanguages[i];
				var (c, _) = tuple;
				if (tuple.Item2 == name)
				{
					return c;
				}
			}
			return "cs";
		}

		private string WrapForEdit(string text, int pixelWidth)
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			BitmapFont font = GameService.Content.get_DefaultFont18();
			int maxW = Math.Max(60, pixelWidth - 24);
			StringBuilder sb = new StringBuilder();
			string[] hardLines = text.Replace("\r", "").Split('\n');
			for (int li = 0; li < hardLines.Length; li++)
			{
				string[] array = hardLines[li].Split(' ');
				string current = "";
				string[] array2 = array;
				foreach (string word in array2)
				{
					string candidate = ((current.Length == 0) ? word : (current + " " + word));
					if (font.MeasureString(candidate).Width <= (float)maxW || current.Length == 0)
					{
						current = candidate;
						continue;
					}
					sb.Append(current).Append("\u200b\n");
					current = word;
				}
				sb.Append(current);
				if (li < hardLines.Length - 1)
				{
					sb.Append('\n');
				}
			}
			return sb.ToString();
		}

		private static string UnwrapFromEdit(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			return text.Replace("\u200b\n", " ").Replace("\u200b", "");
		}
	}
}
