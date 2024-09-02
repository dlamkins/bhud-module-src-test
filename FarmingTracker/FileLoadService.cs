using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class FileLoadService
	{
		private readonly string _modelFilePath;

		public FileLoadService(string modelFilePath)
		{
			_modelFilePath = modelFilePath;
		}

		public async Task<Model> LoadModelFromFile()
		{
			return File.Exists(_modelFilePath) ? (await LoadModelFromModuleFolder(_modelFilePath)) : new Model();
		}

		private static async Task<Model> LoadModelFromModuleFolder(string modelFilePath)
		{
			try
			{
				return FileModelService.CreateModel(JsonConvert.DeserializeObject<FileModel>(await GetFileContentAndThrowIfFileEmpty(modelFilePath)));
			}
			catch (Exception e)
			{
				Module.Logger.Error(e, "Error: Failed to load local model from file. fallback: use empty Stats :(");
				return new Model();
			}
		}

		private static async Task<string> GetFileContentAndThrowIfFileEmpty(string filePath)
		{
			using FileStream fileStream = File.OpenRead(filePath);
			using StreamReader streamReader = new StreamReader(fileStream);
			string obj = await streamReader.ReadToEndAsync();
			if (string.IsNullOrWhiteSpace(obj))
			{
				throw new Exception("file is empty!");
			}
			return obj;
		}
	}
}
