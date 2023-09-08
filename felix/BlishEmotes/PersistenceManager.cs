using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;

namespace felix.BlishEmotes
{
	internal class PersistenceManager
	{
		private static readonly Logger Logger = Logger.GetLogger<PersistenceManager>();

		private string _baseDirectoryPath;

		private string _categoriesFile => Path.Combine(_baseDirectoryPath, "categories.json");

		public PersistenceManager(DirectoriesManager directoriesManager)
		{
			IReadOnlyList<string> registeredDirectories = directoriesManager.RegisteredDirectories;
			if (registeredDirectories.Count == 0)
			{
				Logger.Fatal("No directories registered!");
				throw new Exception("Failed to initialize - No directories registered");
			}
			if (registeredDirectories.Count != 1)
			{
				Logger.Fatal($"Wrong number of registered directories: {registeredDirectories.Count}");
				throw new Exception("Failed to initialize - Wrong number of registered directories");
			}
			_baseDirectoryPath = directoriesManager.GetFullDirectoryPath(registeredDirectories[0]);
		}

		public List<Category> LoadCategories()
		{
			try
			{
				return LoadJson<List<Category>>(_categoriesFile);
			}
			catch (FileNotFoundException ex)
			{
				Logger.Debug("Category file not found.");
				throw ex;
			}
			catch (Exception)
			{
				return new List<Category>();
			}
		}

		public void SaveCategories(List<Category> categories)
		{
			try
			{
				SaveJson(categories, _categoriesFile);
			}
			catch (Exception)
			{
			}
		}

		private void SaveJson<T>(T json, string file)
		{
			try
			{
				string serialized = JsonConvert.SerializeObject(json);
				File.WriteAllText(file, serialized);
				Logger.Debug("Successfully saved json to " + file);
			}
			catch (JsonException e2)
			{
				Logger.Error("Failed to serialize json!");
				Logger.Error(e2.Message);
				Logger.Debug(e2.StackTrace);
				throw e2;
			}
			catch (Exception e)
			{
				Logger.Error("Failed to write json file " + file + " due to " + e.GetType().FullName);
				Logger.Error(e.Message);
				Logger.Debug(e.StackTrace);
				throw e;
			}
		}

		private T LoadJson<T>(string file)
		{
			try
			{
				T? result = JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
				Logger.Debug("Successfully loaded json from " + file);
				return result;
			}
			catch (FileNotFoundException ex)
			{
				Logger.Warn("File " + file + " not found");
				throw ex;
			}
			catch (JsonException e2)
			{
				Logger.Error("Failed to deserialize json!");
				Logger.Debug(e2.Message);
				Logger.Debug(e2.StackTrace);
				throw e2;
			}
			catch (Exception e)
			{
				Logger.Error("Failed to read json file " + file + " due to " + e.GetType().FullName);
				Logger.Error(e.Message);
				Logger.Debug(e.StackTrace);
				throw e;
			}
		}
	}
}
