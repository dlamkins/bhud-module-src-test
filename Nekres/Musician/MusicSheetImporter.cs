using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules;
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
				NotifyFilter = NotifyFilters.LastWrite,
				Filter = "*.xml",
				EnableRaisingEvents = true
			};
			_xmlWatcher.Created += OnXmlCreated;
		}

		private async void OnXmlCreated(object sender, FileSystemEventArgs e)
		{
			await ImportFromFile(e.FullPath);
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
				if (!((Module)MusicianModule.ModuleInstance).get_Loaded())
				{
					break;
				}
				await ImportFromFile(filePath, silent: true);
			}
			IsLoading = false;
			Log = null;
			_loadingIndicator.Report(null);
		}

		private async Task ImportFromFile(string filePath, bool silent = false)
		{
			string log = "Importing " + Path.GetFileName(filePath) + "..";
			MusicianModule.Logger.Info(log);
			Log = log;
			_loadingIndicator.Report(log);
			MusicSheet sheet = MusicSheet.FromXml(filePath);
			if (sheet != null)
			{
				await FileUtil.DeleteAsync(filePath);
				await AddToDatabase(sheet, silent);
			}
		}

		internal async Task ImportFromStream(Stream stream, bool silent = false)
		{
			byte[] buffer = new byte[stream.Length];
			await stream.ReadAsync(buffer, 0, buffer.Length);
			if (MusicSheet.TryParseXml(Encoding.UTF8.GetString(buffer), out var sheet))
			{
				await AddToDatabase(sheet, silent);
				stream.Dispose();
			}
		}

		private async Task AddToDatabase(MusicSheet sheet, bool silent)
		{
			try
			{
				await _sheetService.AddOrUpdate(sheet, silent);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		public void Dispose()
		{
			_xmlWatcher.Created -= OnXmlCreated;
			_xmlWatcher?.Dispose();
		}
	}
}
