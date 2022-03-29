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
		}

		public (double X, double Y) EventCoordsToMapCoords(double eventCoordsX, double eventCoordsY, int map = -1)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			if (map < 0)
			{
				map = _rootPackState.CurrentMapId;
			}
			lock (_mapDetails)
			{
				if (_mapDetails.TryGetValue(map, out var mapDetails))
				{
					Rectangle val = mapDetails.ContinentRect;
					Coordinates2 topLeft = ((Rectangle)(ref val)).get_TopLeft();
					double x = ((Coordinates2)(ref topLeft)).get_X();
					double num = eventCoordsX * 39.37007874015748;
					val = mapDetails.MapRect;
					topLeft = ((Rectangle)(ref val)).get_TopLeft();
					double num2 = num - ((Coordinates2)(ref topLeft)).get_X();
					val = mapDetails.MapRect;
					double num3 = num2 / ((Rectangle)(ref val)).get_Width();
					val = mapDetails.ContinentRect;
					double item = x + num3 * ((Rectangle)(ref val)).get_Width();
					val = mapDetails.ContinentRect;
					topLeft = ((Rectangle)(ref val)).get_TopLeft();
					double y = ((Coordinates2)(ref topLeft)).get_Y();
					double num4 = eventCoordsY * 39.37007874015748;
					val = mapDetails.MapRect;
					topLeft = ((Rectangle)(ref val)).get_TopLeft();
					double num5 = 0.0 - (num4 - ((Coordinates2)(ref topLeft)).get_Y());
					val = mapDetails.MapRect;
					double num6 = num5 / ((Rectangle)(ref val)).get_Height();
					val = mapDetails.ContinentRect;
					return (item, y + num6 * ((Rectangle)(ref val)).get_Height());
				}
			}
			return (0.0, 0.0);
		}

		public override async Task Reload()
		{
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override Task Unload()
		{
			return Task.CompletedTask;
		}
	}
}
