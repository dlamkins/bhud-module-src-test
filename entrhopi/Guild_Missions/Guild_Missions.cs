using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
		private static class Layout
		{
			public const int TopMargin = 10;

			public const int RightMargin = 5;

			public const int BottomMargin = 10;

			public const int LeftMargin = 9;

			public const int ButtonHeight = 30;

			public const int PanelSize = 56;

			public const int MaxResultCount = 7;
		}

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static Module ModuleInstance;

		private Panel trekListPanel;

		private Panel savedTrekListPanel;

		private Panel contentPanel;

		private Panel listPanel;

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
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Expected O, but got Unknown
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Expected O, but got Unknown
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Expected O, but got Unknown
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Expected O, but got Unknown
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
			((Control)val3).set_Size(new Point(((Control)missionTypePanel).get_Width(), 56));
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_Parent((Container)(object)missionTypePanel);
			Panel guildTrekPanel = val3;
			((Control)guildTrekPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GuildTrekContent();
			});
			Image val4 = new Image(AsyncTexture2D.op_Implicit(_guildTrekIcon));
			((Control)val4).set_Size(new Point(56, 56));
			((Control)val4).set_Location(new Point(0, 0));
			((Control)val4).set_Parent((Container)(object)guildTrekPanel);
			Label val5 = new Label();
			val5.set_Text(Common.gmTypeTrek);
			val5.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val5).set_Location(new Point(65, 18));
			val5.set_TextColor(Color.get_White());
			val5.set_ShadowColor(Color.get_Black());
			val5.set_ShowShadow(true);
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Parent((Container)(object)guildTrekPanel);
			Panel val6 = new Panel();
			val6.set_ShowBorder(false);
			((Control)val6).set_Size(new Point(((Control)missionTypePanel).get_Width(), 56));
			((Control)val6).set_Location(new Point(0, 56));
			((Control)val6).set_Parent((Container)(object)missionTypePanel);
			Panel guildBountyPanel = val6;
			((Control)guildBountyPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GuildBountyContent();
			});
			Image val7 = new Image(AsyncTexture2D.op_Implicit(_guildBountyIcon));
			((Control)val7).set_Size(new Point(56, 56));
			((Control)val7).set_Location(new Point(0, 0));
			((Control)val7).set_Parent((Container)(object)guildBountyPanel);
			Label val8 = new Label();
			val8.set_Text(Common.gmTypeBounty);
			val8.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val8).set_Location(new Point(65, 18));
			val8.set_TextColor(Color.get_White());
			val8.set_ShadowColor(Color.get_Black());
			val8.set_ShowShadow(true);
			val8.set_AutoSizeWidth(true);
			val8.set_AutoSizeHeight(true);
			((Control)val8).set_Parent((Container)(object)guildBountyPanel);
			Panel val9 = new Panel();
			val9.set_ShowBorder(false);
			((Control)val9).set_Size(new Point(((Control)missionTypePanel).get_Width(), 56));
			((Control)val9).set_Location(new Point(0, 112));
			((Control)val9).set_Parent((Container)(object)missionTypePanel);
			Panel guildRacePanel = val9;
			((Control)guildRacePanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GuildRaceContent();
			});
			Image val10 = new Image(AsyncTexture2D.op_Implicit(_guildRaceIcon));
			((Control)val10).set_Size(new Point(56, 56));
			((Control)val10).set_Location(new Point(0, 0));
			((Control)val10).set_Parent((Container)(object)guildRacePanel);
			Label val11 = new Label();
			val11.set_Text(Common.gmTypeRace);
			val11.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val11).set_Location(new Point(65, 18));
			val11.set_TextColor(Color.get_White());
			val11.set_ShadowColor(Color.get_Black());
			val11.set_ShowShadow(true);
			val11.set_AutoSizeWidth(true);
			val11.set_AutoSizeHeight(true);
			((Control)val11).set_Parent((Container)(object)guildRacePanel);
			Panel val12 = new Panel();
			val12.set_ShowBorder(false);
			((Control)val12).set_Size(new Point(((Control)missionTypePanel).get_Width(), 56));
			((Control)val12).set_Location(new Point(0, 168));
			((Control)val12).set_Parent((Container)(object)missionTypePanel);
			Panel guildChallengePanel = val12;
			((Control)guildChallengePanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GuildChallengeContent();
			});
			Image val13 = new Image(AsyncTexture2D.op_Implicit(_guildChallengeIcon));
			((Control)val13).set_Size(new Point(56, 56));
			((Control)val13).set_Location(new Point(0, 0));
			((Control)val13).set_Parent((Container)(object)guildChallengePanel);
			Label val14 = new Label();
			val14.set_Text(Common.gmTypeChallenge);
			val14.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val14).set_Location(new Point(65, 18));
			val14.set_TextColor(Color.get_White());
			val14.set_ShadowColor(Color.get_Black());
			val14.set_ShowShadow(true);
			val14.set_AutoSizeWidth(true);
			val14.set_AutoSizeHeight(true);
			((Control)val14).set_Parent((Container)(object)guildChallengePanel);
			Panel val15 = new Panel();
			val15.set_ShowBorder(false);
			((Control)val15).set_Size(new Point(((Control)missionTypePanel).get_Width(), 56));
			((Control)val15).set_Location(new Point(0, 224));
			((Control)val15).set_Parent((Container)(object)missionTypePanel);
			Panel guildPuzzlePanel = val15;
			((Control)guildPuzzlePanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GuildPuzzleContent();
			});
			Image val16 = new Image(AsyncTexture2D.op_Implicit(_guildPuzzleIcon));
			((Control)val16).set_Size(new Point(56, 56));
			((Control)val16).set_Location(new Point(0, 0));
			((Control)val16).set_Parent((Container)(object)guildPuzzlePanel);
			Label val17 = new Label();
			val17.set_Text(Common.gmTypePuzzle);
			val17.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val17).set_Location(new Point(65, 18));
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

		private void GuildTrekContent()
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
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Expected O, but got Unknown
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
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
			Label val3 = new Label();
			val3.set_Text("Searchbox mouse click clears current search,\nEnter key adds topmost item to saved list and clears search");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Location(new Point(220, 18));
			val3.set_TextColor(Color.get_White());
			val3.set_ShadowColor(Color.get_Black());
			val3.set_ShowShadow(true);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)contentPanel);
			TextBox val4 = new TextBox();
			((TextInputBase)val4).set_PlaceholderText(Common.gmSearchPlaceholder);
			((Control)val4).set_Size(new Point(358, 43));
			((TextInputBase)val4).set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Location(new Point(9, 82));
			((Control)val4).set_Parent((Container)(object)contentPanel);
			searchTextBox = val4;
			((Control)searchTextBox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearSearch();
			});
			((TextInputBase)searchTextBox).add_TextChanged((EventHandler<EventArgs>)SearchboxOnTextChanged);
			searchTextBox.add_EnterPressed((EventHandler<EventArgs>)SearchboxEnterPressed);
			Panel val5 = new Panel();
			val5.set_ShowBorder(true);
			val5.set_Title(Common.gmPanelSearchResults);
			((Control)val5).set_Size(new Point(364, ((Control)contentPanel).get_Height() - ((Control)searchTextBox).get_Bottom() - 10));
			((Control)val5).set_Location(new Point(6, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val5).set_Parent((Container)(object)contentPanel);
			trekListPanel = val5;
			Panel val6 = new Panel();
			val6.set_CanScroll(true);
			val6.set_ShowBorder(true);
			val6.set_Title(Common.gmPanelSavedTreks);
			((Control)val6).set_Size(new Point(364, ((Control)contentPanel).get_Height() - ((Control)searchTextBox).get_Bottom() - 30 - 10));
			((Control)val6).set_Location(new Point(((Control)trekListPanel).get_Right() + 9, ((Control)searchTextBox).get_Bottom() + 10));
			((Control)val6).set_Parent((Container)(object)contentPanel);
			savedTrekListPanel = val6;
			StandardButton val7 = new StandardButton();
			val7.set_Text(Common.gmButtonClearAll);
			((Control)val7).set_Size(new Point(110, 30));
			((Control)val7).set_Location(new Point(((Control)trekListPanel).get_Right() + 20, ((Control)searchTextBox).get_Top() - 1));
			((Control)val7).set_Parent((Container)(object)contentPanel);
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearWPList();
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text(Common.gmButtonExport);
			((Control)val8).set_Size(new Point(110, 30));
			((Control)val8).set_Location(new Point(((Control)trekListPanel).get_Right() + 130 + 9, ((Control)searchTextBox).get_Top() - 1));
			((Control)val8).set_Parent((Container)(object)contentPanel);
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ExportWPList();
			});
			StandardButton val9 = new StandardButton();
			val9.set_Text(Common.gmButtonImport);
			((Control)val9).set_Size(new Point(110, 30));
			((Control)val9).set_Location(new Point(((Control)trekListPanel).get_Right() + 250 + 9, ((Control)searchTextBox).get_Top() - 1));
			((Control)val9).set_Parent((Container)(object)contentPanel);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImportWPList();
			});
			StandardButton val10 = new StandardButton();
			val10.set_Text(Common.gmButtonSendToChat);
			((Control)val10).set_Size(new Point(364, 30));
			((Control)val10).set_Location(new Point(((Control)savedTrekListPanel).get_Left(), ((Control)savedTrekListPanel).get_Bottom()));
			((Control)val10).set_Parent((Container)(object)contentPanel);
			((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SendAllSavedToClipboard();
			});
			UpdateSavedWPList();
		}

		private void GuildRaceContent()
		{
			SimpleGuildMissionPanel(AsyncTexture2D.op_Implicit(_guildRaceIcon), Common.gmTypeRace, "XML\\races.xml", "race");
		}

		private void GuildBountyContent()
		{
			SimpleGuildMissionPanel(AsyncTexture2D.op_Implicit(_guildBountyIcon), Common.gmTypeBounty, "XML\\bounties.xml", "bounty", enableScrolling: true);
		}

		private void GuildChallengeContent()
		{
			SimpleGuildMissionPanel(AsyncTexture2D.op_Implicit(_guildChallengeIcon), Common.gmTypeChallenge, "XML\\challenges.xml", "challenge");
		}

		private void GuildPuzzleContent()
		{
			SimpleGuildMissionPanel(AsyncTexture2D.op_Implicit(_guildPuzzleIcon), Common.gmTypePuzzle, "XML\\puzzles.xml", "puzzle");
		}

		private void SimpleGuildMissionPanel(AsyncTexture2D icon, string title, string xmlPath, string xmlElement, bool enableScrolling = false)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			((Container)contentPanel).ClearChildren();
			Image val = new Image(icon);
			((Control)val).set_Size(new Point(72, 72));
			((Control)val).set_Location(new Point(9, 0));
			((Control)val).set_Parent((Container)(object)contentPanel);
			Label val2 = new Label();
			val2.set_Text(title);
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
			((Control)val3).set_Size(new Point(((Control)contentPanel).get_Width() - 9, ((Control)contentPanel).get_Height() - 10));
			((Control)val3).set_Location(new Point(6, 82));
			((Control)val3).set_Parent((Container)(object)contentPanel);
			val3.set_CanScroll(enableScrolling);
			listPanel = val3;
			XDocument xDocument = XDocument.Load(ContentsManager.GetFileStream(xmlPath));
			int index = 0;
			foreach (XElement element in xDocument.Root.Elements(xmlElement))
			{
				AddInfoPanel(element, listPanel, index++);
			}
		}

		private void SearchboxOnTextChanged(object _, EventArgs __)
		{
			string text = ((TextInputBase)searchTextBox).get_Text();
			((Container)trekListPanel).ClearChildren();
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			XDocument xDocument = XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml"));
			int count = 0;
			foreach (XElement trek in xDocument.Root.Elements("trek"))
			{
				if (trek.Element("name_" + ShortUserLocale).Value.ToLower().StartsWith(text.ToLower()))
				{
					AddTrekPanel(trek, trekListPanel, count++, add: true);
					if (count >= 7)
					{
						break;
					}
				}
			}
		}

		private void SearchboxEnterPressed(object _, EventArgs __)
		{
			string text = ((TextInputBase)searchTextBox).get_Text();
			foreach (XElement trek in XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml")).Root.Elements("trek"))
			{
				if (trek.Element("name_" + ShortUserLocale).Value.ToLower().StartsWith(text.ToLower()))
				{
					ToggleWaypoint((int)trek.Element("id"), (int)trek.Element("map_id"), add: true);
					break;
				}
			}
			ClearSearch();
			((TextInputBase)searchTextBox).set_Focused(true);
		}

		private void ToggleWaypoint(int trek, int map, bool add)
		{
			if (add)
			{
				savedGuildTreks.Add(trek, map);
			}
			else
			{
				savedGuildTreks.Remove(trek);
			}
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

		private async Task CopyToClipboard(string text)
		{
			try
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
				ScreenNotification.ShowNotification(Common.gmNotificationClipboardSaved, (NotificationType)0, (Texture2D)null, 2);
			}
			catch (Exception)
			{
				ScreenNotification.ShowNotification(Common.gmNotificationClipboardError, (NotificationType)6, (Texture2D)null, 2);
			}
		}

		private async Task SendAllSavedToClipboard()
		{
			XDocument doc = XDocument.Load(ContentsManager.GetFileStream("XML\\treks.xml"));
			string export = "";
			foreach (KeyValuePair<int, int> wp in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				XElement trek = (from x in doc.Descendants("trek")
					where x.Element("id").Value == wp.Key.ToString()
					select x).FirstOrDefault();
				if (trek != null)
				{
					export = export + trek.Element("name_" + ShortUserLocale).Value + " " + trek.Element("chat_link").Value + " ";
				}
			}
			await CopyToClipboard(export);
		}

		private async Task ExportWPList()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("BlishGM");
			foreach (KeyValuePair<int, int> item in savedGuildTreks.OrderBy((KeyValuePair<int, int> key) => key.Value))
			{
				sb.Append($";{item.Key}");
			}
			await CopyToClipboard(sb.ToString());
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
									ToggleWaypoint((int)xElement.Element("id"), (int)xElement.Element("map_id"), add: true);
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
				CopyToClipboard(trek.Element("name_" + ShortUserLocale).Value + " " + trek.Element("chat_link").Value);
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
					ToggleWaypoint((int)trek.Element("id"), (int)trek.Element("map_id"), add: true);
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
					ToggleWaypoint((int)trek.Element("id"), 0, add: false);
				});
			}
		}

		private void AddInfoPanel(XElement element, Panel parent, int position)
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
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Size(new Point(((Control)parent).get_Width(), 70));
			((Control)val).set_Location(new Point(9, 5 + position * 70));
			((Control)val).set_Parent((Container)(object)parent);
			Panel elementPanel = val;
			Image val2 = new Image(AsyncTexture2D.op_Implicit(_waypointIcon));
			((Control)val2).set_Size(new Point(50, 50));
			((Control)val2).set_Location(new Point(0, 4));
			((Control)val2).set_Parent((Container)(object)elementPanel);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CopyToClipboard(element.Element("name_" + ShortUserLocale).Value + " " + element.Element("chat_link").Value);
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
			((Control)val3).set_Parent((Container)(object)elementPanel);
			Label val4 = new Label();
			val4.set_Text(element.Element("waypoint_name_" + ShortUserLocale).Value);
			val4.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val4).set_Location(new Point(59, 32));
			val4.set_TextColor(Color.get_Silver());
			val4.set_ShadowColor(Color.get_Black());
			val4.set_ShowShadow(true);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Parent((Container)(object)elementPanel);
			StandardButton val5 = new StandardButton();
			val5.set_Text(Common.gmButtonWiki);
			((Control)val5).set_Size(new Point(110, 30));
			((Control)val5).set_Location(new Point(((Control)parent).get_Width() - 110 - 50, 10));
			((Control)val5).set_Parent((Container)(object)elementPanel);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(element.Element("wiki_link_" + ShortUserLocale).Value);
			});
		}

		protected override void Unload()
		{
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_moduleTab);
			ModuleInstance = null;
		}
	}
}
