using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class MapStates : ManagedState
	{
		public readonly struct MapDetails
		{
			public Rectangle ContinentRect { get; }

			public Rectangle MapRect { get; }

			public MapDetails(Rectangle continentRect, Rectangle mapRect)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				ContinentRect = continentRect;
				MapRect = mapRect;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<MapStates>();

		private readonly Dictionary<int, MapDetails> _mapDetails = new Dictionary<int, MapDetails>();

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
					Logger.Warn(ex, "Failed to pull map data from the Gw2 API.  Trying again in 30 seconds.");
					await Task.Yield();
					await Task.Delay(30000);
					await LoadMapData(remainingAttempts - 1);
				}
				else if (ex is TooManyRequestsException)
				{
					Logger.Warn(ex, "After multiple attempts no map data could be loaded due to being rate limited by the API.");
				}
				else
				{
					Logger.Error(ex, "Final attempt to pull map data from the Gw2 API failed.  This session won't have map data.");
				}
			}
			if (maps == null)
			{
				return;
			}
			lock (_mapDetails)
			{
				foreach (Map map in (IEnumerable<Map>)maps)
				{
					_mapDetails[map.get_Id()] = new MapDetails(map.get_ContinentRect(), map.get_MapRect());
				}
			}
			await Reload();
		}

		public void EventCoordsToMapCoords(double eventCoordsX, double eventCoordsY, out double outX, out double outY)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			if (_currentMapDetails.HasValue)
			{
				Rectangle val = _currentMapDetails.Value.ContinentRect;
				Coordinates2 topLeft = ((Rectangle)(ref val)).get_TopLeft();
				double x = ((Coordinates2)(ref topLeft)).get_X();
				double num = eventCoordsX * 39.37007874015748;
				val = _currentMapDetails.Value.MapRect;
				topLeft = ((Rectangle)(ref val)).get_TopLeft();
				double num2 = num - ((Coordinates2)(ref topLeft)).get_X();
				val = _currentMapDetails.Value.MapRect;
				double num3 = num2 / ((Rectangle)(ref val)).get_Width();
				val = _currentMapDetails.Value.ContinentRect;
				outX = x + num3 * ((Rectangle)(ref val)).get_Width();
				val = _currentMapDetails.Value.ContinentRect;
				topLeft = ((Rectangle)(ref val)).get_TopLeft();
				double y = ((Coordinates2)(ref topLeft)).get_Y();
				double num4 = eventCoordsY * 39.37007874015748;
				val = _currentMapDetails.Value.MapRect;
				topLeft = ((Rectangle)(ref val)).get_TopLeft();
				double num5 = 0.0 - (num4 - ((Coordinates2)(ref topLeft)).get_Y());
				val = _currentMapDetails.Value.MapRect;
				double num6 = num5 / ((Rectangle)(ref val)).get_Height();
				val = _currentMapDetails.Value.ContinentRect;
				outY = y + num6 * ((Rectangle)(ref val)).get_Height();
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
