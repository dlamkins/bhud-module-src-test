using Estreya.BlishHUD.EventTable.Models.GW2API.Converter;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.GW2API
{
	public class ContinentFloorRegionDetails
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		[JsonConverter(typeof(CoordinatesConverter))]
		public Coordinates2 LabelCoord { get; set; }

		[JsonConverter(typeof(TopDownRectangleConverter))]
		public Rectangle ContinentRect { get; set; }

		public ContinentFloorRegionDetails()
		{
		}

		public ContinentFloorRegionDetails(ContinentFloorRegion continentFloorRegion)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			Id = continentFloorRegion.get_Id();
			Name = continentFloorRegion.get_Name();
			LabelCoord = continentFloorRegion.get_LabelCoord();
			ContinentRect = continentFloorRegion.get_ContinentRect();
		}
	}
}
