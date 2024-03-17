using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Estreya.BlishHUD.Shared.Services.Audio
{
	public class AudioService : ManagedService
	{
		public enum AudioPlaybackResult
		{
			Success,
			NotFound,
			Busy,
			Failed
		}

		private AsyncLock _audioLock = new AsyncLock();

		private const string SOUND_EFFECT_FILE_EXTENSION = ".wav";

		private const string AUDIO_FOLDER_NAME = "audio";

		private const string SOUND_FILE_INDEX_SEPARATOR = "_";

		private readonly string _rootPath;

		private List<string> _registeredSubfolders;

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
			_registeredSubfolders = new List<string>();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_registeredSubfolders?.Clear();
			_registeredSubfolders = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		public async Task<AudioPlaybackResult> PlaySoundFromFile(string soundName, string subfolder = null, bool failSilent = false)
		{
			if (subfolder != null && !_registeredSubfolders.Contains(subfolder))
			{
				throw new DirectoryNotFoundException("The directory \"" + subfolder + "\" is not registered.");
			}
			string[] files = await GetSoundFiles(soundName, subfolder ?? string.Empty);
			if (files.Length == 0)
			{
				return AudioPlaybackResult.NotFound;
			}
			string file = files.PickRandom();
			return await PlaySoundFromPath(file, failSilent);
		}

		public async Task<AudioPlaybackResult> PlaySoundFromPath(string filePath, bool failSilent = false)
		{
			if (_playRemainingAttempts <= 0)
			{
				return AudioPlaybackResult.Failed;
			}
			if (GameService.GameIntegration.get_Audio().get_AudioDevice() == null)
			{
				return AudioPlaybackResult.Failed;
			}
			if (Path.GetExtension(filePath) != ".wav")
			{
				if (failSilent)
				{
					return AudioPlaybackResult.Failed;
				}
				throw new ArgumentException("filePath", "Sound file does not has the required format \".wav\".");
			}
			if (!File.Exists(filePath))
			{
				if (failSilent)
				{
					return AudioPlaybackResult.Failed;
				}
				throw new FileNotFoundException("Soundfile does not exist.");
			}
			try
			{
				if (_audioLock.IsFree())
				{
					using (await _audioLock.LockAsync())
					{
						Logger.Debug("AudioService started playing sound...");
						using FileStream stream = FileUtil.ReadStream(filePath);
						SoundEffect se = SoundEffect.FromStream((Stream)stream);
						SoundEffectInstance sei = se.CreateInstance();
						sei.set_Volume(GameService.GameIntegration.get_Audio().get_Volume());
						sei.Play();
						try
						{
							await AsyncHelper.WaitUntil(() => (int)sei.get_State() == 2, TimeSpan.FromSeconds(30.0), 500);
							Logger.Debug("AudioService finished playing sound.");
							_playRemainingAttempts = 3;
							return AudioPlaybackResult.Success;
						}
						catch (TimeoutException)
						{
							Logger.Debug("AudioService could not finish playing sound in allocated timeout. This could be the cause of long sound files.");
							return AudioPlaybackResult.Failed;
						}
					}
				}
				Logger.Debug("AudioService is currently busy playing. Skipping");
				return AudioPlaybackResult.Busy;
			}
			catch (Exception ex)
			{
				_playRemainingAttempts--;
				Logger.Warn(ex, "Failed to play sound effect.");
				return AudioPlaybackResult.Failed;
			}
		}

		public void PlaySoundFromRef(string soundName)
		{
			GameService.Content.PlaySoundEffectByName(soundName);
		}

		public Task RegisterSubfolder(params string[] subfolders)
		{
			foreach (string subfolder in subfolders)
			{
				Directory.CreateDirectory(Path.Combine(FullPath, subfolder));
				_registeredSubfolders.Add(subfolder);
			}
			return Task.CompletedTask;
		}

		public Task<string[]> GetSoundFiles(string soundName, string subfolder = null)
		{
			if (subfolder != null && !_registeredSubfolders.Contains(subfolder))
			{
				throw new DirectoryNotFoundException("The directory \"" + subfolder + "\" is not registered.");
			}
			List<string> files = new List<string>();
			string mainFileName = Path.Combine(FullPath, subfolder ?? string.Empty, soundName + ".wav");
			if (File.Exists(mainFileName))
			{
				files.Add(mainFileName);
			}
			string dir = Path.Combine(FullPath, subfolder ?? string.Empty);
			if (Directory.Exists(dir))
			{
				files.AddRange(Directory.GetFiles(dir, soundName + "_*.wav"));
			}
			return Task.FromResult(files.ToArray());
		}

		private async Task<int> GetNextSoundFileIndex(string soundName, string subfolder = null)
		{
			List<int> indexes = (from split in (from f in await GetSoundFiles(soundName, subfolder)
					select Path.GetFileNameWithoutExtension(f).Split(new string[1] { "_" }, StringSplitOptions.RemoveEmptyEntries) into split
					select (split.Length < 2) ? new string[2]
					{
						split[0],
						"0"
					} : split).Select(delegate(string[] split)
				{
					if (!int.TryParse(split[split.Length - 1], out var _))
					{
						List<string> list = split.ToList();
						list.Add("0");
						return list.ToArray();
					}
					return split;
				})
				select split[split.Length - 1] into i
				select Convert.ToInt32(i) into i
				orderby i
				select i).ToList();
			return (indexes.Count == 0) ? (-1) : (indexes.LastOrDefault() + 1);
		}

		public async Task UploadFile(string sourceFilePath, string destinationFileName, string subfolder = null)
		{
			if (Path.GetExtension(sourceFilePath) != ".wav")
			{
				throw new NotSupportedException("The source file is not in the .wav format.");
			}
			if (subfolder != null && !_registeredSubfolders.Contains(subfolder))
			{
				throw new DirectoryNotFoundException("The directory \"" + subfolder + "\" is not registered.");
			}
			int newIndex = await GetNextSoundFileIndex(Path.GetFileNameWithoutExtension(destinationFileName), subfolder);
			if (newIndex != -1)
			{
				destinationFileName = string.Format("{0}{1}{2}", destinationFileName, "_", newIndex);
			}
			File.Copy(sourceFilePath, Path.Combine(FullPath, subfolder ?? string.Empty, destinationFileName + ".wav"), overwrite: true);
		}
	}
}
