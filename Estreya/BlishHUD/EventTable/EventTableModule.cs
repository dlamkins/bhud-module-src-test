using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Contexts;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.Shared.Contexts;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.MumbleInfo.Map;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.Models;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : BaseModule<EventTableModule, ModuleSettings>
	{
		private static TimeSpan _updateEventsInterval = TimeSpan.FromMinutes(30.0);

		private static TimeSpan _checkDrawerSettingInterval = TimeSpan.FromSeconds(30.0);

		private ConcurrentDictionary<string, EventArea> _areas;

		private List<EventCategory> _eventCategories;

		private List<EventCategory> _temporaryEventCategories = new List<EventCategory>();

		private readonly AsyncLock _eventCategoryLock = new AsyncLock();

		private double _lastCheckDrawerSettings;

		private AsyncRef<double> _lastEventUpdate;

		private EventTableContext _eventTableContext;

		private ContextHandle<EventTableContext> _eventTableContextHandle;

		private BitmapFont _defaultFont;

		public override string UrlModuleName => "event-table";

		protected override bool FailIfBackendDown => true;

		private DateTime NowUTC => DateTime.UtcNow;

		private MapUtil MapUtil { get; set; }

		private DynamicEventHandler DynamicEventHandler { get; set; }

		protected override string API_VERSION_NO => "1";

		public override BitmapFont Font
		{
			get
			{
				if (_defaultFont == null)
				{
					using Stream defaultFontStream = base.ContentsManager.GetFileStream("fonts\\Anonymous.ttf");
					_defaultFont = ((defaultFontStream != null) ? FontUtils.FromTrueTypeFont(defaultFontStream.ToByteArray(), 18f, 256, 256).ToBitmapFont() : GameService.Content.get_DefaultFont16());
				}
				return _defaultFont;
			}
		}

		protected override int CornerIconPriority => 1289351278;

		public EventStateService EventStateService { get; private set; }

		public DynamicEventService DynamicEventService { get; private set; }

		[ImportingConstructor]
		public EventTableModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
			_areas = new ConcurrentDictionary<string, EventArea>();
			_eventCategories = new List<EventCategory>();
			_lastEventUpdate = new AsyncRef<double>(0.0);
			_lastCheckDrawerSettings = 0.0;
		}

		protected override async Task LoadAsync()
		{
			Stopwatch sw = Stopwatch.StartNew();
			await base.LoadAsync();
			base.BlishHUDAPIService.NewLogin += BlishHUDAPIService_NewLogin;
			base.BlishHUDAPIService.RefreshedLogin += BlishHUDAPIService_RefreshedLogin;
			base.BlishHUDAPIService.LoggedOut += BlishHUDAPIService_LoggedOut;
			MapUtil = new MapUtil(base.ModuleSettings.MapKeybinding.get_Value(), base.Gw2ApiManager);
			DynamicEventHandler = new DynamicEventHandler(MapUtil, DynamicEventService, base.Gw2ApiManager, base.ModuleSettings);
			DynamicEventHandler.FoundLostEntities += DynamicEventHandler_FoundLostEntities;
			await DynamicEventHandler.AddDynamicEventsToMap();
			await DynamicEventHandler.AddDynamicEventsToWorld();
			await LoadEvents();
			AddAllAreas();
			SetAreaEvents();
			sw.Stop();
			base.Logger.Debug("Loaded in " + sw.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms");
		}

		private void DynamicEventHandler_FoundLostEntities(object sender, EventArgs e)
		{
			ScreenNotification.ShowNotification(new string[2]
			{
				base.TranslationService.GetTranslation("dynamicEventHandler-foundLostEntities1", "GameService.Graphics.World.Entities has lost references."),
				base.TranslationService.GetTranslation("dynamicEventHandler-foundLostEntities2", "Expect dynamic event boundaries on screen.")
			}, ScreenNotification.NotificationType.Warning);
		}

		private void SetAreaEvents()
		{
			foreach (EventArea area in _areas.Values)
			{
				SetAreaEvents(area);
			}
		}

		private void SetAreaEvents(EventArea area)
		{
			area.UpdateAllEvents(_eventCategories);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			RegisterContext();
		}

		private void RegisterContext()
		{
			if (!base.ModuleSettings.RegisterContext.get_Value())
			{
				base.Logger.Info("Event Table context was not registered due to user preferences.");
				return;
			}
			_eventTableContext = new EventTableContext();
			_eventTableContext.RequestAddCategory += EventTableContext_RequestAddCategory;
			_eventTableContext.RequestAddEvent += EventTableContext_RequestAddEvent;
			_eventTableContext.RequestRemoveCategory += EventTableContext_RequestRemoveCategory;
			_eventTableContext.RequestRemoveEvent += EventTableContext_RequestRemoveEvent;
			_eventTableContext.RequestReloadEvents += EventTableContext_RequestReloadEvents;
			_eventTableContext.RequestShowReminder += EventTableContext_RequestShowReminder;
			_eventTableContextHandle = GameService.Contexts.RegisterContext<EventTableContext>(_eventTableContext);
			base.Logger.Info("Event Table context registered.");
		}

		private Task EventTableContext_RequestShowReminder(object sender, ContextEventArgs<ShowReminder> e)
		{
			ShowReminder eArgsContent = e.Content;
			EventNotification eventNotification = new EventNotification(null, eArgsContent.Title, eArgsContent.Message, (!string.IsNullOrWhiteSpace(eArgsContent.Icon)) ? base.IconService.GetIcon(eArgsContent.Icon) : null, base.ModuleSettings.ReminderPosition.X.get_Value(), base.ModuleSettings.ReminderPosition.Y.get_Value(), base.ModuleSettings.ReminderStackDirection.get_Value(), base.IconService, base.ModuleSettings.ReminderLeftClickAction.get_Value() != LeftClickAction.None);
			eventNotification.BackgroundOpacity = base.ModuleSettings.ReminderOpacity.get_Value();
			eventNotification.Show(TimeSpan.FromSeconds(base.ModuleSettings.ReminderDuration.get_Value()));
			return Task.CompletedTask;
		}

		private async Task EventTableContext_RequestReloadEvents(object sender, ContextEventArgs e)
		{
			base.Logger.Info("\"" + e.Caller.FullName + "\" trggered a event reload via context.");
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
			await AsyncHelper.WaitUntil(() => _lastEventUpdate.Value < _updateEventsInterval.TotalMilliseconds, TimeSpan.FromSeconds(15.0));
		}

		private async Task EventTableContext_RequestRemoveEvent(object sender, ContextEventArgs<RemoveEvent> e)
		{
			RemoveEvent eArgsContent = e.Content;
			using (await _eventCategoryLock.LockAsync())
			{
				EventCategory obj = _temporaryEventCategories.FirstOrDefault((EventCategory ec) => ec.Key == eArgsContent.CategoryKey) ?? throw new ArgumentException("Category with key \"" + eArgsContent.CategoryKey + "\" does not exist.");
				if (!obj.Events.Any((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key == eArgsContent.EventKey))
				{
					throw new ArgumentException("Event with the key \"" + eArgsContent.EventKey + "\" does not exist.");
				}
				obj.UpdateOriginalEvents(obj.OriginalEvents.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key != eArgsContent.EventKey).ToList());
				obj.UpdateFillers(obj.FillerEvents.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key != eArgsContent.EventKey).ToList());
				base.Logger.Info("Event \"" + eArgsContent.EventKey + "\" of category \"" + eArgsContent.CategoryKey + "\" was removed via context.");
			}
		}

		private async Task EventTableContext_RequestRemoveCategory(object sender, ContextEventArgs<string> e)
		{
			using (await _eventCategoryLock.LockAsync())
			{
				EventCategory category = _temporaryEventCategories.FirstOrDefault((EventCategory ec) => ec.Key == e.Content) ?? throw new ArgumentException("Category with key \"" + e.Content + "\" does not exist.");
				_temporaryEventCategories.Remove(category);
				base.Logger.Info("Category \"" + category.Name + "\" (" + category.Key + ") was removed via context.");
			}
		}

		private async Task EventTableContext_RequestAddEvent(object sender, ContextEventArgs<AddEvent> e)
		{
			AddEvent eArgsContent = e.Content;
			using (await _eventCategoryLock.LockAsync())
			{
				EventCategory category = _temporaryEventCategories.FirstOrDefault((EventCategory ec) => ec.Key == eArgsContent.CategoryKey) ?? throw new ArgumentException("Category with key \"" + eArgsContent.Key + "\" does not exist.");
				if (category.Events.Any((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key == eArgsContent.Key))
				{
					throw new ArgumentException("Event with the key \"" + eArgsContent.Key + "\" already exists.");
				}
				Estreya.BlishHUD.EventTable.Models.Event newEvent = new Estreya.BlishHUD.EventTable.Models.Event
				{
					Key = eArgsContent.Key,
					Name = eArgsContent.Name,
					APICode = eArgsContent.APICode,
					APICodeType = eArgsContent.APICodeType,
					BackgroundColorCode = eArgsContent.BackgroundColorCode,
					BackgroundColorGradientCodes = eArgsContent.BackgroundColorGradientCodes,
					Duration = eArgsContent.Duration,
					Filler = eArgsContent.Filler,
					Icon = eArgsContent.Icon,
					Location = eArgsContent.Location,
					MapIds = eArgsContent.MapIds,
					Offset = eArgsContent.Offset,
					Repeat = eArgsContent.Repeat,
					StartingDate = eArgsContent.StartingDate,
					Waypoint = eArgsContent.Waypoint,
					Wiki = eArgsContent.Wiki
				};
				if (eArgsContent.Occurences != null)
				{
					newEvent.Occurences.AddRange(eArgsContent.Occurences);
				}
				if (eArgsContent.ReminderTimes != null)
				{
					newEvent.UpdateReminderTimes(eArgsContent.ReminderTimes);
				}
				if (newEvent.Filler)
				{
					category.UpdateFillers(new List<Estreya.BlishHUD.EventTable.Models.Event>(category.FillerEvents) { newEvent });
				}
				else
				{
					category.UpdateOriginalEvents(new List<Estreya.BlishHUD.EventTable.Models.Event>(category.OriginalEvents) { newEvent });
				}
				base.Logger.Info("Event \"" + eArgsContent.Name + "\" (" + eArgsContent.Key + ") of category \"" + category.Name + "\" (" + category.Key + ") was registered via context.");
			}
		}

		private async Task EventTableContext_RequestAddCategory(object sender, ContextEventArgs<AddCategory> e)
		{
			AddCategory eArgsContent = e.Content;
			using (await _eventCategoryLock.LockAsync())
			{
				if (_temporaryEventCategories.Any((EventCategory ec) => ec.Key == eArgsContent.Key))
				{
					throw new ArgumentException("Category with key \"" + eArgsContent.Key + "\" already exists.");
				}
				_temporaryEventCategories.Add(new EventCategory
				{
					Key = eArgsContent.Key,
					Name = eArgsContent.Name,
					Icon = eArgsContent.Icon,
					ShowCombined = eArgsContent.ShowCombined,
					FromContext = true
				});
				base.Logger.Info("Category \"" + eArgsContent.Name + "\" (" + eArgsContent.Key + ") was registered via context.");
			}
		}

		public async Task LoadEvents()
		{
			base.Logger.Info("Load events...");
			using (await _eventCategoryLock.LockAsync())
			{
				base.Logger.Debug("Acquired lock.");
				try
				{
					_eventCategories?.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(RemoveEventHooks);
					_eventCategories?.Clear();
					IFlurlRequest request = GetFlurlClient().Request(base.MODULE_API_URL, "events");
					if (!string.IsNullOrWhiteSpace(base.BlishHUDAPIService.AccessToken))
					{
						base.Logger.Info("Include custom events...");
						request.WithOAuthBearerToken(base.BlishHUDAPIService.AccessToken);
					}
					List<EventCategory> categories = await request.GetJsonAsync<List<EventCategory>>(default(CancellationToken), (HttpCompletionOption)0);
					int eventCategoryCount = categories.Count;
					int eventCount = categories.Sum((EventCategory ec) => ec.Events.Count);
					base.Logger.Info($"Loaded {eventCategoryCount} Categories with {eventCount} Events.");
					if (_temporaryEventCategories != null && _temporaryEventCategories.Count > 0)
					{
						base.Logger.Info($"Include {_temporaryEventCategories.Count} temporary categories with {_temporaryEventCategories.Sum((EventCategory ec) => ec.Events?.Count ?? 0)}.");
						categories.AddRange(_temporaryEventCategories);
					}
					categories.ForEach(delegate(EventCategory ec)
					{
						ec.Load(() => NowUTC, base.TranslationService);
					});
					base.Logger.Debug("Loaded all event categories.");
					AssignEventReminderTimes(categories);
					_eventCategories = categories;
					foreach (Estreya.BlishHUD.EventTable.Models.Event ev in _eventCategories.SelectMany((EventCategory ec) => ec.Events))
					{
						AddEventHooks(ev);
					}
					_lastCheckDrawerSettings = _checkDrawerSettingInterval.TotalMilliseconds;
					SetAreaEvents();
					base.Logger.Debug("Updated events in all areas.");
				}
				catch (FlurlHttpException ex2)
				{
					string message = await ex2.GetResponseStringAsync();
					base.Logger.Warn((Exception)ex2, "Failed loading events: " + message);
				}
				catch (Exception ex)
				{
					base.Logger.Error(ex, "Failed loading events.");
				}
			}
		}

		private void AssignEventReminderTimes(List<EventCategory> categories)
		{
			foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in from ev in categories.SelectMany((EventCategory ec) => ec.Events)
				where !ev.Filler
				select ev)
			{
				if (base.ModuleSettings.ReminderTimesOverride.get_Value().ContainsKey(ev2.SettingKey))
				{
					List<TimeSpan> times = base.ModuleSettings.ReminderTimesOverride.get_Value()[ev2.SettingKey];
					ev2.UpdateReminderTimes(times.ToArray());
				}
			}
		}

		private void CheckDrawerSettings()
		{
			if (!_eventCategoryLock.IsFree())
			{
				return;
			}
			using (_eventCategoryLock.Lock())
			{
				foreach (KeyValuePair<string, EventArea> area in _areas)
				{
					base.ModuleSettings.CheckDrawerSettings(area.Value.Configuration, _eventCategories);
				}
			}
		}

		private void ToggleContainers()
		{
			bool show = base.ShowUI && base.ModuleSettings.GlobalDrawerVisible.get_Value();
			foreach (EventArea area in _areas.Values)
			{
				if (show && area.Enabled && area.CalculateUIVisibility())
				{
					if (!((Control)area).get_Visible())
					{
						((Control)area).Show();
					}
				}
				else if (((Control)area).get_Visible())
				{
					((Control)area).Hide();
				}
			}
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			ToggleContainers();
			base.ModuleSettings.CheckGlobalSizeAndPosition();
			foreach (EventArea area in _areas.Values)
			{
				base.ModuleSettings.CheckDrawerSizeAndPosition(area.Configuration);
			}
			if (_eventCategoryLock.IsFree())
			{
				using (_eventCategoryLock.Lock())
				{
					foreach (Estreya.BlishHUD.EventTable.Models.Event item in _eventCategories.SelectMany((EventCategory ec) => ec.Events))
					{
						item.Update(gameTime);
					}
				}
			}
			DynamicEventHandler.Update(gameTime);
			UpdateUtil.Update(CheckDrawerSettings, gameTime, _checkDrawerSettingInterval.TotalMilliseconds, ref _lastCheckDrawerSettings);
			UpdateUtil.UpdateAsync(LoadEvents, gameTime, _updateEventsInterval.TotalMilliseconds, _lastEventUpdate);
		}

		private bool CalculateReminderUIVisibility()
		{
			bool show = true;
			if (base.ModuleSettings.HideRemindersOnOpenMap.get_Value())
			{
				show &= !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			if (base.ModuleSettings.HideRemindersOnMissingMumbleTicks.get_Value())
			{
				show &= GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds < 0.5;
			}
			if (base.ModuleSettings.HideRemindersInCombat.get_Value())
			{
				show &= !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
			}
			if (base.ModuleSettings.HideRemindersInPvE_OpenWorld.get_Value())
			{
				MapType[] array = new MapType[4];
				RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				MapType[] pveOpenWorldMapTypes = (MapType[])(object)array;
				show &= GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pveOpenWorldMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type()) || MapInfo.MAP_IDS_PVE_COMPETETIVE.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
			if (base.ModuleSettings.HideRemindersInPvE_Competetive.get_Value())
			{
				MapType[] pveCompetetiveMapTypes = (MapType[])(object)new MapType[1] { (MapType)4 };
				show &= GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pveCompetetiveMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type()) || !MapInfo.MAP_IDS_PVE_COMPETETIVE.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			}
			if (base.ModuleSettings.HideRemindersInWvW.get_Value())
			{
				MapType[] array2 = new MapType[5];
				RuntimeHelpers.InitializeArray(array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				MapType[] wvwMapTypes = (MapType[])(object)array2;
				show &= !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !wvwMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type());
			}
			if (base.ModuleSettings.HideRemindersInPvP.get_Value())
			{
				MapType[] pvpMapTypes = (MapType[])(object)new MapType[2]
				{
					(MapType)2,
					(MapType)6
				};
				show &= !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() || !pvpMapTypes.Any((MapType type) => type == GameService.Gw2Mumble.get_CurrentMap().get_Type());
			}
			return show;
		}

		private void AddEventHooks(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			ev.Reminder += Ev_Reminder;
		}

		private void RemoveEventHooks(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			ev.Reminder -= Ev_Reminder;
		}

		private void Ev_Reminder(object sender, TimeSpan e)
		{
			Estreya.BlishHUD.EventTable.Models.Event ev = sender as Estreya.BlishHUD.EventTable.Models.Event;
			if (base.ModuleSettings.RemindersEnabled.get_Value() && !base.ModuleSettings.ReminderDisabledForEvents.get_Value().Contains(ev.SettingKey))
			{
				if (!CalculateReminderUIVisibility())
				{
					base.Logger.Debug("Reminder " + ev.SettingKey + " was not displayed due to UI Visibility settings.");
					return;
				}
				string startsInTranslation = base.TranslationService.GetTranslation("reminder-startsIn", "Starts in");
				EventNotification obj = new EventNotification(ev, startsInTranslation + " " + e.Humanize(2, null, TimeUnit.Week, TimeUnit.Second) + "!", base.ModuleSettings.ReminderPosition.X.get_Value(), base.ModuleSettings.ReminderPosition.Y.get_Value(), base.ModuleSettings.ReminderStackDirection.get_Value(), base.IconService, base.ModuleSettings.ReminderLeftClickAction.get_Value() != LeftClickAction.None)
				{
					BackgroundOpacity = base.ModuleSettings.ReminderOpacity.get_Value()
				};
				((Control)obj).add_Click((EventHandler<MouseEventArgs>)EventNotification_Click);
				((Control)obj).add_Disposed((EventHandler<EventArgs>)EventNotification_Disposed);
				obj.Show(TimeSpan.FromSeconds(base.ModuleSettings.ReminderDuration.get_Value()));
			}
		}

		private void EventNotification_Disposed(object sender, EventArgs e)
		{
			EventNotification obj = sender as EventNotification;
			((Control)obj).remove_Click((EventHandler<MouseEventArgs>)EventNotification_Click);
			((Control)obj).remove_Disposed((EventHandler<EventArgs>)EventNotification_Disposed);
		}

		private void EventNotification_Click(object sender, MouseEventArgs e)
		{
			EventNotification notification = sender as EventNotification;
			switch (base.ModuleSettings.ReminderLeftClickAction.get_Value())
			{
			case LeftClickAction.CopyWaypoint:
				if (!string.IsNullOrWhiteSpace(notification.Model.Waypoint))
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(notification.Model.Waypoint);
					ScreenNotification.ShowNotification(new string[2]
					{
						notification.Model.Name,
						"Copied to clipboard!"
					});
				}
				break;
			case LeftClickAction.NavigateToWaypoint:
			{
				if (string.IsNullOrWhiteSpace(notification.Model.Waypoint))
				{
					break;
				}
				if (base.PointOfInterestService.Loading)
				{
					ScreenNotification.ShowNotification("PointOfInterestService is still loading!", ScreenNotification.NotificationType.Error);
					break;
				}
				PointOfInterest poi = base.PointOfInterestService.GetPointOfInterest(notification.Model.Waypoint);
				if (poi == null)
				{
					ScreenNotification.ShowNotification(notification.Model.Waypoint + " not found!", ScreenNotification.NotificationType.Error);
					break;
				}
				Task.Run(async delegate
				{
					MapUtil.NavigationResult result = await (MapUtil?.NavigateToPosition(poi, base.ModuleSettings.AcceptWaypointPrompt.get_Value()) ?? Task.FromResult(new MapUtil.NavigationResult(success: false, "Variable null.")));
					if (!result.Success)
					{
						ScreenNotification.ShowNotification("Navigation failed: " + (result.Message ?? "Unknown"), ScreenNotification.NotificationType.Error);
					}
				});
				break;
			}
			}
		}

		private void AddAllAreas()
		{
			if (base.ModuleSettings.EventAreaNames.get_Value().Count == 0)
			{
				base.ModuleSettings.EventAreaNames.get_Value().Add("Main");
			}
			foreach (string areaName in base.ModuleSettings.EventAreaNames.get_Value())
			{
				AddArea(areaName);
			}
		}

		private EventAreaConfiguration AddArea(string name)
		{
			EventAreaConfiguration config = base.ModuleSettings.AddDrawer(name, _eventCategories);
			AddArea(config);
			return config;
		}

		private void AddArea(EventAreaConfiguration configuration)
		{
			if (!base.ModuleSettings.EventAreaNames.get_Value().Contains(configuration.Name))
			{
				base.ModuleSettings.EventAreaNames.set_Value(new List<string>(base.ModuleSettings.EventAreaNames.get_Value()) { configuration.Name });
			}
			base.ModuleSettings.UpdateDrawerLocalization(configuration, base.TranslationService);
			EventArea eventArea = new EventArea(configuration, base.IconService, base.TranslationService, EventStateService, base.WorldbossService, base.MapchestService, base.PointOfInterestService, MapUtil, GetFlurlClient(), base.MODULE_API_URL, () => NowUTC, () => ((Module)this).get_Version(), () => base.BlishHUDAPIService.AccessToken, base.ContentsManager);
			((Control)eventArea).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			EventArea area = eventArea;
			_areas.AddOrUpdate(configuration.Name, area, (string name, EventArea prev) => area);
		}

		private void RemoveArea(EventAreaConfiguration configuration)
		{
			base.ModuleSettings.EventAreaNames.set_Value(new List<string>(from areaName in base.ModuleSettings.EventAreaNames.get_Value()
				where areaName != configuration.Name
				select areaName));
			EventArea eventArea = _areas[configuration.Name];
			if (eventArea != null)
			{
				((Control)eventArea).Dispose();
			}
			_areas.TryRemove(configuration.Name, out var _);
			base.ModuleSettings.RemoveDrawer(configuration.Name);
		}

		protected override BaseModuleSettings DefineModuleSettings(SettingCollection settings)
		{
			return new ModuleSettings(settings);
		}

		protected override void OnSettingWindowBuild(TabbedWindow settingWindow)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Expected O, but got Unknown
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Expected O, but got Unknown
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Expected O, but got Unknown
			settingWindow.SavesSize = true;
			settingWindow.CanResize = true;
			settingWindow.RebuildViewAfterResize = true;
			settingWindow.UnloadOnRebuild = false;
			settingWindow.MinSize = settingWindow.Size;
			settingWindow.MaxSize = new Point(((Control)settingWindow).get_Width() * 2, ((Control)settingWindow).get_Height() * 3);
			settingWindow.RebuildDelay = 500;
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156736.png"), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(base.ModuleSettings, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("generalSettingsView-title", "General"), (int?)null));
			AreaSettingsView areaSettingsView = new AreaSettingsView(() => _areas.Values.Select((EventArea area) => area.Configuration), () => _eventCategories, base.ModuleSettings, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, EventStateService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			};
			areaSettingsView.AddArea += delegate(object s, AreaSettingsView.AddAreaEventArgs e)
			{
				e.AreaConfiguration = AddArea(e.Name);
				if (e.AreaConfiguration != null)
				{
					EventArea areaEvents = _areas.Values.Where((EventArea x) => x.Configuration.Name == e.Name).First();
					SetAreaEvents(areaEvents);
				}
			};
			areaSettingsView.RemoveArea += delegate(object s, EventAreaConfiguration e)
			{
				RemoveArea(e);
			};
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("605018.png"), (Func<IView>)(() => (IView)(object)areaSettingsView), base.TranslationService.GetTranslation("areaSettingsView-title", "Event Areas"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("1466345.png"), (Func<IView>)(() => (IView)(object)new ReminderSettingsView(base.ModuleSettings, () => _eventCategories, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("reminderSettingsView-title", "Reminders"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("759448.png"), (Func<IView>)(() => (IView)(object)new DynamicEventsSettingsView(DynamicEventService, base.ModuleSettings, GetFlurlClient(), base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("dynamicEventsSettingsView-title", "Dynamic Events"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156764.png"), (Func<IView>)(() => (IView)(object)new CustomEventView(base.Gw2ApiManager, base.IconService, base.TranslationService, base.BlishHUDAPIService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("customEventView-title", "Custom Events"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("157097.png"), (Func<IView>)(() => (IView)(object)new HelpView(() => _eventCategories, base.MODULE_API_URL, base.Gw2ApiManager, base.IconService, base.TranslationService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("helpView-title", "Help"), (int?)null));
		}

		protected override string GetDirectoryName()
		{
			return "events";
		}

		protected override void ConfigureServices(ServiceConfigurations configurations)
		{
			configurations.BlishHUDAPI.Enabled = true;
			configurations.Account.Enabled = true;
			configurations.Account.AwaitLoading = true;
			configurations.Worldbosses.Enabled = true;
			configurations.Mapchests.Enabled = true;
			configurations.PointOfInterests.Enabled = true;
		}

		private void BlishHUDAPIService_NewLogin(object sender, EventArgs e)
		{
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
		}

		private void BlishHUDAPIService_RefreshedLogin(object sender, EventArgs e)
		{
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
		}

		private void BlishHUDAPIService_LoggedOut(object sender, EventArgs e)
		{
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
		}

		protected override Collection<ManagedService> GetAdditionalServices(string directoryPath)
		{
			Collection<ManagedService> collection = new Collection<ManagedService>();
			EventStateService = new EventStateService(new ServiceConfiguration
			{
				AwaitLoading = false,
				Enabled = true,
				SaveInterval = TimeSpan.FromSeconds(30.0)
			}, directoryPath, () => NowUTC);
			DynamicEventService = new DynamicEventService(new APIServiceConfiguration
			{
				AwaitLoading = false,
				Enabled = true,
				SaveInterval = Timeout.InfiniteTimeSpan
			}, base.Gw2ApiManager, GetFlurlClient(), "https://api.estreya.de/blish-hud", directoryPath);
			collection.Add(EventStateService);
			collection.Add(DynamicEventService);
			return collection;
		}

		protected override AsyncTexture2D GetEmblem()
		{
			return base.IconService.GetIcon(base.IsPrerelease ? "textures/emblem_demo.png" : "102392.png");
		}

		protected override AsyncTexture2D GetCornerIcon()
		{
			return base.IconService.GetIcon("textures/event_boss_grey" + (base.IsPrerelease ? "_demo" : "") + ".png");
		}

		protected override void Unload()
		{
			base.Logger.Debug("Unload module.");
			if (DynamicEventHandler != null)
			{
				DynamicEventHandler.FoundLostEntities -= DynamicEventHandler_FoundLostEntities;
				DynamicEventHandler.Dispose();
				DynamicEventHandler = null;
			}
			_eventTableContextHandle?.Expire();
			base.Logger.Info("Event Table context expired.");
			if (_eventTableContext != null)
			{
				_eventTableContext.RequestAddCategory -= EventTableContext_RequestAddCategory;
				_eventTableContext.RequestAddEvent -= EventTableContext_RequestAddEvent;
				_eventTableContext.RequestRemoveCategory -= EventTableContext_RequestRemoveCategory;
				_eventTableContext.RequestRemoveEvent -= EventTableContext_RequestRemoveEvent;
				_eventTableContext.RequestReloadEvents -= EventTableContext_RequestReloadEvents;
				_eventTableContext.RequestShowReminder -= EventTableContext_RequestShowReminder;
				_eventTableContext = null;
			}
			MapUtil?.Dispose();
			MapUtil = null;
			base.Logger.Debug("Unload drawer.");
			if (_areas != null)
			{
				foreach (EventArea value in _areas.Values)
				{
					if (value != null)
					{
						((Control)value).Dispose();
					}
				}
				_areas?.Clear();
			}
			base.Logger.Debug("Unloaded drawer.");
			base.Logger.Debug("Unload events.");
			using (_eventCategoryLock.Lock())
			{
				foreach (EventCategory eventCategory in _eventCategories)
				{
					eventCategory.Events.ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
					{
						RemoveEventHooks(ev);
					});
				}
				_eventCategories?.Clear();
			}
			base.Logger.Debug("Unloaded events.");
			if (base.BlishHUDAPIService != null)
			{
				base.BlishHUDAPIService.NewLogin -= BlishHUDAPIService_NewLogin;
				base.BlishHUDAPIService.LoggedOut -= BlishHUDAPIService_LoggedOut;
			}
			base.Logger.Debug("Unload base.");
			base.Unload();
			base.Logger.Debug("Unloaded base.");
		}
	}
}
