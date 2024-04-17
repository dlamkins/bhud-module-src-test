using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.FailScreens.Core.Services
{
	internal class StateService : IDisposable
	{
		public enum State
		{
			StandBy,
			Defeated
		}

		private string _lockFile = "silence.wav";

		private DateTime _lastLockFileCheck = DateTime.UtcNow.AddSeconds(10.0);

		public State CurrentState { get; private set; }

		public event EventHandler<ValueEventArgs<State>> StateChanged;

		public StateService()
		{
			GameService.ArcDps.get_Common().Activate();
			GameService.ArcDps.add_RawCombatEvent((EventHandler<RawCombatEventArgs>)ArcDps_RawCombatEvent);
		}

		private void ArcDps_RawCombatEvent(object sender, RawCombatEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Invalid comparison between Unknown and I4
			CombatEvent combatEvent = e.get_CombatEvent();
			if (((combatEvent != null) ? combatEvent.get_Ev() : null) != null && (int)e.get_EventType() == 1 && Convert.ToBoolean(e.get_CombatEvent().get_Src().get_Self()))
			{
				if ((int)e.get_CombatEvent().get_Ev().get_IsStateChange() == 4)
				{
					ChangeState(State.Defeated);
				}
				else if ((int)e.get_CombatEvent().get_Ev().get_IsStateChange() == 3)
				{
					ChangeState(State.StandBy);
				}
			}
		}

		public async Task SetupLockFiles(State state)
		{
			string relLockFilePath2 = $"{state}\\{_lockFile}";
			await FailScreensModule.Instance.ContentsManager.Extract("audio/" + _lockFile, Path.Combine(DirectoryUtil.get_MusicPath(), relLockFilePath2), overwrite: false);
			try
			{
				FailScreensModule.Logger.Info($"Creating {state} playlist.");
				string path = Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}.m3u");
				relLockFilePath2 += "\r\n";
				if (!File.Exists(path))
				{
					goto IL_01aa;
				}
				if (File.ReadAllText(path, Encoding.UTF8).Equals(relLockFilePath2))
				{
					FailScreensModule.Logger.Info($"{state} playlist already exists. Skipping.");
					return;
				}
				FailScreensModule.Logger.Info($"Storing existing {state} playlist as backup.");
				File.Copy(path, Path.Combine(DirectoryUtil.get_MusicPath(), $"{state}.backup.m3u"), overwrite: true);
				goto IL_01aa;
				IL_01aa:
				using FileStream file = File.Create(path);
				file.Position = 0L;
				byte[] content = Encoding.UTF8.GetBytes(relLockFilePath2);
				await file.WriteAsync(content, 0, content.Length);
				FailScreensModule.Logger.Info($"{state} playlist created.");
				ScreenNotification.ShowNotification($"{state} playlist created. Game restart required.", (NotificationType)1, (Texture2D)null, 4);
			}
			catch (Exception e)
			{
				FailScreensModule.Logger.Info(e, e.Message);
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
					ScreenNotification.ShowNotification($"{state} playlist reverted. Game restart required.", (NotificationType)1, (Texture2D)null, 4);
				}
				else if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception e)
			{
				FailScreensModule.Logger.Info(e, e.Message);
			}
		}

		public void Update()
		{
			if (!GameService.ArcDps.get_Running() && DateTime.UtcNow.Subtract(_lastLockFileCheck).TotalMilliseconds > 100.0)
			{
				_lastLockFileCheck = DateTime.UtcNow;
				CheckLockFile(State.Defeated);
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

		private void ChangeState(State state)
		{
			if (CurrentState != state)
			{
				CurrentState = state;
				this.StateChanged?.Invoke(this, new ValueEventArgs<State>(CurrentState));
			}
		}

		public void Dispose()
		{
			GameService.ArcDps.remove_RawCombatEvent((EventHandler<RawCombatEventArgs>)ArcDps_RawCombatEvent);
		}
	}
}
