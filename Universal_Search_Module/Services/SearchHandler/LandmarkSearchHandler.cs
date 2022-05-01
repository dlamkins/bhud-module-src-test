using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;
using Universal_Search_Module.Controls.SearchResultItems;
using Universal_Search_Module.Models;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module.Services.SearchHandler
{
	public class LandmarkSearchHandler : SearchHandler<Landmark>
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly HashSet<Landmark> _landmarks = new HashSet<Landmark>();

		public override string Name => Common.SearchHandler_Landmarks;

		public override string Prefix => "l";

		protected override HashSet<Landmark> SearchItems => _landmarks;

		public LandmarkSearchHandler(Gw2ApiManager gw2ApiManager)
		{
			_gw2ApiManager = gw2ApiManager;
		}

		public override async Task Initialize(Action<string> progress)
		{
			foreach (int floorId in await _gw2ApiManager.get_Gw2ApiClient().V2.Continents[1].Floors.IdsAsync())
			{
				progress(string.Format(Common.SearchHandler_Landmarks_FloorLoading, floorId));
				foreach (KeyValuePair<int, ContinentFloorRegion> region in (await _gw2ApiManager.get_Gw2ApiClient().V2.Continents[1].Floors[floorId].GetAsync()).Regions)
				{
					foreach (KeyValuePair<int, ContinentFloorRegionMap> mapPair in region.Value.Maps)
					{
						foreach (Landmark landmark2 in from landmark in mapPair.Value.PointsOfInterest.Values
							where landmark.Name != null
							select landmark into x
							select new Landmark
							{
								PointOfInterest = x,
								Map = mapPair.Value
							})
						{
							Landmark existingLandmark = _landmarks.FirstOrDefault((Landmark x) => x.PointOfInterest.ChatLink == landmark2.PointOfInterest.ChatLink);
							if (existingLandmark == null)
							{
								_landmarks.Add(landmark2);
							}
							else if (!existingLandmark.Map.PointsOfInterest.Any<KeyValuePair<int, ContinentFloorRegionMapPoi>>((KeyValuePair<int, ContinentFloorRegionMapPoi> x) => x.Value.Type.ToEnum() == PoiType.Waypoint) && landmark2.Map.PointsOfInterest.Any<KeyValuePair<int, ContinentFloorRegionMapPoi>>((KeyValuePair<int, ContinentFloorRegionMapPoi> x) => x.Value.Type.ToEnum() == PoiType.Waypoint))
							{
								_landmarks.Remove(existingLandmark);
								_landmarks.Add(landmark2);
							}
						}
					}
				}
			}
		}

		protected override SearchResultItem CreateSearchResultItem(Landmark item)
		{
			IEnumerable<Landmark> possibleWaypoints = _landmarks.Where((Landmark x) => x.Map == item.Map && x.PointOfInterest.Type.ToEnum() == PoiType.Waypoint);
			if (!possibleWaypoints.Any())
			{
				possibleWaypoints = _landmarks.Where((Landmark x) => x.PointOfInterest.Type.ToEnum() == PoiType.Waypoint);
			}
			return new LandmarkSearchResultItem(possibleWaypoints)
			{
				Landmark = item
			};
		}

		protected override string GetSearchableProperty(Landmark item)
		{
			return item.PointOfInterest.Name;
		}
	}
}
