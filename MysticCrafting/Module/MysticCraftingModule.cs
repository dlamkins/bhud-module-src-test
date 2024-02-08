using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Overview;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class MysticCraftingModule : Blish_HUD.Modules.Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Blish_HUD.Modules.Module>();

		internal static ModuleSettings Settings;

		internal SettingEntry<KeyBinding> ToggleWindowSetting;

		private CornerIcon _cornerIcon;

		private StandardWindow _mainWindow;

		private MainView _mainView;

		private bool dataLoaded;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		private static ServiceContainer ServiceCollection { get; set; }

		private bool TradingPostLoaded { get; set; }

		private bool WalletLoaded { get; set; }

		private bool PlayerItemsLoaded { get; set; }

		private bool PlayerUnlocksLoaded { get; set; }

		[ImportingConstructor]
		public MysticCraftingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
			ToggleWindowSetting = settings.DefineSetting("Toggle main window", new KeyBinding(), () => Common.ToggleWindow, () => "Open and close crafting window");
			ToggleWindowSetting.Value.Enabled = true;
			ToggleWindowSetting.Value.Activated += delegate
			{
				ToggleWindow();
			};
			ToggleWindowSetting.Value.BindingChanged += Value_BindingChanged;
		}

		private void Value_BindingChanged(object sender, EventArgs e)
		{
			KeyBinding binding = sender as KeyBinding;
			if (binding != null)
			{
				binding.Enabled = true;
				binding.Activated += delegate
				{
					ToggleWindow();
				};
			}
		}

		protected override void Initialize()
		{
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			LoadData();
		}

		private void LoadData()
		{
			if (dataLoaded)
			{
				return;
			}
			if (Gw2ApiManager.HasPermissions(new TokenPermission[3]
			{
				TokenPermission.Account,
				TokenPermission.Inventories,
				TokenPermission.Characters
			}) && !PlayerItemsLoaded)
			{
				PlayerItemsLoaded = true;
				Task.Run(async delegate
				{
					await ServiceContainer.PlayerItemService.StartTimedLoadingAsync(300000);
				});
			}
			if (Gw2ApiManager.HasPermissions(new TokenPermission[3]
			{
				TokenPermission.Account,
				TokenPermission.Inventories,
				TokenPermission.Unlocks
			}) && !PlayerUnlocksLoaded)
			{
				PlayerUnlocksLoaded = true;
				Task.Run(async delegate
				{
					await ServiceContainer.PlayerUnlocksService.StartTimedLoadingAsync(300000);
				});
			}
			if (Gw2ApiManager.HasPermission(TokenPermission.Tradingpost) && !TradingPostLoaded)
			{
				TradingPostLoaded = true;
				Task.Run(async delegate
				{
					await ServiceContainer.TradingPostService.StartTimedLoadingAsync(300000);
				});
			}
			if (Gw2ApiManager.HasPermission(TokenPermission.Wallet) && !WalletLoaded)
			{
				WalletLoaded = true;
				Task.Run(async delegate
				{
					await ServiceContainer.WalletService.StartTimedLoadingAsync(300000);
				});
			}
			if (Gw2ApiManager.HasPermission(TokenPermission.Wallet))
			{
				dataLoaded = true;
			}
		}

		protected override async Task LoadAsync()
		{
			ServiceContainer.Register(Gw2ApiManager, DirectoriesManager, ContentsManager);
			await ServiceContainer.LoadAsync();
			LoadData();
		}

		private void BuildCornerIcon()
		{
			_ = ServiceContainer.TextureRepository;
			_cornerIcon = new CornerIcon
			{
				IconName = "Mystic Crafting",
				Icon = ServiceContainer.TextureRepository.GetRefTexture("156685_small.png"),
				HoverIcon = ServiceContainer.TextureRepository.GetRefTexture("156685_big.png"),
				Priority = -1080462457,
				Width = 64,
				Height = 64
			};
			_mainWindow = BuildWindow();
			_mainView = new MainView();
			_cornerIcon.Click += delegate
			{
				ToggleWindow();
			};
		}

		private void ToggleWindow()
		{
			if (_mainWindow.CurrentView == null)
			{
				_mainWindow.Show(_mainView);
			}
			_mainWindow.ToggleWindow();
		}

		private StandardWindow BuildWindow()
		{
			AsyncTexture2D refTexture = ServiceContainer.TextureRepository.GetRefTexture("155978.png");
			int width = refTexture.Width + 80;
			return new StandardWindow(windowSize: new Point(width, refTexture.Height - 150), background: refTexture, windowRegion: new Microsoft.Xna.Framework.Rectangle(40, 26, 925, 730), contentRegion: new Microsoft.Xna.Framework.Rectangle(50, 50, 900, 720))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = "Mystic Crafting",
				Emblem = ServiceContainer.TextureRepository.GetRefTexture("emblem.png"),
				SavesPosition = true,
				Id = "StandardWindow_ExampleModule_38d37290-b5f9-447d-97ea-45b0b50e5f56"
			};
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			BuildCornerIcon();
			Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;
			GameService.Overlay.UserLocaleChanged += async delegate
			{
				_mainWindow?.Dispose();
				_mainWindow = BuildWindow();
				await ServiceContainer.LoadAsync();
			};
			base.OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			Settings = null;
			_cornerIcon?.Dispose();
			_mainView?.DoUnload();
			_mainWindow?.Dispose();
			ServiceContainer.Dispose();
			ServiceCollection = null;
		}
	}
}
