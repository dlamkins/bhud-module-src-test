using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Controls;
using Kenedia.Modules.Characters.Controls.SideMenu;
using Kenedia.Modules.Characters.Enums;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Characters
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Characters : BaseModule<Characters, MainWindow, Settings, PathCollection>
	{
		public readonly ResourceManager RM = new ResourceManager("Kenedia.Modules.Characters.Res.strings", Assembly.GetExecutingAssembly());

		private readonly Ticks _ticks = new Ticks();

		private AnchoredContainer _cornerContainer;

		private NotificationBadge _notificationBadge;

		private Kenedia.Modules.Core.Controls.CornerIcon _cornerIcon;

		private bool _saveCharacters;

		private bool _loadedCharacters;

		private bool _mapsUpdated;

		private Version _version;

		private Character_Model _currentCharacterModel;

		private CancellationTokenSource _characterFileTokenSource;

		public SearchFilterCollection SearchFilters { get; } = new SearchFilterCollection();


		public SearchFilterCollection TagFilters { get; } = new SearchFilterCollection();


		public TagList Tags { get; } = new TagList();


		public CharacterSwapping CharacterSwapping { get; private set; }

		public CharacterSorting CharacterSorting { get; private set; }

		public RunIndicator RunIndicator { get; private set; }

		public RadialMenu RadialMenu { get; private set; }

		public PotraitCapture PotraitCapture { get; private set; }

		public Kenedia.Modules.Core.Controls.LoadingSpinner ApiSpinner { get; private set; }

		public TextureManager TextureManager { get; private set; }

		public ObservableCollection<Character_Model> CharacterModels { get; } = new ObservableCollection<Character_Model>();


		public Character_Model CurrentCharacterModel
		{
			get
			{
				return _currentCharacterModel;
			}
			private set
			{
				if (_currentCharacterModel != value)
				{
					if (_currentCharacterModel != null)
					{
						_currentCharacterModel.Updated -= new EventHandler(CurrentCharacterModel_Updated);
						_currentCharacterModel.IsCurrentCharacter = false;
					}
					_currentCharacterModel = value;
					if (_currentCharacterModel != null)
					{
						_currentCharacterModel.Updated += new EventHandler(CurrentCharacterModel_Updated);
						_currentCharacterModel.UpdateCharacter();
						_currentCharacterModel.IsCurrentCharacter = true;
						base.MainWindow?.SortCharacters();
					}
				}
			}
		}

		public Data Data { get; private set; }

		public OCR OCR { get; private set; }

		public string GlobalAccountsPath { get; set; }

		public string CharactersPath => base.Paths.AccountPath + "characters.json";

		public string AccountImagesPath => base.Paths.AccountPath + "images\\";

		public GW2API_Handler GW2APIHandler { get; private set; }

		[ImportingConstructor]
		public Characters([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			HasGUI = true;
		}

		public override IView GetSettingsView()
		{
			return new SettingsView(delegate
			{
				base.SettingsWindow?.ToggleWindow();
			});
		}

		protected override async void OnLocaleChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> eventArgs)
		{
			await GW2APIHandler.FetchLocale(eventArgs?.NewValue, !_mapsUpdated);
			_mapsUpdated = true;
			base.OnLocaleChanged(sender, eventArgs);
		}

		protected override void Initialize()
		{
			base.Initialize();
			BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Starting " + base.Name + " v." + (object)base.ModuleVersion);
			JsonConvert.set_DefaultSettings((Func<JsonSerializerSettings>)delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Expected O, but got Unknown
				JsonSerializerSettings val = new JsonSerializerSettings();
				val.set_Formatting((Formatting)1);
				val.set_NullValueHandling((NullValueHandling)1);
				return val;
			});
			Data = new Data(base.ContentsManager, base.Paths);
			GlobalAccountsPath = base.Paths.ModulePath + "\\accounts.json";
			if (!File.Exists(base.Paths.ModulePath + "\\gw2.traineddata") || base.Settings.Version.Value != base.ModuleVersion)
			{
				using Stream destination = File.Create(base.Paths.ModulePath + "\\gw2.traineddata");
				Stream fileStream = base.ContentsManager.GetFileStream("data\\gw2.traineddata");
				fileStream.Seek(0L, SeekOrigin.Begin);
				fileStream.CopyTo(destination);
			}
			if (!File.Exists(base.Paths.ModulePath + "\\tesseract.dll") || base.Settings.Version.Value != base.ModuleVersion)
			{
				using Stream target = File.Create(base.Paths.ModulePath + "\\tesseract.dll");
				Stream fileStream2 = base.ContentsManager.GetFileStream("data\\tesseract.dll");
				fileStream2.Seek(0L, SeekOrigin.Begin);
				fileStream2.CopyTo(target);
			}
			CreateToggleCategories();
			base.Settings.ShortcutKey.Value.Enabled = true;
			base.Settings.ShortcutKey.Value.Activated += ShortcutWindowToggle;
			base.Settings.RadialKey.Value.Enabled = true;
			base.Settings.RadialKey.Value.Activated += RadialMenuToggle;
			Tags.CollectionChanged += Tags_CollectionChanged;
			_version = base.Settings.Version.Value;
			base.Settings.Version.Value = base.ModuleVersion;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			base.Settings.ShowCornerIcon.SettingChanged += ShowCornerIcon_SettingChanged;
			base.Settings.UseBetaGamestate.SettingChanged += UseBetaGamestate_SettingChanged;
			base.CoreServices.GameStateDetectionService.Enabled = base.Settings.UseBetaGamestate.Value;
		}

		private void UseBetaGamestate_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			base.CoreServices.GameStateDetectionService.Enabled = e.NewValue;
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			CharacterSwapping = new CharacterSwapping(base.Settings, base.CoreServices.GameStateDetectionService, CharacterModels);
			CharacterSorting = new CharacterSorting(base.Settings, base.CoreServices.GameStateDetectionService, CharacterModels);
			CharacterSwapping.CharacterSorting = CharacterSorting;
			CharacterSorting.CharacterSwapping = CharacterSwapping;
			TextureManager = new TextureManager();
			await Data.Load();
			if (base.Settings.LoadCachedAccounts.Value)
			{
				await LoadCharacters();
			}
			Data.StaticInfo.BetaStateChanged += new EventHandler<bool>(StaticInfo_BetaStateChanged);
		}

		private void StaticInfo_BetaStateChanged(object sender, bool e)
		{
			base.MainWindow?.AdjustForBeta();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			GW2APIHandler = new GW2API_Handler(base.Gw2ApiManager, new Action<IApiV2ObjectList<Character>>(AddOrUpdateCharacters), () => ApiSpinner, base.Paths, Data, () => _notificationBadge);
			GW2APIHandler.AccountChanged += new PropertyChangedEventHandler(GW2APIHandler_AccountChanged);
			base.Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;
			if (base.Settings.ShowCornerIcon.Value)
			{
				CreateCornerIcons();
			}
			CharacterModels.CollectionChanged += OnCharacterCollectionChanged;
			base.CoreServices.InputDetectionService.ClickedOrKey += new EventHandler<double>(InputDetectionService_ClickedOrKey);
		}

		private void GW2APIHandler_AccountChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Paths.AccountName))
			{
				string? accountName = base.Paths.AccountName;
				Account account = GW2APIHandler.Account;
				if (accountName != ((account != null) ? account.get_Name() : null))
				{
					PathCollection paths = base.Paths;
					Account account2 = GW2APIHandler.Account;
					paths.AccountName = ((account2 != null) ? account2.get_Name() : null);
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Account changed. Wipe all account bound data of this session.");
					CharacterModels.Clear();
					base.MainWindow?.CharacterCards.Clear();
					base.MainWindow?.LoadedModels.Clear();
				}
			}
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			base.Update(gameTime);
			_ticks.Global += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_ticks.APIUpdate += gameTime.get_ElapsedGameTime().TotalSeconds;
			_ticks.Save += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_ticks.Tags += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_ticks.OCR += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_ticks.Global > 500.0)
			{
				_ticks.Global = 0.0;
				PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
				string name = ((player != null) ? player.Name : string.Empty);
				bool charSelection = (base.Settings.UseBetaGamestate.Value ? base.CoreServices.GameStateDetectionService.IsCharacterSelection : (!GameService.GameIntegration.Gw2Instance.IsInGame));
				CurrentCharacterModel = ((!charSelection) ? CharacterModels.FirstOrDefault((Character_Model e) => e.Name == name) : null);
				if (!_mapsUpdated && GameService.Gw2Mumble.CurrentMap.Id > 0 && Data.GetMapById(GameService.Gw2Mumble.CurrentMap.Id).Id == 0)
				{
					OnLocaleChanged(this, new Blish_HUD.ValueChangedEventArgs<Locale>((Locale)5, GameService.Overlay.UserLocale.Value));
					_mapsUpdated = true;
				}
				if (CurrentCharacterModel != null)
				{
					CurrentCharacterModel?.UpdateCharacter(player);
				}
				Data?.StaticInfo?.CheckBeta();
			}
			if (_ticks.APIUpdate > 300.0)
			{
				_ticks.APIUpdate = 0.0;
				GW2APIHandler.CheckAPI();
			}
			if (_ticks.Save > 25.0 && _saveCharacters)
			{
				_ticks.Save = 0.0;
				SaveCharacterList();
				_saveCharacters = false;
			}
		}

		protected override void Unload()
		{
			TextureManager = null;
			DeleteCornerIcons();
			CharacterModels.CollectionChanged -= OnCharacterCollectionChanged;
			Tags.CollectionChanged -= Tags_CollectionChanged;
			base.CoreServices.ClientWindowService.ResolutionChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Point>>(ClientWindowService_ResolutionChanged);
			base.Unload();
		}

		protected override void LoadGUI()
		{
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			base.LoadGUI();
			RadialMenu = new RadialMenu(base.Settings, CharacterModels, GameService.Graphics.SpriteScreen, () => CurrentCharacterModel, Data, TextureManager)
			{
				Visible = false,
				ZIndex = 1073741823
			};
			PotraitCapture = new PotraitCapture(base.CoreServices.ClientWindowService, base.CoreServices.SharedSettings, TextureManager)
			{
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false,
				ZIndex = 1073741822,
				AccountImagePath = () => AccountImagesPath
			};
			OCR = new OCR(base.CoreServices.ClientWindowService, base.CoreServices.SharedSettings, base.Settings, base.Paths.ModulePath, CharacterModels);
			RunIndicator = new RunIndicator(CharacterSorting, CharacterSwapping, base.Settings.ShowStatusWindow, TextureManager, base.Settings.ShowChoyaSpinner);
			AsyncTexture2D settingsBg = AsyncTexture2D.FromAssetId(155997);
			Texture2D cutSettingsBg = settingsBg.Texture.GetRegion(0, 0, settingsBg.Width - 482, settingsBg.Height - 390);
			base.SettingsWindow = new SettingsWindow(settingsBg, new Rectangle(30, 30, cutSettingsBg.get_Width() + 10, cutSettingsBg.get_Height()), new Rectangle(30, 35, cutSettingsBg.get_Width() - 5, cutSettingsBg.get_Height() - 15), base.SharedSettingsView, OCR, base.Settings)
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = "❤",
				Subtitle = "❤",
				SavesPosition = true,
				Id = base.Name + " SettingsWindow",
				Version = base.ModuleVersion
			};
			Texture2D bg = TextureManager.GetBackground(Kenedia.Modules.Characters.Services.TextureManager.Backgrounds.MainWindow);
			Texture2D cutBg = bg.GetRegion(25, 25, bg.get_Width() - 100, bg.get_Height() - 325);
			base.MainWindow = new MainWindow(bg, new Rectangle(25, 25, cutBg.get_Width() + 10, cutBg.get_Height()), new Rectangle(35, 14, cutBg.get_Width() - 10, cutBg.get_Height() - 10), base.Settings, TextureManager, CharacterModels, SearchFilters, TagFilters, new Action(OCR.ToggleContainer), delegate
			{
				PotraitCapture.ToggleVisibility();
			}, async delegate
			{
				await GW2APIHandler.CheckAPI();
			}, () => AccountImagesPath, Tags, () => CurrentCharacterModel, Data, CharacterSorting)
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = "❤",
				Subtitle = "❤",
				SavesPosition = true,
				Id = base.Name + " MainWindow",
				Name = base.Name,
				MainWindowEmblem = AsyncTexture2D.FromAssetId(156015),
				CanResize = true,
				Size = base.Settings.WindowSize.Value,
				SettingsWindow = (SettingsWindow)base.SettingsWindow,
				Version = base.ModuleVersion
			};
			SideMenu sideMenu = base.MainWindow.SideMenu;
			SideMenuToggles obj = new SideMenuToggles(TextureManager, TagFilters, SearchFilters, delegate
			{
				base.MainWindow?.FilterCharacters();
			}, Tags, Data)
			{
				Width = base.MainWindow.SideMenu.Width,
				Icon = AsyncTexture2D.FromAssetId(440021)
			};
			SideMenuToggles _toggles = obj;
			sideMenu.AddTab(obj);
			base.MainWindow.SideMenu.AddTab(new SideMenuBehaviors(RM, TextureManager, base.Settings, delegate
			{
				base.MainWindow?.SortCharacters();
			})
			{
				Icon = AsyncTexture2D.FromAssetId(156909)
			});
			base.MainWindow.SideMenu.TogglesTab = _toggles;
			base.MainWindow.SideMenu.SwitchTab(_toggles);
			base.MainWindow?.CreateCharacterControls();
			PotraitCapture.OnImageCaptured = delegate
			{
				base.MainWindow.CharacterEdit.LoadImages(null, null);
				base.MainWindow.CharacterEdit.ShowImages();
			};
			OCR.MainWindow = base.MainWindow;
			CharacterSwapping.HideMainWindow = new Action(base.MainWindow.Hide);
			CharacterSwapping.OCR = OCR;
			CharacterSorting.OCR = OCR;
			CharacterSorting.UpdateCharacterList = new Action(base.MainWindow.PerformFiltering);
			base.CoreServices.ClientWindowService.ResolutionChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Point>>(ClientWindowService_ResolutionChanged);
			GW2APIHandler.MainWindow = base.MainWindow;
		}

		private void ClientWindowService_ResolutionChanged(object sender, Blish_HUD.ValueChangedEventArgs<Point> e)
		{
			base.MainWindow?.CheckOCRRegion();
		}

		protected override void UnloadGUI()
		{
			base.UnloadGUI();
			RadialMenu?.Dispose();
			base.SettingsWindow?.Dispose();
			base.MainWindow?.Dispose();
			PotraitCapture?.Dispose();
			OCR?.Dispose();
			RunIndicator?.Dispose();
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			GW2APIHandler.CheckAPI();
		}

		private void ShortcutWindowToggle(object sender, EventArgs e)
		{
			if (!(Control.ActiveControl is Blish_HUD.Controls.TextBox))
			{
				base.MainWindow?.ToggleWindow();
			}
		}

		private void RadialMenuToggle(object sender, EventArgs e)
		{
			if (base.Settings.EnableRadialMenu.Value && (RadialMenu?.HasDisplayedCharacters() ?? false))
			{
				RadialMenu?.ToggleVisibility();
			}
		}

		private void CurrentCharacterModel_Updated(object sender, EventArgs e)
		{
			base.MainWindow?.SortCharacters();
		}

		private void InputDetectionService_ClickedOrKey(object sender, double e)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.GameIntegration.Gw2Instance.Gw2HasFocus && (!base.Settings.CancelOnlyOnESC.Value || GameService.Input.Keyboard.KeysDown.Contains((Keys)27)))
			{
				List<Keys> keys = new List<Keys>
				{
					base.Settings.LogoutKey.Value.PrimaryKey,
					(Keys)13
				};
				if (GameService.Input.Keyboard.KeysDown.Except(keys).Count() > 0)
				{
					CancelEverything();
				}
			}
		}

		private void CancelEverything()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Invalid comparison between Unknown and I4
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Invalid comparison between Unknown and I4
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Invalid comparison between Unknown and I4
			MouseState mouse = GameService.Input.Mouse.State;
			if (CharacterSwapping.Cancel())
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info(string.Format("Cancel any automated action. Left Mouse Down: {0} | Right Mouse Down: {1} | Keyboard Keys pressed {2}", (int)((MouseState)(ref mouse)).get_LeftButton() == 1, (int)((MouseState)(ref mouse)).get_RightButton() == 1, string.Join("|", GameService.Input.Keyboard.KeysDown.Select((Keys k) => ((object)(Keys)(ref k)).ToString()).ToArray())));
			}
			if (CharacterSorting.Cancel())
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info(string.Format("Cancel any automated action. Left Mouse Down: {0} | Right Mouse Down: {1} | Keyboard Keys pressed {2}", (int)((MouseState)(ref mouse)).get_LeftButton() == 1, (int)((MouseState)(ref mouse)).get_RightButton() == 1, string.Join("|", GameService.Input.Keyboard.KeysDown.Select((Keys k) => ((object)(Keys)(ref k)).ToString()).ToArray())));
			}
		}

		protected override void ReloadKey_Activated(object sender, EventArgs e)
		{
			BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Debug("ReloadKey_Activated: " + base.Name);
			base.ReloadKey_Activated(sender, e);
			CreateCornerIcons();
			GameService.Graphics.SpriteScreen.Visible = true;
			base.MainWindow?.ToggleWindow();
			base.SettingsWindow?.ToggleWindow();
		}

		private void OnCharacterCollectionChanged(object sender, EventArgs e)
		{
			base.MainWindow?.CreateCharacterControls();
		}

		private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateTagsCollection();
		}

		private void UpdateTagsCollection()
		{
			foreach (Character_Model characterModel in CharacterModels)
			{
				characterModel.UpdateTags(Tags);
			}
		}

		private void CreateCornerIcons()
		{
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			DeleteCornerIcons();
			_cornerIcon = new Kenedia.Modules.Core.Controls.CornerIcon
			{
				Icon = AsyncTexture2D.FromAssetId(156678),
				HoverIcon = AsyncTexture2D.FromAssetId(156679),
				SetLocalizedTooltip = () => string.Format(strings.Toggle, base.Name ?? ""),
				Priority = 51294256,
				Parent = GameService.Graphics.SpriteScreen,
				Visible = base.Settings.ShowCornerIcon.Value,
				ClickAction = delegate
				{
					base.MainWindow?.ToggleWindow();
				}
			};
			_cornerContainer = new AnchoredContainer
			{
				Parent = GameService.Graphics.SpriteScreen,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				Anchor = _cornerIcon,
				AnchorPosition = AnchoredContainer.AnchorPos.Bottom,
				RelativePosition = new RectangleDimensions(0, -_cornerIcon.Height / 2),
				CaptureInput = CaptureType.Filter
			};
			_notificationBadge = new NotificationBadge
			{
				Location = new Point(_cornerIcon.Width - 15, 0),
				Parent = _cornerContainer,
				Size = new Point(20),
				Opacity = 0.6f,
				HoveredOpacity = 1f,
				CaptureInput = CaptureType.Filter,
				Anchor = _cornerIcon,
				Visible = false
			};
			ApiSpinner = new Kenedia.Modules.Core.Controls.LoadingSpinner
			{
				Location = new Point(0, _notificationBadge.Bottom),
				Parent = _cornerContainer,
				Size = _cornerIcon.Size,
				BasicTooltipText = strings_common.FetchingApiData,
				Visible = false,
				CaptureInput = null
			};
		}

		private void DeleteCornerIcons()
		{
			_cornerIcon?.Dispose();
			_cornerIcon = null;
			_cornerContainer?.Dispose();
			_cornerContainer = null;
		}

		private void ShowCornerIcon_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			if (e.NewValue)
			{
				CreateCornerIcons();
			}
			else
			{
				DeleteCornerIcons();
			}
		}

		private void CreateToggleCategories()
		{
			new List<SearchFilter<Character_Model>>();
			foreach (KeyValuePair<ProfessionType, Data.Profession> e4 in Data.Professions)
			{
				SearchFilters.Add(e4.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.Value["Profession"].Check && c.Profession == e4.Key));
				SearchFilters.Add("Core " + e4.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.Value["Profession"].Check && c.Profession == e4.Key));
			}
			foreach (KeyValuePair<SpecializationType, Data.Specialization> e3 in Data.Specializations)
			{
				SearchFilters.Add(e3.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.Value["Profession"].Check && c.Specialization == e3.Key));
			}
			foreach (KeyValuePair<int, Data.CraftingProfession> e2 in Data.CrafingProfessions)
			{
				SearchFilters.Add(e2.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => c.Crafting.Find((CharacterCrafting p) => base.Settings.DisplayToggles.Value["CraftingProfession"].Check && p.Id == e2.Value.Id && (!base.Settings.DisplayToggles.Value["OnlyMaxCrafting"].Check || p.Rating >= e2.Value.MaxRating)) != null));
			}
			foreach (KeyValuePair<RaceType, Data.Race> e in Data.Races)
			{
				SearchFilters.Add(e.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.Value["Race"].Check && c.Race == e.Key));
			}
			SearchFilters.Add("Birthday", new SearchFilter<Character_Model>((Character_Model c) => c.HasBirthdayPresent));
			SearchFilters.Add("Hidden", new SearchFilter<Character_Model>((Character_Model c) => !c.Show || (!Data.StaticInfo.IsBeta && c.Beta)));
			SearchFilters.Add("Female", new SearchFilter<Character_Model>((Character_Model c) => (int)c.Gender == 2));
			SearchFilters.Add("Male", new SearchFilter<Character_Model>((Character_Model c) => (int)c.Gender == 1));
		}

		private async void AddOrUpdateCharacters(IApiV2ObjectList<Character> characters)
		{
			if (!_loadedCharacters && base.Settings.LoadCachedAccounts.Value)
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("This is our first API data fetched for this character/session. Trying to load local data first.");
				if (!(await LoadCharacters()).HasValue)
				{
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Checking the cache.");
				}
			}
			BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Update characters for '" + base.Paths.AccountName + "' based on fresh data from the api.");
			if (base.Paths.AccountName == null || ((IReadOnlyCollection<Character>)characters).Count <= 0)
			{
				return;
			}
			var freshList = ((IEnumerable<Character>)characters).Select((Character c) => new
			{
				Name = c.get_Name(),
				Created = c.get_Created()
			}).ToList();
			var oldList = CharacterModels.Select((Character_Model c) => new { c.Name, c.Created }).ToList();
			bool updateMarkedCharacters = false;
			for (int i = CharacterModels.Count - 1; i >= 0; i--)
			{
				Character_Model c3 = CharacterModels[i];
				if (c3 != null && !freshList.Contains(new { c3.Name, c3.Created }))
				{
					if (base.Settings.AutomaticCharacterDelete.Value)
					{
						BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger?.Info($"{c3?.Name} created on {c3?.Created} no longer exists. Delete them!");
						c3?.Delete();
					}
					else if (!c3.MarkedAsDeleted)
					{
						BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger?.Info($"{c3.Name} created on {c3.Created} does not exist in the api data. Mark them as potentially deleted!");
						c3.MarkedAsDeleted = true;
						updateMarkedCharacters = true;
					}
				}
			}
			if (updateMarkedCharacters)
			{
				base.MainWindow.UpdateMissingNotification();
			}
			int pos = 0;
			foreach (Character c2 in (IEnumerable<Character>)characters)
			{
				if (!oldList.Contains(new
				{
					Name = c2.get_Name(),
					Created = c2.get_Created()
				}))
				{
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info($"{c2.get_Name()} created on {c2.get_Created()} does not exist yet. Create them!");
					CharacterModels.Add(new Character_Model(c2, CharacterSwapping, base.Paths.ModulePath, new Action(RequestCharacterSave), CharacterModels, Data)
					{
						Position = pos
					});
				}
				else
				{
					Character_Model character = CharacterModels.FirstOrDefault((Character_Model e) => e.Name == c2.get_Name());
					character?.UpdateCharacter(c2);
					if (character != null)
					{
						character.Position = pos;
					}
				}
				pos++;
			}
			base.MainWindow?.CreateCharacterControls();
			base.MainWindow?.PerformFiltering();
			SaveCharacterList();
		}

		private async Task<bool?> LoadCharacters()
		{
			PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
			if ((player == null || string.IsNullOrEmpty(player.Name)) && string.IsNullOrEmpty(base.Paths.AccountName))
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Player name is currently null or empty. Can not check for the account.");
				return null;
			}
			AccountSummary account = getAccount();
			if (account != null || !string.IsNullOrEmpty(base.Paths.AccountName))
			{
				PathCollection paths = base.Paths;
				if (paths.AccountName == null)
				{
					paths.AccountName = account.AccountName;
				}
				_loadedCharacters = true;
				base.Settings.LoadAccountSettings(base.Paths.AccountName);
				if (!Directory.Exists(AccountImagesPath))
				{
					Directory.CreateDirectory(AccountImagesPath);
				}
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Found '" + (player.Name ?? "Unkown Player name.") + "' in a stored character list for '" + base.Paths.AccountName + "'. Loading characters of '" + base.Paths.AccountName + "'");
				return await LoadCharacterFile();
			}
			return false;
			AccountSummary getAccount()
			{
				try
				{
					string path = GlobalAccountsPath;
					if (File.Exists(path))
					{
						return JsonConvert.DeserializeObject<List<AccountSummary>>(File.ReadAllText(path), SerializerSettings.Default).Find((AccountSummary e) => e.CharacterNames.Contains(player.Name));
					}
				}
				catch (Exception)
				{
				}
				return null;
			}
		}

		private async Task<bool> LoadCharacterFile()
		{
			try
			{
				_characterFileTokenSource?.Cancel();
				_characterFileTokenSource = new CancellationTokenSource();
				bool flag = File.Exists(CharactersPath);
				if (flag)
				{
					flag = await FileExtension.WaitForFileUnlock(CharactersPath, 2500, _characterFileTokenSource.Token);
				}
				if (flag)
				{
					new FileInfo(CharactersPath);
					string text = File.ReadAllText(CharactersPath);
					_ = GameService.Gw2Mumble.PlayerCharacter;
					List<Character_Model> characters = JsonConvert.DeserializeObject<List<Character_Model>>(text, SerializerSettings.Default);
					List<string> names = CharacterModels.Select((Character_Model c) => c.Name).ToList();
					if (characters != null)
					{
						characters.ForEach(delegate(Character_Model c)
						{
							//IL_0092: Unknown result type (might be due to invalid IL or missing references)
							//IL_009c: Expected O, but got Unknown
							if (!names.Contains(c.Name))
							{
								Tags.AddTags(c.Tags);
								CharacterModels.Add(new Character_Model(c, CharacterSwapping, base.Paths.ModulePath, new Action(RequestCharacterSave), CharacterModels, Data)
								{
									Beta = (_version >= new Version(1, 0, 20, (string)null, (string)null) && c.Beta)
								});
								names.Add(c.Name);
							}
						});
						BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Loaded local characters from file '" + CharactersPath + "'.");
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Warn(ex, "Failed to load the local characters from file '" + CharactersPath + "'.");
				File.Copy(CharactersPath, CharactersPath.Replace(".json", " [" + DateTimeOffset.Now.ToUnixTimeSeconds() + "].corrupted.json"));
				return false;
			}
		}

		private async Task SaveCharacterList()
		{
			try
			{
				_characterFileTokenSource?.Cancel();
				_characterFileTokenSource = new CancellationTokenSource();
				if (await FileExtension.WaitForFileUnlock(CharactersPath, 2500, _characterFileTokenSource.Token))
				{
					string json = JsonConvert.SerializeObject((object)CharacterModels, SerializerSettings.Default);
					File.WriteAllText(CharactersPath, json);
				}
				else if (!_characterFileTokenSource.IsCancellationRequested)
				{
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Info("Failed to save the characters file '" + CharactersPath + "'.");
				}
			}
			catch (Exception ex)
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Warn("Failed to save the characters file '" + CharactersPath + "'.");
				BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Warn($"{ex}");
			}
		}

		private void RequestCharacterSave()
		{
			_saveCharacters = true;
		}
	}
}
