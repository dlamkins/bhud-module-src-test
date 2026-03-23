using System;
using System.IO;
using Blish_HUD;
using Newtonsoft.Json;
using SongbookOfTyria.Models;

namespace SongbookOfTyria.Services
{
	public sealed class UserSettingsService
	{
		private static readonly Logger Logger = Logger.GetLogger<UserSettingsService>();

		private const string StateFileName = "user_settings.json";

		private readonly string _cacheDirectory;

		private UserSettingsData _state;

		public event EventHandler<int> FavoriteChanged;

		public UserSettingsService(string cacheDirectory)
		{
			_cacheDirectory = cacheDirectory;
			LoadState();
		}

		public FilterState GetFilterState()
		{
			return _state.FilterState ?? (_state.FilterState = new FilterState());
		}

		public void SaveFilterState(FilterState filterState)
		{
			_state.FilterState = filterState;
			SaveState();
		}

		public TabWindowState GetTabWindowState(int tabId)
		{
			if (_state.TabWindowStates.TryGetValue(tabId, out var windowState))
			{
				return windowState;
			}
			return null;
		}

		public void SaveTabWindowState(int tabId, TabWindowState windowState)
		{
			_state.TabWindowStates[tabId] = windowState;
			SaveState();
		}

		public bool GetGlobalDetailsCollapsed()
		{
			return _state.GlobalDetailsCollapsed;
		}

		public void SaveGlobalDetailsCollapsed(bool collapsed)
		{
			_state.GlobalDetailsCollapsed = collapsed;
			SaveState();
		}

		public bool GetGlobalViewOptionsCollapsed()
		{
			return _state.GlobalViewOptionsCollapsed;
		}

		public void SaveGlobalViewOptionsCollapsed(bool collapsed)
		{
			_state.GlobalViewOptionsCollapsed = collapsed;
			SaveState();
		}

		public bool GetGlobalAudioPlayerCollapsed()
		{
			return _state.GlobalAudioPlayerCollapsed;
		}

		public void SaveGlobalAudioPlayerCollapsed(bool collapsed)
		{
			_state.GlobalAudioPlayerCollapsed = collapsed;
			SaveState();
		}

		public bool GetGlobalPianoKeybindsCollapsed()
		{
			return _state.GlobalPianoKeybindsCollapsed;
		}

		public void SaveGlobalPianoKeybindsCollapsed(bool collapsed)
		{
			_state.GlobalPianoKeybindsCollapsed = collapsed;
			SaveState();
		}

		public PianoKeybinds GetPianoKeybinds()
		{
			return _state.PianoKeybinds ?? (_state.PianoKeybinds = new PianoKeybinds());
		}

		public void SavePianoKeybinds(PianoKeybinds keybinds)
		{
			_state.PianoKeybinds = keybinds;
			SaveState();
		}

		public bool IsFavorite(int tabId)
		{
			return _state.Favorites.Contains(tabId);
		}

		public void SetFavorite(int tabId, bool isFavorite)
		{
			bool changed = false;
			if ((!isFavorite) ? _state.Favorites.Remove(tabId) : _state.Favorites.Add(tabId))
			{
				SaveState();
				this.FavoriteChanged?.Invoke(this, tabId);
			}
		}

		public void ToggleFavorite(int tabId)
		{
			SetFavorite(tabId, !IsFavorite(tabId));
		}

		private void LoadState()
		{
			_state = new UserSettingsData();
			try
			{
				string stateFilePath = Path.Combine(_cacheDirectory, "user_settings.json");
				if (File.Exists(stateFilePath))
				{
					string json = File.ReadAllText(stateFilePath);
					_state = JsonConvert.DeserializeObject<UserSettingsData>(json) ?? new UserSettingsData();
					Logger.Debug("Loaded user settings from disk");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load user settings from disk");
				_state = new UserSettingsData();
			}
		}

		private void SaveState()
		{
			try
			{
				string path = Path.Combine(_cacheDirectory, "user_settings.json");
				string json = JsonConvert.SerializeObject((object)_state, (Formatting)1);
				File.WriteAllText(path, json);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to save user settings to disk");
			}
		}
	}
}
