using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace BhModule.Community.Pathing.State
{
	public class MapStates : ManagedState
	{
		public struct MapDetails
		{
			public double ContinentRectTopLeftX { get; set; }

			public double ContinentRectTopLeftY { get; set; }

			public double MapRectTopLeftX { get; set; }

			public double MapRectTopLeftY { get; set; }

			public double MapRectWidth { get; set; }

			public double MapRectHeight { get; set; }

			public double ContinentRectWidth { get; set; }

			public double ContinentRectHeight { get; set; }

			public MapDetails()
			{
				ContinentRectTopLeftX = 0.0;
				ContinentRectTopLeftY = 0.0;
				MapRectTopLeftX = 0.0;
				MapRectTopLeftY = 0.0;
				MapRectWidth = 0.0;
				MapRectHeight = 0.0;
				ContinentRectWidth = 0.0;
				ContinentRectHeight = 0.0;
			}

			public MapDetails(Rectangle continentRect, Rectangle mapRect)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				Coordinates2 topLeft = ((Rectangle)(ref continentRect)).get_TopLeft();
				ContinentRectTopLeftX = ((Coordinates2)(ref topLeft)).get_X();
				topLeft = ((Rectangle)(ref continentRect)).get_TopLeft();
				ContinentRectTopLeftY = ((Coordinates2)(ref topLeft)).get_Y();
				topLeft = ((Rectangle)(ref mapRect)).get_TopLeft();
				MapRectTopLeftX = ((Coordinates2)(ref topLeft)).get_X();
				topLeft = ((Rectangle)(ref mapRect)).get_TopLeft();
				MapRectTopLeftY = ((Coordinates2)(ref topLeft)).get_Y();
				MapRectWidth = ((Rectangle)(ref mapRect)).get_Width();
				MapRectHeight = ((Rectangle)(ref mapRect)).get_Height();
				ContinentRectWidth = ((Rectangle)(ref continentRect)).get_Width();
				ContinentRectHeight = ((Rectangle)(ref continentRect)).get_Height();
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<MapStates>();

		private Dictionary<int, MapDetails> _mapDetails = new Dictionary<int, MapDetails>();

		private const double METERCONVERSION = 39.37007874015748;

		private MapDetails? _currentMapDetails;

		public MapStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		protected override async Task<bool> Initialize()
		{
			await LoadMapData();
			return true;
		}

		private async Task LoadMapData(int remainingAttempts = 2)
		{
			IApiV2ObjectList<Map> maps = null;
			try
			{
				maps = await ((IAllExpandableClient<Map>)(object)PathingModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).AllAsync(default(CancellationToken));
			}
			catch (Exception ex)
			{
				if (remainingAttempts > 0)
				{
					Logger.Warn(ex, "Failed to pull map data from the Gw2 API.  Trying again in 2 seconds.");
					await Task.Yield();
					await Task.Delay(500);
					await LoadMapData(remainingAttempts - 1);
				}
				else if (ex is TooManyRequestsException)
				{
					Logger.Warn(ex, "After multiple attempts no map data could be loaded due to being rate limited by the API.");
				}
				else
				{
					Logger.Warn(ex, "Final attempt to pull map data from the Gw2 API failed.  This session won't have map data.");
				}
			}
			if (maps == null && !_mapDetails.Any())
			{
				try
				{
					using Stream fs = _rootPackState.Module.ContentsManager.GetFileStream("fallback/mapDetails.json");
					using StreamReader sr = new StreamReader(fs);
					_mapDetails = JsonConvert.DeserializeObject<Dictionary<int, MapDetails>>(await sr.ReadToEndAsync());
				}
				catch (Exception ex2)
				{
					Logger.Warn(ex2, "Loadding fallback/mapDetails.json failed!");
				}
			}
			else if (maps != null)
			{
				lock (_mapDetails)
				{
					foreach (Map map in (IEnumerable<Map>)maps)
					{
						_mapDetails[map.get_Id()] = new MapDetails(map.get_ContinentRect(), map.get_MapRect());
					}
				}
			}
			await Reload();
		}

		public void EventCoordsToMapCoords(double eventCoordsX, double eventCoordsY, out double outX, out double outY)
		{
			if (_currentMapDetails.HasValue)
			{
				outX = _currentMapDetails.Value.ContinentRectTopLeftX + (eventCoordsX * 39.37007874015748 - _currentMapDetails.Value.MapRectTopLeftX) / _currentMapDetails.Value.MapRectWidth * _currentMapDetails.Value.ContinentRectWidth;
				outY = _currentMapDetails.Value.ContinentRectTopLeftY + (0.0 - (eventCoordsY * 39.37007874015748 - _currentMapDetails.Value.MapRectTopLeftY)) / _currentMapDetails.Value.MapRectHeight * _currentMapDetails.Value.ContinentRectHeight;
			}
			else
			{
				outX = 0.0;
				outY = 0.0;
			}
		}

		public override Task Reload()
		{
			lock (_mapDetails)
			{
				if (_mapDetails.TryGetValue(_rootPackState.CurrentMapId, out var mapDetails))
				{
					_currentMapDetails = mapDetails;
				}
				else
				{
					_currentMapDetails = null;
				}
			}
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override Task Unload()
		{
			_currentMapDetails = null;
			return Task.CompletedTask;
		}
	}
}
