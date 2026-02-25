using System;
using System.IO;
using Blish_HUD;
using CinemaModule.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace CinemaModule.Settings
{
	public class CinemaUserSettings
	{
		private static readonly Logger Logger = Logger.GetLogger<CinemaUserSettings>();

		private const string SettingsFileName = "cinema_settings.json";

		private const int MinVolume = 0;

		private const int MaxVolume = 100;

		private const float MinScreenWidth = 4f;

		private const float MaxScreenWidth = 50f;

		private const float ScreenWidthTolerance = 0.001f;

		private readonly string _settingsFilePath;

		private CinemaUserSettingsData _data;

		private StreamPresetData _currentStreamPreset;

		public StreamPresetData CurrentStreamPreset
		{
			get
			{
				return _currentStreamPreset;
			}
			set
			{
				if (_currentStreamPreset != value)
				{
					_currentStreamPreset = value;
					this.CurrentStreamPresetChanged?.Invoke(this, value);
				}
			}
		}

		public string StreamUrl
		{
			get
			{
				return _data.StreamUrl;
			}
			set
			{
				SetPropertyWithEvent(_data.StreamUrl, value, delegate(string v)
				{
					_data.StreamUrl = v;
				}, this.StreamUrlChanged);
			}
		}

		public string CurrentTwitchChannel
		{
			get
			{
				return _data.CurrentTwitchChannel;
			}
			set
			{
				string normalizedValue = value ?? "";
				if (!(_data.CurrentTwitchChannel == normalizedValue))
				{
					_data.CurrentTwitchChannel = normalizedValue;
					Save();
				}
			}
		}

		public StreamSourceType CurrentStreamSourceType
		{
			get
			{
				return _data.CurrentStreamSourceType;
			}
			set
			{
				SetPropertyWithEvent(_data.CurrentStreamSourceType, value, delegate(StreamSourceType v)
				{
					_data.CurrentStreamSourceType = v;
				}, this.CurrentStreamSourceTypeChanged);
			}
		}

		public int Volume
		{
			get
			{
				return _data.Volume;
			}
			set
			{
				SetPropertyWithEvent(_data.Volume, Clamp(value, 0, 100), delegate(int v)
				{
					_data.Volume = v;
				}, this.VolumeChanged);
			}
		}

		public CinemaDisplayMode DisplayMode
		{
			get
			{
				return _data.DisplayMode;
			}
			set
			{
				SetPropertyWithEvent(_data.DisplayMode, value, delegate(CinemaDisplayMode v)
				{
					_data.DisplayMode = v;
				}, this.DisplayModeChanged);
			}
		}

		public string SelectedPresetLocationId
		{
			get
			{
				return _data.SelectedPresetLocationId;
			}
			set
			{
				SetProperty(_data.SelectedPresetLocationId, value, delegate(string v)
				{
					_data.SelectedPresetLocationId = v;
				});
			}
		}

		public string SelectedSavedLocationId
		{
			get
			{
				return _data.SelectedSavedLocationId;
			}
			set
			{
				SetProperty(_data.SelectedSavedLocationId, value, delegate(string v)
				{
					_data.SelectedSavedLocationId = v;
				});
			}
		}

		public WorldPosition3D WorldPosition
		{
			get
			{
				return _data.WorldPosition;
			}
			set
			{
				SetPropertyWithEvent(_data.WorldPosition, value, delegate(WorldPosition3D v)
				{
					_data.WorldPosition = v;
				}, this.WorldPositionChanged);
			}
		}

		public float WorldScreenWidth
		{
			get
			{
				return _data.WorldScreenWidth;
			}
			set
			{
				float clampedValue = Clamp(value, 4f, 50f);
				if (Math.Abs(_data.WorldScreenWidth - clampedValue) > 0.001f)
				{
					_data.WorldScreenWidth = clampedValue;
					Save();
					RaiseEvent(this.WorldScreenWidthChanged, clampedValue);
				}
			}
		}

		public Point WindowPosition
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _data.WindowPosition;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(_data.WindowPosition, value, delegate(Point v)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					_data.WindowPosition = v;
				});
			}
		}

		public Point WindowSize
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _data.WindowSize;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(_data.WindowSize, value, delegate(Point v)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					_data.WindowSize = v;
				});
			}
		}

		public bool WindowLocked
		{
			get
			{
				return _data.WindowLocked;
			}
			set
			{
				SetProperty(_data.WindowLocked, value, delegate(bool v)
				{
					_data.WindowLocked = v;
				});
			}
		}

		public SavedLocationCollection SavedLocations => _data.SavedLocations;

		public SavedStreamCollection SavedStreams => _data.SavedStreams;

		public string SelectedSavedStreamId
		{
			get
			{
				return _data.SelectedSavedStreamId;
			}
			set
			{
				SetProperty(_data.SelectedSavedStreamId, value, delegate(string v)
				{
					_data.SelectedSavedStreamId = v;
				});
			}
		}

		public string SelectedUrlChannelId
		{
			get
			{
				return _data.SelectedUrlChannelId;
			}
			set
			{
				SetProperty(_data.SelectedUrlChannelId, value, delegate(string v)
				{
					_data.SelectedUrlChannelId = v;
				});
			}
		}

		public string TwitchAccessToken
		{
			get
			{
				return _data.TwitchAccessToken;
			}
			set
			{
				SetProperty(_data.TwitchAccessToken, value, delegate(string v)
				{
					_data.TwitchAccessToken = v;
				});
			}
		}

		public string TwitchRefreshToken
		{
			get
			{
				return _data.TwitchRefreshToken;
			}
			set
			{
				SetProperty(_data.TwitchRefreshToken, value, delegate(string v)
				{
					_data.TwitchRefreshToken = v;
				});
			}
		}

		public string LastSelectedSourceCategory
		{
			get
			{
				return _data.LastSelectedSourceCategory;
			}
			set
			{
				SetProperty(_data.LastSelectedSourceCategory, value, delegate(string v)
				{
					_data.LastSelectedSourceCategory = v;
				});
			}
		}

		public string LastSelectedLocationCategory
		{
			get
			{
				return _data.LastSelectedLocationCategory;
			}
			set
			{
				SetProperty(_data.LastSelectedLocationCategory, value, delegate(string v)
				{
					_data.LastSelectedLocationCategory = v;
				});
			}
		}

		public int SelectedSettingsTab
		{
			get
			{
				return _data.SelectedSettingsTab;
			}
			set
			{
				SetProperty(_data.SelectedSettingsTab, value, delegate(int v)
				{
					_data.SelectedSettingsTab = v;
				});
			}
		}

		public int SettingsWindowHeight
		{
			get
			{
				return _data.SettingsWindowHeight;
			}
			set
			{
				SetProperty(_data.SettingsWindowHeight, value, delegate(int v)
				{
					_data.SettingsWindowHeight = v;
				});
			}
		}

		public bool TwitchChatWindowLocked
		{
			get
			{
				return _data.TwitchChatWindowLocked;
			}
			set
			{
				SetProperty(_data.TwitchChatWindowLocked, value, delegate(bool v)
				{
					_data.TwitchChatWindowLocked = v;
				});
			}
		}

		public bool TwitchChatWindowOpen
		{
			get
			{
				return _data.TwitchChatWindowOpen;
			}
			set
			{
				SetProperty(_data.TwitchChatWindowOpen, value, delegate(bool v)
				{
					_data.TwitchChatWindowOpen = v;
				});
			}
		}

		public Point TwitchChatWindowSize
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _data.TwitchChatWindowSize;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(_data.TwitchChatWindowSize, value, delegate(Point v)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					_data.TwitchChatWindowSize = v;
				});
			}
		}

		public string TwitchChatWindowChannel
		{
			get
			{
				return _data.TwitchChatWindowChannel;
			}
			set
			{
				SetProperty(_data.TwitchChatWindowChannel, value ?? "", delegate(string v)
				{
					_data.TwitchChatWindowChannel = v;
				});
			}
		}

		public event EventHandler<string> StreamUrlChanged;

		public event EventHandler<CinemaDisplayMode> DisplayModeChanged;

		public event EventHandler<WorldPosition3D> WorldPositionChanged;

		public event EventHandler<float> WorldScreenWidthChanged;

		public event EventHandler<int> VolumeChanged;

		public event EventHandler<StreamSourceType> CurrentStreamSourceTypeChanged;

		public event EventHandler<StreamPresetData> CurrentStreamPresetChanged;

		public event EventHandler SavedLocationsChanged;

		public event EventHandler SavedStreamsChanged;

		public CinemaUserSettings(string settingsDirectory)
		{
			_settingsFilePath = Path.Combine(settingsDirectory, "cinema_settings.json");
			Load();
		}

		public SavedLocation AddSavedLocation(string name, WorldPosition3D position, float screenWidth)
		{
			SavedLocation location = new SavedLocation(name, position, screenWidth);
			SavedLocations.Locations.Add(location);
			Save();
			RaiseEvent(this.SavedLocationsChanged);
			return location;
		}

		public void UpdateSavedLocation(SavedLocation location)
		{
			int index = SavedLocations.Locations.FindIndex((SavedLocation l) => l.Id == location.Id);
			if (index >= 0)
			{
				SavedLocations.Locations[index] = location;
				Save();
				RaiseEvent(this.SavedLocationsChanged);
			}
		}

		public bool DeleteSavedLocation(string id)
		{
			if (SavedLocations.Locations.RemoveAll((SavedLocation l) => l.Id == id) <= 0)
			{
				return false;
			}
			if (SelectedSavedLocationId == id)
			{
				_data.SelectedSavedLocationId = "";
			}
			Save();
			RaiseEvent(this.SavedLocationsChanged);
			return true;
		}

		public SavedStream AddSavedStream(string name, StreamSourceType sourceType, string value)
		{
			SavedStream stream = new SavedStream(name, sourceType, value);
			SavedStreams.Streams.Add(stream);
			Save();
			RaiseEvent(this.SavedStreamsChanged);
			return stream;
		}

		public void UpdateSavedStream(SavedStream stream)
		{
			int index = SavedStreams.Streams.FindIndex((SavedStream s) => s.Id == stream.Id);
			if (index >= 0)
			{
				SavedStreams.Streams[index] = stream;
				Save();
				RaiseEvent(this.SavedStreamsChanged);
			}
		}

		public bool DeleteSavedStream(string id)
		{
			bool num = SavedStreams.Streams.RemoveAll((SavedStream s) => s.Id == id) > 0;
			if (num)
			{
				if (SelectedSavedStreamId == id)
				{
					_data.SelectedSavedStreamId = "";
				}
				Save();
				RaiseEvent(this.SavedStreamsChanged);
			}
			return num;
		}

		public void SelectTwitchChannel(string channelName)
		{
			SelectedSavedStreamId = "";
			CurrentTwitchChannel = channelName;
			CurrentStreamSourceType = StreamSourceType.TwitchChannel;
			CurrentStreamPreset = null;
		}

		public void SelectUrlChannel(ChannelData channel)
		{
			SelectedSavedStreamId = "";
			CurrentTwitchChannel = "";
			SelectedUrlChannelId = channel.Id;
			CurrentStreamSourceType = StreamSourceType.Url;
			CurrentStreamPreset = channel.ToStreamPresetData();
			StreamUrl = channel.Url;
		}

		public void SelectSavedStream(SavedStream stream)
		{
			SelectedSavedStreamId = stream.Id;
			CurrentStreamSourceType = stream.SourceType;
			CurrentTwitchChannel = ((stream.SourceType == StreamSourceType.TwitchChannel) ? stream.Value : "");
			if (stream.SourceType == StreamSourceType.Url)
			{
				StreamUrl = stream.Value;
			}
			CurrentStreamPreset = null;
		}

		private bool SetProperty<T>(T currentValue, T newValue, Action<T> setter)
		{
			if (object.Equals(currentValue, newValue))
			{
				return false;
			}
			setter(newValue);
			Save();
			return true;
		}

		private bool SetPropertyWithEvent<T>(T currentValue, T newValue, Action<T> setter, EventHandler<T> eventHandler)
		{
			if (!SetProperty(currentValue, newValue, setter))
			{
				return false;
			}
			RaiseEvent(eventHandler, newValue);
			return true;
		}

		private void RaiseEvent<T>(EventHandler<T> eventHandler, T value)
		{
			eventHandler?.Invoke(this, value);
		}

		private void RaiseEvent(EventHandler eventHandler)
		{
			eventHandler?.Invoke(this, EventArgs.Empty);
		}

		private static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			if (value.CompareTo(max) > 0)
			{
				return max;
			}
			return value;
		}

		private void MigrateTwitchChannelFromSelectedStream()
		{
			if (string.IsNullOrEmpty(_data.CurrentTwitchChannel) && !string.IsNullOrEmpty(_data.SelectedSavedStreamId))
			{
				SavedStream selectedStream = _data.SavedStreams.Streams.Find((SavedStream s) => s.Id == _data.SelectedSavedStreamId);
				if (selectedStream != null && selectedStream.SourceType == StreamSourceType.TwitchChannel)
				{
					_data.CurrentTwitchChannel = selectedStream.Value;
					Save();
				}
			}
		}

		private void Load()
		{
			if (File.Exists(_settingsFilePath))
			{
				try
				{
					string json = File.ReadAllText(_settingsFilePath);
					_data = JsonConvert.DeserializeObject<CinemaUserSettingsData>(json) ?? new CinemaUserSettingsData();
					if (_data.SavedLocations == null)
					{
						_data.SavedLocations = new SavedLocationCollection();
					}
					if (_data.SavedStreams == null)
					{
						_data.SavedStreams = new SavedStreamCollection();
					}
					if (_data.WorldPosition == null)
					{
						_data.WorldPosition = new WorldPosition3D(0f, 0f, 0f, 0);
					}
					MigrateTwitchChannelFromSelectedStream();
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to load CinemaHUD settings: " + ex.Message);
					_data = new CinemaUserSettingsData();
				}
			}
			else
			{
				_data = new CinemaUserSettingsData();
			}
		}

		private void Save()
		{
			try
			{
				string directory = Path.GetDirectoryName(_settingsFilePath);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				string json = JsonConvert.SerializeObject((object)_data, (Formatting)1);
				File.WriteAllText(_settingsFilePath, json);
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to save CinemaHUD settings: " + ex.Message);
			}
		}
	}
}
