using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Blish_HUD.Modules.Managers;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Controls.Map;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Rendering.Blender;
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
		private sealed class TimerData
		{
			public Size2 Scaling { get; set; }

			public Func<string> TextCallback { get; set; }

			public FontSize FontSize { get; set; }

			public Vector3 Position { get; set; }

			public Func<Color> ColorCallback { get; set; }

			public Func<Size> TextureSizeCallback { get; set; }
		}

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

		private async Task<Dictionary<Event, EventTimers[]>> GetEventsForMap(int mapId)
		{
			return (await GetAllEvents()).Where((Event ev) => ev.MapIds.Contains(mapId) || ev.Timers.Any((EventTimers t) => t.MapID == mapId)).ToList().ToDictionary((Event k) => k, (Event v) => v.Timers.Where((EventTimers t) => t.MapID == mapId).ToArray());
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
			List<WorldEntity> entities = _worldEntities.Values.SelectMany((List<WorldEntity> v) => v).ToList();
			GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)entities);
			GameService.Graphics.get_World().Update(GameService.Overlay.get_CurrentGameTime());
			entities.ForEach(delegate(WorldEntity x)
			{
				x.Dispose();
			});
			_worldEntities?.Clear();
			if (!_moduleSettings.ShowEventTimersInWorld.get_Value() || !GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			Dictionary<Event, EventTimers[]> eventTimers = await GetEventsForMap(mapId);
			if (eventTimers == null || eventTimers.Count == 0)
			{
				Logger.Debug($"No events found for map {mapId}");
				return;
			}
			Stopwatch sw = Stopwatch.StartNew();
			foreach (KeyValuePair<Event, EventTimers[]> ev in eventTimers)
			{
				await AddEventTimerToWorld(ev);
			}
			sw.Stop();
			Logger.Debug($"Added events for map {mapId} in {sw.ElapsedMilliseconds}ms");
		}

		private void RemoveEventTimerFromWorld(Event ev)
		{
			if (_worldEntities.TryRemove(ev.SettingKey, out var entities))
			{
				GameService.Graphics.get_World().RemoveEntities((IEnumerable<IEntity>)entities);
				GameService.Graphics.get_World().Update(GameService.Overlay.get_CurrentGameTime());
				entities.ForEach(delegate(WorldEntity x)
				{
					x.Dispose();
				});
				entities.Clear();
			}
		}

		private BitmapFont GetFont(FontSize fontSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _fonts.GetOrAdd(fontSize, (Func<FontSize, BitmapFont>)((FontSize size) => GameService.Content.GetFont((FontFace)0, size, (FontStyle)0)));
		}

		public Task AddEventTimerToWorld(KeyValuePair<Event, EventTimers[]> evPair)
		{
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			Event ev = evPair.Key;
			EventTimers[] timers = evPair.Value;
			RemoveEventTimerFromWorld(ev);
			if (!_moduleSettings.ShowEventTimersInWorld.get_Value() || !GameService.Gw2Mumble.get_IsAvailable() || timers == null || (_moduleSettings.DisabledEventTimerSettingKeys.get_Value()?.Contains(ev.SettingKey) ?? false))
			{
				return Task.CompletedTask;
			}
			try
			{
				Func<WorldEntity, bool> renderCondition = (WorldEntity entity) => entity.DistanceToPlayer <= (float)_moduleSettings.EventTimersRenderDistance.get_Value();
				int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				_getNow();
				List<EventWorldTimer> worldTimers = timers.Where((EventTimers t) => t.MapID == mapId && t.World != null && t.World.Length != 0).SelectMany((EventTimers t) => t.World).ToList();
				List<WorldEntity> entites = new List<WorldEntity>();
				Vector3 centerAsWorldMeters = default(Vector3);
				for (int i = 0; i < worldTimers.Count; i++)
				{
					EventWorldTimer worldTimer = worldTimers[i];
					int worldTimerIndex = i;
					((Vector3)(ref centerAsWorldMeters))._002Ector(worldTimer.X, worldTimer.Y, worldTimer.Z);
					entites.AddRange(RenderClassicModel(centerAsWorldMeters, ev, worldTimer, renderCondition, worldTimerIndex));
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

		private List<WorldEntity> RenderClassicModel(Vector3 centerAsWorldMeters, Event ev, EventWorldTimer worldTimer, Func<WorldEntity, bool> renderCondition, int worldTimerIndex)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_061a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_0724: Unknown result type (might be due to invalid IL or missing references)
			//IL_0730: Unknown result type (might be due to invalid IL or missing references)
			//IL_0770: Unknown result type (might be due to invalid IL or missing references)
			//IL_0782: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0830: Unknown result type (might be due to invalid IL or missing references)
			//IL_083c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0882: Unknown result type (might be due to invalid IL or missing references)
			//IL_0894: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0923: Unknown result type (might be due to invalid IL or missing references)
			//IL_0935: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0978: Unknown result type (might be due to invalid IL or missing references)
			//IL_09be: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
			List<WorldEntity> entites = new List<WorldEntity>();
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
			Vector3 boxTopPosition = centerAsWorldMeters + new Vector3(0f, 0f, boxHeight);
			float halfCircleRadius = width / 2f;
			Vector3 halfCirclePosition = boxTopPosition;
			Vector3 texturePosition = halfCirclePosition + new Vector3(0f, 0f, halfCircleRadius * 0.75f);
			float textureScale = 0.5f;
			AsyncTexture2D textureIcon = _iconService.GetIcon(ev.Icon);
			float rotation = worldTimer.Rotation;
			TimerData nameData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = boxTopPosition + new Vector3(0f, 0f, 1.125f),
				Scaling = new Size2(2f, 0.2f),
				TextCallback = () => ev.Name,
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNameTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersNameTextureWidth.get_Value(), _moduleSettings.EventTimersNameTextureHeight.get_Value())
			};
			TimerData durationData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = nameData.Position + new Vector3(0f, 0f, -0.45f),
				Scaling = new Size2(2f, 0.2f),
				TextCallback = () => "Duration: " + ev.Duration.ToTimeSpan().Humanize(1, null, TimeUnit.Hour, TimeUnit.Minute),
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNameTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersDurationTextureWidth.get_Value(), _moduleSettings.EventTimersDurationTextureHeight.get_Value())
			};
			TimerData repeatData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = durationData.Position + new Vector3(0f, 0f, -0.45f),
				Scaling = new Size2(2f, 0.2f),
				TextCallback = () => "Repeats every: " + ev.Repeat.ToTimeSpan().Humanize(),
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersRepeatTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersRepeatTextureWidth.get_Value(), _moduleSettings.EventTimersRepeatTextureHeight.get_Value())
			};
			TimerData remainingData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = repeatData.Position + new Vector3(0f, 0f, -1.25f),
				Scaling = new Size2(2.25f, 0.25f),
				TextCallback = delegate
				{
					Instant currentOccurrence = ev.GetCurrentOccurrence();
					string text = "---";
					if (currentOccurrence != default(Instant))
					{
						text = (currentOccurrence.Plus(ev.Duration) - _getNow()).ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second);
					}
					return "Remaining: " + text;
				},
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersRemainingTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersRemainingTextureWidth.get_Value(), _moduleSettings.EventTimersRemainingTextureHeight.get_Value())
			};
			TimerData startsInData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = remainingData.Position + new Vector3(0f, 0f, -0.55f),
				Scaling = new Size2(2.25f, 0.25f),
				TextCallback = () => "Next in: " + (ev.GetNextOccurrence() - _getNow()).ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second),
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersStartsInTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersStartsInTextureWidth.get_Value(), _moduleSettings.EventTimersStartsInTextureHeight.get_Value())
			};
			TimerData nextOccurrenceData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = startsInData.Position + new Vector3(0f, 0f, -0.55f),
				Scaling = new Size2(2.25f, 0.25f),
				TextCallback = () => $"Next at: {ev.GetNextOccurrence().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).ToDateTimeOffset():g}",
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNextOccurenceTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersNextOccurrenceTextureWidth.get_Value(), _moduleSettings.EventTimersNextOccurrenceTextureHeight.get_Value())
			};
			entites.AddRange(new WorldEntity[16]
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
				new WorldText(nameData.TextCallback, GetFont(nameData.FontSize), nameData.Position, 1f, nameData.ColorCallback)
				{
					TextureSizeCallback = nameData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = nameData.Scaling.Height,
					ScaleX = nameData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(nameData.TextCallback, GetFont(nameData.FontSize), nameData.Position, 1f, nameData.ColorCallback)
				{
					TextureSizeCallback = nameData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation + 180f,
					RotationX = 90f,
					ScaleY = nameData.Scaling.Height,
					ScaleX = nameData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(durationData.TextCallback, GetFont(durationData.FontSize), durationData.Position, 1f, durationData.ColorCallback)
				{
					TextureSizeCallback = durationData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = durationData.Scaling.Height,
					ScaleX = durationData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(durationData.TextCallback, GetFont(durationData.FontSize), durationData.Position, 1f, durationData.ColorCallback)
				{
					TextureSizeCallback = durationData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation + 180f,
					RotationX = 90f,
					ScaleY = durationData.Scaling.Height,
					ScaleX = durationData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(repeatData.TextCallback, GetFont(repeatData.FontSize), repeatData.Position, 1f, repeatData.ColorCallback)
				{
					TextureSizeCallback = repeatData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = repeatData.Scaling.Height,
					ScaleX = repeatData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(repeatData.TextCallback, GetFont(repeatData.FontSize), repeatData.Position, 1f, repeatData.ColorCallback)
				{
					TextureSizeCallback = repeatData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation + 180f,
					RotationX = 90f,
					ScaleY = repeatData.Scaling.Height,
					ScaleX = repeatData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new DissolvableWorldText(remainingData.TextCallback, GetFont(remainingData.FontSize), remainingData.Position, 1f, remainingData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = remainingData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = remainingData.Scaling.Height,
					ScaleX = remainingData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "remaining-front");
					}
				},
				new DissolvableWorldText(remainingData.TextCallback, GetFont(remainingData.FontSize), remainingData.Position, 1f, remainingData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = remainingData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation + 180f,
					RotationX = 90f,
					ScaleY = remainingData.Scaling.Height,
					ScaleX = remainingData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "remaining-back");
					}
				},
				new DissolvableWorldText(startsInData.TextCallback, GetFont(startsInData.FontSize), startsInData.Position, 1f, startsInData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = startsInData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = remainingData.Scaling.Height,
					ScaleX = remainingData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "startsIn-front");
					}
				},
				new DissolvableWorldText(startsInData.TextCallback, GetFont(startsInData.FontSize), startsInData.Position, 1f, startsInData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = startsInData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation + 180f,
					RotationX = 90f,
					ScaleY = remainingData.Scaling.Height,
					ScaleX = remainingData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "startsIn-back");
					}
				},
				new DissolvableWorldText(nextOccurrenceData.TextCallback, GetFont(nextOccurrenceData.FontSize), nextOccurrenceData.Position, 1f, nextOccurrenceData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = nextOccurrenceData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = nextOccurrenceData.Scaling.Height,
					ScaleX = nextOccurrenceData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "nextOccurrence-front");
					}
				},
				new DissolvableWorldText(nextOccurrenceData.TextCallback, GetFont(nextOccurrenceData.FontSize), nextOccurrenceData.Position, 1f, nextOccurrenceData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = nextOccurrenceData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation + 180f,
					RotationX = 90f,
					ScaleY = nextOccurrenceData.Scaling.Height,
					ScaleX = nextOccurrenceData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "nextOccurrence-back");
					}
				}
			});
			return entites;
		}

		private List<WorldEntity> RenderBlackLionBoardModel(Vector3 centerAsWorldMeters, Event ev, EventWorldTimer worldTimer, Func<WorldEntity, bool> renderCondition, int worldTimerIndex)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0701: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Unknown result type (might be due to invalid IL or missing references)
			List<WorldEntity> entites = new List<WorldEntity>();
			float rotation = worldTimer.Rotation;
			BlenderRenderEntity blenderRenderEntity = new BlenderRenderEntity(centerAsWorldMeters)
			{
				RenderCondition = renderCondition,
				ScaleX = 0.5f,
				ScaleY = 0.5f,
				ScaleZ = 0.5f,
				RotationX = 90f,
				RotationZ = rotation - 90f
			};
			Stream objStream = _contentsManager.GetFileStream("props/EventTimers2.obj");
			blenderRenderEntity.LoadModel(ev.SettingKey, objStream, (string fileName) => _contentsManager.GetFileStream("props/" + fileName));
			Matrix rotationMatrixCenterBoard = Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) * Matrix.CreateRotationX(MathHelper.ToRadians(90f));
			Vector3 localOutward = Vector3.get_Forward();
			float moveTextForwardAmount = 0.8f;
			Vector3 centerBoardVector = centerAsWorldMeters + Vector3.TransformNormal(localOutward, rotationMatrixCenterBoard) * moveTextForwardAmount;
			TimerData nameData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = centerBoardVector + new Vector3(0f, 0f, 3.5f),
				Scaling = new Size2(2f, 0.2f),
				TextCallback = () => ev.Name,
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNameTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersNameTextureWidth.get_Value(), _moduleSettings.EventTimersNameTextureHeight.get_Value())
			};
			TimerData durationData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = nameData.Position + new Vector3(0f, 0f, -0.35f),
				Scaling = new Size2(2f, 0.2f),
				TextCallback = () => "Duration: " + ev.Duration.ToTimeSpan().Humanize(1, null, TimeUnit.Hour, TimeUnit.Minute),
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNameTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersDurationTextureWidth.get_Value(), _moduleSettings.EventTimersDurationTextureHeight.get_Value())
			};
			TimerData repeatData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = durationData.Position + new Vector3(0f, 0f, -0.35f),
				Scaling = new Size2(2f, 0.2f),
				TextCallback = () => "Repeats every: " + ev.Repeat.ToTimeSpan().Humanize(),
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersRepeatTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersRepeatTextureWidth.get_Value(), _moduleSettings.EventTimersRepeatTextureHeight.get_Value())
			};
			TimerData remainingData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = repeatData.Position + new Vector3(0f, 0f, -0.45f),
				Scaling = new Size2(2.25f, 0.25f),
				TextCallback = delegate
				{
					Instant currentOccurrence = ev.GetCurrentOccurrence();
					string text = "---";
					if (currentOccurrence != default(Instant))
					{
						text = (currentOccurrence.Plus(ev.Duration) - _getNow()).ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second);
					}
					return "Remaining: " + text;
				},
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersRemainingTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersRemainingTextureWidth.get_Value(), _moduleSettings.EventTimersRemainingTextureHeight.get_Value())
			};
			TimerData startsInData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = remainingData.Position + new Vector3(0f, 0f, -0.45f),
				Scaling = new Size2(2.25f, 0.25f),
				TextCallback = () => "Next in: " + (ev.GetNextOccurrence() - _getNow()).ToTimeSpan().Humanize(2, null, TimeUnit.Week, TimeUnit.Second),
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersStartsInTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersStartsInTextureWidth.get_Value(), _moduleSettings.EventTimersStartsInTextureHeight.get_Value())
			};
			TimerData nextOccurrenceData = new TimerData
			{
				FontSize = (FontSize)36,
				Position = startsInData.Position + new Vector3(0f, 0f, -0.45f),
				Scaling = new Size2(2.25f, 0.25f),
				TextCallback = () => $"Next at: {ev.GetNextOccurrence().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).ToDateTimeOffset():g}",
				ColorCallback = () => ColorExtensions.ToXnaColor(_moduleSettings.EventTimersNextOccurenceTextColor.get_Value().get_Cloth()),
				TextureSizeCallback = () => new Size(_moduleSettings.EventTimersNextOccurrenceTextureWidth.get_Value(), _moduleSettings.EventTimersNextOccurrenceTextureHeight.get_Value())
			};
			entites.AddRange(new WorldEntity[7]
			{
				blenderRenderEntity,
				new WorldText(nameData.TextCallback, GetFont(nameData.FontSize), nameData.Position, 1f, nameData.ColorCallback)
				{
					TextureSizeCallback = nameData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = nameData.Scaling.Height,
					ScaleX = nameData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(durationData.TextCallback, GetFont(durationData.FontSize), durationData.Position, 1f, durationData.ColorCallback)
				{
					TextureSizeCallback = durationData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = durationData.Scaling.Height,
					ScaleX = durationData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new WorldText(repeatData.TextCallback, GetFont(repeatData.FontSize), repeatData.Position, 1f, repeatData.ColorCallback)
				{
					TextureSizeCallback = repeatData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = repeatData.Scaling.Height,
					ScaleX = repeatData.Scaling.Width,
					RenderCondition = renderCondition
				},
				new DissolvableWorldText(remainingData.TextCallback, GetFont(remainingData.FontSize), remainingData.Position, 1f, remainingData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = remainingData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = remainingData.Scaling.Height,
					ScaleX = remainingData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "remaining-front");
					}
				},
				new DissolvableWorldText(startsInData.TextCallback, GetFont(startsInData.FontSize), startsInData.Position, 1f, startsInData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = startsInData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = remainingData.Scaling.Height,
					ScaleX = remainingData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "startsIn-front");
					}
				},
				new DissolvableWorldText(nextOccurrenceData.TextCallback, GetFont(nextOccurrenceData.FontSize), nextOccurrenceData.Position, 1f, nextOccurrenceData.ColorCallback, _contentsManager)
				{
					TextureSizeCallback = nextOccurrenceData.TextureSizeCallback,
					UseTextSizeAsMinimum = true,
					RotationZ = rotation,
					RotationX = 90f,
					ScaleY = nextOccurrenceData.Scaling.Height,
					ScaleX = nextOccurrenceData.Scaling.Width,
					RenderCondition = renderCondition,
					OnBeforeRender = delegate(DissolvableWorldText self)
					{
						RenderDissolvableEffect(self, ev, worldTimerIndex, "nextOccurrence-front");
					}
				}
			});
			return entites;
		}

		private void RenderDissolvableEffect(DissolvableWorldText self, Event ev, int worldTimerIndex, string sectionName)
		{
			Instant next = ev.GetNextOccurrence();
			if (next == default(Instant) || !((next - _getNow()).TotalSeconds <= 5.0))
			{
				return;
			}
			string animationKey = $"{ev.SettingKey}-{worldTimerIndex}-{sectionName}";
			if (_worldEntityAnimations.ContainsKey(animationKey))
			{
				return;
			}
			_worldEntityAnimations.TryAdd(animationKey, ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DissolvableWorldText>(self, (object)new
			{
				Amount = 1f
			}, 3f, 0f, true).OnComplete((Action)delegate
			{
				Tween fadeInTween = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DissolvableWorldText>(self, (object)new
				{
					Amount = 0f
				}, 10f, 0f, true).OnComplete((Action)delegate
				{
					_worldEntityAnimations.TryRemove(animationKey, out var _);
				});
				_worldEntityAnimations.AddOrUpdate(animationKey, fadeInTween, (string s, Tween tween) => fadeInTween);
			}));
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
