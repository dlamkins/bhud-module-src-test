using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
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

		public DataDictionary<int, Map> Maps { get; private set; }

		public DataDictionary<CraftingDisciplineType, CraftingProfession> CraftingProfessions { get; }

		public DataDictionary<Races, Race> Races { get; }

		public DataDictionary<int, Specialization> Specializations { get; }

		public DataDictionary<ProfessionType, Profession> Professions { get; }

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
					(CraftingDisciplineType)0,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)0,
						IconAssetId = 154983,
						MaxRating = 0,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Unbekannt"
							},
							{
								(Locale)0,
								"Unknown"
							},
							{
								(Locale)1,
								"Desconocido"
							},
							{
								(Locale)3,
								"Inconnu"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)1,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)1,
						IconAssetId = 102463,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Konstrukteur"
							},
							{
								(Locale)0,
								"Artificer"
							},
							{
								(Locale)1,
								"Artificiero"
							},
							{
								(Locale)3,
								"Artificier"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)2,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)2,
						IconAssetId = 102461,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Rüstungsschmied"
							},
							{
								(Locale)0,
								"Armorsmith"
							},
							{
								(Locale)1,
								"Forjador de armaduras"
							},
							{
								(Locale)3,
								"Forgeron d'armures"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)3,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)3,
						IconAssetId = 102465,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Chefkoch"
							},
							{
								(Locale)0,
								"Chef"
							},
							{
								(Locale)1,
								"Cocinero"
							},
							{
								(Locale)3,
								"Maître-queux"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)4,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)4,
						IconAssetId = 102458,
						MaxRating = 400,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Juwelier"
							},
							{
								(Locale)0,
								"Jeweler"
							},
							{
								(Locale)1,
								"Joyero"
							},
							{
								(Locale)3,
								"Bijoutier"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)5,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)5,
						IconAssetId = 102462,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Waidmann"
							},
							{
								(Locale)0,
								"Huntsman"
							},
							{
								(Locale)1,
								"Cazador"
							},
							{
								(Locale)3,
								"Chasseur"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)6,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)6,
						IconAssetId = 102464,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Lederer"
							},
							{
								(Locale)0,
								"Leatherworker"
							},
							{
								(Locale)1,
								"Peletero"
							},
							{
								(Locale)3,
								"Travailleur du cuir"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)7,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)7,
						IconAssetId = 1293677,
						MaxRating = 400,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Schreiber"
							},
							{
								(Locale)0,
								"Scribe"
							},
							{
								(Locale)1,
								"Escriba"
							},
							{
								(Locale)3,
								"Illustrateur"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)8,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)8,
						IconAssetId = 102459,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Schneider"
							},
							{
								(Locale)0,
								"Tailor"
							},
							{
								(Locale)1,
								"Sastre"
							},
							{
								(Locale)3,
								"Tailleur"
							}
						}
					}
				},
				{
					(CraftingDisciplineType)9,
					new CraftingProfession
					{
						Id = (CraftingDisciplineType)9,
						IconAssetId = 102460,
						MaxRating = 500,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Waffenschmied"
							},
							{
								(Locale)0,
								"Weaponsmith"
							},
							{
								(Locale)1,
								"Armero"
							},
							{
								(Locale)3,
								"Forgeron d'armes"
							}
						}
					}
				}
			};
			Races = new DataDictionary<Races, Race>(Path.Combine(paths.ModuleDataPath, "races.json"))
			{
				{
					Kenedia.Modules.Core.DataModels.Races.None,
					new Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.None,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Unbekannt"
							},
							{
								(Locale)0,
								"Unknown"
							},
							{
								(Locale)1,
								"Desconocido"
							},
							{
								(Locale)3,
								"Inconnu"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Asura,
					new Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Asura,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Asura"
							},
							{
								(Locale)0,
								"Asura"
							},
							{
								(Locale)1,
								"Asura"
							},
							{
								(Locale)3,
								"Asura"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Charr,
					new Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Charr,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Charr"
							},
							{
								(Locale)0,
								"Charr"
							},
							{
								(Locale)1,
								"Charr"
							},
							{
								(Locale)3,
								"Charr"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Human,
					new Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Human,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Mensch"
							},
							{
								(Locale)0,
								"Human"
							},
							{
								(Locale)1,
								"Humano"
							},
							{
								(Locale)3,
								"Humain"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Norn,
					new Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Norn,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Norn"
							},
							{
								(Locale)0,
								"Norn"
							},
							{
								(Locale)1,
								"Norn"
							},
							{
								(Locale)3,
								"Norn"
							}
						}
					}
				},
				{
					Kenedia.Modules.Core.DataModels.Races.Sylvari,
					new Race
					{
						Id = Kenedia.Modules.Core.DataModels.Races.Sylvari,
						Names = new LocalizedString
						{
							{
								(Locale)2,
								"Sylvari"
							},
							{
								(Locale)0,
								"Sylvari"
							},
							{
								(Locale)1,
								"Sylvari"
							},
							{
								(Locale)3,
								"Sylvaris"
							}
						}
					}
				}
			};
			Maps = new DataDictionary<int, Map>(Path.Combine(paths.ModuleDataPath, "maps.json"), new Func<Task>(UpdateMaps));
			Professions = new DataDictionary<ProfessionType, Profession>(Path.Combine(paths.ModuleDataPath, "professions.json"), new Func<Task>(UpdateProfessions));
			Specializations = new DataDictionary<int, Specialization>(Path.Combine(paths.ModuleDataPath, "specializations.json"), new Func<Task>(UpdateSpecializations));
		}

		public Map GetMapById(int id)
		{
			if (!Maps.ContainsKey(id))
			{
				return new Map
				{
					Name = "Unknown Map",
					Id = 0
				};
			}
			return Maps[id];
		}

		private async Task UpdateProfessions()
		{
			foreach (Profession prof in (IEnumerable<Profession>)(await ((IAllExpandableClient<Profession>)(object)Gw2ApiManager.Gw2ApiClient.get_V2().get_Professions()).AllAsync(default(CancellationToken))))
			{
				if (Enum.TryParse<ProfessionType>(prof.get_Id(), out ProfessionType professionType))
				{
					if (!Professions.ContainsKey(professionType))
					{
						Professions[professionType] = new Profession();
					}
					Professions[professionType].ApplyApiData(prof);
				}
			}
		}

		private async Task UpdateSpecializations()
		{
			foreach (Specialization spec in (IEnumerable<Specialization>)(await ((IAllExpandableClient<Specialization>)(object)Gw2ApiManager.Gw2ApiClient.get_V2().get_Specializations()).AllAsync(default(CancellationToken))))
			{
				if (spec.get_Elite())
				{
					if (!Specializations.ContainsKey(spec.get_Id()))
					{
						Specializations[spec.get_Id()] = new Specialization();
					}
					Specializations[spec.get_Id()].ApplyApiData(spec);
				}
			}
		}

		private async Task UpdateMaps()
		{
			foreach (Map map in (IEnumerable<Map>)(await ((IAllExpandableClient<Map>)(object)Gw2ApiManager.Gw2ApiClient.get_V2().get_Maps()).AllAsync(default(CancellationToken))))
			{
				if (!Maps.ContainsKey(map.get_Id()))
				{
					Maps[map.get_Id()] = new Map();
				}
				Maps[map.get_Id()].ApplyApiData(map);
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
			if ((newValue - 4 <= 1) ? true : false)
			{
				newValue = (Locale)0;
			}
			Profession profession = Professions.Values.LastOrDefault();
			if (profession == null || !profession.Names.TryGetValue(newValue, out var professionName) || string.IsNullOrEmpty(professionName))
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Updating Professions for locale {newValue}.");
				await Professions.Update();
				await Professions.Save();
			}
			Specialization specialization = Specializations.Values.LastOrDefault();
			if (specialization == null || !specialization.Names.TryGetValue(newValue, out var specializationName) || string.IsNullOrEmpty(specializationName))
			{
				BaseModule<Characters, MainWindow, Settings, PathCollection, Kenedia.Modules.Characters.Services.StaticHosting>.Logger.Info($"Updating Specializations for locale {newValue}.");
				await Specializations.Update();
				await Specializations.Save();
			}
			Map map = Maps.Values.LastOrDefault();
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
