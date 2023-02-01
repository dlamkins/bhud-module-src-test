using Estreya.BlishHUD.Shared.Models.GW2API.Converter;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest
{
	public class ContinentDetails
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		[JsonConverter(typeof(CoordinatesConverter))]
		public Coordinates2 ContinentDims { get; set; }

		public int MinZoom { get; set; }

		public int MaxZoom { get; set; }

		public ContinentDetails()
		{
		}

		public ContinentDetails(Continent continent)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Id = continent.get_Id();
			Name = continent.get_Name();
			ContinentDims = continent.get_ContinentDims();
			MinZoom = continent.get_MinZoom();
			MaxZoom = continent.get_MaxZoom();
		}
	}
}
