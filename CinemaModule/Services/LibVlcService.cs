using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;

namespace CinemaModule.Services
{
	internal class LibVlcService
	{
		private static readonly Logger Logger = Logger.GetLogger<LibVlcService>();

		private const string LibVlcVersion = "1";

		private const string VersionFileName = ".libvlc_version";

		private readonly ContentsManager _contentsManager;

		public LibVlcService(ContentsManager contentsManager)
		{
			_contentsManager = contentsManager;
		}

		public async Task ExtractAsync(string targetDir)
		{
			if (IsUpdateRequired(targetDir))
			{
				Logger.Info("LibVLC update required. Clearing existing files...");
				ClearDirectory(targetDir);
			}
			IEnumerable<string> files = LoadFileList();
			bool anyExtracted = false;
			foreach (string file in files)
			{
				string targetPath = Path.Combine(targetDir, file.Replace("libvlc/", ""));
				if (!File.Exists(targetPath))
				{
					await ExtractFileAsync(file, targetPath);
					anyExtracted = true;
				}
			}
			if (anyExtracted)
			{
				WriteVersionFile(targetDir);
			}
		}

		private bool IsUpdateRequired(string targetDir)
		{
			string versionFilePath = Path.Combine(targetDir, ".libvlc_version");
			if (!File.Exists(versionFilePath))
			{
				if (Directory.Exists(targetDir))
				{
					return Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories).Length != 0;
				}
				return false;
			}
			try
			{
				return File.ReadAllText(versionFilePath).Trim() != "1";
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to read LibVLC version file");
				return true;
			}
		}

		private void WriteVersionFile(string targetDir)
		{
			try
			{
				File.WriteAllText(Path.Combine(targetDir, ".libvlc_version"), "1");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to write LibVLC version file");
			}
		}

		private void ClearDirectory(string targetDir)
		{
			try
			{
				if (!Directory.Exists(targetDir))
				{
					return;
				}
				string[] files = Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories);
				foreach (string file in files)
				{
					try
					{
						File.Delete(file);
					}
					catch (Exception ex2)
					{
						Logger.Warn("Failed to delete file " + file + ": " + ex2.Message);
					}
				}
				files = Directory.GetDirectories(targetDir, "*", SearchOption.AllDirectories);
				foreach (string dir in files)
				{
					try
					{
						if (Directory.Exists(dir) && Directory.GetFiles(dir).Length == 0)
						{
							Directory.Delete(dir, recursive: false);
						}
					}
					catch (Exception ex3)
					{
						Logger.Warn("Failed to delete directory " + dir + ": " + ex3.Message);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to clear LibVLC directory");
			}
		}

		private async Task ExtractFileAsync(string sourceFile, string targetPath)
		{
			using Stream stream = _contentsManager.GetFileStream(sourceFile);
			if (stream != null)
			{
				string targetDirectory = Path.GetDirectoryName(targetPath);
				if (!Directory.Exists(targetDirectory))
				{
					Directory.CreateDirectory(targetDirectory);
				}
				using FileStream fileStream = File.Create(targetPath);
				await stream.CopyToAsync(fileStream);
			}
			else
			{
				Logger.Warn("ContentsManager returned null for: " + sourceFile);
			}
		}

		[IteratorStateMachine(typeof(_003CLoadFileList_003Ed__10))]
		private IEnumerable<string> LoadFileList()
		{
			return new _003CLoadFileList_003Ed__10(-2)
			{
				_003C_003E4__this = this
			};
		}

		public static string GetBinPath(string libvlcDir)
		{
			return Path.Combine(libvlcDir, "win-x64");
		}
	}
}
