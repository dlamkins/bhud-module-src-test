using System;
using System.Collections.Generic;
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

		private const string DefaultApiBaseUrl = "https://www.gw2opus.com/wp-json/cinemahud/v3";

		private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10.0);

		private readonly HttpClient _httpClient;

		private readonly string _apiBaseUrl;

		private readonly TextureService _textureService;

		private PresetsResponse _cachedPresets;

		private bool _isLoaded;

		public IReadOnlyList<WorldLocationCategory> WorldLocationCategories => _cachedPresets?.WorldLocationCategories ?? new List<WorldLocationCategory>();

		public IReadOnlyList<WorldLocationPresetData> WorldLocationPresets => WorldLocationCategories.SelectMany((WorldLocationCategory c) => c.Locations).ToList();

		public IReadOnlyList<StreamCategory> StreamCategories => _cachedPresets?.StreamCategories ?? new List<StreamCategory>();

		public IReadOnlyList<string> TwitchChannels => StreamCategories.Where((StreamCategory c) => c.IsTwitch).SelectMany((StreamCategory c) => c.TwitchChannelNames).ToList();

		public bool IsLoaded => _isLoaded;

		public event EventHandler PresetsLoaded;

		public event EventHandler PresetImagesLoaded;

		public ChannelData FindChannelById(string channelId)
		{
			if (string.IsNullOrEmpty(channelId) || _cachedPresets?.StreamCategories == null)
			{
				return null;
			}
			foreach (StreamCategory streamCategory in _cachedPresets.StreamCategories)
			{
				ChannelData channel = streamCategory.Channels.FirstOrDefault((ChannelData c) => c.Id == channelId);
				if (channel != null)
				{
					return channel;
				}
			}
			return null;
		}

		public PresetService(TextureService textureService)
			: this(textureService, "https://www.gw2opus.com/wp-json/cinemahud/v3")
		{
		}

		public PresetService(TextureService textureService, string apiBaseUrl)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			_textureService = textureService;
			_apiBaseUrl = apiBaseUrl;
			HttpClient val = new HttpClient();
			val.set_Timeout(RequestTimeout);
			_httpClient = val;
		}

		public async Task LoadPresetsAsync()
		{
			_ = 1;
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "/config");
				if (!response.get_IsSuccessStatusCode())
				{
					Logger.Warn($"Failed to load presets from API: {response.get_StatusCode()}");
					return;
				}
				_cachedPresets = JsonConvert.DeserializeObject<PresetsResponse>(await response.get_Content().ReadAsStringAsync());
				ParseStreamCategories();
				_isLoaded = true;
				LoadPresetImagesAsync();
				this.PresetsLoaded?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load presets from API");
			}
		}

		private void ParseStreamCategories()
		{
			if (_cachedPresets?.StreamCategories == null)
			{
				return;
			}
			foreach (StreamCategory streamCategory in _cachedPresets.StreamCategories)
			{
				streamCategory.ParseChannels();
			}
		}

		private async Task LoadPresetImagesAsync()
		{
			List<Task> tasks = new List<Task>();
			if (_cachedPresets?.WorldLocationCategories != null)
			{
				foreach (WorldLocationCategory category2 in _cachedPresets.WorldLocationCategories)
				{
					if (!string.IsNullOrEmpty(category2.Icon))
					{
						tasks.Add(LoadWorldLocationCategoryIconAsync(category2));
					}
					foreach (WorldLocationPresetData location in category2.Locations)
					{
						tasks.Add(LoadImagesForWorldLocationAsync(location));
					}
				}
			}
			if (_cachedPresets?.StreamCategories != null)
			{
				foreach (StreamCategory category in _cachedPresets.StreamCategories)
				{
					if (!string.IsNullOrEmpty(category.Icon))
					{
						tasks.Add(LoadCategoryIconAsync(category));
					}
					foreach (ChannelData channel in category.Channels)
					{
						tasks.Add(LoadImagesForChannelAsync(channel));
					}
				}
			}
			await Task.WhenAll(tasks);
			this.PresetImagesLoaded?.Invoke(this, EventArgs.Empty);
		}

		private async Task LoadWorldLocationCategoryIconAsync(WorldLocationCategory category)
		{
			if (!string.IsNullOrEmpty(category.Icon))
			{
				category.IconTexture = await _textureService.GetPresetImageAsync("loc_cat_" + category.Id + "_icon", category.Icon);
			}
		}

		private async Task LoadImagesForWorldLocationAsync(WorldLocationPresetData preset)
		{
			Task<AsyncTexture2D> avatarTask = _textureService.GetPresetImageAsync(preset.Id + "_avatar", preset.Avatar);
			Task<AsyncTexture2D> pictureTask = _textureService.GetPresetImageAsync(preset.Id + "_picture", preset.Picture);
			await Task.WhenAll<AsyncTexture2D>(avatarTask, pictureTask);
			preset.AvatarTexture = avatarTask.Result;
			preset.PictureTexture = pictureTask.Result;
		}

		private async Task LoadCategoryIconAsync(StreamCategory category)
		{
			if (!string.IsNullOrEmpty(category.Icon))
			{
				category.IconTexture = await _textureService.GetPresetImageAsync("cat_" + category.Id + "_icon", category.Icon);
			}
		}

		private async Task LoadImagesForChannelAsync(ChannelData channel)
		{
			List<Task> tasks = new List<Task>();
			if (!string.IsNullOrEmpty(channel.Avatar))
			{
				tasks.Add(_textureService.GetPresetImageAsync("ch_" + channel.Id + "_avatar", channel.Avatar).ContinueWith((Task<AsyncTexture2D> t) => channel.AvatarTexture = t.Result, TaskContinuationOptions.OnlyOnRanToCompletion));
			}
			if (!string.IsNullOrEmpty(channel.StaticImage))
			{
				tasks.Add(_textureService.GetPresetImageAsync("ch_" + channel.Id + "_static", channel.StaticImage).ContinueWith((Task<AsyncTexture2D> t) => channel.StaticImageTexture = t.Result, TaskContinuationOptions.OnlyOnRanToCompletion));
			}
			if (tasks.Count > 0)
			{
				await Task.WhenAll(tasks);
			}
		}

		public void Dispose()
		{
			HttpClient httpClient = _httpClient;
			if (httpClient != null)
			{
				((HttpMessageInvoker)httpClient).Dispose();
			}
		}
	}
}
