using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Blish_HUD.Modules.Managers;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Controls.Map;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls.Map;
using Estreya.BlishHUD.Shared.Controls.World;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Glide;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using NodaTime;

namespace Estreya.BlishHUD.EventTable.Managers
{
	public class EventTimerHandler : IDisposable, IUpdatable
	{
		private static readonly Logger Logger = Logger.GetLogger<EventTimerHandler>();

		private static TimeSpan _checkLostEntitiesInterval = TimeSpan.FromSeconds(5.0);

		private double _lastLostEntitiesCheck;

		private static TimeSpan _readdInterval = TimeSpan.FromSeconds(5.0);

		private AsyncRef<double> _lastReadd = new AsyncRef<double>(0.0);

		private bool _notifiedLostEntities;

		private ConcurrentDictionary<FontSize, BitmapFont> _fonts = new ConcurrentDictionary<FontSize, BitmapFont>();

		private readonly Gw2ApiManager _apiManager;

		private readonly Func<Task<List<Event>>> _getEvents;

		private readonly Func<Instant> _getNow;

		private readonly MapUtil _mapUtil;

		private readonly ModuleSettings _moduleSettings;

		private readonly TranslationService _translationService;

		private readonly IconService _iconService;

		private readonly ContentsManager _contentsManager;

		private readonly ConcurrentQueue<(string Key, bool Add)> _entityQueue = new ConcurrentQueue<(string, bool)>();

		private readonly ConcurrentDictionary<string, List<MapEntity>> _mapEntities = new ConcurrentDictionary<string, List<MapEntity>>();

		private readonly ConcurrentDictionary<string, List<WorldEntity>> _worldEntities = new ConcurrentDictionary<string, List<WorldEntity>>();

		private readonly ConcurrentDictionary<string, Tween> _worldEntityAnimations = new ConcurrentDictionary<string, Tween>();

		public event EventHandler FoundLostEntities;

		public EventTimerHandler(Func<Task<List<Event>>> getEvents, Func<Instant> getNow, MapUtil mapUtil, Gw2ApiManager apiManager, ModuleSettings moduleSettings, TranslationService translationService, IconService iconService, ContentsManager contentsManager)
		{
			_getEvents = getEvents;
			_getNow = getNow;
			_mapUtil = mapUtil;
			_apiManager = apiManager;
			_moduleSettings = moduleSettings;
			_translationService = translationService;
			_iconService = iconService;
			_contentsManager = contentsManager;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_moduleSettings.ShowEventTimersOnMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowEventTimersOnMap_SettingChanged);
			_moduleSettings.ShowEventTimersInWorld.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowEventTimersInWorld_SettingChanged);
			_moduleSettings.DisabledEventTimerSettingKeys.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)DisabledEventTimerSettingKeys_SettingChanged);
		}

		public void Update(GameTime gameTime)
		{
		}

		private async Task AddAll()
		{
			await AddEventTimersToMap();
			await AddEventTimersToWorld();
		}

		private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			Logger.Debug($"Changed map to id {e.get_Value()}");
			await AddAll();
		}

		private async Task<List<Event>> GetAllEvents()
		{
			return (await _getEvents()).Where((Event ev) => ev.Timers != null).ToList();
		}

		private async Task<List<Event>> GetEventsForMap(int mapId)
		{
			return (await GetAllEvents()).Where((Event ev) => ev.MapIds.Contains(mapId)).ToList();
		}

		public async Task AddEventTimersToMap()
		{
			_ = 1;
			try
			{
				_mapEntities?.Values.ToList().ForEach(delegate(List<MapEntity> m)
				{
					_mapUtil.RemoveEntities(m.ToArray());
				});
				_mapEntities?.Clear();
				if (!_moduleSettings.ShowEventTimersOnMap.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
				{
					return;
				}
				int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				List<Event> events = await GetAllEvents();
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
				Logger.Warn(ex, "Failed to add event timers to map.");
			}
		}

		private void RemoveEventTimerFromMap(Event ev)
		{
			if (_mapEntities.ContainsKey(ev.SettingKey))
			{
				_mapUtil.RemoveEntities(_mapEntities[ev.SettingKey].ToArray());
				_mapEntities.TryRemove(ev.SettingKey, out var _);
			}
		}

		public Task AddEventTimerToMap(Event ev)
		{
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			RemoveEventTimerFromMap(ev);
			if (!_moduleSettings.ShowEventTimersOnMap.get_Value() || !GameService.Gw2Mumble.get_IsAvailable() || ev.Timers == null || (_moduleSettings.DisabledEventTimerSettingKeys.get_Value()?.Contains(ev.SettingKey) ?? false))
			{
				return Task.CompletedTask;
			}
			try
			{
				List<Estreya.BlishHUD.EventTable.Models.EventMapTimer> list = ev.Timers.Where((EventTimers t) => t.Map != null && t.Map.Length != 0).SelectMany((EventTimers t) => t.Map).ToList();
				List<MapEntity> entities = new List<MapEntity>();
				foreach (Estreya.BlishHUD.EventTable.Models.EventMapTimer mapTimer in list)
				{
					MapEntity circle = _mapUtil.AddEntity(new Estreya.BlishHUD.EventTable.Controls.Map.EventMapTimer(ev, mapTimer, Color.get_DarkOrange(), _getNow, _translationService, 3f));
					circle.TooltipText = ev.Name ?? "";
					entities.Add(circle);
				}
				_mapEntities.AddOrUpdate(ev.SettingKey, entities, (string _, List<MapEntity> prev) => prev.Concat(entities).ToList());
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to add " + ev.SettingKey + " to map.");
			}
			return Task.CompletedTask;
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
			List<Event> events = await GetEventsForMap(mapId);
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
			Logger.Debug($"Added events for map {mapId} in {sw.ElapsedMilliseconds}ms");
		}

		private void RemoveEventTimerFromWorld(Event ev)
		{
			if (_worldEntities.ContainsKey(ev.SettingKey))
			{
				GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities[ev.SettingKey]);
				_worldEntities.TryRemove(ev.SettingKey, out var _);
			}
		}

		private BitmapFont GetFont(FontSize fontSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _fonts.GetOrAdd(fontSize, (Func<FontSize, BitmapFont>)((FontSize size) => GameService.Content.GetFont((FontFace)0, size, (FontStyle)0)));
		}

		public Task AddEventTimerToWorld(Event ev)
		{
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0525: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_0547: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0562: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_0743: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0782: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0823: Unknown result type (might be due to invalid IL or missing references)
			//IL_0827: Unknown result type (might be due to invalid IL or missing references)
			//IL_0858: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			RemoveEventTimerFromWorld(ev);
			if (!_moduleSettings.ShowEventTimersInWorld.get_Value() || !GameService.Gw2Mumble.get_IsAvailable() || ev.Timers == null || (_moduleSettings.DisabledEventTimerSettingKeys.get_Value()?.Contains(ev.SettingKey) ?? false))
			{
				return Task.CompletedTask;
			}
			try
			{
				Func<WorldEntity, bool> renderCondition = (WorldEntity entity) => entity.DistanceToPlayer <= (float)_moduleSettings.EventTimersRenderDistance.get_Value();
				int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				_getNow();
				List<EventWorldTimer> worldTimers = ev.Timers.Where((EventTimers t) => t.MapID == mapId && t.World != null && t.World.Length != 0).SelectMany((EventTimers t) => t.World).ToList();
				List<WorldEntity> entites = new List<WorldEntity>();
				Vector3 centerAsWorldMeters = default(Vector3);
				for (int i = 0; i < worldTimers.Count; i++)
				{
					EventWorldTimer worldTimer = worldTimers[i];
					((Vector3)(ref centerAsWorldMeters))._002Ector(worldTimer.X, worldTimer.Y, worldTimer.Z);
					float width = 5f;
					float boxHeight = 3.5f;
					List<Vector3> statuePoints = new List<Vector3>
					{
						new Vector3(0f - width / 2f, 0f, 0f),
						new Vector3(width / 2f, 0f, 0f),
						new Vector3(width / 2f, 0f, boxHeight),
						new Vector3(0f - width / 2f, 0f, boxHeight),
						new Vector3(0f - width / 2f, 0f, 0f)
					};
					bool first = true;
					statuePoints = statuePoints.SelectMany(delegate(Vector3 t)
					{
						//IL_0000: Unknown result type (might be due to invalid IL or missing references)
						IEnumerable<Vector3> result = Enumerable.Repeat<Vector3>(t, first ? 1 : 2);
						first = false;
						return result;
					}).ToList();
					statuePoints = statuePoints.Take(statuePoints.Count - 1).ToList();
					Vector3 val = centerAsWorldMeters + new Vector3(0f, 0f, boxHeight);
					float halfCircleRadius = width / 2f;
					Vector3 halfCirclePosition = val;
					Vector3 texturePosition = halfCirclePosition + new Vector3(0f, 0f, halfCircleRadius * 0.75f);
					float textureScale = 1f;
					AsyncTexture2D textureIcon = _iconService.GetIcon(ev.Icon);
					new Size(128, 128);
					float rotation = worldTimer.Rotation;
					Func<string> remainingText = delegate
					{
						Instant currentOccurrence = ev.GetCurrentOccurrence();
						string text = "---";
						if (currentOccurrence != default(Instant))
						{
							text = (currentOccurrence.Plus(ev.Duration) - _getNow()).ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second);
						}
						return "Remaining: " + text;
					};
					BitmapFont remainingFont = GetFont((FontSize)36);
					float remainingScale = 0.4f;
					float remainingScaleWidth = 3.75f;
					Color remainingColor = ColorExtensions.ToXnaColor(_moduleSettings.EventTimersRemainingTextColor.get_Value().get_Cloth());
					Func<string> startsInText = () => "Next in: " + (ev.GetNextOccurrence() - _getNow()).ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second);
					BitmapFont startsInFont = GetFont((FontSize)36);
					float startsInScale = 0.4f;
					float startsInScaleWidth = 4f;
					Color startsInColor = ColorExtensions.ToXnaColor(_moduleSettings.EventTimersStartsInTextColor.get_Value().get_Cloth());
					float nextOccurrenceScale = 0.4f;
					float nextOccurrenceScaleWidth = 3f;
					Func<string> nextOccurrenceText = () => ev.GetNextOccurrence().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).ToDateTimeOffset()
						.ToString("g", Thread.CurrentThread.CurrentUICulture);
					Color nextOccurrenceColor = ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNextOccurenceTextColor.get_Value().get_Cloth());
					BitmapFont nextOccurrenceFont = GetFont((FontSize)36);
					Func<string> nameText = () => ev.Name ?? "";
					BitmapFont nameFont = GetFont((FontSize)36);
					float nameScale = 0.6f;
					float nameScaleWidth = width / 1.5f;
					Color nameColor = ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNameTextColor.get_Value().get_Cloth());
					Func<string> durationText = () => "Duration: " + ev.Duration.ToTimeSpan().Humanize(1, null, TimeUnit.Hour, TimeUnit.Minute);
					BitmapFont durationFont = GetFont((FontSize)36);
					float durationScale = 0.4f;
					float durationScaleWidth = 3.5f;
					Color durationColor = ColorExtensions.ToXnaColor(_moduleSettings.EventTimersDurationTextColor.get_Value().get_Cloth());
					Func<string> repeatText = () => "Repeats every: " + ev.Repeat.ToTimeSpan().Humanize();
					BitmapFont repeatFont = GetFont((FontSize)36);
					float repeatScale = 0.4f;
					float repeatScaleWidth = 3.5f;
					Color repeatColor = ColorExtensions.ToXnaColor(_moduleSettings.EventTimersRepeatTextColor.get_Value().get_Cloth());
					Vector3 namePosition = texturePosition + new Vector3(0f, 0f, -0.75f);
					Vector3 durationPosition = namePosition + new Vector3(0f, 0f, 0f - (nameScale / 2f + durationScale / 2f));
					Vector3 repeatPosition = durationPosition + new Vector3(0f, 0f, 0f - (durationScale / 2f + repeatScale / 2f));
					Vector3 remainingPosition = val + new Vector3(0f, 0f, 0f - nameScale / 2f);
					Vector3 startsInPosition = remainingPosition + new Vector3(0f, 0f, 0f - remainingScale);
					Vector3 nextOccurrencePosition = startsInPosition + new Vector3(0f, 0f, 0f - startsInScale);
					_ = halfCirclePosition + new Vector3(0f, 0f, halfCircleRadius + 0.5f);
					entites.AddRange(new WorldEntity[15]
					{
						new WorldPolygone(centerAsWorldMeters, statuePoints.ToArray())
						{
							RotationZ = rotation,
							RenderCondition = renderCondition
						},
						new WorldHalfCircle(halfCirclePosition, halfCircleRadius)
						{
							RotationZ = rotation,
							RotationX = 90f,
							RenderCondition = renderCondition
						},
						new WorldTexture(textureIcon, texturePosition, textureScale)
						{
							RotationZ = rotation,
							RotationX = 90f,
							RenderCondition = renderCondition
						},
						new WorldTexture(textureIcon, texturePosition, textureScale)
						{
							RotationZ = rotation + 180f,
							RotationX = 90f,
							RenderCondition = renderCondition
						},
						new WorldText(remainingText, remainingFont, remainingPosition, remainingScale, remainingColor)
						{
							RotationZ = rotation,
							RotationX = 90f,
							ScaleX = remainingScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(remainingText, remainingFont, remainingPosition, remainingScale, remainingColor)
						{
							RotationZ = rotation + 180f,
							RotationX = 90f,
							ScaleX = remainingScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(startsInText, startsInFont, startsInPosition, startsInScale, startsInColor)
						{
							RotationZ = rotation,
							RotationX = 90f,
							ScaleX = startsInScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(startsInText, startsInFont, startsInPosition, startsInScale, startsInColor)
						{
							RotationZ = rotation + 180f,
							RotationX = 90f,
							ScaleX = startsInScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(nextOccurrenceText, nextOccurrenceFont, nextOccurrencePosition, nextOccurrenceScale, nextOccurrenceColor)
						{
							RotationZ = rotation,
							RotationX = 90f,
							ScaleX = nextOccurrenceScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(nextOccurrenceText, nextOccurrenceFont, nextOccurrencePosition, nextOccurrenceScale, nextOccurrenceColor)
						{
							RotationZ = rotation + 180f,
							RotationX = 90f,
							ScaleX = nextOccurrenceScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(nameText, nameFont, namePosition, nameScale, nameColor)
						{
							RotationZ = rotation,
							RotationX = 90f,
							ScaleX = nameScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(durationText, durationFont, durationPosition, durationScale, durationColor)
						{
							RotationZ = rotation,
							RotationX = 90f,
							ScaleX = durationScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(durationText, durationFont, durationPosition, durationScale, durationColor)
						{
							RotationZ = rotation + 180f,
							RotationX = 90f,
							ScaleX = durationScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(repeatText, repeatFont, repeatPosition, repeatScale, repeatColor)
						{
							RotationZ = rotation,
							RotationX = 90f,
							ScaleX = repeatScaleWidth,
							RenderCondition = renderCondition
						},
						new WorldText(repeatText, repeatFont, repeatPosition, repeatScale, repeatColor)
						{
							RotationZ = rotation + 180f,
							RotationX = 90f,
							ScaleX = repeatScaleWidth,
							RenderCondition = renderCondition
						}
					});
				}
				_worldEntities.AddOrUpdate(ev.SettingKey, entites, (string _, List<WorldEntity> prev) => prev.Concat(entites).ToList());
				GameService.Graphics.get_World().AddEntities((IEnumerable<IEntity>)entites);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to add " + ev.SettingKey + " to world.");
			}
			return Task.CompletedTask;
		}

		public async Task NotifyUpdatedEvents()
		{
			await AddAll();
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

		private async void DisabledEventTimerSettingKeys_SettingChanged(object sender, ValueChangedEventArgs<List<string>> e)
		{
			await AddAll();
		}

		private async void ShowEventTimersInWorld_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			await AddAll();
		}

		private async void ShowEventTimersOnMap_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			await AddAll();
		}

		public void Dispose()
		{
			GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)_worldEntities.Values.SelectMany((List<WorldEntity> v) => v));
			_worldEntities?.Clear();
			_mapEntities?.Values.ToList().ForEach(delegate(List<MapEntity> me)
			{
				_mapUtil.RemoveEntities(me.ToArray());
			});
			_mapEntities?.Clear();
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			_moduleSettings.ShowEventTimersOnMap.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowEventTimersOnMap_SettingChanged);
			_moduleSettings.ShowEventTimersInWorld.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowEventTimersInWorld_SettingChanged);
			_moduleSettings.DisabledEventTimerSettingKeys.remove_SettingChanged((EventHandler<ValueChangedEventArgs<List<string>>>)DisabledEventTimerSettingKeys_SettingChanged);
			_fonts?.Clear();
		}
	}
}
