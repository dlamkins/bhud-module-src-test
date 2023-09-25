using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using SemVer;
using felix.BlishEmotes;
using felix.BlishEmotes.Exceptions;
using felix.BlishEmotes.Strings;
using felix.BlishEmotes.UI.Controls;
using felix.BlishEmotes.UI.Views;

namespace BlishEmotesList
{
	[Export(typeof(Module))]
	public class EmoteLisModule : Module
	{
		private struct ApiEmotesReturn
		{
			public List<string> UnlockableEmotesIds;

			public List<string> UnlockedEmotesIds;
		}

		private static readonly Logger Logger = Logger.GetLogger<EmoteLisModule>();

		private Helper _helper;

		internal PersistenceManager PersistenceManager;

		internal CategoriesManager CategoriesManager;

		internal EmotesManager EmotesManager;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _emoteListMenuStrip;

		private TabbedWindow2 _settingsWindow;

		private RadialMenu _radialMenu;

		public ModuleSettings Settings;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public EmoteLisModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			_helper = new Helper();
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings, _helper);
			Settings.GlobalHideCornerIcon.SettingChanged += delegate(object sender, ValueChangedEventArgs<bool> args)
			{
				if (args.NewValue)
				{
					_cornerIcon?.Dispose();
				}
				else
				{
					InitCornerIcon();
				}
			};
			Settings.GlobalUseCategories.SettingChanged += delegate
			{
				DrawUI();
			};
			Settings.GlobalKeyBindToggleEmoteList.Value.Enabled = true;
			Settings.GlobalKeyBindToggleEmoteList.Value.Activated += delegate
			{
				if (!GameService.GameIntegration.Gw2Instance.IsInGame)
				{
					Logger.Debug("Disabled outside game.");
				}
				else if (Settings.GlobalUseRadialMenu.Value)
				{
					_radialMenu?.Show();
				}
				else
				{
					ShowEmoteList(atCornerIcon: false);
				}
			};
			Settings.GlobalKeyBindToggleSynchronize.Value.Enabled = true;
			Settings.GlobalKeyBindToggleSynchronize.Value.Activated += delegate
			{
				_helper.IsEmoteSynchronized = !_helper.IsEmoteSynchronized;
				DrawUI(excludeRadial: true);
				Logger.Debug("Toggled IsEmoteSynchronized");
			};
			Settings.GlobalKeyBindToggleTargeting.Value.Enabled = true;
			Settings.GlobalKeyBindToggleTargeting.Value.Activated += delegate
			{
				_helper.IsEmoteTargeted = !_helper.IsEmoteTargeted;
				DrawUI(excludeRadial: true);
				Logger.Debug("Toggled IsEmoteTargeted");
			};
			Settings.OnAnyEmotesRadialSettingsChanged += delegate
			{
				if (_radialMenu != null)
				{
					_radialMenu.Emotes = EmotesManager?.GetRadial();
				}
			};
		}

		protected override void Initialize()
		{
			if (Program.OverlayVersion < new SemVer.Version(1, 1, 0))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.TacO, new object[1] { true });
				}
				catch
				{
				}
			}
			Gw2ApiManager.SubtokenUpdated += OnApiSubTokenUpdated;
			PersistenceManager = new PersistenceManager(DirectoriesManager);
			EmotesManager = new EmotesManager(ContentsManager, Settings);
			CategoriesManager = new CategoriesManager(PersistenceManager);
			CategoriesManager.CategoriesUpdated += delegate
			{
				DrawUI(excludeRadial: true);
			};
			if (!Settings.GlobalHideCornerIcon.Value)
			{
				InitCornerIcon();
			}
			_settingsWindow = new TabbedWindow2(ContentsManager.GetTexture("textures\\156006.png"), new Microsoft.Xna.Framework.Rectangle(35, 36, 900, 640), new Microsoft.Xna.Framework.Rectangle(95, 42, 821, 592))
			{
				Title = Common.settings_ui_title,
				Parent = GameService.Graphics.SpriteScreen,
				Location = new Point(100, 100),
				Emblem = ContentsManager.GetTexture("textures\\102390.png"),
				Id = base.Namespace + "_SettingsWindow",
				SavesPosition = true
			};
			_settingsWindow.Tabs.Add(new Tab(ContentsManager.GetTexture("textures\\155052.png"), () => new GlobalSettingsView(Settings), Common.settings_ui_global_tab));
			_settingsWindow.Tabs.Add(new Tab(ContentsManager.GetTexture("textures\\156909.png"), () => new CategorySettingsView(CategoriesManager, EmotesManager, _helper), Common.settings_ui_categories_tab));
			_settingsWindow.Tabs.Add(new Tab(ContentsManager.GetTexture("textures\\156734+155150.png"), () => new EmoteHotkeySettingsView(Settings), Common.settings_ui_emoteHotkeys_tab));
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			DrawUI();
			base.OnModuleLoaded(e);
		}

		protected override async Task LoadAsync()
		{
			try
			{
				CategoriesManager.Load();
				EmotesManager.Load();
				await UpdateEmotesFromApi();
				CategoriesManager.ResolveEmoteIds(EmotesManager.GetAll());
				Settings.InitEmotesSettings(EmotesManager.GetAll());
				DrawUI();
			}
			catch (Exception e)
			{
				Logger.Fatal("LoadAsync failed!");
				Logger.Error(e.ToString());
			}
		}

		public override IView GetSettingsView()
		{
			return new DummySettingsView(_settingsWindow.Show);
		}

		protected override void Update(GameTime gameTime)
		{
			if (_radialMenu.Visible && !Settings.GlobalKeyBindToggleEmoteList.Value.IsTriggering)
			{
				_radialMenu.Hide();
			}
		}

		private void InitCornerIcon()
		{
			_cornerIcon?.Dispose();
			_cornerIcon = new CornerIcon
			{
				Icon = ContentsManager.GetTexture("textures/emotes_icon.png"),
				BasicTooltipText = Common.cornerIcon_tooltip,
				Priority = -620003847
			};
			_cornerIcon.Click += delegate
			{
				ShowEmoteList();
			};
			_cornerIcon.RightMouseButtonReleased += delegate
			{
				_settingsWindow.Show();
			};
		}

		private void DrawUI(bool excludeRadial = false)
		{
			_emoteListMenuStrip?.Dispose();
			_emoteListMenuStrip = new ContextMenuStrip();
			List<ContextMenuStripItem> menuItems = (Settings.GlobalUseCategories.Value ? GetCategoryMenuItems() : GetEmotesMenuItems(EmotesManager.GetAll()));
			_emoteListMenuStrip.AddMenuItems(menuItems);
			if (!excludeRadial)
			{
				_radialMenu?.Dispose();
				_radialMenu = new RadialMenu(_helper, Settings, ContentsManager.GetTexture("textures/2107931.png"))
				{
					Parent = GameService.Graphics.SpriteScreen,
					Emotes = EmotesManager.GetRadial()
				};
			}
		}

		private async void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			await UpdateEmotesFromApi();
		}

		private void ShowEmoteList(bool atCornerIcon = true)
		{
			if (atCornerIcon)
			{
				_emoteListMenuStrip?.Show(_cornerIcon);
			}
			else if (GameService.Input.Mouse.CursorIsVisible)
			{
				_emoteListMenuStrip?.Show(GameService.Input.Mouse.Position);
			}
			else
			{
				Logger.Debug("Emote list display conditions failed");
			}
		}

		private void AddEmoteModifierStatus(ref List<ContextMenuStripItem> items)
		{
			if (_helper.IsEmoteSynchronized)
			{
				items.Insert(0, new ContextMenuStripItem("[ " + Common.emote_synchronizeActive.ToUpper() + " ]"));
			}
			if (_helper.IsEmoteTargeted)
			{
				items.Insert(0, new ContextMenuStripItem("[ " + Common.emote_targetingActive.ToUpper() + " ]"));
			}
		}

		private List<ContextMenuStripItem> GetCategoryMenuItems()
		{
			List<ContextMenuStripItem> items = new List<ContextMenuStripItem>();
			foreach (Category category in CategoriesManager.GetAll())
			{
				ContextMenuStrip categorySubMenu = new ContextMenuStrip();
				categorySubMenu.AddMenuItems(GetEmotesMenuItems(category.Emotes));
				ContextMenuStripItem menuItem = new ContextMenuStripItem
				{
					Text = category.Name,
					Submenu = categorySubMenu
				};
				items.Add(menuItem);
			}
			AddEmoteModifierStatus(ref items);
			return items;
		}

		private ContextMenuStrip GetToggleFavContextMenu(felix.BlishEmotes.Emote emote)
		{
			bool isFav = false;
			try
			{
				isFav = CategoriesManager.IsEmoteInCategory(CategoriesManager.FavouriteCategoryId, emote);
			}
			catch (NotFoundException)
			{
			}
			ContextMenuStripItem toggleFavMenuItem = new ContextMenuStripItem
			{
				Text = Common.emote_categoryFavourite,
				CanCheck = true,
				Checked = isFav
			};
			toggleFavMenuItem.CheckedChanged += delegate(object sender, CheckChangedEvent args)
			{
				CategoriesManager.ToggleEmoteFromCategory(CategoriesManager.FavouriteCategoryId, emote);
				DrawUI();
				Logger.Debug($"Toggled favourite for {emote.Id} to ${args.Checked}");
			};
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.AddMenuItem(toggleFavMenuItem);
			return contextMenuStrip;
		}

		private List<ContextMenuStripItem> GetEmotesMenuItems(List<felix.BlishEmotes.Emote> emotes)
		{
			List<ContextMenuStripItem> items = new List<ContextMenuStripItem>();
			foreach (felix.BlishEmotes.Emote emote in emotes)
			{
				ContextMenuStripItem menuItem = new ContextMenuStripItem
				{
					Text = _helper.EmotesResourceManager.GetString(emote.Id),
					Enabled = !emote.Locked,
					Submenu = GetToggleFavContextMenu(emote)
				};
				menuItem.Click += delegate
				{
					_helper.SendEmoteCommand(emote);
				};
				items.Add(menuItem);
			}
			items.Sort((ContextMenuStripItem x, ContextMenuStripItem y) => x.Text.CompareTo(y.Text));
			AddEmoteModifierStatus(ref items);
			return items;
		}

		private async Task UpdateEmotesFromApi()
		{
			Logger.Debug("Update emotes from api");
			ApiEmotesReturn apiEmotes = await LoadEmotesFromApi();
			List<felix.BlishEmotes.Emote> emotes = EmotesManager.GetAll();
			foreach (felix.BlishEmotes.Emote emote in emotes)
			{
				emote.Locked = false;
				if (apiEmotes.UnlockableEmotesIds.Contains(emote.Id) && !apiEmotes.UnlockedEmotesIds.Contains(emote.Id))
				{
					emote.Locked = true;
				}
			}
			EmotesManager.UpdateAll(emotes);
		}

		private async Task<ApiEmotesReturn> LoadEmotesFromApi()
		{
			ApiEmotesReturn returnVal = default(ApiEmotesReturn);
			try
			{
				if (Gw2ApiManager.HasPermissions(new TokenPermission[3]
				{
					TokenPermission.Account,
					TokenPermission.Progression,
					TokenPermission.Unlocks
				}))
				{
					Logger.Debug("Load emotes from API");
					returnVal.UnlockableEmotesIds = new List<string>(await Gw2ApiManager.Gw2ApiClient.V2.Emotes.IdsAsync());
					returnVal.UnlockedEmotesIds = new List<string>(await Gw2ApiManager.Gw2ApiClient.V2.Account.Emotes.GetAsync());
					return returnVal;
				}
				returnVal.UnlockableEmotesIds = new List<string>();
				returnVal.UnlockedEmotesIds = new List<string>();
				return returnVal;
			}
			catch (Exception e)
			{
				Logger.Warn("Failed to fetch emotes from API");
				Logger.Debug(e.Message);
				returnVal.UnlockableEmotesIds = new List<string>();
				returnVal.UnlockedEmotesIds = new List<string>();
				return returnVal;
			}
		}

		protected override void Unload()
		{
			Gw2ApiManager.SubtokenUpdated -= OnApiSubTokenUpdated;
			Settings.Unload();
			_cornerIcon?.Dispose();
			_settingsWindow?.Dispose();
			_radialMenu?.Dispose();
			CategoriesManager.Unload();
		}
	}
}
