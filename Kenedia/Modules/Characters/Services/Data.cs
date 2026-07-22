using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Characters.Services
{
	public class Data : IDisposable
	{
		private const int MAX_LOADING_ATTEMPTS = 5;

		private bool _isDisposed;

		private DateTime _lastLoadingTry = DateTime.MinValue;

		private int _loadingAttempts;

		public bool IsLoaded { get; private set; }

		public bool StaticContentLoaded { get; private set; }

		public StaticInfo StaticInfo { get; set; }

		public ContentsManager ContentsManager { get; }

		public PathCollection Paths { get; }

		public Gw2ApiManager Gw2ApiManager { get; }

		public StaticHosting StaticHosting { get; }

		public DataDictionary<int, Kenedia.Modules.Core.DataModels.Map> Maps { get; private set; }

		public DataDictionary<CraftingDisciplineType, CraftingProfession> CraftingProfessions { get; }

		public DataDictionary<Races, Kenedia.Modules.Characters.Models.Race> Races { get; }

		public DataDictionary<int, Kenedia.Modules.Characters.Models.Specialization> Specializations { get; }

		public DataDictionary<ProfessionType, Kenedia.Modules.Characters.Models.Profession> Professions { get; }

		public event EventHandler<bool> Loaded;

		public event EventHandler<bool> BetaStateChanged;

		public Data(ContentsManager contentsManager, PathCollection paths, Gw2ApiManager gw2ApiManager, StaticHosting staticHosting)
		{
			ContentsManager = contentsManager;
			Paths = paths;
			Gw2ApiManager = gw2ApiManager;
			StaticHosting = staticHosting;
			CraftingProfessions = new DataDictionary<CraftingDisciplineType, CraftingProfession>(Path.Combine(paths.ModuleDataPath, "crafting_disciplines.json"))
			{
				{
					CraftingDisciplineType.Unknown,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Unknown,
						IconAssetId = 154983,
						MaxRating = 0,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Unbekannt"
							},
							{
								Locale.English,
								"Unknown"
							},
							{
								Locale.Spanish,
								"Desconocido"
							},
							{
								Locale.French,
								"Inconnu"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Artificer,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Artificer,
						IconAssetId = 102463,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Konstrukteur"
							},
							{
								Locale.English,
								"Artificer"
							},
							{
								Locale.Spanish,
								"Artificiero"
							},
							{
								Locale.French,
								"Artificier"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Armorsmith,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Armorsmith,
						IconAssetId = 102461,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Rüstungsschmied"
							},
							{
								Locale.English,
								"Armorsmith"
							},
							{
								Locale.Spanish,
								"Forjador de armaduras"
							},
							{
								Locale.French,
								"Forgeron d'armures"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Chef,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Chef,
						IconAssetId = 102465,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Chefkoch"
							},
							{
								Locale.English,
								"Chef"
							},
							{
								Locale.Spanish,
								"Cocinero"
							},
							{
								Locale.French,
								"Maître-queux"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Jeweler,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Jeweler,
						IconAssetId = 102458,
						MaxRating = 400,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Juwelier"
							},
							{
								Locale.English,
								"Jeweler"
							},
							{
								Locale.Spanish,
								"Joyero"
							},
							{
								Locale.French,
								"Bijoutier"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Huntsman,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Huntsman,
						IconAssetId = 102462,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Waidmann"
							},
							{
								Locale.English,
								"Huntsman"
							},
							{
								Locale.Spanish,
								"Cazador"
							},
							{
								Locale.French,
								"Chasseur"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Leatherworker,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Leatherworker,
						IconAssetId = 102464,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Lederer"
							},
							{
								Locale.English,
								"Leatherworker"
							},
							{
								Locale.Spanish,
								"Peletero"
							},
							{
								Locale.French,
								"Travailleur du cuir"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Scribe,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Scribe,
						IconAssetId = 1293677,
						MaxRating = 400,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Schreiber"
							},
							{
								Locale.English,
								"Scribe"
							},
							{
								Locale.Spanish,
								"Escriba"
							},
							{
								Locale.French,
								"Illustrateur"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Tailor,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Tailor,
						IconAssetId = 102459,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Schneider"
							},
							{
								Locale.English,
								"Tailor"
							},
							{
								Locale.Spanish,
								"Sastre"
							},
							{
								Locale.French,
								"Tailleur"
							}
						}
					}
				},
				{
					CraftingDisciplineType.Weaponsmith,
					new CraftingProfession
					{
						Id = CraftingDisciplineType.Weaponsmith,
						IconAssetId = 102460,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Waffenschmied"
							},
							{
								Locale.English,
								"Weaponsmith"
							},
							{
								Locale.Spanish,
								"Armero"
							},
							{
								Locale.French,
								"Forgeron d'armes"
							}
						}
					}
				}
			};
			Races = new DataDictionary<Races, Kenedia.Modules.Characters.Models.Race>(Path.Combine(paths.ModuleDataPath, "races.json"))
			{
				{
					Kenedia.Modules.Core.DataModels.Races.None,
					new Kenedia.Modules.Characters.Models.Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.None,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Unbekannt"
							},
							{
								Locale.English,
								"Unknown"
							},
							{
								Locale.Spanish,
								"Desconocido"
							},
							{
								Locale.French,
								"Inconnu"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Asura,
					new Kenedia.Modules.Characters.Models.Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Asura,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Asura"
							},
							{
								Locale.English,
								"Asura"
							},
							{
								Locale.Spanish,
								"Asura"
							},
							{
								Locale.French,
								"Asura"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Charr,
					new Kenedia.Modules.Characters.Models.Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Charr,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Charr"
							},
							{
								Locale.English,
								"Charr"
							},
							{
								Locale.Spanish,
								"Charr"
							},
							{
								Locale.French,
								"Charr"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Human,
					new Kenedia.Modules.Characters.Models.Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Human,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Mensch"
							},
							{
								Locale.English,
								"Human"
							},
							{
								Locale.Spanish,
								"Humano"
							},
							{
								Locale.French,
								"Humain"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Norn,
					new Kenedia.Modules.Characters.Models.Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Norn,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Norn"
							},
							{
								Locale.English,
								"Norn"
							},
							{
								Locale.Spanish,
								"Norn"
							},
							{
								Locale.French,
								"Norn"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Sylvari,
					new Kenedia.Modules.Characters.Models.Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Sylvari,
						Names = new LocalizedString
						{
							{
								Locale.German,
								"Sylvari"
							},
							{
								Locale.English,
								"Sylvari"
							},
							{
								Locale.Spanish,
								"Sylvari"
							},
							{
								Locale.French,
								"Sylvaris"
							}
						}
					}
				}
			};
			Maps = new DataDictionary<int, Kenedia.Modules.Core.DataModels.Map>(Path.Combine(paths.ModuleDataPath, "maps.json"), new Func<Task>(UpdateMaps));
			Professions = new DataDictionary<ProfessionType, Kenedia.Modules.Characters.Models.Profession>(Path.Combine(paths.ModuleDataPath, "professions.json"), new Func<Task>(UpdateProfessions));
			Specializations = new DataDictionary<int, Kenedia.Modules.Characters.Models.Specialization>(Path.Combine(paths.ModuleDataPath, "specializations.json"), new Func<Task>(UpdateSpecializations));
		}

		public Kenedia.Modules.Core.DataModels.Map GetMapById(int id)
		{
			if (!Maps.ContainsKey(id))
			{
				return new Kenedia.Modules.Core.DataModels.Map
				{
					Name = "Unknown Map",
					Id = 0
				};
			}
			return Maps[id];
		}

		private async Task UpdateProfessions()
		{
			foreach (Gw2Sharp.WebApi.V2.Models.Profession prof in await Gw2ApiManager.Gw2ApiClient.V2.Professions.AllAsync())
			{
				if (Enum.TryParse<ProfessionType>(prof.Id, out var professionType))
				{
					if (!Professions.ContainsKey(professionType))
					{
						Professions[professionType] = new Kenedia.Modules.Characters.Models.Profession();
					}
					Professions[professionType].ApplyApiData(prof);
				}
			}
		}

		private async Task UpdateSpecializations()
		{
			foreach (Gw2Sharp.WebApi.V2.Models.Specialization spec in await Gw2ApiManager.Gw2ApiClient.V2.Specializations.AllAsync())
			{
				if (spec.Elite)
				{
					if (!Specializations.ContainsKey(spec.Id))
					{
						Specializations[spec.Id] = new Kenedia.Modules.Characters.Models.Specialization();
					}
					Specializations[spec.Id].ApplyApiData(spec);
				}
			}
		}

		private async Task UpdateMaps()
		{
			foreach (Gw2Sharp.WebApi.V2.Models.Map map in await Gw2ApiManager.Gw2ApiClient.V2.Maps.AllAsync())
			{
				if (!Maps.ContainsKey(map.Id))
				{
					Maps[map.Id] = new Kenedia.Modules.Core.DataModels.Map();
				}
				Maps[map.Id].ApplyApiData(map);
			}
		}

		public async Task Load()
		{
			List<IDataDictionary> dataDictionaries = new List<IDataDictionary>(3) { Professions, Specializations, Maps };
			StaticHosting.Versions versions = null;
			try
			{
				if (StaticInfo != null)
				{
					StaticInfo.BetaStateChanged -= new EventHandler<bool>(StaticInfo_BetaStateChanged);
				}
				versions = await StaticHosting.GetStaticVersions();
				StaticInfo = await StaticHosting.GetStaticContent<StaticInfo>("static_info.json");
				StaticInfo.BetaStateChanged += new EventHandler<bool>(StaticInfo_BetaStateChanged);
				StaticContentLoaded = true;
			}
			catch (Exception ex2)
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Warn($"Failed to retrieve static versions. Use local data: {ex2}");
			}
			if (versions == null)
			{
				versions = new StaticHosting.Versions();
			}
			bool allLoaded = true;
			foreach (IDataDictionary dict in dataDictionaries)
			{
				try
				{
					string name = dict.FileName;
					bool loaded = await dict.Load();
					if (dict.IsOutdated(versions[name]))
					{
						BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info("Data dictionary '" + name + "' is outdated. Updating...");
						await dict.Update(versions[name]);
						await dict.Save();
						loaded = true;
						BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Data dictionary '{name}' updated to version {dict.Version}.");
					}
					allLoaded = allLoaded && loaded;
				}
				catch (Exception ex)
				{
					BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Warn($"Failed to load data dictionary from '{dict.FilePath}': {ex}");
					allLoaded = false;
				}
			}
			IsLoaded = allLoaded && StaticContentLoaded;
			OnIsLoaded();
		}

		private void StaticInfo_BetaStateChanged(object sender, bool e)
		{
			this.BetaStateChanged?.Invoke(this, e);
		}

		private void OnIsLoaded()
		{
			this.Loaded?.Invoke(this, IsLoaded);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (StaticInfo != null)
				{
					StaticInfo.BetaStateChanged -= new EventHandler<bool>(StaticInfo_BetaStateChanged);
				}
				Professions?.Clear();
				Specializations?.Clear();
				Maps?.Clear();
				Races?.Clear();
				CraftingProfessions?.Clear();
			}
		}

		public async Task UpdateLocale(Blish_HUD.ValueChangedEventArgs<Locale> eventArgs)
		{
			Locale newValue = eventArgs.NewValue;
			if (((uint)(newValue - 4) <= 1u) ? true : false)
			{
				newValue = Locale.English;
			}
			Kenedia.Modules.Characters.Models.Profession profession = Professions.Values.LastOrDefault();
			if (profession == null || !profession.Names.TryGetValue(newValue, out var professionName) || string.IsNullOrEmpty(professionName))
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Updating Professions for locale {newValue}.");
				await Professions.Update();
				await Professions.Save();
			}
			Kenedia.Modules.Characters.Models.Specialization specialization = Specializations.Values.LastOrDefault();
			if (specialization == null || !specialization.Names.TryGetValue(newValue, out var specializationName) || string.IsNullOrEmpty(specializationName))
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Updating Specializations for locale {newValue}.");
				await Specializations.Update();
				await Specializations.Save();
			}
			Kenedia.Modules.Core.DataModels.Map map = Maps.Values.LastOrDefault();
			if (map == null || !map.Names.TryGetValue(newValue, out var mapName) || string.IsNullOrEmpty(mapName))
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Updating Maps for locale {newValue}.");
				await Maps.Update();
				await Maps.Save();
				LocalizingService.OnLocaleChanged(this, eventArgs);
			}
		}

		public async void Update()
		{
			StaticInfo?.CheckBeta();
			if (!IsLoaded && _loadingAttempts < 5 && (DateTime.UtcNow - _lastLoadingTry).TotalSeconds > 60.0)
			{
				_loadingAttempts++;
				await Load();
				_lastLoadingTry = DateTime.UtcNow;
			}
		}
	}
}
