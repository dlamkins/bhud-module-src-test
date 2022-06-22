using Estreya.BlishHUD.EventTable.Models.GW2API.Converter;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.GW2API
{
	public class ContinentFloorDetails
	{
		public int Id { get; set; }

		[JsonConverter(typeof(CoordinatesConverter))]
		public Coordinates2 TextureDims { get; set; }

		[JsonConverter(typeof(TopDownRectangleConverter))]
		public Rectangle ClampedView { get; set; }

		public ContinentFloorDetails()
		{
		}

		public ContinentFloorDetails(ContinentFloor continentFloor)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Id = continentFloor.get_Id();
			TextureDims = continentFloor.get_TextureDims();
			ClampedView = continentFloor.get_ClampedView();
		}
	}
}
