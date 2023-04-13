using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
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
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.MumbleInfo.Map;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.Models;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : BaseModule<EventTableModule, ModuleSettings>
	{
		private ConcurrentDictionary<string, EventArea> _areas = new ConcurrentDictionary<string, EventArea>();

		private AsyncLock _eventCategoryLock = new AsyncLock();

		private List<EventCategory> _eventCategories = new List<EventCategory>();

		private static TimeSpan _updateEventsInterval = TimeSpan.FromMinutes(30.0);

		private AsyncRef<double> _lastEventUpdate = new AsyncRef<double>(0.0);

		private static TimeSpan _checkDrawerSettingInterval = TimeSpan.FromSeconds(30.0);

		private double _lastCheckDrawerSettings;

		public override string WebsiteModuleName => "event-table";

		private DateTime NowUTC => DateTime.UtcNow;

		public EventState EventState { get; private set; }

		public DynamicEventState DynamicEventState { get; private set; }

		private MapUtil MapUtil { get; set; }

		private DynamicEventHandler DynamicEventHandler { get; set; }

		protected override string API_VERSION_NO => "1";

		[ImportingConstructor]
		public EventTableModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			base.BlishHUDAPIState.NewLogin += BlishHUDAPIState_NewLogin;
			base.BlishHUDAPIState.LoggedOut += BlishHUDAPIState_LoggedOut;
			MapUtil = new MapUtil(base.ModuleSettings.MapKeybinding.get_Value(), base.Gw2ApiManager);
			DynamicEventHandler = new DynamicEventHandler(MapUtil, DynamicEventState, base.Gw2ApiManager, base.ModuleSettings);
			await DynamicEventHandler.AddDynamicEventsToMap();
			await DynamicEventHandler.AddDynamicEventsToWorld();
			await LoadEvents();
			AddAllAreas();
			SetAreaEvents();
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if ((int)e.get_EventType() == 256 && !GameService.Input.get_Keyboard().TextFieldIsActive())
			{
				e.get_Key();
				_ = 85;
			}
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

		public async Task LoadEvents()
		{
			base.Logger.Info("Load events...");
			using (await _eventCategoryLock.LockAsync())
			{
				base.Logger.Debug("Acquired lock.");
				try
				{
					_eventCategories?.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
					{
						RemoveEventHooks(ev);
					});
					_eventCategories?.Clear();
					IFlurlRequest request = GetFlurlClient().Request(base.API_URL, "events");
					if (!string.IsNullOrWhiteSpace(base.BlishHUDAPIState.AccessToken))
					{
						base.Logger.Info("Include custom events...");
						request.WithOAuthBearerToken(base.BlishHUDAPIState.AccessToken);
					}
					List<EventCategory> categories = await request.GetJsonAsync<List<EventCategory>>(default(CancellationToken), (HttpCompletionOption)0);
					int eventCategoryCount = categories.Count;
					int eventCount = categories.Sum((EventCategory ec) => ec.Events.Count);
					base.Logger.Info($"Loaded {eventCategoryCount} Categories with {eventCount} Events.");
					categories.ForEach(delegate(EventCategory ec)
					{
						ec.Load(() => NowUTC, base.TranslationState);
					});
					base.Logger.Debug("Loaded all event categories.");
					AssignEventReminderTimes(categories);
					_eventCategories = categories;
					foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in _eventCategories.SelectMany((EventCategory ec) => ec.Events))
					{
						AddEventHooks(ev2);
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

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			if (base.ModuleSettings.GlobalDrawerVisible.get_Value())
			{
				ToggleContainers(show: true);
			}
		}

		private void ToggleContainers(bool show)
		{
			if (!base.ModuleSettings.GlobalDrawerVisible.get_Value())
			{
				show = false;
			}
			_areas.Values.ToList().ForEach(delegate(EventArea area)
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
			});
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			ToggleContainers(base.ShowUI);
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
				string startsInTranslation = base.TranslationState.GetTranslation("eventArea-reminder-startsIn", "Starts in");
				EventNotification eventNotification = new EventNotification(ev, startsInTranslation + " " + e.Humanize(2, null, TimeUnit.Week, TimeUnit.Second) + "!", base.ModuleSettings.ReminderPosition.X.get_Value(), base.ModuleSettings.ReminderPosition.Y.get_Value(), base.IconState);
				eventNotification.BackgroundOpacity = base.ModuleSettings.ReminderOpacity.get_Value();
				eventNotification.Show(TimeSpan.FromSeconds(base.ModuleSettings.ReminderDuration.get_Value()));
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
			base.ModuleSettings.UpdateDrawerLocalization(configuration, base.TranslationState);
			EventArea eventArea = new EventArea(configuration, base.IconState, base.TranslationState, EventState, base.WorldbossState, base.MapchestState, base.PointOfInterestState, MapUtil, GetFlurlClient(), base.API_URL, () => NowUTC, () => ((Module)this).get_Version(), () => base.BlishHUDAPIState.AccessToken);
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

		protected override void OnSettingWindowBuild(TabbedWindow2 settingWindow)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Expected O, but got Unknown
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Expected O, but got Unknown
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("156736.png"), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(base.ModuleSettings, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "General", (int?)null));
			AreaSettingsView areaSettingsView = new AreaSettingsView(() => _areas.Values.Select((EventArea area) => area.Configuration), () => _eventCategories, base.ModuleSettings, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, EventState, GameService.Content.get_DefaultFont16())
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
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("605018.png"), (Func<IView>)(() => (IView)(object)areaSettingsView), "Event Areas", (int?)null));
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("1466345.png"), (Func<IView>)(() => (IView)(object)new ReminderSettingsView(base.ModuleSettings, () => _eventCategories, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Reminders", (int?)null));
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("759448.png"), (Func<IView>)(() => (IView)(object)new DynamicEventsSettingsView(DynamicEventState, base.ModuleSettings, GetFlurlClient(), base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Dynamic Events", (int?)null));
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("156764.png"), (Func<IView>)(() => (IView)(object)new CustomEventView(base.Gw2ApiManager, base.IconState, base.TranslationState, base.BlishHUDAPIState)
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Custom Events", (int?)null));
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("157097.png"), (Func<IView>)(() => (IView)(object)new HelpView(() => _eventCategories, base.API_URL, base.Gw2ApiManager, base.IconState, base.TranslationState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Help", (int?)null));
		}

		protected override string GetDirectoryName()
		{
			return "events";
		}

		protected override void ConfigureStates(StateConfigurations configurations)
		{
			configurations.BlishHUDAPI.Enabled = true;
			configurations.Account.Enabled = true;
			configurations.Worldbosses.Enabled = true;
			configurations.Mapchests.Enabled = true;
			configurations.PointOfInterests.Enabled = true;
		}

		private void BlishHUDAPIState_NewLogin(object sender, EventArgs e)
		{
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
		}

		private void BlishHUDAPIState_LoggedOut(object sender, EventArgs e)
		{
			_lastEventUpdate.Value = _updateEventsInterval.TotalMilliseconds;
		}

		protected override Collection<ManagedState> GetAdditionalStates(string directoryPath)
		{
			Collection<ManagedState> collection = new Collection<ManagedState>();
			EventState = new EventState(new StateConfiguration
			{
				AwaitLoading = false,
				Enabled = true,
				SaveInterval = TimeSpan.FromSeconds(30.0)
			}, directoryPath, () => NowUTC);
			DynamicEventState = new DynamicEventState(new APIStateConfiguration
			{
				AwaitLoading = false,
				Enabled = true,
				SaveInterval = Timeout.InfiniteTimeSpan
			}, base.Gw2ApiManager, GetFlurlClient(), API_ROOT_URL);
			collection.Add(EventState);
			collection.Add(DynamicEventState);
			return collection;
		}

		protected override AsyncTexture2D GetEmblem()
		{
			return base.IconState.GetIcon(base.IsPrerelease ? "textures/emblem_demo.png" : "102392.png");
		}

		protected override AsyncTexture2D GetCornerIcon()
		{
			return base.IconState.GetIcon("textures/event_boss_grey" + (base.IsPrerelease ? "_demo" : "") + ".png");
		}

		protected override void Unload()
		{
			base.Logger.Debug("Unload module.");
			MapUtil?.Dispose();
			MapUtil = null;
			base.Logger.Debug("Unload drawer.");
			foreach (EventArea value in _areas.Values)
			{
				if (value != null)
				{
					((Control)value).Dispose();
				}
			}
			_areas?.Clear();
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
			base.Logger.Debug("Unload base.");
			base.Unload();
			base.Logger.Debug("Unloaded base.");
		}
	}
}
