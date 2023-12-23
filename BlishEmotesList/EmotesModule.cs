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
using felix.BlishEmotes.Services;
using felix.BlishEmotes.Strings;
using felix.BlishEmotes.UI.Controls;
using felix.BlishEmotes.UI.Views;

namespace BlishEmotesList
{
	[Export(typeof(Module))]
	public class EmotesModule : Module
	{
		private struct ApiEmotesReturn
		{
			public List<string> UnlockableEmotesIds;

			public List<string> UnlockedEmotesIds;
		}

		internal static EmotesModule ModuleInstance;

		private static readonly Logger Logger = Logger.GetLogger<EmotesModule>();

		internal PersistenceManager PersistenceManager;

		internal TexturesManager TexturesManager;

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
		public EmotesModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
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
				DrawEmoteListContextMenu();
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
				EmotesManager.IsEmoteSynchronized = !EmotesManager.IsEmoteSynchronized;
				UpdateUI();
				Logger.Debug("Toggled IsEmoteSynchronized");
			};
			Settings.GlobalKeyBindToggleTargeting.Value.Enabled = true;
			Settings.GlobalKeyBindToggleTargeting.Value.Activated += delegate
			{
				EmotesManager.IsEmoteTargeted = !EmotesManager.IsEmoteTargeted;
				UpdateUI();
				Logger.Debug("Toggled IsEmoteTargeted");
			};
			Settings.OnAnyEmotesRadialSettingsChanged += delegate
			{
				UpdateRadialMenu();
			};
			Settings.EmoteShortcutActivated += OnEmoteSelected;
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
			TexturesManager = new TexturesManager(ContentsManager, DirectoriesManager);
			EmotesManager = new EmotesManager(ContentsManager, Settings);
			CategoriesManager = new CategoriesManager(TexturesManager, PersistenceManager);
			CategoriesManager.CategoriesUpdated += delegate
			{
				UpdateUI();
			};
			if (!Settings.GlobalHideCornerIcon.Value)
			{
				InitCornerIcon();
			}
			_settingsWindow = new TabbedWindow2(TexturesManager.GetTexture(Textures.Background), new Microsoft.Xna.Framework.Rectangle(35, 36, 900, 640), new Microsoft.Xna.Framework.Rectangle(95, 42, 821, 592))
			{
				Title = Common.settings_ui_title,
				Parent = GameService.Graphics.SpriteScreen,
				Location = new Point(100, 100),
				Emblem = TexturesManager.GetTexture(Textures.SettingsIcon),
				Id = base.Namespace + "_SettingsWindow",
				SavesPosition = true
			};
			_settingsWindow.Tabs.Add(new Tab(TexturesManager.GetTexture(Textures.GlobalSettingsIcon), () => new GlobalSettingsView(Settings), Common.settings_ui_global_tab));
			_settingsWindow.Tabs.Add(new Tab(TexturesManager.GetTexture(Textures.CategorySettingsIcon), () => new CategorySettingsView(CategoriesManager, EmotesManager), Common.settings_ui_categories_tab));
			_settingsWindow.Tabs.Add(new Tab(TexturesManager.GetTexture(Textures.HotkeySettingsIcon), () => new EmoteHotkeySettingsView(Settings), Common.settings_ui_emoteHotkeys_tab));
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
				InitUI();
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
				Icon = TexturesManager.GetTexture(Textures.ModuleIcon),
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

		private void InitUI()
		{
			DrawEmoteListContextMenu();
			_radialMenu = new RadialMenu(Settings, TexturesManager.GetTexture(Textures.LockedTexture))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Emotes = EmotesManager.GetRadial(),
				Categories = CategoriesManager.GetAll()
			};
			_radialMenu.EmoteSelected += OnEmoteSelected;
		}

		private void OnEmoteSelected(object sender, felix.BlishEmotes.Emote emote)
		{
			EmotesManager.SendEmoteCommand(emote);
		}

		private void UpdateUI()
		{
			DrawEmoteListContextMenu();
			UpdateRadialMenu();
		}

		private void UpdateRadialMenu()
		{
			if (_radialMenu != null)
			{
				_radialMenu.Categories = CategoriesManager?.GetAll() ?? new List<Category>();
				_radialMenu.Emotes = EmotesManager?.GetRadial() ?? new List<felix.BlishEmotes.Emote>();
				_radialMenu.IsEmoteSynchronized = EmotesManager.IsEmoteSynchronized;
				_radialMenu.IsEmoteTargeted = EmotesManager.IsEmoteTargeted;
			}
		}

		private void DrawEmoteListContextMenu()
		{
			_emoteListMenuStrip?.Dispose();
			_emoteListMenuStrip = new ContextMenuStrip();
			List<ContextMenuStripItem> menuItems = (Settings.GlobalUseCategories.Value ? GetCategoryMenuItems() : GetEmotesMenuItems(EmotesManager.GetAll()));
			_emoteListMenuStrip.AddMenuItems(menuItems);
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
			if (EmotesManager.IsEmoteSynchronized)
			{
				items.Insert(0, new ContextMenuStripItem("[ " + Common.emote_synchronizeActive.ToUpper() + " ]"));
			}
			if (EmotesManager.IsEmoteTargeted)
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
				DrawEmoteListContextMenu();
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
					Text = emote.Label,
					Enabled = !emote.Locked,
					Submenu = GetToggleFavContextMenu(emote)
				};
				menuItem.Click += delegate
				{
					EmotesManager.SendEmoteCommand(emote);
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
			Settings.EmoteShortcutActivated -= OnEmoteSelected;
			Settings.Unload();
			_cornerIcon?.Dispose();
			_settingsWindow?.Dispose();
			_radialMenu.EmoteSelected -= OnEmoteSelected;
			_radialMenu?.Dispose();
			CategoriesManager.Unload();
			EmotesManager.Unload();
			TexturesManager.Dispose();
			ModuleInstance = null;
		}
	}
}
