using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls.Map;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls.Map;
using Estreya.BlishHUD.Shared.Controls.World;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Managers
{
	public class EventTimerHandler : IDisposable, IUpdatable
	{
		private static readonly Logger Logger = Logger.GetLogger<EventTimerHandler>();

		private static TimeSpan _checkLostEntitiesInterval = TimeSpan.FromSeconds(5.0);

		private double _lastLostEntitiesCheck;

		private bool _notifiedLostEntities;

		private readonly Gw2ApiManager _apiManager;

		private readonly Func<Task<List<Event>>> _getEvents;

		private readonly Func<DateTime> _getNow;

		private readonly MapUtil _mapUtil;

		private readonly ModuleSettings _moduleSettings;

		private readonly TranslationService _translationService;

		private readonly ConcurrentQueue<(string Key, bool Add)> _entityQueue = new ConcurrentQueue<(string, bool)>();

		private readonly ConcurrentDictionary<string, MapEntity> _mapEntities = new ConcurrentDictionary<string, MapEntity>();

		private readonly ConcurrentDictionary<string, List<WorldEntity>> _worldEntities = new ConcurrentDictionary<string, List<WorldEntity>>();

		public event EventHandler FoundLostEntities;

		public EventTimerHandler(Func<Task<List<Event>>> getEvents, Func<DateTime> getNow, MapUtil mapUtil, Gw2ApiManager apiManager, ModuleSettings moduleSettings, TranslationService translationService)
		{
			_getEvents = getEvents;
			_getNow = getNow;
			_mapUtil = mapUtil;
			_apiManager = apiManager;
			_moduleSettings = moduleSettings;
			_translationService = translationService;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}

		public void Update(GameTime gameTime)
		{
			UpdateUtil.Update(CheckLostEntityReferences, gameTime, _checkLostEntitiesInterval.TotalMilliseconds, ref _lastLostEntitiesCheck);
			(string, bool) element;
			while (_entityQueue.TryDequeue(out element))
			{
				try
				{
					if (element.Item2)
					{
						Task.Run(async delegate
						{
						});
					}
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "Failed updating event " + element.Item1);
				}
			}
		}

		private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			await AddEventTimersToMap();
			await AddEventTimersToWorld();
		}

		private async Task<List<Event>> GetEventForMap(int mapId)
		{
			return (await _getEvents()).Where((Event ev) => ev.MapIds.Contains(mapId)).ToList();
		}

		public async Task AddEventTimersToMap()
		{
			_ = 1;
			try
			{
				_mapEntities?.Values.ToList().ForEach(delegate(MapEntity m)
				{
					_mapUtil.RemoveEntity(m);
				});
				_mapEntities?.Clear();
				if (!_moduleSettings.ShowEventTimersOnMap.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
				{
					return;
				}
				int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				List<Event> events = await GetEventForMap(mapId);
				if (events == null || events.Count == 0)
				{
					Logger.Debug($"No events found for map {mapId}");
					return;
				}
				foreach (Event ev in events)
				{
					await AddEventTimerToMap(ev);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to add dynamic events to map.");
			}
		}

		private void RemoveEventTimerFromMap(Event ev)
		{
			if (_mapEntities.ContainsKey(ev.SettingKey))
			{
				_mapUtil.RemoveEntity(_mapEntities[ev.SettingKey]);
				_mapEntities.TryRemove(ev.SettingKey, out var _);
			}
		}

		public async Task AddEventTimerToMap(Event ev)
		{
			RemoveEventTimerFromMap(ev);
			if (!_moduleSettings.ShowEventTimersOnMap.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			try
			{
				MapEntity circle = _mapUtil.AddEntity(new EventTimer(ev, Color.get_DarkOrange(), _getNow, _translationService, 3f));
				circle.TooltipText = ev.Name ?? "";
				_mapEntities.AddOrUpdate(ev.SettingKey, circle, (string _, MapEntity _) => circle);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to add " + ev.SettingKey + " to map.");
			}
		}

		public async Task AddEventTimersToWorld()
		{
			GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities.Values.SelectMany((List<WorldEntity> v) => v));
			_worldEntities?.Clear();
			if (!_moduleSettings.ShowEventTimersInWorld.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			List<Event> events = await GetEventForMap(mapId);
			if (events == null || events.Count == 0)
			{
				Logger.Debug($"No events found for map {mapId}");
				return;
			}
			Stopwatch sw = Stopwatch.StartNew();
			foreach (Event ev in events)
			{
				await AddEventTimerToWorld(ev);
			}
			sw.Stop();
			Logger.Debug($"Added events in {sw.ElapsedMilliseconds}ms");
		}

		private void RemoveEventTimerFromWorld(Event ev)
		{
			if (_worldEntities.ContainsKey(ev.SettingKey))
			{
				GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities[ev.SettingKey]);
				_worldEntities.TryRemove(ev.SettingKey, out var _);
			}
		}

		public async Task AddEventTimerToWorld(Event ev)
		{
			RemoveEventTimerFromWorld(ev);
			if (_moduleSettings.ShowEventTimersInWorld.get_Value())
			{
				GameService.Gw2Mumble.get_IsAvailable();
			}
		}

		private void CheckLostEntityReferences()
		{
			bool hasEntities = (from e in GameService.Graphics.get_World().get_Entities()
				where e is WorldEntity
				select e).Any();
			if (!_notifiedLostEntities && !_moduleSettings.ShowEventTimersInWorld.get_Value() && hasEntities)
			{
				try
				{
					this.FoundLostEntities?.Invoke(this, EventArgs.Empty);
				}
				catch (Exception)
				{
				}
				_notifiedLostEntities = true;
			}
			if (_moduleSettings.ShowEventTimersInWorld.get_Value())
			{
				_notifiedLostEntities = false;
			}
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
		}
	}
}
