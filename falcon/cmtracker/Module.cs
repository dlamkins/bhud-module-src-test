using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using falcon.cmtracker.Controls;
using falcon.cmtracker.Persistance;

namespace falcon.cmtracker
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static Module ModuleInstance;

		public static SettingEntry<bool> W3_Keep_Construct;

		public static SettingEntry<bool> W4_Cairn;

		public static SettingEntry<bool> W4_Mursaat_Overseer;

		public static SettingEntry<bool> W4_Samarog;

		public static SettingEntry<bool> W5_Soulless_Horror;

		public static SettingEntry<bool> W4_Deimos;

		public static SettingEntry<bool> W5_Dhuum;

		public static SettingEntry<bool> W6_Conjured_Amalgamate;

		public static SettingEntry<bool> W6_Twin_Largos;

		public static SettingEntry<bool> W7_Sabir;

		public static SettingEntry<bool> W7_Adina;

		public static SettingEntry<bool> W7_Qadim2;

		public static SettingEntry<bool> W6_Qadim;

		public static SettingEntry<bool> Strike_Aetherblade_Hideout;

		public static SettingEntry<bool> Strike_Xunlai_Jade_Junkyard;

		public static SettingEntry<bool> Strike_Kaineng_Overlook;

		public static SettingEntry<bool> Strike_Harvest_Temple;

		internal Texture2D _CmClearsIconTexture;

		internal Texture2D _CmClearsLogoTexture;

		internal Texture2D _deletedItemTexture;

		internal Texture2D _sortByStrikeTexture;

		internal Texture2D _sortByRaidTexture;

		internal Texture2D _notificationBackroundTexture;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private WindowTab _ClearsTab;

		private Panel _modulePanel;

		private List<BossButton> _displayedBosses;

		private Panel _squadPanel;

		private string CmTrakcerTabName = "CM Tracker";

		private string SORTBY_ALL;

		private string SORTBY_STRIKES;

		private string SORTBY_RAID;

		private string VisitUsAndHelpText;

		private string ClearCheckboxTooltipText;

		private string CurrentSortMethod;

		private Bosses _myBossesClears;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection obj = settings.AddSubCollection("Managed Settings", false);
			W3_Keep_Construct = obj.DefineSetting<bool>("W3_Keep_Construct", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W4_Cairn = obj.DefineSetting<bool>("W4_Cairn", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W4_Mursaat_Overseer = obj.DefineSetting<bool>("W4_Mursaat_Overseer", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W4_Samarog = obj.DefineSetting<bool>("W4_Samarog", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W4_Deimos = obj.DefineSetting<bool>("W4_Deimos", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W5_Soulless_Horror = obj.DefineSetting<bool>("W5_Soulless_Horror", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W5_Dhuum = obj.DefineSetting<bool>("W5_Dhuum", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W6_Conjured_Amalgamate = obj.DefineSetting<bool>("W6_Conjured_Amalgamate", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W6_Twin_Largos = obj.DefineSetting<bool>("W6_Twin_Largos", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W7_Adina = obj.DefineSetting<bool>("W7_Adina", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W7_Sabir = obj.DefineSetting<bool>("W7_Sabir", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W7_Qadim2 = obj.DefineSetting<bool>("W7_Qadim2", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			W6_Qadim = obj.DefineSetting<bool>("W6_Qadim", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			Strike_Aetherblade_Hideout = obj.DefineSetting<bool>("Strike_Aetherblade_Hideout", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			Strike_Xunlai_Jade_Junkyard = obj.DefineSetting<bool>("Strike_Xunlai_Jade_Junkyard", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			Strike_Kaineng_Overlook = obj.DefineSetting<bool>("Strike_Kaineng_Overlook", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
			Strike_Harvest_Temple = obj.DefineSetting<bool>("Strike_Harvest_Temple", false, (string)null, (string)null, (SettingTypeRendererDelegate)null);
		}

		protected override void Initialize()
		{
			_displayedBosses = new List<BossButton>();
			LoadTextures();
		}

		protected override async Task LoadAsync()
		{
			if (_myBossesClears == null)
			{
				_myBossesClears = new Bosses();
			}
		}

		private void ChangeLocalization(object sender, EventArgs e)
		{
			SORTBY_ALL = "Show All";
			SORTBY_STRIKES = "Strike";
			SORTBY_RAID = "Raid";
			CurrentSortMethod = SORTBY_ALL;
			Panel modulePanel = _modulePanel;
			if (modulePanel != null)
			{
				((Control)modulePanel).Dispose();
			}
			_modulePanel = BuildHomePanel((WindowBase)(object)GameService.Overlay.get_BlishHudWindow());
			if (_ClearsTab != null)
			{
				GameService.Overlay.get_BlishHudWindow().RemoveTab(_ClearsTab);
			}
			_ClearsTab = GameService.Overlay.get_BlishHudWindow().AddTab(CmTrakcerTabName, AsyncTexture2D.op_Implicit(_CmClearsIconTexture), _modulePanel, 0);
		}

		private void LoadTextures()
		{
			_CmClearsIconTexture = ContentsManager.GetTexture("logo.png");
			_CmClearsLogoTexture = ContentsManager.GetTexture("logo.png");
			_deletedItemTexture = ContentsManager.GetTexture("deleted_item.png");
			_sortByStrikeTexture = ContentsManager.GetTexture("icon_strike.png");
			_sortByRaidTexture = ContentsManager.GetTexture("icon_raid.png");
			_notificationBackroundTexture = ContentsManager.GetTexture("ns-button.png");
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			ChangeLocalization(null, null);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)ChangeLocalization);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			Panel squadPanel = _squadPanel;
			if (squadPanel != null)
			{
				((Control)squadPanel).Dispose();
			}
			foreach (BossButton displayedBoss in _displayedBosses)
			{
				if (displayedBoss != null)
				{
					((Control)displayedBoss).Dispose();
				}
			}
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_ClearsTab);
			ModuleInstance = null;
		}

		private Panel BuildHomePanel(WindowBase wndw)
		{
			return BuildTokensPanel(wndw);
		}

		public Panel BuildTokensPanel(WindowBase wndw)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_CanScroll(false);
			Rectangle contentRegion = ((Container)wndw).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			Panel hPanel = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent((Container)(object)hPanel);
			LoadingSpinner pageLoading = val2;
			((Control)pageLoading).set_Location(new Point(((Control)hPanel).get_Size().X / 2 - ((Control)pageLoading).get_Size().X / 2, ((Control)hPanel).get_Size().Y / 2 - ((Control)pageLoading).get_Size().Y / 2));
			foreach (BossButton displayedBoss in _displayedBosses)
			{
				((Control)displayedBoss).Dispose();
			}
			_displayedBosses.Clear();
			FinishLoadingCmTrackerPanel(wndw, hPanel, _myBossesClears);
			((Control)pageLoading).Dispose();
			return hPanel;
		}

		private void RepositionTokens()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			int pos = 0;
			foreach (BossButton e in _displayedBosses)
			{
				int x = pos % 3;
				int y = pos / 3;
				((Control)e).set_Location(new Point(x * (((Control)e).get_Width() + 8), y * (((Control)e).get_Height() + 8)));
				((Container)(Panel)((Control)e).get_Parent()).set_VerticalScrollOffset(0);
				((Control)((Control)e).get_Parent()).Invalidate();
				if (((Control)e).get_Visible())
				{
					pos++;
				}
			}
		}

		private void MousePressedSortButton(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Control bSortMethod = (Control)sender;
			bSortMethod.set_Size(new Point(bSortMethod.get_Size().X - 4, bSortMethod.get_Size().Y - 4));
			CurrentSortMethod = bSortMethod.get_BasicTooltipText();
			foreach (BossButton boss in _displayedBosses)
			{
				if (CurrentSortMethod == SORTBY_RAID && boss.Token.bossType == BossType.Raid)
				{
					((Control)boss).set_Visible(true);
				}
				else if (CurrentSortMethod == SORTBY_STRIKES && boss.Token.bossType == BossType.Strike)
				{
					((Control)boss).set_Visible(true);
				}
				else if (CurrentSortMethod == SORTBY_ALL)
				{
					((Control)boss).set_Visible(true);
				}
				else
				{
					((Control)boss).set_Visible(false);
				}
			}
			RepositionTokens();
		}

		private void MouseLeftSortButton(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			((Control)sender).set_Size(new Point(32, 32));
		}

		private void FinishLoadingCmTrackerPanel(WindowBase wndw, Panel hPanel, Bosses currentClears)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Expected O, but got Unknown
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Expected O, but got Unknown
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Expected O, but got Unknown
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Expected O, but got Unknown
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Expected O, but got Unknown
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Expected O, but got Unknown
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Expected O, but got Unknown
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Expected O, but got Unknown
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)hPanel);
			((Control)val).set_Size(new Point(((Control)hPanel).get_Width(), 50));
			((Control)val).set_Location(new Point(0, 0));
			val.set_CanScroll(false);
			Panel header = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)header);
			((Control)val2).set_Size(new Point(140, 32));
			((Control)val2).set_Location(new Point(((Control)header).get_Right() - 140 - 5, ((Control)header).get_Location().Y));
			val2.set_ShowTint(true);
			Panel sortingsMenu = val2;
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)sortingsMenu);
			((Control)val3).set_Size(new Point(32, 32));
			((Control)val3).set_Location(new Point(5, 0));
			val3.set_Texture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("255369")));
			((Control)val3).set_BackgroundColor(Color.get_Transparent());
			((Control)val3).set_BasicTooltipText(SORTBY_ALL);
			Image bSortByAll = val3;
			((Control)bSortByAll).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByAll).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByAll).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val4 = new Image();
			((Control)val4).set_Parent((Container)(object)sortingsMenu);
			((Control)val4).set_Size(new Point(32, 32));
			((Control)val4).set_Location(new Point(((Control)bSortByAll).get_Right() + 20 + 5, 0));
			val4.set_Texture(AsyncTexture2D.op_Implicit(_sortByRaidTexture));
			((Control)val4).set_BasicTooltipText(SORTBY_RAID);
			Image bSortByRaid = val4;
			((Control)bSortByRaid).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByRaid).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByRaid).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val5 = new Image();
			((Control)val5).set_Parent((Container)(object)sortingsMenu);
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_Location(new Point(((Control)bSortByRaid).get_Right() + 5, 0));
			val5.set_Texture(AsyncTexture2D.op_Implicit(_sortByStrikeTexture));
			((Control)val5).set_BasicTooltipText(SORTBY_STRIKES);
			((Control)val5).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)val5).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)val5).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)hPanel);
			((Control)val6).set_Size(new Point(((Control)hPanel).get_Width(), 50));
			((Control)val6).set_Location(new Point(0, ((Control)hPanel).get_Height() - 50));
			val6.set_CanScroll(false);
			Panel footer = val6;
			Panel val7 = new Panel();
			((Control)val7).set_Parent((Container)(object)hPanel);
			((Control)val7).set_Size(new Point(((Control)header).get_Size().X, ((Control)hPanel).get_Height() - ((Control)header).get_Height() - ((Control)footer).get_Height()));
			((Control)val7).set_Location(new Point(0, ((Control)header).get_Bottom()));
			val7.set_ShowBorder(true);
			val7.set_CanScroll(true);
			val7.set_ShowTint(true);
			_squadPanel = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)hPanel);
			((Control)val8).set_Size(new Point(200, 30));
			((Control)val8).set_Location(new Point(((Control)_squadPanel).get_Location().X + ((Control)_squadPanel).get_Width() - 200 - 5, ((Control)_squadPanel).get_Location().Y + ((Control)_squadPanel).get_Height() + 10));
			val8.set_Text("Reset Weekly Clears");
			((Control)val8).set_BasicTooltipText(ClearCheckboxTooltipText);
			((Control)val8).set_Visible(true);
			StandardButton clearCheckbox = val8;
			Panel val9 = new Panel();
			((Control)val9).set_Parent((Container)(object)hPanel);
			((Control)val9).set_Size(new Point(450, 100));
			((Control)val9).set_Location(new Point(((Control)_squadPanel).get_Location().X + ((Control)_squadPanel).get_Width() - 450 - 5, ((Control)_squadPanel).get_Location().Y + ((Control)_squadPanel).get_Height() + 10));
			val9.set_ShowBorder(false);
			val9.set_CanScroll(false);
			val9.set_ShowTint(false);
			((Control)val9).set_Visible(false);
			Panel confirmPanel = val9;
			TextBox val10 = new TextBox();
			((Control)val10).set_Parent((Container)(object)confirmPanel);
			((Control)val10).set_Size(new Point(300, 30));
			((Control)val10).set_Location(new Point(5, 5));
			((TextInputBase)val10).set_Text("Are you sure you want to rest weekly clears?");
			((Control)val10).set_BackgroundColor(Color.get_Transparent());
			TextBox confirmText = val10;
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)confirmPanel);
			((Control)val11).set_Size(new Point(50, 30));
			((Control)val11).set_Location(new Point(((Control)confirmText).get_Right() + 5, ((Control)confirmText).get_Location().Y));
			val11.set_Text("Yes");
			StandardButton yesButton = val11;
			StandardButton val12 = new StandardButton();
			((Control)val12).set_Parent((Container)(object)confirmPanel);
			((Control)val12).set_Size(new Point(50, 30));
			((Control)val12).set_Location(new Point(((Control)yesButton).get_Right() + 5, ((Control)confirmText).get_Location().Y));
			val12.set_Text("No");
			StandardButton noButton = val12;
			((Container)confirmPanel).AddChild((Control)(object)confirmText);
			((Container)confirmPanel).AddChild((Control)(object)noButton);
			((Container)confirmPanel).AddChild((Control)(object)yesButton);
			((Control)noButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)clearCheckbox).set_Visible(true);
				((Control)confirmPanel).set_Visible(false);
			});
			((Control)yesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				foreach (Token token in currentClears.Tokens)
				{
					token.setting.set_Value(false);
				}
				foreach (BossButton displayedBoss in _displayedBosses)
				{
					displayedBoss.Background = (displayedBoss.Token.setting.get_Value() ? Color.get_Green() : Color.get_Black());
				}
				((Control)clearCheckbox).set_Visible(true);
				((Control)confirmPanel).set_Visible(false);
			});
			((Control)clearCheckbox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)clearCheckbox).set_Visible(false);
				((Control)confirmPanel).set_Visible(true);
			});
			Panel val13 = new Panel();
			((Control)val13).set_Parent((Container)(object)hPanel);
			((Control)val13).set_Size(new Point(((Control)header).get_Size().X, ((Control)hPanel).get_Height() - ((Control)header).get_Height() - ((Control)footer).get_Height()));
			((Control)val13).set_Location(new Point(0, ((Control)header).get_Bottom()));
			val13.set_ShowBorder(true);
			val13.set_CanScroll(true);
			val13.set_ShowTint(true);
			Panel contentPanel = val13;
			if (currentClears.Tokens != null)
			{
				foreach (Token boss in currentClears.Tokens)
				{
					BossButton bossButton = new BossButton();
					((Control)bossButton).set_Parent((Container)(object)contentPanel);
					((DetailsButton)bossButton).set_Icon(AsyncTexture2D.op_Implicit((boss.Icon == null) ? ContentsManager.GetTexture("icon_token.png") : ContentsManager.GetTexture(boss.Icon)));
					bossButton.Font = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)0);
					((DetailsButton)bossButton).set_Text(boss.Name);
					((DetailsButton)bossButton).set_HighlightType((DetailsHighlightType)1);
					bossButton.Token = boss;
					bossButton.Background = (boss.setting.get_Value() ? Color.get_Green() : Color.get_Black());
					BossButton BossButton = bossButton;
					((Control)BossButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						//IL_003b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0042: Unknown result type (might be due to invalid IL or missing references)
						boss.setting.set_Value(!boss.setting.get_Value());
						BossButton.Background = (boss.setting.get_Value() ? Color.get_Green() : Color.get_Black());
					});
					_displayedBosses.Add(BossButton);
				}
			}
			RepositionTokens();
		}
	}
}
