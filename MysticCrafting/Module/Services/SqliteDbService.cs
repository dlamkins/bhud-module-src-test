using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;

namespace MysticCrafting.Module.Services
{
	public class SqliteDbService : ISqliteDbService
	{
		private readonly Logger _logger = Logger.GetLogger<DataService>();

		private const string DirectoryName = "mystic_crafting";

		private const string DatabaseFileName = "data.db";

		private readonly DirectoriesManager _directoriesManager;

		public string DatabaseFilePath => GetFilePath("data.db");

		public string DatabaseFileResourceName => "MysticCrafting.Module.EmbeddedResources.data.db";

		private string BaseDirectory => _directoriesManager.GetFullDirectoryPath("mystic_crafting");

		public SqliteDbService(DirectoriesManager directoriesManager)
		{
			_directoriesManager = directoriesManager;
		}

		public string GetFilePath(string fileName)
		{
			return Path.Combine(BaseDirectory, fileName);
		}

		public async Task InitializeDatabaseFile()
		{
			if (!NewDatabaseVersionAvailable())
			{
				await Task.CompletedTask;
				return;
			}
			using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DatabaseFileResourceName);
			if (resourceStream == null)
			{
				_logger.Error("Could not find embedded resource data.db");
				return;
			}
			using FileStream file = new FileStream(GetFilePath("data.db"), FileMode.Create, FileAccess.Write);
			await resourceStream.CopyToAsync(file);
		}

		private bool NewDatabaseVersionAvailable()
		{
			if (!File.Exists(DatabaseFilePath))
			{
				return true;
			}
			using FileStream stream = File.OpenRead(DatabaseFilePath);
			using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DatabaseFileResourceName);
			if (resourceStream == null)
			{
				_logger.Error("Could not find embedded resource data.db");
				return false;
			}
			int sqliteUserVersion = GetSqliteUserVersion(stream);
			int embeddedVersion = GetSqliteUserVersion(resourceStream);
			return sqliteUserVersion != embeddedVersion;
		}

		private int GetSqliteUserVersion(Stream stream)
		{
			try
			{
				byte[] buffer = new byte[4];
				stream.Position = 60L;
				stream.Read(buffer, 0, buffer.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(buffer);
				}
				return BitConverter.ToInt32(buffer, 0);
			}
			catch (Exception ex)
			{
				_logger.Error("Could not get user version from SQLite database: " + ex.Message);
			}
			return 0;
		}
	}
}
