using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Blish_HUD;

namespace Nekres.ChatMacros.Core
{
	internal static class FileUtil
	{
		public static bool Exists(string filePath, out string path, Logger logger = null, params string[] basePaths)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger<BlishHud>();
			}
			path = string.Empty;
			if (filePath.IsNullOrEmpty())
			{
				return false;
			}
			try
			{
				filePath = filePath.Replace("%20", " ");
				if (filePath.IsPathFullyQualified())
				{
					path = filePath;
					return File.Exists(filePath);
				}
				filePath = filePath.TrimStart('/');
				filePath = filePath.TrimStart('\\');
				filePath = filePath.Replace("/", "\\");
				for (int i = 0; i < basePaths.Length; i++)
				{
					string testPath = Path.Combine(basePaths[i], filePath);
					testPath = Path.GetFullPath(testPath);
					if (File.Exists(testPath))
					{
						path = testPath;
						return true;
					}
				}
			}
			catch (Exception e)
			{
				logger.Info(e, e.Message);
			}
			return false;
		}

		public static bool TryReadAllLines(string filePath, out IReadOnlyList<string> lines, Logger logger = null, params string[] basePaths)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger<BlishHud>();
			}
			List<string> result = (List<string>)(lines = new List<string>());
			if (!Exists(filePath, out var path, logger, basePaths))
			{
				return false;
			}
			try
			{
				result.AddRange(ReadAllLines(path));
			}
			catch (Exception e)
			{
				logger.Info(e, e.Message);
				return false;
			}
			return true;
		}

		public static void OpenExternally(string path)
		{
			using Process fileopener = new Process();
			fileopener.StartInfo.FileName = "explorer";
			fileopener.StartInfo.Arguments = "\"" + path + "\"";
			fileopener.Start();
		}

		public static IEnumerable<string> ReadAllLines(string path)
		{
			using FileStream fil = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using StreamReader sr = new StreamReader(fil);
			List<string> file = new List<string>();
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				if (line != null)
				{
					file.Add(line);
				}
			}
			return file;
		}
	}
}
