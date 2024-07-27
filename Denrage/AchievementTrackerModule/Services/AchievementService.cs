using System;
using System.Collections.Generic;
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
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.Models;
using Flurl.Http;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementService : IAchievementService, IDisposable
	{
		private const string DataVersionUrl = "https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/version.json";

		private const string AchievementDataUrl = "https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/achievement_data.json";

		private const string AchievementTablesUrl = "https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/achievement_tables.json";

		private const string SubPagesUrl = "https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/subPages.json";

		private const string VersionFileName = "version.json";

		private const string AchievementDataFileName = "achievement_data.json";

		private const string AchievementTablesFileName = "achievement_tables.json";

		private const string SubPagesFileName = "subPages.json";

		private readonly ContentsManager contentsManager;

		private readonly Gw2ApiManager gw2ApiManager;

		private readonly Logger logger;

		private readonly DirectoriesManager directoriesManager;

		private readonly Func<IPersistanceService> getPersistanceService;

		private readonly ITextureService textureService;

		private Task trackAchievementProgressTask;

		private CancellationTokenSource trackAchievementProgressCancellationTokenSource;

		private readonly Dictionary<int, Func<int, int>> specialSnowflakeCompletedHandling = new Dictionary<int, Func<int, int>>
		{
			{
				5693,
				(int index) => index switch
				{
					5 => 8, 
					4 => 7, 
					3 => 6, 
					2 => 2, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5700,
				(int index) => index switch
				{
					3 => 8, 
					2 => 5, 
					1 => 2, 
					0 => 1, 
					_ => -1, 
				}
			},
			{
				5704,
				(int index) => index switch
				{
					3 => 8, 
					2 => 5, 
					1 => 2, 
					0 => 1, 
					_ => -1, 
				}
			},
			{
				5703,
				(int index) => index switch
				{
					6 => 7, 
					5 => 5, 
					4 => 4, 
					3 => 3, 
					2 => 2, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5697,
				(int index) => index switch
				{
					4 => 6, 
					3 => 5, 
					2 => 3, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5688,
				(int index) => index switch
				{
					4 => 8, 
					3 => 7, 
					2 => 6, 
					1 => 4, 
					0 => 3, 
					_ => -1, 
				}
			},
			{
				5709,
				(int index) => index switch
				{
					4 => 7, 
					3 => 6, 
					2 => 4, 
					1 => 2, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5698,
				(int index) => index switch
				{
					3 => 6, 
					2 => 4, 
					1 => 3, 
					0 => 0, 
					_ => -1, 
				}
			},
			{
				5691,
				(int index) => index switch
				{
					3 => 7, 
					2 => 6, 
					1 => 5, 
					0 => 4, 
					_ => -1, 
				}
			},
			{
				5708,
				(int index) => index switch
				{
					4 => 8, 
					3 => 5, 
					2 => 2, 
					1 => 1, 
					0 => 0, 
					_ => -1, 
				}
			}
		};

		public Dictionary<int, List<int>> ManualCompletedAchievements { get; set; } = new Dictionary<int, List<int>>();


		public IEnumerable<AccountAchievement> PlayerAchievements { get; private set; }

		public IReadOnlyList<AchievementTableEntry> Achievements { get; private set; }

		public IReadOnlyList<CollectionAchievementTable> AchievementDetails { get; private set; }

		public IEnumerable<AchievementGroup> AchievementGroups { get; private set; }

		public IEnumerable<AchievementCategory> AchievementCategories { get; private set; }

		public IReadOnlyList<SubPageInformation> Subpages { get; private set; }

		public event Action PlayerAchievementsLoaded;

		public event Action ApiAchievementsLoaded;

		public AchievementService(ContentsManager contentsManager, Gw2ApiManager gw2ApiManager, Logger logger, DirectoriesManager directoriesManager, Func<IPersistanceService> getPersistanceService, ITextureService textureService)
		{
			this.contentsManager = contentsManager;
			this.gw2ApiManager = gw2ApiManager;
			this.logger = logger;
			this.directoriesManager = directoriesManager;
			this.getPersistanceService = getPersistanceService;
			this.textureService = textureService;
		}

		public void ToggleManualCompleteStatus(int achievementId, int bit)
		{
			if (specialSnowflakeCompletedHandling.TryGetValue(achievementId, out var conversionFunc))
			{
				bit = conversionFunc(bit);
			}
			if (PlayerAchievements != null)
			{
				AccountAchievement achievement = PlayerAchievements.FirstOrDefault((AccountAchievement x) => x.get_Id() == achievementId);
				if (achievement != null && (achievement.get_Done() || achievement.get_Bits().Contains(bit)))
				{
					return;
				}
			}
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
			this.PlayerAchievementsLoaded?.Invoke();
		}

		public static string ByteArrayToString(byte[] ba)
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
			int tries = 0;
			do
			{
				if (tries == 3)
				{
					logger.Error("Couldn't download file, please download it manually! " + url);
					return false;
				}
				await url.DownloadFileAsync(folder, fileName);
				tries++;
			}
			while (!File.Exists(Path.Combine(folder, fileName)) || !CheckMd5(md5, Path.Combine(folder, fileName)));
			return true;
		}

		public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			logger.Info("Reading saved achievement information");
			JsonSerializerOptions serializerOptions = new JsonSerializerOptions
			{
				Converters = 
				{
					(JsonConverter)new RewardConverter(),
					(JsonConverter)new AchievementTableEntryDescriptionConverter(),
					(JsonConverter)new CollectionAchievementTableEntryConverter(),
					(JsonConverter)new SubPageInformationConverter()
				}
			};
			try
			{
				string dataFolder = directoriesManager.GetFullDirectoryPath("achievement_module");
				Directory.CreateDirectory(dataFolder);
				bool downloadData = false;
				if (!File.Exists(Path.Combine(dataFolder, "version.json")) || !File.Exists(Path.Combine(dataFolder, "achievement_data.json")) || !File.Exists(Path.Combine(dataFolder, "achievement_tables.json")) || !File.Exists(Path.Combine(dataFolder, "subPages.json")))
				{
					downloadData = true;
				}
				else
				{
					AchievementDataMetadata githubMetadata2 = await "https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/version.json".GetJsonAsync<AchievementDataMetadata>(default(CancellationToken), (HttpCompletionOption)0);
					using FileStream utf8Json = File.Open(Path.Combine(dataFolder, "version.json"), FileMode.Open);
					AchievementDataMetadata localMetadata = await JsonSerializer.DeserializeAsync<AchievementDataMetadata>(utf8Json, serializerOptions, cancellationToken);
					if (localMetadata.Version != -1 && (localMetadata.Version != githubMetadata2.Version || !CheckMd5(githubMetadata2.AchievementDataMd5, Path.Combine(dataFolder, "achievement_data.json")) || !CheckMd5(githubMetadata2.AchievementTablesMd5, Path.Combine(dataFolder, "achievement_tables.json")) || !CheckMd5(githubMetadata2.SubPagesMd5, Path.Combine(dataFolder, "subPages.json"))))
					{
						downloadData = true;
					}
				}
				if (downloadData)
				{
					logger.Info("Downloading AchievementData");
					await "https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/version.json".DownloadFileAsync(dataFolder, "version.json");
					using FileStream utf8Json = File.Open(Path.Combine(dataFolder, "version.json"), FileMode.Open);
					AchievementDataMetadata githubMetadata2 = await JsonSerializer.DeserializeAsync<AchievementDataMetadata>(utf8Json, serializerOptions, cancellationToken);
					bool flag = !(await DownloadFile("https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/achievement_data.json", dataFolder, "achievement_data.json", githubMetadata2.AchievementDataMd5));
					if (!flag)
					{
						flag = !(await DownloadFile("https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/achievement_tables.json", dataFolder, "achievement_tables.json", githubMetadata2.AchievementTablesMd5));
					}
					bool flag2 = flag;
					if (!flag2)
					{
						flag2 = !(await DownloadFile("https://bhm.blishhud.com/Denrage.AchievementTrackerModule/data/subPages.json", dataFolder, "subPages.json", githubMetadata2.SubPagesMd5));
					}
					if (flag2)
					{
						return;
					}
				}
				using (FileStream utf8Json = File.Open(Path.Combine(dataFolder, "achievement_data.json"), FileMode.Open))
				{
					Achievements = (await JsonSerializer.DeserializeAsync<List<AchievementTableEntry>>(utf8Json, serializerOptions, cancellationToken)).AsReadOnly();
				}
				using (FileStream utf8Json = File.Open(Path.Combine(dataFolder, "achievement_tables.json"), FileMode.Open))
				{
					AchievementDetails = (await JsonSerializer.DeserializeAsync<List<CollectionAchievementTable>>(utf8Json, serializerOptions)).AsReadOnly();
				}
				using FileStream utf8Json = File.Open(Path.Combine(dataFolder, "subPages.json"), FileMode.Open);
				Subpages = (await JsonSerializer.DeserializeAsync<List<SubPageInformation>>(utf8Json, serializerOptions)).AsReadOnly();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on deserializing cached achievement data!");
				throw;
			}
			logger.Info("Finished reading saved achievement information");
			ManualCompletedAchievements = getPersistanceService().Get().ManualCompletedAchievements;
			Task.Run(async delegate
			{
				await InitializeApiAchievements();
			});
			await LoadPlayerAchievements(forceRefresh: false, cancellationToken);
		}

		private async Task InitializeApiAchievements(CancellationToken cancellationToken = default(CancellationToken))
		{
			logger.Info("Getting achievement data from api");
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
				logger.Info("Finished getting achievement data from api");
				this.ApiAchievementsLoaded?.Invoke();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed getting api achievements. Retrying in 5 minutes");
				await Task.Delay(TimeSpan.FromMinutes(5.0));
				Task.Run(async delegate
				{
					await InitializeApiAchievements();
				});
			}
		}

		public bool HasFinishedAchievement(int achievementId)
		{
			if (PlayerAchievements == null)
			{
				return false;
			}
			AccountAchievement achievement = PlayerAchievements.FirstOrDefault((AccountAchievement x) => x.get_Id() == achievementId);
			if (achievement != null)
			{
				return achievement.get_Done();
			}
			return false;
		}

		public bool HasFinishedAchievementBit(int achievementId, int positionIndex)
		{
			if (specialSnowflakeCompletedHandling.TryGetValue(achievementId, out var conversionFunc))
			{
				positionIndex = conversionFunc(positionIndex);
			}
			if (ManualCompletedAchievements.TryGetValue(achievementId, out var manualAchievement) && manualAchievement.Contains(positionIndex))
			{
				return true;
			}
			if (PlayerAchievements == null)
			{
				return false;
			}
			AccountAchievement achievement = PlayerAchievements.FirstOrDefault((AccountAchievement x) => x.get_Id() == achievementId);
			if (achievement != null)
			{
				return achievement.get_Bits()?.Contains(positionIndex) ?? false;
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
				logger.Info("Refreshing Player Achievements");
				try
				{
					PlayerAchievements = (IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Achievements()).GetAsync(cancellationToken));
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
						foreach (int bit in item.get_Bits())
						{
							if (achievementBits.Contains(bit))
							{
								achievementBits.Remove(bit);
							}
						}
					}
					Task.Run(delegate
					{
						this.PlayerAchievementsLoaded?.Invoke();
					}, cancellationToken);
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Exception occured during refresh of player achievements. Skipping this time.");
				}
				TrackAchievementProgress();
			}
			else
			{
				logger.Info("Permissions not granted");
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
			trackAchievementProgressCancellationTokenSource.Cancel();
		}
	}
}
