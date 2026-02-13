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

		private const int MinWindowEdgeSpacing = 50;

		private const int DefaultWindowX = 100;

		private const int DefaultWindowY = 50;

		private readonly string _settingsFilePath;

		private CinemaUserSettingsData _data;

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
				return _data.CurrentTwitchChannel ?? "";
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
				return _data.SelectedPresetLocationId ?? "";
			}
			set
			{
				_data.SelectedPresetLocationId = value;
				Save();
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
				_data.SelectedSavedLocationId = value;
				Save();
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
				_data.WorldPosition = value;
				Save();
				RaiseEvent(this.WorldPositionChanged, value);
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
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_data.WindowPosition = value;
				Save();
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
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_data.WindowSize = value;
				Save();
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
				_data.SelectedSavedStreamId = value;
				Save();
			}
		}

		public event EventHandler<string> StreamUrlChanged;

		public event EventHandler<CinemaDisplayMode> DisplayModeChanged;

		public event EventHandler<WorldPosition3D> WorldPositionChanged;

		public event EventHandler<float> WorldScreenWidthChanged;

		public event EventHandler<int> VolumeChanged;

		public event EventHandler<StreamSourceType> CurrentStreamSourceTypeChanged;

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
			if (SavedLocations.Locations.RemoveAll((SavedLocation l) => l.Id == id) > 0)
			{
				if (SelectedSavedLocationId == id)
				{
					_data.SelectedSavedLocationId = "";
				}
				Save();
				RaiseEvent(this.SavedLocationsChanged);
				return true;
			}
			return false;
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

		public string GetCurrentTwitchChannel()
		{
			if (!string.IsNullOrEmpty(CurrentTwitchChannel))
			{
				return CurrentTwitchChannel;
			}
			return null;
		}

		private bool SetPropertyWithEvent<T>(T currentValue, T newValue, Action<T> setter, EventHandler<T> eventHandler)
		{
			if (object.Equals(currentValue, newValue))
			{
				return false;
			}
			setter(newValue);
			Save();
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

		private static int Clamp(int value, int min, int max)
		{
			return Math.Max(min, Math.Min(max, value));
		}

		private static float Clamp(float value, float min, float max)
		{
			return Math.Max(min, Math.Min(max, value));
		}

		private void Load()
		{
			if (File.Exists(_settingsFilePath))
			{
				try
				{
					string json = File.ReadAllText(_settingsFilePath);
					_data = JsonConvert.DeserializeObject<CinemaUserSettingsData>(json) ?? new CinemaUserSettingsData();
					_data.SavedLocations = _data.SavedLocations ?? new SavedLocationCollection();
					_data.SavedStreams = _data.SavedStreams ?? new SavedStreamCollection();
					_data.WorldPosition = _data.WorldPosition ?? new WorldPosition3D(0f, 0f, 0f, 0);
					if (string.IsNullOrEmpty(_data.CurrentTwitchChannel) && !string.IsNullOrEmpty(_data.SelectedSavedStreamId))
					{
						SavedStream selectedStream = _data.SavedStreams?.Streams.Find((SavedStream s) => s.Id == _data.SelectedSavedStreamId);
						if (selectedStream != null && selectedStream.SourceType == StreamSourceType.TwitchChannel)
						{
							_data.CurrentTwitchChannel = selectedStream.Value;
							Save();
							Logger.Debug("Populated CurrentTwitchChannel from selected stream: " + selectedStream.Value);
						}
					}
					Logger.Info("Loaded CinemaHUD settings from JSON");
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
