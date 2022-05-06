using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using GatheringTools.ToolSearch.Model;
using Newtonsoft.Json;

namespace GatheringTools.ToolSearch.Services
{
	public class FileService
	{
		public static async Task<IEnumerable<GatheringTool>> GetAllGatheringToolsFromFiles(ContentsManager contentsManager, Logger logger)
		{
			Task<List<GatheringTool>> knownGatheringToolsTask = GetGatheringToolsFromFile("toolSearch\\data\\gatheringToolsFromV2ItemsApi.json", contentsManager, logger);
			Task<List<GatheringTool>> unknownGatheringToolsTask = GetGatheringToolsFromFile("toolSearch\\data\\gatheringToolsMissingInV2ItemsApi.json", contentsManager, logger);
			await Task.WhenAll<List<GatheringTool>>(knownGatheringToolsTask, unknownGatheringToolsTask);
			return knownGatheringToolsTask.Result.Concat(unknownGatheringToolsTask.Result);
		}

		private static async Task<List<GatheringTool>> GetGatheringToolsFromFile(string filePath, ContentsManager contentsManager, Logger logger)
		{
			try
			{
				using Stream fileStream = contentsManager.GetFileStream(filePath);
				using StreamReader streamReader = new StreamReader(fileStream);
				return JsonConvert.DeserializeObject<List<GatheringTool>>(await streamReader.ReadToEndAsync());
			}
			catch (Exception e)
			{
				logger.Error(e, "Failed to read ref\\" + filePath + ". :(");
				return new List<GatheringTool>();
			}
		}
	}
}
