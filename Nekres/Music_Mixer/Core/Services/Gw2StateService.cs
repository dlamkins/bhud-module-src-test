using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Music_Mixer.Core.Services
{
	public class Gw2StateService : IDisposable
	{
		public enum State
		{
			StandBy,
			Mounted,
			Battle,
			Competitive,
			Defeated
		}

		private TyrianTime _prevTyrianTime;

		private bool _prevIsSubmerged = GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z <= 0f;

		private State _currentState;

		private readonly NTimer _inCombatTimer;

		private readonly NTimer _outOfCombatTimer;

		private readonly NTimer _outOfCombatTimerLong;

		private string _lockFile = "silence.wav";

		private DateTime _lastLockFileCheck = DateTime.UtcNow.AddSeconds(10.0);

		private int[] _guildHallIds = new int[23]
		{
			1068, 1101, 1107, 1108, 1121, 1125, 1069, 1071, 1076, 1104,
			1124, 1144, 1214, 1215, 1224, 1232, 1243, 1250, 1419, 1426,
			1435, 1444, 1462
		};

		public TyrianTime TyrianTime
		{
			get
			{
				return _prevTyrianTime;
			}
			private set
			{
				if (_prevTyrianTime != value)
				{
					_prevTyrianTime = value;
					this.TyrianTimeChanged?.Invoke(this, new ValueEventArgs<TyrianTime>(value));
				}
			}
		}

		public bool IsSubmerged
		{
			get
			{
				return _prevIsSubmerged;
			}
			private set
			{
				if (_prevIsSubmerged != value)
				{
					_prevIsSubmerged = value;
					this.IsSubmergedChanged?.Invoke(this, new ValueEventArgs<bool>(value));
				}
			}
		}

		public State CurrentState
		{
			get
			{
				return _currentState;
			}
			set
			{
				if (_currentState != value)
				{
					this.StateChanged?.Invoke(this, new ValueChangedEventArgs<State>(_currentState, value));
					_currentState = value;
				}
			}
		}

		public event EventHandler<ValueChangedEventArgs<State>> StateChanged;

		public event EventHandler<ValueEventArgs<TyrianTime>> TyrianTimeChanged;

		public event EventHandler<ValueEventArgs<bool>> IsSubmergedChanged;

		public Gw2StateService()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			_inCombatTimer = new NTimer(6500.0)
			{
				AutoReset = false
			};
			_inCombatTimer.Elapsed += InCombatTimerElapsed;
			_outOfCombatTimer = new NTimer(3250.0)
			{
				AutoReset = false
			};
			_outOfCombatTimer.Elapsed += OutOfCombatTimerElapsed;
			_outOfCombatTimerLong = new NTimer(20250.0)
			{
				AutoReset = false
			};
			_outOfCombatTimerLong.Elapsed += OutOfCombatTimerElapsed;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Closed((EventHandler<EventArgs>)OnGw2Closed);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		private void ChangeState(State newState)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Invalid comparison between Unknown and I4
			switch (newState)
			{
			case State.Battle:
				if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsPublic())
				{
					CurrentState = newState;
					break;
				}
				goto default;
			case State.Mounted:
				if ((int)GameService.Gw2Mumble.get_CurrentMap().get_Type() == 5 || _guildHallIds.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id()))
				{
					CurrentState = newState;
				}
				break;
			default:
				CurrentState = newState;
				break;
			}
		}

		public async Task SetupLockFiles(State state)
		{
			string relLockFilePath2 = $"{state}\\{_lockFile}";
			await MusicMixer.Instance.ContentsManager.Extract("audio/" + _lockFile, Path.Combine(DirectoryUtil.get_MusicPath(), relLockFilePath2), overwrite: false);
			try
			{
				MusicMixer.Logger.Info($"Creating {state} playlist.");
				string path = Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}.m3u");
				relLockFilePath2 += "\r\n";
				if (!File.Exists(path))
				{
					goto IL_01aa;
				}
				if (File.ReadAllText(path, Encoding.UTF8).Equals(relLockFilePath2))
				{
					MusicMixer.Logger.Info($"{state} playlist already exists. Skipping.");
					return;
				}
				MusicMixer.Logger.Info($"Storing existing {state} playlist as backup.");
				File.Copy(path, Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}.backup.m3u"), overwrite: true);
				goto IL_01aa;
				IL_01aa:
				using FileStream file = File.Create(path);
				file.Position = 0L;
				byte[] content = Encoding.UTF8.GetBytes(relLockFilePath2);
				await file.WriteAsync(content, 0, content.Length);
				MusicMixer.Logger.Info($"{state} playlist created.");
				ScreenNotification.ShowNotification($"{state} playlist created. Game restart required.", (NotificationType)1, (Texture2D)null, 4);
			}
			catch (Exception e)
			{
				MusicMixer.Logger.Info(e, e.Message);
			}
		}

		public void RevertLockFiles(State state)
		{
			try
			{
				string backupPath = Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}.backup.m3u");
				string path = Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}.m3u");
				if (File.Exists(backupPath))
				{
					File.Copy(backupPath, path, overwrite: true);
					File.Delete(backupPath);
				}
				else if (File.Exists(path))
				{
					File.Delete(path);
				}
				ScreenNotification.ShowNotification($"{state} playlist reverted. Game restart required.", (NotificationType)1, (Texture2D)null, 4);
			}
			catch (Exception e)
			{
				MusicMixer.Logger.Info(e, e.Message);
			}
		}

		private void InCombatTimerElapsed(object sender, EventArgs e)
		{
			ChangeState(State.Battle);
		}

		private void OutOfCombatTimerElapsed(object sender, EventArgs e)
		{
			ChangeState(State.StandBy);
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Closed((EventHandler<EventArgs>)OnGw2Closed);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			if (_inCombatTimer != null)
			{
				_inCombatTimer.Elapsed -= InCombatTimerElapsed;
				_inCombatTimer.Dispose();
			}
			if (_outOfCombatTimer != null)
			{
				_outOfCombatTimer.Elapsed -= OutOfCombatTimerElapsed;
				_outOfCombatTimer.Dispose();
			}
			if (_outOfCombatTimerLong != null)
			{
				_outOfCombatTimerLong.Elapsed -= OutOfCombatTimerElapsed;
				_outOfCombatTimerLong.Dispose();
			}
		}

		public void Update()
		{
			if (DateTime.UtcNow.Subtract(_lastLockFileCheck).TotalMilliseconds > 200.0)
			{
				_lastLockFileCheck = DateTime.UtcNow;
				CheckLockFile(State.Defeated);
				if (GameService.Gw2Mumble.get_IsAvailable())
				{
					CheckTyrianTime();
					CheckWaterLevel();
					CheckMountChanged();
					CheckIsInCombatChanged();
				}
			}
		}

		private void CheckLockFile(State state)
		{
			if (FileUtil.IsFileLocked(Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}\\{_lockFile}")))
			{
				ChangeState(state);
			}
			else if (CurrentState == state)
			{
				ChangeState(State.StandBy);
			}
		}

		private void CheckWaterLevel()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			IsSubmerged = GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z <= 0f;
		}

		private void CheckTyrianTime()
		{
			TyrianTime = TyrianTimeUtil.GetCurrentDayCycle();
		}

		private void CheckMountChanged()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if ((int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() > 0)
			{
				if (!GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
				{
					ChangeState(State.Mounted);
				}
			}
			else if (CurrentState == State.Mounted)
			{
				CurrentState = State.StandBy;
			}
		}

		private void CheckIsInCombatChanged()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Invalid comparison between Unknown and I4
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				_inCombatTimer.Restart();
			}
			else if (CurrentState == State.Battle)
			{
				if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsInstance() || GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvW() || (int)GameService.Gw2Mumble.get_CurrentMap().get_Type() == 16)
				{
					_outOfCombatTimerLong.Restart();
				}
				else
				{
					_outOfCombatTimer.Restart();
				}
			}
			else
			{
				_inCombatTimer.Stop();
			}
		}

		private void OnIsInGameChanged(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				Reset();
			}
		}

		private void OnGw2Closed(object sender, EventArgs e)
		{
			Reset();
		}

		private void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			Reset();
		}

		private void Reset()
		{
			_outOfCombatTimer.Stop();
			_outOfCombatTimerLong.Stop();
			_inCombatTimer.Stop();
			CurrentState = State.StandBy;
		}
	}
}
