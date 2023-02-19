using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
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
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Humanizer;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : BaseModule<EventTableModule, ModuleSettings>
	{
		private Dictionary<string, EventArea> _areas = new Dictionary<string, EventArea>();

		private AsyncLock _eventCategoryLock = new AsyncLock();

		private List<EventCategory> _eventCategories = new List<EventCategory>();

		private static TimeSpan _updateEventsInterval = TimeSpan.FromMinutes(30.0);

		private AsyncRef<double> _lastEventUpdate = new AsyncRef<double>(0.0);

		private List<IEntity> _worldEntites = new List<IEntity>();

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
			MapUtil = new MapUtil(base.ModuleSettings.MapKeybinding.get_Value(), base.Gw2ApiManager);
			DynamicEventHandler = new DynamicEventHandler(MapUtil, DynamicEventState, base.Gw2ApiManager, base.ModuleSettings);
			await DynamicEventHandler.AddDynamicEventsToMap();
			await DynamicEventHandler.AddDynamicEventsToWorld();
			base.Logger.Debug("Load events.");
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
			using (await _eventCategoryLock.LockAsync())
			{
				try
				{
					_eventCategories?.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
					{
						RemoveEventHooks(ev);
					});
					_eventCategories?.Clear();
					List<EventCategory> categories = await GetFlurlClient().Request(base.API_URL, "events").GetJsonAsync<List<EventCategory>>(default(CancellationToken), (HttpCompletionOption)0);
					int eventCategoryCount = categories.Count;
					int eventCount = categories.Sum((EventCategory ec) => ec.Events.Count);
					base.Logger.Info($"Loaded {eventCategoryCount} Categories with {eventCount} Events.");
					categories.ForEach(delegate(EventCategory ec)
					{
						ec.Load(base.TranslationState);
					});
					_eventCategories = categories;
					_eventCategories.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
					{
						AddEventHooks(ev);
					});
					SetAreaEvents();
				}
				catch (FlurlHttpException ex2)
				{
					string message = await ex2.GetResponseStringAsync();
					base.Logger.Warn((Exception)ex2, "Failed loading events: " + message);
				}
				catch (Exception ex)
				{
					base.Logger.Warn(ex, "Failed loading events.");
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
				if (show && area.Enabled)
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
					DateTime now = NowUTC;
					_eventCategories.SelectMany((EventCategory ec) => ec.Events).ToList().ForEach(delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
					{
						ev.Update(now);
					});
				}
			}
			DynamicEventHandler.Update(gameTime);
			UpdateUtil.UpdateAsync(LoadEvents, gameTime, _updateEventsInterval.TotalMilliseconds, _lastEventUpdate);
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
				string startsInTranslation = base.TranslationState.GetTranslation("eventArea-reminder-startsIn", "Starts in");
				EventNotification eventNotification = new EventNotification(ev, startsInTranslation + " " + e.Humanize() + "!", base.ModuleSettings.ReminderPosition.X.get_Value(), base.ModuleSettings.ReminderPosition.Y.get_Value(), base.IconState);
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
			EventArea eventArea = new EventArea(configuration, base.IconState, base.TranslationState, EventState, base.WorldbossState, base.MapchestState, base.PointOfInterestState, MapUtil, GetFlurlClient(), base.API_URL, () => NowUTC, () => ((Module)this).get_Version());
			((Control)eventArea).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			EventArea area = eventArea;
			_areas.Add(configuration.Name, area);
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
			_areas.Remove(configuration.Name);
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
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("156736.png"), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(base.ModuleSettings, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "General", (int?)null));
			AreaSettingsView areaSettingsView = new AreaSettingsView(() => _areas.Values.Select((EventArea area) => area.Configuration), () => _eventCategories, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, EventState, GameService.Content.get_DefaultFont16())
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
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("841721.png"), (Func<IView>)(() => (IView)(object)new ReminderSettingsView(base.ModuleSettings, () => _eventCategories, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Reminders", (int?)null));
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("759448.png"), (Func<IView>)(() => (IView)(object)new DynamicEventsSettingsView(DynamicEventState, base.ModuleSettings, base.Gw2ApiManager, base.IconState, base.TranslationState, base.SettingEventState, GameService.Content.get_DefaultFont16())
			{
				DefaultColor = base.ModuleSettings.DefaultGW2Color
			}), "Dynamic Events", (int?)null));
			base.SettingsWindow.get_Tabs().Add(new Tab(base.IconState.GetIcon("482926.png"), (Func<IView>)(() => (IView)(object)new HelpView(() => _eventCategories, base.API_URL, base.Gw2ApiManager, base.IconState, base.TranslationState, GameService.Content.get_DefaultFont16())
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
			configurations.Account.Enabled = true;
			configurations.Worldbosses.Enabled = true;
			configurations.Mapchests.Enabled = true;
			configurations.PointOfInterests.Enabled = true;
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
			}, base.Gw2ApiManager, GetFlurlClient());
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
