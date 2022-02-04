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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		internal string GuildTrekTabName = "Guild Missions";

		private WindowTab _moduleTab;

		private TextBox searchTextBox;

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
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_moduleTab = GameService.Overlay.get_BlishHudWindow().AddTab(GuildTrekTabName, AsyncTexture2D.op_Implicit(_guildMissionIcon), GuildMissionsView((WindowBase)(object)GameService.Overlay.get_BlishHudWindow()));
			((Module)this).OnModuleLoaded(e);
		}

		private Panel GuildMissionsView(WindowBase wndw)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Expected O, but got Unknown
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Expected O, but got Unknown
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Expected O, but got Unknown
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0547: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0578: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			Rectangle contentRegion = ((Container)wndw).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			Panel parentPanel = val;
			Panel val2 = new Panel();
			val2.set_ShowBorder(true);
			val2.set_Title("Choose Guild Mission Type");
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
			val5.set_Text("Trek");
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
			val8.set_Text("Bounty");
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
			val11.set_Text("Race");
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
			val14.set_Text("Challenge");
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
			val17.set_Text("Puzzle");
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
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Expected O, but got Unknown
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Expected O, but got Unknown
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Expected O, but got Unknown
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildTrekIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text("Trek");
			val2.set_Font(GameService.Content.get_DefaultFont32());
			((Control)val2).set_Location(new Point(82, 18));
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)contentPanel);
			TextBox val3 = new TextBox();
			((TextInputBase)val3).set_PlaceholderText("Enter name here ...");
			((Control)val3).set_Size(new Point(358, 43));
			((TextInputBase)val3).set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(9, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			searchTextBox = val3;
			((TextInputBase)searchTextBox).add_TextChanged((EventHandler<EventArgs>)SearchboxOnTextChanged);
			Panel val4 = new Panel();
			val4.set_ShowBorder(true);
			val4.set_Title("Search Results");
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - ((Control)searchTextBox).get_Bottom() - 10));
			((Control)val4).set_Location(new Point(6, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			trekListPanel = val4;
			Panel val5 = new Panel();
			val5.set_CanScroll(true);
			val5.set_ShowBorder(true);
			val5.set_Title("Saved Treks");
			((Control)val5).set_Size(new Point(364, ((Control)contentPanel).get_Height() - ((Control)searchTextBox).get_Bottom() - 10));
			((Control)val5).set_Location(new Point(((Control)trekListPanel).get_Right() + 9, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val5).set_Parent((Container)(object)contentPanel);
			savedTrekListPanel = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Clear All");
			((Control)val6).set_Size(new Point(110, 30));
			((Control)val6).set_Location(new Point(((Control)trekListPanel).get_Right() + 20, ((Control)searchTextBox).get_Top() - 1));
			((Control)val6).set_Parent((Container)(object)contentPanel);
			StandardButton clearAllButton = val6;
			((Control)clearAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearWPList();
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Export");
			((Control)val7).set_Size(new Point(110, 30));
			((Control)val7).set_Location(new Point(((Control)trekListPanel).get_Right() + 130 + 9, ((Control)searchTextBox).get_Top() - 1));
			((Control)val7).set_Parent((Container)(object)contentPanel);
			StandardButton exportButton = val7;
			((Control)exportButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ExportWPList();
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text("Import");
			((Control)val8).set_Size(new Point(110, 30));
			((Control)val8).set_Location(new Point(((Control)trekListPanel).get_Right() + 250 + 9, ((Control)searchTextBox).get_Top() - 1));
			((Control)val8).set_Parent((Container)(object)contentPanel);
			StandardButton importButton = val8;
			((Control)importButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImportWPList();
			});
			UpdateSavedWPList();
		}

		private void guildRaceContent()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildRaceIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text("Race");
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
			val3.set_Title("List");
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title("Info");
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildrace_data.xml"));
			int i = 0;
			foreach (XElement race in doc.Root.Elements("race"))
			{
				ViewInfoPanel(race, listPanel, i, "race");
				i++;
			}
		}

		private void guildBountyContent()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildBountyIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text("Bounty");
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
			val3.set_Title("List");
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10 - 72));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title("Info");
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildbounty_data.xml"));
			int i = 0;
			foreach (XElement bounty in doc.Root.Elements("bounty"))
			{
				ViewInfoPanel(bounty, listPanel, i, "bounty", 20);
				i++;
			}
			listPanel.set_CanScroll(true);
		}

		private void guildChallengeContent()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildChallengeIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text("Challenge");
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
			val3.set_Title("List");
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title("Info");
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildchallenge_data.xml"));
			int i = 0;
			foreach (XElement challenge in doc.Root.Elements("challenge"))
			{
				ViewInfoPanel(challenge, listPanel, i, "challenge");
				i++;
			}
		}

		private void guildPuzzleContent()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_guildPuzzleIcon));
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text("Puzzle");
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
			val3.set_Title("List");
			((Control)val3).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			listPanel = val3;
			Panel val4 = new Panel();
			val4.set_CanScroll(true);
			val4.set_ShowBorder(true);
			val4.set_Title("Info");
			((Control)val4).set_Size(new Point(364, ((Control)contentPanel).get_Height() - 10));
			((Control)val4).set_Location(new Point(((Control)listPanel).get_Right() + 9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			infoPanel = val4;
			((Container)listPanel).ClearChildren();
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildpuzzle_data.xml"));
			int i = 0;
			foreach (XElement puzzle in doc.Root.Elements("puzzle"))
			{
				ViewInfoPanel(puzzle, listPanel, i, "puzzle");
				i++;
			}
		}

		private void lockedContent()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			((Container)contentPanel).ClearChildren();
			Image val = new Image(AsyncTexture2D.op_Implicit(_lockedIcon));
			((Control)val).set_Size(new Point(656, 680));
			((Control)val).set_Location(new Point(((Control)contentPanel).get_Width() / 2 - 328, ((Control)contentPanel).get_Height() / 2 - 340));
			((Control)val).set_Parent((Container)(object)contentPanel);
		}

		private void SearchboxOnTextChanged(object sender, EventArgs e)
		{
			int i = 0;
			string searchText = ((TextInputBase)searchTextBox).get_Text();
			((Container)trekListPanel).ClearChildren();
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildtrek_data.xml"));
			foreach (XElement trek in doc.Root.Elements("trek"))
			{
				if (trek.Element("Name").Value.ToLower().StartsWith(searchText.ToLower()))
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

		private void ExportWPList()
		{
			int i = 0;
			string export = "BlishGM";
			foreach (KeyValuePair<int, int> item in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				export = export + ";" + item.Key;
			}
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(export).ContinueWith(delegate(Task<bool> clipboardResult)
			{
				if (clipboardResult.IsFaulted)
				{
					ScreenNotification.ShowNotification("Failed to copy export to clipboard. Try again.", (NotificationType)6, (Texture2D)null, 2);
				}
				else
				{
					ScreenNotification.ShowNotification("Copied export to clipboard!", (NotificationType)0, (Texture2D)null, 2);
				}
			});
		}

		private void ImportWPList()
		{
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildtrek_data.xml"));
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
									where x.Element("ID").Value == wp
									select x).FirstOrDefault();
								if (xElement != null)
								{
									AddWPToList((int)xElement.Element("ID"), (int)xElement.Element("MapID"));
									num++;
								}
							}
						}
						ScreenNotification.ShowNotification("Imported " + (num - 1) + " waypoints from clipboard!", (NotificationType)0, (Texture2D)null, 2);
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
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("guildtrek_data.xml"));
			int i = 0;
			foreach (KeyValuePair<int, int> wp in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				XElement trek = (from x in doc.Descendants("trek")
					where x.Element("ID").Value == wp.Key.ToString()
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
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Expected O, but got Unknown
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
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
			Image trekPanelWPImage = val2;
			((Control)trekPanelWPImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(trek.Element("Name").Value + " " + trek.Element("WaypointChatcode").Value).ContinueWith(delegate(Task<bool> clipboardResult)
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
			val3.set_Text(trek.Element("Name").Value + " (" + trek.Element("MapName").Value + ")");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(59, 3));
			val3.set_TextColor(Color.get_White());
			val3.set_ShadowColor(Color.get_Black());
			val3.set_ShowShadow(true);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)trekPanel);
			Label val4 = new Label();
			val4.set_Text(trek.Element("WaypointName").Value);
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
				Image addImage = val5;
				((Control)addImage).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					AddWPToList((int)trek.Element("ID"), (int)trek.Element("MapID"));
				});
			}
			if (remove)
			{
				Image val6 = new Image(AsyncTexture2D.op_Implicit(_closeTexture));
				((Control)val6).set_Size(new Point(20, 20));
				((Control)val6).set_Location(new Point(((Control)parent).get_Width() - 40, 4));
				((Control)val6).set_Parent((Container)(object)trekPanel);
				Image removeImage = val6;
				((Control)removeImage).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					RemoveWPFromList((int)trek.Element("ID"));
				});
			}
		}

		private void ViewInfoPanel(XElement element, Panel parent, int position, string type, int offset = 0)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
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
			Image trekPanelWPImage = val2;
			((Control)trekPanelWPImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(element.Element("Name").Value + " " + element.Element("WaypointChatcode").Value).ContinueWith(delegate(Task<bool> clipboardResult)
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
			val3.set_Text(element.Element("Name").Value + " (" + element.Element("MapName").Value + ")");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(59, 3));
			val3.set_TextColor(Color.get_White());
			val3.set_ShadowColor(Color.get_Black());
			val3.set_ShowShadow(true);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)trekPanel);
			Label val4 = new Label();
			val4.set_Text(element.Element("WaypointName").Value);
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
			Image addImage = val5;
			((Control)addImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DisplayInfo((int)element.Element("ID"), type, element);
			});
		}

		private void DisplayInfo(int v, string type, XElement element)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			int offset = 0;
			((Container)infoPanel).ClearChildren();
			infoPanel.set_Title("Info: " + element.Element("Name").Value);
			if (element.Element("Wiki") != null)
			{
				StandardButton val = new StandardButton();
				val.set_Text("Open Wiki");
				((Control)val).set_Size(new Point(110, 30));
				((Control)val).set_Location(new Point(4, 4));
				((Control)val).set_Parent((Container)(object)infoPanel);
				StandardButton openWikiBttn = val;
				((Control)openWikiBttn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start(element.Element("Wiki").Value);
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
