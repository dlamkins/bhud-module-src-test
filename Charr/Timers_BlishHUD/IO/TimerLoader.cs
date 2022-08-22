using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Blish_HUD.Content;
using Charr.Timers_BlishHUD.Pathing.Content;

namespace Charr.Timers_BlishHUD.IO
{
	public class TimerLoader : IDisposable
	{
		private HashSet<string> _normalTimerFiles;

		private Dictionary<string, HashSet<ZipArchiveEntry>> _zipTimerFileEntries;

		private DirectoryReader _directoryReader;

		private readonly Dictionary<string, SortedZipArchiveReader> _zipDataReaders;

		private PathableResourceManager _directoryResourceManager;

		private Dictionary<string, PathableResourceManager> _zipResourceManagers;

		public string TimerTimerDirectory { get; set; }

		public TimerLoader(string timerDirectory)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			TimerTimerDirectory = timerDirectory;
			_normalTimerFiles = new HashSet<string>();
			_directoryReader = new DirectoryReader(timerDirectory);
			_directoryResourceManager = new PathableResourceManager((IDataReader)(object)_directoryReader);
			_zipTimerFileEntries = new Dictionary<string, HashSet<ZipArchiveEntry>>();
			_zipDataReaders = new Dictionary<string, SortedZipArchiveReader>();
			_zipResourceManagers = new Dictionary<string, PathableResourceManager>();
		}

		public void LoadFiles(Action<TimerStream> loadFileFunc)
		{
			_normalTimerFiles.UnionWith(Directory.GetFiles(TimerTimerDirectory, "*.bhtimer", SearchOption.AllDirectories));
			foreach (string file in _normalTimerFiles)
			{
				loadFileFunc(new TimerStream(_directoryReader.GetFileStream(file), _directoryResourceManager, file));
			}
			string[] files = Directory.GetFiles(TimerTimerDirectory, "*.zip", SearchOption.AllDirectories);
			foreach (string zipFile in files)
			{
				if (!_zipDataReaders.TryGetValue(zipFile, out var zipDataReader))
				{
					zipDataReader = new SortedZipArchiveReader(zipFile);
					_zipDataReaders.Add(zipFile, zipDataReader);
					_zipResourceManagers.Add(zipFile, new PathableResourceManager((IDataReader)(object)zipDataReader));
					_zipTimerFileEntries.Add(zipFile, new HashSet<ZipArchiveEntry>());
				}
				_zipTimerFileEntries[zipFile].UnionWith(zipDataReader.GetValidFileEntries(".bhtimer"));
				foreach (ZipArchiveEntry entry in _zipTimerFileEntries[zipFile])
				{
					loadFileFunc(new TimerStream(zipDataReader.GetFileStream(entry.get_Name()), _zipResourceManagers[zipFile], entry.get_Name(), isFromZip: true, zipFile));
				}
			}
		}

		public void ReloadFile(Action<TimerStream> loadFileFunc, string timerFileName)
		{
			if (_normalTimerFiles.Contains(timerFileName))
			{
				loadFileFunc(new TimerStream(_directoryReader.GetFileStream(timerFileName), _directoryResourceManager, timerFileName));
			}
		}

		public void ReloadFile(Action<TimerStream> loadFileFunc, string zipFile, string timerFileName)
		{
			if (_zipTimerFileEntries.ContainsKey(zipFile))
			{
				SortedZipArchiveReader zipDataReader = _zipDataReaders[zipFile];
				loadFileFunc(new TimerStream(zipDataReader.GetFileStream(timerFileName), _zipResourceManagers[zipFile], timerFileName, isFromZip: true, zipFile));
			}
		}

		public void Dispose()
		{
			_directoryResourceManager?.Dispose();
			foreach (PathableResourceManager value in _zipResourceManagers.Values)
			{
				value.Dispose();
			}
		}
	}
}
