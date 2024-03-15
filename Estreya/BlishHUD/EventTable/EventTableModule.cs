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
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models.GW2API.PointOfInterest;
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

		private readonly AsyncLock _eventCategoryLock = new AsyncLock();

		private double _lastCheckDrawerSettings;

		private AsyncRef<double> _lastEventUpdate;

		private EventTableContext _eventTableContext;

		private ContextManager _contextManager;

		private ContextHandle<EventTableContext> _eventTableContextHandle;

		private BitmapFont _defaultFont;

		protected override string UrlModuleName => "event-table";

		protected override bool NeedsBackend => true;

		protected override bool EnableMetrics => true;

		private DateTime NowUTC => DateTime.UtcNow;

		private MapUtil MapUtil { get; set; }

		private DynamicEventHandler DynamicEventHandler { get; set; }

		protected override string API_VERSION_NO => "1";

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
			base.BlishHUDAPIService.NewLogin += BlishHUDAPIService_NewLogin;
			base.BlishHUDAPIService.RefreshedLogin += BlishHUDAPIService_RefreshedLogin;
			base.BlishHUDAPIService.LoggedOut += BlishHUDAPIService_LoggedOut;
			MapUtil = new MapUtil(base.ModuleSettings.MapKeybinding.get_Value(), base.Gw2ApiManager);
			DynamicEventHandler = new DynamicEventHandler(MapUtil, DynamicEventService, base.Gw2ApiManager, base.ModuleSettings);
			DynamicEventHandler.FoundLostEntities += DynamicEventHandler_FoundLostEntities;
			await DynamicEventHandler.AddDynamicEventsToMap();
			await DynamicEventHandler.AddDynamicEventsToWorld();
			base.ModuleSettings.IncludeSelfHostedEvents.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)IncludeSelfHostedEvents_SettingChanged);
			AddAllAreas();
			await LoadEvents();
			sw.Stop();
			base.Logger.Debug("Loaded in " + sw.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms");
		}

		private async void IncludeSelfHostedEvents_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			await ReloadEvents();
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
			_contextManager = new ContextManager(_eventTableContext, base.ModuleSettings, DynamicEventService, base.IconService, EventStateService, async delegate
			{
				using (await _eventCategoryLock.LockAsync())
				{
					return _eventCategories.SelectMany((EventCategory ec) => ec.Events);
				}
			});
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

		private async Task LoadEvents()
		{
			base.Logger.Info("Load events...");
			using (await _eventCategoryLock.LockAsync())
			{
				base.Logger.Debug("Acquired lock.");
				try
				{
					_eventCategories?.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(RemoveEventHooks);
					_eventCategories?.Clear();
					if (HasErrorState(Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup.BACKEND_UNAVAILABLE))
					{
						base.Logger.Warn($"Abort event loading due to error state \"{Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup.BACKEND_UNAVAILABLE}\".");
						SetAreaEvents();
						return;
					}
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
					List<EventCategory> contextEvents = _contextManager?.GetContextCategories();
					if (contextEvents != null && contextEvents.Count > 0)
					{
						base.Logger.Info($"Include {contextEvents.Count} context categories with {contextEvents.Sum((EventCategory ec) => ec.Events?.Count ?? 0)} events.");
						categories.AddRange(contextEvents);
					}
					if (base.ModuleSettings.IncludeSelfHostedEvents.get_Value())
					{
						Dictionary<string, List<SelfHostedEventEntry>> selfHostedEvents = await LoadSelfHostedEvents();
						if (selfHostedEvents != null)
						{
							foreach (KeyValuePair<string, List<SelfHostedEventEntry>> selfHostedCategory in selfHostedEvents)
							{
								if (!categories.Any((EventCategory c) => c.Key == selfHostedCategory.Key))
								{
									continue;
								}
								EventCategory category = categories.Find((EventCategory c) => c.Key == selfHostedCategory.Key);
								foreach (SelfHostedEventEntry selfHostedEvent in selfHostedCategory.Value)
								{
									Estreya.BlishHUD.EventTable.Models.Event ev2 = new Estreya.BlishHUD.EventTable.Models.Event
									{
										Key = selfHostedEvent.EventKey,
										Name = (selfHostedEvent.EventName ?? selfHostedEvent.EventKey),
										Duration = selfHostedEvent.Duration,
										HostedBySystem = false
									};
									ev2.Occurences.Add(selfHostedEvent.StartTime.UtcDateTime);
									category.OriginalEvents.Add(ev2);
								}
							}
						}
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
		}

		private async Task<Dictionary<string, List<SelfHostedEventEntry>>> LoadSelfHostedEvents()
		{
			try
			{
				Dictionary<string, List<SelfHostedEventEntry>> obj = await GetFlurlClient().Request(base.MODULE_API_URL, "self-hosting").GetJsonAsync<Dictionary<string, List<SelfHostedEventEntry>>>(default(CancellationToken), (HttpCompletionOption)0);
				int eventCategoryCount = obj.Count;
				int eventCount = obj.Sum((KeyValuePair<string, List<SelfHostedEventEntry>> ec) => ec.Value.Count);
				base.Logger.Info($"Loaded {eventCategoryCount} self hosted categories with {eventCount} events.");
				return obj;
			}
			catch (FlurlHttpException ex2)
			{
				string message = await ex2.GetResponseStringAsync();
				base.Logger.Warn((Exception)ex2, "Failed loading self hosted events: " + message);
			}
			catch (Exception ex)
			{
				base.Logger.Warn(ex, "Failed loading self hosted events.");
			}
			return null;
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
			DynamicEventHandler?.Update(gameTime);
			_contextManager?.Update(gameTime);
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

		private async void Ev_Reminder(object sender, TimeSpan e)
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
				string message = translation + " " + e.Humanize(6, null, TimeUnit.Week, base.ModuleSettings.ReminderMinTimeUnit.get_Value()) + "!";
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
				base.AudioService.PlaySoundFromFile("reminder", silent: true);
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

		private void EventNotification_Click(object sender, MouseEventArgs e)
		{
			EventNotification notification = sender as EventNotification;
			string waypoint = notification?.Model?.GetWaypoint(base.AccountService.Account);
			switch (base.ModuleSettings.ReminderLeftClickAction.get_Value())
			{
			case LeftClickAction.CopyWaypoint:
				if (notification != null && notification.Model != null && !string.IsNullOrWhiteSpace(waypoint))
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypoint);
					ScreenNotification.ShowNotification(new string[2]
					{
						notification.Model.Name,
						"Copied to clipboard!"
					});
				}
				break;
			case LeftClickAction.NavigateToWaypoint:
			{
				if (notification == null || notification.Model == null || string.IsNullOrWhiteSpace(waypoint) || base.PointOfInterestService == null)
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

		private void EventNotification_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
			EventNotification notification = sender as EventNotification;
			if (base.ModuleSettings.ReminderRightClickAction.get_Value() == EventReminderRightClickAction.Dismiss && notification != null)
			{
				((Control)notification).Dispose();
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
			EventArea eventArea = new EventArea(configuration, base.IconService, base.TranslationService, EventStateService, base.WorldbossService, base.MapchestService, base.PointOfInterestService, base.AccountService, MapUtil, GetFlurlClient(), base.MODULE_API_URL, () => NowUTC, () => ((Module)this).get_Version(), () => base.BlishHUDAPIService.AccessToken, () => base.ModuleSettings.EventAreaNames.get_Value().ToArray().ToList(), () => base.ModuleSettings.ReminderDisabledForEvents.get_Value().ToArray().ToList(), base.ContentsManager);
			((Control)eventArea).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			EventArea area = eventArea;
			area.CopyToAreaClicked += new EventHandler<(string, string)>(EventArea_CopyToAreaClicked);
			area.MoveToAreaClicked += new EventHandler<(string, string)>(EventArea_MoveToAreaClicked);
			area.EnableReminderClicked += EventArea_EnableReminderClicked;
			area.DisableReminderClicked += EventArea_DisableReminderClicked;
			((Control)area).add_Disposed((EventHandler<EventArgs>)EventArea_Disposed);
			_areas.AddOrUpdate(configuration.Name, area, (string name, EventArea prev) => area);
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
			return new ModuleSettings(settings);
		}

		protected override void OnSettingWindowBuild(TabbedWindow settingWindow)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Expected O, but got Unknown
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Expected O, but got Unknown
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Expected O, but got Unknown
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Expected O, but got Unknown
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
			AreaSettingsView areaSettingsView = new AreaSettingsView(() => _areas.Values.Select((EventArea area) => area.Configuration), () => _eventCategories, base.ModuleSettings, base.AccountService, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, EventStateService)
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
					if (!(current.Configuration.Name == e.Name))
					{
						current.Configuration.DisabledEventKeys.set_Value(new List<string>(e.DisabledEventKeys.get_Value()));
					}
				}
				return Task.CompletedTask;
			};
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("605018.png"), (Func<IView>)(() => (IView)(object)areaSettingsView), base.TranslationService.GetTranslation("areaSettingsView-title", "Event Areas"), (int?)null));
			ReminderSettingsView reminderSettingsView = new ReminderSettingsView(base.ModuleSettings, () => _eventCategories, () => _areas.Keys.ToList(), base.AccountService, base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService)
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
			base.SettingsWindow.Tabs.Add(new Tab(base.IconService.GetIcon("156764.png"), (Func<IView>)(() => (IView)(object)new BlishHUDAPIView(base.Gw2ApiManager, base.IconService, base.TranslationService, base.BlishHUDAPIService, GetFlurlClient())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Estreya BlishHUD API", (int?)null));
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
			}, directoryPath, () => NowUTC);
			DynamicEventService = new DynamicEventService(new APIServiceConfiguration
			{
				AwaitLoading = false,
				Enabled = true,
				SaveInterval = Timeout.InfiniteTimeSpan
			}, base.Gw2ApiManager, GetFlurlClient(), base.API_ROOT_URL, directoryPath);
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

		protected override AsyncTexture2D GetErrorCornerIcon()
		{
			return base.IconService.GetIcon("textures/event_boss_grey_error.png");
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
			if (base.BlishHUDAPIService != null)
			{
				base.BlishHUDAPIService.NewLogin -= BlishHUDAPIService_NewLogin;
				base.BlishHUDAPIService.LoggedOut -= BlishHUDAPIService_LoggedOut;
			}
			base.ModuleSettings.IncludeSelfHostedEvents.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)IncludeSelfHostedEvents_SettingChanged);
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
