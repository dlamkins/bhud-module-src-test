using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery;
using MysticCrafting.Module.RecipeTree;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;
using MysticCrafting.Module.Update;
using SQLitePCL;
using SemVer;

namespace MysticCrafting.Module
{
	[Export(typeof(Module))]
	public class MysticCraftingModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal static ModuleSettings Settings;

		private CornerIcon _cornerIcon;

		private static StandardWindow _mainWindow;

		private DiscoveryTabView _discoveryTabView;

		private RecipeDetailsView _recipeDetailsView;

		private TabbedWindow2 _mainTabbedWindow;

		private static VersionUpdateWindow _updateWindow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		private static ServiceContainer ServiceCollection { get; set; }

		[ImportingConstructor]
		public MysticCraftingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
			Settings.ToggleWindowSetting.get_Value().set_Enabled(true);
			Settings.ToggleWindowSetting.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				ToggleWindow();
			});
			Settings.ToggleWindowSetting.get_Value().add_BindingChanged((EventHandler<EventArgs>)Value_BindingChanged);
		}

		private void Value_BindingChanged(object sender, EventArgs e)
		{
			KeyBinding binding = (KeyBinding)((sender is KeyBinding) ? sender : null);
			if (binding != null)
			{
				binding.set_Enabled(true);
				binding.add_Activated((EventHandler<EventArgs>)delegate
				{
					ToggleWindow();
				});
				KeyBindingUpdated();
			}
		}

		private void KeyBindingUpdated()
		{
			if (_cornerIcon != null)
			{
				_cornerIcon.set_IconName("Mystic Crafting [" + Settings.ToggleWindowSetting.get_Value().GetBindingDisplayText() + "]");
				((Control)_cornerIcon).set_BasicTooltipText(_cornerIcon.get_IconName());
			}
			if (_mainWindow != null)
			{
				string keyBind = Settings.ToggleWindowSetting.get_Value().GetBindingDisplayText();
				((WindowBase2)_mainWindow).set_Subtitle(string.IsNullOrEmpty(keyBind) ? MysticCrafting.Module.Strings.Discovery.DiscoveryWindowSubTitle : (MysticCrafting.Module.Strings.Discovery.DiscoveryWindowSubTitle + " [" + keyBind + "]"));
			}
		}

		protected override void Initialize()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			SQLite3Provider_dynamic_cdecl.Setup("e_sqlite3", (IGetFunctionPointer)new ModuleGetFunctionPointer("e_sqlite3"));
			raw.SetProvider((ISQLite3Provider)new SQLite3Provider_dynamic_cdecl());
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			LoadApiServices();
		}

		private void LoadApiServices()
		{
			Task.Run(async delegate
			{
				await ServiceContainer.ApiServiceManager.LoadServicesAsync();
			});
		}

		protected override async Task LoadAsync()
		{
			ServiceContainer.Register(Gw2ApiManager, DirectoriesManager, ContentsManager);
			await ServiceContainer.LoadAsync();
			LoadApiServices();
		}

		private void BuildCornerIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(ServiceContainer.TextureRepository.GetRefTexture("156685_small.png"));
			val.set_HoverIcon(ServiceContainer.TextureRepository.GetRefTexture("156685_big.png"));
			val.set_Priority(-1080462457);
			((Control)val).set_Width(64);
			((Control)val).set_Height(64);
			_cornerIcon = val;
			KeyBindingUpdated();
			_mainWindow = BuildWindow();
			_discoveryTabView = new DiscoveryTabView(this);
			Settings.LastAcknowledgedUpdate.add_SettingChanged((EventHandler<ValueChangedEventArgs<Version>>)delegate
			{
				if (Settings.LastAcknowledgedUpdate.get_Value() == base.ModuleParameters.get_Manifest().get_Version())
				{
					ToggleWindow();
				}
			});
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIconOnClick);
		}

		private void CornerIconOnClick(object s, MouseEventArgs e)
		{
			VersionUpdateWindow versionUpdateWindow = new VersionUpdateWindow(base.ModuleParameters.get_Manifest());
			((Control)versionUpdateWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_updateWindow = versionUpdateWindow;
			((Control)_updateWindow).add_Disposed((EventHandler<EventArgs>)delegate
			{
				_updateWindow = null;
			});
			if (Settings.LastAcknowledgedUpdate.get_Value() != base.ModuleParameters.get_Manifest().get_Version())
			{
				if (!((Control)_updateWindow).get_Visible())
				{
					((Control)_updateWindow).Show();
				}
				else
				{
					((Control)_updateWindow).Hide();
				}
			}
			else
			{
				ToggleWindow();
			}
		}

		public static void OpenUpdateWindow()
		{
			VersionUpdateWindow updateWindow = _updateWindow;
			if (updateWindow != null)
			{
				((Control)updateWindow).Show();
			}
		}

		public static bool WindowIsOpen()
		{
			return ((Control)_mainWindow).get_Visible();
		}

		private void ToggleWindow()
		{
			if (((WindowBase2)_mainWindow).get_CurrentView() == null)
			{
				_mainWindow.Show((IView)(object)_discoveryTabView);
			}
			LoadApiServices();
			((WindowBase2)_mainWindow).ToggleWindow();
		}

		private void ToggleTabbedWindow()
		{
			if (((WindowBase2)_mainTabbedWindow).get_CurrentView() == null)
			{
				((Control)_mainTabbedWindow).Show();
			}
			((WindowBase2)_mainTabbedWindow).ToggleWindow();
		}

		private StandardWindow BuildWindow()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			AsyncTexture2D refTexture = ServiceContainer.TextureRepository.GetRefTexture("155978.png");
			int width = refTexture.get_Width() + 80;
			int height = refTexture.get_Height() - 150;
			string keyBind = Settings.ToggleWindowSetting.get_Value().GetBindingDisplayText();
			StandardWindow val = new StandardWindow(refTexture, new Rectangle(40, 26, 925, 730), new Rectangle(50, 50, 900, 720), new Point(width, height));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Mystic Crafting");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.GetRefTexture("102529.png")));
			((WindowBase2)val).set_Subtitle(string.IsNullOrEmpty(keyBind) ? MysticCrafting.Module.Strings.Discovery.DiscoveryWindowSubTitle : (MysticCrafting.Module.Strings.Discovery.DiscoveryWindowSubTitle + " [" + keyBind + "]"));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("StandardWindow_ExampleModule_38d37290-b5f9-447d-97ea-45b0b50e5f56");
			return val;
		}

		private TabbedWindow2 BuildTabbedWindow()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			TabbedWindow2 val = new TabbedWindow2(ServiceContainer.TextureRepository.GetRefTexture("155978_trimmed4.png"), new Rectangle(24, 30, 1100, 800), new Rectangle(75, 30, 1040, 775));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Mystic Crafting");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.GetRefTexture("102529.png")));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("StandardWindow_ExampleModule_38d37290-b5f9-447d-97ea-45b0b50e5f56");
			return val;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			BuildCornerIcon();
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)async delegate
			{
				bool visible = ((Control)_mainWindow).get_Visible();
				StandardWindow mainWindow = _mainWindow;
				if (mainWindow != null)
				{
					((Control)mainWindow).Dispose();
				}
				_mainWindow = BuildWindow();
				if (visible)
				{
					ToggleWindow();
				}
			});
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			Settings = null;
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			((View<IDiscoveryTabPresenter>)_discoveryTabView)?.DoUnload();
			StandardWindow mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			ServiceContainer.Dispose();
			ServiceCollection = null;
		}
	}
}
