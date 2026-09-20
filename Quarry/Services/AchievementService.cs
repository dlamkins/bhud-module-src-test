using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Debug;
using Blish_HUD.Modules.Managers;
using Flurl.Http;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class AchievementService : IAchievementService, IDisposable
	{
		private const string DataVersionUrl = "https://bhm.blishhud.com/ArranPell.Quarry/data/version.json";

		private const string AchievementDataUrl = "https://bhm.blishhud.com/ArranPell.Quarry/data/achievement_data.json";

		private const string AchievementTablesUrl = "https://bhm.blishhud.com/ArranPell.Quarry/data/achievement_tables.json";

		private const string VersionFileName = "version.json";

		private const string AchievementDataFileName = "achievement_data.json";

		private const string AchievementTablesFileName = "achievement_tables.json";

		private readonly ContentsManager contentsManager;

		private readonly Gw2ApiManager gw2ApiManager;

		private readonly Logger logger;

		private readonly DirectoriesManager directoriesManager;

		private readonly Func<IPersistenceService> getPersistenceService;

		private readonly ITextureService textureService;

		private readonly Func<IBitAlignmentService> getBitAlignmentService;

		private Task trackAchievementProgressTask;

		private bool permissionsWarningLogged;

		private bool loadRetryScheduled;

		private CancellationTokenSource trackAchievementProgressCancellationTokenSource;

		private readonly CancellationTokenSource apiAchievementsCancellationTokenSource = new CancellationTokenSource();

		private Lazy<Task<IReadOnlyList<CollectionAchievementTable>>> achievementDetailsLazy = new Lazy<Task<IReadOnlyList<CollectionAchievementTable>>>(() => Task.FromResult((IReadOnlyList<CollectionAchievementTable>)Array.Empty<CollectionAchievementTable>()));

		public Dictionary<int, List<int>> ManualCompletedAchievements { get; set; } = new Dictionary<int, List<int>>();


		public object ManualCompletedSync { get; } = new object();


		public IEnumerable<AccountAchievement> PlayerAchievements { get; private set; }

		public IReadOnlyDictionary<int, AccountAchievement> PlayerAchievementsById { get; private set; } = new Dictionary<int, AccountAchievement>();


		public IReadOnlyList<AchievementTableEntry> Achievements { get; private set; }

		public IReadOnlyDictionary<int, AchievementTableEntry> AchievementsById { get; private set; } = new Dictionary<int, AchievementTableEntry>();


		public DateTime AchievementTablesSnapshotDate { get; private set; }

		public IEnumerable<AchievementGroup> AchievementGroups { get; private set; }

		public IEnumerable<AchievementCategory> AchievementCategories { get; private set; }

		public event Action PlayerAchievementsLoaded;

		public event Action ApiAchievementsLoaded;

		public Task<IReadOnlyList<CollectionAchievementTable>> GetAchievementDetailsAsync()
		{
			return achievementDetailsLazy.Value;
		}

		public AchievementService(ContentsManager contentsManager, Gw2ApiManager gw2ApiManager, Logger logger, DirectoriesManager directoriesManager, Func<IPersistenceService> getPersistenceService, ITextureService textureService, Func<IBitAlignmentService> getBitAlignmentService)
		{
			this.contentsManager = contentsManager;
			this.gw2ApiManager = gw2ApiManager;
			this.logger = logger;
			this.directoriesManager = directoriesManager;
			this.getPersistenceService = getPersistenceService;
			this.textureService = textureService;
			this.getBitAlignmentService = getBitAlignmentService;
		}

		public void ToggleManualCompleteStatus(int achievementId, int bit)
		{
			bit = getBitAlignmentService().MapRowToBit(achievementId, bit);
			if (bit < 0 || (PlayerAchievementsById.TryGetValue(achievementId, out var achievement) && (achievement.get_Done() || (achievement.get_Bits()?.Contains(bit) ?? false))))
			{
				return;
			}
			lock (ManualCompletedSync)
			{
				if (!ManualCompletedAchievements.TryGetValue(achievementId, out var achievementBits))
				{
					achievementBits = new List<int>();
					ManualCompletedAchievements[achievementId] = achievementBits;
				}
				if (achievementBits.Contains(bit))
				{
					achievementBits.Remove(bit);
				}
				else
				{
					achievementBits.Add(bit);
				}
			}
			this.PlayerAchievementsLoaded?.Invoke();
		}

		private static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		}

		private bool CheckMd5(string md5ToCheck, string filePath)
		{
			using MD5 md5 = MD5.Create();
			using FileStream fileStream = File.Open(filePath, FileMode.Open);
			return md5ToCheck.Equals(ByteArrayToString(md5.ComputeHash(fileStream)), StringComparison.OrdinalIgnoreCase);
		}

		private async Task<bool> DownloadFile(string url, string folder, string fileName, string md5)
		{
			string tempName = fileName + ".tmp";
			string tempPath = Path.Combine(folder, tempName);
			string finalPath = Path.Combine(folder, fileName);
			for (int attempt = 1; attempt <= 3; attempt++)
			{
				await url.DownloadFileAsync(folder, tempName);
				if (File.Exists(tempPath) && CheckMd5(md5, tempPath))
				{
					File.Copy(tempPath, finalPath, overwrite: true);
					File.Delete(tempPath);
					return true;
				}
			}
			TryDeleteQuietly(tempPath);
			logger.Warn("Couldn't download " + url + ": three attempts, none matched the published md5.");
			return false;
		}

		private static void TryDeleteQuietly(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception)
			{
			}
		}

		private void ScheduleLoadRetry(CancellationToken cancellationToken)
		{
			if (loadRetryScheduled)
			{
				return;
			}
			loadRetryScheduled = true;
			Task.Run(async delegate
			{
				_ = 1;
				try
				{
					await Task.Delay(TimeSpan.FromMinutes(5.0), cancellationToken);
					logger.Info("Retrying the achievement data download.");
					await LoadAsync(cancellationToken);
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex)
				{
					logger.Warn(ex, "The achievement data download retry failed; not retrying again this session.");
				}
			}, cancellationToken);
		}

		public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			Stopwatch overallStopwatch = Stopwatch.StartNew();
			logger.Debug("Reading saved achievement information");
			JsonSerializerOptions serializerOptions = new JsonSerializerOptions
			{
				Converters = 
				{
					(JsonConverter)new RewardConverter(),
					(JsonConverter)new AchievementTableEntryDescriptionConverter(),
					(JsonConverter)new CollectionAchievementTableEntryConverter()
				}
			};
			string dataFolder = directoriesManager.GetFullDirectoryPath("quarry");
			Directory.CreateDirectory(dataFolder);
			bool hasCachedFiles = File.Exists(Path.Combine(dataFolder, "version.json")) && File.Exists(Path.Combine(dataFolder, "achievement_data.json")) && File.Exists(Path.Combine(dataFolder, "achievement_tables.json"));
			bool downloadData = !hasCachedFiles;
			AchievementDataMetadata githubMetadata2;
			if (hasCachedFiles)
			{
				githubMetadata2 = null;
				Stopwatch stageStopwatch3 = Stopwatch.StartNew();
				try
				{
					githubMetadata2 = await "https://bhm.blishhud.com/ArranPell.Quarry/data/version.json".GetJsonAsync<AchievementDataMetadata>(default(CancellationToken), (HttpCompletionOption)0);
					logger.Debug($"Startup timing: remote version.json GET took {stageStopwatch3.ElapsedMilliseconds} ms");
				}
				catch (Exception ex4)
				{
					logger.Warn(ex4, "Couldn't reach the wiki data server to check for updates; using the cached copy.");
				}
				if (githubMetadata2 != null)
				{
					try
					{
						using FileStream utf8Json = File.Open(Path.Combine(dataFolder, "version.json"), FileMode.Open);
						AchievementDataMetadata localMetadata = await JsonSerializer.DeserializeAsync<AchievementDataMetadata>(utf8Json, serializerOptions, cancellationToken);
						if (localMetadata == null)
						{
							throw new InvalidDataException("version.json deserialized to null.");
						}
						if (localMetadata.Version != -1)
						{
							if (localMetadata.Version != githubMetadata2.Version)
							{
								downloadData = true;
							}
							else
							{
								stageStopwatch3.Restart();
								bool num = CheckMd5(githubMetadata2.AchievementDataMd5, Path.Combine(dataFolder, "achievement_data.json"));
								logger.Debug($"Startup timing: achievement_data.json md5 check took {stageStopwatch3.ElapsedMilliseconds} ms");
								stageStopwatch3.Restart();
								bool achievementTablesFresh = CheckMd5(githubMetadata2.AchievementTablesMd5, Path.Combine(dataFolder, "achievement_tables.json"));
								logger.Debug($"Startup timing: achievement_tables.json md5 check took {stageStopwatch3.ElapsedMilliseconds} ms");
								if (!num || !achievementTablesFresh)
								{
									downloadData = true;
								}
							}
						}
					}
					catch (Exception ex3)
					{
						logger.Warn(ex3, "The cached version.json is unreadable; re-downloading the wiki data.");
						downloadData = true;
					}
				}
				githubMetadata2 = null;
			}
			if (downloadData)
			{
				logger.Info("Downloading AchievementData");
				try
				{
					string versionTempName = "version.json.tmp";
					string versionTempPath = Path.Combine(dataFolder, versionTempName);
					await "https://bhm.blishhud.com/ArranPell.Quarry/data/version.json".DownloadFileAsync(dataFolder, versionTempName);
					using (FileStream utf8Json = File.Open(versionTempPath, FileMode.Open))
					{
						githubMetadata2 = await JsonSerializer.DeserializeAsync<AchievementDataMetadata>(utf8Json, serializerOptions, cancellationToken);
					}
					Stopwatch stageStopwatch3 = Stopwatch.StartNew();
					bool achievementDataOk = await DownloadFile("https://bhm.blishhud.com/ArranPell.Quarry/data/achievement_data.json", dataFolder, "achievement_data.json", githubMetadata2.AchievementDataMd5);
					logger.Debug($"Startup timing: achievement_data.json download took {stageStopwatch3.ElapsedMilliseconds} ms");
					bool achievementTablesOk = achievementDataOk;
					if (achievementDataOk)
					{
						stageStopwatch3.Restart();
						achievementTablesOk = await DownloadFile("https://bhm.blishhud.com/ArranPell.Quarry/data/achievement_tables.json", dataFolder, "achievement_tables.json", githubMetadata2.AchievementTablesMd5);
						logger.Debug($"Startup timing: achievement_tables.json download took {stageStopwatch3.ElapsedMilliseconds} ms");
					}
					if (achievementDataOk && achievementTablesOk)
					{
						File.Copy(versionTempPath, Path.Combine(dataFolder, "version.json"), overwrite: true);
						TryDeleteQuietly(versionTempPath);
					}
					else
					{
						TryDeleteQuietly(versionTempPath);
						if (!hasCachedFiles)
						{
							logger.Error("Failed to download achievement data and no cached copy exists; the module cannot load achievement information. Retrying in 5 minutes.");
							ScheduleLoadRetry(cancellationToken);
							return;
						}
						logger.Warn("Failed to download fresh achievement data; continuing with the cached copy.");
					}
				}
				catch (Exception ex2)
				{
					if (ex2 is UnauthorizedAccessException)
					{
						Contingency.NotifyFileSaveAccessDenied(dataFolder, "cache Quarry's achievement data", false);
					}
					else if (!hasCachedFiles)
					{
						Contingency.NotifyHttpAccessDenied("download Quarry's achievement data from bhm.blishhud.com");
					}
					if (!hasCachedFiles)
					{
						logger.Error(ex2, "Failed to download achievement data and no cached copy exists; the module cannot load achievement information. Retrying in 5 minutes.");
						ScheduleLoadRetry(cancellationToken);
						return;
					}
					logger.Warn(ex2, "Failed to download fresh achievement data; continuing with the cached copy.");
				}
			}
			try
			{
				Stopwatch stageStopwatch3 = Stopwatch.StartNew();
				using (FileStream utf8Json = File.Open(Path.Combine(dataFolder, "achievement_data.json"), FileMode.Open))
				{
					Achievements = (await JsonSerializer.DeserializeAsync<List<AchievementTableEntry>>(utf8Json, serializerOptions, cancellationToken)).AsReadOnly();
				}
				logger.Debug($"Startup timing: achievement_data.json deserialize took {stageStopwatch3.ElapsedMilliseconds} ms");
				Dictionary<int, AchievementTableEntry> achievementsById = new Dictionary<int, AchievementTableEntry>();
				foreach (AchievementTableEntry achievement in Achievements)
				{
					achievementsById[achievement.Id] = achievement;
				}
				AchievementsById = achievementsById;
				string achievementTablesPath = Path.Combine(dataFolder, "achievement_tables.json");
				AchievementTablesSnapshotDate = File.GetLastWriteTimeUtc(achievementTablesPath);
				achievementDetailsLazy = new Lazy<Task<IReadOnlyList<CollectionAchievementTable>>>(async delegate
				{
					Stopwatch lazyStopwatch = Stopwatch.StartNew();
					using FileStream achievementDetails = File.Open(achievementTablesPath, FileMode.Open);
					ReadOnlyCollection<CollectionAchievementTable> result = (await JsonSerializer.DeserializeAsync<List<CollectionAchievementTable>>(achievementDetails, serializerOptions)).AsReadOnly();
					logger.Debug($"Startup timing: achievement_tables.json deserialize took {lazyStopwatch.ElapsedMilliseconds} ms (deferred)");
					return result;
				});
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on deserializing cached achievement data!");
				throw;
			}
			logger.Info(string.Format("Startup timing: achievement data ready in {0} ms ({1}); {2} achievements.", overallStopwatch.ElapsedMilliseconds, downloadData ? "downloaded" : "cached", Achievements.Count));
			ManualCompletedAchievements = getPersistenceService().Get().ManualCompletedAchievements;
			Stopwatch apiStopwatch = Stopwatch.StartNew();
			Task.Run(async delegate
			{
				await InitializeApiAchievements(apiAchievementsCancellationTokenSource.Token);
				logger.Debug($"Startup timing: AchievementCategories/AchievementGroups took {apiStopwatch.ElapsedMilliseconds} ms");
			});
			Stopwatch accountStopwatch = Stopwatch.StartNew();
			LoadPlayerAchievements(forceRefresh: false, cancellationToken).ContinueWith(delegate
			{
				logger.Debug($"Startup timing: Account.Achievements took {accountStopwatch.ElapsedMilliseconds} ms");
			}, cancellationToken);
		}

		private async Task InitializeApiAchievements(CancellationToken cancellationToken = default(CancellationToken))
		{
			logger.Debug("Getting achievement data from api");
			try
			{
				AchievementGroups = (IEnumerable<AchievementGroup>)(await ((IAllExpandableClient<AchievementGroup>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()
					.get_Groups()).AllAsync(cancellationToken));
				AchievementCategories = (IEnumerable<AchievementCategory>)(await ((IAllExpandableClient<AchievementCategory>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()
					.get_Categories()).AllAsync(cancellationToken));
				foreach (AchievementCategory category in AchievementCategories)
				{
					textureService.GetTexture(RenderUrl.op_Implicit(category.get_Icon()));
				}
				logger.Debug("Finished getting achievement data from api");
				this.ApiAchievementsLoaded?.Invoke();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "Failed getting api achievements. Retrying in 5 minutes");
				try
				{
					await Task.Delay(TimeSpan.FromMinutes(5.0), cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				Task.Run(async delegate
				{
					await InitializeApiAchievements(cancellationToken);
				}, cancellationToken);
			}
		}

		public bool HasFinishedAchievement(int achievementId)
		{
			if (PlayerAchievementsById.TryGetValue(achievementId, out var achievement) && achievement.get_Done())
			{
				return true;
			}
			return IsCompleteFromBits(achievementId, achievement);
		}

		private bool IsCompleteFromBits(int achievementId, AccountAchievement achievement)
		{
			if (!ManualCompletedAchievements.TryGetValue(achievementId, out var manualBits) || manualBits.Count == 0)
			{
				return false;
			}
			if (!getBitAlignmentService().TryGetCachedAchievement(achievementId, out var apiAchievement) || apiAchievement.get_Bits() == null || apiAchievement.get_Bits().Count == 0)
			{
				return false;
			}
			for (int bit = 0; bit < apiAchievement.get_Bits().Count; bit++)
			{
				if (!((achievement == null) ? null : achievement.get_Bits()?.Contains(bit)).GetValueOrDefault() && !manualBits.Contains(bit))
				{
					return false;
				}
			}
			return true;
		}

		public bool HasFinishedAchievementBit(int achievementId, int positionIndex)
		{
			int bitIndex = getBitAlignmentService().MapRowToBit(achievementId, positionIndex);
			if (bitIndex < 0)
			{
				return false;
			}
			if (ManualCompletedAchievements.TryGetValue(achievementId, out var manualAchievement) && manualAchievement.Contains(bitIndex))
			{
				return true;
			}
			if (PlayerAchievementsById.TryGetValue(achievementId, out var achievement))
			{
				return achievement.get_Bits()?.Contains(bitIndex) ?? false;
			}
			return false;
		}

		public bool HasFinishedBitIndex(int achievementId, int bit)
		{
			if (bit < 0)
			{
				return false;
			}
			if (ManualCompletedAchievements.TryGetValue(achievementId, out var manualAchievement) && manualAchievement.Contains(bit))
			{
				return true;
			}
			if (PlayerAchievementsById.TryGetValue(achievementId, out var achievement))
			{
				return achievement.get_Bits()?.Contains(bit) ?? false;
			}
			return false;
		}

		public async Task LoadPlayerAchievements(bool forceRefresh = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (!forceRefresh && PlayerAchievements != null)
			{
				return;
			}
			if (gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)6
			}))
			{
				logger.Debug("Refreshing Player Achievements");
				try
				{
					PlayerAchievements = (IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Achievements()).GetAsync(cancellationToken));
					PlayerAchievementsById = PlayerAchievements.ToDictionary((AccountAchievement a) => a.get_Id(), (AccountAchievement a) => a);
					lock (ManualCompletedSync)
					{
						foreach (AccountAchievement item in PlayerAchievements)
						{
							if (!ManualCompletedAchievements.TryGetValue(item.get_Id(), out var achievementBits))
							{
								continue;
							}
							if (item.get_Done())
							{
								ManualCompletedAchievements.Remove(item.get_Id());
								continue;
							}
							foreach (int bit in item.get_Bits() ?? Array.Empty<int>())
							{
								if (achievementBits.Contains(bit))
								{
									achievementBits.Remove(bit);
								}
							}
						}
					}
					Task.Run(delegate
					{
						try
						{
							this.PlayerAchievementsLoaded?.Invoke();
						}
						catch (Exception ex2)
						{
							logger.Error(ex2, "Exception occured in a PlayerAchievementsLoaded subscriber.");
						}
					}, cancellationToken);
				}
				catch (Exception ex)
				{
					logger.Warn(ex, "Exception occured during refresh of player achievements. Skipping this time.");
				}
				TrackAchievementProgress();
			}
			else if (!permissionsWarningLogged)
			{
				permissionsWarningLogged = true;
				logger.Warn("API key permissions 'account' and 'progression' not granted (yet): achievement progress is unavailable until they are. Normal for a moment at startup, before the subtoken arrives; a problem if it persists.");
			}
		}

		private void TrackAchievementProgress()
		{
			if (trackAchievementProgressTask == null)
			{
				trackAchievementProgressCancellationTokenSource = new CancellationTokenSource();
				trackAchievementProgressTask = Task.Run((Func<Task>)TrackAchievementProgressMethod);
			}
		}

		private async Task TrackAchievementProgressMethod()
		{
			_ = 1;
			try
			{
				while (true)
				{
					trackAchievementProgressCancellationTokenSource.Token.ThrowIfCancellationRequested();
					await Task.Delay(TimeSpan.FromMinutes(5.0), trackAchievementProgressCancellationTokenSource.Token);
					await LoadPlayerAchievements(forceRefresh: true, trackAchievementProgressCancellationTokenSource.Token);
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		public void Dispose()
		{
			trackAchievementProgressCancellationTokenSource?.Cancel();
			trackAchievementProgressCancellationTokenSource?.Dispose();
			apiAchievementsCancellationTokenSource.Cancel();
			apiAchievementsCancellationTokenSource.Dispose();
		}
	}
}
