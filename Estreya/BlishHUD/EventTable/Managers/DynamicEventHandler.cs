using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.Shared.Controls.Map;
using Estreya.BlishHUD.Shared.Controls.World;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Managers
{
	public class DynamicEventHandler : IDisposable, IUpdatable
	{
		private static readonly Logger Logger = Logger.GetLogger<DynamicEventHandler>();

		private readonly MapUtil _mapUtil;

		private readonly DynamicEventState _dynamicEventState;

		private readonly Gw2ApiManager _apiManager;

		private readonly ModuleSettings _moduleSettings;

		private ConcurrentDictionary<string, MapEntity> _mapEntities = new ConcurrentDictionary<string, MapEntity>();

		private ConcurrentDictionary<string, List<WorldEntity>> _worldEntities = new ConcurrentDictionary<string, List<WorldEntity>>();

		private ConcurrentQueue<(string Key, bool Add)> _entityQueue = new ConcurrentQueue<(string, bool)>();

		public DynamicEventHandler(MapUtil mapUtil, DynamicEventState dynamicEventState, Gw2ApiManager apiManager, ModuleSettings moduleSettings)
		{
			_mapUtil = mapUtil;
			_dynamicEventState = dynamicEventState;
			_apiManager = apiManager;
			_moduleSettings = moduleSettings;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_moduleSettings.ShowDynamicEventsOnMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventsOnMap_SettingChanged);
			_moduleSettings.ShowDynamicEventInWorld.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventsInWorldSetting_SettingChanged);
			_moduleSettings.DisabledDynamicEventIds.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)DisabledDynamicEventIds_SettingChanged);
		}

		private void DisabledDynamicEventIds_SettingChanged(object sender, ValueChangedEventArgs<List<string>> e)
		{
			IEnumerable<string> enumerable = from newKey in e.get_NewValue()
				where !e.get_PreviousValue().Any((string oldKey) => oldKey == newKey)
				select newKey;
			IEnumerable<string> removeElements = from oldKey in e.get_PreviousValue()
				where !e.get_NewValue().Any((string newKey) => newKey == oldKey)
				select oldKey;
			foreach (string newElement in enumerable)
			{
				_entityQueue.Enqueue((newElement, false));
			}
			foreach (string oldElement in removeElements)
			{
				_entityQueue.Enqueue((oldElement, true));
			}
		}

		private async void ShowDynamicEventsInWorldSetting_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			await AddDynamicEventsToWorld();
		}

		private async void ShowDynamicEventsOnMap_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			await AddDynamicEventsToMap();
		}

		private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			await AddDynamicEventsToMap();
			await AddDynamicEventsToWorld();
		}

		public async Task AddDynamicEventsToMap()
		{
			_ = 1;
			try
			{
				_mapEntities?.Values.ToList().ForEach(delegate(MapEntity m)
				{
					_mapUtil.RemoveEntity(m);
				});
				_mapEntities?.Clear();
				if (!_moduleSettings.ShowDynamicEventsOnMap.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
				{
					return;
				}
				if (!(await _dynamicEventState.WaitForCompletion(TimeSpan.FromMinutes(5.0))))
				{
					Logger.Debug("DynamicEventState did not finish in the given timespan. Abort.");
					return;
				}
				int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				IOrderedEnumerable<DynamicEventState.DynamicEvent> events = _dynamicEventState.GetEventsByMap(mapId)?.Where((DynamicEventState.DynamicEvent de) => !_moduleSettings.DisabledDynamicEventIds.get_Value().Contains(de.ID)).OrderByDescending(delegate(DynamicEventState.DynamicEvent d)
				{
					double[][] points = d.Location.Points;
					return (points != null) ? points.Length : 0;
				}).ThenByDescending((DynamicEventState.DynamicEvent d) => d.Location.Radius);
				if (events == null)
				{
					Logger.Debug($"No events found for map {mapId}");
					return;
				}
				new List<MapEntity>();
				foreach (DynamicEventState.DynamicEvent ev in events)
				{
					await AddDynamicEventToMap(ev);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to add dynamic events to map.");
			}
		}

		private void RemoveDynamicEventFromMap(DynamicEventState.DynamicEvent dynamicEvent)
		{
			if (_mapEntities.ContainsKey(dynamicEvent.ID))
			{
				_mapUtil.RemoveEntity(_mapEntities[dynamicEvent.ID]);
				_mapEntities.TryRemove(dynamicEvent.ID, out var _);
			}
		}

		public async Task AddDynamicEventToMap(DynamicEventState.DynamicEvent dynamicEvent)
		{
			RemoveDynamicEventFromMap(dynamicEvent);
			if (!_moduleSettings.ShowDynamicEventsOnMap.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			if (!(await _dynamicEventState.WaitForCompletion(TimeSpan.FromMinutes(5.0))))
			{
				Logger.Debug("DynamicEventState did not finish in the given timespan. Abort.");
				return;
			}
			try
			{
				Map map = await ((IBulkExpandableClient<Map, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(dynamicEvent.MapId, default(CancellationToken));
				Vector2 coords = map.EventMapCoordinatesToMapCoordinates(new Vector2((float)dynamicEvent.Location.Center[0], (float)dynamicEvent.Location.Center[1]));
				switch (dynamicEvent.Location.Type)
				{
				case "sphere":
				case "cylinder":
				{
					double radiusScale = map.GetDynamicEventMapLengthScale(dynamicEvent.Location.Radius);
					double radius = dynamicEvent.Location.Radius * 0.0416666679084301 / radiusScale;
					MapEntity circle = _mapUtil.AddCircle(coords.X, coords.Y, radius, Color.get_DarkOrange(), 3f);
					circle.TooltipText = $"{dynamicEvent.Name} (Level {dynamicEvent.Level})";
					_mapEntities.AddOrUpdate(dynamicEvent.ID, circle, (string _, MapEntity _) => circle);
					break;
				}
				case "poly":
				{
					List<float[]> points = new List<float[]>();
					double[][] points2 = dynamicEvent.Location.Points;
					foreach (double[] item in points2)
					{
						Vector2 polyCoords = map.EventMapCoordinatesToMapCoordinates(new Vector2((float)item[0], (float)item[1]));
						points.Add(new float[2] { polyCoords.X, polyCoords.Y });
					}
					MapEntity border = _mapUtil.AddBorder(coords.X, coords.Y, points.ToArray(), Color.get_DarkOrange(), 4f);
					border.TooltipText = $"{dynamicEvent.Name} (Level {dynamicEvent.Level})";
					_mapEntities.AddOrUpdate(dynamicEvent.ID, border, (string _, MapEntity _) => border);
					break;
				}
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to add " + dynamicEvent.Name + " to map.");
			}
		}

		private bool WorldEventRenderCondition(WorldEntity worldEntity)
		{
			if (_moduleSettings.ShowDynamicEventsInWorldOnlyWhenInside.get_Value())
			{
				return worldEntity.IsPlayerInside(!_moduleSettings.IgnoreZAxisOnDynamicEventsInWorld.get_Value());
			}
			return (float)_moduleSettings.DynamicEventsRenderDistance.get_Value() >= worldEntity.DistanceToPlayer;
		}

		public async Task AddDynamicEventsToWorld()
		{
			GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities.Values.SelectMany((List<WorldEntity> v) => v));
			_worldEntities?.Clear();
			if (!_moduleSettings.ShowDynamicEventInWorld.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			if (!(await _dynamicEventState.WaitForCompletion(TimeSpan.FromMinutes(5.0))))
			{
				Logger.Debug("DynamicEventState did not finish in the given timespan. Abort.");
				return;
			}
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			IEnumerable<DynamicEventState.DynamicEvent> events = from de in _dynamicEventState.GetEventsByMap(mapId)
				where !_moduleSettings.DisabledDynamicEventIds.get_Value().Contains(de.ID)
				select de;
			if (events == null)
			{
				Logger.Debug($"No events found for map {mapId}");
				return;
			}
			Stopwatch sw = Stopwatch.StartNew();
			foreach (DynamicEventState.DynamicEvent ev in events)
			{
				await AddDynamicEventToWorld(ev);
			}
			sw.Stop();
			Logger.Debug($"Added events in {sw.ElapsedMilliseconds}ms");
		}

		private void RemoveDynamicEventFromWorld(DynamicEventState.DynamicEvent dynamicEvent)
		{
			if (_worldEntities.ContainsKey(dynamicEvent.ID))
			{
				GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities[dynamicEvent.ID]);
				_worldEntities.TryRemove(dynamicEvent.ID, out var _);
			}
		}

		public async Task AddDynamicEventToWorld(DynamicEventState.DynamicEvent dynamicEvent)
		{
			RemoveDynamicEventFromWorld(dynamicEvent);
			if (!_moduleSettings.ShowDynamicEventInWorld.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			Map map = await ((IBulkExpandableClient<Map, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(dynamicEvent.MapId, default(CancellationToken));
			Vector2 centerAsMapCoords = map.EventMapCoordinatesToMapCoordinates(new Vector2((float)dynamicEvent.Location.Center[0], (float)dynamicEvent.Location.Center[1]));
			Vector3 centerAsWorldMeters = map.MapCoordsToWorldMeters(new Vector2(centerAsMapCoords.X, centerAsMapCoords.Y));
			centerAsWorldMeters.Z = (float)Math.Abs(dynamicEvent.Location.Center[2].ToMeters());
			try
			{
				List<WorldEntity> entites = new List<WorldEntity>();
				switch (dynamicEvent.Location.Type)
				{
				case "poly":
				{
					List<WorldEntity> list = entites;
					list.Add(await GetPolygone(dynamicEvent, map, centerAsWorldMeters, WorldEventRenderCondition));
					break;
				}
				case "sphere":
				{
					List<WorldEntity> list = entites;
					list.Add(await GetSphere(dynamicEvent, map, centerAsWorldMeters, WorldEventRenderCondition));
					break;
				}
				case "cylinder":
				{
					List<WorldEntity> list = entites;
					list.Add(await GetCylinder(dynamicEvent, map, centerAsWorldMeters, WorldEventRenderCondition));
					break;
				}
				}
				_worldEntities.AddOrUpdate(dynamicEvent.ID, entites, (string _, List<WorldEntity> prev) => prev.Concat(entites).ToList());
				GameService.Graphics.get_World().AddEntities((IEnumerable<IEntity>)entites);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to add " + dynamicEvent.Name + " to world.");
			}
		}

		private async Task<WorldEntity> GetSphere(DynamicEventState.DynamicEvent ev, Map map, Vector3 centerAsWorldMeters, Func<WorldEntity, bool> renderCondition)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			int tessellation = 50;
			int connections = tessellation / 5;
			if (connections > tessellation)
			{
				throw new ArgumentOutOfRangeException("connections", "connections can't be greater than tessellation");
			}
			double radiusScale = map.GetDynamicEventMapLengthScale(ev.Location.Radius);
			float radius = (float)ev.Location.Radius.ToMeters() / (float)radiusScale;
			List<Vector3> points = new List<Vector3>();
			for (int i = 0; i < tessellation; i++)
			{
				float num = (float)((double)((float)i / (float)tessellation * 2f) * Math.PI);
				float xScaled = (float)Math.Cos(num);
				float num2 = (float)Math.Sin(num);
				float x = xScaled * radius;
				float y = num2 * radius;
				points.Add(new Vector3(x, y, 0f));
			}
			bool first = true;
			points = points.SelectMany(delegate(Vector3 t)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				IEnumerable<Vector3> result3 = Enumerable.Repeat<Vector3>(t, first ? 1 : 2);
				first = false;
				return result3;
			}).ToList();
			points.Add(points[0]);
			int connectionSteps = tessellation / connections;
			float bendSteps = 12f;
			List<Vector3> connectionPoints = new List<Vector3>();
			Vector3 mid = default(Vector3);
			Vector3 up = default(Vector3);
			Vector3 centerUp = default(Vector3);
			Vector3 down = default(Vector3);
			Vector3 centerDown = default(Vector3);
			for (int p = 0; p < tessellation; p += connectionSteps)
			{
				Vector3 point = points[p * 2];
				((Vector3)(ref mid))._002Ector(point.X, point.Y, 0f);
				List<Vector3> bendPointsUp = new List<Vector3>();
				((Vector3)(ref up))._002Ector(0f, 0f, radius);
				((Vector3)(ref centerUp))._002Ector(mid.X + up.X, mid.Y + up.Y, mid.Z + up.Z);
				for (float ratio2 = 0f; ratio2 <= 1f; ratio2 += 1f / bendSteps)
				{
					Vector3 val = Vector3.Lerp(mid, centerUp, ratio2);
					Vector3 tangent2 = Vector3.Lerp(centerUp, up, ratio2);
					Vector3 curve = Vector3.Lerp(val, tangent2, ratio2);
					bendPointsUp.Add(curve);
				}
				bool bendPointsUpFirst = true;
				bendPointsUp = bendPointsUp.SelectMany(delegate(Vector3 t)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					IEnumerable<Vector3> result2 = Enumerable.Repeat<Vector3>(t, bendPointsUpFirst ? 1 : 2);
					bendPointsUpFirst = false;
					return result2;
				}).ToList();
				bendPointsUp.RemoveAt(bendPointsUp.Count - 1);
				connectionPoints.AddRange(bendPointsUp);
				((Vector3)(ref down))._002Ector(0f, 0f, 0f - radius);
				List<Vector3> bendPointsDown = new List<Vector3>();
				((Vector3)(ref centerDown))._002Ector(mid.X + down.X, mid.Y + down.Y, mid.Z + down.Z);
				for (float ratio = 0f; ratio <= 1f; ratio += 1f / bendSteps)
				{
					Vector3 val2 = Vector3.Lerp(mid, centerDown, ratio);
					Vector3 tangent3 = Vector3.Lerp(centerDown, down, ratio);
					Vector3 curve2 = Vector3.Lerp(val2, tangent3, ratio);
					bendPointsDown.Add(curve2);
				}
				bool bendPointsDownFirst = true;
				bendPointsDown = bendPointsDown.SelectMany(delegate(Vector3 t)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					IEnumerable<Vector3> result = Enumerable.Repeat<Vector3>(t, bendPointsDownFirst ? 1 : 2);
					bendPointsDownFirst = false;
					return result;
				}).ToList();
				bendPointsDown.RemoveAt(bendPointsDown.Count - 1);
				connectionPoints.AddRange(bendPointsDown);
			}
			IEnumerable<Vector3> allPoints = points.Concat(connectionPoints);
			return new WorldPolygone(centerAsWorldMeters, allPoints.ToArray(), Color.get_White(), renderCondition);
		}

		private async Task<WorldEntity> GetCylinder(DynamicEventState.DynamicEvent ev, Map map, Vector3 centerAsWorldMeters, Func<WorldEntity, bool> renderCondition)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			int tessellation = 50;
			int connections = tessellation / 4;
			if (connections > tessellation)
			{
				throw new ArgumentOutOfRangeException("connections", "connections can't be greater than tessellation");
			}
			double radiusScale = map.GetDynamicEventMapLengthScale(ev.Location.Radius);
			float radius = (float)ev.Location.Radius.ToMeters() / (float)radiusScale;
			double heightScale = map.GetDynamicEventMapLengthScale(ev.Location.Height);
			float height = (float)ev.Location.Height.ToMeters() / (float)heightScale;
			List<Vector3> points = new List<Vector3>();
			for (int j = 0; j < tessellation; j++)
			{
				float num = (float)((double)((float)j / (float)tessellation * 2f) * Math.PI);
				float xScaled = (float)Math.Cos(num);
				float num2 = (float)Math.Sin(num);
				float x2 = xScaled * radius;
				float y = num2 * radius;
				points.Add(new Vector3(x2, y, centerAsWorldMeters.Z));
			}
			bool first = true;
			points = points.SelectMany(delegate(Vector3 t)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				IEnumerable<Vector3> result = Enumerable.Repeat<Vector3>(t, first ? 1 : 2);
				first = false;
				return result;
			}).ToList();
			points.Add(points[0]);
			Vector3[][] perZRangePoints = (from z in new double[2] { 0.0, height }
				orderby z
				select ((IEnumerable<Vector3>)points).Select((Func<Vector3, Vector3>)((Vector3 mp) => new Vector3(mp.X, mp.Y, (float)z))).ToArray()).ToArray();
			List<Vector3> connectPoints = new List<Vector3>();
			for (int p = 0; p < connections; p++)
			{
				float num3 = (float)((double)((float)p / (float)connections * 2f) * Math.PI);
				float xScaled2 = (float)Math.Cos(num3);
				float num4 = (float)Math.Sin(num3);
				float x3 = xScaled2 * radius;
				float y2 = num4 * radius;
				for (int i = 0; i < perZRangePoints.Length - 1; i++)
				{
					Vector3[] curr = perZRangePoints[i];
					Vector3[] next = perZRangePoints[i + 1];
					if (curr.Length != next.Length)
					{
						throw new ArgumentOutOfRangeException("WorldPolygone.Points", "Length does not match.");
					}
					connectPoints.Add(new Vector3(x3, y2, curr[0].Z));
					connectPoints.Add(new Vector3(x3, y2, next[0].Z));
				}
			}
			IEnumerable<Vector3> allPoints = perZRangePoints.SelectMany((Vector3[] x) => x).Concat(connectPoints);
			return new WorldPolygone(centerAsWorldMeters, allPoints.ToArray(), Color.get_White(), renderCondition);
		}

		private async Task<WorldEntity> GetPolygone(DynamicEventState.DynamicEvent dynamicEvent, Map map, Vector3 centerAsWorldMeters, Func<WorldEntity, bool> renderCondition)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] points = ((IEnumerable<double[]>)dynamicEvent.Location.Points).Select((Func<double[], Vector3>)delegate(double[] p)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				Vector2 val = map.EventMapCoordinatesToMapCoordinates(new Vector2((float)p[0], (float)p[1]));
				Vector3 val2 = map.MapCoordsToWorldMeters(new Vector2(val.X, val.Y));
				return new Vector3(val2.X, val2.Y, centerAsWorldMeters.Z);
			}).ToArray();
			bool first = true;
			points = points.SelectMany(delegate(Vector3 t)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				IEnumerable<Vector3> result = Enumerable.Repeat<Vector3>(t, first ? 1 : 2);
				first = false;
				return result;
			}).ToArray();
			List<Vector3> list = points.ToList();
			list.Add(points[0]);
			points = list.ToArray();
			Vector3[] mappedPoints = (Vector3[])(object)new Vector3[points.Length];
			for (int j = 0; j < points.Length; j++)
			{
				Vector3 offset = points[j] - centerAsWorldMeters;
				mappedPoints[j] = offset;
			}
			Vector3[][] perZRangePoints = (from z in dynamicEvent.Location.ZRange
				orderby z
				select (double)centerAsWorldMeters.Z + z.ToMeters() into z
				select ((IEnumerable<Vector3>)mappedPoints).Select((Func<Vector3, Vector3>)((Vector3 mp) => new Vector3(mp.X, mp.Y, (float)z))).ToArray()).ToArray();
			List<Vector3> connectPoints = new List<Vector3>();
			for (int i = 0; i < perZRangePoints.Length - 1; i++)
			{
				Vector3[] curr = perZRangePoints[i];
				Vector3[] next = perZRangePoints[i + 1];
				if (curr.Length != next.Length)
				{
					throw new ArgumentOutOfRangeException("points", "Length does not match.");
				}
				for (int p2 = 0; p2 < curr.Length; p2++)
				{
					Vector3 currP = curr[p2];
					Vector3 nextP = next[p2];
					connectPoints.Add(currP);
					connectPoints.Add(nextP);
				}
			}
			IEnumerable<Vector3> allPoints = perZRangePoints.SelectMany((Vector3[] x) => x).Concat(connectPoints);
			return new WorldPolygone(centerAsWorldMeters, allPoints.ToArray(), Color.get_White(), renderCondition);
		}

		public void Dispose()
		{
			GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities.Values.SelectMany((List<WorldEntity> v) => v));
			_worldEntities?.Clear();
			_mapEntities?.Values.ToList().ForEach(delegate(MapEntity me)
			{
				_mapUtil.RemoveEntity(me);
			});
			_mapEntities?.Clear();
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_moduleSettings.ShowDynamicEventsOnMap.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventsOnMap_SettingChanged);
			_moduleSettings.ShowDynamicEventInWorld.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventsInWorldSetting_SettingChanged);
			_moduleSettings.DisabledDynamicEventIds.remove_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)DisabledDynamicEventIds_SettingChanged);
		}

		public void Update(GameTime gameTime)
		{
			(string Key, bool Add) element;
			DynamicEventState.DynamicEvent dynamicEvent;
			while (_entityQueue.TryDequeue(out element))
			{
				try
				{
					dynamicEvent = _dynamicEventState.Events.Where((DynamicEventState.DynamicEvent e) => e.ID == element.Key).First();
					if (element.Add)
					{
						Task.Run(async delegate
						{
							await AddDynamicEventToMap(dynamicEvent);
							await AddDynamicEventToWorld(dynamicEvent);
						});
					}
					else
					{
						RemoveDynamicEventFromMap(dynamicEvent);
						RemoveDynamicEventFromWorld(dynamicEvent);
					}
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "Failed updating event " + element.Key);
				}
			}
		}
	}
}
