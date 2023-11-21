using Estreya.BlishHUD.Shared.Models.GW2API.Converter;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest
{
	public class PointOfInterest
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		[JsonConverter(typeof(CoordinatesConverter))]
		public Coordinates2 Coordinates { get; set; }

		public PoiType Type { get; set; }

		public string ChatLink { get; set; } = string.Empty;


		public RenderUrl? Icon { get; set; }

		public ContinentDetails Continent { get; set; }

		public ContinentFloorDetails Floor { get; set; }

		public ContinentFloorRegionDetails Region { get; set; }

		public ContinentFloorRegionMapDetails Map { get; set; }

		public PointOfInterest()
		{
		}

		public PointOfInterest(ContinentFloorRegionMapPoi poi)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			Id = poi.get_Id();
			Name = poi.get_Name();
			Coordinates = poi.get_Coord();
			Type = ApiEnum<PoiType>.op_Implicit(poi.get_Type());
			ChatLink = poi.get_ChatLink();
			Icon = poi.get_Icon();
			Type = poi.get_Type().get_Value();
		}

		public static implicit operator ContinentFloorRegionMapPoi(PointOfInterest poi)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			ContinentFloorRegionMapPoi val = new ContinentFloorRegionMapPoi();
			val.set_Id(poi.Id);
			val.set_Name(poi.Name);
			val.set_Coord(poi.Coordinates);
			val.set_Type(ApiEnum<PoiType>.op_Implicit(poi.Type));
			val.set_ChatLink(poi.ChatLink);
			val.set_Icon(poi.Icon);
			val.set_Floor(poi.Floor?.Id ?? 0);
			return val;
		}

		public override string ToString()
		{
			return "Continent: " + (Continent?.Name ?? "Unknown") + " - Map: " + (Map?.Name ?? "Unknown") + " - Region: " + (Region?.Name ?? "Unknown") + " - Floor: " + (Floor?.Id.ToString() ?? "Unknown") + " - Name: " + Name;
		}
	}
}
