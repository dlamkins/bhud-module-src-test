using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Enums;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Models
{
	[DataContract]
	public class Character_Model
	{
		private readonly Action _requestSave;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly Data _data;

		private readonly CharacterSwapping _characterSwapping;

		private AsyncTexture2D _icon;

		private bool _initialized;

		private bool _pathChecked;

		private string _name;

		private int _level;

		private int _map;

		private Gender _gender;

		private RaceType _race;

		private ProfessionType _profession;

		private SpecializationType _specialization;

		private DateTimeOffset _created;

		private DateTime _lastModified;

		private DateTime _lastLogin;

		private string _iconPath;

		private bool _show = true;

		private int _position;

		private int _index;

		private bool _showOnRadial;

		private bool _hadBirthday;

		[DataMember]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				SetProperty(ref _name, value, "Name");
			}
		}

		[DataMember]
		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				SetProperty(ref _level, value, "Level");
			}
		}

		[DataMember]
		public int Map
		{
			get
			{
				return _map;
			}
			set
			{
				SetProperty(ref _map, value, "Map");
			}
		}

		[DataMember]
		public List<CharacterCrafting> Crafting { get; } = new List<CharacterCrafting>();


		public List<KeyValuePair<int, Data.CraftingProfession>> CraftingDisciplines
		{
			get
			{
				List<KeyValuePair<int, Data.CraftingProfession>> list = new List<KeyValuePair<int, Data.CraftingProfession>>();
				if (_data != null)
				{
					foreach (CharacterCrafting crafting in Crafting)
					{
						Data.CraftingProfession craftingProf = _data.CrafingProfessions.Where((KeyValuePair<int, Data.CraftingProfession> e) => e.Value.Id == crafting.Id)?.FirstOrDefault().Value;
						if (craftingProf != null)
						{
							list.Add(new KeyValuePair<int, Data.CraftingProfession>(crafting.Rating, craftingProf));
						}
					}
					return list;
				}
				return list;
			}
		}

		[DataMember]
		public RaceType Race
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _race;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _race, value, "Race");
			}
		}

		[DataMember]
		public ProfessionType Profession
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _profession;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _profession, value, "Profession");
			}
		}

		[DataMember]
		public SpecializationType Specialization
		{
			get
			{
				return _specialization;
			}
			set
			{
				SetProperty(ref _specialization, value, "Specialization");
			}
		}

		[DataMember]
		public DateTimeOffset Created
		{
			get
			{
				return _created;
			}
			set
			{
				SetProperty(ref _created, value, "Created");
			}
		}

		[DataMember]
		public DateTime LastModified
		{
			get
			{
				return _lastModified;
			}
			set
			{
				SetProperty(ref _lastModified, value, "LastModified");
			}
		}

		public int OrderIndex { get; set; }

		public int OrderOffset { get; set; }

		[DataMember]
		public DateTime LastLogin
		{
			get
			{
				return _lastLogin.AddMilliseconds(-OrderOffset);
			}
			set
			{
				SetProperty(ref _lastLogin, value, "LastLogin");
			}
		}

		[DataMember]
		public string IconPath
		{
			get
			{
				return _iconPath;
			}
			set
			{
				_iconPath = value;
				_icon = null;
				_pathChecked = false;
			}
		}

		public string MapName
		{
			get
			{
				if (_data == null || _data.Maps[Map] == null)
				{
					return string.Empty;
				}
				return _data.Maps[Map].Name;
			}
		}

		public string RaceName
		{
			get
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (_data == null)
				{
					return "Data not loaded.";
				}
				return _data.Races[Race].Name;
			}
		}

		public string ProfessionName
		{
			get
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (_data == null)
				{
					return "Data not loaded.";
				}
				return _data.Professions[Profession].Name;
			}
		}

		public string SpecializationName
		{
			get
			{
				if (_data == null || Specialization == SpecializationType.None || !Enum.IsDefined(typeof(SpecializationType), Specialization))
				{
					return ProfessionName;
				}
				return _data.Specializations[Specialization].Name;
			}
		}

		public AsyncTexture2D ProfessionIcon => _data?.Professions[Profession].IconBig;

		public AsyncTexture2D SpecializationIcon
		{
			get
			{
				if (_data == null || Specialization == SpecializationType.None || !Enum.IsDefined(typeof(SpecializationType), Specialization))
				{
					return ProfessionIcon;
				}
				return _data.Specializations[Specialization].IconBig;
			}
		}

		public AsyncTexture2D Icon
		{
			get
			{
				if (!_pathChecked)
				{
					string path = ModulePath + (IconPath ?? string.Empty);
					if (IconPath != null && File.Exists(path))
					{
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
						{
							_icon = AsyncTexture2D.op_Implicit(TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(ModulePath + IconPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));
						});
					}
					_pathChecked = true;
				}
				return _icon ?? SpecializationIcon;
			}
			set
			{
				_icon = value;
				OnUpdated(save: false);
			}
		}

		public bool HasDefaultIcon => Icon == SpecializationIcon;

		[DataMember]
		public bool Show
		{
			get
			{
				return _show;
			}
			set
			{
				SetProperty(ref _show, value, "Show");
			}
		}

		[DataMember]
		public TagList Tags { get; private set; } = new TagList();


		[DataMember]
		public int Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				Save();
			}
		}

		[DataMember]
		public int Index
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
				Save();
			}
		}

		[DataMember]
		public Gender Gender
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _gender;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_gender = value;
				Save();
			}
		}

		public int Age
		{
			get
			{
				DateTimeOffset today = DateTimeOffset.UtcNow;
				int age = today.Year - Created.Year;
				if (Created.Date > today.AddYears(-age))
				{
					age--;
				}
				return age;
			}
		}

		[DataMember]
		public bool HadBirthday
		{
			get
			{
				return _hadBirthday;
			}
			set
			{
				SetProperty(ref _hadBirthday, value, "HadBirthday");
			}
		}

		public bool HasBirthdayPresent
		{
			get
			{
				if (HadBirthday)
				{
					return true;
				}
				for (int i = 1; i < 100; i++)
				{
					DateTime birthDay = Created.AddYears(i).DateTime;
					if (birthDay <= DateTime.UtcNow)
					{
						if (birthDay > LastLogin)
						{
							HadBirthday = true;
							return true;
						}
						continue;
					}
					return false;
				}
				return false;
			}
		}

		public string ModulePath { get; private set; }

		[DataMember]
		public bool ShowOnRadial
		{
			get
			{
				return _showOnRadial;
			}
			set
			{
				SetProperty(ref _showOnRadial, value, "ShowOnRadial");
			}
		}

		public event EventHandler Updated;

		public event EventHandler Deleted;

		public Character_Model()
		{
		}

		public Character_Model(Character character, CharacterSwapping characterSwapping, string modulePath, Action requestSave, ObservableCollection<Character_Model> characterModels, Data data)
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected I4, but got Unknown
			_characterSwapping = characterSwapping;
			ModulePath = modulePath;
			_requestSave = requestSave;
			_characterModels = characterModels;
			_data = data;
			_name = character.get_Name();
			_level = character.get_Level();
			_race = (RaceType)Enum.Parse(typeof(RaceType), character.get_Race());
			_profession = (ProfessionType)Enum.Parse(typeof(ProfessionType), character.get_Profession());
			_specialization = SpecializationType.None;
			_created = character.get_Created();
			_lastModified = character.get_LastModified().UtcDateTime;
			_lastLogin = ((_lastLogin > character.get_LastModified().UtcDateTime) ? _lastLogin : character.get_LastModified().UtcDateTime);
			_gender = ApiEnum<Gender>.op_Implicit(character.get_Gender());
			foreach (CharacterCraftingDiscipline disc in character.get_Crafting().ToList())
			{
				Crafting.Add(new CharacterCrafting
				{
					Id = (int)disc.get_Discipline().get_Value(),
					Rating = disc.get_Rating(),
					Active = disc.get_Active()
				});
			}
			_initialized = true;
		}

		public Character_Model(Character_Model character, CharacterSwapping characterSwapping, string modulePath, Action requestSave, ObservableCollection<Character_Model> characterModels, Data data)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			_characterSwapping = characterSwapping;
			ModulePath = modulePath;
			_requestSave = requestSave;
			_characterModels = characterModels;
			_data = data;
			_name = character.Name;
			_level = character.Level;
			_map = character.Map;
			Crafting = character.Crafting;
			_race = character.Race;
			_profession = character.Profession;
			_specialization = character.Specialization;
			_created = character.Created;
			_lastModified = character.LastModified;
			_lastLogin = character.LastLogin;
			_iconPath = character.IconPath;
			_show = character.Show;
			Tags = character.Tags;
			_position = character.Position;
			_index = character.Index;
			_gender = character.Gender;
			_showOnRadial = character.ShowOnRadial;
			_hadBirthday = character.HadBirthday;
			_initialized = true;
		}

		public void Delete()
		{
			this.Deleted?.Invoke(null, null);
			_characterModels.Remove(this);
			Save();
		}

		protected bool SetProperty<T>(ref T property, T newValue, [CallerMemberName] string caller = "")
		{
			if (object.Equals(property, newValue))
			{
				return false;
			}
			property = newValue;
			if (_initialized && caller != "LastLogin" && caller != "LastModified" && caller != "LastLogin")
			{
				OnUpdated();
			}
			return true;
		}

		public void UpdateTags(TagList tags)
		{
			for (int i = Tags.Count - 1; i >= 0; i--)
			{
				if (!tags.Contains(Tags[i]))
				{
					Tags.RemoveAt(i);
				}
			}
			OnUpdated();
		}

		public void AddTag(string tag)
		{
			Tags.Add(tag);
			OnUpdated();
		}

		public void RemoveTag(string tag)
		{
			if (Tags.Remove(tag))
			{
				OnUpdated();
			}
		}

		public (string, int, int, int, bool) NameMatches(string name)
		{
			List<(string, int, int, int, bool)> distances = new List<(string, int, int, int, bool)>();
			foreach (Character_Model c in _characterModels)
			{
				int distance = name.LevenshteinDistance(c.Name);
				int listDistance = c.Position.Difference(Position);
				if (listDistance < 5)
				{
					distances.Add((c.Name, distance, listDistance, listDistance + distance, c.Name == Name));
				}
			}
			distances.Sort(((string, int, int, int, bool) a, (string, int, int, int, bool) b) => a.Item4.CompareTo(b.Item4));
			(string, int, int, int, bool)? bestMatch = distances?.FirstOrDefault();
			if (bestMatch.HasValue && bestMatch.HasValue)
			{
				foreach (var match in distances.Where(((string, int, int, int, bool) e) => e.Item4 == bestMatch.Value.Item4))
				{
					if (match.Item1 == Name)
					{
						return match;
					}
				}
			}
			return bestMatch.Value;
		}

		public void Save()
		{
			if (_initialized)
			{
				_requestSave?.Invoke();
			}
		}

		public void Swap(bool ignoreOCR = false)
		{
			Save();
			_characterSwapping?.Start(this, ignoreOCR);
		}

		public void UpdateCharacter(PlayerCharacter character = null)
		{
			if (character == null)
			{
				character = GameService.Gw2Mumble.get_PlayerCharacter();
			}
			if (character != null && character.get_Name() == Name)
			{
				Specialization = (SpecializationType)character.get_Specialization();
				Map = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				LastLogin = DateTime.UtcNow;
			}
		}

		public void UpdateCharacter(Character character)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected I4, but got Unknown
			_name = character.get_Name();
			_level = character.get_Level();
			_race = (RaceType)Enum.Parse(typeof(RaceType), character.get_Race());
			_profession = (ProfessionType)Enum.Parse(typeof(ProfessionType), character.get_Profession());
			_specialization = SpecializationType.None;
			_created = character.get_Created();
			_lastModified = character.get_LastModified().UtcDateTime;
			_lastLogin = ((_lastLogin > character.get_LastModified().UtcDateTime) ? _lastLogin : character.get_LastModified().UtcDateTime);
			_gender = ApiEnum<Gender>.op_Implicit(character.get_Gender());
			foreach (CharacterCraftingDiscipline disc in character.get_Crafting().ToList())
			{
				CharacterCrafting craft = Crafting.Find((CharacterCrafting e) => e.Id == (int)disc.get_Discipline().get_Value());
				bool num = craft != null;
				if (craft == null)
				{
					craft = new CharacterCrafting();
				}
				craft.Id = (int)disc.get_Discipline().get_Value();
				craft.Rating = disc.get_Rating();
				craft.Active = disc.get_Active();
				if (!num)
				{
					Crafting.Add(craft);
				}
			}
		}

		private void OnUpdated(bool save = true)
		{
			this.Updated?.Invoke(this, EventArgs.Empty);
			if (save)
			{
				Save();
			}
		}
	}
}
