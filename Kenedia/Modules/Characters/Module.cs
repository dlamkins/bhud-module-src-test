using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		private static class Last
		{
			public static double Tick_PanelUpdate;

			public static double Tick_Save;

			public static double Tick_Update;

			public static double Tick_APIUpdate;

			public static double Tick_FadeEffect;

			public static DateTime Tick_ImageSave;

			public static Character character { get; set; }

			public static string CharName { get; set; }

			public static int CharCount { get; set; }
		}

		private static class Current
		{
			public static Character character { get; set; }
		}

		private class Match
		{
			public List<ToggleIcon> toggleIcons = new List<ToggleIcon>();

			public bool match;

			public bool disabled
			{
				get
				{
					foreach (ToggleIcon toggleIcon in toggleIcons)
					{
						if (toggleIcon != null && toggleIcon._State == 1)
						{
							return false;
						}
					}
					return true;
				}
			}

			public bool isMatching
			{
				get
				{
					if (!disabled)
					{
						return match;
					}
					return true;
				}
			}
		}

		public localMapModel[] __Maps;

		public List<string> __ProfessionNames;

		public List<string> __SpecializationNames;

		public List<string> _ProfessionNames;

		public List<string> _SpecializationNames;

		internal static Module ModuleInstance;

		public static DateTime dateZero;

		public static DateTime lastLogout;

		private static bool requestAPI = true;

		public static bool filterCharacterPanel = true;

		public ContentService contentService = new ContentService();

		public static string CharactersPath;

		public static string AccountPath;

		public static string AccountImagesPath;

		public static string GlobalImagesPath;

		public static _Settings Settings = new _Settings();

		public static RECT GameWindow_Rectangle;

		public static FlowPanel CharacterPanel;

		public static FlowPanel filterPanel;

		public static TextBox filterTextBox;

		public static StandardButton clearButton;

		public static Image infoImage;

		public static StandardButton refreshAPI;

		public static CornerIcon cornerButton;

		public static List<ToggleIcon> filterProfessions = new List<ToggleIcon>();

		public static List<ToggleIcon> filterCrafting = new List<ToggleIcon>();

		public static List<ToggleIcon> filterRaces = new List<ToggleIcon>();

		public static List<ToggleIcon> filterSpecs = new List<ToggleIcon>();

		public static List<ToggleIcon> filterBaseSpecs = new List<ToggleIcon>();

		public static Label racesLabel;

		public static Label specializationLabel;

		public static Label customTagsLabel;

		public static FlowPanel filterTagsPanel;

		public static BasicContainer captureBox;

		public static bool charactersLoaded;

		public static bool saveCharacters;

		public static bool loginCharacter_Swapped;

		public static bool showAllCharacters;

		public static bool screenCapture;

		public static Character loginCharacter;

		public static Account API_Account;

		public static List<string> CharacterNames = new List<string>();

		public static List<Character> Characters = new List<Character>();

		public static List<string> Tags = new List<string>();

		public static List<TagEntry> TagEntries = new List<TagEntry>();

		public static readonly Logger Logger = Logger.GetLogger<Module>();

		private const int WINDOW_WIDTH = 385;

		private const int WINDOW_HEIGHT = 920;

		private const int TITLEBAR_HEIGHT = 33;

		public static VirtualKeyShort[] ModKeyMapping;

		public static Character swapCharacter;

		public localMapModel[] _Maps
		{
			get
			{
				return __Maps;
			}
			set
			{
				_Maps = value;
			}
		}

		public SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		public ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		public DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		public Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public static StandardWindow MainWidow { get; private set; }

		public static FilterWindow filterWindow { get; private set; }

		public static CharacterDetailWindow subWindow { get; private set; }

		public static ImageSelector ImageSelectorWindow { get; private set; }

		public static ScreenCaptureWindow screenCaptureWindow { get; private set; }

		public static AccountInfo userAccount { get; set; }

		public async Task _Fetch_ApiData()
		{
			await _Fetch_Maps();
			await _Fetch_Professions();
			await _Fetch_Specializations();
			await _Fetch_Races();
		}

		public async Task _Fetch_Maps()
		{
			IApiV2ObjectList<Map> maps = await Gw2ApiManager.get_Gw2ApiClient().V2.Maps.AllAsync();
			localMapModel[] mapArray = new localMapModel[maps[maps.Count - 1].Id + 1];
			string path = "C:\\maps.json";
			if (System.IO.File.Exists(path))
			{
				foreach (localMapModel map3 in JsonConvert.DeserializeObject<List<localMapModel>>(System.IO.File.ReadAllText(path))!)
				{
					mapArray[map3.Id] = map3;
				}
			}
			foreach (Map map2 in maps)
			{
				if (mapArray[map2.Id] == null)
				{
					mapArray[map2.Id] = new localMapModel
					{
						Id = map2.Id,
						Name = map2.Name,
						Floors = map2.Floors,
						DefaultFloor = map2.DefaultFloor,
						ContinentId = map2.ContinentId
					};
				}
				else
				{
					mapArray[map2.Id].Name = map2.Name;
				}
			}
			List<localMapModel> _data = new List<localMapModel>();
			localMapModel[] array = mapArray;
			foreach (localMapModel map in array)
			{
				if (map != null)
				{
					_data.Add(map);
				}
			}
			string json = JsonConvert.SerializeObject(_data.ToArray());
			System.IO.File.WriteAllText(path, json);
		}

		public async Task _Fetch_Professions()
		{
			IApiV2ObjectList<Profession> professions = await Gw2ApiManager.get_Gw2ApiClient().V2.Professions.AllAsync();
			_Professions[] resultArray = new _Professions[Enum.GetValues(typeof(Professions)).Cast<int>().Max() + 1];
			string path = "C:\\professions.json";
			if (System.IO.File.Exists(path))
			{
				foreach (_Professions profession3 in JsonConvert.DeserializeObject<List<_Professions>>(System.IO.File.ReadAllText(path))!)
				{
					resultArray[profession3.Id] = profession3;
				}
			}
			foreach (Profession profession2 in professions)
			{
				int id = (int)Enum.Parse(typeof(Professions), profession2.Id);
				if (resultArray[id] == null)
				{
					resultArray[id] = new _Professions
					{
						Id = id,
						Name = profession2.Name,
						API_Id = profession2.Id
					};
				}
				else
				{
					resultArray[id].Name = profession2.Name;
				}
			}
			List<_Professions> _data = new List<_Professions>();
			_Professions[] array = resultArray;
			foreach (_Professions profession in array)
			{
				if (profession != null)
				{
					_data.Add(profession);
				}
			}
			string json = JsonConvert.SerializeObject(_data.ToArray());
			System.IO.File.WriteAllText(path, json);
		}

		public async Task _Fetch_Specializations()
		{
			IApiV2ObjectList<Specialization> specs = await Gw2ApiManager.get_Gw2ApiClient().V2.Specializations.AllAsync();
			_Specializations[] resultArray = new _Specializations[Enum.GetValues(typeof(Specializations)).Cast<int>().Max() + 1];
			string path = "C:\\specialization.json";
			if (System.IO.File.Exists(path))
			{
				foreach (_Specializations spec3 in JsonConvert.DeserializeObject<List<_Specializations>>(System.IO.File.ReadAllText(path))!)
				{
					resultArray[spec3.Id] = spec3;
				}
			}
			foreach (Specialization spec2 in specs)
			{
				if (spec2.Elite)
				{
					int id = spec2.Id;
					if (resultArray[id] == null)
					{
						resultArray[id] = new _Specializations
						{
							Id = id,
							Name = spec2.Name,
							API_Id = spec2.Id
						};
					}
					else
					{
						resultArray[id].Name = spec2.Name;
					}
				}
			}
			List<_Specializations> _data = new List<_Specializations>();
			_Specializations[] array = resultArray;
			foreach (_Specializations spec in array)
			{
				if (spec != null)
				{
					_data.Add(spec);
				}
			}
			string json = JsonConvert.SerializeObject(_data.ToArray());
			System.IO.File.WriteAllText(path, json);
		}

		public async Task _Fetch_Races()
		{
			IApiV2ObjectList<Race> races = await Gw2ApiManager.get_Gw2ApiClient().V2.Races.AllAsync();
			_Race[] resultArray = new _Race[Enum.GetValues(typeof(RaceEnum)).Cast<int>().Max() + 1];
			string path = "C:\\races.json";
			if (System.IO.File.Exists(path))
			{
				foreach (_Race race3 in JsonConvert.DeserializeObject<List<_Race>>(System.IO.File.ReadAllText(path))!)
				{
					resultArray[race3.Id] = race3;
				}
			}
			foreach (Race race2 in races)
			{
				int id = (int)Enum.Parse(typeof(RaceEnum), race2.Id);
				if (resultArray[id] == null)
				{
					resultArray[id] = new _Race
					{
						Id = id,
						Name = race2.Name,
						API_Id = race2.Id
					};
				}
				else
				{
					resultArray[id].Name = race2.Name;
				}
			}
			List<_Race> _data = new List<_Race>();
			_Race[] array = resultArray;
			foreach (_Race race in array)
			{
				if (race != null)
				{
					_data.Add(race);
				}
			}
			string json = JsonConvert.SerializeObject(_data.ToArray());
			System.IO.File.WriteAllText(path, json);
		}

		public async Task<string> getMapNameAsync(int id)
		{
			localMapModel map = _Maps[id];
			if (map != null)
			{
				if (map.Name != null)
				{
					_ = map.Name != "";
					return map.Name;
				}
				return map.Name;
			}
			return "Unkown Map ID";
		}

		public async Task<string> getRace(string id)
		{
			Race result = await Gw2ApiManager.get_Gw2ApiClient().V2.Races.GetAsync(id);
			if (result != null)
			{
				return result.Name;
			}
			return "Unkown Race";
		}

		public async Task<string> getProfession(string id)
		{
			Profession result = await Gw2ApiManager.get_Gw2ApiClient().V2.Professions.GetAsync(id);
			if (result != null)
			{
				return result.Name;
			}
			return "Unkown Race";
		}

		public async Task<string> getSpecialization(int id)
		{
			Specialization result = await Gw2ApiManager.get_Gw2ApiClient().V2.Specializations.GetAsync(id);
			if (result != null)
			{
				return result.Name;
			}
			return "Unkown Race";
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Expected O, but got Unknown
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Expected O, but got Unknown
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Expected O, but got Unknown
			Settings.AutoLogin = settings.DefineSetting<bool>("AutoLogin", false, (Func<string>)(() => common.AutoLogin_DisplayName), (Func<string>)(() => common.AutoLogin_Description));
			Settings.EnterOnSwap = settings.DefineSetting<bool>("EnterOnSwap", false, (Func<string>)(() => common.EnterOnSwap_DisplayName), (Func<string>)(() => common.EnterOnSwap_Description));
			Settings.DoubleClickToEnter = settings.DefineSetting<bool>("DoubleClickToEnter", false, (Func<string>)(() => common.DoubleClickToEnter_DisplayName), (Func<string>)(() => common.DoubleClickToEnter_Description));
			Settings.EnterToLogin = settings.DefineSetting<bool>("EnterToLogin", false, (Func<string>)(() => common.EnterToLogin_DisplayName), (Func<string>)(() => common.EnterToLogin_Description));
			Settings.FadeSubWindows = settings.DefineSetting<bool>("FadeSubWindows", false, (Func<string>)(() => common.FadeOut_DisplayName), (Func<string>)(() => common.FadeOut_Description));
			Settings.OnlyMaxCrafting = settings.DefineSetting<bool>("OnlyMaxCrafting", true, (Func<string>)(() => common.OnlyMaxCrafting_DisplayName), (Func<string>)(() => common.OnlyMaxCrafting_Description));
			Settings.OnlyMaxCrafting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				foreach (Character current in Characters)
				{
					if (current.loaded && current.Crafting.Count > 0)
					{
						foreach (DataImage current2 in current.characterControl.crafting_Images)
						{
							((Control)current2).set_Visible(!Settings.OnlyMaxCrafting.get_Value() || (current2.Crafting.Id == 4 && current2.Crafting.Rating == 400) || current2.Crafting.Rating == 500);
						}
						((Control)current.characterControl.crafting_Panel).Invalidate();
					}
				}
			});
			Settings.FocusFilter = settings.DefineSetting<bool>("FocusFilter", false, (Func<string>)(() => common.FocusFilter_DisplayName), (Func<string>)(() => common.FocusFilter_Description));
			Settings.FilterDelay = settings.DefineSetting<int>("FilterDelay", 150, (Func<string>)(() => string.Format(common.FilterDelay_DisplayName, Settings.FilterDelay.get_Value())), (Func<string>)(() => common.FilterDelay_Description));
			SettingComplianceExtensions.SetRange(Settings.FilterDelay, 0, 500);
			Settings.FilterDelay.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
			{
				Settings._FilterDelay = Settings.FilterDelay.get_Value() / 2;
			});
			Settings._FilterDelay = Settings.FilterDelay.get_Value() / 2;
			Settings.SwapDelay = settings.DefineSetting<int>("SwapDelay", 500, (Func<string>)(() => string.Format(common.SwapDelay_DisplayName, Settings.SwapDelay.get_Value())), (Func<string>)(() => common.SwapDelay_Description));
			SettingComplianceExtensions.SetRange(Settings.SwapDelay, 0, 5000);
			Settings.LogoutKey = settings.DefineSetting<KeyBinding>("LogoutKey", new KeyBinding(Keys.F12), (Func<string>)(() => common.Logout), (Func<string>)(() => common.LogoutDescription));
			Settings.ShortcutKey = settings.DefineSetting<KeyBinding>("ShortcutKey", new KeyBinding((ModifierKeys)4, Keys.C), (Func<string>)(() => common.ShortcutToggle_DisplayName), (Func<string>)(() => common.ShortcutToggle_Description));
			Settings.SwapModifier = settings.DefineSetting<KeyBinding>("SwapModifier", new KeyBinding(Keys.None), (Func<string>)(() => common.SwapModifier_DisplayName), (Func<string>)(() => common.SwapModifier_Description));
		}

		public async void FetchAPI(bool force = false)
		{
			_ = 1;
			try
			{
				if (Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)new TokenPermission[2]
				{
					TokenPermission.Account,
					TokenPermission.Characters
				}) && API_Account != null && userAccount != null)
				{
					Account account = await Gw2ApiManager.get_Gw2ApiClient().V2.Account.GetAsync();
					userAccount.LastModified = account.LastModified;
					userAccount.Save();
					if (userAccount.CharacterUpdateNeeded() || force)
					{
						userAccount.LastBlishUpdate = ((userAccount.LastBlishUpdate > account.LastModified) ? userAccount.LastBlishUpdate : account.LastModified);
						userAccount.Save();
						IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Character> obj = await Gw2ApiManager.get_Gw2ApiClient().V2.Characters.AllAsync();
						Logger.Debug("Updating Characters ....");
						Character last = null;
						int i = 0;
						foreach (Gw2Sharp.WebApi.V2.Models.Character c in obj)
						{
							Character character = getCharacter(c.Name);
							character.Name = character.Name ?? c.Name;
							character.Race = (RaceType)Enum.Parse(typeof(RaceType), c.Race);
							character._Profession = (int)Enum.Parse(typeof(Professions), c.Profession.ToString());
							character.Profession = (ProfessionType)Enum.Parse(typeof(ProfessionType), c.Profession.ToString());
							character._Specialization = ((character._Specialization > -1) ? character._Specialization : (-1));
							character.Level = c.Level;
							character.Created = c.Created;
							character.contentsManager = ContentsManager;
							character.apiManager = Gw2ApiManager;
							character.Crafting = new List<CharacterCrafting>();
							foreach (CharacterCraftingDiscipline disc in c.Crafting.ToList())
							{
								character.Crafting.Add(new CharacterCrafting
								{
									Id = (int)disc.Discipline.Value,
									Rating = disc.Rating,
									Active = disc.Active
								});
							}
							character.apiIndex = i;
							if (character.LastModified == dateZero || character.LastModified < account.LastModified.UtcDateTime)
							{
								character.LastModified = account.LastModified.UtcDateTime.AddSeconds(-i);
							}
							if (character.lastLogin == dateZero)
							{
								character.lastLogin = c.LastModified.UtcDateTime;
							}
							last = character;
							i++;
						}
						last?.Save();
						filterCharacterPanel = true;
						Logger.Debug("Characters Updated!");
					}
					double lastModified = DateTimeOffset.UtcNow.Subtract(userAccount.LastModified).TotalSeconds;
					double lastUpdate = DateTimeOffset.UtcNow.Subtract(userAccount.LastBlishUpdate).TotalSeconds;
					((Control)infoImage).set_BasicTooltipText("Last Modified: " + Math.Round(lastModified) + Environment.NewLine + "Last Blish Login: " + Math.Round(lastUpdate));
				}
				else
				{
					ScreenNotification.ShowNotification(common.Error_Competivive, (NotificationType)2, (Texture2D)null, 4);
					Logger.Error("This API Token has not the required permissions!");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch characters from the API.");
			}
		}

		private async void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_ = 1;
			try
			{
				Logger.Debug("API Subtoken Updated!");
				if (Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)new TokenPermission[2]
				{
					TokenPermission.Account,
					TokenPermission.Characters
				}))
				{
					Account account = (API_Account = await Gw2ApiManager.get_Gw2ApiClient().V2.Account.GetAsync());
					string path = DirectoriesManager.GetFullDirectoryPath("characters") + "\\" + API_Account.Name;
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					CharactersPath = path + "\\characters.json";
					AccountPath = path + "\\account.json";
					AccountImagesPath = path + "\\images\\";
					if (!Directory.Exists(AccountImagesPath))
					{
						Directory.CreateDirectory(AccountImagesPath);
					}
					if (userAccount == null)
					{
						userAccount = new AccountInfo
						{
							Name = account.Name,
							LastModified = account.LastModified
						};
					}
					if (System.IO.File.Exists(AccountPath))
					{
						requestAPI = false;
						foreach (AccountInfo acc in JsonConvert.DeserializeObject<List<AccountInfo>>(System.IO.File.ReadAllText(AccountPath))!)
						{
							if (acc.Name == account.Name)
							{
								userAccount.LastBlishUpdate = acc.LastBlishUpdate;
								break;
							}
						}
					}
					LoadCharacterList();
					if (userAccount.CharacterUpdateNeeded())
					{
						userAccount.LastBlishUpdate = account.LastModified;
						userAccount.Save();
						Logger.Debug("Updating Characters ....");
						Logger.Debug("The last API modification is more recent than our last local data track.");
						IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Character> obj = await Gw2ApiManager.get_Gw2ApiClient().V2.Characters.AllAsync();
						Character last = null;
						int i = 0;
						foreach (Gw2Sharp.WebApi.V2.Models.Character c in obj)
						{
							Character character2 = getCharacter(c.Name);
							character2.Name = character2.Name ?? c.Name;
							character2.Race = (RaceType)Enum.Parse(typeof(RaceType), c.Race);
							character2._Profession = (int)Enum.Parse(typeof(Professions), c.Profession.ToString());
							character2.Profession = (ProfessionType)Enum.Parse(typeof(ProfessionType), c.Profession.ToString());
							character2._Specialization = ((character2._Specialization > -1) ? character2._Specialization : (-1));
							character2.Level = c.Level;
							character2.Created = c.Created;
							character2.apiIndex = i;
							if (character2.LastModified == dateZero || character2.LastModified < account.LastModified.UtcDateTime)
							{
								character2.LastModified = account.LastModified.UtcDateTime.AddSeconds(-i);
							}
							if (character2.lastLogin == dateZero)
							{
								character2.lastLogin = c.LastModified.UtcDateTime;
							}
							character2.contentsManager = ContentsManager;
							character2.apiManager = Gw2ApiManager;
							character2.Crafting = new List<CharacterCrafting>();
							foreach (CharacterCraftingDiscipline disc in c.Crafting.ToList())
							{
								character2.Crafting.Add(new CharacterCrafting
								{
									Id = (int)disc.Discipline.Value,
									Rating = disc.Rating,
									Active = disc.Active
								});
							}
							if (!CharacterNames.Contains(c.Name))
							{
								CharacterNames.Add(c.Name);
								Characters.Add(character2);
							}
							last = character2;
							i++;
						}
						last?.Save();
					}
					PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
					foreach (Character character in Characters)
					{
						character.Create_UI_Elements();
						if (player != null && player.get_Name() == character.Name)
						{
							Current.character = character;
						}
					}
					charactersLoaded = true;
					filterCharacterPanel = true;
					double lastModified = DateTimeOffset.UtcNow.Subtract(userAccount.LastModified).TotalSeconds;
					double lastUpdate = DateTimeOffset.UtcNow.Subtract(userAccount.LastBlishUpdate).TotalSeconds;
					((Control)infoImage).set_BasicTooltipText("Last Modified: " + Math.Round(lastModified) + Environment.NewLine + "Last Blish Login: " + Math.Round(lastUpdate));
				}
				else
				{
					ScreenNotification.ShowNotification(common.Error_InvalidPermissions, (NotificationType)2, (Texture2D)null, 4);
					Logger.Error("This API Token has not the required permissions!");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch characters from the API.");
			}
		}

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			ModuleInstance = this;
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		public static void ResetGameWindow()
		{
			MoveWindow(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), GameWindow_Rectangle.Left, GameWindow_Rectangle.Top, GameWindow_Rectangle.Right - GameWindow_Rectangle.Left, GameWindow_Rectangle.Bottom - GameWindow_Rectangle.Top, Repaint: false);
		}

		protected override void Initialize()
		{
			Logger.Debug("Initializing ...");
			ModKeyMapping = (VirtualKeyShort[])(object)new VirtualKeyShort[5];
			ModKeyMapping[1] = (VirtualKeyShort)17;
			ModKeyMapping[2] = (VirtualKeyShort)18;
			ModKeyMapping[4] = (VirtualKeyShort)160;
			LoadTextures();
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			AccountPath = DirectoriesManager.GetFullDirectoryPath("characters") + "\\accounts.json";
			GlobalImagesPath = DirectoriesManager.GetFullDirectoryPath("characters") + "\\images\\";
			LoadCustomImages();
			DataManager.ContentsManager = ContentsManager;
			DataManager.Load();
			Settings.ShortcutKey.get_Value().set_Enabled(true);
			Settings.ShortcutKey.get_Value().add_Activated((EventHandler<EventArgs>)OnKeyPressed_ToggleMenu);
			Settings.SwapModifier.get_Value().set_Enabled(true);
			Settings.SwapModifier.get_Value().add_Activated((EventHandler<EventArgs>)OnKeyPressed_LogoutMod);
			RECT pos = default(RECT);
			GetWindowRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), ref pos);
			GameWindow_Rectangle = pos;
			CreateFolders();
		}

		private void CreateFolders()
		{
			string basePath = DirectoriesManager.GetFullDirectoryPath("characters");
			foreach (string Path in new List<string> { "\\images" })
			{
				if (!Directory.Exists(basePath + Path))
				{
					Directory.CreateDirectory(basePath + Path);
				}
			}
		}

		private void LoadCustomImages()
		{
			if (!Directory.Exists(GlobalImagesPath))
			{
				return;
			}
			List<string> global_images = Directory.GetFiles(GlobalImagesPath, "*.png", SearchOption.AllDirectories).ToList();
			Textures.CustomImages = new Texture2D[global_images.Count + 100];
			Textures.CustomImageNames = new List<string>();
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				Logger.Debug("Loading all custom Images ... ");
				string fullDirectoryPath = DirectoriesManager.GetFullDirectoryPath("characters");
				int num = 0;
				foreach (string current in global_images)
				{
					Textures.CustomImages[num] = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
					Textures.CustomImages[num].Name = current.Replace(fullDirectoryPath, "");
					Textures.CustomImageNames.Add(Textures.CustomImages[num].Name);
					num++;
				}
				Textures.Loaded = true;
				if (ImageSelectorWindow != null)
				{
					ImageSelectorWindow.LoadImages();
				}
			});
		}

		private void LoadTextures()
		{
			Logger.Debug("Loading Textures....");
			Textures.Backgrounds = new Texture2D[Enum.GetValues(typeof(_Backgrounds)).Cast<int>().Max() + 1];
			foreach (_Backgrounds background in Enum.GetValues(typeof(_Backgrounds)))
			{
				Texture2D[] backgrounds = Textures.Backgrounds;
				ContentsManager contentsManager = ContentsManager;
				int num = (int)background;
				backgrounds[(int)background] = contentsManager.GetTexture("textures\\backgrounds\\" + num + ".png");
			}
			Textures.Emblems = new Texture2D[Enum.GetValues(typeof(_Emblems)).Cast<int>().Max() + 1];
			foreach (_Backgrounds emblem in Enum.GetValues(typeof(_Emblems)))
			{
				Texture2D[] emblems = Textures.Emblems;
				ContentsManager contentsManager2 = ContentsManager;
				int num = (int)emblem;
				emblems[(int)emblem] = contentsManager2.GetTexture("textures\\emblems\\" + num + ".png");
			}
			Textures.Icons = new Texture2D[Enum.GetValues(typeof(Icons)).Cast<int>().Max() + 1];
			foreach (Icons icon in Enum.GetValues(typeof(Icons)))
			{
				Texture2D[] icons = Textures.Icons;
				ContentsManager contentsManager3 = ContentsManager;
				int num = (int)icon;
				icons[(int)icon] = contentsManager3.GetTexture("textures\\icons\\" + num + ".png");
			}
			Textures.Races = new Texture2D[6];
			Textures.RacesDisabled = new Texture2D[6];
			foreach (RaceType race in Enum.GetValues(typeof(RaceType)))
			{
				Textures.Races[(uint)race] = ContentsManager.GetTexture("textures\\races\\" + race.ToString() + ".png");
				Textures.RacesDisabled[(uint)race] = ContentsManager.GetTexture("textures\\races gray\\" + race.ToString() + ".png");
			}
			Textures.Professions = new Texture2D[Enum.GetValues(typeof(Professions)).Cast<int>().Max() + 1];
			Textures.ProfessionsDisabled = new Texture2D[Enum.GetValues(typeof(Professions)).Cast<int>().Max() + 1];
			Textures.ProfessionsWhite = new Texture2D[Enum.GetValues(typeof(Professions)).Cast<int>().Max() + 1];
			foreach (Professions profession in Enum.GetValues(typeof(Professions)))
			{
				Texture2D[] professions = Textures.Professions;
				ContentsManager contentsManager4 = ContentsManager;
				int num = (int)profession;
				professions[(int)profession] = contentsManager4.GetTexture("textures\\professions\\" + num + ".png");
				Texture2D[] professionsDisabled = Textures.ProfessionsDisabled;
				ContentsManager contentsManager5 = ContentsManager;
				num = (int)profession;
				professionsDisabled[(int)profession] = contentsManager5.GetTexture("textures\\professions gray\\" + num + ".png");
				Texture2D[] professionsWhite = Textures.ProfessionsWhite;
				ContentsManager contentsManager6 = ContentsManager;
				num = (int)profession;
				professionsWhite[(int)profession] = contentsManager6.GetTexture("textures\\professions white\\" + num + ".png");
			}
			Textures.Specializations = new Texture2D[Enum.GetValues(typeof(Specializations)).Cast<int>().Max() + 1];
			Textures.SpecializationsDisabled = new Texture2D[Enum.GetValues(typeof(Specializations)).Cast<int>().Max() + 1];
			Textures.SpecializationsWhite = new Texture2D[Enum.GetValues(typeof(Specializations)).Cast<int>().Max() + 1];
			foreach (Specializations specialization in Enum.GetValues(typeof(Specializations)))
			{
				Texture2D[] specializations = Textures.Specializations;
				ContentsManager contentsManager7 = ContentsManager;
				int num = (int)specialization;
				specializations[(int)specialization] = contentsManager7.GetTexture("textures\\specializations\\" + num + ".png");
				Texture2D[] specializationsDisabled = Textures.SpecializationsDisabled;
				ContentsManager contentsManager8 = ContentsManager;
				num = (int)specialization;
				specializationsDisabled[(int)specialization] = contentsManager8.GetTexture("textures\\specializations gray\\" + num + ".png");
				Texture2D[] specializationsWhite = Textures.SpecializationsWhite;
				ContentsManager contentsManager9 = ContentsManager;
				num = (int)specialization;
				specializationsWhite[(int)specialization] = contentsManager9.GetTexture("textures\\specializations white\\" + num + ".png");
			}
			Textures.Crafting = new Texture2D[Enum.GetValues(typeof(Crafting)).Cast<int>().Max() + 1];
			Textures.CraftingDisabled = new Texture2D[Enum.GetValues(typeof(Crafting)).Cast<int>().Max() + 1];
			foreach (Crafting crafting in Enum.GetValues(typeof(Crafting)))
			{
				Texture2D[] crafting2 = Textures.Crafting;
				ContentsManager contentsManager10 = ContentsManager;
				int num = (int)crafting;
				crafting2[(int)crafting] = contentsManager10.GetTexture("textures\\crafting\\" + num + ".png");
				Texture2D[] craftingDisabled = Textures.CraftingDisabled;
				ContentsManager contentsManager11 = ContentsManager;
				num = (int)crafting;
				craftingDisabled[(int)crafting] = contentsManager11.GetTexture("textures\\crafting gray\\" + num + ".png");
			}
		}

		private void Load_UserLocale()
		{
			ImageSelectorWindow.UpdateLanguage();
			if (screenCaptureWindow != null)
			{
				screenCaptureWindow.UpdateLanguage();
			}
			foreach (Character c in Characters)
			{
				CharacterTooltip obj = (CharacterTooltip)(object)((Control)c.characterControl).get_Tooltip();
				obj._Map.Text = DataManager.getMapName(c.Map);
				obj._Race.Text = DataManager.getRaceName(c.Race.ToString());
				obj._switchInfoLabel.set_Text(string.Format(common.DoubleClickToSwap, c.Name));
			}
			((TextInputBase)filterTextBox).set_PlaceholderText(common.SearchFor);
			((Control)filterTextBox).set_BasicTooltipText(common.SearchGuide);
			clearButton.set_Text(common.Clear);
			subWindow.loginCharacter.set_Text(common.LoginCharacter);
			filterWindow.CustomTags.Text = common.CustomTags;
			filterWindow.Utility.Text = common.Utility;
			filterWindow.Crafting.Text = common.CraftingProfession;
			filterWindow.Profession.Text = common.Profession;
			filterWindow.Specialization.Text = common.Specialization;
			((Control)filterWindow.visibleToggle).set_BasicTooltipText(common.ToggleVisible);
			((Control)filterWindow.birthdayToggle).set_BasicTooltipText(common.Birthday);
			filterWindow.toggleSpecsButton.set_Text(common.ToggleAll);
			foreach (Character character in Characters)
			{
				character.UpdateLanguage();
			}
			foreach (ToggleIcon toggle5 in filterProfessions)
			{
				if (toggle5 != null)
				{
					((Control)toggle5).set_BasicTooltipText(DataManager.getProfessionName(toggle5.Id));
				}
			}
			foreach (ToggleIcon toggle4 in filterBaseSpecs)
			{
				if (toggle4 != null)
				{
					((Control)toggle4).set_BasicTooltipText(DataManager.getProfessionName(toggle4.Id));
				}
			}
			foreach (ToggleIcon toggle3 in filterCrafting)
			{
				if (toggle3 != null)
				{
					((Control)toggle3).set_BasicTooltipText(DataManager.getCraftingName(toggle3.Id));
					if (toggle3.Id == 0)
					{
						((Control)toggle3).set_BasicTooltipText(common.NoCraftingProfession);
					}
				}
			}
			foreach (ToggleIcon toggle2 in filterSpecs)
			{
				if (toggle2 != null)
				{
					((Control)toggle2).set_BasicTooltipText(DataManager.getSpecName(toggle2.Id));
				}
			}
			foreach (ToggleIcon toggle in filterRaces)
			{
				if (toggle != null)
				{
					((Control)toggle).set_BasicTooltipText(DataManager.getRaceName(toggle.Id));
				}
			}
		}

		private bool LoadCharacterList()
		{
			try
			{
				if (System.IO.File.Exists(CharactersPath))
				{
					requestAPI = false;
					List<JsonCharacter> characters = JsonConvert.DeserializeObject<List<JsonCharacter>>(System.IO.File.ReadAllText(CharactersPath));
					if (characters != null)
					{
						foreach (JsonCharacter c in characters)
						{
							Character character2 = new Character();
							character2.contentsManager = ContentsManager;
							character2.apiManager = Gw2ApiManager;
							character2.Race = c.Race;
							character2.Name = c.Name;
							character2.lastLogin = c.lastLogin;
							character2._Profession = c.Profession;
							character2._Specialization = c.Specialization;
							character2.Crafting = c.Crafting;
							character2.apiIndex = c.apiIndex;
							character2.Created = c.Created;
							character2.LastModified = c.LastModified;
							character2.Map = c.Map;
							character2.Level = c.Level;
							character2.Tags = ((c.Tags != null && c.Tags != "") ? c.Tags.Split('|').ToList() : new List<string>());
							character2.loginCharacter = c.loginCharacter;
							character2.include = c.include;
							character2.Icon = c.Icon;
							Character character = character2;
							if (CharacterNames.Contains(character.Name))
							{
								continue;
							}
							foreach (string tag in character.Tags)
							{
								if (!Tags.Contains(tag))
								{
									Tags.Add(tag);
								}
							}
							Characters.Add(character);
							CharacterNames.Add(character.Name);
							if (c.loginCharacter)
							{
								loginCharacter = character;
							}
						}
						foreach (string txt in Tags)
						{
							TagEntry entry = new TagEntry(txt, new Character(), filterTagsPanel, showButton: false, contentService.GetFont((FontFace)0, (FontSize)14, (FontStyle)0));
							((Control)entry).add_Click((EventHandler<MouseEventArgs>)delegate
							{
								if (((TextInputBase)filterTextBox).get_Text().ToLower().Contains(txt.ToLower()))
								{
									((TextInputBase)filterTextBox).get_Text();
									txt.Replace("; -t " + txt + ";", "");
									txt.Replace("; -t " + txt, "");
									txt.Replace("-t " + txt + ";", "");
									txt.Replace("-t " + txt, "");
									((TextInputBase)filterTextBox).set_Text(txt.Trim());
								}
								else
								{
									TextBox obj = filterTextBox;
									((TextInputBase)obj).set_Text(((TextInputBase)obj).get_Text() + (((((TextInputBase)filterTextBox).get_Text().Trim().EndsWith(";") || ((TextInputBase)filterTextBox).get_Text().Trim() == "") ? " " : "; ") + "-t " + txt).Trim());
								}
							});
							TagEntries.Add(entry);
						}
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load the local characters from file '" + CharactersPath + "'.");
				return false;
			}
		}

		public void SaveCharacters()
		{
			if (API_Account == null)
			{
				return;
			}
			List<JsonCharacter> _data = new List<JsonCharacter>();
			foreach (Character c in Characters)
			{
				JsonCharacter jsonCharacter = new JsonCharacter
				{
					Name = c.Name,
					Race = c.Race,
					Specialization = c._Specialization,
					Profession = c._Profession,
					Crafting = c.Crafting,
					lastLogin = c.lastLogin,
					apiIndex = c.apiIndex,
					Created = c.Created,
					LastModified = c.LastModified,
					Map = c.Map,
					Level = c.Level,
					Tags = string.Join("|", c.Tags),
					loginCharacter = c.loginCharacter,
					include = c.include,
					Icon = c.Icon
				};
				_data.Add(jsonCharacter);
			}
			string json = JsonConvert.SerializeObject(_data.ToArray());
			System.IO.File.WriteAllText(CharactersPath, json);
		}

		protected override async Task LoadAsync()
		{
			CornerIcon val = new CornerIcon();
			val.set_IconName("Characters");
			val.set_Icon(AsyncTexture2D.op_Implicit(Textures.Icons[22]));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(Textures.Icons[23]));
			val.set_Priority(4);
			cornerButton = val;
			((Control)cornerButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)MainWidow).ToggleWindow();
			});
		}

		private Character getCharacter(string name)
		{
			foreach (Character c in Characters)
			{
				if (c.Name == name)
				{
					return c;
				}
			}
			return new Character();
		}

		private void UpdateCharacterPanel()
		{
			if (!charactersLoaded)
			{
				return;
			}
			Characters.Sort((Character a, Character b) => b.LastModified.CompareTo(a.LastModified));
			string txt = ((TextInputBase)filterTextBox).get_Text().ToLower();
			string[] array = txt.Split(';');
			List<string> mapFilters = new List<string>();
			List<string> craftingFilters = new List<string>();
			List<string> professionFilters = new List<string>();
			List<string> birthdayFilters = new List<string>();
			List<string> raceFilters = new List<string>();
			List<string> nameFilters = new List<string>();
			List<string> tagFilters = new List<string>();
			List<string> filterParts = new List<string>();
			string[] array2 = array;
			foreach (string part in array2)
			{
				filterParts.Add(part.Trim().ToLower());
			}
			foreach (string part2 in filterParts)
			{
				if (!(part2 != ""))
				{
					continue;
				}
				string s6 = part2;
				if (s6 != null)
				{
					if (s6.StartsWith("-c "))
					{
						craftingFilters.Add(s6.ToLower().Replace("-c ", "").Trim());
						continue;
					}
					string s5 = s6;
					if (s5.StartsWith("-p "))
					{
						professionFilters.Add(s5.ToLower().Replace("-p ", "").Trim());
						continue;
					}
					string s4 = s6;
					if (s4.StartsWith("-t "))
					{
						tagFilters.Add(s4.ToLower().Replace("-t ", "").Trim());
						continue;
					}
					string s3 = s6;
					if (s3.StartsWith("-m "))
					{
						mapFilters.Add(s3.ToLower().Replace("-m ", "").Trim());
						continue;
					}
					string s2 = s6;
					if (s2.StartsWith("-r "))
					{
						raceFilters.Add(s2.ToLower().Replace("-r ", "").Trim());
						continue;
					}
					string s = s6;
					if (s.StartsWith("-b"))
					{
						birthdayFilters.Add(s.ToLower().Replace("-b ", "").Trim());
						continue;
					}
				}
				nameFilters.Add(part2.ToLower().Trim());
			}
			foreach (Character character in Characters)
			{
				if ((character.include || showAllCharacters) && matchingFilterString(character) && matchingToggles(character))
				{
					character.Show();
				}
				else
				{
					character.Hide();
				}
			}
			if (CharacterPanel != null)
			{
				CharacterPanel.SortChildren<CharacterControl>((Comparison<CharacterControl>)((CharacterControl a, CharacterControl b) => b.assignedCharacter.LastModified.CompareTo(a.assignedCharacter.LastModified)));
			}
			bool matchingFilterString(Character c)
			{
				if (txt == "")
				{
					return true;
				}
				foreach (string s12 in craftingFilters)
				{
					foreach (CharacterCrafting crafting2 in c.Crafting)
					{
						if (crafting2.Active && DataManager.getCraftingName(crafting2.Id).ToLower().Contains(s12))
						{
							return true;
						}
					}
				}
				foreach (string s11 in tagFilters)
				{
					foreach (string tag in c.Tags)
					{
						if (tag.ToLower().Contains(s11))
						{
							return true;
						}
					}
				}
				foreach (string s10 in professionFilters)
				{
					string professionName = DataManager.getProfessionName(c._Profession);
					string specName = DataManager.getSpecName(c._Specialization);
					if (professionName.ToLower().Contains(s10))
					{
						return true;
					}
					if (c._Specialization > 0 && specName.ToLower().Contains(s10))
					{
						return true;
					}
				}
				foreach (string item in birthdayFilters)
				{
					_ = item;
					if (((Control)c.characterControl.birthday_Image).get_Visible())
					{
						return true;
					}
				}
				foreach (string s9 in raceFilters)
				{
					if (DataManager.getRaceName(c.Race.ToString()).ToString().ToLower()
						.Contains(s9))
					{
						return true;
					}
				}
				foreach (string s8 in mapFilters)
				{
					string map = DataManager.getMapName(c.Map);
					if (map != null && map.ToLower().Contains(s8))
					{
						return true;
					}
				}
				foreach (string s7 in nameFilters)
				{
					if (c.Name.ToLower().Contains(s7))
					{
						return true;
					}
				}
				return false;
			}
			static bool matchingToggles(Character c)
			{
				Match professionMatch = new Match
				{
					toggleIcons = filterProfessions
				};
				Match craftingMatch = new Match
				{
					toggleIcons = filterCrafting
				};
				Match raceMatch = new Match
				{
					toggleIcons = filterRaces
				};
				Match specMatch = new Match
				{
					toggleIcons = filterSpecs
				};
				specMatch.toggleIcons.AddRange(filterBaseSpecs);
				Match birthdayMatch = new Match
				{
					toggleIcons = { filterWindow.birthdayToggle }
				};
				foreach (ToggleIcon toggle5 in filterProfessions)
				{
					if (toggle5 != null && toggle5._State == 1 && toggle5 != null && toggle5._State == 1 && toggle5.Id == c._Profession)
					{
						professionMatch.match = true;
					}
				}
				foreach (ToggleIcon toggle4 in filterSpecs)
				{
					if (toggle4 != null && toggle4._State == 1 && toggle4.Id == c._Specialization)
					{
						specMatch.match = true;
					}
				}
				foreach (ToggleIcon toggle3 in filterBaseSpecs)
				{
					if (toggle3 != null && toggle3._State == 1 && c._Specialization == 0 && toggle3.Id == c._Profession)
					{
						specMatch.match = true;
					}
				}
				foreach (ToggleIcon toggle2 in filterRaces)
				{
					if (toggle2 != null && toggle2._State == 1 && toggle2.Id == (int)c.Race)
					{
						raceMatch.match = true;
					}
				}
				foreach (ToggleIcon toggle in filterCrafting)
				{
					if (toggle != null && toggle._State == 1 && toggle.Id == 0 && c.Crafting.Count == 0)
					{
						craftingMatch.match = true;
						break;
					}
					if (toggle != null && toggle._State == 1)
					{
						foreach (CharacterCrafting crafting in c.Crafting)
						{
							if (crafting.Active && toggle.Id == crafting.Id && (!Settings.OnlyMaxCrafting.get_Value() || crafting.Rating == 500 || ((crafting.Id == 4 || crafting.Id == 7) && crafting.Rating == 400)))
							{
								craftingMatch.match = true;
							}
						}
					}
				}
				switch (filterWindow.birthdayToggle._State)
				{
				case 0:
					birthdayMatch.match = true;
					break;
				case 1:
					birthdayMatch.match = ((Control)c.characterControl.birthday_Image).get_Visible();
					break;
				case 2:
					birthdayMatch.match = !((Control)c.characterControl.birthday_Image).get_Visible();
					break;
				}
				if (craftingMatch.isMatching && professionMatch.isMatching && birthdayMatch.isMatching && raceMatch.isMatching)
				{
					return specMatch.isMatching;
				}
				return false;
			}
		}

		private void Update_CurrentCharacter()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && charactersLoaded)
			{
				PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
				Last.character = Current.character;
				Current.character = getCharacter(player.get_Name());
				Current.character.UpdateCharacter();
				if (Last.character != Current.character && userAccount != null)
				{
					userAccount.LastBlishUpdate = DateTimeOffset.UtcNow;
					userAccount.Save();
					filterCharacterPanel = true;
					Current.character.Save();
				}
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
			CreateWindow();
			CreateFilterWindow();
			CreateSubWindow();
			CreateImageSelector();
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				if (GameService.Graphics.get_Resolution().X == 1084 && GameService.Graphics.get_Resolution().Y == 761)
				{
					((Control)screenCaptureWindow).Dispose();
					CreateScreenCapture();
				}
			});
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			player.add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)delegate
			{
				if (Current.character != null)
				{
					Current.character.UpdateProfession();
					filterCharacterPanel = true;
				}
			});
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)delegate
			{
				Load_UserLocale();
			});
			Load_UserLocale();
			if (Settings.AutoLogin.get_Value() && (player == null || player.get_Name() == ""))
			{
				Keyboard.Stroke((VirtualKeyShort)13, false);
			}
		}

		private void ResetFilters()
		{
			foreach (List<ToggleIcon> item in new List<List<ToggleIcon>> { filterCrafting, filterProfessions, filterSpecs, filterRaces, filterBaseSpecs })
			{
				foreach (ToggleIcon toggle in item)
				{
					if (toggle != null)
					{
						toggle._State = 0;
					}
				}
			}
			filterWindow.birthdayToggle._State = 0;
			((TextInputBase)filterTextBox).set_Text((string)null);
			filterCharacterPanel = true;
		}

		private void CreateWindow()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Expected O, but got Unknown
			StandardWindow val = new StandardWindow(Textures.Backgrounds[1], new Microsoft.Xna.Framework.Rectangle(15, 45, 365, 920), new Microsoft.Xna.Framework.Rectangle(10, 15, 385, 920));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Characters");
			((WindowBase2)val).set_Emblem(Textures.Emblems[1]);
			((WindowBase2)val).set_Subtitle("❤");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("CharactersWindow");
			MainWidow = val;
			((Control)MainWidow).add_Hidden((EventHandler<EventArgs>)delegate
			{
				((Control)subWindow).Hide();
				((Control)filterWindow).Hide();
				((Control)ImageSelectorWindow).Hide();
			});
			((Control)MainWidow).add_Shown((EventHandler<EventArgs>)delegate
			{
				((Control)subWindow).Hide();
				((Control)filterWindow).Hide();
				((Control)ImageSelectorWindow).Hide();
				if (Settings.FocusFilter.get_Value())
				{
					Control.set_ActiveControl((Control)(object)filterTextBox);
					((TextInputBase)filterTextBox).set_Focused(true);
				}
			});
			Image val2 = new Image();
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[2]));
			((Control)val2).set_Size(new Point(28, 28));
			((Control)val2).set_Location(new Point(((Control)MainWidow).get_Width() - 25, -5));
			((Control)val2).set_Parent((Container)(object)MainWidow);
			((Control)val2).set_Visible(false);
			infoImage = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Size(new Point(185, 25));
			((Control)val3).set_Location(new Point(175, 0));
			((Control)val3).set_Parent((Container)(object)MainWidow);
			val3.set_Text("Refresh API Data");
			((Control)val3).set_Visible(false);
			refreshAPI = val3;
			((Control)refreshAPI).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				FetchAPI(force: true);
			});
			TextBox val4 = new TextBox();
			((TextInputBase)val4).set_PlaceholderText("Search for ...");
			((Control)val4).set_Size(new Point(287, 30));
			((TextInputBase)val4).set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Location(new Point(5, 20));
			((Control)val4).set_Parent((Container)(object)MainWidow);
			((Control)val4).set_BasicTooltipText("'-c CraftingDiscipline'" + Environment.NewLine + "'-p Profession/Specialization'" + Environment.NewLine + "'-r Race'" + Environment.NewLine + "'-b(irthday)'" + Environment.NewLine + "'-c Chef; -p Warrior' will show all warriors and all chefs");
			filterTextBox = val4;
			new Tooltip();
			((TextInputBase)filterTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				filterCharacterPanel = true;
			});
			filterTextBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				if (Settings.EnterToLogin.get_Value())
				{
					foreach (CharacterControl characterControl in ((Container)CharacterPanel).get_Children())
					{
						if (((Control)characterControl).get_Visible())
						{
							characterControl.assignedCharacter.Swap();
							break;
						}
					}
				}
			});
			((Control)filterTextBox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)filterWindow).get_Visible())
				{
					((Control)filterWindow).Hide();
				}
				else
				{
					((Control)filterWindow).Show();
				}
			});
			StandardButton val5 = new StandardButton();
			val5.set_Text("Clear");
			((Control)val5).set_Location(new Point(292, 19));
			((Control)val5).set_Size(new Point(73, 32));
			((Control)val5).set_Parent((Container)(object)MainWidow);
			val5.set_ResizeIcon(true);
			clearButton = val5;
			((Control)clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ResetFilters();
			});
			FlowPanel val6 = new FlowPanel();
			((Panel)val6).set_CanScroll(true);
			((Panel)val6).set_ShowBorder(true);
			((Control)val6).set_Parent((Container)(object)MainWidow);
			((Control)val6).set_Width(((Control)MainWidow).get_Width());
			((Control)val6).set_Height(((Control)MainWidow).get_Height() - (((Control)clearButton).get_Location().Y + ((Control)clearButton).get_Height() + 5 + 50));
			((Control)val6).set_Location(new Point(0, ((Control)clearButton).get_Location().Y + ((Control)clearButton).get_Height() + 5));
			val6.set_FlowDirection((ControlFlowDirection)3);
			CharacterPanel = val6;
		}

		private void CreateFilterWindow()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_080a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0816: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_083c: Unknown result type (might be due to invalid IL or missing references)
			//IL_084b: Expected O, but got Unknown
			new ContentService();
			Specializations[] specs = new Specializations[27]
			{
				Specializations.Dragonhunter,
				Specializations.Berserker,
				Specializations.Scrapper,
				Specializations.Druid,
				Specializations.Daredevil,
				Specializations.Tempest,
				Specializations.Chronomancer,
				Specializations.Reaper,
				Specializations.Herald,
				Specializations.Firebrand,
				Specializations.Spellbreaker,
				Specializations.Holosmith,
				Specializations.Soulbeast,
				Specializations.Deadeye,
				Specializations.Weaver,
				Specializations.Mirage,
				Specializations.Scourge,
				Specializations.Renegade,
				Specializations.Willbender,
				Specializations.Bladesworn,
				Specializations.Mechanist,
				Specializations.Untamed,
				Specializations.Specter,
				Specializations.Catalyst,
				Specializations.Virtuoso,
				Specializations.Harbinger,
				Specializations.Vindicator
			};
			Point windowPadding = new Point(5, 5);
			FilterWindow obj = new FilterWindow();
			((Control)obj).set_Height(500);
			((Container)obj).set_HeightSizingMode((SizingMode)1);
			((Control)obj).set_Width(300);
			((Control)obj).set_Location(new Point(((Control)MainWidow).get_Location().X + 385 - 25, ((Control)MainWidow).get_Location().Y + 60));
			((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			obj.Texture = Textures.Backgrounds[3];
			((Container)obj).set_AutoSizePadding(new Point(windowPadding.X, windowPadding.Y));
			filterWindow = obj;
			((Control)filterWindow).add_Shown((EventHandler<EventArgs>)delegate
			{
				((Control)subWindow).Hide();
			});
			((Control)MainWidow).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				((Control)filterWindow).set_Location(new Point(((Control)MainWidow).get_Location().X + 385 - 25, ((Control)MainWidow).get_Location().Y + 60));
			});
			FlowPanel val = new FlowPanel();
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Parent((Container)(object)filterWindow);
			val.set_ControlPadding(new Vector2(2f, 5f));
			FlowPanel mainPanel = val;
			Image val2 = new Image();
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[35]));
			((Control)val2).set_Parent((Container)(object)filterWindow);
			((Control)val2).set_Location(new Point(((Control)filterWindow).get_Width() - 23, 2));
			((Control)val2).set_Size(new Point(21, 23));
			Image closeButton = val2;
			((Control)closeButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				closeButton.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[36]));
			});
			((Control)closeButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				closeButton.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[35]));
			});
			((Control)closeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)filterWindow).Hide();
			});
			FilterWindow obj2 = filterWindow;
			HeadedFlowRegion headedFlowRegion = new HeadedFlowRegion();
			((Container)headedFlowRegion).set_WidthSizingMode((SizingMode)2);
			headedFlowRegion.Text = "Utility";
			((Control)headedFlowRegion).set_Width(((Control)filterWindow).get_Width() - windowPadding.X * 2);
			((Control)headedFlowRegion).set_Parent((Container)(object)mainPanel);
			obj2.Utility = headedFlowRegion;
			FlowPanel region = filterWindow.Utility.contentFlowPanel;
			region.set_OuterControlPadding(new Vector2(0f, 0f));
			FilterWindow obj3 = filterWindow;
			ToggleIcon toggleIcon = new ToggleIcon();
			((Image)toggleIcon).set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[43]));
			((Control)toggleIcon).set_Parent((Container)(object)region);
			toggleIcon._State = 0;
			toggleIcon._MaxState = 2;
			toggleIcon._Textures.Add(Textures.Icons[43]);
			toggleIcon._Textures.Add(Textures.Icons[42]);
			obj3.visibleToggle = toggleIcon;
			((Control)filterWindow.visibleToggle).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				showAllCharacters = filterWindow.visibleToggle._State == 1;
			});
			FilterWindow obj4 = filterWindow;
			ToggleIcon toggleIcon2 = new ToggleIcon();
			((Control)toggleIcon2).set_Parent((Container)(object)region);
			toggleIcon2._Textures.Add(Textures.Icons[24]);
			toggleIcon2._Textures.Add(Textures.Icons[17]);
			toggleIcon2._Textures.Add(Textures.Icons[25]);
			toggleIcon2._MaxState = 3;
			toggleIcon2._State = 0;
			obj4.birthdayToggle = toggleIcon2;
			foreach (RaceType index5 in Enum.GetValues(typeof(RaceType)))
			{
				List<ToggleIcon> list = filterRaces;
				ToggleIcon obj5 = new ToggleIcon
				{
					_Textures = 
					{
						Textures.RacesDisabled[(uint)index5],
						Textures.Races[(uint)index5]
					},
					_State = 0,
					_MaxState = 2
				};
				((Control)obj5).set_Size(new Point(24, 24));
				((Image)obj5).set_Texture(AsyncTexture2D.op_Implicit(Textures.RacesDisabled[(uint)index5]));
				((Control)obj5).set_Parent((Container)(object)region);
				obj5.Id = (int)index5;
				list.Insert((int)index5, obj5);
				Label val3 = new Label();
				val3.set_Text("");
				((Control)val3).set_Parent((Container)(object)region);
				((Control)val3).set_Visible(true);
				((Control)val3).set_Width(5);
			}
			FilterWindow obj6 = filterWindow;
			HeadedFlowRegion headedFlowRegion2 = new HeadedFlowRegion();
			((Container)headedFlowRegion2).set_WidthSizingMode((SizingMode)2);
			headedFlowRegion2.Text = common.CraftingProfession;
			((Control)headedFlowRegion2).set_Width(((Control)filterWindow).get_Width() - windowPadding.X * 2);
			((Control)headedFlowRegion2).set_Parent((Container)(object)mainPanel);
			obj6.Crafting = headedFlowRegion2;
			region = filterWindow.Crafting.contentFlowPanel;
			filterCrafting = new List<ToggleIcon>(new ToggleIcon[Textures.Crafting.Length]);
			foreach (Crafting index4 in Enum.GetValues(typeof(Crafting)))
			{
				List<ToggleIcon> list2 = filterCrafting;
				ToggleIcon toggleIcon3 = new ToggleIcon();
				((Control)toggleIcon3).set_Size(new Point(28, 28));
				toggleIcon3._Textures.Add(Textures.CraftingDisabled[(int)index4]);
				toggleIcon3._Textures.Add(Textures.Crafting[(int)index4]);
				toggleIcon3._State = 0;
				toggleIcon3._MaxState = 2;
				((Control)toggleIcon3).set_Parent((Container)(object)region);
				toggleIcon3.Id = (int)index4;
				list2.Insert((int)index4, toggleIcon3);
			}
			FilterWindow obj7 = filterWindow;
			HeadedFlowRegion headedFlowRegion3 = new HeadedFlowRegion();
			((Container)headedFlowRegion3).set_WidthSizingMode((SizingMode)2);
			headedFlowRegion3.Text = common.Profession;
			((Control)headedFlowRegion3).set_Width(((Control)filterWindow).get_Width() - windowPadding.X * 2);
			((Control)headedFlowRegion3).set_Parent((Container)(object)mainPanel);
			obj7.Profession = headedFlowRegion3;
			region = filterWindow.Profession.contentFlowPanel;
			filterProfessions = new List<ToggleIcon>(new ToggleIcon[Textures.Professions.Length]);
			foreach (Professions index3 in Enum.GetValues(typeof(Professions)))
			{
				List<ToggleIcon> list3 = filterProfessions;
				ToggleIcon obj8 = new ToggleIcon
				{
					_Textures = 
					{
						Textures.ProfessionsDisabled[(int)index3],
						Textures.Professions[(int)index3]
					},
					_State = 0,
					_MaxState = 2
				};
				((Control)obj8).set_Parent((Container)(object)region);
				obj8.Id = (int)index3;
				list3.Insert((int)index3, obj8);
			}
			FilterWindow obj9 = filterWindow;
			HeadedFlowRegion headedFlowRegion4 = new HeadedFlowRegion();
			((Container)headedFlowRegion4).set_WidthSizingMode((SizingMode)2);
			headedFlowRegion4.Text = common.Specialization;
			((Control)headedFlowRegion4).set_Width(((Control)filterWindow).get_Width() - windowPadding.X * 2);
			((Control)headedFlowRegion4).set_Parent((Container)(object)mainPanel);
			obj9.Specialization = headedFlowRegion4;
			region = filterWindow.Specialization.contentFlowPanel;
			filterBaseSpecs = new List<ToggleIcon>(new ToggleIcon[Textures.Professions.Length]);
			foreach (Professions index2 in Enum.GetValues(typeof(Professions)))
			{
				List<ToggleIcon> list4 = filterBaseSpecs;
				ToggleIcon obj10 = new ToggleIcon
				{
					_Textures = 
					{
						Textures.ProfessionsDisabled[(int)index2],
						Textures.Professions[(int)index2]
					},
					_State = 0,
					_MaxState = 2
				};
				((Control)obj10).set_Parent((Container)(object)region);
				obj10.Id = (int)index2;
				list4.Insert((int)index2, obj10);
			}
			filterSpecs = new List<ToggleIcon>(new ToggleIcon[Textures.Specializations.Length]);
			Specializations[] array = specs;
			for (int i = 0; i < array.Length; i++)
			{
				int index = (int)array[i];
				List<ToggleIcon> list5 = filterSpecs;
				ToggleIcon obj11 = new ToggleIcon
				{
					_Textures = 
					{
						Textures.SpecializationsDisabled[index],
						Textures.Specializations[index]
					},
					_State = 0,
					_MaxState = 2
				};
				((Control)obj11).set_Parent((Container)(object)region);
				obj11.Id = index;
				list5.Insert(index, obj11);
			}
			FilterWindow obj12 = filterWindow;
			StandardButton val4 = new StandardButton();
			val4.set_Text(common.ToggleAll);
			((Control)val4).set_Parent((Container)(object)region);
			((Control)val4).set_Size(new Point(((Control)region).get_Width() - 10, 25));
			((Control)val4).set_Padding(new Thickness(0f, 3f));
			obj12.toggleSpecsButton = val4;
			((Control)region).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)filterWindow.toggleSpecsButton).set_Width(((Control)region).get_Width() - 10);
			});
			((Control)filterWindow.toggleSpecsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				int state = ((filterBaseSpecs[1]._State != 1) ? 1 : 0);
				foreach (List<ToggleIcon> item in new List<List<ToggleIcon>> { filterSpecs, filterBaseSpecs })
				{
					foreach (ToggleIcon current in item)
					{
						if (current != null)
						{
							current._State = state;
						}
					}
				}
				filterWindow.birthdayToggle._State = 0;
				filterCharacterPanel = true;
			});
			FilterWindow obj13 = filterWindow;
			HeadedFlowRegion headedFlowRegion5 = new HeadedFlowRegion();
			((Container)headedFlowRegion5).set_WidthSizingMode((SizingMode)2);
			headedFlowRegion5.Text = common.CustomTags;
			((Control)headedFlowRegion5).set_Width(((Control)filterWindow).get_Width() - windowPadding.X * 2);
			((Control)headedFlowRegion5).set_Parent((Container)(object)mainPanel);
			obj13.CustomTags = headedFlowRegion5;
			region = filterWindow.CustomTags.contentFlowPanel;
			region.set_ControlPadding(new Vector2(3f, 2f));
			filterTagsPanel = region;
			((Control)filterWindow).Hide();
		}

		private void CreateSubWindow()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Expected O, but got Unknown
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Expected O, but got Unknown
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Expected O, but got Unknown
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Expected O, but got Unknown
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Expected O, but got Unknown
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0639: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Expected O, but got Unknown
			ContentService contentService = new ContentService();
			int offset = 105;
			CharacterDetailWindow characterDetailWindow = new CharacterDetailWindow();
			((Control)characterDetailWindow).set_Location(new Point(((Control)MainWidow).get_Location().X + 385 - 25, ((Control)MainWidow).get_Location().Y + offset));
			((Control)characterDetailWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			characterDetailWindow.Texture = Textures.Backgrounds[3];
			((Control)characterDetailWindow).set_Width(350);
			((Container)characterDetailWindow).set_HeightSizingMode((SizingMode)1);
			subWindow = characterDetailWindow;
			((Control)subWindow).add_Shown((EventHandler<EventArgs>)delegate
			{
				((Control)filterWindow).Hide();
			});
			((Control)MainWidow).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				((Control)subWindow).set_Location(new Point(((Control)MainWidow).get_Location().X + 385 - 25, ((Control)MainWidow).get_Location().Y + offset));
			});
			CharacterDetailWindow characterDetailWindow2 = subWindow;
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)subWindow);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(58, 58));
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.Backgrounds[7]));
			((Control)val).set_Visible(false);
			characterDetailWindow2.border_TopRight = val;
			CharacterDetailWindow characterDetailWindow3 = subWindow;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)subWindow);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(new Point(58, 58));
			val2.set_Texture(AsyncTexture2D.op_Implicit(Textures.Backgrounds[6]));
			((Control)val2).set_Visible(false);
			characterDetailWindow3.border_BottomLeft = val2;
			CharacterDetailWindow characterDetailWindow4 = subWindow;
			Image val3 = new Image();
			((Control)val3).set_Location(new Point(0, 0));
			val3.set_Texture(AsyncTexture2D.op_Implicit(Textures.Professions[1]));
			((Control)val3).set_Size(new Point(58, 58));
			((Control)val3).set_Parent((Container)(object)subWindow);
			characterDetailWindow4.spec_Image = val3;
			((Control)subWindow.spec_Image).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
				{
					subWindow.spec_Image.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[45]));
				}
			});
			((Control)subWindow.spec_Image).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				subWindow.spec_Image.set_Texture(AsyncTexture2D.op_Implicit(subWindow.assignedCharacter.getProfessionTexture()));
			});
			((Control)subWindow.spec_Image).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ImageSelectorWindow.assignedCharacter = subWindow.assignedCharacter;
				((Control)ImageSelectorWindow).set_Visible(true);
			});
			CharacterDetailWindow characterDetailWindow5 = subWindow;
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(60, 0));
			val4.set_Text("py" + ((Module)this).get_Name() + "yq");
			((Control)val4).set_Parent((Container)(object)subWindow);
			((Control)val4).set_Height(25);
			((Control)val4).set_Width(((Control)subWindow).get_Width() - 60 - 32 - 5);
			val4.set_Font(contentService.GetFont((FontFace)0, (FontSize)18, (FontStyle)0));
			val4.set_VerticalAlignment((VerticalAlignment)1);
			characterDetailWindow5.name_Label = val4;
			CharacterDetailWindow characterDetailWindow6 = subWindow;
			Image val5 = new Image();
			((Control)val5).set_Location(new Point(((Control)subWindow.name_Label).get_Location().X + ((Control)subWindow.name_Label).get_Width() + 5, 0));
			val5.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[42]));
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_Parent((Container)(object)subWindow);
			((Control)val5).set_BasicTooltipText(string.Format(common.ShowHide_Tooltip, "Name"));
			characterDetailWindow6.include_Image = val5;
			((Control)subWindow.include_Image).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				subWindow.assignedCharacter.include = !subWindow.assignedCharacter.include;
				subWindow.assignedCharacter.Save();
				subWindow.include_Image.set_Texture(AsyncTexture2D.op_Implicit(subWindow.assignedCharacter.include ? Textures.Icons[42] : Textures.Icons[43]));
			});
			Image val6 = new Image();
			val6.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[19]));
			((Control)val6).set_Parent((Container)(object)subWindow);
			((Control)val6).set_Location(new Point(55, 27));
			((Control)val6).set_Size(new Point(((Control)subWindow).get_Width() - 165, 4));
			CharacterDetailWindow characterDetailWindow7 = subWindow;
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(60, 33));
			val7.set_Text(DataManager.getProfessionName(1));
			((Control)val7).set_Parent((Container)(object)subWindow);
			((Control)val7).set_Height(25);
			((Control)val7).set_Width(((Control)subWindow).get_Width() - 165);
			val7.set_Font(contentService.GetFont((FontFace)0, (FontSize)18, (FontStyle)0));
			val7.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val7).set_Visible(false);
			characterDetailWindow7.spec_Label = val7;
			CharacterDetailWindow characterDetailWindow8 = subWindow;
			Checkbox val8 = new Checkbox();
			((Control)val8).set_Location(new Point(60, 33));
			val8.set_Text(common.LoginCharacter);
			((Control)val8).set_Parent((Container)(object)subWindow);
			((Control)val8).set_Height(25);
			((Control)val8).set_Width(((Control)subWindow).get_Width() - 165);
			characterDetailWindow8.loginCharacter = val8;
			((Control)subWindow.loginCharacter).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (subWindow.loginCharacter.get_Checked())
				{
					foreach (Character character in Characters)
					{
						character.loginCharacter = character == subWindow.assignedCharacter && subWindow.loginCharacter.get_Checked();
					}
				}
				else
				{
					subWindow.assignedCharacter.loginCharacter = subWindow.loginCharacter.get_Checked();
				}
				subWindow.assignedCharacter.Save();
			});
			CharacterDetailWindow characterDetailWindow9 = subWindow;
			TextBox val9 = new TextBox();
			((Control)val9).set_Parent((Container)(object)subWindow);
			((Control)val9).set_Location(new Point(5, 75));
			((Control)val9).set_Size(new Point(((Control)subWindow).get_Width() - 5 - 25 - 5, 25));
			((TextInputBase)val9).set_PlaceholderText("PvE, WvW, PvP, Raiding, ERP ...");
			characterDetailWindow9.tag_TextBox = val9;
			((TextInputBase)subWindow.tag_TextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (((TextInputBase)subWindow.tag_TextBox).get_Text() != null && ((TextInputBase)subWindow.tag_TextBox).get_Text().Trim() != "")
				{
					subWindow.addTag_Button.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[39]));
				}
				else
				{
					subWindow.addTag_Button.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[40]));
				}
			});
			CharacterDetailWindow characterDetailWindow10 = subWindow;
			Image val10 = new Image();
			val10.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[40]));
			((Control)val10).set_Parent((Container)(object)subWindow);
			((Control)val10).set_Location(new Point(((Control)subWindow).get_Width() - 25 - 5, 73));
			((Control)val10).set_Size(new Point(29, 29));
			characterDetailWindow10.addTag_Button = val10;
			((Control)subWindow.addTag_Button).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				subWindow.addTag_Button.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[41]));
			});
			((Control)subWindow.addTag_Button).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				subWindow.addTag_Button.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[40]));
			});
			((Control)subWindow.addTag_Button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				addTag();
			});
			subWindow.tag_TextBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				addTag();
			});
			CharacterDetailWindow characterDetailWindow11 = subWindow;
			FlowPanel val11 = new FlowPanel();
			((Control)val11).set_Parent((Container)(object)subWindow);
			((Control)val11).set_Location(new Point(5, 95));
			((Control)val11).set_Width(330);
			((Panel)val11).set_ShowBorder(true);
			val11.set_OuterControlPadding(new Vector2(2f, 2f));
			val11.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val11).set_HeightSizingMode((SizingMode)1);
			characterDetailWindow11.customTags_Panel = val11;
			((Control)subWindow).Hide();
			void addTag()
			{
				string txt = ((subWindow.tag_TextBox != null && ((TextInputBase)subWindow.tag_TextBox).get_Text().Trim() != "") ? ((TextInputBase)subWindow.tag_TextBox).get_Text() : null);
				if (txt != null && subWindow.assignedCharacter != null && !subWindow.assignedCharacter.Tags.Contains(txt.Trim()))
				{
					new TagEntry(txt, subWindow.assignedCharacter, subWindow.customTags_Panel);
					subWindow.assignedCharacter.Tags.Add(txt);
					subWindow.assignedCharacter.Save();
					if (!Tags.Contains(txt))
					{
						Tags.Add(txt);
						TagEntry entry = new TagEntry(txt, new Character(), filterTagsPanel, showButton: false, contentService.GetFont((FontFace)0, (FontSize)14, (FontStyle)0));
						((Control)entry).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							if (((TextInputBase)filterTextBox).get_Text().ToLower().Contains(txt.ToLower()))
							{
								((TextInputBase)filterTextBox).get_Text();
								txt.Replace("; -t " + txt + ";", "");
								txt.Replace("; -t " + txt, "");
								txt.Replace("-t " + txt + ";", "");
								txt.Replace("-t " + txt, "");
								((TextInputBase)filterTextBox).set_Text(txt.Trim());
							}
							else
							{
								TextBox obj = filterTextBox;
								((TextInputBase)obj).set_Text(((TextInputBase)obj).get_Text() + (((((TextInputBase)filterTextBox).get_Text().Trim().EndsWith(";") || ((TextInputBase)filterTextBox).get_Text().Trim() == "") ? " " : "; ") + "-t " + txt).Trim());
							}
						});
						TagEntries.Add(entry);
					}
					((TextInputBase)subWindow.tag_TextBox).set_Text((string)null);
					subWindow.customTags_Panel.SortChildren<TagEntry>((Comparison<TagEntry>)((TagEntry a, TagEntry b) => a.textLabel.get_Text().CompareTo(b.textLabel.get_Text())));
					((Control)subWindow).Invalidate();
				}
			}
		}

		private void CreateScreenCapture()
		{
			Point resolution = GameService.Graphics.get_Resolution();
			int sidePadding = 255;
			int bottomPadding = 100;
			int topMenuHeight = 60;
			ScreenCaptureWindow obj = new ScreenCaptureWindow(new Point(resolution.X - (sidePadding - 90), topMenuHeight))
			{
				showBackground = false,
				FrameColor = Microsoft.Xna.Framework.Color.Transparent
			};
			((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)obj).set_Location(new Point(sidePadding, resolution.Y - bottomPadding - topMenuHeight));
			((Control)obj).set_Visible(screenCapture);
			obj.LoadCustomImages = LoadCustomImages;
			screenCaptureWindow = obj;
		}

		private void CreateImageSelector()
		{
			int offset = 105;
			ImageSelector imageSelector = new ImageSelector(987, ((Control)MainWidow).get_Height() - offset - 10);
			((Control)imageSelector).set_Location(new Point(((Control)MainWidow).get_Location().X + 385 - 25, ((Control)MainWidow).get_Location().Y + offset));
			((Control)imageSelector).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			imageSelector.Texture = Textures.Backgrounds[16];
			imageSelector.FrameColor = Microsoft.Xna.Framework.Color.AliceBlue;
			((Control)imageSelector).set_Visible(false);
			ImageSelectorWindow = imageSelector;
			((Control)MainWidow).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				((Control)ImageSelectorWindow).set_Location(new Point(((Control)MainWidow).get_Location().X + 385 - 25, ((Control)MainWidow).get_Location().Y + offset));
			});
		}

		private void OnKeyPressed_LogoutMod(object o, EventArgs e)
		{
			Settings.SwapModifierPressed = DateTime.Now;
		}

		private void OnKeyPressed_ToggleMenu(object o, EventArgs e)
		{
			if (!(Control.get_ActiveControl() is TextBox))
			{
				StandardWindow mainWidow = MainWidow;
				if (mainWidow != null)
				{
					((WindowBase2)mainWidow).ToggleWindow();
				}
			}
		}

		protected override void Update(GameTime gameTime)
		{
			Last.Tick_Save += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_Update += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_APIUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_PanelUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_FadeEffect += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (charactersLoaded && Last.Tick_Update > 250.0)
			{
				Last.Tick_Update = -250.0;
				Update_CurrentCharacter();
				if (screenCaptureWindow == null)
				{
					CreateScreenCapture();
				}
				foreach (Character character in Characters)
				{
					if (character.characterControl != null && ((Control)character.characterControl).get_Visible())
					{
						character.Update_UI_Time();
					}
				}
				if (Settings.AutoLogin.get_Value() && !loginCharacter_Swapped && loginCharacter != null && swapCharacter == null)
				{
					loginCharacter_Swapped = true;
					loginCharacter.Swap();
				}
				if (swapCharacter != null && !GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && DateTime.UtcNow.Subtract(lastLogout).TotalMilliseconds >= (double)Settings.SwapDelay.get_Value())
				{
					swapCharacter.Swap();
					swapCharacter = null;
				}
			}
			if (Settings.FadeSubWindows.get_Value() && Last.Tick_FadeEffect > 30.0)
			{
				Last.Tick_FadeEffect = -30.0;
				if (((Control)filterWindow).get_Visible() && DateTime.Now.Subtract(filterWindow.lastInput).TotalMilliseconds >= 2500.0)
				{
					((Control)filterWindow).set_Opacity(((Control)filterWindow).get_Opacity() - 0.1f);
					if (((Control)filterWindow).get_Opacity() <= 0f)
					{
						((Control)filterWindow).Hide();
					}
				}
				if (((Control)subWindow).get_Visible() && DateTime.Now.Subtract(subWindow.lastInput).TotalMilliseconds >= 3500.0)
				{
					((Control)subWindow).set_Opacity(((Control)subWindow).get_Opacity() - 0.1f);
					if (((Control)subWindow).get_Opacity() <= 0f)
					{
						((Control)subWindow).Hide();
					}
				}
			}
			if (charactersLoaded && Last.Tick_PanelUpdate > (double)Settings._FilterDelay)
			{
				Last.Tick_PanelUpdate = -Settings._FilterDelay;
				if (filterCharacterPanel)
				{
					filterCharacterPanel = false;
					UpdateCharacterPanel();
				}
			}
			if (Last.Tick_Save > 250.0 && saveCharacters)
			{
				Last.Tick_Save = -250.0;
				SaveCharacters();
			}
			if (Last.Tick_APIUpdate > 30000.0 && userAccount != null)
			{
				Logger.Debug("Check GW2 API for Updates.");
				Last.Tick_APIUpdate = -30000.0;
				FetchAPI();
			}
		}

		protected override void Unload()
		{
			StandardWindow mainWidow = MainWidow;
			if (mainWidow != null)
			{
				((Control)mainWidow).Dispose();
			}
			CharacterDetailWindow characterDetailWindow = subWindow;
			if (characterDetailWindow != null)
			{
				((Control)characterDetailWindow).Dispose();
			}
			FilterWindow obj = filterWindow;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			CornerIcon obj2 = cornerButton;
			if (obj2 != null)
			{
				((Control)obj2).Dispose();
			}
			ScreenCaptureWindow obj3 = screenCaptureWindow;
			if (obj3 != null)
			{
				((Control)obj3).Dispose();
			}
			Settings.ShortcutKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnKeyPressed_ToggleMenu);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			CharacterNames = new List<string>();
			Characters = new List<Character>();
			Tags = new List<string>();
			TagEntries = new List<TagEntry>();
			userAccount = null;
			swapCharacter = null;
			API_Account = null;
			Current.character = null;
			Last.character = null;
			Textures.Backgrounds = null;
			Textures.Crafting = null;
			Textures.CraftingDisabled = null;
			Textures.Emblems = null;
			Textures.Icons = null;
			Textures.Professions = null;
			Textures.ProfessionsDisabled = null;
			Textures.ProfessionsWhite = null;
			Textures.Races = null;
			Textures.RacesDisabled = null;
			Textures.Specializations = null;
			Textures.SpecializationsDisabled = null;
			Textures.SpecializationsWhite = null;
			ModuleInstance = null;
		}
	}
}
