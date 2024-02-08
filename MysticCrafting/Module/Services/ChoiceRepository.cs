using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Services
{
	public class ChoiceRepository : IChoiceRepository, IRepository
	{
		public const string ItemSourceType = "ItemSource";

		public const string VendorType = "Vendor";

		public const string TradingPostType = "TradingPost";

		private readonly IDataService _dataService;

		private string DirectoryPath = "Local";

		public bool LocalOnly => true;

		public IList<NodeChoice> Choices { get; set; } = new List<NodeChoice>();


		public bool Loaded { get; set; }

		public string FileName => DirectoryPath + "\\choices.json";

		private Timer _saveTimer { get; set; }

		public int SaveInterval { get; set; } = 3000;


		public ChoiceRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			if (!File.Exists(_dataService.GetFilePath(FileName)))
			{
				string fullDirPath = _dataService.GetFilePath(DirectoryPath);
				if (!Directory.Exists(fullDirPath))
				{
					Directory.CreateDirectory(fullDirPath);
				}
			}
			else
			{
				Choices = (await _dataService.LoadFromFileAsync<IList<NodeChoice>>(FileName)) ?? new List<NodeChoice>();
			}
			InitializeTimer();
			return $"{Choices.Count} choices loaded";
		}

		public void InitializeTimer()
		{
			_saveTimer = new Timer(SaveInterval);
			_saveTimer.Elapsed += async delegate
			{
				_saveTimer.Stop();
				await _dataService.SaveFileAsync(FileName, Choices);
			};
		}

		public void SaveChoice(string id, string value, ChoiceType type)
		{
			NodeChoice existingChoice = Choices.FirstOrDefault((NodeChoice c) => c.UniqueId.Equals(id, StringComparison.InvariantCultureIgnoreCase) && c.Type.Equals(type));
			if (existingChoice != null)
			{
				existingChoice.Value = value;
			}
			else
			{
				Choices.Add(new NodeChoice
				{
					UniqueId = id,
					Value = value,
					Type = type
				});
			}
			SaveToFile();
		}

		public NodeChoice GetChoice(string uniqueId, ChoiceType type)
		{
			return Choices.FirstOrDefault((NodeChoice c) => c.UniqueId.Equals(uniqueId, StringComparison.InvariantCultureIgnoreCase) && c.Type.Equals(type));
		}

		private void SaveToFile()
		{
			if (_saveTimer == null)
			{
				InitializeTimer();
			}
			_saveTimer?.Stop();
			_saveTimer?.Start();
		}
	}
}
