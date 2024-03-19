using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Nekres.Music_Mixer.Core.Services.Data;

namespace Nekres.Music_Mixer.Core.Services.Audio
{
	internal class AudioService : IDisposable
	{
		private bool _muted;

		private AudioSource _currentSource;

		private AudioSource _previousSource;

		private TimeSpan _interuptedAt;

		private readonly TaskScheduler _scheduler;

		public bool Muted
		{
			get
			{
				return _muted;
			}
			set
			{
				_muted = value;
				if (!AudioTrack.IsEmpty)
				{
					AudioTrack.Muted = value;
				}
			}
		}

		public AudioTrack AudioTrack { get; private set; }

		public bool Loading { get; private set; }

		public event EventHandler<ValueEventArgs<AudioSource>> MusicChanged;

		public AudioService()
		{
			AudioTrack = AudioTrack.Empty;
			_scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			MusicMixer.Instance.Gw2State.IsSubmergedChanged += OnIsSubmergedChanged;
			MusicMixer.Instance.Gw2State.StateChanged += OnStateChanged;
			GameService.GameIntegration.get_Gw2Instance().add_Gw2LostFocus((EventHandler<EventArgs>)OnGw2LostFocus);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2AcquiredFocus((EventHandler<EventArgs>)OnGw2AcquiredFocus);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Closed((EventHandler<EventArgs>)OnGw2Closed);
		}

		public async Task Play(AudioSource source)
		{
			if (!Loading && source != null && !source.IsEmpty && !string.IsNullOrEmpty(source.PageUrl))
			{
				if (string.IsNullOrEmpty(source.AudioUrl))
				{
					source.AudioUrl = await MusicMixer.Instance.YtDlp.GetAudioOnlyUrl(source.PageUrl);
					MusicMixer.Instance.Data.Upsert(source);
				}
				_currentSource = source;
				await TryPlay(source);
			}
		}

		private async Task<bool> TryPlay(AudioSource source)
		{
			Loading = true;
			try
			{
				return await Task.Factory.StartNew(async delegate
				{
					AudioTrack track = await AudioTrack.TryGetStream(source);
					if (track.IsEmpty || source.State != (MusicMixer.Instance?.Gw2State?.CurrentState).GetValueOrDefault())
					{
						track.Dispose();
						return false;
					}
					if (!AudioTrack.IsEmpty)
					{
						AudioTrack.Finished -= OnSoundtrackFinished;
						await AudioTrack.DisposeAsync();
					}
					if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning())
					{
						AudioUtil.SetVolume(GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().Id, 0.1f);
					}
					AudioTrack = track;
					AudioTrack.Muted = Muted;
					AudioTrack.Finished += OnSoundtrackFinished;
					await AudioTrack.Play();
					this.MusicChanged?.Invoke(this, new ValueEventArgs<AudioSource>(_currentSource));
					return true;
				}, CancellationToken.None, TaskCreationOptions.None, _scheduler).Unwrap();
			}
			finally
			{
				Loading = false;
			}
		}

		public void Stop()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning())
			{
				AudioUtil.SetVolume(GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().Id, 1f);
			}
			AudioTrack?.Dispose();
			AudioTrack = AudioTrack.Empty;
		}

		private async void OnSoundtrackFinished(object o, EventArgs e)
		{
			_currentSource = null;
			await ChangeContext(MusicMixer.Instance.Gw2State.CurrentState);
		}

		public void Pause()
		{
			AudioTrack?.Pause();
		}

		public void Resume()
		{
			AudioTrack?.Resume();
		}

		public void SaveContext()
		{
			if (!AudioTrack.IsEmpty && _currentSource != null)
			{
				_interuptedAt = AudioTrack.CurrentTime;
				_previousSource = _currentSource;
			}
		}

		public async Task<bool> PlayFromSave()
		{
			if (_previousSource == null || _interuptedAt > _previousSource.Duration)
			{
				return false;
			}
			if (!AudioTrack.IsEmpty && AudioTrack.Source.AudioUrl.Equals(_previousSource.AudioUrl))
			{
				return true;
			}
			if (await TryPlay(_previousSource))
			{
				AudioTrack?.Seek(_interuptedAt);
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			MusicMixer.Instance.Gw2State.IsSubmergedChanged -= OnIsSubmergedChanged;
			MusicMixer.Instance.Gw2State.StateChanged -= OnStateChanged;
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2LostFocus((EventHandler<EventArgs>)OnGw2LostFocus);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2AcquiredFocus((EventHandler<EventArgs>)OnGw2AcquiredFocus);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Closed((EventHandler<EventArgs>)OnGw2Closed);
			AudioTrack?.Dispose();
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning())
			{
				AudioUtil.SetVolume(GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().Id, 1f);
			}
		}

		private void OnGw2LostFocus(object o, EventArgs e)
		{
			MusicMixer instance = MusicMixer.Instance;
			if (instance != null && instance.MuteWhenInBackground.get_Value())
			{
				Pause();
			}
		}

		private void OnGw2AcquiredFocus(object o, EventArgs e)
		{
			Resume();
		}

		private void OnGw2Closed(object o, EventArgs e)
		{
			Stop();
		}

		private async void OnStateChanged(object o, ValueChangedEventArgs<Gw2StateService.State> e)
		{
			if (e.get_NewValue() == Gw2StateService.State.StandBy)
			{
				Stop();
			}
			Gw2StateService.State previousValue = e.get_PreviousValue();
			if ((uint)(previousValue - 1) <= 1u)
			{
				Stop();
			}
			if (!(await PlayFromSave()))
			{
				await ChangeContext(e.get_NewValue());
			}
		}

		private async Task ChangeContext(Gw2StateService.State state)
		{
			int dayCycle = (int)MusicMixer.Instance.Gw2State.TyrianTime;
			AudioSource audio = AudioSource.Empty;
			switch (state)
			{
			case Gw2StateService.State.Mounted:
			{
				if (!MusicMixer.Instance.Data.GetMountPlaylist(GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount(), out var context) || context.IsEmpty || !context.Enabled)
				{
					return;
				}
				audio = context.GetRandom(dayCycle);
				break;
			}
			case Gw2StateService.State.Defeated:
			{
				if (!MusicMixer.Instance.Data.GetDefeatedPlaylist(out var context2) || context2.IsEmpty || !context2.Enabled)
				{
					return;
				}
				audio = context2.GetRandom(dayCycle);
				break;
			}
			}
			audio.State = state;
			await Play(audio);
		}

		private void OnIsSubmergedChanged(object o, ValueEventArgs<bool> e)
		{
			AudioTrack?.ToggleSubmergedFx(e.get_Value());
		}
	}
}
