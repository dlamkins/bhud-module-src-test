using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Estreya.BlishHUD.Shared.Services.Audio
{
	public class AudioService : ManagedService
	{
		private const string SOUND_EFFECT_FILE_EXTENSION = ".wav";

		private const string AUDIO_FOLDER_NAME = "audio";

		private readonly string _rootPath;

		private int _playRemainingAttempts = 3;

		private string FullPath => Path.Combine(_rootPath, "audio");

		public AudioService(ServiceConfiguration configuration, string rootPath)
			: base(configuration)
		{
			if (string.IsNullOrWhiteSpace(rootPath))
			{
				throw new ArgumentNullException("rootPath");
			}
			_rootPath = rootPath;
		}

		protected override Task Initialize()
		{
			if (!Directory.Exists(FullPath))
			{
				Directory.CreateDirectory(FullPath);
			}
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		public void PlaySoundFromFile(string soundName, bool silent = false)
		{
			string filePath = Path.Combine(FullPath, soundName + ".wav");
			PlaySoundFromPath(filePath, silent);
		}

		public void PlaySoundFromPath(string filePath, bool silent = false)
		{
			if (_playRemainingAttempts <= 0 || GameService.GameIntegration.get_Audio().get_AudioDevice() == null)
			{
				return;
			}
			if (Path.GetExtension(filePath) != ".wav" && !silent)
			{
				throw new ArgumentException("filePath", "Sound file does not has the required format \".wav\".");
			}
			if (!File.Exists(filePath) && !silent)
			{
				throw new FileNotFoundException("Soundfile does not exist.");
			}
			try
			{
				using FileStream stream = FileUtil.ReadStream(filePath);
				SoundEffect.FromStream((Stream)stream).Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
				_playRemainingAttempts = 3;
			}
			catch (Exception ex)
			{
				_playRemainingAttempts--;
				Logger.Warn(ex, "Failed to play sound effect.");
			}
		}

		public void PlaySoundFromRef(string soundName)
		{
			GameService.Content.PlaySoundEffectByName(soundName);
		}
	}
}
