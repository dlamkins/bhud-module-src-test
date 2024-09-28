using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class FileSaver
	{
		private readonly string _modelFilePath;

		public FileSaver(string modelFilePath)
		{
			_modelFilePath = modelFilePath;
		}

		public void SaveModelToFileSync(Model model)
		{
			try
			{
				string fileModelJson = SerializeModelToJson(model);
				File.WriteAllText(_modelFilePath, fileModelJson);
			}
			catch (Exception e)
			{
				Module.Logger.Error(e, "Error: Failed to save model to file. :(");
			}
		}

		public async Task SaveModelToFile(Model model)
		{
			try
			{
				string fileModelJson = SerializeModelToJson(model);
				await WriteFileAsync(_modelFilePath, fileModelJson);
			}
			catch (Exception e)
			{
				Module.Logger.Error(e, "Error: Failed to saving model to file. :(");
			}
		}

		public static async Task WriteFileAsync(string filePath, string fileContent)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			using StreamWriter streamWriter = new StreamWriter(filePath);
			await streamWriter.WriteAsync(fileContent);
			await streamWriter.FlushAsync();
		}

		private static string SerializeModelToJson(Model model)
		{
			return JsonConvert.SerializeObject((object)FileModelCreator.CreateFileModel(model));
		}
	}
}
