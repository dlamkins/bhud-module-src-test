using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters
{
	[Export(typeof(Module))]
	public class Characters : BaseModule<Characters, MainWindow, SettingsModel>
	{
		public readonly ResourceManager RM = new ResourceManager("Kenedia.Modules.Characters.Res.strings", Assembly.GetExecutingAssembly());

		private readonly Ticks _ticks = new Ticks();

		private bool _saveCharacters;

		private CornerIcon _cornerIcon;

		private Character_Model _currentCharacterModel;

		public SearchFilterCollection SearchFilters { get; } = new SearchFilterCollection();


		public SearchFilterCollection TagFilters { get; } = new SearchFilterCollection();


		public TagList Tags { get; } = new TagList();


		public CharacterSwapping CharacterSwapping { get; private set; }

		public CharacterSorting CharacterSorting { get; private set; }

		public RunIndicator RunIndicator { get; private set; }

		public new SettingsWindow SettingsWindow { get; private set; }

		public RadialMenu RadialMenu { get; private set; }

		public PotraitCapture PotraitCapture { get; private set; }

		public LoadingSpinner APISpinner { get; private set; }

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
						_currentCharacterModel.Updated -= CurrentCharacterModel_Updated;
						_currentCharacterModel.IsCurrentCharacter = false;
					}
					_currentCharacterModel = value;
					if (_currentCharacterModel != null)
					{
						_currentCharacterModel.Updated += CurrentCharacterModel_Updated;
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

		public string CharactersPath { get; set; }

		public string AccountImagesPath { get; set; }

		public string AccountPath { get; set; }

		public GW2API_Handler GW2APIHandler { get; private set; }

		[ImportingConstructor]
		public Characters([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			BaseModule<Characters, MainWindow, SettingsModel>.ModuleInstance = this;
			HasGUI = true;
			Data = new Data(base.ContentsManager);
		}

		private void CurrentCharacterModel_Updated(object sender, EventArgs e)
		{
			base.MainWindow?.SortCharacters();
		}

		public void UpdateFolderPaths(string accountName, bool api_handled = true)
		{
			base.Paths.AccountName = accountName;
			base.Settings.LoadAccountSettings(accountName);
			string b = base.Paths.ModulePath;
			AccountPath = b + accountName;
			CharactersPath = b + accountName + "\\characters.json";
			AccountImagesPath = b + accountName + "\\images\\";
			if (!Directory.Exists(AccountPath))
			{
				Directory.CreateDirectory(AccountPath);
			}
			if (!Directory.Exists(AccountImagesPath))
			{
				Directory.CreateDirectory(AccountImagesPath);
			}
			if (api_handled && CharacterModels.Count == 0)
			{
				LoadCharacters();
			}
		}

		public bool LoadCharacters()
		{
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player == null || string.IsNullOrEmpty(player.get_Name()))
			{
				return false;
			}
			AccountSummary account = getAccount();
			if (account != null)
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Debug("Found '" + player.get_Name() + "' in a stored character list for '" + account.AccountName + "'. Loading characters of '" + account.AccountName + "'");
				UpdateFolderPaths(account.AccountName, api_handled: false);
				return LoadCharacterFile();
			}
			return false;
			AccountSummary getAccount()
			{
				try
				{
					string path = GlobalAccountsPath;
					if (File.Exists(path))
					{
						return JsonConvert.DeserializeObject<List<AccountSummary>>(File.ReadAllText(path)).Find((AccountSummary e) => e.CharacterNames.Contains(player.get_Name()));
					}
				}
				catch
				{
				}
				return null;
			}
		}

		public bool LoadCharacterFile()
		{
			try
			{
				if (File.Exists(CharactersPath))
				{
					new FileInfo(CharactersPath);
					string text = File.ReadAllText(CharactersPath);
					GameService.Gw2Mumble.get_PlayerCharacter();
					List<Character_Model> characters = JsonConvert.DeserializeObject<List<Character_Model>>(text);
					List<string> names = new List<string>();
					if (characters != null)
					{
						characters.ForEach(delegate(Character_Model c)
						{
							if (!CharacterModels.Contains(c) && !names.Contains(c.Name))
							{
								foreach (string current in c.Tags)
								{
									if (!Tags.Contains(current))
									{
										Tags.AddTag(current, fireEvent: false);
									}
								}
								CharacterModels.Add(new Character_Model(c, CharacterSwapping, base.Paths.ModulePath, RequestCharacterSave, CharacterModels, Data));
								names.Add(c.Name);
							}
						});
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Warn(ex, "Failed to load the local characters from file '" + CharactersPath + "'.");
				File.Copy(CharactersPath, CharactersPath.Replace(".json", " [" + DateTimeOffset.Now.ToUnixTimeSeconds() + "].corruped.json"));
				return false;
			}
		}

		public void SaveCharacterList()
		{
			string json = JsonConvert.SerializeObject((object)CharacterModels, (Formatting)1);
			File.WriteAllText(CharactersPath, json);
		}

		public void AddOrUpdateCharacters(IApiV2ObjectList<Character> characters)
		{
			BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info("Update characters based on fresh data from the api.");
			var freshList = ((IEnumerable<Character>)characters).Select((Character c) => new
			{
				Name = c.get_Name(),
				Created = c.get_Created()
			}).ToList();
			var oldList = CharacterModels.Select((Character_Model c) => new { c.Name, c.Created }).ToList();
			for (int i = 0; i < CharacterModels.Count; i++)
			{
				Character_Model c2 = CharacterModels[i];
				if (!freshList.Contains(new { c2.Name, c2.Created }))
				{
					BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info($"{c2.Name} created on {c2.Created} no longer exists. Delete it!");
					c2.Delete();
				}
			}
			int pos = 0;
			foreach (Character c3 in (IEnumerable<Character>)characters)
			{
				if (!oldList.Contains(new
				{
					Name = c3.get_Name(),
					Created = c3.get_Created()
				}))
				{
					BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info($"{c3.get_Name()} created on {c3.get_Created()} does not exist yet. Create it!");
					CharacterModels.Add(new Character_Model(c3, CharacterSwapping, base.Paths.ModulePath, RequestCharacterSave, CharacterModels, Data)
					{
						Position = pos
					});
				}
				else
				{
					Character_Model character = CharacterModels.FirstOrDefault((Character_Model e) => e.Name == c3.get_Name());
					character?.UpdateCharacter(c3);
					if (character != null)
					{
						character.Position = pos;
					}
				}
				pos++;
			}
			if (!File.Exists(CharactersPath) || base.Settings.ImportVersion.get_Value() < OldCharacterModel.ImportVersion)
			{
				string p = CharactersPath.Replace("kenedia\\", "");
				if (File.Exists(p))
				{
					BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info($"This is the first start of {((Module)this).get_Name()} since import version {OldCharacterModel.ImportVersion}. Importing old data from {p}!");
					OldCharacterModel.Import(p, CharacterModels, AccountImagesPath, base.Paths.AccountName, BaseModule<Characters, MainWindow, SettingsModel>.Logger, Tags);
				}
				base.Settings.ImportVersion.set_Value(OldCharacterModel.ImportVersion);
			}
			SaveCharacterList();
			base.MainWindow?.PerformFiltering();
		}

		protected override void Initialize()
		{
			base.Initialize();
			BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info("Starting " + ((Module)this).get_Name() + " v." + (object)base.ModuleVersion);
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
			GlobalAccountsPath = base.Paths.ModulePath + "\\accounts.json";
			if (!File.Exists(base.Paths.ModulePath + "\\gw2.traineddata") || base.Settings.Version.get_Value() != base.ModuleVersion)
			{
				using Stream destination = File.Create(base.Paths.ModulePath + "\\gw2.traineddata");
				Stream fileStream = base.ContentsManager.GetFileStream("data\\gw2.traineddata");
				fileStream.Seek(0L, SeekOrigin.Begin);
				fileStream.CopyTo(destination);
			}
			if (!File.Exists(base.Paths.ModulePath + "\\tesseract.dll") || base.Settings.Version.get_Value() != base.ModuleVersion)
			{
				using Stream target = File.Create(base.Paths.ModulePath + "\\tesseract.dll");
				Stream fileStream2 = base.ContentsManager.GetFileStream("data\\tesseract.dll");
				fileStream2.Seek(0L, SeekOrigin.Begin);
				fileStream2.CopyTo(target);
			}
			CreateToggleCategories();
			base.Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			base.Settings.ShortcutKey.get_Value().set_Enabled(true);
			base.Settings.ShortcutKey.get_Value().add_Activated((EventHandler<EventArgs>)ShortcutWindowToggle);
			base.Settings.RadialKey.get_Value().set_Enabled(true);
			base.Settings.RadialKey.get_Value().add_Activated((EventHandler<EventArgs>)RadialMenuToggle);
			Tags.CollectionChanged += Tags_CollectionChanged;
			base.Settings.Version.set_Value(base.ModuleVersion);
			GW2APIHandler = new GW2API_Handler(base.Gw2ApiManager, AddOrUpdateCharacters, () => (LoadingSpinner)(object)APISpinner, GlobalAccountsPath, UpdateFolderPaths);
		}

		private void RadialMenuToggle(object sender, EventArgs e)
		{
			if (base.Settings.EnableRadialMenu.get_Value() && (RadialMenu?.HasDisplayedCharacters() ?? false))
			{
				((Control)(object)RadialMenu)?.ToggleVisibility();
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			base.Settings = new SettingsModel(settings);
			base.Settings.ShowCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			_ticks.Global += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_ticks.APIUpdate += gameTime.get_ElapsedGameTime().TotalSeconds;
			_ticks.Save += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_ticks.Tags += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			_ticks.OCR += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_ticks.Global > 500.0)
			{
				_ticks.Global = 0.0;
				PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
				string name = ((player != null) ? player.get_Name() : string.Empty);
				bool charSelection = (base.Settings.UseBetaGamestate.get_Value() ? base.Services.GameState.IsCharacterSelection : (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame()));
				CurrentCharacterModel = ((!charSelection) ? CharacterModels.FirstOrDefault((Character_Model e) => e.Name == name) : null);
				if (CurrentCharacterModel != null)
				{
					CurrentCharacterModel?.UpdateCharacter(player);
				}
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

		public void RequestCharacterSave()
		{
			_saveCharacters = true;
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
			MouseState mouse = GameService.Input.get_Mouse().get_State();
			if (CharacterSwapping.Cancel())
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info(string.Format("Cancel any automated action. Left Mouse Down: {0} | Right Mouse Down: {1} | Keyboard Keys pressed {2}", (int)((MouseState)(ref mouse)).get_LeftButton() == 1, (int)((MouseState)(ref mouse)).get_RightButton() == 1, string.Join("|", (from k in GameService.Input.get_Keyboard().get_KeysDown()
					select ((object)(Keys)(ref k)).ToString()).ToArray())));
			}
			if (CharacterSorting.Cancel())
			{
				BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info(string.Format("Cancel any automated action. Left Mouse Down: {0} | Right Mouse Down: {1} | Keyboard Keys pressed {2}", (int)((MouseState)(ref mouse)).get_LeftButton() == 1, (int)((MouseState)(ref mouse)).get_RightButton() == 1, string.Join("|", (from k in GameService.Input.get_Keyboard().get_KeysDown()
					select ((object)(Keys)(ref k)).ToString()).ToArray())));
			}
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			CharacterSwapping = new CharacterSwapping(base.Settings, base.Services.GameState, CharacterModels);
			CharacterSorting = new CharacterSorting(base.Settings, base.Services.GameState, CharacterModels);
			CharacterSwapping.CharacterSorting = CharacterSorting;
			CharacterSorting.CharacterSwapping = CharacterSwapping;
			TextureManager = new TextureManager(base.Services.TexturesService);
			if (base.Settings.ShowCornerIcon.get_Value())
			{
				CreateCornerIcons();
			}
			PlayerCharacter playerCharacter = GameService.Gw2Mumble.get_PlayerCharacter();
			playerCharacter.add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)ForceUpdate);
			playerCharacter.add_NameChanged((EventHandler<ValueEventArgs<string>>)ForceUpdate);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)ForceUpdate);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)ForceUpdate);
			CharacterModels.CollectionChanged += OnCharacterCollectionChanged;
			if (base.Settings.LoadCachedAccounts.get_Value())
			{
				LoadCharacters();
			}
			base.Services.InputDetectionService.ClickedOrKey += InputDetectionService_ClickedOrKey;
		}

		private void InputDetectionService_ClickedOrKey(object sender, double e)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				CancelEverything();
			}
		}

		protected override void Unload()
		{
			TextureManager = null;
			DeleteCornerIcons();
			CharacterModels.CollectionChanged -= OnCharacterCollectionChanged;
			PlayerCharacter playerCharacter = GameService.Gw2Mumble.get_PlayerCharacter();
			playerCharacter.remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)ForceUpdate);
			playerCharacter.remove_NameChanged((EventHandler<ValueEventArgs<string>>)ForceUpdate);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)ForceUpdate);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)ForceUpdate);
			Tags.CollectionChanged -= Tags_CollectionChanged;
			base.Unload();
		}

		protected override void ReloadKey_Activated(object sender, EventArgs e)
		{
			base.ReloadKey_Activated(sender, e);
			((Control)GameService.Graphics.get_SpriteScreen()).set_Visible(true);
			MainWindow mainWindow = base.MainWindow;
			if (mainWindow != null)
			{
				((WindowBase2)mainWindow).ToggleWindow();
			}
			SettingsWindow settingsWindow = SettingsWindow;
			if (settingsWindow != null)
			{
				((WindowBase2)settingsWindow).ToggleWindow();
			}
		}

		private void OnCharacterCollectionChanged(object sender, EventArgs e)
		{
			base.MainWindow?.CreateCharacterControls(CharacterModels);
		}

		private void ShortcutWindowToggle(object sender, EventArgs e)
		{
			if (!(Control.get_ActiveControl() is TextBox))
			{
				MainWindow mainWindow = base.MainWindow;
				if (mainWindow != null)
				{
					((WindowBase2)mainWindow).ToggleWindow();
				}
			}
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			GW2APIHandler.CheckAPI();
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

		private void ForceUpdate(object sender, EventArgs e)
		{
			_ticks.Global = 2000000.0;
			if (CurrentCharacterModel != null)
			{
				CurrentCharacterModel.LastLogin = DateTime.UtcNow;
			}
			CurrentCharacterModel = null;
		}

		private void CreateCornerIcons()
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			DeleteCornerIcons();
			CornerIcon cornerIcon = new CornerIcon();
			((CornerIcon)cornerIcon).set_Icon(AsyncTexture2D.FromAssetId(156678));
			((CornerIcon)cornerIcon).set_HoverIcon(AsyncTexture2D.FromAssetId(156679));
			cornerIcon.SetLocalizedTooltip = () => string.Format(strings.Toggle, ((Module)this).get_Name() ?? "");
			((Control)cornerIcon).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)cornerIcon).set_Visible(base.Settings.ShowCornerIcon.get_Value());
			cornerIcon.ClickAction = delegate
			{
				MainWindow mainWindow = base.MainWindow;
				if (mainWindow != null)
				{
					((WindowBase2)mainWindow).ToggleWindow();
				}
			};
			_cornerIcon = cornerIcon;
			LoadingSpinner loadingSpinner = new LoadingSpinner();
			((Control)loadingSpinner).set_Location(new Point(((Control)_cornerIcon).get_Left(), ((Control)_cornerIcon).get_Bottom() + 3));
			((Control)loadingSpinner).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)loadingSpinner).set_Size(new Point(((Control)_cornerIcon).get_Width(), ((Control)_cornerIcon).get_Height()));
			((Control)loadingSpinner).set_BasicTooltipText(strings_common.FetchingApiData);
			((Control)loadingSpinner).set_Visible(false);
			APISpinner = loadingSpinner;
			((Control)_cornerIcon).add_Moved((EventHandler<MovedEventArgs>)CornerIcon_Moved);
		}

		private void DeleteCornerIcons()
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Moved((EventHandler<MovedEventArgs>)CornerIcon_Moved);
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			_cornerIcon = null;
			LoadingSpinner aPISpinner = APISpinner;
			if (aPISpinner != null)
			{
				((Control)aPISpinner).Dispose();
			}
			APISpinner = null;
		}

		private void CornerIcon_Moved(object sender, MovedEventArgs e)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (APISpinner != null)
			{
				((Control)APISpinner).set_Location(new Point(((Control)_cornerIcon).get_Left(), ((Control)_cornerIcon).get_Bottom() + 3));
			}
		}

		private void ShowCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				CreateCornerIcons();
			}
			else
			{
				DeleteCornerIcons();
			}
		}

		protected override void LoadGUI()
		{
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			base.LoadGUI();
			RadialMenu radialMenu = new RadialMenu(base.Settings, CharacterModels, (Container)(object)GameService.Graphics.get_SpriteScreen(), () => CurrentCharacterModel, Data, TextureManager);
			((Control)radialMenu).set_Visible(false);
			((Control)radialMenu).set_ZIndex(1073741823);
			RadialMenu = radialMenu;
			PotraitCapture potraitCapture = new PotraitCapture(base.Services.ClientWindowService, base.Services.SharedSettings, TextureManager);
			((Control)potraitCapture).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)potraitCapture).set_Visible(false);
			((Control)potraitCapture).set_ZIndex(2147483646);
			potraitCapture.AccountImagePath = () => AccountImagesPath;
			PotraitCapture = potraitCapture;
			OCR = new OCR(base.Services.ClientWindowService, base.Services.SharedSettings, base.Settings, base.Paths.ModulePath, CharacterModels);
			RunIndicator = new RunIndicator(CharacterSorting, CharacterSwapping, base.Settings.ShowStatusWindow, TextureManager, base.Settings.ShowChoyaSpinner);
			AsyncTexture2D settingsBg = AsyncTexture2D.FromAssetId(155997);
			Texture2D cutSettingsBg = Texture2DExtension.GetRegion(settingsBg.get_Texture(), 0, 0, settingsBg.get_Width() - 482, settingsBg.get_Height() - 390);
			SettingsWindow settingsWindow = new SettingsWindow(settingsBg, new Rectangle(30, 30, cutSettingsBg.get_Width() + 10, cutSettingsBg.get_Height()), new Rectangle(30, 35, cutSettingsBg.get_Width() - 5, cutSettingsBg.get_Height() - 15), base.SharedSettingsView, OCR, base.Settings);
			((Control)settingsWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)settingsWindow).set_Title("❤");
			((WindowBase2)settingsWindow).set_Subtitle("❤");
			((WindowBase2)settingsWindow).set_SavesPosition(true);
			((WindowBase2)settingsWindow).set_Id("CharactersSettingsWindow");
			settingsWindow.Version = base.ModuleVersion;
			SettingsWindow = settingsWindow;
			Texture2D bg = TextureManager.GetBackground(TextureManager.Backgrounds.MainWindow);
			Texture2D cutBg = Texture2DExtension.GetRegion(bg, 25, 25, bg.get_Width() - 100, bg.get_Height() - 325);
			MainWindow mainWindow = new MainWindow(bg, new Rectangle(25, 25, cutBg.get_Width() + 10, cutBg.get_Height()), new Rectangle(35, 14, cutBg.get_Width() - 10, cutBg.get_Height() - 10), base.Settings, TextureManager, CharacterModels, SearchFilters, TagFilters, OCR.ToggleContainer, delegate
			{
				((Control)(object)PotraitCapture).ToggleVisibility();
			}, async delegate
			{
				await GW2APIHandler.CheckAPI();
			}, () => AccountImagesPath, Tags, () => CurrentCharacterModel, Data, CharacterSorting);
			((Control)mainWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)mainWindow).set_Title("❤");
			((WindowBase2)mainWindow).set_Subtitle("❤");
			((WindowBase2)mainWindow).set_SavesPosition(true);
			((WindowBase2)mainWindow).set_Id("CharactersWindow");
			((WindowBase2)mainWindow).set_CanResize(true);
			((Control)mainWindow).set_Size(base.Settings.WindowSize.get_Value());
			mainWindow.SettingsWindow = SettingsWindow;
			mainWindow.Version = base.ModuleVersion;
			base.MainWindow = mainWindow;
			SideMenu sideMenu = base.MainWindow.SideMenu;
			SideMenuToggles sideMenuToggles = new SideMenuToggles(TextureManager, TagFilters, SearchFilters, delegate
			{
				base.MainWindow?.FilterCharacters();
			}, Tags, Data);
			((Control)sideMenuToggles).set_Width(((Control)base.MainWindow.SideMenu).get_Width());
			sideMenuToggles.Icon = AsyncTexture2D.FromAssetId(440021);
			SideMenuToggles _toggles = sideMenuToggles;
			sideMenu.AddTab(sideMenuToggles);
			base.MainWindow.SideMenu.AddTab(new SideMenuBehaviors(RM, TextureManager, base.Settings, delegate
			{
				base.MainWindow?.SortCharacters();
			})
			{
				Icon = AsyncTexture2D.FromAssetId(156909)
			});
			base.MainWindow.SideMenu.TogglesTab = _toggles;
			base.MainWindow.SideMenu.SwitchTab(_toggles);
			base.MainWindow?.CreateCharacterControls(CharacterModels);
			PotraitCapture.OnImageCaptured = delegate
			{
				base.MainWindow.CharacterEdit.LoadImages(null, null);
			};
			CharacterSwapping.HideMainWindow = ((Control)base.MainWindow).Hide;
			CharacterSwapping.OCR = OCR;
			CharacterSorting.OCR = OCR;
			CharacterSorting.UpdateCharacterList = base.MainWindow.PerformFiltering;
		}

		protected override void UnloadGUI()
		{
			base.UnloadGUI();
			RadialMenu radialMenu = RadialMenu;
			if (radialMenu != null)
			{
				((Control)radialMenu).Dispose();
			}
			SettingsWindow settingsWindow = SettingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			MainWindow mainWindow = base.MainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			PotraitCapture potraitCapture = PotraitCapture;
			if (potraitCapture != null)
			{
				((Control)potraitCapture).Dispose();
			}
			OCR?.Dispose();
			RunIndicator runIndicator = RunIndicator;
			if (runIndicator != null)
			{
				((Control)runIndicator).Dispose();
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(delegate
			{
				SettingsWindow settingsWindow = SettingsWindow;
				if (settingsWindow != null)
				{
					((WindowBase2)settingsWindow).ToggleWindow();
				}
			});
		}

		private void CreateToggleCategories()
		{
			new List<SearchFilter<Character_Model>>();
			foreach (KeyValuePair<ProfessionType, Data.Profession> e4 in Data.Professions)
			{
				SearchFilters.Add(e4.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.get_Value()["Profession"].Check && c.Profession == e4.Key));
				SearchFilters.Add("Core " + e4.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.get_Value()["Profession"].Check && c.Profession == e4.Key));
			}
			foreach (KeyValuePair<SpecializationType, Data.Specialization> e3 in Data.Specializations)
			{
				SearchFilters.Add(e3.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.get_Value()["Profession"].Check && c.Specialization == e3.Key));
			}
			foreach (KeyValuePair<int, Data.CraftingProfession> e2 in Data.CrafingProfessions)
			{
				SearchFilters.Add(e2.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => c.Crafting.Find((CharacterCrafting p) => base.Settings.DisplayToggles.get_Value()["CraftingProfession"].Check && p.Id == e2.Value.Id && (!base.Settings.DisplayToggles.get_Value()["OnlyMaxCrafting"].Check || p.Rating >= e2.Value.MaxRating)) != null));
			}
			foreach (KeyValuePair<RaceType, Data.Race> e in Data.Races)
			{
				SearchFilters.Add(e.Value.Name, new SearchFilter<Character_Model>((Character_Model c) => base.Settings.DisplayToggles.get_Value()["Race"].Check && c.Race == e.Key));
			}
			SearchFilters.Add("Birthday", new SearchFilter<Character_Model>((Character_Model c) => c.HasBirthdayPresent));
			SearchFilters.Add("Hidden", new SearchFilter<Character_Model>((Character_Model c) => !c.Show));
			SearchFilters.Add("Female", new SearchFilter<Character_Model>((Character_Model c) => (int)c.Gender == 2));
			SearchFilters.Add("Male", new SearchFilter<Character_Model>((Character_Model c) => (int)c.Gender == 1));
		}
	}
}
