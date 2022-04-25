using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI;

namespace Nekres.Musician
{
	internal class MusicSheetImporter : IDisposable
	{
		private readonly FileSystemWatcher _xmlWatcher;

		private readonly MusicSheetService _sheetService;

		private readonly IProgress<string> _loadingIndicator;

		public bool IsLoading { get; private set; }

		public string Log { get; private set; }

		public MusicSheetImporter(MusicSheetService sheetService, IProgress<string> loadingIndicator)
		{
			_sheetService = sheetService;
			_loadingIndicator = loadingIndicator;
			_xmlWatcher = new FileSystemWatcher(sheetService.CacheDir)
			{
				NotifyFilter = (NotifyFilters.FileName | NotifyFilters.LastWrite),
				Filter = "*.xml",
				EnableRaisingEvents = true
			};
			_xmlWatcher.Created += OnXmlCreated;
		}

		private async void OnXmlCreated(object sender, FileSystemEventArgs e)
		{
			await ConvertXml(e.FullPath);
		}

		public void Init()
		{
			Thread thread = new Thread(LoadSheetsInBackground);
			thread.IsBackground = true;
			thread.Start();
		}

		private async void LoadSheetsInBackground()
		{
			IsLoading = true;
			IEnumerable<string> initialFiles = from s in Directory.EnumerateFiles(_sheetService.CacheDir)
				where Path.GetExtension(s).Equals(".xml")
				select s;
			foreach (string filePath in initialFiles)
			{
				await ConvertXml(filePath, silent: true);
			}
			IsLoading = false;
			Log = null;
			_loadingIndicator.Report(null);
		}

		private async Task ConvertXml(string filePath, bool silent = false)
		{
			string log = "Importing " + Path.GetFileName(filePath) + "..";
			MusicianModule.Logger.Info(log);
			Log = log;
			_loadingIndicator.Report(log);
			MusicSheet musicSheet = MusicSheet.FromXml(filePath);
			if (musicSheet != null)
			{
				await FileUtil.DeleteAsync(filePath);
				await _sheetService.AddOrUpdate(musicSheet, silent);
			}
		}

		public void Dispose()
		{
			_xmlWatcher.Created -= OnXmlCreated;
			_xmlWatcher.Changed -= OnXmlCreated;
			_xmlWatcher.Dispose();
			_xmlWatcher?.Dispose();
			_sheetService?.Dispose();
		}
	}
}
