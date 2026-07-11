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
using Estreya.BlishHUD.EventTable.Models.Reminders;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.EventTable.UI.Views.Wizard;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Chat;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.MumbleInfo.Map;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.Models;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using NodaTime;
using NodaTime.Extensions;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : BaseModule<EventTableModule, ModuleSettings>
	{
		private static TimeSpan _updateEventsInterval = TimeSpan.FromMinutes(30.0);

		private static TimeSpan _checkDrawerSettingInterval = TimeSpan.FromSeconds(30.0);

		private ConcurrentDictionary<string, EventArea> _areas;

		private List<EventCategory> _eventCategories;

		private List<EventCategory> _eventCategoriesCompact;

		private readonly AsyncLock _eventCategoryLock = new AsyncLock();

		private double _lastCheckDrawerSettings;

		private AsyncRef<double> _lastEventUpdate;

		private EventTableContext _eventTableContext;

		private ContextManager _contextManager;

		private ContextHandle<EventTableContext> _eventTableContextHandle;

		private StandardWindow _eventTimesTableWindow;

		private BitmapFont _defaultFont;

		protected override string UrlModuleName => "event-table";

		protected override bool NeedsBackend => true;

		protected override bool EnableMetrics => false;

		private Instant NowUTC => SystemClock.Instance.GetCurrentInstant();

		private MapUtil MapUtil { get; set; }

		private DynamicEventHandler DynamicEventHandler { get; set; }

		protected override string API_VERSION_NO => "2";

		protected override BitmapFont Font
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

		public EventTimerHandler EventTimerHandler { get; private set; }

		public SelfHostingEventService SelfHostingEventService { get; private set; }

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
			_eventCategoriesCompact = new List<EventCategory>();
			_lastEventUpdate = new AsyncRef<double>(0.0);
			_lastCheckDrawerSettings = 0.0;
			base.BackendConnectionRestored += EventTableModule_BackendConnectionRestored;
		}

		private async Task EventTableModule_BackendConnectionRestored(object sender)
		{
			await ReloadEvents();
		}

		protected override async Task LoadAsync()
		{
			Stopwatch sw = Stopwatch.StartNew();
			base.ModuleSettings.ValidateAndTryFixSettings();
			await base.LoadAsync();
			await base.AudioService.RegisterSubfolder(EventNotification.GetAudioServiceBaseSubfolder());
			await base.AudioService.RegisterSubfolder(EventNotification.GetAudioServiceEventsSubfolder());
			base.BlishHUDAPIService.NewLogin += BlishHUDAPIService_NewLogin;
			base.BlishHUDAPIService.RefreshedLogin += BlishHUDAPIService_RefreshedLogin;
			base.BlishHUDAPIService.LoggedOut += BlishHUDAPIService_LoggedOut;
			MapUtil = new MapUtil(base.ModuleSettings.MapKeybinding.get_Value(), base.Gw2ApiManager);
			DynamicEventHandler = new DynamicEventHandler(MapUtil, DynamicEventService, base.Gw2ApiManager, base.ModuleSettings);
			DynamicEventHandler.FoundLostEntities += DynamicEventHandler_FoundLostEntities;
			await DynamicEventHandler.AddDynamicEventsToMap();
			await DynamicEventHandler.AddDynamicEventsToWorld();
			EventTimerHandler = new EventTimerHandler(async delegate
			{
				using (await _eventCategoryLock.LockAsync())
				{
					return _eventCategories.SelectMany((EventCategory ec) => ec.Events).ToList();
				}
			}, () => NowUTC, MapUtil, base.Gw2ApiManager, base.ModuleSettings, base.TranslationService, base.IconService, base.ContentsManager);
			EventTimerHandler.FoundLostEntities += EventTimerHandler_FoundLostEntities;
			base.ModuleSettings.ShowEventTimeTableWindowKeybinding.get_Value().add_Activated((EventHandler<EventArgs>)OnShowEventTimeTableWindowKeybindingActivated);
			AddAllAreas();
			await LoadEventsAsync();
			await PerformSanityCheck();
			sw.Stop();
			base.Logger.Debug("Loaded in " + sw.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms");
		}

		private void OnShowEventTimeTableWindowKeybindingActivated(object sender, EventArgs e)
		{
			ShowEventTimesTable(showAndDontToggle: false);
		}

		private void DynamicEventHandler_FoundLostEntities(object sender, EventArgs e)
		{
			ScreenNotification.ShowNotification(new string[2]
			{
				base.TranslationService.GetTranslation("dynamicEventHandler-foundLostEntities1", "GameService.Graphics.World.Entities has lost references."),
				base.TranslationService.GetTranslation("dynamicEventHandler-foundLostEntities2", "Expect dynamic event boundaries on screen.")
			}, ScreenNotification.NotificationType.Warning);
		}

		private void EventTimerHandler_FoundLostEntities(object sender, EventArgs e)
		{
			ScreenNotification.ShowNotification(new string[2]
			{
				base.TranslationService.GetTranslation("eventTimerHandler-foundLostEntities1", "GameService.Graphics.World.Entities has lost references."),
				base.TranslationService.GetTranslation("eventTimerHandler-foundLostEntities2", "Expect event timers on map or in world.")
			}, ScreenNotification.NotificationType.Warning);
		}

		private async Task SetAreaEventsAsync()
		{
			await Task.WhenAll(_areas.Values.Select(SetAreaEventsAsync));
		}

		private async Task SetAreaEventsAsync(EventArea area)
		{
			if (!area.Configuration.CompactMode.get_Value())
			{
				await area.UpdateAllEventsAsync(_eventCategories);
			}
			else
			{
				await area.UpdateAllEventsAsync(_eventCategoriesCompact);
			}
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
			_contextManager = new ContextManager(_eventTableContext, base.ModuleSettings, DynamicEventService, base.IconService, EventStateService, base.AudioService, async () => (await GetAllEvents()).SelectMany((EventCategory ec) => ec.Events));
			_contextManager.ReloadEvents += ContextManager_ReloadEvents;
			_eventTableContextHandle = GameService.Contexts.RegisterContext<EventTableContext>(_eventTableContext);
			base.Logger.Info("Event Table context registered.");
		}

		private async Task ContextManager_ReloadEvents(object sender)
		{
			await ReloadEvents();
		}

		private async Task ReloadEvents()
		{
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
			await AsyncHelper.WaitUntil(() => _lastEventUpdate.Value < _updateEventsInterval.TotalMilliseconds, TimeSpan.FromSeconds(15.0));
		}

		private async Task LoadEventsAsync()
		{
			base.Logger.Info("Load all events...");
			using (await _eventCategoryLock.LockAsync())
			{
				base.Logger.Debug("Acquired lock.");
				try
				{
					_eventCategories?.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(RemoveEventHooks);
					_eventCategoriesCompact?.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(RemoveEventHooks);
					_eventCategories?.Clear();
					_eventCategoriesCompact?.Clear();
					if (HasErrorState(Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup.BACKEND_UNAVAILABLE))
					{
						base.Logger.Warn($"Abort event loading due to error state \"{Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup.BACKEND_UNAVAILABLE}\".");
						SetAreaEventsAsync();
						return;
					}
					Task<List<EventCategory>> defaultEventsTask = LoadDefaultEventsAsync();
					Task<List<EventCategory>> compactEventsTask = LoadCompactEventsAsync();
					await Task.WhenAll<List<EventCategory>>(defaultEventsTask, compactEventsTask);
					List<EventCategory> defaultEvents = defaultEventsTask.Result;
					List<EventCategory> compactEvents = compactEventsTask.Result;
					AssignEventReminderTimes(defaultEvents);
					_eventCategories = defaultEvents;
					_eventCategoriesCompact = compactEvents;
					foreach (Estreya.BlishHUD.EventTable.Models.Event ev in _eventCategories.SelectMany((EventCategory ec) => ec.Events))
					{
						AddEventHooks(ev);
					}
					_lastCheckDrawerSettings = _checkDrawerSettingInterval.TotalMilliseconds;
					await SetAreaEventsAsync();
					base.Logger.Debug("Updated events in all areas.");
					ReportErrorState(Estreya.BlishHUD.EventTable.Models.ModuleErrorStateGroup.LOADING_EVENTS, null);
				}
				catch (FlurlHttpException ex2)
				{
					string message = await ex2.GetResponseStringAsync();
					base.Logger.Warn((Exception)ex2, "Failed loading events: " + message);
					ReportErrorState(Estreya.BlishHUD.EventTable.Models.ModuleErrorStateGroup.LOADING_EVENTS, "Failed loading events: " + message);
				}
				catch (Exception ex)
				{
					base.Logger.Error(ex, "Failed loading events.");
					ReportErrorState(Estreya.BlishHUD.EventTable.Models.ModuleErrorStateGroup.LOADING_EVENTS, "Failed loading events.");
				}
			}
			await (EventTimerHandler?.NotifyUpdatedEvents() ?? Task.CompletedTask);
		}

		private async Task<List<EventCategory>> LoadDefaultEventsAsync()
		{
			base.Logger.Info("Load default events...");
			if (HasErrorState(Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup.BACKEND_UNAVAILABLE))
			{
				return new List<EventCategory>();
			}
			IFlurlRequest request = GetFlurlClient().Request(base.MODULE_API_URL);
			if (!string.IsNullOrWhiteSpace(base.BlishHUDAPIService.AccessToken))
			{
				base.Logger.Info("Include custom events in default...");
				request.WithOAuthBearerToken(base.BlishHUDAPIService.AccessToken);
			}
			List<EventCategory> categories = await request.GetJsonAsync<List<EventCategory>>(default(CancellationToken), (HttpCompletionOption)0);
			int eventCategoryCount = categories.Count;
			int eventCount = categories.Sum((EventCategory ec) => ec.Events.Count);
			base.Logger.Info($"Loaded  {eventCategoryCount} default categories with {eventCount} Events.");
			List<EventCategory> contextEvents = _contextManager?.GetContextCategories();
			if (contextEvents != null && contextEvents.Count > 0)
			{
				base.Logger.Info($"Include {contextEvents.Count} context categories with {contextEvents.Sum((EventCategory ec) => ec.Events?.Count ?? 0)} events in default.");
				categories.AddRange(contextEvents);
			}
			categories.ForEach(delegate(EventCategory ec)
			{
				ec.Load(() => NowUTC, base.TranslationService);
			});
			base.Logger.Debug("Loaded all default event categories.");
			return categories;
		}

		private async Task<List<EventCategory>> LoadCompactEventsAsync()
		{
			base.Logger.Info("Load compact events...");
			if (HasErrorState(Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup.BACKEND_UNAVAILABLE))
			{
				return new List<EventCategory>();
			}
			IFlurlRequest requestCompact = GetFlurlClient().Request(base.MODULE_API_URL, "compact");
			if (!string.IsNullOrWhiteSpace(base.BlishHUDAPIService.AccessToken))
			{
				base.Logger.Info("Include custom events in compact...");
				requestCompact.WithOAuthBearerToken(base.BlishHUDAPIService.AccessToken);
			}
			List<EventCategory> categoriesCompact = await requestCompact.GetJsonAsync<List<EventCategory>>(default(CancellationToken), (HttpCompletionOption)0);
			int eventCategoryCompactCount = categoriesCompact.Count;
			int eventCompactCount = categoriesCompact.Sum((EventCategory ec) => ec.Events.Count);
			base.Logger.Info($"Loaded {eventCategoryCompactCount} compact categories with {eventCompactCount} events.");
			List<EventCategory> contextEvents = _contextManager?.GetContextCategories();
			if (contextEvents != null && contextEvents.Count > 0)
			{
				base.Logger.Info($"Include {contextEvents.Count} context categories with {contextEvents.Sum((EventCategory ec) => ec.Events?.Count ?? 0)} events for compact.");
				categoriesCompact.AddRange(contextEvents);
			}
			categoriesCompact.ForEach(delegate(EventCategory ec)
			{
				ec.Load(() => NowUTC, base.TranslationService);
			});
			base.Logger.Debug("Loaded all compact event categories.");
			return categoriesCompact;
		}

		private void AssignEventReminderTimes(List<EventCategory> categories)
		{
			foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in from ev in categories.SelectMany((EventCategory ec) => ec.Events)
				where !ev.Filler
				select ev)
			{
				if (!base.ModuleSettings.ReminderTimesOverride.get_Value().ContainsKey(ev2.SettingKey))
				{
					ev2.UpdateReminderTimesDefault();
					continue;
				}
				List<Duration> times = base.ModuleSettings.ReminderTimesOverride.get_Value()[ev2.SettingKey].Select((TimeSpan x) => x.ToDuration()).ToList();
				ev2.UpdateReminderTimes(times.ToArray());
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
					if (!area.Value.IsTogglingCompactMode)
					{
						base.ModuleSettings.CheckDrawerSettings(area.Value.Configuration, area.Value.Configuration.CompactMode.get_Value() ? _eventCategoriesCompact : _eventCategories);
					}
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
			DynamicEventHandler?.Update(gameTime);
			EventTimerHandler?.Update(gameTime);
			_contextManager?.Update(gameTime);
			UpdateUtil.Update(CheckDrawerSettings, gameTime, _checkDrawerSettingInterval.TotalMilliseconds, ref _lastCheckDrawerSettings);
			UpdateUtil.UpdateAsync(LoadEventsAsync, gameTime, _updateEventsInterval.TotalMilliseconds, _lastEventUpdate);
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

		private async Task<List<EventCategory>> GetAllEvents()
		{
			using (await _eventCategoryLock.LockAsync())
			{
				return _eventCategories.ToArray().ToList();
			}
		}

		private void AddEventHooks(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			ev.Reminder += Ev_Reminder;
		}

		private void RemoveEventHooks(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			ev.Reminder -= Ev_Reminder;
		}

		private async void Ev_Reminder(object sender, Duration e)
		{
			Estreya.BlishHUD.EventTable.Models.Event ev = sender as Estreya.BlishHUD.EventTable.Models.Event;
			if (!base.ModuleSettings.RemindersEnabled.get_Value() || base.ModuleSettings.ReminderDisabledForEvents.get_Value().Contains(ev.SettingKey))
			{
				return;
			}
			if (!CalculateReminderUIVisibility())
			{
				base.Logger.Debug("Reminder " + ev.SettingKey + " was not displayed due to UI Visibility settings.");
				return;
			}
			if (base.ModuleSettings.DisableRemindersWhenEventFinished.get_Value())
			{
				string areaName = base.ModuleSettings.DisableRemindersWhenEventFinishedArea.get_Value();
				if ((!(areaName == "Any")) ? EventStateService.Contains(areaName, ev.SettingKey) : EventStateService.Contains(ev.SettingKey))
				{
					base.Logger.Debug("Reminder " + ev.SettingKey + " was not displayed due to being completed/hidden in the area \"" + areaName + "\".");
					return;
				}
			}
			try
			{
				string translation = base.TranslationService.GetTranslation("reminder-startsIn", "Starts in");
				string title = ev.Name;
				string message = translation + " " + e.ToTimeSpan().Humanize(6, null, TimeUnit.Week, base.ModuleSettings.ReminderMinTimeUnit.get_Value()) + "!";
				AsyncTexture2D icon = (AsyncTexture2D)(string.IsNullOrWhiteSpace(ev.Icon) ? ((object)new AsyncTexture2D()) : ((object)base.IconService.GetIcon(ev.Icon)));
				ReminderType value = base.ModuleSettings.ReminderType.get_Value();
				if ((value == ReminderType.Control || value == ReminderType.Both) ? true : false)
				{
					EventNotification eventNotification = EventNotification.ShowAsControl(ev, title, message, icon, base.IconService, base.ModuleSettings);
					((Control)eventNotification).add_Click((EventHandler<MouseEventArgs>)EventNotification_Click);
					((Control)eventNotification).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)EventNotification_RightMouseButtonPressed);
					((Control)eventNotification).add_Disposed((EventHandler<EventArgs>)EventNotification_Disposed);
				}
				value = base.ModuleSettings.ReminderType.get_Value();
				if ((value == ReminderType.Windows || value == ReminderType.Both) ? true : false)
				{
					await EventNotification.ShowAsWindowsNotification(title, message, icon);
				}
				await EventNotification.PlaySound(base.AudioService, ev);
			}
			catch (Exception ex)
			{
				base.Logger.Warn(ex, "Failed to show reminder for event \"" + ev.SettingKey + "\"");
			}
		}

		private void EventNotification_Disposed(object sender, EventArgs e)
		{
			EventNotification obj = sender as EventNotification;
			((Control)obj).remove_Click((EventHandler<MouseEventArgs>)EventNotification_Click);
			((Control)obj).remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)EventNotification_RightMouseButtonPressed);
			((Control)obj).remove_Disposed((EventHandler<EventArgs>)EventNotification_Disposed);
		}

		private async void EventNotification_Click(object sender, MouseEventArgs e)
		{
			try
			{
				Estreya.BlishHUD.EventTable.Models.Event model = (sender as EventNotification)?.Model;
				string waypoint = model?.GetWaypoint(base.AccountService?.Account);
				switch (base.ModuleSettings.ReminderLeftClickAction.get_Value())
				{
				case LeftClickAction.CopyWaypoint:
				{
					if (model == null || string.IsNullOrWhiteSpace(waypoint))
					{
						break;
					}
					string eventChatFormat = model.GetChatText(base.ModuleSettings.ReminderEventChatFormat.get_Value(), model.GetNextOccurrence(), base.AccountService?.Account);
					if ((int)GameService.Input.get_Keyboard().get_ActiveModifiers() == 1)
					{
						try
						{
							await base.ChatService.ChangeChannel(ChatChannel.Squad);
							await base.ChatService.ChangeChannel(base.ModuleSettings.ReminderWaypointSendingChannel.get_Value(), base.ModuleSettings.ReminderWaypointSendingGuild.get_Value(), GameService.Gw2Mumble.get_PlayerCharacter().get_Name());
							await base.ChatService.Send(eventChatFormat);
						}
						catch (Exception ex2)
						{
							base.Logger.Warn(ex2, "Could not paste waypoint into chat. Event: " + model.SettingKey);
							ScreenNotification.ShowNotification(new string[2] { "Waypoint could not be pasted in chat.", "See log for more information." }, ScreenNotification.NotificationType.Error, null, 5);
						}
					}
					else
					{
						await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(eventChatFormat);
						ScreenNotification.ShowNotification(new string[2] { model.Name, "Copied to clipboard!" });
					}
					break;
				}
				case LeftClickAction.NavigateToWaypoint:
				{
					if (string.IsNullOrWhiteSpace(waypoint) || base.PointOfInterestService == null)
					{
						break;
					}
					if (base.PointOfInterestService.Loading)
					{
						ScreenNotification.ShowNotification("PointOfInterestService is still loading!", ScreenNotification.NotificationType.Error);
						break;
					}
					PointOfInterest poi = base.PointOfInterestService.GetPointOfInterest(waypoint);
					if (poi == null)
					{
						ScreenNotification.ShowNotification(waypoint + " not found!", ScreenNotification.NotificationType.Error);
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
			catch (Exception ex)
			{
				base.Logger.Warn(ex, "Could not handle reminder left click.");
			}
		}

		private void EventNotification_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			try
			{
				EventNotification notification = sender as EventNotification;
				if (base.ModuleSettings.ReminderRightClickAction.get_Value() == EventReminderRightClickAction.Dismiss && notification != null)
				{
					((Control)notification).Dispose();
				}
			}
			catch (Exception ex)
			{
				base.Logger.Warn(ex, "Could not handle reminder right click.");
			}
		}

		private void AddAllAreas()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			if (base.ModuleSettings.EventAreaNames.get_Value().Count == 0)
			{
				AddArea("Main", new KeyBinding((ModifierKeys)2, (Keys)69));
				return;
			}
			foreach (string areaName in base.ModuleSettings.EventAreaNames.get_Value())
			{
				AddArea(areaName);
			}
		}

		private EventAreaConfiguration AddArea(string name, KeyBinding enabledKeybinding = null)
		{
			EventAreaConfiguration config = base.ModuleSettings.AddDrawer(name, _eventCategories, enabledKeybinding);
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
			EventArea eventArea = new EventArea(configuration, base.IconService, base.TranslationService, EventStateService, base.WorldbossService, base.MapchestService, base.PointOfInterestService, base.AccountService, base.ChatService, MapUtil, GetFlurlClient(), () => GetJsonSerializer(), base.MODULE_API_URL, () => NowUTC, () => ((Module)this).get_Version(), () => base.BlishHUDAPIService.AccessToken, () => base.ModuleSettings.EventAreaNames.get_Value().ToArray().ToList(), () => base.ModuleSettings.ReminderDisabledForEvents.get_Value().ToArray().ToList(), base.ContentsManager);
			((Control)eventArea).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			EventArea area = eventArea;
			area.CopyToAreaClicked += new EventHandler<(string, string)>(EventArea_CopyToAreaClicked);
			area.MoveToAreaClicked += new EventHandler<(string, string)>(EventArea_MoveToAreaClicked);
			area.EnableReminderClicked += EventArea_EnableReminderClicked;
			area.DisableReminderClicked += EventArea_DisableReminderClicked;
			area.CompactModeToggled += EventArea_CompactModeToggled;
			((Control)area).add_Disposed((EventHandler<EventArgs>)EventArea_Disposed);
			_areas.AddOrUpdate(configuration.Name, area, (string name, EventArea prev) => area);
		}

		private async Task EventArea_CompactModeToggled(object sender)
		{
			EventArea sourceArea = sender as EventArea;
			sourceArea.IsTogglingCompactMode = true;
			try
			{
				sourceArea.Configuration.DisabledCompletionActionForEvents.get_Value().Clear();
				sourceArea.Configuration.DisabledEventKeys.get_Value().Clear();
				sourceArea.Configuration.EventOrder.get_Value().Clear();
				GameService.Settings.Save(false);
				await SetAreaEventsAsync(sourceArea);
			}
			finally
			{
				sourceArea.IsTogglingCompactMode = false;
			}
			_lastCheckDrawerSettings = _checkDrawerSettingInterval.TotalMilliseconds;
		}

		private void EventArea_DisableReminderClicked(object sender, string e)
		{
			base.ModuleSettings.ReminderDisabledForEvents.set_Value(new List<string>(base.ModuleSettings.ReminderDisabledForEvents.get_Value()) { e });
		}

		private void EventArea_EnableReminderClicked(object sender, string e)
		{
			base.ModuleSettings.ReminderDisabledForEvents.set_Value(new List<string>(from k in base.ModuleSettings.ReminderDisabledForEvents.get_Value()
				where k != e
				select k));
		}

		private void EventArea_MoveToAreaClicked(object sender, (string EventSettingKey, string DestinationArea) e)
		{
			EventArea sourceArea = sender as EventArea;
			EventArea value = _areas.First((KeyValuePair<string, EventArea> a) => a.Key == e.DestinationArea).Value;
			sourceArea.DisableEvent(e.EventSettingKey);
			value.EnableEvent(e.EventSettingKey);
		}

		private void EventArea_CopyToAreaClicked(object sender, (string EventSettingKey, string DestinationArea) e)
		{
			_areas.First((KeyValuePair<string, EventArea> a) => a.Key == e.DestinationArea).Value.EnableEvent(e.EventSettingKey);
		}

		private void EventArea_Disposed(object sender, EventArgs e)
		{
			EventArea obj = sender as EventArea;
			obj.CopyToAreaClicked -= new EventHandler<(string, string)>(EventArea_CopyToAreaClicked);
			obj.MoveToAreaClicked -= new EventHandler<(string, string)>(EventArea_MoveToAreaClicked);
			obj.EnableReminderClicked -= EventArea_EnableReminderClicked;
			obj.DisableReminderClicked -= EventArea_DisableReminderClicked;
			obj.CompactModeToggled -= EventArea_CompactModeToggled;
			((Control)obj).remove_Disposed((EventHandler<EventArgs>)EventArea_Disposed);
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
			return new ModuleSettings(settings, ((Module)this).get_Version(), () => NowUTC);
		}

		protected override void OnSettingWindowBuild(TabbedWindow settingWindow)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Expected O, but got Unknown
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Expected O, but got Unknown
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Expected O, but got Unknown
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Expected O, but got Unknown
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Expected O, but got Unknown
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Expected O, but got Unknown
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Expected O, but got Unknown
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Expected O, but got Unknown
			settingWindow.SavesSize = true;
			settingWindow.CanResize = true;
			settingWindow.RebuildViewAfterResize = true;
			settingWindow.UnloadOnRebuild = false;
			settingWindow.MinSize = settingWindow.Size;
			settingWindow.MaxSize = new Point(((Control)settingWindow).get_Width() * 2, ((Control)settingWindow).get_Height() * 3);
			settingWindow.RebuildDelay = 500;
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156736.png"), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(base.ModuleSettings, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.MetricsService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("generalSettingsView-title", "General"), (int?)null));
			AreaSettingsView areaSettingsView = new AreaSettingsView(() => _areas.Values.Select((EventArea area) => area.Configuration), (EventAreaConfiguration config) => config.CompactMode.get_Value() ? _eventCategoriesCompact : _eventCategories, base.ModuleSettings, base.AccountService, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, EventStateService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			};
			areaSettingsView.AddArea += delegate(object s, AreaSettingsView.AddAreaEventArgs e)
			{
				e.AreaConfiguration = AddArea(e.Name);
				if (e.AreaConfiguration != null)
				{
					EventArea areaEventsAsync = _areas.Values.Where((EventArea x) => x.Configuration.Name == e.Name).First();
					SetAreaEventsAsync(areaEventsAsync);
				}
			};
			areaSettingsView.RemoveArea += delegate(object s, EventAreaConfiguration e)
			{
				RemoveArea(e);
			};
			areaSettingsView.SyncEnabledEventsToReminders += delegate(object s, EventAreaConfiguration e)
			{
				base.ModuleSettings.ReminderDisabledForEvents.set_Value(new List<string>(e.DisabledEventKeys.get_Value()));
				return Task.CompletedTask;
			};
			areaSettingsView.SyncEnabledEventsFromReminders += delegate(object s, EventAreaConfiguration e)
			{
				e.DisabledEventKeys.set_Value(new List<string>(base.ModuleSettings.ReminderDisabledForEvents.get_Value()));
				return Task.CompletedTask;
			};
			areaSettingsView.SyncEnabledEventsToOtherAreas += delegate(object s, EventAreaConfiguration e)
			{
				if (_areas == null)
				{
					throw new ArgumentNullException("_areas", "Areas are not available.");
				}
				foreach (EventArea current in _areas.Values)
				{
					if (!(current.Configuration.Name == e.Name) && !current.Configuration.CompactMode.get_Value())
					{
						current.Configuration.DisabledEventKeys.set_Value(new List<string>(e.DisabledEventKeys.get_Value()));
					}
				}
				return Task.CompletedTask;
			};
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("605018.png"), (Func<IView>)(() => (IView)(object)areaSettingsView), base.TranslationService.GetTranslation("areaSettingsView-title", "Event Areas"), (int?)null));
			string upcomingEventsFeatureName = "event_table-upcoming_events";
			bool upcomingEventsFeatureEnabled = AsyncHelper.RunSync(async () => await IsFeatureEnabledAsync(upcomingEventsFeatureName));
			base.Logger.Info($"Feature flag \"{upcomingEventsFeatureName}\" is enabled: {upcomingEventsFeatureEnabled}");
			if (upcomingEventsFeatureEnabled)
			{
				UpcomingEventsView upcomingEventsView = new UpcomingEventsView(() => _eventCategories, () => NowUTC, base.ModuleSettings, isExternal: false, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.AccountService);
				upcomingEventsView.OpenExternallyClicked += UpcomingEventsViewOnOpenExternallyClicked;
				base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156328.png"), (Func<IView>)(() => (IView)(object)upcomingEventsView), base.TranslationService.GetTranslation("upcomingEventsView-title", "Upcoming Events"), (int?)null));
			}
			ReminderSettingsView reminderSettingsView = new ReminderSettingsView(base.ModuleSettings, () => _eventCategories, () => _areas.Keys.ToList(), base.AccountService, base.AudioService, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			};
			reminderSettingsView.SyncEnabledEventsToAreas += delegate
			{
				if (_areas == null)
				{
					throw new ArgumentNullException("_areas", "Areas are not available.");
				}
				foreach (EventArea value in _areas.Values)
				{
					value.Configuration.DisabledEventKeys.set_Value(new List<string>(base.ModuleSettings.ReminderDisabledForEvents.get_Value()));
				}
				return Task.CompletedTask;
			};
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("1466345.png"), (Func<IView>)(() => (IView)(object)reminderSettingsView), base.TranslationService.GetTranslation("reminderSettingsView-title", "Reminders"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("759448.png"), (Func<IView>)(() => (IView)(object)new DynamicEventsSettingsView(DynamicEventService, base.ModuleSettings, GetFlurlClient(), base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("dynamicEventsSettingsView-title", "Dynamic Events"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("3126786.png"), (Func<IView>)(() => (IView)(object)new EventTimersSettingsView(base.ModuleSettings, GetAllEvents, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.AccountService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("eventTimersSettingsView-title", "Event Timers"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156680.png"), (Func<IView>)(() => (IView)(object)new SelfHostingEventsView(base.ModuleSettings, SelfHostingEventService, base.Gw2ApiManager, base.IconService, base.TranslationService, base.AccountService, base.ChatService, () => NowUTC)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("selfHostingEventsSettingsView-title", "Self Hosting Events"), (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156764.png"), (Func<IView>)(() => (IView)(object)new EventTableBlishHUDAPIView(base.Gw2ApiManager, base.IconService, base.TranslationService, base.BlishHUDAPIService, GetFlurlClient())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Estreya BlishHUD", (int?)null));
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("157097.png"), (Func<IView>)(() => (IView)(object)new HelpView(() => _eventCategories, base.MODULE_API_URL, base.Gw2ApiManager, base.IconService, base.TranslationService)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), base.TranslationService.GetTranslation("helpView-title", "Help"), (int?)null));
		}

		private void UpcomingEventsViewOnOpenExternallyClicked(object sender, EventArgs e)
		{
			ShowEventTimesTable(showAndDontToggle: true);
		}

		private async Task ShowEventTimesTable(bool showAndDontToggle)
		{
			string upcomingEventsFeatureName = "event_table-upcoming_events";
			bool upcomingEventsFeatureEnabled = await IsFeatureEnabledAsync(upcomingEventsFeatureName);
			base.Logger.Info($"Feature flag \"{upcomingEventsFeatureName}\" is enabled: {upcomingEventsFeatureEnabled}");
			if (!upcomingEventsFeatureEnabled)
			{
				return;
			}
			if (_eventTimesTableWindow == null)
			{
				_eventTimesTableWindow = WindowUtil.CreateStandardWindow(base.ModuleSettings, "Event Times", ((object)this).GetType(), Guid.Parse("7dd51c83-67aa-4cfb-9fe3-a16a8b30892d"), base.IconService);
				_eventTimesTableWindow.SavesSize = true;
				_eventTimesTableWindow.CanResize = true;
				_eventTimesTableWindow.CanCloseWithEscape = false;
				_eventTimesTableWindow.RebuildViewAfterResize = true;
				_eventTimesTableWindow.UnloadOnRebuild = false;
				_eventTimesTableWindow.MinSize = _eventTimesTableWindow.Size;
				_eventTimesTableWindow.MaxSize = new Point(((Control)_eventTimesTableWindow).get_Width() * 2, ((Control)_eventTimesTableWindow).get_Height() * 3);
			}
			_eventTimesTableWindow.CanCloseWithEscape = base.ModuleSettings.CloseEventTimeTableWithEsc.get_Value();
			_ = _eventTimesTableWindow.CurrentView;
			if (showAndDontToggle || !((Control)_eventTimesTableWindow).get_Visible())
			{
				UpcomingEventsView view = new UpcomingEventsView(() => _eventCategories, () => NowUTC, base.ModuleSettings, isExternal: true, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.AccountService);
				await _eventTimesTableWindow.Show((IView)(object)view);
			}
			else
			{
				((Control)_eventTimesTableWindow).Hide();
			}
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
			configurations.Audio.Enabled = true;
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
			}, directoryPath, () => NowUTC, () => GetJsonSerializer());
			DynamicEventService = new DynamicEventService(new APIServiceConfiguration
			{
				AwaitLoading = false,
				Enabled = true,
				SaveInterval = Timeout.InfiniteTimeSpan
			}, base.Gw2ApiManager, GetFlurlClient(), base.MODULE_API_URL, directoryPath);
			SelfHostingEventService = new SelfHostingEventService(new APIServiceConfiguration
			{
				AwaitLoading = true,
				Enabled = true,
				SaveInterval = Timeout.InfiniteTimeSpan,
				UpdateInterval = TimeSpan.FromMinutes(5.0)
			}, base.Gw2ApiManager, GetFlurlClient(), base.MODULE_API_URL, base.AccountService, base.BlishHUDAPIService);
			collection.Add(EventStateService);
			collection.Add(DynamicEventService);
			collection.Add(SelfHostingEventService);
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

		protected override AsyncTexture2D GetErrorCornerIcon()
		{
			return base.IconService.GetIcon("textures/event_boss_grey_error.png");
		}

		protected override List<WizardView> GetWizardViews()
		{
			return new List<WizardView>
			{
				new WizardWelcomeView(base.Gw2ApiManager, base.IconService, base.TranslationService),
				new WizardAreasView(_areas.Values.Select((EventArea area) => area.Configuration).ToList(), base.Gw2ApiManager, base.IconService, base.TranslationService),
				new WizardRemindersView(base.ModuleSettings, base.AudioService, base.Gw2ApiManager, base.IconService, base.TranslationService)
			};
		}

		private async Task PerformSanityCheck()
		{
			List<string> reminderFunctionBlockers = await GetReminderFunctionBlockers();
			if (reminderFunctionBlockers.Count > 0)
			{
				base.Logger.Warn("Found possible reminder function blockers: {blockers}", new object[1] { string.Join(", ", reminderFunctionBlockers) });
			}
			foreach (EventAreaConfiguration eventAreaConfiguration in _areas.Values.Select((EventArea x) => x.Configuration))
			{
				List<string> blockers = await GetEventAreaFunctionBlockers(eventAreaConfiguration);
				if (blockers.Count > 0)
				{
					base.Logger.Warn("Found possible function blockers for event area {area}: {blockers}", new object[2]
					{
						eventAreaConfiguration.Name,
						string.Join(", ", blockers)
					});
				}
			}
		}

		private Task<List<string>> GetEventAreaFunctionBlockers(EventAreaConfiguration areaConfiguration)
		{
			List<string> blockers2 = new List<string>();
			if (!areaConfiguration.Enabled.get_Value())
			{
				AddSureBlocker(blockers2, "not-enabled");
			}
			if (areaConfiguration.Size.X.get_Value() <= 100)
			{
				AddSureBlocker(blockers2, "unrealistic-small-width");
			}
			if (areaConfiguration.DisabledEventKeys.get_Value().Count > 0)
			{
				AddPossibleBlocker(blockers2, "disabled-for-some-events");
			}
			if (areaConfiguration.EventHeight.get_Value() <= 5)
			{
				AddPossibleBlocker(blockers2, "unrealistic-small-event-height");
			}
			if (areaConfiguration.LimitMapType.get_Value() != 0)
			{
				AddPossibleBlocker(blockers2, "limit-map-type");
			}
			if (areaConfiguration.HideOnMissingMumbleTicks.get_Value())
			{
				AddPossibleBlocker(blockers2, "hide-on-missing-mumble-ticks");
			}
			if (areaConfiguration.HideInPvE_OpenWorld.get_Value())
			{
				AddPossibleBlocker(blockers2, "hide-in-pve-open-world");
			}
			if (areaConfiguration.TimeSpan.get_Value() <= 10)
			{
				AddPossibleBlocker(blockers2, "unrealistic-small-timespan");
			}
			return Task.FromResult(blockers2);
			static void AddBlocker(List<string> blockers, string prefix, string blocker)
			{
				blockers.Add(prefix + ":" + blocker);
			}
			static void AddPossibleBlocker(List<string> blockers, string blocker)
			{
				AddBlocker(blockers, "possible", blocker);
			}
			static void AddSureBlocker(List<string> blockers, string blocker)
			{
				AddBlocker(blockers, "sure", blocker);
			}
		}

		private Task<List<string>> GetReminderFunctionBlockers()
		{
			List<string> blockers2 = new List<string>();
			if (!base.ModuleSettings.RemindersEnabled.get_Value())
			{
				AddSureBlocker(blockers2, "not-enabled");
			}
			if (base.ModuleSettings.ReminderDisabledForEvents.get_Value().Count > 0)
			{
				AddPossibleBlocker(blockers2, "disabled-for-some-events");
			}
			if (base.ModuleSettings.ReminderType.get_Value() == ReminderType.Windows)
			{
				AddPossibleBlocker(blockers2, "type-windows");
			}
			if (base.ModuleSettings.DisableRemindersWhenEventFinished.get_Value())
			{
				AddPossibleBlocker(blockers2, "disabled-when-event-finished-from-" + base.ModuleSettings.DisableRemindersWhenEventFinishedArea.get_Value());
			}
			if (base.ModuleSettings.HideRemindersOnMissingMumbleTicks.get_Value())
			{
				AddPossibleBlocker(blockers2, "hide-on-missing-mumble-ticks");
			}
			if (base.ModuleSettings.HideRemindersInPvE_OpenWorld.get_Value())
			{
				AddPossibleBlocker(blockers2, "hide-in-pve-open-world");
			}
			return Task.FromResult(blockers2);
			static void AddBlocker(List<string> blockers, string prefix, string blocker)
			{
				blockers.Add(prefix + ":" + blocker);
			}
			static void AddPossibleBlocker(List<string> blockers, string blocker)
			{
				AddBlocker(blockers, "possible", blocker);
			}
			static void AddSureBlocker(List<string> blockers, string blocker)
			{
				AddBlocker(blockers, "sure", blocker);
			}
		}

		private void UnloadContext()
		{
			_eventTableContextHandle?.Expire();
			base.Logger.Info("Event Table context expired.");
			if (_contextManager != null)
			{
				_contextManager.Dispose();
				_contextManager.ReloadEvents -= ContextManager_ReloadEvents;
				_contextManager = null;
			}
			_eventTableContext = null;
			_eventTableContextHandle = null;
		}

		protected override void Unload()
		{
			base.Logger.Debug("Unload module.");
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
			if (DynamicEventHandler != null)
			{
				DynamicEventHandler.FoundLostEntities -= DynamicEventHandler_FoundLostEntities;
				DynamicEventHandler.Dispose();
				DynamicEventHandler = null;
			}
			if (EventTimerHandler != null)
			{
				EventTimerHandler.FoundLostEntities -= EventTimerHandler_FoundLostEntities;
				EventTimerHandler.Dispose();
				EventTimerHandler = null;
			}
			if (base.BlishHUDAPIService != null)
			{
				base.BlishHUDAPIService.NewLogin -= BlishHUDAPIService_NewLogin;
				base.BlishHUDAPIService.LoggedOut -= BlishHUDAPIService_LoggedOut;
			}
			base.ModuleSettings.ShowEventTimeTableWindowKeybinding.get_Value().remove_Activated((EventHandler<EventArgs>)OnShowEventTimeTableWindowKeybindingActivated);
			base.Logger.Debug("Unloaded events.");
			UnloadContext();
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
			base.Logger.Debug("Unload base.");
			base.Unload();
			base.Logger.Debug("Unloaded base.");
		}
	}
}
