using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Attributes;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class Data : IDisposable
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

		public event EventHandler Loaded;

		public Data(Paths paths, Gw2ApiManager gw2ApiManager, Func<NotificationBadge> notificationBadge, Func<LoadingSpinner> spinner)
		{
			Paths = paths;
			Gw2ApiManager = gw2ApiManager;
			_getNotificationBadge = notificationBadge;
			_getSpinner = spinner;
		}

		public IEnumerator<(string name, BaseMappedDataEntry map)> GetEnumerator()
		{
			IEnumerable<PropertyInfo> propertiesToEnumerate = from property in GetType().GetProperties()
				where property.GetCustomAttribute<EnumeratorMemberAttribute>() != null
				select property;
			foreach (PropertyInfo property2 in propertiesToEnumerate)
			{
				yield return (property2.Name, property2.GetValue(this) as BaseMappedDataEntry);
			}
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

		public async Task<bool> Load()
		{
			if (Common.Now - LastLoadAttempt <= 180000.0)
			{
				return false;
			}
			LoadingSpinner spinner = Spinner;
			LastLoadAttempt = Common.Now;
			BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info("Loading data");
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
						DateTime endTime2 = DateTime.Now.AddMinutes(3.0);
						badge2.AddNotification(new ConditionalNotification
						{
							NotificationText = $"Failed to get the version file. Retry at {DateTime.Now.AddMinutes(3.0):T}",
							Condition = () => DateTime.Now >= endTime2
						});
					}
					spinner?.Hide();
					return false;
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
					BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info("All data loaded!");
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
							Condition = () => DateTime.Now >= endTime
						});
						BaseModule<BuildsManager, MainWindow, Settings, Kenedia.Modules.BuildsManager.Models.Paths>.Logger.Info(txt);
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
