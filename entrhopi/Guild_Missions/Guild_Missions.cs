using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using entrhopi.Guild_Missions.Strings;

namespace entrhopi.Guild_Missions
{
	[Export(typeof(Module))]
	public class Guild_Missions : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static Module ModuleInstance;

		private const int TOP_MARGIN = 10;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private const int LEFT_MARGIN = 9;

		private const int MAX_RESULT_COUNT = 7;

		private Panel trekListPanel;

		private Panel savedTrekListPanel;

		private Panel contentPanel;

		private Panel listPanel;

		private Panel infoPanel;

		public List<Panel> resultPanels = new List<Panel>();

		private Dictionary<int, int> savedGuildTreks = new Dictionary<int, int>();

		private Texture2D _guildMissionIcon;

		private Texture2D _guildTrekIcon;

		private Texture2D _guildBountyIcon;

		private Texture2D _guildRaceIcon;

		private Texture2D _guildPuzzleIcon;

		private Texture2D _guildChallengeIcon;

		private Texture2D _lockedIcon;

		private Texture2D _wipIcon;

		private Texture2D _waypointIcon;

		private Texture2D _rightArrowIcon;

		private Texture2D _closeTexture;

		internal string GuildMissionsTabName = Common.gmTabName;

		private WindowTab _moduleTab;

		private TextBox searchTextBox;

		private string ShortUserLocale = "en";

		private Dictionary<int, Texture2D> _guildRaceMap = new Dictionary<int, Texture2D>();

		private int panelsize = 56;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Guild_Missions([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = (Module)(object)this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			_guildMissionIcon = ContentsManager.GetTexture("528697.png");
			_guildTrekIcon = ContentsManager.GetTexture("1228320.png");
			_guildBountyIcon = ContentsManager.GetTexture("1228316.png");
			_guildRaceIcon = ContentsManager.GetTexture("1228319.png");
			_guildPuzzleIcon = ContentsManager.GetTexture("1228318.png");
			_guildChallengeIcon = ContentsManager.GetTexture("1228317.png");
			_lockedIcon = ContentsManager.GetTexture("1827421.png");
			_wipIcon = ContentsManager.GetTexture("2221493.png");
			_waypointIcon = ContentsManager.GetTexture("157354.png");
			_rightArrowIcon = ContentsManager.GetTexture("784266.png");
			_closeTexture = ContentsManager.GetTexture("close_icon.png");
			_guildRaceMap.Add(1, ContentsManager.GetTexture("racemaps/bear_lope.jpg"));
			_guildRaceMap.Add(2, ContentsManager.GetTexture("racemaps/chicken_run.jpg"));
			_guildRaceMap.Add(3, ContentsManager.GetTexture("racemaps/crab_scuttle.jpg"));
			_guildRaceMap.Add(4, ContentsManager.GetTexture("racemaps/devourer_burrow.jpg"));
			_guildRaceMap.Add(5, ContentsManager.GetTexture("racemaps/ghost_wolf_run.jpg"));
			_guildRaceMap.Add(6, ContentsManager.GetTexture("racemaps/quaggan_paddle.jpg"));
			_guildRaceMap.Add(7, ContentsManager.GetTexture("racemaps/spider_scurry.jpg"));
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			switch (((object)(Locale)(ref value)).ToString())
			{
			case "German":
				ShortUserLocale = "de";
				break;
			case "English":
				ShortUserLocale = "en";
				break;
			case "Spanish":
				ShortUserLocale = "es";
				break;
			case "French":
				ShortUserLocale = "fr";
				break;
			}
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_moduleTab = GameService.Overlay.get_BlishHudWindow().AddTab(GuildMissionsTabName, AsyncTexture2D.op_Implicit(_guildMissionIcon), GuildMissionsView((WindowBase)(object)GameService.Overlay.get_BlishHudWindow()));
			((Module)this).OnModuleLoaded(e);
		}

		private Panel GuildMissionsView(WindowBase wndw)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Expected O, but got Unknown
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Expected O, but got Unknown
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Expected O, but got Unknown
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			Rectangle contentRegion = ((Container)wndw).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			Panel parentPanel = val;
			Panel val2 = new Panel();
			val2.set_ShowBorder(true);
			val2.set_Title(Common.gmTypeSelect);
			((Control)val2).set_Size(new Point(265, ((Control)parentPanel).get_Height() - 10));
			((Control)val2).set_Location(new Point(9, 10));
			((Control)val2).set_Parent((Container)(object)parentPanel);
			Panel missionTypePanel = val2;
			Panel val3 = new Panel();
			val3.set_ShowBorder(false);
			((Control)val3).set_Size(new Point(((Control)missionTypePanel).get_Width(), panelsize));
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_Parent((Container)(object)missionTypePanel);
			Panel guildTrekPanel = val3;
			((Control)guildTrekPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guildTrekContent();
			});
			Image val4 = new Image(AsyncTexture2D.op_Implicit(_guildTrekIcon));
			((Control)val4).set_Size(new Point(panelsize, panelsize));
			((Control)val4).set_Location(new Point(0, 0));
			((Control)val4).set_Parent((Container)(object)guildTrekPanel);
			Label val5 = new Label();
			val5.set_Text(Common.gmTypeTrek);
			val5.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val5).set_Location(new Point(9 + panelsize, panelsize / 2 - 10));
			val5.set_TextColor(Color.get_White());
			val5.set_ShadowColor(Color.get_Black());
			val5.set_ShowShadow(true);
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Parent((Container)(object)guildTrekPanel);
			Panel val6 = new Panel();
			val6.set_ShowBorder(false);
			((Control)val6).set_Size(new Point(((Control)missionTypePanel).get_Width(), panelsize));
			((Control)val6).set_Location(new Point(0, panelsize));
			((Control)val6).set_Parent((Container)(object)missionTypePanel);
			Panel guildBountyPanel = val6;
			((Control)guildBountyPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guildBountyContent();
			});
			Image val7 = new Image(AsyncTexture2D.op_Implicit(_guildBountyIcon));
			((Control)val7).set_Size(new Point(panelsize, panelsize));
			((Control)val7).set_Location(new Point(0, 0));
			((Control)val7).set_Parent((Container)(object)guildBountyPanel);
			Label val8 = new Label();
			val8.set_Text(Common.gmTypeBounty);
			val8.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val8).set_Location(new Point(9 + panelsize, panelsize / 2 - 10));
			val8.set_TextColor(Color.get_White());
			val8.set_ShadowColor(Color.get_Black());
			val8.set_ShowShadow(true);
			val8.set_AutoSizeWidth(true);
			val8.set_AutoSizeHeight(true);
			((Control)val8).set_Parent((Container)(object)guildBountyPanel);
			Panel val9 = new Panel();
			val9.set_ShowBorder(false);
			((Control)val9).set_Size(new Point(((Control)missionTypePanel).get_Width(), panelsize));
			((Control)val9).set_Location(new Point(0, panelsize * 2));
			((Control)val9).set_Parent((Container)(object)missionTypePanel);
			Panel guildRacePanel = val9;
			((Control)guildRacePanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guildRaceContent();
			});
			Image val10 = new Image(AsyncTexture2D.op_Implicit(_guildRaceIcon));
			((Control)val10).set_Size(new Point(panelsize, panelsize));
			((Control)val10).set_Location(new Point(0, 0));
			((Control)val10).set_Parent((Container)(object)guildRacePanel);
			Label val11 = new Label();
			val11.set_Text(Common.gmTypeRace);
			val11.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val11).set_Location(new Point(9 + panelsize, panelsize / 2 - 10));
			val11.set_TextColor(Color.get_White());
			val11.set_ShadowColor(Color.get_Black());
			val11.set_ShowShadow(true);
			val11.set_AutoSizeWidth(true);
			val11.set_AutoSizeHeight(true);
			((Control)val11).set_Parent((Container)(object)guildRacePanel);
			Panel val12 = new Panel();
			val12.set_ShowBorder(false);
			((Control)val12).set_Size(new Point(((Control)missionTypePanel).get_Width(), panelsize));
			((Control)val12).set_Location(new Point(0, panelsize * 3));
			((Control)val12).set_Parent((Container)(object)missionTypePanel);
			Panel guildChallengePanel = val12;
			((Control)guildChallengePanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guildChallengeContent();
			});
			Image val13 = new Image(AsyncTexture2D.op_Implicit(_guildChallengeIcon));
			((Control)val13).set_Size(new Point(panelsize, panelsize));
			((Control)val13).set_Location(new Point(0, 0));
			((Control)val13).set_Parent((Container)(object)guildChallengePanel);
			Label val14 = new Label();
			val14.set_Text(Common.gmTypeChallenge);
			val14.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val14).set_Location(new Point(9 + panelsize, panelsize / 2 - 10));
			val14.set_TextColor(Color.get_White());
			val14.set_ShadowColor(Color.get_Black());
			val14.set_ShowShadow(true);
			val14.set_AutoSizeWidth(true);
			val14.set_AutoSizeHeight(true);
			((Control)val14).set_Parent((Container)(object)guildChallengePanel);
			Panel val15 = new Panel();
			val15.set_ShowBorder(false);
			((Control)val15).set_Size(new Point(((Control)missionTypePanel).get_Width(), panelsize));
			((Control)val15).set_Location(new Point(0, panelsize * 4));
			((Control)val15).set_Parent((Container)(object)missionTypePanel);
			Panel guildPuzzlePanel = val15;
			((Control)guildPuzzlePanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				guildPuzzleContent();
			});
			Image val16 = new Image(AsyncTexture2D.op_Implicit(_guildPuzzleIcon));
			((Control)val16).set_Size(new Point(panelsize, panelsize));
			((Control)val16).set_Location(new Point(0, 0));
			((Control)val16).set_Parent((Container)(object)guildPuzzlePanel);
			Label val17 = new Label();
			val17.set_Text(Common.gmTypePuzzle);
			val17.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val17).set_Location(new Point(9 + panelsize, panelsize / 2 - 10));
			val17.set_TextColor(Color.get_White());
			val17.set_ShadowColor(Color.get_Black());
			val17.set_ShowShadow(true);
			val17.set_AutoSizeWidth(true);
			val17.set_AutoSizeHeight(true);
			((Control)val17).set_Parent((Container)(object)guildPuzzlePanel);
			Panel val18 = new Panel();
			val18.set_ShowBorder(false);
			((Control)val18).set_Size(new Point(((Control)parentPanel).get_Width() - ((Control)missionTypePanel).get_Right() - 5, ((Control)parentPanel).get_Height() - 10));
			((Control)val18).set_Location(new Point(((Control)missionTypePanel).get_Right() + 9, 10));
			((Control)val18).set_Parent((Container)(object)parentPanel);
			contentPanel = val18;
			return parentPanel;
		}

		private void guildTrekContent()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Expected O, but got Unknown
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildTrekIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text(Common.gmTypeTrek);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			((Control)val2).set_Location(new Point(82, 18));
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)contentPanel);
			TextBox val3 = new TextBox();
			((TextInputBase)val3).set_PlaceholderText(Common.gmSearchPlaceholder);
			((Control)val3).set_Size(new Point(358, 43));
			((TextInputBase)val3).set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(9, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			searchTextBox = val3;
			((Control)searchTextBox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearSearch();
			});
			((TextInputBase)searchTextBox).add_TextChanged((EventHandler<EventArgs>)SearchboxOnTextChanged);
			Panel val4 = new Panel();
			val4.set_ShowBorder(true);
			val4.set_Title(Common.gmPanelSearchResults);
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - ((Control)searchTextBox).get_Bottom() - 10));
			((Control)val4).set_Location(new Point(6, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			trekListPanel = val4;
			Panel val5 = new Panel();
			val5.set_CanScroll(true);
			val5.set_ShowBorder(true);
			val5.set_Title(Common.gmPanelSavedTreks);
			((Control)val5).set_Size(new Point(364, ((Control)contentPanel).get_Height() - ((Control)searchTextBox).get_Bottom() - 10));
			((Control)val5).set_Location(new Point(((Control)trekListPanel).get_Right() + 9, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val5).set_Parent((Container)(object)contentPanel);
			savedTrekListPanel = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text(Common.gmButtonClearAll);
			((Control)val6).set_Size(new Point(110, 30));
			((Control)val6).set_Location(new Point(((Control)trekListPanel).get_Right() + 20, ((Control)searchTextBox).get_Top() - 1));
			((Control)val6).set_Parent((Container)(object)contentPanel);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearWPList();
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text(Common.gmButtonExport);
			((Control)val7).set_Size(new Point(110, 30));
			((Control)val7).set_Location(new Point(((Control)trekListPanel).get_Right() + 130 + 9, ((Control)searchTextBox).get_Top() - 1));
			((Control)val7).set_Parent((Container)(object)contentPanel);
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ExportWPList();
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text(Common.gmButtonImport);
			((Control)val8).set_Size(new Point(110, 30));
			((Control)val8).set_Location(new Point(((Control)trekListPanel).get_Right() + 250 + 9, ((Control)searchTextBox).get_Top() - 1));
			((Control)val8).set_Parent((Container)(object)contentPanel);
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImportWPList();
			});
			StandardButton val9 = new StandardButton();
			val9.set_Text(Common.gmButtonSendToChat);
			((Control)val9).set_Size(new Point(110, 30));
			((Control)val9).set_Location(new Point(((Control)trekListPanel).get_Right() + 20, ((Control)searchTextBox).get_Top() - 31));
			((Control)val9).set_Parent((Container)(object)contentPanel);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				sendToChat();
			});
			UpdateSavedWPList();
		}

		private void guildRaceContent()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildRaceIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text(Common.gmTypeRace);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			((Control)val2).set_Location(new Point(82, 18));
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)contentPanel);
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			val3.set_Title(Common.gmPanelList);
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title(Common.gmPanelInfo);
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument xDocument = XDocument.Load(ContentsManager.GetFileStream("XML\\races.xml"));
			int i = 0;
			foreach (XElement race in xDocument.Root.Elements("race"))
			{
				ViewInfoPanel(race, listPanel, i, "race");
				i++;
			}
		}

		private void guildBountyContent()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildBountyIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text(Common.gmTypeBounty);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			((Control)val2).set_Location(new Point(82, 18));
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)contentPanel);
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			val3.set_Title(Common.gmPanelList);
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10 - 72));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title(Common.gmPanelInfo);
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument xDocument = XDocument.Load(ContentsManager.GetFileStream("XML\\bounties.xml"));
			int i = 0;
			foreach (XElement bounty in xDocument.Root.Elements("bounty"))
			{
				ViewInfoPanel(bounty, listPanel, i, "bounty", 20);
				i++;
			}
			listPanel.set_CanScroll(true);
		}

		private void guildChallengeContent()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildChallengeIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text(Common.gmTypeChallenge);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			((Control)val2).set_Location(new Point(82, 18));
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)contentPanel);
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			val3.set_Title(Common.gmPanelList);
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title(Common.gmPanelInfo);
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument xDocument = XDocument.Load(ContentsManager.GetFileStream("XML\\challenges.xml"));
			int i = 0;
			foreach (XElement challenge in xDocument.Root.Elements("challenge"))
			{
				ViewInfoPanel(challenge, listPanel, i, "challenge");
				i++;
			}
		}

		private void guildPuzzleContent()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildPuzzleIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text(Common.gmTypePuzzle);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			((Control)val2).set_Location(new Point(82, 18));
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)contentPanel);
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			val3.set_Title(Common.gmPanelList);
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title(Common.gmPanelInfo);
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument xDocument = XDocument.Load(ContentsManager.GetFileStream("XML\\puzzles.xml"));
			int i = 0;
			foreach (XElement puzzle in xDocument.Root.Elements("puzzle"))
			{
				ViewInfoPanel(puzzle, listPanel, i, "puzzle");
				i++;
			}
		}

		private void SearchboxOnTextChanged(object sender, EventArgs e)
		{
			int i = 0;
			string searchText = ((TextInputBase)searchTextBox).get_Text();
			((Container)trekListPanel).ClearChildren();
			foreach (XElement trek in XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml")).Root.Elements("trek"))
			{
				if (trek.Element("name_" + ShortUserLocale).Value.ToLower().StartsWith(searchText.ToLower()))
				{
					AddTrekPanel(trek, trekListPanel, i, add: true);
					i++;
					if (i >= 7)
					{
						break;
					}
				}
			}
		}

		private void AddWPToList(int trekID, int mapID)
		{
			if (!savedGuildTreks.ContainsKey(trekID))
			{
				savedGuildTreks.Add(trekID, mapID);
				UpdateSavedWPList();
			}
		}

		private void RemoveWPFromList(int trek)
		{
			savedGuildTreks.Remove(trek);
			UpdateSavedWPList();
		}

		private void ClearWPList()
		{
			savedGuildTreks.Clear();
			((Container)savedTrekListPanel).ClearChildren();
		}

		private void ClearSearch()
		{
			((TextInputBase)searchTextBox).set_Text("");
		}

		private void sendToChat()
		{
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml"));
			int i = 0;
			string export = "";
			foreach (KeyValuePair<int, int> wp in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				XElement trek = (from x in doc.Descendants("trek")
					where x.Element("id").Value == wp.Key.ToString()
					select x).FirstOrDefault();
				if (trek != null)
				{
					export = export + trek.Element("name_" + ShortUserLocale).Value + " " + trek.Element("chat_link").Value + " ";
					i++;
				}
			}
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(export).ContinueWith(delegate(Task<bool> clipboardResult)
			{
				if (clipboardResult.IsFaulted)
				{
					ScreenNotification.ShowNotification("Failed to copy waypoint to clipboard. Try again.", (NotificationType)6, (Texture2D)null, 2);
				}
				else
				{
					ScreenNotification.ShowNotification("Copied waypoint to clipboard!", (NotificationType)0, (Texture2D)null, 2);
				}
			});
		}

		private void ExportWPList()
		{
			int i = 0;
			string export = "BlishGM";
			foreach (KeyValuePair<int, int> wp in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				i++;
				export = export + ";" + wp.Key;
			}
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(export).ContinueWith(delegate(Task<bool> clipboardResult)
			{
				if (clipboardResult.IsFaulted)
				{
					ScreenNotification.ShowNotification(Common.gmNotificationClipboardError, (NotificationType)6, (Texture2D)null, 2);
				}
				else
				{
					ScreenNotification.ShowNotification(string.Format(Common.gmNotificationClipboardSaved, i), (NotificationType)0, (Texture2D)null, 2);
				}
			});
		}

		private void ImportWPList()
		{
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml"));
			ClipboardUtil.get_WindowsClipboardService().GetTextAsync().ContinueWith(delegate(Task<string> import)
			{
				if (!import.IsFaulted)
				{
					if (!string.IsNullOrEmpty(import.Result))
					{
						int num = 0;
						string[] array = import.Result.Split(';');
						foreach (string wp in array)
						{
							if (num == 0 && string.Equals(wp, "BlishGM"))
							{
								num++;
							}
							else
							{
								if (num == 0 && !string.Equals(wp, "BlishGM"))
								{
									return;
								}
								Logger.Warn((Exception)import.Exception, num + ":" + wp);
								XElement xElement = (from x in doc.Descendants("trek")
									where x.Element("id").Value == wp
									select x).FirstOrDefault();
								if (xElement != null)
								{
									AddWPToList((int)xElement.Element("id"), (int)xElement.Element("map_id"));
									num++;
								}
							}
						}
						ScreenNotification.ShowNotification(string.Format(Common.gmNotificationClipboardRead, num - 1), (NotificationType)0, (Texture2D)null, 2);
					}
				}
				else
				{
					Logger.Warn((Exception)import.Exception, "Failed to read clipboard text from system clipboard!");
				}
			});
		}

		private void UpdateSavedWPList()
		{
			((Container)savedTrekListPanel).ClearChildren();
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml"));
			int i = 0;
			foreach (KeyValuePair<int, int> wp in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				XElement trek = (from x in doc.Descendants("trek")
					where x.Element("id").Value == wp.Key.ToString()
					select x).FirstOrDefault();
				if (trek != null)
				{
					AddTrekPanel(trek, savedTrekListPanel, i, add: false, remove: true);
					i++;
				}
			}
		}

		private void AddTrekPanel(XElement trek, Panel parent, int position, bool add = false, bool remove = false)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Size(new Point(((Control)parent).get_Width(), 70));
			((Control)val).set_Location(new Point(9, 5 + position * 70));
			((Control)val).set_Parent((Container)(object)parent);
			Panel trekPanel = val;
			Image val2 = new Image(AsyncTexture2D.op_Implicit(_waypointIcon));
			((Control)val2).set_Size(new Point(50, 50));
			((Control)val2).set_Location(new Point(0, 4));
			((Control)val2).set_Parent((Container)(object)trekPanel);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(trek.Element("name_" + ShortUserLocale).Value + " " + trek.Element("chat_link").Value).ContinueWith(delegate(Task<bool> clipboardResult)
				{
					if (clipboardResult.IsFaulted)
					{
						ScreenNotification.ShowNotification("Failed to copy waypoint to clipboard. Try again.", (NotificationType)6, (Texture2D)null, 2);
					}
					else
					{
						ScreenNotification.ShowNotification("Copied waypoint to clipboard!", (NotificationType)0, (Texture2D)null, 2);
					}
				});
			});
			Label val3 = new Label();
			val3.set_Text(trek.Element("name_" + ShortUserLocale).Value + " (" + trek.Element("map_name_" + ShortUserLocale).Value + ")");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(59, 3));
			val3.set_TextColor(Color.get_White());
			val3.set_ShadowColor(Color.get_Black());
			val3.set_ShowShadow(true);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)trekPanel);
			Label val4 = new Label();
			val4.set_Text(trek.Element("waypoint_name_" + ShortUserLocale).Value);
			val4.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val4).set_Location(new Point(59, 32));
			val4.set_TextColor(Color.get_Silver());
			val4.set_ShadowColor(Color.get_Black());
			val4.set_ShowShadow(true);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Parent((Container)(object)trekPanel);
			if (add)
			{
				Image val5 = new Image(AsyncTexture2D.op_Implicit(_rightArrowIcon));
				((Control)val5).set_Size(new Point(70, 70));
				((Control)val5).set_Location(new Point(((Control)parent).get_Width() - 70, -10));
				((Control)val5).set_Parent((Container)(object)trekPanel);
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					AddWPToList((int)trek.Element("id"), (int)trek.Element("map_id"));
				});
			}
			if (remove)
			{
				Image val6 = new Image(AsyncTexture2D.op_Implicit(_closeTexture));
				((Control)val6).set_Size(new Point(20, 20));
				((Control)val6).set_Location(new Point(((Control)parent).get_Width() - 40, 4));
				((Control)val6).set_Parent((Container)(object)trekPanel);
				((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					RemoveWPFromList((int)trek.Element("id"));
				});
			}
		}

		private void ViewInfoPanel(XElement element, Panel parent, int position, string type, int offset = 0)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Size(new Point(((Control)parent).get_Width(), 70));
			((Control)val).set_Location(new Point(9, 5 + position * 70));
			((Control)val).set_Parent((Container)(object)parent);
			Panel trekPanel = val;
			Image val2 = new Image(AsyncTexture2D.op_Implicit(_waypointIcon));
			((Control)val2).set_Size(new Point(50, 50));
			((Control)val2).set_Location(new Point(0, 4));
			((Control)val2).set_Parent((Container)(object)trekPanel);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(element.Element("name_" + ShortUserLocale).Value + " " + element.Element("chat_link").Value).ContinueWith(delegate(Task<bool> clipboardResult)
				{
					if (clipboardResult.IsFaulted)
					{
						ScreenNotification.ShowNotification("Failed to copy waypoint to clipboard. Try again.", (NotificationType)6, (Texture2D)null, 2);
					}
					else
					{
						ScreenNotification.ShowNotification("Copied waypoint to clipboard!", (NotificationType)0, (Texture2D)null, 2);
					}
				});
			});
			Label val3 = new Label();
			val3.set_Text(element.Element("name_" + ShortUserLocale).Value + " (" + element.Element("map_name_" + ShortUserLocale).Value + ")");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(59, 3));
			val3.set_TextColor(Color.get_White());
			val3.set_ShadowColor(Color.get_Black());
			val3.set_ShowShadow(true);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)trekPanel);
			Label val4 = new Label();
			val4.set_Text(element.Element("waypoint_name_" + ShortUserLocale).Value);
			val4.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val4).set_Location(new Point(59, 32));
			val4.set_TextColor(Color.get_Silver());
			val4.set_ShadowColor(Color.get_Black());
			val4.set_ShowShadow(true);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Parent((Container)(object)trekPanel);
			Image val5 = new Image(AsyncTexture2D.op_Implicit(_rightArrowIcon));
			((Control)val5).set_Size(new Point(70, 70));
			((Control)val5).set_Location(new Point(((Control)parent).get_Width() - 70 - offset, -10));
			((Control)val5).set_Parent((Container)(object)trekPanel);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DisplayInfo((int)element.Element("id"), type, element);
			});
		}

		private void DisplayInfo(int v, string type, XElement element)
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			int offset = 0;
			((Container)infoPanel).ClearChildren();
			infoPanel.set_Title(Common.gmPanelInfo + ": " + element.Element("name_" + ShortUserLocale).Value);
			if (element.Element("wiki_link_" + ShortUserLocale) != null)
			{
				StandardButton val = new StandardButton();
				val.set_Text(Common.gmButtonWiki);
				((Control)val).set_Size(new Point(110, 30));
				((Control)val).set_Location(new Point(4, 4));
				((Control)val).set_Parent((Container)(object)infoPanel);
				((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start(element.Element("wiki_link_" + ShortUserLocale).Value);
				});
				offset += 40;
			}
			if (type == "race")
			{
				Image val2 = new Image(AsyncTexture2D.op_Implicit(_guildRaceMap[v]));
				((Control)val2).set_Size(new Point(310, 500));
				((Control)val2).set_Location(new Point(4, 4 + offset));
				((Control)val2).set_Parent((Container)(object)infoPanel);
			}
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_moduleTab);
			ModuleInstance = null;
		}
	}
}
