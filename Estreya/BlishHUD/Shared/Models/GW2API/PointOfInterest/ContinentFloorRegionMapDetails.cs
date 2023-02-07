using Estreya.BlishHUD.Shared.Models.GW2API.Converter;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest
{
	public class ContinentFloorRegionMapDetails
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public int MinLevel { get; set; }

		public int MaxLevel { get; set; }

		public int DefaultFloor { get; set; }

		[JsonConverter(typeof(CoordinatesConverter))]
		public Coordinates2 LabelCoord { get; set; }

		[JsonConverter(typeof(BottomUpRectangleConverter))]
		public Rectangle MapRect { get; set; }

		[JsonConverter(typeof(TopDownRectangleConverter))]
		public Rectangle ContinentRect { get; set; }

		public ContinentFloorRegionMapDetails()
		{
		}

		public ContinentFloorRegionMapDetails(ContinentFloorRegionMap continentFloorRegionMap)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			Id = continentFloorRegionMap.get_Id();
			Name = continentFloorRegionMap.get_Name();
			MinLevel = continentFloorRegionMap.get_MinLevel();
			MaxLevel = continentFloorRegionMap.get_MaxLevel();
			DefaultFloor = continentFloorRegionMap.get_DefaultFloor();
			LabelCoord = continentFloorRegionMap.get_LabelCoord();
			MapRect = continentFloorRegionMap.get_MapRect();
			ContinentRect = continentFloorRegionMap.get_ContinentRect();
		}
	}
}
