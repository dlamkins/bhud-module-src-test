using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using CinemaModule.Models;
using Newtonsoft.Json;

namespace CinemaModule.Services
{
	public class PresetService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<PresetService>();

		private const string DefaultApiBaseUrl = "https://www.gw2opus.com/wp-json/cinemahud/v1";

		private const string ImageCacheSubfolder = "presets";

		private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10.0);

		private readonly HttpClient _httpClient;

		private readonly string _apiBaseUrl;

		private readonly ImageCacheService _imageCache;

		private PresetsResponse _cachedPresets;

		private bool _isLoaded;

		public IReadOnlyList<WorldLocationPresetData> WorldLocationPresets => _cachedPresets?.WorldLocations ?? new List<WorldLocationPresetData>();

		public IReadOnlyList<StreamPresetData> StreamPresets => _cachedPresets?.Streams ?? new List<StreamPresetData>();

		public IReadOnlyList<string> TwitchChannels => _cachedPresets?.TwitchChannels ?? new List<string>();

		public bool IsLoaded => _isLoaded;

		public event EventHandler PresetsLoaded;

		public PresetService(string cacheDirectory)
			: this(cacheDirectory, "https://www.gw2opus.com/wp-json/cinemahud/v1")
		{
		}

		public PresetService(string cacheDirectory, string apiBaseUrl)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_apiBaseUrl = apiBaseUrl;
			HttpClient val = new HttpClient();
			val.set_Timeout(RequestTimeout);
			_httpClient = val;
			string imageCacheDir = Path.Combine(cacheDirectory, "presets");
			_imageCache = new ImageCacheService(imageCacheDir, _httpClient);
		}

		public async Task LoadPresetsAsync()
		{
			_ = 1;
			try
			{
				Logger.Info("Loading presets from API...");
				HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "/config");
				if (!response.get_IsSuccessStatusCode())
				{
					Logger.Warn($"Failed to load presets from API: {response.get_StatusCode()}");
					return;
				}
				_cachedPresets = JsonConvert.DeserializeObject<PresetsResponse>(await response.get_Content().ReadAsStringAsync());
				_isLoaded = true;
				Logger.Info($"Loaded {WorldLocationPresets.Count} world locations, {StreamPresets.Count} streams, {TwitchChannels.Count} Twitch channels");
				LoadPresetImagesAsync();
				this.PresetsLoaded?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load presets from API");
			}
		}

		private async Task LoadPresetImagesAsync()
		{
			if (_cachedPresets?.WorldLocations != null && _cachedPresets.WorldLocations.Count != 0)
			{
				await Task.WhenAll(_cachedPresets.WorldLocations.Select(LoadImagesForPresetAsync));
				Logger.Debug($"Finished loading images for {_cachedPresets.WorldLocations.Count} world location presets");
			}
		}

		private async Task LoadImagesForPresetAsync(WorldLocationPresetData preset)
		{
			Task<AsyncTexture2D> avatarTask = _imageCache.GetImageAsync(preset.Id + "_avatar", preset.Avatar);
			Task<AsyncTexture2D> pictureTask = _imageCache.GetImageAsync(preset.Id + "_picture", preset.Picture);
			await Task.WhenAll<AsyncTexture2D>(avatarTask, pictureTask);
			preset.AvatarTexture = avatarTask.Result;
			preset.PictureTexture = pictureTask.Result;
		}

		public void Dispose()
		{
			_imageCache?.Dispose();
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
	}
}
