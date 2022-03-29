using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Characters.Strings;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters
{
	public static class DataManager
	{
		public class _Names
		{
			public string de { get; set; }

			public string en { get; set; }

			public string es { get; set; }

			public string fr { get; set; }
		}

		public class _jsonProfession
		{
			public _Names _Names = new _Names();

			public int Id { get; set; }

			public string API_Id { get; set; }

			public _Profession[] Load()
			{
				string path = "data\\professions.json";
				_Profession[] professions = new _Profession[1];
				string jsonString = new StreamReader(ContentsManager.GetFileStream(path)).ReadToEnd();
				if (jsonString != null && jsonString != "")
				{
					List<_jsonProfession>? list = JsonConvert.DeserializeObject<List<_jsonProfession>>(jsonString);
					professions = new _Profession[list.Aggregate((_jsonProfession i1, _jsonProfession i2) => (i1.Id <= i2.Id) ? i2 : i1).Id + 1];
					{
						foreach (_jsonProfession entry in list!)
						{
							professions[entry.Id] = new _Profession
							{
								_Names = entry._Names,
								API_Id = entry.API_Id,
								Id = entry.Id
							};
						}
						return professions;
					}
				}
				return professions;
			}
		}

		public class _jsonSpecialization
		{
			public _Names _Names = new _Names();

			public int Id { get; set; }

			public int API_Id { get; set; }

			public _Specialization[] Load()
			{
				string path = "data\\specialization.json";
				_Specialization[] specializations = new _Specialization[1];
				string jsonString = new StreamReader(ContentsManager.GetFileStream(path)).ReadToEnd();
				if (jsonString != null && jsonString != "")
				{
					List<_jsonSpecialization>? list = JsonConvert.DeserializeObject<List<_jsonSpecialization>>(jsonString);
					specializations = new _Specialization[list.Aggregate((_jsonSpecialization i1, _jsonSpecialization i2) => (i1.Id <= i2.Id) ? i2 : i1).Id + 1];
					{
						foreach (_jsonSpecialization entry in list!)
						{
							specializations[entry.Id] = new _Specialization
							{
								_Names = entry._Names,
								API_Id = entry.API_Id,
								Id = entry.Id
							};
						}
						return specializations;
					}
				}
				return specializations;
			}
		}

		public class _jsonRace
		{
			public _Names _Names = new _Names();

			public int Id { get; set; }

			public string API_Id { get; set; }

			public _Race[] Load()
			{
				string path = "data\\races.json";
				_Race[] races = new _Race[1];
				string jsonString = new StreamReader(ContentsManager.GetFileStream(path)).ReadToEnd();
				if (jsonString != null && jsonString != "")
				{
					List<_jsonRace>? list = JsonConvert.DeserializeObject<List<_jsonRace>>(jsonString);
					races = new _Race[list.Aggregate((_jsonRace i1, _jsonRace i2) => (i1.Id <= i2.Id) ? i2 : i1).Id + 1];
					{
						foreach (_jsonRace entry in list!)
						{
							races[entry.Id] = new _Race
							{
								_Names = entry._Names,
								API_Id = entry.API_Id,
								Id = entry.Id
							};
						}
						return races;
					}
				}
				return races;
			}
		}

		public class _jsonMap
		{
			public _Names _Names = new _Names();

			public int Id;

			public int API_Id;

			public IReadOnlyList<int> Floors;

			public int DefaultFloor;

			public int ContinentId;

			public _Map[] Load()
			{
				string path = "data\\maps.json";
				_Map[] maps = new _Map[1];
				string jsonString = new StreamReader(ContentsManager.GetFileStream(path)).ReadToEnd();
				if (jsonString != null && jsonString != "")
				{
					List<_jsonMap>? list = JsonConvert.DeserializeObject<List<_jsonMap>>(jsonString);
					maps = new _Map[list.Aggregate((_jsonMap i1, _jsonMap i2) => (i1.Id <= i2.Id) ? i2 : i1).Id + 1];
					{
						foreach (_jsonMap entry in list!)
						{
							maps[entry.Id] = new _Map
							{
								_Names = entry._Names,
								API_Id = entry.Id,
								Id = entry.Id
							};
						}
						return maps;
					}
				}
				return maps;
			}
		}

		public class _jsonCrafting
		{
			public _Names _Names = new _Names();

			public int Id { get; set; }

			public string API_Id { get; set; }

			public _Crafting[] Load()
			{
				string path = "data\\crafting_professions.json";
				_Crafting[] disciplines = new _Crafting[1];
				string jsonString = new StreamReader(ContentsManager.GetFileStream(path)).ReadToEnd();
				if (jsonString != null && jsonString != "")
				{
					List<_jsonCrafting>? list = JsonConvert.DeserializeObject<List<_jsonCrafting>>(jsonString);
					disciplines = new _Crafting[list.Aggregate((_jsonCrafting i1, _jsonCrafting i2) => (i1.Id <= i2.Id) ? i2 : i1).Id + 1];
					{
						foreach (_jsonCrafting entry in list!)
						{
							disciplines[entry.Id] = new _Crafting
							{
								_Names = entry._Names,
								API_Id = entry.Id,
								Id = entry.Id
							};
						}
						return disciplines;
					}
				}
				return disciplines;
			}
		}

		public class _Profession : _jsonProfession
		{
			public string Name
			{
				get
				{
					return GameService.Overlay.UserLocale.Value switch
					{
						Locale.German => _Names.de, 
						Locale.French => _Names.fr, 
						Locale.Spanish => _Names.es, 
						_ => _Names.en, 
					};
				}
				set
				{
					switch (GameService.Overlay.UserLocale.Value)
					{
					case Locale.German:
						_Names.de = value;
						break;
					case Locale.French:
						_Names.fr = value;
						break;
					case Locale.Spanish:
						_Names.es = value;
						break;
					default:
						_Names.en = value;
						break;
					}
				}
			}
		}

		public class _Specialization : _jsonSpecialization
		{
			public string Name
			{
				get
				{
					return GameService.Overlay.UserLocale.Value switch
					{
						Locale.German => _Names.de, 
						Locale.French => _Names.fr, 
						Locale.Spanish => _Names.es, 
						_ => _Names.en, 
					};
				}
				set
				{
					switch (GameService.Overlay.UserLocale.Value)
					{
					case Locale.German:
						_Names.de = value;
						break;
					case Locale.French:
						_Names.fr = value;
						break;
					case Locale.Spanish:
						_Names.es = value;
						break;
					default:
						_Names.en = value;
						break;
					}
				}
			}
		}

		public class _Race : _jsonRace
		{
			public string Name
			{
				get
				{
					return GameService.Overlay.UserLocale.Value switch
					{
						Locale.German => _Names.de, 
						Locale.French => _Names.fr, 
						Locale.Spanish => _Names.es, 
						_ => _Names.en, 
					};
				}
				set
				{
					switch (GameService.Overlay.UserLocale.Value)
					{
					case Locale.German:
						_Names.de = value;
						break;
					case Locale.French:
						_Names.fr = value;
						break;
					case Locale.Spanish:
						_Names.es = value;
						break;
					default:
						_Names.en = value;
						break;
					}
				}
			}
		}

		public class _Map : _jsonMap
		{
			public string Name
			{
				get
				{
					return GameService.Overlay.UserLocale.Value switch
					{
						Locale.German => _Names.de, 
						Locale.French => _Names.fr, 
						Locale.Spanish => _Names.es, 
						_ => _Names.en, 
					};
				}
				set
				{
					switch (GameService.Overlay.UserLocale.Value)
					{
					case Locale.German:
						_Names.de = value;
						break;
					case Locale.French:
						_Names.fr = value;
						break;
					case Locale.Spanish:
						_Names.es = value;
						break;
					default:
						_Names.en = value;
						break;
					}
				}
			}
		}

		public class _Crafting : _jsonMap
		{
			public string Name
			{
				get
				{
					return GameService.Overlay.UserLocale.Value switch
					{
						Locale.German => _Names.de, 
						Locale.French => _Names.fr, 
						Locale.Spanish => _Names.es, 
						_ => _Names.en, 
					};
				}
				set
				{
					switch (GameService.Overlay.UserLocale.Value)
					{
					case Locale.German:
						_Names.de = value;
						break;
					case Locale.French:
						_Names.fr = value;
						break;
					case Locale.Spanish:
						_Names.es = value;
						break;
					default:
						_Names.en = value;
						break;
					}
				}
			}
		}

		public static ContentsManager ContentsManager;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		public static _Profession[] _Professions { get; set; }

		public static _Specialization[] _Specializations { get; set; }

		public static _Race[] _Races { get; set; }

		public static _Map[] _Maps { get; set; }

		public static _Crafting[] _Craftings { get; set; }

		public static void Load()
		{
			_Professions = new _jsonProfession().Load();
			_Specializations = new _jsonSpecialization().Load();
			_Races = new _jsonRace().Load();
			_Maps = new _jsonMap().Load();
			_Craftings = new _jsonCrafting().Load();
		}

		public static string getMapName(int id)
		{
			if (_Maps.Length <= id || _Maps[id] == null)
			{
				return common.Unkown + " " + common.Map;
			}
			return _Maps[id].Name;
		}

		public static string getMapName(string id)
		{
			_Map[] maps = _Maps;
			foreach (_Map map in maps)
			{
				if (map != null && (map._Names.de == id || map._Names.en == id || map._Names.es == id || map._Names.fr == id))
				{
					return map.Name;
				}
			}
			return common.Unkown + common.Map;
		}

		public static string getRaceName(int id)
		{
			if (_Races.Length <= id || _Races[id] == null)
			{
				return common.Unkown + " " + common.Race;
			}
			return _Races[id].Name;
		}

		public static string getRaceName(string id)
		{
			_Race[] races = _Races;
			foreach (_Race race in races)
			{
				if (race != null && race.API_Id == id)
				{
					return race.Name;
				}
			}
			return common.Unkown + " " + common.Race;
		}

		public static string getSpecName(int id)
		{
			if (_Specializations.Length <= id || _Specializations[id] == null)
			{
				return common.Unkown + " " + common.Specialization;
			}
			return _Specializations[id].Name;
		}

		public static string getSpecName(string id)
		{
			_Specialization[] specializations = _Specializations;
			foreach (_Specialization spec in specializations)
			{
				if (spec != null && (spec._Names.de == id || spec._Names.en == id || spec._Names.es == id || spec._Names.fr == id))
				{
					return spec.Name;
				}
			}
			return common.Unkown + " " + common.Specialization;
		}

		public static string getProfessionName(int id)
		{
			if (_Professions.Length <= id || _Professions[id] == null)
			{
				return common.Unkown + " " + common.Profession;
			}
			return _Professions[id].Name;
		}

		public static string getProfessionName(string id)
		{
			_Profession[] professions = _Professions;
			foreach (_Profession spec in professions)
			{
				if (spec != null && spec.API_Id == id)
				{
					return spec.Name;
				}
			}
			return common.Unkown + " " + common.Profession;
		}

		public static string getCraftingName(string id)
		{
			return id switch
			{
				"Armorsmith" => common.Armorsmith, 
				"Artificer" => common.Artificer, 
				"Chef" => common.Chef, 
				"Huntsman" => common.Huntsman, 
				"Jeweler" => common.Jeweler, 
				"Leatherworker" => common.Leatherworker, 
				"Scribe" => common.Scribe, 
				"Tailor" => common.Tailor, 
				"Weaponsmith" => common.Weaponsmith, 
				_ => common.Unkown + " " + common.CraftingProfession, 
			};
		}

		public static string getCraftingName(int id)
		{
			if (_Craftings.Length <= id || _Craftings[id] == null)
			{
				return common.Unkown + " " + common.CraftingProfession;
			}
			return _Craftings[id].Name;
		}
	}
}
