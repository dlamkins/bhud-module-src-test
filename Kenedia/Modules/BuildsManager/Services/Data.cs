using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Attributes;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class Data : IDisposable, IEnumerable<(string name, BaseMappedDataEntry map)>, IEnumerable
	{
		public static readonly Dictionary<int, int?> SkinDictionary = new Dictionary<int, int?>
		{
			{ 30684, 5013 },
			{ 30687, 4997 },
			{ 30689, 4995 },
			{ 30690, 5022 },
			{ 30692, 5005 },
			{ 30696, 5018 },
			{ 30699, 5020 },
			{ 30691, 5164 },
			{ 30686, 5000 },
			{ 30685, 4998 },
			{ 30693, 5008 },
			{ 30694, 5021 },
			{ 30702, 5001 },
			{ 30700, 4992 },
			{ 30697, 4990 },
			{ 30688, 4994 },
			{ 30695, 4989 },
			{ 30698, 5019 },
			{ 30701, 5129 },
			{ 79895, 854 },
			{ 80384, 818 },
			{ 80435, 808 },
			{ 80254, 807 },
			{ 80205, 812 },
			{ 80277, 797 },
			{ 80557, 801 },
			{ 79838, 856 },
			{ 80296, 817 },
			{ 80145, 805 },
			{ 80578, 806 },
			{ 80161, 811 },
			{ 80252, 796 },
			{ 80281, 799 },
			{ 79873, 855 },
			{ 80248, 819 },
			{ 80131, 810 },
			{ 80190, 809 },
			{ 80111, 813 },
			{ 80356, 798 },
			{ 80399, 803 },
			{ 74155, 10161 },
			{ 92991, 1614376 },
			{ 81908, 1614709 },
			{ 91234, 1614682 }
		};

		private readonly Func<NotificationBadge> _getNotificationBadge;

		private readonly Func<LoadingSpinner> _getSpinner;

		private CancellationTokenSource _cancellationTokenSource;

		private bool _isDisposed;

		public NotificationBadge NotificationBadge
		{
			get
			{
				NotificationBadge badge = _getNotificationBadge?.Invoke();
				if (badge == null)
				{
					return null;
				}
				return badge;
			}
		}

		public LoadingSpinner Spinner
		{
			get
			{
				LoadingSpinner spinner = _getSpinner?.Invoke();
				if (spinner == null)
				{
					return null;
				}
				return spinner;
			}
		}

		public bool IsLoaded
		{
			get
			{
				using (IEnumerator<(string, BaseMappedDataEntry)> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Item2.IsLoaded)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public double LastLoadAttempt { get; private set; } = double.MinValue;


		[EnumeratorMember]
		public ProfessionDataEntry Professions { get; } = new ProfessionDataEntry();


		[EnumeratorMember]
		public RaceDataEntry Races { get; } = new RaceDataEntry();


		[EnumeratorMember]
		public PetDataEntry Pets { get; } = new PetDataEntry();


		[EnumeratorMember]
		public StatMappedDataEntry Stats { get; } = new StatMappedDataEntry();


		[EnumeratorMember]
		public PvpAmuletMappedDataEntry PvpAmulets { get; } = new PvpAmuletMappedDataEntry();


		[EnumeratorMember]
		public ItemMappedDataEntry<Armor> Armors { get; } = new ItemMappedDataEntry<Armor>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Weapon> Weapons { get; } = new ItemMappedDataEntry<Weapon>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Trinket> Trinkets { get; } = new ItemMappedDataEntry<Trinket>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Trinket> Backs { get; } = new ItemMappedDataEntry<Trinket>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Sigil> PvpSigils { get; } = new ItemMappedDataEntry<Sigil>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Sigil> PveSigils { get; } = new ItemMappedDataEntry<Sigil>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Rune> PvpRunes { get; } = new ItemMappedDataEntry<Rune>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Rune> PveRunes { get; } = new ItemMappedDataEntry<Rune>();


		[EnumeratorMember]
		public ItemMappedDataEntry<PowerCore> PowerCores { get; } = new ItemMappedDataEntry<PowerCore>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Relic> PveRelics { get; } = new ItemMappedDataEntry<Relic>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Relic> PvpRelics { get; } = new ItemMappedDataEntry<Relic>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Enhancement> Enhancements { get; } = new ItemMappedDataEntry<Enhancement>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Nourishment> Nourishments { get; } = new ItemMappedDataEntry<Nourishment>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Infusion> Infusions { get; } = new ItemMappedDataEntry<Infusion>();


		[EnumeratorMember]
		public ItemMappedDataEntry<Enrichment> Enrichments { get; } = new ItemMappedDataEntry<Enrichment>();


		public Paths Paths { get; }

		public Gw2ApiManager Gw2ApiManager { get; }

		public StaticHosting StaticHosting { get; }

		public ContentsManager ContentsManager { get; }

		public StaticStats? StatsMap { get; private set; }

		public StaticVersion? Versions { get; private set; }

		public event EventHandler Loaded;

		public Data(Paths paths, Gw2ApiManager gw2ApiManager, Func<NotificationBadge> notificationBadge, Func<LoadingSpinner> spinner, StaticHosting staticHosting, ContentsManager contentsManager)
		{
			Paths = paths;
			Gw2ApiManager = gw2ApiManager;
			StaticHosting = staticHosting;
			ContentsManager = contentsManager;
			_getSpinner = spinner;
			_getNotificationBadge = notificationBadge;
		}

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__99))]
		public IEnumerator<(string name, BaseMappedDataEntry map)> GetEnumerator()
		{
			return new _003CGetEnumerator_003Ed__99(0)
			{
				_003C_003E4__this = this
			};
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public async Task<bool> Load(bool force)
		{
			if (force)
			{
				LastLoadAttempt = double.MinValue;
			}
			return await Load();
		}

		public async Task<bool> Load(Locale _)
		{
			return await Load(force: true);
		}

		public async Task<bool> Load()
		{
			if (Common.Now - LastLoadAttempt <= 180000.0)
			{
				return false;
			}
			LoadingSpinner spinner = Spinner;
			LastLoadAttempt = Common.Now;
			BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Loading data");
			spinner?.Show();
			NotificationBadge?.Notifications?.Clear();
			try
			{
				_cancellationTokenSource?.Cancel();
				_cancellationTokenSource = new CancellationTokenSource();
				if (Versions == null)
				{
					Versions = await StaticHosting.GetStaticVersion();
				}
				if (StatsMap == null)
				{
					StatsMap = await StaticHosting.GetStaticStats();
				}
				DateTime endTime = DateTime.Now.AddMinutes(3.0);
				string error_text = ((Versions == null) ? $"Failed to get the version file. Using locally cached data only.\nRetry at {DateTime.Now.AddMinutes(3.0):T}" : ((StatsMap == null) ? $"Failed to get the stats file. Using locally cached data only.\nRetry at {DateTime.Now.AddMinutes(3.0):T}" : null));
				if (!string.IsNullOrEmpty(error_text))
				{
					NotificationBadge?.AddNotification(new ConditionalNotification
					{
						NotificationText = error_text,
						Condition = () => DateTime.Now >= endTime
					});
				}
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info((Versions == null) ? "Version file could not be fetched." : "Versions file fetched from static hosting.");
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info((StatsMap == null) ? "Stats file could not be fetched." : "Stats file fetched from static hosting.");
				List<string> files = new List<string>(2) { "stat_map.png", "static_stats.json" };
				List<string> list = new List<string>();
				list.AddRange(files);
				list.AddRange(this.Select<(string, BaseMappedDataEntry), string>(((string name, BaseMappedDataEntry map) e) => e.name + ".json"));
				files = list;
				foreach (string file in files)
				{
					string path3 = Path.Combine(Paths.ModuleDataPath, file);
					if (File.Exists(path3))
					{
						continue;
					}
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Exporting default data for " + file + "...");
					using Stream target = File.Create(path3);
					using Stream source = ContentsManager.GetFileStream("data\\default_api_data\\" + file);
					source.Seek(0L, SeekOrigin.Begin);
					source.CopyTo(target);
				}
				bool localDataLoaded = true;
				List<string> failedLocalParts = new List<string>();
				StaticStats localStats = (File.Exists(Path.Combine(Paths.ModuleDataPath, "static_stats.json")) ? JsonConvert.DeserializeObject<StaticStats>(File.ReadAllText(Path.Combine(Paths.ModuleDataPath, "static_stats.json"))) : null);
				bool statsImageExists = File.Exists(Path.Combine(BuildsManager.Data.Paths.ModuleDataPath, "stat_map.png"));
				if (localStats != null && statsImageExists)
				{
					Stat.StatTextureMap = localStats.TextureMapInfo;
				}
				using (IEnumerator<(string name, BaseMappedDataEntry map)> enumerator2 = GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						var (name2, map2) = enumerator2.Current;
						if (_cancellationTokenSource.IsCancellationRequested)
						{
							localDataLoaded = false;
							failedLocalParts.Add(name2);
						}
						else if (!map2.IsLoaded)
						{
							string path2 = Path.Combine(Paths.ModuleDataPath, name2 + ".json");
							bool success = await map2.LoadCached(name2, path2, _cancellationTokenSource.Token);
							localDataLoaded = localDataLoaded && success;
							if (!success)
							{
								failedLocalParts.Add(name2);
							}
						}
					}
				}
				if (!localDataLoaded)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Local data failed to load: " + string.Join(", ", failedLocalParts));
				}
				else
				{
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("All local data loaded successfully.");
				}
				bool remoteDataLoaded = true;
				List<string> remoteFailedParts = new List<string>();
				if (Versions != null && StatsMap != null)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Loading remote data...");
					if (localStats == null || localStats.Version < StatsMap!.Version)
					{
						File.WriteAllText(Path.Combine(Paths.ModuleDataPath, "static_stats.json"), JsonConvert.SerializeObject((object)StatsMap, (Formatting)1));
						string url = StatsMap!.ImageUrl;
						HttpClient client = new HttpClient();
						try
						{
							BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Downloading stat map image from " + url + "...");
							byte[] data = await client.GetByteArrayAsync(url);
							File.WriteAllBytes(Path.Combine(Paths.ModuleDataPath, "stat_map.png"), data);
							Stat.StatTextureMap = StatsMap!.TextureMapInfo;
						}
						finally
						{
							((IDisposable)client)?.Dispose();
						}
					}
					Locale value = GameService.Overlay.UserLocale.Value;
					bool flag = (((uint)(value - 4) <= 1u) ? true : false);
					Locale lang = ((!flag) ? GameService.Overlay.UserLocale.Value : Locale.English);
					await PreFetchItemIds(Versions);
					using (IEnumerator<(string name, BaseMappedDataEntry map)> enumerator2 = GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							var (name2, map) = enumerator2.Current;
							if (_cancellationTokenSource.IsCancellationRequested)
							{
								remoteDataLoaded = false;
								remoteFailedParts.Add(name2);
								continue;
							}
							string path = Path.Combine(Paths.ModuleDataPath, name2 + ".json");
							if (!(await map.IsIncomplete(name2, Versions![name2], path, Gw2ApiManager, _cancellationTokenSource.Token)).Item1)
							{
								continue;
							}
							if (_cancellationTokenSource.IsCancellationRequested)
							{
								remoteDataLoaded = false;
								remoteFailedParts.Add(name2);
								continue;
							}
							BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Updating data for " + name2 + " " + ((map.Version < Versions![name2].Version) ? $"from version {map.Version} to {Versions![name2].Version}" : $"for {lang} locale..."));
							bool updated = await map.Update(name2, Versions![name2], path, Gw2ApiManager, _cancellationTokenSource.Token);
							remoteDataLoaded = remoteDataLoaded && updated;
							if (!updated)
							{
								remoteFailedParts.Add(name2);
							}
						}
					}
					if (!remoteDataLoaded)
					{
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Remote data failed to load: " + string.Join(", ", remoteFailedParts));
					}
					else
					{
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("All remote data loaded successfully.");
					}
				}
				if (localDataLoaded || remoteDataLoaded)
				{
					GameService.Graphics.QueueMainThreadRender(delegate
					{
						this.Loaded?.Invoke(this, EventArgs.Empty);
					});
					spinner?.Hide();
					return true;
				}
				string txt = $"Failed to load local and remote data. Click to retry.{Environment.NewLine}Automatic retry at {endTime:T}" + Environment.NewLine + "Failed to load locally: " + string.Join(", ", failedLocalParts) + Environment.NewLine + "Failed to load remotely: " + string.Join(", ", remoteFailedParts);
				NotificationBadge?.AddNotification(new ConditionalNotification
				{
					NotificationText = txt,
					Condition = () => DateTime.Now >= endTime || IsLoaded
				});
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info(txt);
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info(ex, "An error occurred while loading data.");
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info(ex.Message);
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info(ex.InnerException.Message);
			}
			return false;
		}

		private async Task<bool> PreFetchItemIds(StaticVersion versions)
		{
			List<int> itemIds = new List<int>();
			using (IEnumerator<(string name, BaseMappedDataEntry map)> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var (name, map) = enumerator.Current;
					if (!map.GetType().IsGenericType || !(map.GetType().GetGenericTypeDefinition() == typeof(ItemMappedDataEntry<>)))
					{
						continue;
					}
					Type innerType = map.GetType().GetGenericArguments()[0];
					if (typeof(BaseItem).IsAssignableFrom(innerType))
					{
						var (isIncomplete, ids) = await map.IsIncomplete(name, versions[name], Path.Combine(Paths.ModuleDataPath, name + ".json"), Gw2ApiManager, _cancellationTokenSource.Token);
						if (ids.Any() && ids.All((object e) => e is int))
						{
							itemIds = (isIncomplete ? itemIds.Union(ids.Cast<int>()).ToList() : itemIds);
						}
					}
				}
			}
			List<List<int>> idSets = itemIds.ToList().ChunkBy(200);
			if (itemIds.Count > 0)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info($"Pre-Fetching {itemIds.Count} items from API...");
				int count = 0;
				foreach (List<int> idSet in idSets)
				{
					if (_cancellationTokenSource.IsCancellationRequested)
					{
						return false;
					}
					count += idSet.Count;
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info($"Fetching {count}/{itemIds.Count} items...");
					await Gw2ApiManager.Gw2ApiClient.V2.Items.ManyAsync(idSet);
				}
			}
			return true;
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			using IEnumerator<(string, BaseMappedDataEntry)> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Item2?.Dispose();
			}
		}
	}
}
