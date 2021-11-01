using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gw2Sharp.Models;
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

		private async Task LoadMapData()
		{
			foreach (Map map in (IEnumerable<Map>)(await ((IAllExpandableClient<Map>)(object)PathingModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).AllAsync(default(CancellationToken))))
			{
				_mapDetails[map.get_Id()] = new MapDetails(map.get_ContinentRect(), map.get_MapRect());
			}
		}

		public (double X, double Y) EventCoordsToMapCoords(double eventCoordsX, double eventCoordsY, int map = -1)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			if (map < 0)
			{
				map = _rootPackState.CurrentMapId;
			}
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
