using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
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
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
	{
		private static class Last
		{
			public static double Tick_PanelUpdate;

			public static double Tick_Save;

			public static double Tick_Update;

			public static double Tick_APIUpdate;

			public static Character character { get; set; }

			public static string CharName { get; set; }

			public static int CharCount { get; set; }
		}

		private static class Current
		{
			public static Character character { get; set; }
		}

		public localMapModel[] __Maps;

		public List<string> __ProfessionNames;

		public List<string> __SpecializationNames;

		public List<string> _ProfessionNames;

		public List<string> _SpecializationNames;

		public static _Settings Settings = new _Settings();

		private DateTime dateZero;

		public static DateTime lastLogout;

		private static bool requestAPI = true;

		private static bool filterCharacterPanel = true;

		public ContentService contentService = new ContentService();

		public static string CharactersPath;

		public static string AccountPath;

		public static FlowPanel CharacterPanel;

		public static FlowPanel filterPanel;

		public static TextBox filterTextBox;

		public static StandardButton clearButton;

		public static Image infoImage;

		public static Image expandButton;

		public static StandardButton refreshAPI;

		public static CornerIcon cornerButton;

		public static ToggleImage birthdayToggle;

		public static ToggleImage[] filterProfessions;

		public static ToggleImage[] filterCrafting;

		public static ToggleImage[] filterRaces;

		public static ToggleImage[] filterSpecs;

		public static ToggleImage[] filterBaseSpecs;

		public static Label racesLabel;

		public static Label specializationLabel;

		public static Label customTagsLabel;

		public static FlowPanel filterTagsPanel;

		public static bool charactersLoaded;

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

		public SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		public ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		public DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		public Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		public static StandardWindow MainWidow { get; private set; }

		public static BasicContainer filterWindow { get; private set; }

		public static CharacterDetailWindow subWindow { get; private set; }

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
			IApiV2ObjectList<Map> maps = await Gw2ApiManager.Gw2ApiClient.V2.Maps.AllAsync();
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
			IApiV2ObjectList<Profession> professions = await Gw2ApiManager.Gw2ApiClient.V2.Professions.AllAsync();
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
			IApiV2ObjectList<Specialization> specs = await Gw2ApiManager.Gw2ApiClient.V2.Specializations.AllAsync();
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
			IApiV2ObjectList<Race> races = await Gw2ApiManager.Gw2ApiClient.V2.Races.AllAsync();
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
			Race result = await Gw2ApiManager.Gw2ApiClient.V2.Races.GetAsync(id);
			if (result != null)
			{
				return result.Name;
			}
			return "Unkown Race";
		}

		public async Task<string> getProfession(string id)
		{
			Profession result = await Gw2ApiManager.Gw2ApiClient.V2.Professions.GetAsync(id);
			if (result != null)
			{
				return result.Name;
			}
			return "Unkown Race";
		}

		public async Task<string> getSpecialization(int id)
		{
			Specialization result = await Gw2ApiManager.Gw2ApiClient.V2.Specializations.GetAsync(id);
			if (result != null)
			{
				return result.Name;
			}
			return "Unkown Race";
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings.LogoutKey = settings.DefineSetting("LogoutKey", new KeyBinding(Keys.F12), () => common.Logout, () => common.LogoutDescription);
			Settings.EnterOnSwap = settings.DefineSetting("EnterOnSwap", defaultValue: false, () => common.LoginAfterSelect, () => common.LoginAfterSelect);
			Settings.SwapDelay = settings.DefineSetting("SwapDelay", 500, () => string.Format(common.SwapDelay_DisplayName, Settings.SwapDelay.Value), () => common.SwapDelay_Description);
			Settings.SwapDelay.SetRange(0, 5000);
			Settings.FilterDelay = settings.DefineSetting("FilterDelay", 150, () => string.Format(common.FilterDelay_DisplayName, Settings.FilterDelay.Value), () => common.FilterDelay_Description);
			Settings.FilterDelay.SetRange(0, 500);
			Settings.FilterDelay.SettingChanged += delegate
			{
				Settings._FilterDelay = Settings.FilterDelay.Value / 2;
			};
			Settings._FilterDelay = Settings.FilterDelay.Value / 2;
		}

		public async void FetchAPI(bool force = false)
		{
			if (Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Characters
			}) && API_Account != null && userAccount != null)
			{
				Account account = await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync();
				userAccount.LastModified = account.LastModified;
				userAccount.Save();
				if (userAccount.CharacterUpdateNeeded() || force)
				{
					userAccount.LastBlishUpdate = ((userAccount.LastBlishUpdate > account.LastModified) ? userAccount.LastBlishUpdate : account.LastModified);
					userAccount.Save();
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Character> obj = await Gw2ApiManager.Gw2ApiClient.V2.Characters.AllAsync();
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
				infoImage.BasicTooltipText = "Last Modified: " + Math.Round(lastModified) + Environment.NewLine + "Last Blish Login: " + Math.Round(lastUpdate);
			}
			else
			{
				ScreenNotification.ShowNotification(common.Error_Competivive, ScreenNotification.NotificationType.Error);
				Logger.Error("This API Token has not the required permissions!");
			}
		}

		private async void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Logger.Debug("API Subtoken Updated!");
			if (Gw2ApiManager.HasPermissions(new TokenPermission[2]
			{
				TokenPermission.Account,
				TokenPermission.Characters
			}))
			{
				Account account = await Gw2ApiManager.Gw2ApiClient.V2.Account.GetAsync();
				Logger.Debug("Account Age: " + account.Age.TotalSeconds + " seconds");
				Logger.Debug("LastModified: " + account.LastModified.ToString());
				API_Account = account;
				string path = DirectoriesManager.GetFullDirectoryPath("characters") + "\\" + API_Account.Name;
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				CharactersPath = path + "\\characters.json";
				AccountPath = path + "\\account.json";
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
					IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Character> obj = await Gw2ApiManager.Gw2ApiClient.V2.Characters.AllAsync();
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
				PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
				foreach (Character character in Characters)
				{
					Logger.Debug("Adding UI Elements for " + character.Name);
					character.Create_UI_Elements();
					if (player != null && player.Name == character.Name)
					{
						Current.character = character;
					}
				}
				charactersLoaded = true;
				filterCharacterPanel = true;
				double lastModified = DateTimeOffset.UtcNow.Subtract(userAccount.LastModified).TotalSeconds;
				double lastUpdate = DateTimeOffset.UtcNow.Subtract(userAccount.LastBlishUpdate).TotalSeconds;
				infoImage.BasicTooltipText = "Last Modified: " + Math.Round(lastModified) + Environment.NewLine + "Last Blish Login: " + Math.Round(lastUpdate);
			}
			else
			{
				ScreenNotification.ShowNotification(common.Error_InvalidPermissions, ScreenNotification.NotificationType.Error);
				Logger.Error("This API Token has not the required permissions!");
			}
		}

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			Logger.Debug("Initializing ...");
			ModKeyMapping = new VirtualKeyShort[5];
			ModKeyMapping[1] = VirtualKeyShort.CONTROL;
			ModKeyMapping[2] = VirtualKeyShort.MENU;
			ModKeyMapping[4] = VirtualKeyShort.LSHIFT;
			LoadTextures();
			Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;
			AccountPath = DirectoriesManager.GetFullDirectoryPath("characters") + "\\accounts.json";
			DataManager.ContentsManager = ContentsManager;
			DataManager.Load();
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
			}
			Textures.Specializations = new Texture2D[Enum.GetValues(typeof(Specializations)).Cast<int>().Max() + 1];
			Textures.SpecializationsDisabled = new Texture2D[Enum.GetValues(typeof(Specializations)).Cast<int>().Max() + 1];
			foreach (Specializations specialization in Enum.GetValues(typeof(Specializations)))
			{
				Texture2D[] specializations = Textures.Specializations;
				ContentsManager contentsManager6 = ContentsManager;
				int num = (int)specialization;
				specializations[(int)specialization] = contentsManager6.GetTexture("textures\\specializations\\" + num + ".png");
				Texture2D[] specializationsDisabled = Textures.SpecializationsDisabled;
				ContentsManager contentsManager7 = ContentsManager;
				num = (int)specialization;
				specializationsDisabled[(int)specialization] = contentsManager7.GetTexture("textures\\specializations gray\\" + num + ".png");
			}
			Textures.Crafting = new Texture2D[Enum.GetValues(typeof(Crafting)).Cast<int>().Max() + 1];
			Textures.CraftingDisabled = new Texture2D[Enum.GetValues(typeof(Crafting)).Cast<int>().Max() + 1];
			foreach (Crafting crafting in Enum.GetValues(typeof(Crafting)))
			{
				Texture2D[] crafting2 = Textures.Crafting;
				ContentsManager contentsManager8 = ContentsManager;
				int num = (int)crafting;
				crafting2[(int)crafting] = contentsManager8.GetTexture("textures\\crafting\\" + num + ".png");
				Texture2D[] craftingDisabled = Textures.CraftingDisabled;
				ContentsManager contentsManager9 = ContentsManager;
				num = (int)crafting;
				craftingDisabled[(int)crafting] = contentsManager9.GetTexture("textures\\crafting gray\\" + num + ".png");
			}
		}

		private void Load_UserLocale()
		{
			foreach (Character c in Characters)
			{
				CharacterTooltip obj = (CharacterTooltip)c.characterControl.Tooltip;
				obj._mapLabel.Text = DataManager.getMapName(c.map);
				obj._raceLabel.Text = DataManager.getRaceName(c.Race.ToString());
				c.switchButton.BasicTooltipText = string.Format(common.Switch, c.Name);
			}
			filterTextBox.PlaceholderText = common.SearchFor;
			clearButton.Text = common.Clear;
			racesLabel.Text = common.Race;
			specializationLabel.Text = common.Specialization;
			customTagsLabel.Text = common.CustomTags;
			ToggleImage[] array = filterProfessions;
			foreach (ToggleImage toggle in array)
			{
				if (toggle != null)
				{
					toggle.BasicTooltipText = DataManager.getProfessionName(toggle.Id);
				}
			}
			array = filterBaseSpecs;
			foreach (ToggleImage toggle2 in array)
			{
				if (toggle2 != null)
				{
					toggle2.BasicTooltipText = DataManager.getProfessionName(toggle2.Id);
				}
			}
			array = filterCrafting;
			foreach (ToggleImage toggle3 in array)
			{
				if (toggle3 != null)
				{
					toggle3.BasicTooltipText = DataManager.getCraftingName(Enum.GetName(typeof(Crafting), toggle3.Id));
					if (toggle3.Id == 0)
					{
						toggle3.BasicTooltipText = common.NoCraftingProfession;
					}
				}
			}
			array = filterSpecs;
			foreach (ToggleImage toggle4 in array)
			{
				if (toggle4 != null)
				{
					toggle4.BasicTooltipText = DataManager.getSpecName(toggle4.Id);
				}
			}
			array = filterRaces;
			foreach (ToggleImage toggle5 in array)
			{
				if (toggle5 != null)
				{
					toggle5.BasicTooltipText = DataManager.getRaceName(toggle5.Id);
				}
			}
			birthdayToggle.BasicTooltipText = common.Birthday;
		}

		private void LoadCharacterList()
		{
			Logger.Debug("Character File exists: " + System.IO.File.Exists(CharactersPath));
			if (!System.IO.File.Exists(CharactersPath))
			{
				return;
			}
			requestAPI = false;
			foreach (JsonCharacter c in JsonConvert.DeserializeObject<List<JsonCharacter>>(System.IO.File.ReadAllText(CharactersPath))!)
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
				character2.map = c.map;
				character2.Level = c.Level;
				character2.Tags = ((c.Tags != null && c.Tags != "") ? c.Tags.Split('|').ToList() : new List<string>());
				Character character = character2;
				foreach (string tag in character.Tags)
				{
					if (!Tags.Contains(tag))
					{
						Tags.Add(tag);
					}
				}
				Characters.Add(character);
				CharacterNames.Add(character.Name);
			}
			new Character();
			foreach (string tag2 in Tags)
			{
				TagEntry entry = new TagEntry(tag2, new Character(), filterTagsPanel, showButton: false, contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size14, ContentService.FontStyle.Regular));
				entry.Click += delegate
				{
					TextBox textBox = filterTextBox;
					textBox.Text = textBox.Text + ((filterTextBox.Text.Trim().EndsWith(";") || filterTextBox.Text.Trim() == "") ? " " : "; ") + "-t " + tag2;
				};
				TagEntries.Add(entry);
			}
		}

		protected override async Task LoadAsync()
		{
			cornerButton = new CornerIcon
			{
				IconName = "Characters",
				Icon = Textures.Icons[22],
				HoverIcon = Textures.Icons[23],
				Priority = 4
			};
			cornerButton.Click += delegate
			{
				MainWidow.ToggleWindow();
				subWindow.Hide();
				filterWindow.Hide();
			};
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
			string txt = filterTextBox.Text.ToLower();
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
				if (matchingFilterString(character) && matchingToggles(character))
				{
					character.Show();
				}
				else
				{
					character.Hide();
				}
			}
			CharacterPanel.SortChildren((CharacterControl a, CharacterControl b) => b.assignedCharacter.LastModified.CompareTo(a.assignedCharacter.LastModified));
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
						if (crafting2.Active && Enum.GetName(typeof(Crafting), crafting2.Id).ToLower().Contains(s12))
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
					if (c.birthdayImage.Visible)
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
					string map = DataManager.getMapName(c.map);
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
				bool professionMatch = false;
				bool craftingMatch = false;
				bool birthdayMatch = false;
				bool raceMatch = false;
				bool specMatch = false;
				ToggleImage[] array3 = filterProfessions;
				foreach (ToggleImage toggle in array3)
				{
					if (toggle != null && toggle._State == 1 && toggle.Id == c._Profession)
					{
						professionMatch = true;
					}
				}
				array3 = filterSpecs;
				foreach (ToggleImage toggle2 in array3)
				{
					if (toggle2 != null && toggle2._State == 1 && toggle2.Id == c._Specialization)
					{
						specMatch = true;
					}
				}
				array3 = filterBaseSpecs;
				foreach (ToggleImage toggle3 in array3)
				{
					if (toggle3 != null && toggle3._State == 1 && c._Specialization == 0 && toggle3.Id == c._Profession)
					{
						specMatch = true;
					}
				}
				array3 = filterRaces;
				foreach (ToggleImage toggle4 in array3)
				{
					if (toggle4 != null && toggle4._State == 1 && toggle4.Id == (int)c.Race)
					{
						raceMatch = true;
					}
				}
				array3 = filterCrafting;
				foreach (ToggleImage toggle5 in array3)
				{
					if (toggle5 != null && toggle5._State == 1 && toggle5.Id == 0 && c.Crafting.Count == 0)
					{
						craftingMatch = true;
						break;
					}
					if (toggle5 != null && toggle5._State == 1)
					{
						foreach (CharacterCrafting crafting in c.Crafting)
						{
							if (crafting.Active && toggle5.Id == crafting.Id)
							{
								craftingMatch = true;
							}
						}
					}
				}
				switch (birthdayToggle._State)
				{
				case 0:
					birthdayMatch = true;
					break;
				case 1:
					birthdayMatch = c.birthdayImage.Visible;
					break;
				case 2:
					birthdayMatch = !c.birthdayImage.Visible;
					break;
				}
				return craftingMatch && professionMatch && birthdayMatch && raceMatch && specMatch;
			}
		}

		private void Update_CurrentCharacter()
		{
			if (GameService.GameIntegration.Gw2Instance.IsInGame && charactersLoaded)
			{
				PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
				Last.character = Current.character;
				Current.character = getCharacter(player.Name);
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
			base.OnModuleLoaded(e);
			CreateWindow();
			CreateFilterWindow();
			CreateSubWindow();
			GameService.Gw2Mumble.PlayerCharacter.SpecializationChanged += delegate
			{
				if (Current.character != null)
				{
					Current.character.UpdateProfession();
					filterCharacterPanel = true;
				}
			};
			GameService.Overlay.UserLocaleChanged += delegate
			{
				Load_UserLocale();
			};
			Load_UserLocale();
		}

		private void CreateWindow()
		{
			MainWidow = new StandardWindow(Textures.Backgrounds[1], new Microsoft.Xna.Framework.Rectangle(15, 45, 365, 920), new Microsoft.Xna.Framework.Rectangle(10, 15, 385, 920))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = "Characters",
				Emblem = Textures.Emblems[1],
				Subtitle = "❤",
				SavesPosition = true,
				Id = "CharactersWindow"
			};
			infoImage = new Image
			{
				Texture = Textures.Icons[2],
				Size = new Point(28, 28),
				Location = new Point(MainWidow.Width - 25, -5),
				Parent = MainWidow
			};
			refreshAPI = new StandardButton
			{
				Size = new Point(185, 25),
				Location = new Point(175, 0),
				Parent = MainWidow,
				Text = "Refresh API Data",
				Visible = false
			};
			refreshAPI.Click += delegate
			{
				FetchAPI(force: true);
			};
			TextBox textBox = new TextBox();
			textBox.PlaceholderText = "Search for ...";
			textBox.Size = new Point(282, 30);
			textBox.Font = GameService.Content.DefaultFont16;
			textBox.Location = new Point(10, 20);
			textBox.Parent = MainWidow;
			textBox.BasicTooltipText = "'-c CraftingDiscipline'" + Environment.NewLine + "'-p Profession/Specialization'" + Environment.NewLine + "'-r Race'" + Environment.NewLine + "'-b(irthday)'" + Environment.NewLine + "'-c Chef; -p Warrior' will show all warriors and all chefs";
			filterTextBox = textBox;
			new Tooltip();
			filterTextBox.TextChanged += delegate
			{
				filterCharacterPanel = true;
			};
			clearButton = new StandardButton
			{
				Text = "Clear",
				Location = new Point(292, 19),
				Size = new Point(73, 32),
				Parent = MainWidow,
				ResizeIcon = true
			};
			clearButton.Click += delegate
			{
				reset();
			};
			filterPanel = new FlowPanel
			{
				Parent = MainWidow,
				Size = new Point(329, 60),
				Location = new Point(10, 55),
				FlowDirection = ControlFlowDirection.LeftToRight
			};
			filterProfessions = new ToggleImage[Textures.Professions.Length];
			foreach (Professions profession in Enum.GetValues(typeof(Professions)))
			{
				filterProfessions[(int)profession] = new ToggleImage
				{
					_State = 1,
					_MaxState = 2,
					Size = new Point(32, 32),
					Texture = Textures.Professions[(int)profession],
					Parent = filterPanel,
					Id = (int)profession
				};
				filterProfessions[(int)profession]._Textures = new Texture2D[2];
				filterProfessions[(int)profession]._Textures[0] = Textures.ProfessionsDisabled[(int)profession];
				filterProfessions[(int)profession]._Textures[1] = Textures.Professions[(int)profession];
				filterProfessions[(int)profession].Click += delegate
				{
					filterProfessions[(int)profession].Toggle();
					filterCharacterPanel = true;
				};
			}
			birthdayToggle = new ToggleImage
			{
				isActive = false,
				Size = new Point(32, 32),
				Texture = Textures.Icons[24],
				Parent = filterPanel,
				_State = 0,
				_MaxState = 3
			};
			birthdayToggle._Textures = new Texture2D[3];
			birthdayToggle._Textures[0] = Textures.Icons[24];
			birthdayToggle._Textures[1] = Textures.Icons[17];
			birthdayToggle._Textures[2] = Textures.Icons[25];
			birthdayToggle.Click += delegate
			{
				birthdayToggle.Toggle();
				filterCharacterPanel = true;
			};
			new Label
			{
				Text = "",
				Size = new Point(16, 32),
				Parent = filterPanel,
				Visible = false
			};
			filterCrafting = new ToggleImage[Textures.Crafting.Length];
			foreach (Crafting crafting in Enum.GetValues(typeof(Crafting)))
			{
				if (crafting != 0)
				{
					filterCrafting[(int)crafting] = new ToggleImage
					{
						_State = 1,
						_MaxState = 2,
						Size = new Point(32, 32),
						Texture = Textures.Crafting[(int)crafting],
						Parent = filterPanel,
						Id = (int)crafting
					};
					filterCrafting[(int)crafting]._Textures = new Texture2D[2];
					filterCrafting[(int)crafting]._Textures[0] = Textures.CraftingDisabled[(int)crafting];
					filterCrafting[(int)crafting]._Textures[1] = Textures.Crafting[(int)crafting];
					filterCrafting[(int)crafting].Click += delegate
					{
						filterCrafting[(int)crafting].Toggle();
						filterCharacterPanel = true;
					};
				}
			}
			filterCrafting[0] = new ToggleImage
			{
				_State = 1,
				_MaxState = 2,
				Size = new Point(32, 32),
				Texture = Textures.Crafting[0],
				Parent = filterPanel,
				Id = 0
			};
			filterCrafting[0]._Textures = new Texture2D[2];
			filterCrafting[0]._Textures[0] = Textures.CraftingDisabled[0];
			filterCrafting[0]._Textures[1] = Textures.Crafting[0];
			filterCrafting[0].Click += delegate
			{
				filterCrafting[0].Toggle();
				filterCharacterPanel = true;
			};
			expandButton = new Image
			{
				Texture = Textures.Icons[33],
				Size = new Point(26, 64),
				Location = new Point(334, 57),
				Parent = MainWidow
			};
			expandButton.MouseEntered += delegate
			{
				if (filterWindow.Visible)
				{
					expandButton.Texture = Textures.Icons[38];
				}
				else
				{
					expandButton.Texture = Textures.Icons[34];
				}
			};
			expandButton.MouseLeft += delegate
			{
				if (filterWindow.Visible)
				{
					expandButton.Texture = Textures.Icons[37];
				}
				else
				{
					expandButton.Texture = Textures.Icons[33];
				}
			};
			expandButton.Click += delegate
			{
				if (filterWindow.Visible)
				{
					filterWindow.Hide();
					expandButton.Texture = Textures.Icons[34];
				}
				else
				{
					filterWindow.Show();
					subWindow.Hide();
					expandButton.Texture = Textures.Icons[38];
				}
			};
			CharacterPanel = new FlowPanel
			{
				CanScroll = true,
				ShowBorder = true,
				Parent = MainWidow,
				Size = new Point(MainWidow.Width, MainWidow.Height - 161),
				Location = new Point(0, 119),
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			static void reset()
			{
				foreach (ToggleImage[] item in new List<ToggleImage[]> { filterCrafting, filterProfessions, filterSpecs, filterRaces, filterBaseSpecs })
				{
					foreach (ToggleImage toggle in item)
					{
						if (toggle != null)
						{
							toggle._State = 1;
						}
					}
				}
				birthdayToggle._State = 0;
				filterTextBox.Text = null;
				filterCharacterPanel = true;
			}
		}

		private void CreateFilterWindow()
		{
			ContentService contentService = new ContentService();
			filterWindow = new BasicContainer
			{
				HeightSizingMode = SizingMode.AutoSize,
				Width = 300,
				Location = new Point(MainWidow.Location.X + 385 - 25, MainWidow.Location.Y + 60),
				Parent = GameService.Graphics.SpriteScreen,
				Texture = Textures.Backgrounds[3],
				AutoSizePadding = new Point(5, 5)
			};
			MainWidow.Moved += delegate
			{
				filterWindow.Location = new Point(MainWidow.Location.X + 385 - 25, MainWidow.Location.Y + 60);
			};
			Image closeButton = new Image
			{
				Texture = Textures.Icons[35],
				Parent = filterWindow,
				Location = new Point(filterWindow.Width - 23, 2),
				Size = new Point(21, 23)
			};
			closeButton.MouseEntered += delegate
			{
				closeButton.Texture = Textures.Icons[36];
			};
			closeButton.MouseLeft += delegate
			{
				closeButton.Texture = Textures.Icons[35];
			};
			closeButton.Click += delegate
			{
				filterWindow.Hide();
				expandButton.Texture = Textures.Icons[33];
			};
			racesLabel = new Label
			{
				Text = "Races",
				Parent = filterWindow,
				Location = new Point(10, 5),
				Visible = true,
				Width = 200,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular),
				Height = 27
			};
			new Image
			{
				Texture = Textures.Icons[19],
				Parent = filterWindow,
				Location = new Point(0, 30),
				Size = new Point(filterWindow.Width, 4)
			};
			FlowPanel racePanel = new FlowPanel
			{
				Parent = filterWindow,
				Size = new Point(filterWindow.Width - 10, 30),
				Location = new Point(5, 45),
				FlowDirection = ControlFlowDirection.LeftToRight
			};
			filterRaces = new ToggleImage[Textures.Races.Length];
			foreach (RaceType race in Enum.GetValues(typeof(RaceType)))
			{
				int index3 = (int)race;
				filterRaces[index3] = new ToggleImage
				{
					_State = 1,
					_MaxState = 2,
					Size = new Point(24, 24),
					Texture = Textures.Races[index3],
					Parent = racePanel,
					Id = index3
				};
				filterRaces[index3]._Textures = new Texture2D[2];
				filterRaces[index3]._Textures[0] = Textures.RacesDisabled[index3];
				filterRaces[index3]._Textures[1] = Textures.Races[index3];
				filterRaces[index3].Click += delegate
				{
					filterRaces[index3].Toggle();
					filterCharacterPanel = true;
				};
				new Label
				{
					Text = "",
					Parent = racePanel,
					Visible = true,
					Width = 5
				};
			}
			specializationLabel = new Label
			{
				Text = "Specializations",
				Parent = filterWindow,
				Location = new Point(10, 85),
				Visible = true,
				Width = 200,
				Height = 27,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular)
			};
			new Image
			{
				Texture = Textures.Icons[19],
				Parent = filterWindow,
				Location = new Point(0, 110),
				Size = new Point(filterWindow.Width, 4)
			};
			FlowPanel specPanel = new FlowPanel
			{
				Parent = filterWindow,
				Size = new Point(filterWindow.Width - 10, 128),
				Location = new Point(5, 120),
				FlowDirection = ControlFlowDirection.LeftToRight
			};
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
			filterBaseSpecs = new ToggleImage[Textures.Professions.Length];
			foreach (Professions profession in Enum.GetValues(typeof(Professions)))
			{
				int index2 = (int)profession;
				filterBaseSpecs[index2] = new ToggleImage
				{
					_State = 1,
					_MaxState = 2,
					Size = new Point(32, 32),
					Texture = Textures.Professions[index2],
					Parent = specPanel,
					Id = index2
				};
				filterBaseSpecs[index2]._Textures = new Texture2D[2];
				filterBaseSpecs[index2]._Textures[0] = Textures.ProfessionsDisabled[index2];
				filterBaseSpecs[index2]._Textures[1] = Textures.Professions[index2];
				filterBaseSpecs[index2].Click += delegate
				{
					filterBaseSpecs[index2].Toggle();
					filterCharacterPanel = true;
				};
			}
			filterSpecs = new ToggleImage[Textures.Specializations.Length];
			Specializations[] array = specs;
			foreach (Specializations spec in array)
			{
				int index = (int)spec;
				filterSpecs[index] = new ToggleImage
				{
					_State = 1,
					_MaxState = 2,
					Size = new Point(32, 32),
					Texture = Textures.Specializations[index],
					Parent = specPanel,
					Id = index
				};
				filterSpecs[index]._Textures = new Texture2D[2];
				filterSpecs[index]._Textures[0] = Textures.SpecializationsDisabled[index];
				filterSpecs[index]._Textures[1] = Textures.Specializations[index];
				filterSpecs[index].Click += delegate
				{
					filterSpecs[index].Toggle();
					filterCharacterPanel = true;
				};
			}
			StandardButton btn = new StandardButton
			{
				Text = "Toggle all",
				Parent = filterWindow,
				Location = new Point(5, specPanel.Location.Y + specPanel.Height + 5),
				Size = new Point(specPanel.Width, 25)
			};
			btn.Click += delegate
			{
				int state = ((filterBaseSpecs[1]._State != 1) ? 1 : 0);
				foreach (ToggleImage[] item in new List<ToggleImage[]> { filterSpecs, filterBaseSpecs })
				{
					foreach (ToggleImage toggleImage in item)
					{
						if (toggleImage != null)
						{
							toggleImage._State = state;
						}
					}
				}
				birthdayToggle._State = 0;
				filterCharacterPanel = true;
			};
			customTagsLabel = new Label
			{
				Text = common.CustomTags,
				Parent = filterWindow,
				Location = new Point(10, btn.Location.Y + btn.Height + 10),
				Visible = true,
				Width = 200,
				Height = 27,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular)
			};
			new Image
			{
				Texture = Textures.Icons[19],
				Parent = filterWindow,
				Location = new Point(0, btn.Location.Y + btn.Height + 10 + 25),
				Size = new Point(filterWindow.Width, 4)
			};
			filterTagsPanel = new FlowPanel
			{
				Parent = filterWindow,
				Location = new Point(5, btn.Location.Y + btn.Height + 10 + 25 + 8),
				Width = filterWindow.Width,
				OuterControlPadding = new Vector2(2f, 2f),
				ControlPadding = new Vector2(5f, 2f),
				HeightSizingMode = SizingMode.AutoSize
			};
			filterWindow.Hide();
		}

		private void CreateSubWindow()
		{
			ContentService contentService = new ContentService();
			int offset = 169;
			subWindow = new CharacterDetailWindow
			{
				Location = new Point(MainWidow.Location.X + 385 - 25, MainWidow.Location.Y + offset),
				Parent = GameService.Graphics.SpriteScreen,
				Texture = Textures.Backgrounds[3],
				Width = 350,
				HeightSizingMode = SizingMode.AutoSize
			};
			MainWidow.Moved += delegate
			{
				subWindow.Location = new Point(MainWidow.Location.X + 385 - 25, MainWidow.Location.Y + offset);
			};
			subWindow.spec_Image = new Image
			{
				Location = new Point(0, 0),
				Texture = Textures.Professions[1],
				Size = new Point(58, 58),
				Parent = subWindow
			};
			subWindow.name_Label = new Label
			{
				Location = new Point(60, 0),
				Text = "py" + base.Name + "yq",
				Parent = subWindow,
				Height = 25,
				Width = subWindow.Width - 165,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular),
				VerticalAlignment = VerticalAlignment.Middle
			};
			new Image
			{
				Texture = Textures.Icons[19],
				Parent = subWindow,
				Location = new Point(55, 27),
				Size = new Point(subWindow.Width - 165, 4)
			};
			subWindow.spec_Label = new Label
			{
				Location = new Point(60, 33),
				Text = DataManager.getProfessionName(1),
				Parent = subWindow,
				Height = 25,
				Width = subWindow.Width - 165,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size18, ContentService.FontStyle.Regular),
				VerticalAlignment = VerticalAlignment.Middle
			};
			subWindow.tag_TextBox = new TextBox
			{
				Parent = subWindow,
				Location = new Point(5, 75),
				Size = new Point(subWindow.Width - 5 - 25 - 5, 25),
				PlaceholderText = "PvE, WvW, PvP, Raiding, ERP ..."
			};
			subWindow.tag_TextBox.TextChanged += delegate
			{
				if (subWindow.tag_TextBox.Text != null && subWindow.tag_TextBox.Text.Trim() != "")
				{
					subWindow.addTag_Button.Texture = Textures.Icons[39];
				}
				else
				{
					subWindow.addTag_Button.Texture = Textures.Icons[40];
				}
			};
			subWindow.addTag_Button = new Image
			{
				Texture = Textures.Icons[40],
				Parent = subWindow,
				Location = new Point(subWindow.Width - 25 - 5, 73),
				Size = new Point(29, 29)
			};
			subWindow.addTag_Button.MouseEntered += delegate
			{
				subWindow.addTag_Button.Texture = Textures.Icons[41];
			};
			subWindow.addTag_Button.MouseLeft += delegate
			{
				subWindow.addTag_Button.Texture = Textures.Icons[40];
			};
			subWindow.addTag_Button.Click += delegate
			{
				string txt = ((subWindow.tag_TextBox != null && subWindow.tag_TextBox.Text.Trim() != "") ? subWindow.tag_TextBox.Text : null);
				if (txt != null && subWindow.assignedCharacter != null && !subWindow.assignedCharacter.Tags.Contains(txt))
				{
					new TagEntry(txt, subWindow.assignedCharacter, subWindow.customTags_Panel);
					subWindow.assignedCharacter.Tags.Add(txt);
					subWindow.assignedCharacter.Save();
					if (!Tags.Contains(txt))
					{
						Tags.Add(txt);
						TagEntry tagEntry = new TagEntry(txt, new Character(), filterTagsPanel, showButton: false, contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size14, ContentService.FontStyle.Regular));
						tagEntry.Click += delegate
						{
							TextBox textBox = filterTextBox;
							textBox.Text = textBox.Text + ((filterTextBox.Text.Trim().EndsWith(";") || filterTextBox.Text.Trim() == "") ? " " : "; ") + "-t " + txt;
						};
						TagEntries.Add(tagEntry);
					}
					subWindow.tag_TextBox.Text = null;
					subWindow.customTags_Panel.SortChildren((TagEntry a, TagEntry b) => a.textLabel.Text.CompareTo(b.textLabel.Text));
					subWindow.Invalidate();
				}
			};
			subWindow.customTags_Panel = new FlowPanel
			{
				Parent = subWindow,
				Location = new Point(5, 95),
				Width = 330,
				ShowBorder = true,
				OuterControlPadding = new Vector2(2f, 2f),
				ControlPadding = new Vector2(5f, 2f),
				HeightSizingMode = SizingMode.AutoSize
			};
			subWindow.Hide();
		}

		protected override void Update(GameTime gameTime)
		{
			Last.Tick_Save += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_Update += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_APIUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
			Last.Tick_PanelUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (charactersLoaded && Last.Tick_Update > 250.0)
			{
				Last.Tick_Update = -250.0;
				Update_CurrentCharacter();
				foreach (Character character in Characters)
				{
					if (character.characterControl.Visible)
					{
						character.Update_UI_Time();
					}
				}
				if (swapCharacter != null && !GameService.GameIntegration.Gw2Instance.IsInGame && DateTime.UtcNow.Subtract(lastLogout).TotalMilliseconds >= (double)Settings.SwapDelay.Value)
				{
					swapCharacter.Swap();
					swapCharacter = null;
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
			if (Last.Tick_Save > 15000.0 && userAccount != null)
			{
				Last.Tick_Save = -15000.0;
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
		}
	}
}
