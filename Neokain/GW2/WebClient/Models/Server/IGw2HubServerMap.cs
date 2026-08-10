using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.WebClient.Models.Maps;

namespace Neokain.GW2.WebClient.Models.Server
{
	internal interface IGw2HubServerMap
	{
		Task<Gw2MapDto?> GetMap(int mapId);

		Task<List<Gw2MapDto>> GetMaps(List<int> mapIds);

		Task<string?> GetMapName(int mapId);

		Task EnsureMapsExist(List<int> mapIds);

		Task<Gw2ContinentDto?> GetContinent(int continentId);

		Task<List<Gw2ContinentDto>> GetContinents();

		Task<CoordinateDto> ConvertMapToContinent(int mapId, double mapX, double mapY);

		Task<CoordinateDto> ConvertContinentToMap(int mapId, double continentX, double continentY);
	}
}
