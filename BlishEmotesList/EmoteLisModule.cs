using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using felix.BlishEmotes;
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

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _emoteListMenuStrip;

		private TabbedWindow2 _settingsWindow;

		private RadialMenu _radialMenu;

		public ModuleSettings Settings;

		private List<felix.BlishEmotes.Emote> _emotes;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		private List<felix.BlishEmotes.Emote> _radialEnabledEmotes => new List<felix.BlishEmotes.Emote>(_emotes.Where((felix.BlishEmotes.Emote el) => !Settings.EmotesRadialEnabledMap.ContainsKey(el) || Settings.EmotesRadialEnabledMap[el].Value));

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
			Settings.OnAnyEmotesRadialSettingsChanged += delegate
			{
				if (_radialMenu != null)
				{
					_radialMenu.Emotes = _radialEnabledEmotes;
				}
			};
		}

		protected override void Initialize()
		{
			Gw2ApiManager.SubtokenUpdated += OnApiSubTokenUpdated;
			_emotes = new List<felix.BlishEmotes.Emote>();
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
				_emotes = LoadEmotesResource();
				await UpdateEmotesFromApi();
				Settings.InitEmotesSettings(_emotes);
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
		}

		private void DrawUI()
		{
			_emoteListMenuStrip?.Dispose();
			_emoteListMenuStrip = new ContextMenuStrip();
			List<ContextMenuStripItem> menuItems = (Settings.GlobalUseCategories.Value ? GetCategoryMenuItems() : GetEmotesMenuItems(_emotes));
			_emoteListMenuStrip.AddMenuItems(menuItems);
			_radialMenu?.Dispose();
			_radialMenu = new RadialMenu(_helper, Settings, ContentsManager.GetTexture("textures/2107931.png"))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Emotes = _radialEnabledEmotes
			};
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

		private List<ContextMenuStripItem> GetCategoryMenuItems()
		{
			List<ContextMenuStripItem> items = new List<ContextMenuStripItem>();
			foreach (Category categoryEnum in Enum.GetValues(typeof(Category)))
			{
				List<felix.BlishEmotes.Emote> emotesForCategory = _emotes.Where((felix.BlishEmotes.Emote emote) => emote.Category == categoryEnum).ToList();
				ContextMenuStrip categorySubMenu = new ContextMenuStrip();
				categorySubMenu.AddMenuItems(GetEmotesMenuItems(emotesForCategory));
				ContextMenuStripItem menuItem = new ContextMenuStripItem
				{
					Text = categoryEnum.Label(),
					Submenu = categorySubMenu
				};
				items.Add(menuItem);
			}
			return items;
		}

		private List<ContextMenuStripItem> GetEmotesMenuItems(List<felix.BlishEmotes.Emote> emotes)
		{
			List<ContextMenuStripItem> items = new List<ContextMenuStripItem>();
			foreach (felix.BlishEmotes.Emote emote in emotes)
			{
				ContextMenuStripItem menuItem = new ContextMenuStripItem
				{
					Text = _helper.EmotesResourceManager.GetString(emote.Id),
					Enabled = !emote.Locked
				};
				menuItem.Click += delegate
				{
					_helper.SendEmoteCommand(emote);
				};
				items.Add(menuItem);
			}
			items.Sort((ContextMenuStripItem x, ContextMenuStripItem y) => x.Text.CompareTo(y.Text));
			return items;
		}

		private async Task UpdateEmotesFromApi()
		{
			Logger.Debug("Update emotes from api");
			ApiEmotesReturn apiEmotes = await LoadEmotesFromApi();
			foreach (felix.BlishEmotes.Emote emote in _emotes)
			{
				emote.Locked = false;
				if (apiEmotes.UnlockableEmotesIds.Contains(emote.Id) && !apiEmotes.UnlockedEmotesIds.Contains(emote.Id))
				{
					emote.Locked = true;
				}
			}
		}

		private List<felix.BlishEmotes.Emote> LoadEmotesResource()
		{
			string fileContents;
			using (StreamReader reader = new StreamReader(ContentsManager.GetFileStream("json/emotes.json")))
			{
				fileContents = reader.ReadToEnd();
			}
			List<felix.BlishEmotes.Emote> emotes = JsonConvert.DeserializeObject<List<felix.BlishEmotes.Emote>>(fileContents);
			foreach (felix.BlishEmotes.Emote emote in emotes)
			{
				emote.Texture = ContentsManager.GetTexture("textures/emotes/" + emote.TextureRef, ContentsManager.GetTexture("textures/missing-texture.png"));
			}
			return emotes;
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
		}
	}
}
