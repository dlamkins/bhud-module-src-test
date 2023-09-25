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
				SaveJson(categories, Category.VERSION, _categoriesFile);
			}
			catch (Exception)
			{
			}
		}

		private void SaveJson<T>(T json, string version, string file)
		{
			try
			{
				string serialized = JsonConvert.SerializeObject(new JsonVersionWrapper<T>(version, json));
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
				JsonVersionWrapper<T>? jsonVersionWrapper = JsonConvert.DeserializeObject<JsonVersionWrapper<T>>(File.ReadAllText(file));
				Logger.Debug("Successfully loaded json from " + file);
				return jsonVersionWrapper!.Data;
			}
			catch (FileNotFoundException ex)
			{
				Logger.Warn("File " + file + " not found");
				throw ex;
			}
			catch (JsonException e2)
			{
				Logger.Error("Failed to deserialize json!");
				Logger.Error(e2.Message);
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
