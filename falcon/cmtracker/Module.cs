using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using falcon.cmtracker.Controls;
using falcon.cmtracker.Persistance;

namespace falcon.cmtracker
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
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

		private string VisitUsAndHelpText;

		private string ClearCheckboxTooltipText;

		private string CurrentSortMethod;

		private Bosses _myBossesClears;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection settingCollection = settings.AddSubCollection("Managed Settings");
			CURRENT_ACCOUNT = settingCollection.DefineSetting("CURRENT_ACCOUNT", "Local");
			CM_CLEARS = settingCollection.DefineSetting("CM_CLEARS", "");
		}

		protected override void Initialize()
		{
			_displayedBosses = new List<BossButton>();
			_localSetting = new SettingUtil(CM_CLEARS.Value);
			if (string.IsNullOrEmpty(CM_CLEARS.Value))
			{
				_localSetting.AddNewAccount("Local");
			}
			Gw2ApiManager.SubtokenUpdated += OnApiSubTokenUpdated;
			LoadTextures();
		}

		protected override async Task LoadAsync()
		{
			if (_myBossesClears == null)
			{
				_myBossesClears = new Bosses(_localSetting.GetSettingForAccount(CURRENT_ACCOUNT.Value));
			}
		}

		private void ChangeLocalization(object sender, EventArgs e)
		{
			SORTBY_ALL = "Show All";
			SORTBY_STRIKES = "Strike";
			SORTBY_RAID = "Raid";
			CurrentSortMethod = SORTBY_ALL;
			_modulePanel?.Dispose();
			_modulePanel = BuildHomePanel(GameService.Overlay.BlishHudWindow);
			if (_ClearsTab != null)
			{
				GameService.Overlay.BlishHudWindow.RemoveTab(_ClearsTab);
			}
			_ClearsTab = GameService.Overlay.BlishHudWindow.AddTab(CmTrakcerTabName, _CmClearsIconTexture, _modulePanel, 0);
		}

		private void CMClearsUpdated(object sender, EventArgs e)
		{
			_localSetting.UpdateSettting((string)sender);
		}

		private void OnLocalSettingUpdated(object sender, EventArgs e)
		{
			SettingUtil temp = (SettingUtil)sender;
			CM_CLEARS.Value = temp.SettingString;
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
			GameService.Overlay.UserLocaleChanged += ChangeLocalization;
			_localSetting.PropertyChanged += OnLocalSettingUpdated;
			base.OnModuleLoaded(e);
		}

		private async Task FetchCurrentAccountName()
		{
			try
			{
				Logger.Debug("Getting user achievements from the API.");
				Account account = await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync();
				Logger.Debug("Loaded {Account} player name from the API.", account.Name);
				_localSetting.AddNewAccount(account.Name);
				accountDropDown.Items.Clear();
				foreach (string Item in _localSetting.GetAllAccounts())
				{
					accountDropDown.Items.Add(Item);
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
			_squadPanel?.Dispose();
			Gw2ApiManager.SubtokenUpdated -= OnApiSubTokenUpdated;
			foreach (BossButton displayedBoss in _displayedBosses)
			{
				displayedBoss?.Dispose();
			}
			GameService.Overlay.BlishHudWindow.RemoveTab(_ClearsTab);
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
			Panel hPanel = new Panel
			{
				CanScroll = false,
				Size = wndw.ContentRegion.Size
			};
			LoadingSpinner pageLoading = new LoadingSpinner
			{
				Parent = hPanel
			};
			pageLoading.Location = new Point(hPanel.Size.X / 2 - pageLoading.Size.X / 2, hPanel.Size.Y / 2 - pageLoading.Size.Y / 2);
			foreach (BossButton displayedBoss in _displayedBosses)
			{
				displayedBoss.Dispose();
			}
			_displayedBosses.Clear();
			FinishLoadingCmTrackerPanel(wndw, hPanel);
			pageLoading.Dispose();
			return hPanel;
		}

		private void RepositionTokens()
		{
			int pos = 0;
			foreach (BossButton e in _displayedBosses)
			{
				int x = pos % 3;
				int y = pos / 3;
				e.Location = new Point(x * (e.Width + 8), y * (e.Height + 8));
				((Panel)e.Parent).VerticalScrollOffset = 0;
				e.Parent.Invalidate();
				if (e.Visible)
				{
					pos++;
				}
			}
		}

		private void MousePressedSortButton(object sender, MouseEventArgs e)
		{
			Control bSortMethod = (Control)sender;
			bSortMethod.Size = new Point(bSortMethod.Size.X - 4, bSortMethod.Size.Y - 4);
			CurrentSortMethod = bSortMethod.BasicTooltipText;
			foreach (BossButton boss in _displayedBosses)
			{
				if (CurrentSortMethod == SORTBY_RAID && boss.Token.bossType == BossType.Raid)
				{
					boss.Visible = true;
				}
				else if (CurrentSortMethod == SORTBY_STRIKES && boss.Token.bossType == BossType.Strike)
				{
					boss.Visible = true;
				}
				else if (CurrentSortMethod == SORTBY_ALL)
				{
					boss.Visible = true;
				}
				else
				{
					boss.Visible = false;
				}
			}
			RepositionTokens();
		}

		private void MouseLeftSortButton(object sender, MouseEventArgs e)
		{
			((Control)sender).Size = new Point(32, 32);
		}

		private void FinishLoadingCmTrackerPanel(WindowBase wndw, Panel hPanel)
		{
			Panel header = new Panel
			{
				Parent = hPanel,
				Size = new Point(hPanel.Width, 50),
				Location = new Point(0, 0),
				CanScroll = false
			};
			Panel accountPanel = new Panel
			{
				Parent = header,
				Size = new Point(340, 32),
				Location = new Point(header.Left + 5, header.Location.Y),
				ShowTint = false
			};
			Label accountLabel = new Label
			{
				Parent = accountPanel,
				Size = new Point(50, 32),
				Location = new Point(accountPanel.Left, accountPanel.Location.Y),
				Text = "Account"
			};
			accountDropDown = new Dropdown
			{
				Parent = accountPanel,
				Size = new Point(140, 32),
				Location = new Point(accountLabel.Right + 5, accountPanel.Location.Y)
			};
			accountDropDown.SelectedItem = CURRENT_ACCOUNT.Value;
			foreach (string Item in _localSetting.GetAllAccounts())
			{
				accountDropDown.Items.Add(Item);
			}
			accountDropDown.ValueChanged += delegate(object sender, ValueChangedEventArgs args)
			{
				CURRENT_ACCOUNT.Value = args.CurrentValue;
				_displayedBosses.Clear();
				_myBossesClears = new Bosses(_localSetting.GetSettingForAccount(args.CurrentValue));
				SetupBossClears();
			};
			Panel sortingsMenu = new Panel
			{
				Parent = header,
				Size = new Point(140, 32),
				Location = new Point(header.Right - 140 - 5, header.Location.Y),
				ShowTint = true
			};
			Image bSortByAll = new Image
			{
				Parent = sortingsMenu,
				Size = new Point(32, 32),
				Location = new Point(5, 0),
				Texture = GameService.Content.GetTexture("255369"),
				BackgroundColor = Microsoft.Xna.Framework.Color.Transparent,
				BasicTooltipText = SORTBY_ALL
			};
			bSortByAll.LeftMouseButtonPressed += MousePressedSortButton;
			bSortByAll.LeftMouseButtonReleased += MouseLeftSortButton;
			bSortByAll.MouseLeft += MouseLeftSortButton;
			Image bSortByRaid = new Image
			{
				Parent = sortingsMenu,
				Size = new Point(32, 32),
				Location = new Point(bSortByAll.Right + 20 + 5, 0),
				Texture = _sortByRaidTexture,
				BasicTooltipText = SORTBY_RAID
			};
			bSortByRaid.LeftMouseButtonPressed += MousePressedSortButton;
			bSortByRaid.LeftMouseButtonReleased += MouseLeftSortButton;
			bSortByRaid.MouseLeft += MouseLeftSortButton;
			Image image = new Image();
			image.Parent = sortingsMenu;
			image.Size = new Point(32, 32);
			image.Location = new Point(bSortByRaid.Right + 5, 0);
			image.Texture = _sortByStrikeTexture;
			image.BasicTooltipText = SORTBY_STRIKES;
			image.LeftMouseButtonPressed += MousePressedSortButton;
			image.LeftMouseButtonReleased += MouseLeftSortButton;
			image.MouseLeft += MouseLeftSortButton;
			Panel footer = new Panel
			{
				Parent = hPanel,
				Size = new Point(hPanel.Width, 50),
				Location = new Point(0, hPanel.Height - 50),
				CanScroll = false
			};
			_squadPanel = new Panel
			{
				Parent = hPanel,
				Size = new Point(header.Size.X, hPanel.Height - header.Height - footer.Height),
				Location = new Point(0, header.Bottom),
				ShowBorder = true,
				CanScroll = true,
				ShowTint = true
			};
			StandardButton clearCheckbox = new StandardButton
			{
				Parent = hPanel,
				Size = new Point(200, 30),
				Location = new Point(_squadPanel.Location.X + _squadPanel.Width - 200 - 5, _squadPanel.Location.Y + _squadPanel.Height + 10),
				Text = "Reset Weekly Clears",
				BasicTooltipText = ClearCheckboxTooltipText,
				Visible = true
			};
			Panel confirmPanel = new Panel
			{
				Parent = hPanel,
				Size = new Point(550, 100),
				Location = new Point(_squadPanel.Location.X + _squadPanel.Width - 550 - 5, _squadPanel.Location.Y + _squadPanel.Height + 10),
				ShowBorder = false,
				CanScroll = false,
				ShowTint = false,
				Visible = false
			};
			Label confirmText = new Label
			{
				Parent = confirmPanel,
				Size = new Point(300, 30),
				Location = new Point(5, 5),
				Text = "Are you sure you want to rest weekly clears?",
				BackgroundColor = Microsoft.Xna.Framework.Color.Transparent
			};
			StandardButton yesButton = new StandardButton
			{
				Parent = confirmPanel,
				Size = new Point(50, 30),
				Location = new Point(confirmText.Right + 5, confirmText.Location.Y),
				Text = "Yes"
			};
			StandardButton yesAllButton = new StandardButton
			{
				Parent = confirmPanel,
				Size = new Point(130, 30),
				Location = new Point(yesButton.Right + 5, confirmText.Location.Y),
				Text = "Yes/All Accounts"
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Parent = confirmPanel;
			standardButton.Size = new Point(50, 30);
			standardButton.Location = new Point(yesAllButton.Right + 5, confirmText.Location.Y);
			standardButton.Text = "No";
			standardButton.Click += delegate
			{
				clearCheckbox.Visible = true;
				confirmPanel.Visible = false;
			};
			yesButton.Click += delegate
			{
				foreach (Token token in _myBossesClears.Tokens)
				{
					token.setting.Value = false;
				}
				foreach (BossButton displayedBoss in _displayedBosses)
				{
					displayedBoss.Background = (displayedBoss.Token.setting.Value ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Black);
				}
				clearCheckbox.Visible = true;
				confirmPanel.Visible = false;
			};
			yesAllButton.Click += delegate
			{
				foreach (Token token2 in _myBossesClears.Tokens)
				{
					token2.setting.Value = false;
				}
				foreach (BossButton displayedBoss2 in _displayedBosses)
				{
					displayedBoss2.Background = (displayedBoss2.Token.setting.Value ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Black);
				}
				_localSetting.ResetAllValues();
				clearCheckbox.Visible = true;
				confirmPanel.Visible = false;
			};
			clearCheckbox.Click += delegate
			{
				clearCheckbox.Visible = false;
				confirmPanel.Visible = true;
			};
			contentPanel = new Panel
			{
				Parent = hPanel,
				Size = new Point(header.Size.X, hPanel.Height - header.Height - footer.Height),
				Location = new Point(0, header.Bottom),
				ShowBorder = true,
				CanScroll = true,
				ShowTint = true
			};
			SetupBossClears();
		}

		private void SetupBossClears()
		{
			if (_myBossesClears.Tokens != null)
			{
				contentPanel.ClearChildren();
				foreach (Token boss in _myBossesClears.Tokens)
				{
					BossButton BossButton = new BossButton
					{
						Parent = contentPanel,
						Icon = ((boss.Icon == null) ? ContentsManager.GetTexture("icon_token.png") : ContentsManager.GetTexture(boss.Icon)),
						Font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size16, ContentService.FontStyle.Regular),
						Text = boss.Name,
						HighlightType = DetailsHighlightType.ScrollingHighlight,
						Token = boss,
						Background = (boss.setting.Value ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Black)
					};
					BossButton.Click += delegate
					{
						boss.setting.Value = !boss.setting.Value;
						BossButton.Background = (boss.setting.Value ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Black);
					};
					_displayedBosses.Add(BossButton);
				}
			}
			RepositionTokens();
		}
	}
}
