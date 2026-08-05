using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Glide;
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

		private Panel _railPanel;

		private string _filterXp;

		private FlowPanel _listPanel;

		private StandardButton _exportBtn;

		private StandardButton _importBtn;

		private Panel _detailPanel;

		private LorebookEntry _selected;

		private float _textFontSize = 18f;

		private bool _readerFullscreen;

		private string _lastSearch;

		private Timer _editFlushTimer;

		private volatile bool _editDirty;

		private DetailMode _mode;

		private static readonly string[] SortItems = new string[5] { "Newest first", "Oldest first", "Title A–Z", "Title Z–A", "Color" };

		private static readonly (string Name, string Code)[] RailPresets = new(string, string)[8]
		{
			("Core", "GW2"),
			("Heart of Thorns", "HoT"),
			("Path of Fire", "PoF"),
			("Icebrood Saga", "IBS"),
			("End of Dragons", "EoD"),
			("Secrets of the Obscure", "SotO"),
			("Janthir Wilds", "JW"),
			("Visions of Eternity", "VoE")
		};

		private const int ListRowH = 58;

		internal static readonly string[] ExpansionPresets = new string[9] { "(none)", "Core", "Heart of Thorns", "Path of Fire", "Icebrood Saga", "End of Dragons", "Secrets of the Obscure", "Janthir Wilds", "Visions of Eternity" };

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
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Expected O, but got Unknown
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Expected O, but got Unknown
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Expected O, but got Unknown
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Expected O, but got Unknown
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Expected O, but got Unknown
			FlushEdits();
			_root.ClearChildren();
			int totalW = _root.get_ContentRegion().Width;
			int totalH = _root.get_ContentRegion().Height;
			if (_readerFullscreen && _selected != null)
			{
				BuildFullscreenReader(totalW, totalH);
				return;
			}
			_readerFullscreen = false;
			if (_mode == DetailMode.Edit && _selected != null)
			{
				ShowEditor(_selected, totalW, totalH);
				return;
			}
			int railW = (_module.EncyclopediaRailCollapsedSetting.get_Value() ? 64 : 268);
			int listX = 8 + railW + 8;
			int leftW = Math.Max(250, (int)((float)(totalW - railW) * 0.4f));
			Panel val = new Panel();
			((Control)val).set_Parent(_root);
			((Control)val).set_Location(new Point(8, 6));
			((Control)val).set_Width(railW);
			((Control)val).set_Height(totalH - 14);
			val.set_ShowBorder(true);
			_railPanel = val;
			FillRail();
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent(_root);
			((Control)val2).set_Location(new Point(listX, 6));
			((Control)val2).set_Width(leftW - 8);
			((TextInputBase)val2).set_PlaceholderText("Search title, text, metadata…");
			_searchBox = val2;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				RefreshList();
			});
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Parent(_root);
			((Control)val3).set_Location(new Point(listX, 38));
			((Control)val3).set_Width(leftW - 8);
			_sortDropdown = val3;
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
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent(_root);
			((Control)val4).set_Location(new Point(listX, 70));
			((Control)val4).set_Width(leftW - 8);
			_colorFilter = val4;
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
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent(_root);
			((Control)val5).set_Location(new Point(listX, 102));
			((Control)val5).set_Width(leftW - 8);
			((Control)val5).set_Height(totalH - 150);
			val5.set_FlowDirection((ControlFlowDirection)3);
			val5.set_ControlPadding(new Vector2(0f, 3f));
			((Panel)val5).set_CanScroll(true);
			((Panel)val5).set_ShowBorder(true);
			_listPanel = val5;
			int halfW = (leftW - 16) / 2;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent(_root);
			((Control)val6).set_Location(new Point(listX, totalH - 42));
			((Control)val6).set_Width(halfW);
			val6.set_Text("Export…");
			_exportBtn = val6;
			((Control)_exportBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.ExportCatalogDialog();
			});
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent(_root);
			((Control)val7).set_Location(new Point(listX + halfW + 8, totalH - 42));
			((Control)val7).set_Width(halfW);
			val7.set_Text("Import…");
			_importBtn = val7;
			((Control)_importBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.ImportCatalogDialog();
			});
			int detailX = listX + leftW + 8;
			Panel val8 = new Panel();
			((Control)val8).set_Parent(_root);
			((Control)val8).set_Location(new Point(detailX, 6));
			((Control)val8).set_Width(totalW - detailX - 8);
			((Control)val8).set_Height(totalH - 14);
			val8.set_ShowBorder(true);
			_detailPanel = val8;
			RefreshList();
			if (_selected != null)
			{
				ShowPreview(_selected);
			}
			else
			{
				ShowEmpty();
			}
		}

		public void FillRail()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (_railPanel == null)
			{
				return;
			}
			((Container)_railPanel).ClearChildren();
			bool mini = _module.EncyclopediaRailCollapsedSetting.get_Value();
			int rowW = ((Control)_railPanel).get_Width() - 8;
			int y = 4;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)_railPanel);
			((Control)val).set_Location(new Point(4, y));
			((Control)val).set_Width(rowW);
			val.set_Text(mini ? "»" : "« Minimize");
			((Control)val).set_BasicTooltipText(mini ? "Expand expansion rail" : "Collapse to icons only");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.EncyclopediaRailCollapsedSetting.set_Value(!mini);
				BuildLayout();
			});
			y += 34;
			Dictionary<string, int> counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			int noXp = 0;
			int total = 0;
			foreach (LorebookEntry item in _catalog.All)
			{
				total++;
				string xp = (item.Expansion ?? "").Trim();
				if (xp.Length == 0)
				{
					noXp++;
				}
				else
				{
					counts[xp] = (counts.TryGetValue(xp, out var c2) ? c2 : 0) + 1;
				}
			}
			y = AddRailRow(y, rowW, mini, null, "All books", "ALL", total);
			(string, string)[] railPresets = RailPresets;
			for (int i = 0; i < railPresets.Length; i++)
			{
				var (name, code) = railPresets[i];
				counts.TryGetValue(name, out var c);
				counts.Remove(name);
				y = AddRailRow(y, rowW, mini, name, name, code, c);
			}
			counts.Remove("Short Stories");
			foreach (KeyValuePair<string, int> kv in counts.OrderBy((KeyValuePair<string, int> k) => k.Key))
			{
				y = AddRailRow(y, rowW, mini, kv.Key, kv.Key, (kv.Key.Length <= 4) ? kv.Key : (kv.Key.Substring(0, 3) + "…"), kv.Value);
			}
			counts.TryGetValue("Short Stories", out var storyCount);
			counts.Remove("Short Stories");
			y = AddRailRow(y, rowW, mini, "Short Stories", "Short Stories", "SS", storyCount);
			if (noXp > 0)
			{
				AddRailRow(y, rowW, mini, "", "No expansion", "—", noXp);
			}
		}

		private int AddRailRow(int y, int rowW, bool mini, string value, string label, string code, int count)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			bool active = _filterXp == value;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_railPanel);
			((Control)val).set_Location(new Point(4, y));
			((Control)val).set_Width(rowW);
			((Control)val).set_Height(46);
			((Control)val).set_BackgroundColor((Color)(active ? new Color(60, 70, 90) : Color.get_Transparent()));
			((Control)val).set_BasicTooltipText($"{label} ({count})");
			Panel row = val;
			if (count == 0 && value != null)
			{
				((Control)row).set_Opacity(0.45f);
			}
			Texture2D icon = ((value == null || value.Length == 0) ? null : _module.GetExpansionIcon(value));
			int tx;
			if (icon != null)
			{
				Image val2 = new Image(AsyncTexture2D.op_Implicit(icon));
				((Control)val2).set_Parent((Container)(object)row);
				((Control)val2).set_Location(new Point(mini ? ((rowW - 34) / 2) : 6, 6));
				((Control)val2).set_Size(new Point(34, 34));
				tx = 46;
			}
			else
			{
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)row);
				((Control)val3).set_Location(new Point((!mini) ? 6 : 0, 12));
				((Control)val3).set_Width(mini ? rowW : 36);
				((Control)val3).set_Height(22);
				val3.set_Text(code);
				val3.set_HorizontalAlignment((HorizontalAlignment)(mini ? 1 : 0));
				val3.set_Font(GameService.Content.get_DefaultFont16());
				tx = 48;
			}
			if (!mini)
			{
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)row);
				((Control)val4).set_Location(new Point(tx, 12));
				((Control)val4).set_Width(rowW - tx - 46 - 6);
				((Control)val4).set_Height(22);
				val4.set_Text(label);
				val4.set_Font(GameService.Content.get_DefaultFont16());
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)row);
				((Control)val5).set_Location(new Point(rowW - 46 - 4, 12));
				((Control)val5).set_Width(46);
				((Control)val5).set_Height(22);
				val5.set_Text(count.ToString());
				val5.set_HorizontalAlignment((HorizontalAlignment)2);
				val5.set_TextColor(new Color(170, 170, 170));
				val5.set_Font(GameService.Content.get_DefaultFont16());
			}
			((Control)row).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (!active)
				{
					((Control)row).set_BackgroundColor(new Color(50, 56, 68));
				}
			});
			((Control)row).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				if (!active)
				{
					((Control)row).set_BackgroundColor(Color.get_Transparent());
				}
			});
			((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_filterXp = value;
				FillRail();
				RefreshList();
			});
			return y + 46 + 4;
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
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			if (_listPanel == null || _mode == DetailMode.Edit)
			{
				return;
			}
			foreach (Control item in ((Container)_listPanel).get_Children().ToList())
			{
				item.Dispose();
			}
			((Container)_listPanel).ClearChildren();
			LorebookCatalog catalog = _catalog;
			TextBox searchBox = _searchBox;
			string search = ((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null);
			SortMode sort = CurrentSort();
			Dropdown colorFilter = _colorFilter;
			List<LorebookEntry> results = catalog.Query(search, sort, (colorFilter != null) ? colorFilter.get_SelectedItem() : null);
			if (_filterXp != null)
			{
				results = ((_filterXp.Length == 0) ? results.Where((LorebookEntry e) => string.IsNullOrWhiteSpace(e.Expansion)).ToList() : results.Where((LorebookEntry e) => string.Equals(e.Expansion?.Trim(), _filterXp, StringComparison.OrdinalIgnoreCase)).ToList());
			}
			if (_filterXp == "Short Stories")
			{
				foreach (LorebookEntry entry2 in results)
				{
					AddListRow(entry2);
				}
				ShortStory[] all = ShortStoryLibrary.All;
				foreach (ShortStory story in all)
				{
					if (!results.Any((LorebookEntry e) => string.Equals(e.Title, story.Title, StringComparison.OrdinalIgnoreCase)))
					{
						AddStoryRow(story);
					}
				}
				return;
			}
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
			if (_readerFullscreen)
			{
				return;
			}
			FillRail();
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
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_listPanel);
			((Control)val).set_Width(((Control)_listPanel).get_Width() - 20);
			((Control)val).set_Height(58);
			val.set_ShowBorder(false);
			((Control)val).set_BackgroundColor((Color)((_selected != null && _selected.Id == entry.Id) ? new Color(60, 70, 90) : Color.get_Transparent()));
			Panel row = val;
			bool isSelected = _selected != null && _selected.Id == entry.Id;
			((Control)row).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (!isSelected)
				{
					((Control)row).set_BackgroundColor(new Color(48, 54, 66));
				}
			});
			((Control)row).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				if (!isSelected)
				{
					((Control)row).set_BackgroundColor(Color.get_Transparent());
				}
			});
			(int R, int G, int B) tuple = Palette.Resolve(entry.ColorTag);
			int cr = tuple.R;
			int cg = tuple.G;
			int cb = tuple.B;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(6);
			((Control)val2).set_Height(58);
			((Control)val2).set_BackgroundColor(new Color(cr, cg, cb));
			if (!entry.Opened)
			{
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)row);
				((Control)val3).set_Location(new Point(0, 0));
				((Control)val3).set_Width(6);
				((Control)val3).set_Height(58);
				((Control)val3).set_BackgroundColor(new Color(233, 201, 106));
				Panel glow = val3;
				Tween tween = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(glow, (object)new
				{
					Opacity = 0.25f
				}, 0.9f, 0f, true).Repeat(-1).Reflect();
				((Control)glow).add_Disposed((EventHandler<EventArgs>)delegate
				{
					tween.Cancel();
				});
			}
			int titleX = 14;
			Texture2D xpIcon = _module.GetExpansionIcon(entry.Expansion);
			if (xpIcon != null)
			{
				Image val4 = new Image(AsyncTexture2D.op_Implicit(xpIcon));
				((Control)val4).set_Parent((Container)(object)row);
				((Control)val4).set_Location(new Point(12, 15));
				((Control)val4).set_Size(new Point(28, 28));
				titleX = 46;
			}
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			((Control)val5).set_Location(new Point(titleX, 6));
			((Control)val5).set_Width(((Control)row).get_Width() - titleX - (entry.Opened ? 8 : 54));
			((Control)val5).set_Height(26);
			val5.set_Text(entry.DisplayTitle);
			val5.set_Font(GameService.Content.get_DefaultFont18());
			if (!entry.Opened)
			{
				Label val6 = new Label();
				((Control)val6).set_Parent((Container)(object)row);
				((Control)val6).set_Location(new Point(((Control)row).get_Width() - 50, 8));
				((Control)val6).set_Width(44);
				((Control)val6).set_Height(20);
				val6.set_Text("NEW");
				val6.set_HorizontalAlignment((HorizontalAlignment)2);
				val6.set_TextColor(new Color(233, 201, 106));
				val6.set_Font(GameService.Content.get_DefaultFont14());
			}
			string meta = entry.MetadataLine;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)row);
			((Control)val7).set_Location(new Point(titleX, 32));
			((Control)val7).set_Width(((Control)row).get_Width() - titleX - 8);
			((Control)val7).set_Height(20);
			val7.set_Text(string.IsNullOrEmpty(meta) ? entry.TimestampLocal.ToString("g") : meta);
			val7.set_TextColor(new Color(165, 165, 165));
			val7.set_Font(GameService.Content.get_DefaultFont14());
			((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_selected = entry;
				_mode = DetailMode.Preview;
				RefreshList();
				ShowPreview(entry);
			});
		}

		private void AddStoryRow(ShortStory story)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_listPanel);
			((Control)val).set_Width(((Control)_listPanel).get_Width() - 20);
			((Control)val).set_Height(58);
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			Panel row = val;
			((Control)row).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((Control)row).set_BackgroundColor(new Color(48, 54, 66));
			});
			((Control)row).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)row).set_BackgroundColor(Color.get_Transparent());
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			((Control)val2).set_Location(new Point(14, 6));
			((Control)val2).set_Width(((Control)row).get_Width() - 22);
			((Control)val2).set_Height(26);
			val2.set_Text(story.Title);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_TextColor(new Color(190, 190, 190));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			((Control)val3).set_Location(new Point(14, 32));
			((Control)val3).set_Width(((Control)row).get_Width() - 22);
			((Control)val3).set_Height(20);
			val3.set_Text("Not downloaded · " + story.Timeline);
			val3.set_TextColor(new Color(145, 145, 145));
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowStoryOffer(story);
			});
		}

		private void ShowStoryOffer(ShortStory story)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Expected O, but got Unknown
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			_mode = DetailMode.Empty;
			_selected = null;
			((Container)_detailPanel).ClearChildren();
			int w = ((Control)_detailPanel).get_Width();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_detailPanel);
			((Control)val).set_Location(new Point(12, 10));
			((Control)val).set_Width(w - 24);
			((Control)val).set_Height(28);
			val.set_Text(story.Title);
			val.set_Font(GameService.Content.get_DefaultFont18());
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_detailPanel);
			((Control)val2).set_Location(new Point(12, 42));
			((Control)val2).set_Width(w - 24);
			((Control)val2).set_Height(20);
			val2.set_Text(story.Writer + " · " + story.Published + " · " + story.Timeline);
			val2.set_TextColor(new Color(170, 170, 170));
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_detailPanel);
			((Control)val3).set_Location(new Point(12, 74));
			((Control)val3).set_Width(130);
			val3.set_Text("Download");
			StandardButton getBtn = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_detailPanel);
			((Control)val4).set_Location(new Point(150, 78));
			((Control)val4).set_Width(w - 170);
			((Control)val4).set_Height(20);
			val4.set_Text("");
			val4.set_TextColor(new Color(170, 200, 170));
			Label status = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)_detailPanel);
			((Control)val5).set_Location(new Point(12, 116));
			((Control)val5).set_Width(w - 24);
			((Control)val5).set_Height(Math.Max(60, ((Control)_detailPanel).get_Height() - 128));
			val5.set_FlowDirection((ControlFlowDirection)3);
			val5.set_ControlPadding(new Vector2(0f, 8f));
			((Panel)val5).set_CanScroll(true);
			FlowPanel whyBox = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)whyBox);
			((Control)val6).set_Width(((Control)whyBox).get_Width() - 20);
			val6.set_AutoSizeHeight(true);
			val6.set_Text("Why isn't this already here?");
			val6.set_Font(GameService.Content.get_DefaultFont16());
			val6.set_TextColor(new Color(200, 200, 200));
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)whyBox);
			((Control)val7).set_Width(((Control)whyBox).get_Width() - 20);
			val7.set_AutoSizeHeight(true);
			val7.set_WrapText(true);
			val7.set_TextColor(new Color(160, 160, 160));
			val7.set_Text("The short stories were written and published by ArenaNet, so the module does not ship copies of them. Instead it fetches this one from the Guild Wars 2 Wiki when you ask for it, and stores it in your own codex together with a link back to the source.");
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)whyBox);
			((Control)val8).set_Width(((Control)whyBox).get_Width() - 20);
			val8.set_AutoSizeHeight(true);
			val8.set_WrapText(true);
			val8.set_TextColor(new Color(160, 160, 160));
			val8.set_Text("You need an internet connection for this step only — once downloaded, the story is yours to read offline.");
			string text;
			((Control)getBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)getBtn).set_Enabled(false);
				status.set_Text("Downloading…");
				Task.Run(async delegate
				{
					try
					{
						text = await ShortStoryLibrary.FetchAsync(story, _module.StoryImageDirectory).ConfigureAwait(continueOnCapturedContext: false);
						_module.RunOnMainThread(delegate
						{
							LorebookEntry lorebookEntry = _catalog.AddCaptured(story.Title, text);
							if (lorebookEntry != null)
							{
								lorebookEntry.Expansion = "Short Stories";
								lorebookEntry.Theme = story.Timeline;
								lorebookEntry.Location = story.Writer;
								lorebookEntry.Notes = "Source: " + story.Url;
								lorebookEntry.Opened = false;
								_catalog.Update(lorebookEntry);
								FillRail();
								RefreshList();
								_selected = lorebookEntry;
								ShowPreview(lorebookEntry);
							}
						});
					}
					catch (Exception ex)
					{
						Logger.GetLogger<EncyclopediaView>().Warn(ex, "Short story download failed.");
						_module.RunOnMainThread(delegate
						{
							//IL_0025: Unknown result type (might be due to invalid IL or missing references)
							status.set_Text("Download failed — check your connection and try again.");
							status.set_TextColor(new Color(210, 150, 150));
							((Control)getBtn).set_Enabled(true);
						});
					}
				});
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

		private static Panel MakeConfirmButton(Container parent, Point loc, string text, Color bg)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Location(loc);
			((Control)val).set_Width(80);
			((Control)val).set_Height(26);
			((Control)val).set_BackgroundColor(bg);
			Panel p = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)p);
			((Control)val2).set_Location(new Point(0, 3));
			((Control)val2).set_Width(80);
			((Control)val2).set_Height(20);
			val2.set_Text(text);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			return p;
		}

		private void ShowPreview(LorebookEntry entry)
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Expected O, but got Unknown
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Expected O, but got Unknown
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			FlushEdits();
			_mode = DetailMode.Preview;
			if (!entry.Opened)
			{
				entry.Opened = true;
				_catalog.Update(entry);
			}
			((Container)_detailPanel).ClearChildren();
			int w = ((Control)_detailPanel).get_Width();
			int h = ((Control)_detailPanel).get_Height();
			int titleX = 12;
			Texture2D pvIcon = _module.GetExpansionIcon(entry.Expansion);
			if (pvIcon != null)
			{
				Image val = new Image(AsyncTexture2D.op_Implicit(pvIcon));
				((Control)val).set_Parent((Container)(object)_detailPanel);
				((Control)val).set_Location(new Point(12, 10));
				((Control)val).set_Size(new Point(26, 26));
				titleX = 46;
			}
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_detailPanel);
			((Control)val2).set_Location(new Point(titleX, 10));
			((Control)val2).set_Width(w - titleX - 12);
			((Control)val2).set_Height(28);
			val2.set_Text(entry.DisplayTitle);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_detailPanel);
			((Control)val3).set_Location(new Point(12, 44));
			((Control)val3).set_Width(80);
			val3.set_Text("▶ Play");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.PlayFromCatalog(entry);
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_detailPanel);
			((Control)val4).set_Location(new Point(96, 44));
			((Control)val4).set_Width(80);
			val4.set_Text(_module.SpeechPaused ? "Resume" : "Pause");
			StandardButton pauseBtn = val4;
			((Control)pauseBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				pauseBtn.set_Text(_module.TogglePauseSpeaking() ? "Resume" : "Pause");
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_detailPanel);
			((Control)val5).set_Location(new Point(180, 44));
			((Control)val5).set_Width(72);
			val5.set_Text("■ Stop");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StopSpeaking();
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_detailPanel);
			((Control)val6).set_Location(new Point(256, 44));
			((Control)val6).set_Width(80);
			val6.set_Text("Edit");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_selected = entry;
				_mode = DetailMode.Edit;
				BuildLayout();
			});
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_detailPanel);
			((Control)val7).set_Location(new Point(w - 92, 44));
			((Control)val7).set_Width(80);
			val7.set_Text("Delete");
			StandardButton delBtn = val7;
			((Control)delBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				((Control)delBtn).set_Visible(false);
				Panel yes = null;
				Panel no = null;
				yes = MakeConfirmButton((Container)(object)_detailPanel, new Point(w - 176, 44), "Confirm", new Color(52, 122, 60));
				no = MakeConfirmButton((Container)(object)_detailPanel, new Point(w - 92, 44), "Cancel", new Color(142, 48, 42));
				((Control)yes).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_catalog.Remove(entry.Id);
					_selected = null;
					RefreshList();
					ShowEmpty();
				});
				((Control)no).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((Control)yes).Dispose();
					((Control)no).Dispose();
					((Control)delBtn).set_Visible(true);
				});
			});
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)_detailPanel);
			((Control)val8).set_Location(new Point(w - 92, 78));
			((Control)val8).set_Width(36);
			val8.set_Text("A-");
			StandardButton fontMinus = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)_detailPanel);
			((Control)val9).set_Location(new Point(w - 52, 78));
			((Control)val9).set_Width(36);
			val9.set_Text("A+");
			string meta = entry.MetadataLine;
			if (!string.IsNullOrEmpty(meta))
			{
				Label val10 = new Label();
				((Control)val10).set_Parent((Container)(object)_detailPanel);
				((Control)val10).set_Location(new Point(12, 82));
				((Control)val10).set_Width(w - 120);
				((Control)val10).set_Height(18);
				val10.set_Text(meta);
				val10.set_TextColor(new Color(190, 190, 190));
			}
			string body = BuildBody(entry);
			BookReaderPanel bookReaderPanel = new BookReaderPanel(_textRenderer, _parchment, _module.GetRefTexture("arrow_left.png"), _module.GetRefTexture("arrow_right.png"), _module.GetRefTexture("ornament_corner.png"), _module.GetRefTexture("seal.png"), _module.GetRefTexture("expand.png"), _module.GetRefTexture("collapse.png"));
			((Control)bookReaderPanel).set_Parent((Container)(object)_detailPanel);
			((Control)bookReaderPanel).set_Location(new Point(12, 106));
			((Control)bookReaderPanel).set_Width(w - 24);
			((Control)bookReaderPanel).set_Height(h - 118);
			bookReaderPanel.FontSize = _textFontSize;
			BookReaderPanel reader = bookReaderPanel;
			reader.SetEntry(entry, body, _module.GetExpansionStampIcon(entry.Expansion));
			TextBox searchBox = _searchBox;
			string needle = (_lastSearch = ((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null) ?? _lastSearch);
			if (!string.IsNullOrWhiteSpace(needle))
			{
				reader.Highlight = needle;
				int hitPage = reader.FirstPageWith(needle.Trim());
				if (hitPage > 0)
				{
					reader.GoToPage(hitPage);
				}
			}
			reader.FullscreenToggled += delegate
			{
				_readerFullscreen = true;
				BuildLayout();
			};
			((Control)reader).set_Opacity(0f);
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<BookReaderPanel>(reader, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true);
			((Control)fontMinus).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_textFontSize = Math.Max(12f, _textFontSize - 2f);
				reader.FontSize = _textFontSize;
			});
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_textFontSize = Math.Min(40f, _textFontSize + 2f);
				reader.FontSize = _textFontSize;
			});
		}

		private void ShowEditor(LorebookEntry entry, int w, int h)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Expected O, but got Unknown
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Expected O, but got Unknown
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Expected O, but got Unknown
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Expected O, but got Unknown
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0552: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Expected O, but got Unknown
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Expected O, but got Unknown
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0628: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_064a: Unknown result type (might be due to invalid IL or missing references)
			_mode = DetailMode.Edit;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(_root);
			((Control)val).set_Location(new Point(12, 10));
			((Control)val).set_Width(150);
			val.set_Text("✓ Done editing");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				FlushEdits();
				_mode = DetailMode.Preview;
				BuildLayout();
			});
			Label val2 = new Label();
			((Control)val2).set_Parent(_root);
			((Control)val2).set_Location(new Point(172, 14));
			((Control)val2).set_Width(Math.Max(20, w - 184));
			((Control)val2).set_Height(24);
			val2.set_Text("Editing: " + entry.DisplayTitle);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_TextColor(new Color(210, 200, 170));
			int formW = Math.Min(340, w / 3);
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent(_root);
			((Control)val3).set_Location(new Point(12, 46));
			((Control)val3).set_Width(formW);
			((Control)val3).set_Height(h - 58);
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(0f, 5f));
			((Panel)val3).set_CanScroll(true);
			FlowPanel form = val3;
			AddLabel((Container)(object)form, "Title");
			TextBox titleBox = AddBox((Container)(object)form, entry.Title, formW - 4);
			((TextInputBase)titleBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Title = ((TextInputBase)titleBox).get_Text();
				Save(entry);
			});
			AddLabel((Container)(object)form, "Color tag");
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)form);
			((Control)val4).set_Width(formW - 4);
			Dropdown colorDd = val4;
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
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Parent((Container)(object)form);
			((Control)val5).set_Width(formW - 4);
			Dropdown expDd = val5;
			string[] expansionPresets = ExpansionPresets;
			foreach (string xp in expansionPresets)
			{
				expDd.get_Items().Add(xp);
			}
			string curXp = (string.IsNullOrWhiteSpace(entry.Expansion) ? ExpansionPresets[0] : entry.Expansion.Trim());
			if (!expDd.get_Items().Contains(curXp))
			{
				expDd.get_Items().Add(curXp);
			}
			expDd.set_SelectedItem(curXp);
			expDd.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				entry.Expansion = ((expDd.get_SelectedItem() == ExpansionPresets[0]) ? "" : expDd.get_SelectedItem());
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
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Parent((Container)(object)form);
			((Control)val6).set_Width(formW - 4);
			Dropdown langDd = val6;
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int i = 0; i < targetLanguages.Length; i++)
			{
				string name = targetLanguages[i].Item2;
				langDd.get_Items().Add(name);
			}
			langDd.set_SelectedItem(NameForCode(string.IsNullOrEmpty(entry.TranslatedLang) ? _module.TranslateTargetSetting.get_Value() : entry.TranslatedLang));
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)form);
			((Control)val7).set_Width(formW - 4);
			((Control)val7).set_Height(22);
			val7.set_Text("");
			val7.set_TextColor(new Color(190, 190, 190));
			Label status = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)form);
			((Control)val8).set_Width(150);
			val8.set_Text("Translate now");
			string tr;
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				string target = CodeForName(langDd.get_SelectedItem());
				status.set_Text("Translating…");
				Task.Run(async delegate
				{
					try
					{
						tr = await TranslationService.TranslateAsync(entry.Text, target);
						_module.RunOnMainThread(delegate
						{
							entry.TranslatedText = tr;
							entry.TranslatedLang = target;
							Save(entry);
							status.set_Text("Saved.");
						});
					}
					catch (Exception ex)
					{
						_module.RunOnMainThread(delegate
						{
							status.set_Text("Translation failed.");
						});
						Logger.GetLogger<EncyclopediaView>().Warn(ex, "Translate failed.");
					}
				});
			});
			int editX = 12 + formW + 12;
			Label val9 = new Label();
			((Control)val9).set_Parent(_root);
			((Control)val9).set_Location(new Point(editX, 46));
			((Control)val9).set_Width(w - editX - 12);
			((Control)val9).set_Height(20);
			val9.set_Text("Book text (fix OCR errors, add page 2…)");
			val9.set_TextColor(new Color(200, 200, 200));
			int editW = w - editX - 12;
			int editH = h - 130;
			Panel val10 = new Panel();
			((Control)val10).set_Parent(_root);
			((Control)val10).set_Location(new Point(editX, 70));
			((Control)val10).set_Width(editW);
			((Control)val10).set_Height(editH);
			val10.set_CanScroll(true);
			Panel editScroll = val10;
			int boxW = editW - 18;
			BitmapFont editFont = GameService.Content.get_DefaultFont18();
			string wrapped = WrapForEdit(entry.Text, boxW);
			MultilineTextBox val11 = new MultilineTextBox();
			((Control)val11).set_Parent((Container)(object)editScroll);
			((Control)val11).set_Location(new Point(0, 0));
			((Control)val11).set_Width(boxW);
			((TextInputBase)val11).set_Font(editFont);
			((Control)val11).set_Height(EditContentHeight(wrapped));
			((TextInputBase)val11).set_Text(wrapped);
			MultilineTextBox textEdit = val11;
			((TextInputBase)textEdit).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				entry.Text = UnwrapFromEdit(((TextInputBase)textEdit).get_Text());
				if (EditContentHeight(((TextInputBase)textEdit).get_Text()) != ((Control)textEdit).get_Height())
				{
					_module.RunOnMainThread(delegate
					{
						if (((Control)textEdit).get_Parent() != null)
						{
							((Control)textEdit).set_Height(EditContentHeight(((TextInputBase)textEdit).get_Text()));
						}
					});
				}
				ScheduleEditFlush();
			});
			Label val12 = new Label();
			((Control)val12).set_Parent(_root);
			((Control)val12).set_Location(new Point(editX, h - 56));
			((Control)val12).set_Width(w - editX - 12);
			((Control)val12).set_Height(40);
			val12.set_Text("Tip: editor uses a basic font without accents, but the preview shows full diacritics.");
			val12.set_TextColor(new Color(150, 150, 150));
		}

		private void Save(LorebookEntry entry)
		{
			_catalog.Update(entry);
			RefreshList();
		}

		private void ScheduleEditFlush()
		{
			_editDirty = true;
			_editFlushTimer?.Dispose();
			_editFlushTimer = new Timer(delegate
			{
				if (_editDirty)
				{
					_editDirty = false;
					_catalog.Flush();
				}
			}, null, 1200, -1);
		}

		public void FlushEdits()
		{
			_editFlushTimer?.Dispose();
			_editFlushTimer = null;
			if (_editDirty)
			{
				_editDirty = false;
				_catalog.Flush();
			}
		}

		private static int EditContentHeight(string wrapped)
		{
			int lines = 1;
			if (wrapped != null)
			{
				for (int i = 0; i < wrapped.Length; i++)
				{
					if (wrapped[i] == '\n')
					{
						lines++;
					}
				}
			}
			float lh = GameService.Content.get_DefaultFont18().get_LineHeight();
			return (int)((float)lines * lh) + 8;
		}

		private static string BuildBody(LorebookEntry entry)
		{
			string body = entry.Text;
			if (!string.IsNullOrEmpty(entry.TranslatedText))
			{
				body = body + "\n\n———  " + (entry.TranslatedLang ?? "translation") + "  ———\n\n" + entry.TranslatedText;
			}
			return body;
		}

		private void BuildFullscreenReader(int totalW, int totalH)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			BookReaderPanel bookReaderPanel = new BookReaderPanel(_textRenderer, _parchment, _module.GetRefTexture("arrow_left.png"), _module.GetRefTexture("arrow_right.png"), _module.GetRefTexture("ornament_corner.png"), _module.GetRefTexture("seal.png"), _module.GetRefTexture("expand.png"), _module.GetRefTexture("collapse.png"));
			((Control)bookReaderPanel).set_Parent(_root);
			((Control)bookReaderPanel).set_Location(new Point(8, 6));
			((Control)bookReaderPanel).set_Width(totalW - 16);
			((Control)bookReaderPanel).set_Height(totalH - 14);
			bookReaderPanel.FontSize = _textFontSize;
			BookReaderPanel reader = bookReaderPanel;
			reader.SetEntry(_selected, BuildBody(_selected), _module.GetExpansionStampIcon(_selected.Expansion));
			reader.Highlight = _lastSearch;
			reader.IsFullscreen = true;
			reader.FullscreenToggled += delegate
			{
				_readerFullscreen = false;
				BuildLayout();
			};
			((Control)reader).set_Opacity(0f);
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<BookReaderPanel>(reader, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(_root);
			((Control)val).set_Location(new Point(16, 12));
			((Control)val).set_Width(70);
			val.set_Text("▶ Play");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.PlayFromCatalog(_selected);
			});
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent(_root);
			((Control)val2).set_Location(new Point(90, 12));
			((Control)val2).set_Width(78);
			val2.set_Text(_module.SpeechPaused ? "Resume" : "Pause");
			StandardButton pauseBtn = val2;
			((Control)pauseBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				pauseBtn.set_Text(_module.TogglePauseSpeaking() ? "Resume" : "Pause");
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent(_root);
			((Control)val3).set_Location(new Point(172, 12));
			((Control)val3).set_Width(70);
			val3.set_Text("■ Stop");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StopSpeaking();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent(_root);
			((Control)val4).set_Location(new Point(250, 12));
			((Control)val4).set_Width(36);
			val4.set_Text("A-");
			StandardButton fontMinus = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent(_root);
			((Control)val5).set_Location(new Point(290, 12));
			((Control)val5).set_Width(36);
			val5.set_Text("A+");
			((Control)fontMinus).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_textFontSize = Math.Max(12f, _textFontSize - 2f);
				reader.FontSize = _textFontSize;
			});
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_textFontSize = Math.Min(40f, _textFontSize + 2f);
				reader.FontSize = _textFontSize;
			});
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
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			text = Regex.Replace(text, "⟦IMG:([^⟧]*)⟧", "[image: $1]");
			BitmapFont font = GameService.Content.get_DefaultFont18();
			int maxW = Math.Max(60, pixelWidth - 24);
			StringBuilder sb = new StringBuilder();
			string[] hardLines = text.Replace("\r", "").Split('\n');
			for (int li = 0; li < hardLines.Length; li++)
			{
				if (hardLines[li].Length == 0)
				{
					sb.Append(' ');
					if (li < hardLines.Length - 1)
					{
						sb.Append('\n');
					}
					continue;
				}
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
			text = text.Replace("\u200b\n", " ").Replace("\u200b", "");
			text = Regex.Replace(text, "\\[image:\\s*([^\\]]*)\\]", "⟦IMG:$1⟧");
			string[] lines = text.Replace("\r", "").Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = lines[i].TrimEnd();
			}
			return string.Join("\n", lines);
		}
	}
}
