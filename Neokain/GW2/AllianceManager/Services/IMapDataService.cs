using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Maps;

namespace Neokain.GW2.AllianceManager.Services
{
	public interface IMapDataService
	{
		Task<Gw2MapDto> GetMapAsync(int mapId);

		Task<string> GetMapNameAsync(int mapId);

		Task PreloadMapsAsync(IEnumerable<int> mapIds);

		void ClearCache();
	}
}
