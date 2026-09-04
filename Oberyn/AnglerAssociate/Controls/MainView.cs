using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Oberyn.AnglerAssociate.Data;
using Oberyn.AnglerAssociate.Models;
using Oberyn.AnglerAssociate.Services;

namespace Oberyn.AnglerAssociate.Controls
{
	public class MainView : Panel
	{
		private static readonly (Region Region, string IconFile, string Tooltip)[] RegionBadges = new(Region, string, string)[8]
		{
			(Region.Tyria, "tyria.png", "Tyria & Global"),
			(Region.Orr, "orr.png", "Orr"),
			(Region.MaguumaJungle, "maguuma.png", "Maguuma Jungle"),
			(Region.CrystalDesert, "crystal_desert.png", "Crystal Desert"),
			(Region.Cantha, "cantha.png", "Cantha"),
			(Region.HornOfMaguuma, "horn_of_maguuma.png", "Horn of Maguuma"),
			(Region.Janthir, "janthir.png", "Janthir"),
			(Region.Castora, "castora.png", "Castora")
		};

		private static readonly Dictionary<Region, string> RegionPortraitFiles = new Dictionary<Region, string>
		{
			{
				Region.Tyria,
				"portrait_tyria.png"
			},
			{
				Region.Orr,
				"portrait_orr.png"
			},
			{
				Region.MaguumaJungle,
				"portrait_maguuma.png"
			},
			{
				Region.CrystalDesert,
				"portrait_crystal_desert.png"
			},
			{
				Region.Cantha,
				"portrait_cantha.png"
			},
			{
				Region.HornOfMaguuma,
				"portrait_horn_of_maguuma.png"
			},
			{
				Region.Janthir,
				"portrait_janthir.png"
			},
			{
				Region.Castora,
				"portrait_castora.png"
			}
		};

		private readonly ContentsManager _contentsManager;

		private readonly AchievementProgressService _achievementProgress;

		private readonly Label _dailyLabel;

		private readonly DayNightBanner _tyriaBanner;

		private readonly DayNightBanner _canthaBanner;

		private readonly List<(Region Region, Image Image)> _regionBadges = new List<(Region, Image)>();

		private readonly Dictionary<Region, AsyncTexture2D> _regionColorTextures = new Dictionary<Region, AsyncTexture2D>();

		private readonly Dictionary<Region, AsyncTexture2D> _regionPortraitTextures = new Dictionary<Region, AsyncTexture2D>();

		private Image _regionPortrait;

		private AsyncTexture2D _regionNullTexture;

		private Region? _selectedRegion;

		private readonly Dropdown _achievementDropdown;

		private readonly Dropdown _baitDropdown;

		private TextBox _holeFilterSearchBox;

		private Panel _holeFilterPopup;

		private readonly List<Panel> _holeFilterResultControls = new List<Panel>();

		private List<(string Display, FishingHole Hole)> _holeFilterOptions = new List<(string, FishingHole)>();

		private FishingHole? _selectedHoleFilter;

		private readonly Checkbox _availableNowCheckbox;

		private readonly Checkbox _hideCollectedCheckbox;

		private readonly TextBox _searchBox;

		private readonly Image _refreshButton;

		private readonly Panel _tableRows;

		private readonly List<Panel> _rowControls = new List<Panel>();

		private Label _allCollectedLabel;

		private readonly Dictionary<string, Bait> _baitDisplayToValue = new Dictionary<string, Bait>();

		private Panel _tableHeaderPanel;

		private Panel _detailPanel;

		private Fish _detailFish;

		private Label _detailFishNameLabel;

		private Panel _detailInfoPanel;

		private Panel _holeRowsPanel;

		private readonly List<Panel> _holeRowControls = new List<Panel>();

		private const int ColumnWidth = 170;

		private const int RegionColumnWidth = 110;

		private Panel _regionColumnPanel;

		private readonly List<Panel> _regionColumnControls = new List<Panel>();

		private Panel _mapColumnPanel;

		private readonly List<Panel> _mapColumnControls = new List<Panel>();

		private Panel _areaColumnPanel;

		private readonly List<Panel> _areaColumnControls = new List<Panel>();

		private List<PlaceNode> _currentRegions;

		private PlaceNode _selectedRegionNode;

		private PlaceNode _selectedMapNode;

		private const int RowHeight = 40;

		private const int TableTop = 25;

		private const double PixelsPerChar = 7.0;

		public MainView(ContentsManager contentsManager, AchievementProgressService achievementProgress, int contentHeight)
			: this()
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Expected O, but got Unknown
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Expected O, but got Unknown
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Expected O, but got Unknown
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Expected O, but got Unknown
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_0547: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_0571: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Expected O, but got Unknown
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_0608: Unknown result type (might be due to invalid IL or missing references)
			//IL_0612: Unknown result type (might be due to invalid IL or missing references)
			//IL_0622: Expected O, but got Unknown
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0651: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_065e: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Expected O, but got Unknown
			//IL_0685: Unknown result type (might be due to invalid IL or missing references)
			//IL_068a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_0743: Unknown result type (might be due to invalid IL or missing references)
			//IL_074a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0757: Expected O, but got Unknown
			//IL_0769: Unknown result type (might be due to invalid IL or missing references)
			//IL_076e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_077f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0789: Unknown result type (might be due to invalid IL or missing references)
			//IL_0794: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Expected O, but got Unknown
			//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Expected O, but got Unknown
			_contentsManager = contentsManager;
			_achievementProgress = achievementProgress;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Today's daily: " + DailyFisherRotation.GetToday());
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(300);
			((Control)val).set_Height(24);
			_dailyLabel = val;
			DayNightBanner dayNightBanner = new DayNightBanner(contentsManager, "tyria", "Tyria", Cycle.Tyria);
			((Control)dayNightBanner).set_Parent((Container)(object)this);
			((Control)dayNightBanner).set_Location(new Point(0, 40));
			_tyriaBanner = dayNightBanner;
			DayNightBanner dayNightBanner2 = new DayNightBanner(contentsManager, "cantha", "Cantha/Castora", Cycle.CanthaCastora);
			((Control)dayNightBanner2).set_Parent((Container)(object)this);
			((Control)dayNightBanner2).set_Location(new Point(130, 40));
			_canthaBanner = dayNightBanner2;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Search fish");
			((Control)val2).set_Location(new Point(0, 460));
			((Control)val2).set_Width(250);
			((Control)val2).set_Height(20);
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)this);
			((TextInputBase)val3).set_PlaceholderText("Fish name...");
			((Control)val3).set_Location(new Point(0, 480));
			((Control)val3).set_Width(225);
			((Control)val3).set_Height(24);
			_searchBox = val3;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				RebuildRows();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("X");
			((Control)val4).set_Location(new Point(228, 480));
			((Control)val4).set_Width(22);
			((Control)val4).set_Height(24);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val4).set_BasicTooltipText("Clear search");
			((Control)val4).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)_searchBox).set_Text(string.Empty);
				RebuildRows();
			});
			Image val5 = new Image();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/reload.png")));
			((Control)val5).set_Location(new Point(0, 520));
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_BasicTooltipText("Refresh achievement progress");
			_refreshButton = val5;
			((Control)_refreshButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _achievementProgress.RefreshAsync();
				RefreshLiveData();
				RebuildRows();
			});
			int col1 = 320;
			int col2 = 540;
			int col3 = 760;
			int y = 0;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Filter by region");
			((Control)val6).set_Location(new Point(col1, y));
			((Control)val6).set_Width(420);
			((Control)val6).set_Height(20);
			y += 20;
			_regionNullTexture = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/null.png"));
			int bx = col1;
			(Region, string, string)[] regionBadges = RegionBadges;
			for (int i = 0; i < regionBadges.Length; i++)
			{
				(Region, string, string) badge = regionBadges[i];
				Texture2D colorTexture = contentsManager.GetTexture("icons/" + badge.Item2);
				_regionColorTextures[badge.Item1] = AsyncTexture2D.op_Implicit(colorTexture);
				_regionPortraitTextures[badge.Item1] = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/" + RegionPortraitFiles[badge.Item1]));
				Image val7 = new Image();
				((Control)val7).set_Parent((Container)(object)this);
				val7.set_Texture(AsyncTexture2D.op_Implicit(colorTexture));
				((Control)val7).set_Location(new Point(bx, y));
				((Control)val7).set_Size(new Point(40, 40));
				((Control)val7).set_BasicTooltipText(badge.Item3);
				Image badgeImage = val7;
				var (capturedRegion, _, _) = badge;
				((Control)badgeImage).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_selectedRegion = ((_selectedRegion == capturedRegion) ? null : new Region?(capturedRegion));
					RefreshRegionBadgeTextures();
					RebuildAchievementDropdown();
					RebuildBaitDropdown();
					RebuildHoleFilterOptions();
					RebuildRows();
				});
				_regionBadges.Add((badge.Item1, badgeImage));
				bx += 48;
			}
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Location(new Point(940, 0));
			((Control)val8).set_Size(new Point(80, 110));
			((Control)val8).set_Visible(false);
			_regionPortrait = val8;
			RefreshRegionBadgeTextures();
			Checkbox val9 = new Checkbox();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text("Hide collected");
			((Control)val9).set_Location(new Point(col3, y));
			val9.set_Checked(false);
			_hideCollectedCheckbox = val9;
			_hideCollectedCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				RebuildRows();
			});
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Parent((Container)(object)this);
			val10.set_Text("Show only available now");
			((Control)val10).set_Location(new Point(col3, y + 25));
			val10.set_Checked(false);
			_availableNowCheckbox = val10;
			_availableNowCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				RebuildRows();
			});
			y += 45;
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)this);
			val11.set_Text("Filter by achievement");
			((Control)val11).set_Location(new Point(col1, y));
			((Control)val11).set_Width(200);
			((Control)val11).set_Height(20);
			Label val12 = new Label();
			((Control)val12).set_Parent((Container)(object)this);
			val12.set_Text("Filter by bait");
			((Control)val12).set_Location(new Point(col2, y));
			((Control)val12).set_Width(200);
			((Control)val12).set_Height(20);
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)this);
			val13.set_Text("Filter by fishing hole");
			((Control)val13).set_Location(new Point(col3, y));
			((Control)val13).set_Width(200);
			((Control)val13).set_Height(20);
			y += 20;
			Dropdown val14 = new Dropdown();
			((Control)val14).set_Parent((Container)(object)this);
			((Control)val14).set_Location(new Point(col1, y));
			((Control)val14).set_Width(200);
			_achievementDropdown = val14;
			_achievementDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRows();
			});
			Dropdown val15 = new Dropdown();
			((Control)val15).set_Parent((Container)(object)this);
			((Control)val15).set_Location(new Point(col2, y));
			((Control)val15).set_Width(200);
			_baitDropdown = val15;
			_baitDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				RebuildRows();
			});
			TextBox val16 = new TextBox();
			((Control)val16).set_Parent((Container)(object)this);
			((TextInputBase)val16).set_PlaceholderText("Type to search...");
			((Control)val16).set_Location(new Point(col3, y));
			((Control)val16).set_Width(200);
			_holeFilterSearchBox = val16;
			((TextInputBase)_holeFilterSearchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				RenderHoleFilterResults();
			});
			Label val17 = new Label();
			((Control)val17).set_Parent((Container)(object)this);
			val17.set_Text("X");
			((Control)val17).set_Location(new Point(col3 + 200 - 22, y));
			((Control)val17).set_Width(22);
			((Control)val17).set_Height(24);
			val17.set_HorizontalAlignment((HorizontalAlignment)1);
			val17.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val17).set_BasicTooltipText("Clear hole filter");
			((Control)val17).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)_holeFilterSearchBox).set_Text(string.Empty);
				_selectedHoleFilter = null;
				RenderHoleFilterResults();
				RebuildRows();
			});
			Panel val18 = new Panel();
			((Control)val18).set_Parent((Container)(object)this);
			((Control)val18).set_Location(new Point(col3, y + 26));
			((Control)val18).set_Width(200);
			((Control)val18).set_Height(280);
			val18.set_CanScroll(true);
			val18.set_ShowBorder(true);
			((Control)val18).set_BackgroundColor(new Color(20, 20, 20, 235));
			((Control)val18).set_Visible(false);
			((Control)val18).set_ZIndex(100);
			_holeFilterPopup = val18;
			y += 40;
			BuildTableHeader(320, y);
			Panel val19 = new Panel();
			((Control)val19).set_Parent((Container)(object)this);
			((Control)val19).set_Location(new Point(320, y + 25));
			((Control)val19).set_Width(732);
			((Control)val19).set_Height(Math.Max(contentHeight - (y + 25) - 10, 100));
			val19.set_CanScroll(true);
			_tableRows = val19;
			Label val20 = new Label();
			((Control)val20).set_Parent((Container)(object)this);
			((Control)val20).set_Location(new Point(320, y + 25));
			((Control)val20).set_Width(732);
			((Control)val20).set_Height(40);
			val20.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val20).set_Visible(false);
			_allCollectedLabel = val20;
			int detailTop = y;
			int detailHeight = Math.Max(contentHeight - detailTop - 10, 100);
			BuildDetailPanel(contentsManager, 320, detailTop, 732, detailHeight);
			RebuildAchievementDropdown();
			RebuildBaitDropdown();
			RebuildHoleFilterOptions();
			RebuildRows();
		}

		public void RefreshLiveData()
		{
			_tyriaBanner.Refresh();
			_canthaBanner.Refresh();
			_dailyLabel.set_Text("Today's daily: " + DailyFisherRotation.GetToday());
		}

		private void BuildDetailPanel(ContentsManager contentsManager, int x, int y, int width, int height)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Expected O, but got Unknown
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Expected O, but got Unknown
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Expected O, but got Unknown
			BitmapFont headerFont = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			((Control)val).set_Visible(false);
			_detailPanel = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)_detailPanel);
			val2.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/arrow_up.png")));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(new Point(32, 32));
			((Control)val2).set_BasicTooltipText("Back to fish list");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				HideDetailView();
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_detailPanel);
			val3.set_Font(headerFont);
			((Control)val3).set_Location(new Point(40, 0));
			((Control)val3).set_Width(300);
			((Control)val3).set_Height(32);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			_detailFishNameLabel = val3;
			int col1X = 250;
			int col2X = col1X + 110 + 10;
			int col3X = col2X + 170 + 10;
			int columnHeight = Math.Max(height - 100 - 10, 100);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_detailPanel);
			val4.set_Text("Fishing Hole");
			val4.set_Font(headerFont);
			((Control)val4).set_Location(new Point(0, 80));
			((Control)val4).set_Width(170);
			((Control)val4).set_Height(20);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_detailPanel);
			val5.set_Text("Power");
			val5.set_Font(headerFont);
			((Control)val5).set_Location(new Point(170, 80));
			((Control)val5).set_Width(50);
			((Control)val5).set_Height(20);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)_detailPanel);
			((Control)val6).set_Location(new Point(0, 100));
			((Control)val6).set_Width(240);
			((Control)val6).set_Height(columnHeight);
			val6.set_CanScroll(true);
			_holeRowsPanel = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_detailPanel);
			val7.set_Text("Region");
			val7.set_Font(headerFont);
			((Control)val7).set_Location(new Point(col1X, 80));
			((Control)val7).set_Width(110);
			((Control)val7).set_Height(20);
			((Control)val7).set_BasicTooltipText("Select a fishing hole on the left to see where to find it.");
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)_detailPanel);
			val8.set_Text("Map");
			val8.set_Font(headerFont);
			((Control)val8).set_Location(new Point(col2X, 80));
			((Control)val8).set_Width(170);
			((Control)val8).set_Height(20);
			((Control)val8).set_BasicTooltipText("Select a region to see the list of maps with this hole.");
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)_detailPanel);
			val9.set_Text("Nearest Waypoint");
			val9.set_Font(headerFont);
			((Control)val9).set_Location(new Point(col3X, 80));
			((Control)val9).set_Width(170);
			((Control)val9).set_Height(20);
			((Control)val9).set_BasicTooltipText("Select a map to see the list of the nearest waypoints.");
			Panel val10 = new Panel();
			((Control)val10).set_Parent((Container)(object)_detailPanel);
			((Control)val10).set_Location(new Point(col1X, 100));
			((Control)val10).set_Width(110);
			((Control)val10).set_Height(columnHeight);
			val10.set_CanScroll(true);
			_regionColumnPanel = val10;
			Panel val11 = new Panel();
			((Control)val11).set_Parent((Container)(object)_detailPanel);
			((Control)val11).set_Location(new Point(col2X, 100));
			((Control)val11).set_Width(170);
			((Control)val11).set_Height(columnHeight);
			val11.set_CanScroll(true);
			_mapColumnPanel = val11;
			Panel val12 = new Panel();
			((Control)val12).set_Parent((Container)(object)_detailPanel);
			((Control)val12).set_Location(new Point(col3X, 100));
			((Control)val12).set_Width(170);
			((Control)val12).set_Height(columnHeight);
			val12.set_CanScroll(true);
			_areaColumnPanel = val12;
		}

		private void ShowFishDetail(Fish fish)
		{
			_detailFish = fish;
			_detailFishNameLabel.set_Text(fish.Name);
			RebuildDetailInfo();
			RebuildHoleRows();
			_currentRegions = null;
			_selectedRegionNode = null;
			_selectedMapNode = null;
			RenderRegionColumn();
			RenderMapColumn();
			RenderAreaColumn();
			((Control)_tableHeaderPanel).set_Visible(false);
			((Control)_tableRows).set_Visible(false);
			((Control)_allCollectedLabel).set_Visible(false);
			((Control)_detailPanel).set_Visible(true);
		}

		private void HideDetailView()
		{
			((Control)_detailPanel).set_Visible(false);
			((Control)_tableHeaderPanel).set_Visible(true);
			((Control)_tableRows).set_Visible(true);
		}

		private void RebuildDetailInfo()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			Panel detailInfoPanel = _detailInfoPanel;
			if (detailInfoPanel != null)
			{
				((Control)detailInfoPanel).Dispose();
			}
			int groupX = Math.Max((((Control)_detailPanel).get_Width() - 260) / 2, 40);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_detailPanel);
			((Control)val).set_Location(new Point(groupX, 32));
			((Control)val).set_Width(260);
			((Control)val).set_Height(40);
			_detailInfoPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_detailInfoPanel);
			val2.set_Text(_detailFish.Rarity.ToString());
			val2.set_TextColor(RarityColors.Get(_detailFish.Rarity));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(100);
			((Control)val2).set_Height(40);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			BuildBaitCell(_detailInfoPanel, _detailFish, new Point(100, 0));
			BuildTimeOfDayCell(_detailInfoPanel, _detailFish, new Point(180, 0));
		}

		private void RebuildHoleRows()
		{
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			foreach (Panel holeRowControl in _holeRowControls)
			{
				((Control)holeRowControl).Dispose();
			}
			_holeRowControls.Clear();
			if (_detailFish.AllHoles.Count == 0)
			{
				bool isOpenWater = _detailFish.Hole1.GetValueOrDefault() == FishingHole.OpenWater || _detailFish.Hole2.GetValueOrDefault() == FishingHole.OpenWater || _detailFish.Hole3.GetValueOrDefault() == FishingHole.OpenWater || _detailFish.Hole4.GetValueOrDefault() == FishingHole.OpenWater;
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_holeRowsPanel);
				((Control)val).set_Location(new Point(0, 0));
				((Control)val).set_Width(220);
				((Control)val).set_Height(40);
				Panel emptyRow = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)emptyRow);
				val2.set_Text(isOpenWater ? "Any Open Water hole" : "No catch-location data available.");
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Width(220);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				_holeRowControls.Add(emptyRow);
				return;
			}
			int rowY = 0;
			foreach (FishHoleEntry entry in _detailFish.AllHoles)
			{
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)_holeRowsPanel);
				((Control)val3).set_Location(new Point(0, rowY));
				((Control)val3).set_Width(220);
				((Control)val3).set_Height(40);
				Panel row = val3;
				string holeText = EnumDisplay.Format(entry.Hole);
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)row);
				val4.set_Text(TruncateToWidth(holeText, 170));
				((Control)val4).set_BasicTooltipText(holeText);
				((Control)val4).set_Location(new Point(0, 0));
				((Control)val4).set_Width(170);
				((Control)val4).set_Height(40);
				val4.set_VerticalAlignment((VerticalAlignment)1);
				Label nameLabel = val4;
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)row);
				val5.set_Text(entry.Power.ToString());
				((Control)val5).set_Location(new Point(170, 0));
				((Control)val5).set_Width(50);
				((Control)val5).set_Height(40);
				val5.set_VerticalAlignment((VerticalAlignment)1);
				val5.set_HorizontalAlignment((HorizontalAlignment)1);
				FishingHole capturedHole = entry.Hole;
				((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectHole(capturedHole);
				});
				((Control)nameLabel).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectHole(capturedHole);
				});
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectHole(capturedHole);
				});
				_holeRowControls.Add(row);
				rowY += 40;
			}
		}

		private void SelectHole(FishingHole hole)
		{
			_currentRegions = (HolePlaces.ByHole.TryGetValue(hole, out var places) ? places : new List<PlaceNode>());
			_selectedRegionNode = null;
			_selectedMapNode = null;
			RenderRegionColumn();
			RenderMapColumn();
			RenderAreaColumn();
		}

		private void SelectRegion(PlaceNode region)
		{
			_selectedRegionNode = region;
			_selectedMapNode = null;
			RenderRegionColumn();
			RenderMapColumn();
			RenderAreaColumn();
		}

		private void SelectMap(PlaceNode map)
		{
			_selectedMapNode = map;
			RenderMapColumn();
			RenderAreaColumn();
		}

		private void RenderRegionColumn()
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Expected O, but got Unknown
			foreach (Panel regionColumnControl in _regionColumnControls)
			{
				((Control)regionColumnControl).Dispose();
			}
			_regionColumnControls.Clear();
			BitmapFont boldFont = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2);
			if (_currentRegions == null || _currentRegions.Count <= 0)
			{
				return;
			}
			int rowY = 0;
			foreach (PlaceNode node in _currentRegions)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_regionColumnPanel);
				((Control)val).set_Location(new Point(0, rowY));
				((Control)val).set_Width(110);
				((Control)val).set_Height(40);
				Panel row = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(TruncateToWidth(node.Label, 110));
				((Control)val2).set_BasicTooltipText(node.Label);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Width(110);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				Label label = val2;
				if (node == _selectedRegionNode)
				{
					label.set_Font(boldFont);
				}
				PlaceNode capturedNode = node;
				((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectRegion(capturedNode);
				});
				((Control)label).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectRegion(capturedNode);
				});
				_regionColumnControls.Add(row);
				rowY += 40;
			}
		}

		private void RenderMapColumn()
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			foreach (Panel mapColumnControl in _mapColumnControls)
			{
				((Control)mapColumnControl).Dispose();
			}
			_mapColumnControls.Clear();
			BitmapFont boldFont = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2);
			List<PlaceNode> maps = _selectedRegionNode?.Children;
			if (maps == null || maps.Count <= 0)
			{
				return;
			}
			int rowY = 0;
			foreach (PlaceNode node in maps)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_mapColumnPanel);
				((Control)val).set_Location(new Point(0, rowY));
				((Control)val).set_Width(170);
				((Control)val).set_Height(40);
				Panel row = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(TruncateToWidth(node.Label, 170));
				((Control)val2).set_BasicTooltipText(node.Label);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Width(170);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				Label label = val2;
				if (node == _selectedMapNode)
				{
					label.set_Font(boldFont);
				}
				PlaceNode capturedNode = node;
				((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectMap(capturedNode);
				});
				((Control)label).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectMap(capturedNode);
				});
				_mapColumnControls.Add(row);
				rowY += 40;
			}
		}

		private void RenderAreaColumn()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			foreach (Panel areaColumnControl in _areaColumnControls)
			{
				((Control)areaColumnControl).Dispose();
			}
			_areaColumnControls.Clear();
			List<PlaceNode> areas = _selectedMapNode?.Children;
			if (areas == null || areas.Count <= 0)
			{
				return;
			}
			int rowY = 0;
			foreach (PlaceNode node in areas)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_areaColumnPanel);
				((Control)val).set_Location(new Point(0, rowY));
				((Control)val).set_Width(170);
				((Control)val).set_Height(40);
				Panel row = val;
				string tooltip = "Click to copy to clipboard";
				if (LocationWaypoints.ByName.TryGetValue(node.Label, out var area) && !string.IsNullOrEmpty(area.SpecialNote))
				{
					tooltip = tooltip + "\n" + area.SpecialNote;
				}
				if (node.Label.Length > 24)
				{
					tooltip = node.Label + "\n" + tooltip;
				}
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(TruncateToWidth(node.Label, 170));
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Width(170);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				((Control)row).set_BasicTooltipText(tooltip);
				((Control)val2).set_BasicTooltipText(tooltip);
				string capturedLabel = node.Label;
				((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CopyWaypoint(capturedLabel);
				});
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CopyWaypoint(capturedLabel);
				});
				_areaColumnControls.Add(row);
				rowY += 40;
			}
		}

		private static void CopyWaypoint(string areaName)
		{
			if (LocationWaypoints.ByName.TryGetValue(areaName, out var area) && !string.IsNullOrEmpty(area.Waypoint))
			{
				CopyToClipboard(area.Waypoint);
			}
		}

		private static void CopyToClipboard(string text)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				try
				{
					Clipboard.SetText(text);
				}
				catch
				{
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}

		private static bool MatchesRegion(Fish fish, Region region)
		{
			if (region == Region.Tyria)
			{
				if (fish.Region != Region.Tyria)
				{
					return fish.Region == Region.Global;
				}
				return true;
			}
			return fish.Region == region;
		}

		private void RefreshRegionBadgeTextures()
		{
			foreach (var regionBadge in _regionBadges)
			{
				var (region, _) = regionBadge;
				regionBadge.Image.set_Texture((!_selectedRegion.HasValue || _selectedRegion == region) ? _regionColorTextures[region] : _regionNullTexture);
			}
			if (_selectedRegion.HasValue && _regionPortraitTextures.TryGetValue(_selectedRegion.Value, out var portraitTexture))
			{
				_regionPortrait.set_Texture(portraitTexture);
				((Control)_regionPortrait).set_Visible(true);
			}
			else
			{
				((Control)_regionPortrait).set_Visible(false);
			}
		}

		private void BuildTableHeader(int x, int y)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			string[] headers = new string[6] { "Fish", "Found in", "Rarity", "Hole", "Bait", "Time" };
			int[] widths = new int[6] { 150, 170, 100, 130, 80, 80 };
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(710);
			((Control)val).set_Height(24);
			_tableHeaderPanel = val;
			BitmapFont headerFont = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2);
			int cx = 0;
			for (int i = 0; i < headers.Length; i++)
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)_tableHeaderPanel);
				val2.set_Text(headers[i]);
				val2.set_Font(headerFont);
				((Control)val2).set_Location(new Point(cx, 0));
				((Control)val2).set_Width(widths[i]);
				((Control)val2).set_Height(24);
				val2.set_HorizontalAlignment((HorizontalAlignment)((i != 0) ? 1 : 0));
				cx += widths[i];
			}
		}

		private void RebuildAchievementDropdown()
		{
			_achievementDropdown.get_Items().Clear();
			_achievementDropdown.get_Items().Add("All");
			IEnumerable<Fish> source = ((!_selectedRegion.HasValue) ? FishCatalog.All : FishCatalog.All.Where((Fish f) => MatchesRegion(f, _selectedRegion.Value)));
			SortedSet<string> names = new SortedSet<string>();
			foreach (Fish f2 in source.Where((Fish f) => f.Collection != null))
			{
				names.Add(f2.Collection);
				if (f2.AvidCollection != null && _achievementProgress.IsCollectionDone(f2.CollectionId))
				{
					names.Add(f2.AvidCollection);
				}
			}
			foreach (string name in names)
			{
				_achievementDropdown.get_Items().Add(name);
			}
			_achievementDropdown.set_SelectedItem("All");
		}

		private void RebuildBaitDropdown()
		{
			_baitDropdown.get_Items().Clear();
			_baitDisplayToValue.Clear();
			_baitDropdown.get_Items().Add("All");
			foreach (Bait bait in from b in ((!_selectedRegion.HasValue) ? FishCatalog.All : FishCatalog.All.Where((Fish f) => MatchesRegion(f, _selectedRegion.Value))).Select((Fish f) => f.Bait).Distinct()
				orderby b.ToString()
				select b)
			{
				string display = EnumDisplay.Format(bait);
				_baitDropdown.get_Items().Add(display);
				_baitDisplayToValue[display] = bait;
			}
			_baitDropdown.set_SelectedItem("All");
		}

		private void RebuildHoleFilterOptions()
		{
			IEnumerable<Fish> obj = ((!_selectedRegion.HasValue) ? FishCatalog.All : FishCatalog.All.Where((Fish f) => MatchesRegion(f, _selectedRegion.Value)));
			SortedDictionary<string, FishingHole> holes = new SortedDictionary<string, FishingHole>();
			foreach (Fish f2 in obj)
			{
				FishingHole?[] array = new FishingHole?[4] { f2.Hole1, f2.Hole2, f2.Hole3, f2.Hole4 };
				for (int i = 0; i < array.Length; i++)
				{
					FishingHole? hole = array[i];
					if (hole.HasValue && hole.Value != 0)
					{
						holes[EnumDisplay.Format(hole.Value)] = hole.Value;
					}
				}
				foreach (FishHoleEntry entry in f2.AllHoles)
				{
					holes[EnumDisplay.Format(entry.Hole)] = entry.Hole;
				}
			}
			_holeFilterOptions = holes.Select((KeyValuePair<string, FishingHole> kv) => (kv.Key, kv.Value)).ToList();
			if (_selectedHoleFilter.HasValue && !_holeFilterOptions.Any(((string Display, FishingHole Hole) o) => o.Hole == _selectedHoleFilter.Value))
			{
				_selectedHoleFilter = null;
				((TextInputBase)_holeFilterSearchBox).set_Text(string.Empty);
			}
			RenderHoleFilterResults();
		}

		private void RenderHoleFilterResults()
		{
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			foreach (Panel holeFilterResultControl in _holeFilterResultControls)
			{
				((Control)holeFilterResultControl).Dispose();
			}
			_holeFilterResultControls.Clear();
			string searchText = ((TextInputBase)_holeFilterSearchBox).get_Text()?.Trim();
			((Control)_holeFilterPopup).set_Visible(!string.IsNullOrEmpty(searchText));
			if (string.IsNullOrEmpty(searchText))
			{
				return;
			}
			IEnumerable<(string Display, FishingHole Hole)> enumerable = _holeFilterOptions.Where(((string Display, FishingHole Hole) o) => o.Display.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
			int rowY = 0;
			foreach (var option in enumerable)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_holeFilterPopup);
				((Control)val).set_Location(new Point(0, rowY));
				((Control)val).set_Width(180);
				((Control)val).set_Height(40);
				Panel row = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(option.Display);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Width(180);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				(string Display, FishingHole Hole) capturedOption = option;
				((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectThis();
				});
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectThis();
				});
				_holeFilterResultControls.Add(row);
				rowY += 40;
				void SelectThis()
				{
					_selectedHoleFilter = capturedOption.Hole;
					((TextInputBase)_holeFilterSearchBox).set_Text(capturedOption.Display);
					((Control)_holeFilterPopup).set_Visible(false);
					RebuildRows();
				}
			}
		}

		private static bool FishMatchesHole(Fish fish, FishingHole hole)
		{
			if (IsWildcardHole(fish.Hole1) || IsWildcardHole(fish.Hole2) || IsWildcardHole(fish.Hole3) || IsWildcardHole(fish.Hole4))
			{
				return true;
			}
			return FishHasExactHole(fish, hole);
		}

		private static bool IsWildcardHole(FishingHole? hole)
		{
			if (hole != FishingHole.Any)
			{
				return hole.GetValueOrDefault() == FishingHole.OpenWater;
			}
			return true;
		}

		private static bool FishHasExactHole(Fish fish, FishingHole hole)
		{
			if (fish.Hole1 == hole || fish.Hole2 == hole || fish.Hole3 == hole || fish.Hole4 == hole)
			{
				return true;
			}
			return fish.AllHoles.Any((FishHoleEntry entry) => entry.Hole == hole);
		}

		private void RebuildRows()
		{
			foreach (Panel rowControl in _rowControls)
			{
				((Control)rowControl).Dispose();
			}
			_rowControls.Clear();
			bool onlyAvailableNow = _availableNowCheckbox.get_Checked();
			bool hideCollected = _hideCollectedCheckbox.get_Checked();
			string searchText = ((TextInputBase)_searchBox).get_Text()?.Trim();
			string achievementFilter = null;
			IEnumerable<Fish> fish;
			if (!string.IsNullOrEmpty(searchText))
			{
				fish = FishCatalog.All.Where((Fish f) => f.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
			}
			else
			{
				achievementFilter = _achievementDropdown.get_SelectedItem();
				string baitFilter = _baitDropdown.get_SelectedItem();
				fish = ((!_selectedRegion.HasValue) ? FishCatalog.All : FishCatalog.All.Where((Fish f) => MatchesRegion(f, _selectedRegion.Value)));
				if (achievementFilter != null && achievementFilter != "All")
				{
					fish = fish.Where((Fish f) => f.Collection == achievementFilter || f.AvidCollection == achievementFilter);
				}
				if (baitFilter != null && baitFilter != "All" && _baitDisplayToValue.TryGetValue(baitFilter, out var baitValue))
				{
					fish = fish.Where((Fish f) => f.Bait == baitValue);
				}
				if (_selectedHoleFilter.HasValue)
				{
					FishingHole holeValue = _selectedHoleFilter.Value;
					fish = from f in fish
						where FishMatchesHole(f, holeValue)
						orderby FishHasExactHole(f, holeValue) descending
						select f;
				}
				if (onlyAvailableNow)
				{
					fish = fish.Where(IsAvailableNow);
				}
				if (hideCollected)
				{
					fish = fish.Where((Fish f) => (achievementFilter == null || !(achievementFilter != "All") || !(f.AvidCollection == achievementFilter)) ? (!_achievementProgress.IsFishCaught(f)) : (!_achievementProgress.IsFishCaughtForAvid(f)));
				}
			}
			List<Fish> fishList = fish.ToList();
			int rowY = 0;
			foreach (Fish f2 in fishList)
			{
				Panel row = BuildRow(f2, rowY);
				_rowControls.Add(row);
				rowY += 40;
			}
			bool showAllCollected = hideCollected && fishList.Count == 0 && achievementFilter != null && achievementFilter != "All";
			((Control)_allCollectedLabel).set_Visible(showAllCollected);
			if (showAllCollected)
			{
				_allCollectedLabel.set_Text("All fish collected for " + achievementFilter + "! Tasty, tasty ambergris!");
			}
		}

		private static bool IsAvailableNow(Fish fish)
		{
			if (fish.Cycle == Cycle.Global)
			{
				return fish.TimeOfDay == TimeOfDay.Any;
			}
			TimeOfDay state = TyrianClock.GetState(fish.Cycle).State;
			return fish.IsCatchableAt(state);
		}

		private Panel BuildRow(Fish fish, int y)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_tableRows);
			((Control)val).set_Location(new Point(0, y));
			((Control)val).set_Width(710);
			((Control)val).set_Height(40);
			Panel row = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text(fish.Name);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(150);
			((Control)val2).set_Height(40);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_BasicTooltipText("Click for fishing hole details");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowFishDetail(fish);
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(FormatFoundInShort(fish));
			((Control)val3).set_BasicTooltipText(FormatFoundInFull(fish));
			((Control)val3).set_Location(new Point(150, 0));
			((Control)val3).set_Width(170);
			((Control)val3).set_Height(40);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(fish.Rarity.ToString());
			val4.set_TextColor(RarityColors.Get(fish.Rarity));
			((Control)val4).set_Location(new Point(320, 0));
			((Control)val4).set_Width(100);
			((Control)val4).set_Height(40);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)row);
			val5.set_Text(FormatHolesShort(fish));
			((Control)val5).set_BasicTooltipText(FormatHolesFull(fish));
			((Control)val5).set_Location(new Point(420, 0));
			((Control)val5).set_Width(130);
			((Control)val5).set_Height(40);
			val5.set_VerticalAlignment((VerticalAlignment)1);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			BuildBaitCell(row, fish, new Point(550, 0));
			BuildTimeOfDayCell(row, fish, new Point(630, 0));
			return row;
		}

		private static string TruncateToWidth(string text, int columnWidthPx)
		{
			int maxChars = (int)((double)columnWidthPx / 7.0);
			if (text.Length <= maxChars)
			{
				return text;
			}
			return text.Substring(0, Math.Max(maxChars - 3, 1)) + "...";
		}

		private static string FormatFoundInShort(Fish fish)
		{
			if (string.IsNullOrEmpty(fish.FoundIn))
			{
				return EnumDisplay.Format(fish.Location);
			}
			string firstPart = fish.FoundIn.Split(new string[2] { " and ", "," }, StringSplitOptions.None)[0].Trim();
			return TruncateToWidth((firstPart.Length < fish.FoundIn.Length) ? (firstPart + ", ...") : firstPart, 170);
		}

		private static string FormatFoundInFull(Fish fish)
		{
			if (!string.IsNullOrEmpty(fish.FoundIn))
			{
				return fish.FoundIn;
			}
			return EnumDisplay.Format(fish.Location);
		}

		private void BuildTimeOfDayCell(Panel row, Fish fish, Point location)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			List<AsyncTexture2D> textures = TimeOfDayIcons.GetTextures(_contentsManager, fish);
			string tooltip = FormatTimeOfDayFull(fish);
			int totalWidth = textures.Count * 28 + (textures.Count - 1) * 4;
			int startX = location.X + (80 - totalWidth) / 2;
			int iconY = location.Y + 6;
			for (int i = 0; i < textures.Count; i++)
			{
				Image val = new Image();
				((Control)val).set_Parent((Container)(object)row);
				val.set_Texture(textures[i]);
				((Control)val).set_Location(new Point(startX + i * 32, iconY));
				((Control)val).set_Size(new Point(28, 28));
				((Control)val).set_BasicTooltipText(tooltip);
			}
		}

		private void BuildBaitCell(Panel row, Fish fish, Point location)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = BaitIcons.GetTexture(fish.Bait);
			if (texture != null)
			{
				Image val = new Image();
				((Control)val).set_Parent((Container)(object)row);
				val.set_Texture(texture);
				((Control)val).set_Location(new Point(location.X + 24, 4));
				((Control)val).set_Size(new Point(32, 32));
				((Control)val).set_BasicTooltipText(EnumDisplay.Format(fish.Bait));
			}
			else
			{
				string baitText = EnumDisplay.Format(fish.Bait);
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(TruncateToWidth(baitText, 80));
				((Control)val2).set_BasicTooltipText(baitText);
				((Control)val2).set_Location(location);
				((Control)val2).set_Width(80);
				((Control)val2).set_Height(40);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
			}
		}

		private static string FormatHolesShort(Fish fish)
		{
			List<string> holes = (from h in new FishingHole?[4] { fish.Hole1, fish.Hole2, fish.Hole3, fish.Hole4 }
				where h.HasValue
				select EnumDisplay.Format(h.Value)).ToList();
			if (holes.Count == 0)
			{
				return "Any";
			}
			return TruncateToWidth((holes.Count == 1) ? holes[0] : (holes[0] + ", ..."), 130);
		}

		private static string FormatHolesFull(Fish fish)
		{
			IEnumerable<string> holes = from h in new FishingHole?[4] { fish.Hole1, fish.Hole2, fish.Hole3, fish.Hole4 }
				where h.HasValue
				select EnumDisplay.Format(h.Value);
			string joined = string.Join(", ", holes);
			if (!string.IsNullOrEmpty(joined))
			{
				return joined;
			}
			return "Any";
		}

		private static string FormatTimeOfDayFull(Fish fish)
		{
			if (fish.TimeOfDay == TimeOfDay.Any)
			{
				if (!fish.HigherChance.HasValue)
				{
					return "Any";
				}
				return $"Any (favors {fish.HigherChance})";
			}
			if (fish.TimeOfDay2.HasValue)
			{
				string text = $"{fish.TimeOfDay}/{fish.TimeOfDay2}";
				if (!fish.HigherChance.HasValue)
				{
					return text;
				}
				return $"{text} (favors {fish.HigherChance})";
			}
			return fish.TimeOfDay.ToString();
		}
	}
}
