using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace OpNop.EnterTheSandstorm
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private IWavePlayer _soundClip;

		private VolumeSampleProvider _volumeSampler;

		private LoopingAudioStream _audioStream;

		private double _timeSinceUpdate;

		private int LastMap;

		private bool _dryTopTriggered;

		private readonly int DryTop = 988;

		private readonly int Oasis = 1210;

		private SettingEntry<float> _masterVolume;

		private SettingEntry<bool> _loop;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_masterVolume = settings.DefineSetting("MasterVolume.", 50f, () => "Master Volume", () => "Getting too dusty for ya?");
			_loop = settings.DefineSetting("Loop", defaultValue: true, () => "Loop in Dry Top during Sandstorm", () => "Do you want the song to loop during the sandstorm event, or only at the start?");
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			Stream stream = ContentsManager.GetFileStream("sandstorm.mp3");
			_audioStream = new LoopingAudioStream(new Mp3FileReader(stream));
			_volumeSampler = new VolumeSampleProvider(_audioStream.ToSampleProvider());
			_soundClip = new WaveOutEvent();
			_soundClip.Init(_volumeSampler);
			_volumeSampler.Volume = 0f;
			GameService.GameIntegration.Gw2Instance.Gw2Closed += GameIntegration_Gw2Closed;
			GameService.GameIntegration.Gw2Instance.Gw2Started += GameIntegration_Gw2Started;
			base.OnModuleLoaded(e);
		}

		private void GameIntegration_Gw2Started(object sender, EventArgs e)
		{
		}

		private void GameIntegration_Gw2Closed(object sender, EventArgs e)
		{
			_soundClip.Stop();
		}

		private async void UpdateVolume(GameTime gameTime, int CurrentMap)
		{
			if (_timeSinceUpdate < 200.0)
			{
				_timeSinceUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
				return;
			}
			_timeSinceUpdate = 0.0;
			_ = GameService.Gw2Mumble.PlayerCharacter.Position;
			float volume;
			if (DryTop == CurrentMap)
			{
				if (DateTime.UtcNow.Minute > 39 && DateTime.UtcNow.Minute <= 59)
				{
					if (!_dryTopTriggered)
					{
						_soundClip.Stop();
						_audioStream.Seek(0L, SeekOrigin.Begin);
						_soundClip.Play();
						_dryTopTriggered = true;
					}
					volume = ((DateTime.UtcNow.Minute != 59) ? (_masterVolume.Value / 100f) : Map(DateTime.UtcNow.Second, 44f, 59f, _masterVolume.Value / 100f, 0f));
				}
				else
				{
					volume = 0f;
					_soundClip.Stop();
					_dryTopTriggered = false;
				}
			}
			else
			{
				volume = 0f;
			}
			volume = Clamp(volume, 0f, _masterVolume.Value / 100f);
			_volumeSampler.Volume = volume;
		}

		public static float Clamp(float value, float min, float max)
		{
			if (!(value < min))
			{
				if (!(value > max))
				{
					return value;
				}
				return max;
			}
			return min;
		}

		private static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
		{
			return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
		}

		protected override async void Update(GameTime gameTime)
		{
			int CurrentMap = GameService.Gw2Mumble.CurrentMap.Id;
			if (LastMap != CurrentMap)
			{
				LastMap = CurrentMap;
				if (CurrentMap == DryTop || CurrentMap == Oasis)
				{
					if (_soundClip.PlaybackState != PlaybackState.Playing)
					{
						_audioStream.Seek(0L, SeekOrigin.Begin);
						_soundClip.Play();
					}
				}
				else
				{
					_soundClip.Stop();
				}
			}
			UpdateVolume(gameTime, CurrentMap);
		}

		protected override void Unload()
		{
			_soundClip?.Stop();
			_soundClip?.Dispose();
		}
	}
}
