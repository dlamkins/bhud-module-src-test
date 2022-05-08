using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Flurl.Http;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementService : IAchievementService, IDisposable
	{
		private readonly ContentsManager contentsManager;

		private readonly Gw2ApiManager gw2ApiManager;

		private readonly Logger logger;

		private Task trackAchievementProgressTask;

		private CancellationTokenSource trackAchievementProgressCancellationTokenSource;

		public IEnumerable<AccountAchievement> PlayerAchievements { get; private set; }

		public IReadOnlyList<AchievementTableEntry> Achievements { get; private set; }

		public IReadOnlyList<CollectionAchievementTable> AchievementDetails { get; private set; }

		public IEnumerable<AchievementGroup> AchievementGroups { get; private set; }

		public IEnumerable<AchievementCategory> AchievementCategories { get; private set; }

		public event Action PlayerAchievementsLoaded;

		public event Action ApiAchievementsLoaded;

		public AchievementService(ContentsManager contentsManager, Gw2ApiManager gw2ApiManager, Logger logger)
		{
			this.contentsManager = contentsManager;
			this.gw2ApiManager = gw2ApiManager;
			this.logger = logger;
		}

		public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			logger.Info("Reading saved achievement information");
			JsonSerializerOptions val = new JsonSerializerOptions();
			val.get_Converters().Add((JsonConverter)(object)new RewardConverter());
			val.get_Converters().Add((JsonConverter)(object)new AchievementTableEntryDescriptionConverter());
			val.get_Converters().Add((JsonConverter)(object)new CollectionAchievementTableEntryConverter());
			JsonSerializerOptions serializerOptions = val;
			try
			{
				using (Stream stream = contentsManager.GetFileStream("achievement_data.json"))
				{
					Achievements = (await JsonSerializer.DeserializeAsync<List<AchievementTableEntry>>(stream, serializerOptions, cancellationToken)).AsReadOnly();
				}
				using Stream stream = contentsManager.GetFileStream("achievement_tables.json");
				AchievementDetails = (await JsonSerializer.DeserializeAsync<List<CollectionAchievementTable>>(stream, serializerOptions, default(CancellationToken))).AsReadOnly();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on deserializing cached achievement data!");
				throw;
			}
			logger.Info("Finished reading saved achievement information");
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

		public AsyncTexture2D GetImage(string imageUrl, Action beforeSwap)
		{
			return GetImageInternal(async () => await GeneratedExtensions.GetStreamAsync(DownloadWikiContent(imageUrl), default(CancellationToken), (HttpCompletionOption)0), beforeSwap);
		}

		public async Task<string> GetDirectImageLink(string imagePath, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (imagePath.Contains("File:"))
			{
				try
				{
					string obj = await GeneratedExtensions.GetStringAsync(DownloadWikiContent(imagePath), cancellationToken, (HttpCompletionOption)0);
					int fillImageStartIndex = obj.IndexOf("fullImageLink");
					int hrefStartIndex = obj.IndexOf("href=", fillImageStartIndex);
					int linkStartIndex = obj.IndexOf("\"", hrefStartIndex) + 1;
					int linkEndIndex = obj.IndexOf("\"", linkStartIndex);
					return obj.Substring(linkStartIndex, linkEndIndex - linkStartIndex);
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Exception occured on parsing an image path");
					return string.Empty;
				}
			}
			return imagePath;
		}

		public AsyncTexture2D GetImageFromIndirectLink(string imagePath, Action beforeSwap)
		{
			return GetImageInternal(async delegate
			{
				string url = await GetDirectImageLink(imagePath);
				return await GeneratedExtensions.GetStreamAsync(DownloadWikiContent(url), default(CancellationToken), (HttpCompletionOption)0);
			}, beforeSwap);
		}

		private AsyncTexture2D GetImageInternal(Func<Task<Stream>> getImageStream, Action beforeSwap)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			AsyncTexture2D texture = new AsyncTexture2D(Textures.get_TransparentPixel());
			Stream imageStream;
			Task.Run(async delegate
			{
				try
				{
					imageStream = await getImageStream();
					beforeSwap();
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
					{
						texture.SwapTexture(TextureUtil.FromStreamPremultiplied(device, imageStream));
						imageStream.Close();
					});
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Exception occured on downloading/swapping image");
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						texture.SwapTexture(Textures.get_Error());
					});
				}
			});
			return texture;
		}

		private IFlurlRequest DownloadWikiContent(string url)
		{
			return GeneratedExtensions.WithHeader("https://wiki.guildwars2.com" + url, "user-agent", (object)"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");
		}

		public void Dispose()
		{
			trackAchievementProgressCancellationTokenSource.Cancel();
		}
	}
}
