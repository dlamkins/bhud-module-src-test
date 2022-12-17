using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
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

		public static SettingEntry<string> CURRENT_ACCOUNT;

		public static SettingEntry<string> CM_CLEARS;

		internal Texture2D _CmClearsIconTexture;

		internal Texture2D _CmClearsLogoTexture;

		internal Texture2D _deletedItemTexture;

		internal Texture2D _sortByStrikeTexture;

		internal Texture2D _sortByRaidTexture;

		internal Texture2D _notificationBackroundTexture;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private const string LOCAL_ACCOUNT_NAME = "Local";

		private WindowTab _ClearsTab;

		private Panel _modulePanel;

		private List<BossButton> _displayedBosses;

		private Panel _squadPanel;

		private static SettingUtil _localSetting;

		private Dropdown accountDropDown;

		private Panel contentPanel;

		private string CmTrakcerTabName = "CM Tracker";

		private string SORTBY_ALL;

		private string SORTBY_STRIKES;

		private string SORTBY_RAID;

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
			CURRENT_ACCOUNT = obj.DefineSetting<string>("CURRENT_ACCOUNT", "Local", (Func<string>)null, (Func<string>)null);
			CM_CLEARS = obj.DefineSetting<string>("CM_CLEARS", "", (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			_displayedBosses = new List<BossButton>();
			_localSetting = new SettingUtil(CM_CLEARS.get_Value());
			if (string.IsNullOrEmpty(CM_CLEARS.get_Value()))
			{
				_localSetting.AddNewAccount("Local");
			}
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			LoadTextures();
		}

		protected override async Task LoadAsync()
		{
			if (_myBossesClears == null)
			{
				_myBossesClears = new Bosses(_localSetting.GetSettingForAccount(CURRENT_ACCOUNT.get_Value()));
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

		private void CMClearsUpdated(object sender, EventArgs e)
		{
			_localSetting.UpdateSettting((string)sender);
		}

		private void OnLocalSettingUpdated(object sender, EventArgs e)
		{
			SettingUtil temp = (SettingUtil)sender;
			CM_CLEARS.set_Value(temp.SettingString);
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
			_localSetting.PropertyChanged += OnLocalSettingUpdated;
			((Module)this).OnModuleLoaded(e);
		}

		private async Task FetchCurrentAccountName()
		{
			try
			{
				Logger.Debug("Getting user achievements from the API.");
				Account account = await ((IBlobClient<Account>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken));
				Logger.Debug("Loaded {Account} player name from the API.", new object[1] { account.get_Name() });
				_localSetting.AddNewAccount(account.get_Name());
				accountDropDown.get_Items().Clear();
				foreach (string Item in _localSetting.GetAllAccounts())
				{
					accountDropDown.get_Items().Add(Item);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load account name.");
			}
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
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
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

		private async void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			await FetchCurrentAccountName();
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
			FinishLoadingCmTrackerPanel(wndw, hPanel);
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

		private void FinishLoadingCmTrackerPanel(WindowBase wndw, Panel hPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Expected O, but got Unknown
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Expected O, but got Unknown
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Expected O, but got Unknown
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Expected O, but got Unknown
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Expected O, but got Unknown
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Expected O, but got Unknown
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_0573: Expected O, but got Unknown
			//IL_0573: Unknown result type (might be due to invalid IL or missing references)
			//IL_0578: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Expected O, but got Unknown
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Expected O, but got Unknown
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			//IL_0621: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0650: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0701: Unknown result type (might be due to invalid IL or missing references)
			//IL_070d: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)hPanel);
			((Control)val).set_Size(new Point(((Control)hPanel).get_Width(), 50));
			((Control)val).set_Location(new Point(0, 0));
			val.set_CanScroll(false);
			Panel header = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)header);
			((Control)val2).set_Size(new Point(340, 32));
			((Control)val2).set_Location(new Point(((Control)header).get_Left() + 5, ((Control)header).get_Location().Y));
			val2.set_ShowTint(false);
			Panel accountPanel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)accountPanel);
			((Control)val3).set_Size(new Point(50, 32));
			((Control)val3).set_Location(new Point(((Control)accountPanel).get_Left(), ((Control)accountPanel).get_Location().Y));
			val3.set_Text("Account");
			Label accountLabel = val3;
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)accountPanel);
			((Control)val4).set_Size(new Point(140, 32));
			((Control)val4).set_Location(new Point(((Control)accountLabel).get_Right() + 5, ((Control)accountPanel).get_Location().Y));
			accountDropDown = val4;
			accountDropDown.set_SelectedItem(CURRENT_ACCOUNT.get_Value());
			foreach (string Item in _localSetting.GetAllAccounts())
			{
				accountDropDown.get_Items().Add(Item);
			}
			accountDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object sender, ValueChangedEventArgs args)
			{
				CURRENT_ACCOUNT.set_Value(args.get_CurrentValue());
				_displayedBosses.Clear();
				_myBossesClears = new Bosses(_localSetting.GetSettingForAccount(args.get_CurrentValue()));
				SetupBossClears();
			});
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)header);
			((Control)val5).set_Size(new Point(140, 32));
			((Control)val5).set_Location(new Point(((Control)header).get_Right() - 140 - 5, ((Control)header).get_Location().Y));
			val5.set_ShowTint(true);
			Panel sortingsMenu = val5;
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)sortingsMenu);
			((Control)val6).set_Size(new Point(32, 32));
			((Control)val6).set_Location(new Point(5, 0));
			val6.set_Texture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("255369")));
			((Control)val6).set_BackgroundColor(Color.get_Transparent());
			((Control)val6).set_BasicTooltipText(SORTBY_ALL);
			Image bSortByAll = val6;
			((Control)bSortByAll).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByAll).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByAll).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val7 = new Image();
			((Control)val7).set_Parent((Container)(object)sortingsMenu);
			((Control)val7).set_Size(new Point(32, 32));
			((Control)val7).set_Location(new Point(((Control)bSortByAll).get_Right() + 20 + 5, 0));
			val7.set_Texture(AsyncTexture2D.op_Implicit(_sortByRaidTexture));
			((Control)val7).set_BasicTooltipText(SORTBY_RAID);
			Image bSortByRaid = val7;
			((Control)bSortByRaid).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByRaid).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByRaid).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)sortingsMenu);
			((Control)val8).set_Size(new Point(32, 32));
			((Control)val8).set_Location(new Point(((Control)bSortByRaid).get_Right() + 5, 0));
			val8.set_Texture(AsyncTexture2D.op_Implicit(_sortByStrikeTexture));
			((Control)val8).set_BasicTooltipText(SORTBY_STRIKES);
			((Control)val8).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)val8).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)val8).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Panel val9 = new Panel();
			((Control)val9).set_Parent((Container)(object)hPanel);
			((Control)val9).set_Size(new Point(((Control)hPanel).get_Width(), 50));
			((Control)val9).set_Location(new Point(0, ((Control)hPanel).get_Height() - 50));
			val9.set_CanScroll(false);
			Panel footer = val9;
			Panel val10 = new Panel();
			((Control)val10).set_Parent((Container)(object)hPanel);
			((Control)val10).set_Size(new Point(((Control)header).get_Size().X, ((Control)hPanel).get_Height() - ((Control)header).get_Height() - ((Control)footer).get_Height()));
			((Control)val10).set_Location(new Point(0, ((Control)header).get_Bottom()));
			val10.set_ShowBorder(true);
			val10.set_CanScroll(true);
			val10.set_ShowTint(true);
			_squadPanel = val10;
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)hPanel);
			((Control)val11).set_Size(new Point(200, 30));
			((Control)val11).set_Location(new Point(((Control)_squadPanel).get_Location().X + ((Control)_squadPanel).get_Width() - 200 - 5, ((Control)_squadPanel).get_Location().Y + ((Control)_squadPanel).get_Height() + 10));
			val11.set_Text("Reset Weekly Clears");
			((Control)val11).set_BasicTooltipText(ClearCheckboxTooltipText);
			((Control)val11).set_Visible(true);
			StandardButton clearCheckbox = val11;
			Panel val12 = new Panel();
			((Control)val12).set_Parent((Container)(object)hPanel);
			((Control)val12).set_Size(new Point(550, 100));
			((Control)val12).set_Location(new Point(((Control)_squadPanel).get_Location().X + ((Control)_squadPanel).get_Width() - 550 - 5, ((Control)_squadPanel).get_Location().Y + ((Control)_squadPanel).get_Height() + 10));
			val12.set_ShowBorder(false);
			val12.set_CanScroll(false);
			val12.set_ShowTint(false);
			((Control)val12).set_Visible(false);
			Panel confirmPanel = val12;
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)confirmPanel);
			((Control)val13).set_Size(new Point(300, 30));
			((Control)val13).set_Location(new Point(5, 5));
			val13.set_Text("Are you sure you want to rest weekly clears?");
			((Control)val13).set_BackgroundColor(Color.get_Transparent());
			Label confirmText = val13;
			StandardButton val14 = new StandardButton();
			((Control)val14).set_Parent((Container)(object)confirmPanel);
			((Control)val14).set_Size(new Point(50, 30));
			((Control)val14).set_Location(new Point(((Control)confirmText).get_Right() + 5, ((Control)confirmText).get_Location().Y));
			val14.set_Text("Yes");
			StandardButton yesButton = val14;
			StandardButton val15 = new StandardButton();
			((Control)val15).set_Parent((Container)(object)confirmPanel);
			((Control)val15).set_Size(new Point(130, 30));
			((Control)val15).set_Location(new Point(((Control)yesButton).get_Right() + 5, ((Control)confirmText).get_Location().Y));
			val15.set_Text("Yes/All Accounts");
			StandardButton yesAllButton = val15;
			StandardButton val16 = new StandardButton();
			((Control)val16).set_Parent((Container)(object)confirmPanel);
			((Control)val16).set_Size(new Point(50, 30));
			((Control)val16).set_Location(new Point(((Control)yesAllButton).get_Right() + 5, ((Control)confirmText).get_Location().Y));
			val16.set_Text("No");
			((Control)val16).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)clearCheckbox).set_Visible(true);
				((Control)confirmPanel).set_Visible(false);
			});
			((Control)yesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				foreach (Token token in _myBossesClears.Tokens)
				{
					token.setting.Value = false;
				}
				foreach (BossButton displayedBoss in _displayedBosses)
				{
					displayedBoss.Background = (displayedBoss.Token.setting.Value ? Color.get_Green() : Color.get_Black());
				}
				((Control)clearCheckbox).set_Visible(true);
				((Control)confirmPanel).set_Visible(false);
			});
			((Control)yesAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				foreach (Token token2 in _myBossesClears.Tokens)
				{
					token2.setting.Value = false;
				}
				foreach (BossButton displayedBoss2 in _displayedBosses)
				{
					displayedBoss2.Background = (displayedBoss2.Token.setting.Value ? Color.get_Green() : Color.get_Black());
				}
				_localSetting.ResetAllValues();
				((Control)clearCheckbox).set_Visible(true);
				((Control)confirmPanel).set_Visible(false);
			});
			((Control)clearCheckbox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)clearCheckbox).set_Visible(false);
				((Control)confirmPanel).set_Visible(true);
			});
			Panel val17 = new Panel();
			((Control)val17).set_Parent((Container)(object)hPanel);
			((Control)val17).set_Size(new Point(((Control)header).get_Size().X, ((Control)hPanel).get_Height() - ((Control)header).get_Height() - ((Control)footer).get_Height()));
			((Control)val17).set_Location(new Point(0, ((Control)header).get_Bottom()));
			val17.set_ShowBorder(true);
			val17.set_CanScroll(true);
			val17.set_ShowTint(true);
			contentPanel = val17;
			SetupBossClears();
		}

		private void SetupBossClears()
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			if (_myBossesClears.Tokens != null)
			{
				((Container)contentPanel).ClearChildren();
				foreach (Token boss in _myBossesClears.Tokens)
				{
					BossButton bossButton = new BossButton();
					((Control)bossButton).set_Parent((Container)(object)contentPanel);
					((DetailsButton)bossButton).set_Icon(AsyncTexture2D.op_Implicit((boss.Icon == null) ? ContentsManager.GetTexture("icon_token.png") : ContentsManager.GetTexture(boss.Icon)));
					bossButton.Font = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)0);
					((DetailsButton)bossButton).set_Text(boss.Name);
					((DetailsButton)bossButton).set_HighlightType((DetailsHighlightType)1);
					bossButton.Token = boss;
					bossButton.Background = (boss.setting.Value ? Color.get_Green() : Color.get_Black());
					BossButton BossButton = bossButton;
					((Control)BossButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						//IL_003b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0042: Unknown result type (might be due to invalid IL or missing references)
						boss.setting.Value = !boss.setting.Value;
						BossButton.Background = (boss.setting.Value ? Color.get_Green() : Color.get_Black());
					});
					_displayedBosses.Add(BossButton);
				}
			}
			RepositionTokens();
		}
	}
}
