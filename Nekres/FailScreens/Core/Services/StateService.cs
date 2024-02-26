using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
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
				FailScreensModule.Logger.Info(e, e.Message);
			}
		}

		public void Update()
		{
			if (DateTime.UtcNow.Subtract(_lastLockFileCheck).TotalMilliseconds > 100.0)
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
		}
	}
}
