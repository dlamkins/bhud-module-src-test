using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using GatheringTools.ToolSearch.Model;
using Newtonsoft.Json;

namespace GatheringTools.ToolSearch.Services
{
	public class FileService
	{
		public static async Task<List<GatheringTool>> GetAllGatheringToolsFromFile(ContentsManager contentsManager, Logger logger)
		{
			try
			{
				using Stream fileStream = contentsManager.GetFileStream("toolSearch\\gatheringTools.json");
				using StreamReader streamReader = new StreamReader(fileStream);
				return JsonConvert.DeserializeObject<List<GatheringTool>>(await streamReader.ReadToEndAsync());
			}
			catch (Exception e)
			{
				logger.Error(e, "Failed to read ref\\toolSearch\\gatheringTools.json. :(");
				return new List<GatheringTool>();
			}
		}
	}
}
