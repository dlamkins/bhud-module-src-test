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


		public List<Locale> LoadedLocales => (from x in Professions.Values.FirstOrDefault()?.Names
			where !string.IsNullOrEmpty(x.Value)
			select x.Key).ToList();

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

		public event EventHandler Loaded;

		public Data(Paths paths, Gw2ApiManager gw2ApiManager, Func<NotificationBadge> notificationBadge, Func<LoadingSpinner> spinner, StaticHosting staticHosting)
		{
			Paths = paths;
			Gw2ApiManager = gw2ApiManager;
			StaticHosting = staticHosting;
			_getSpinner = spinner;
			_getNotificationBadge = notificationBadge;
		}

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__90))]
		public IEnumerator<(string name, BaseMappedDataEntry map)> GetEnumerator()
		{
			return new _003CGetEnumerator_003Ed__90(0)
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

		public async Task<bool> Load(Locale locale)
		{
			return await Load(!LoadedLocales.Contains(locale));
		}

		public unsafe async Task<bool> Load()
		{
			if (Common.Now - LastLoadAttempt <= 180000.0)
			{
				return false;
			}
			LoadingSpinner spinner = Spinner;
			LastLoadAttempt = Common.Now;
			BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Loading data");
			try
			{
				_cancellationTokenSource?.Cancel();
				_cancellationTokenSource = new CancellationTokenSource();
				StaticVersion versions = await StaticHosting.GetStaticVersion();
				if (versions == null)
				{
					NotificationBadge badge2 = NotificationBadge;
					if (badge2 != null)
					{
						DateTime endTime3 = DateTime.Now.AddMinutes(3.0);
						badge2.AddNotification(new ConditionalNotification
						{
							NotificationText = $"Failed to get the version file. Retry at {DateTime.Now.AddMinutes(3.0):T}",
							Condition = () => DateTime.Now >= endTime3
						});
					}
					spinner?.Hide();
					return false;
				}
				StaticStats stats = await StaticHosting.GetStaticStats();
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Latest static stats version: " + (object)stats?.Version);
				if (stats == null)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Failed to get the stats file.");
					NotificationBadge badge3 = NotificationBadge;
					if (badge3 != null)
					{
						DateTime endTime2 = DateTime.Now.AddMinutes(3.0);
						badge3.AddNotification(new ConditionalNotification
						{
							NotificationText = $"Failed to get the stats file. Retry at {DateTime.Now.AddMinutes(3.0):T}",
							Condition = () => DateTime.Now >= endTime2
						});
					}
					spinner?.Hide();
					return false;
				}
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info($"Latest static stats version: {stats.Version}");
				StaticStats localStats = (File.Exists(Path.Combine(Paths.ModuleDataPath, "static_stats.json")) ? JsonConvert.DeserializeObject<StaticStats>(File.ReadAllText(Path.Combine(Paths.ModuleDataPath, "static_stats.json"))) : null);
				if (localStats == null || localStats.Version < stats.Version)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Updating local static stats file...");
					File.WriteAllText(Path.Combine(Paths.ModuleDataPath, "static_stats.json"), JsonConvert.SerializeObject((object)stats, (Formatting)1));
					string url = stats.ImageUrl;
					HttpClient client = new HttpClient();
					try
					{
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Downloading stat map image from " + url + "...");
						byte[] data = await client.GetByteArrayAsync(url);
						File.WriteAllBytes(Path.Combine(Paths.ModuleDataPath, "stat_map.png"), data);
					}
					finally
					{
						((IDisposable)client)?.Dispose();
					}
				}
				BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("Apply stat texture map...");
				Stat.StatTextureMap = stats.TextureMapInfo;
				List<int> itemIds = new List<int>();
				using (IEnumerator<(string name, BaseMappedDataEntry map)> enumerator = GetEnumerator())
				{
					AsyncTaskMethodBuilder<bool> asyncTaskMethodBuilder = default(AsyncTaskMethodBuilder<bool>);
					while (enumerator.MoveNext())
					{
						var (name2, map2) = enumerator.Current;
						if (!map2.GetType().IsGenericType || !(map2.GetType().GetGenericTypeDefinition() == typeof(ItemMappedDataEntry<>)))
						{
							continue;
						}
						Type innerType = map2.GetType().GetGenericArguments()[0];
						if (!typeof(BaseItem).IsAssignableFrom(innerType))
						{
							continue;
						}
						dynamic dynMap = map2;
						dynamic awaiter = dynMap.LoadAndGetPending(name2, versions[name2], Path.Combine(Paths.ModuleDataPath, name2 + ".json")).GetAwaiter();
						if (!(bool)awaiter.IsCompleted)
						{
							ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
							if (awaiter2 == null)
							{
								INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
								asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003CLoad_003Ed__94*)/*Error near IL_06d2: stateMachine*/);
							}
							else
							{
								asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003CLoad_003Ed__94*)/*Error near IL_06e5: stateMachine*/);
							}
							/*Error near IL_06ee: leave MoveNext - await not detected correctly*/;
						}
						object ids = awaiter.GetResult();
						List<int> idList = ids as List<int>;
						itemIds = ((idList != null) ? itemIds.Union(idList).ToList() : itemIds);
					}
				}
				List<List<int>> idSets = itemIds.ToList().ChunkBy(200);
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
				bool failed = false;
				string loadStatus = string.Empty;
				using (IEnumerator<(string name, BaseMappedDataEntry map)> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var (name, map) = enumerator.Current;
						if (_cancellationTokenSource.IsCancellationRequested)
						{
							return false;
						}
						string path = Path.Combine(Paths.ModuleDataPath, name + ".json");
						bool success = await map.LoadAndUpdate(name, versions[name], path, Gw2ApiManager, _cancellationTokenSource.Token);
						failed = failed || !success;
						if (failed)
						{
							loadStatus += string.Format("{0}{1}: {2} [{3} | {4}] ", Environment.NewLine, name, success, ((object)map?.Version)?.ToString() ?? "0.0.0", versions[name].Version);
						}
					}
				}
				if (!failed)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info("All data loaded!");
					GameService.Graphics.QueueMainThreadRender(delegate
					{
						this.Loaded?.Invoke(this, EventArgs.Empty);
					});
				}
				else
				{
					NotificationBadge badge = NotificationBadge;
					if (badge != null)
					{
						string txt = $"Failed to load some data. Click to retry.{Environment.NewLine}Automatic retry at {DateTime.Now.AddMinutes(3.0):T}{loadStatus}";
						DateTime endTime = DateTime.Now.AddMinutes(3.0);
						badge.AddNotification(new ConditionalNotification
						{
							NotificationText = txt,
							Condition = () => DateTime.Now >= endTime || IsLoaded
						});
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Info(txt);
					}
				}
				spinner?.Hide();
				return !failed;
			}
			catch
			{
			}
			return false;
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
