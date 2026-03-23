using System;
using System.Threading.Tasks;
using Blish_HUD;
using SongbookOfTyria.Models;
using SongbookOfTyria.Models.Api;

namespace SongbookOfTyria.Services
{
	public sealed class TabsCacheService
	{
		private static readonly Logger Logger = Logger.GetLogger<TabsCacheService>();

		private readonly ApiService _apiService;

		private TabsResponse _cachedTabsResponse;

		public bool HasCachedTabs
		{
			get
			{
				if (_cachedTabsResponse?.Tabs != null)
				{
					return _cachedTabsResponse.Tabs.Count > 0;
				}
				return false;
			}
		}

		public event EventHandler<TabsResponse> TabsLoaded;

		public TabsCacheService(ApiService apiService)
		{
			_apiService = apiService;
			InitializeAsync();
		}

		private async Task InitializeAsync()
		{
			await PreloadTabsFromApiAsync();
		}

		private async Task PreloadTabsFromApiAsync()
		{
			try
			{
				Logger.Info("Preloading tabs from API...");
				TabsResponse freshResponse = await _apiService.GetTabsAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (freshResponse != null)
				{
					_cachedTabsResponse = freshResponse;
					Logger.Info("Preloaded {TabCount} tabs from API", new object[1] { freshResponse.Tabs?.Count ?? 0 });
					this.TabsLoaded?.Invoke(this, freshResponse);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to preload tabs from API");
			}
		}

		public async Task RefreshTabsAsync()
		{
			_cachedTabsResponse = null;
			await PreloadTabsFromApiAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		public TabsResponse GetCachedTabs()
		{
			return _cachedTabsResponse;
		}

		public async Task<TabsResponse> GetTabsAsync(string type = "all", string beginner = "all", string search = null)
		{
			if (_cachedTabsResponse?.Tabs != null && _cachedTabsResponse.Tabs.Count > 0)
			{
				RefreshTabsInBackgroundAsync(type, beginner, search);
				return _cachedTabsResponse;
			}
			TabsResponse freshResponse = await _apiService.GetTabsAsync(type, beginner, search).ConfigureAwait(continueOnCapturedContext: false);
			if (freshResponse == null)
			{
				return _cachedTabsResponse;
			}
			_cachedTabsResponse = freshResponse;
			return freshResponse;
		}

		private async Task RefreshTabsInBackgroundAsync(string type, string beginner, string search)
		{
			try
			{
				TabsResponse freshResponse = await _apiService.GetTabsAsync(type, beginner, search).ConfigureAwait(continueOnCapturedContext: false);
				if (freshResponse != null)
				{
					_cachedTabsResponse = freshResponse;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Background refresh failed");
			}
		}

		public async Task<MusicTab> GetTabDetailsAsync(MusicTab basicTab)
		{
			if (basicTab == null || string.IsNullOrEmpty(basicTab.ApiUrl))
			{
				return basicTab;
			}
			Logger.Debug("GetTabDetailsAsync: Fetching fresh data from API for tab {0} (Id: {1}): {2}", new object[3] { basicTab.Name, basicTab.Id, basicTab.ApiUrl });
			MusicTab freshTab = await _apiService.GetTabByUrlAsync(basicTab.ApiUrl);
			if (freshTab == null)
			{
				Logger.Warn("GetTabDetailsAsync: API returned null, falling back to basic tab data");
				return basicTab;
			}
			Logger.Debug("GetTabDetailsAsync: Got fresh data (LastUpdated: {0}, HasNotation: {1})", new object[2]
			{
				freshTab.LastUpdated,
				!string.IsNullOrEmpty(freshTab.NotationBlishhud)
			});
			return freshTab;
		}

		public void ClearInMemoryCache()
		{
			_cachedTabsResponse = null;
			Logger.Info("In-memory cache cleared");
		}
	}
}
