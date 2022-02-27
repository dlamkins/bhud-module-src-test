using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Models;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable
{
	public class ModuleSettings
	{
		public class ModuleSettingsChangedEventArgs
		{
			public string Name { get; set; }

			public object Value { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		private const string GLOBAL_SETTINGS = "event-table-global-settings";

		private const string LOCATION_SETTINGS = "event-table-location-settings";

		private const string EVENT_SETTINGS = "event-table-event-settings";

		private const string EVENT_LIST_SETTINGS = "event-table-event-list-settings";

		private Color DefaultGW2Color { get; set; }

		public SettingCollection Settings { get; private set; }

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalEnabled { get; private set; }

		public SettingEntry<KeyBinding> GlobalEnabledHotkey { get; private set; }

		public SettingEntry<bool> RegisterCornerIcon { get; private set; }

		public SettingEntry<bool> AutomaticallyUpdateEventFile { get; private set; }

		public SettingEntry<Color> BackgroundColor { get; private set; }

		public SettingEntry<float> BackgroundColorOpacity { get; private set; }

		public SettingEntry<bool> HideOnMissingMumbleTicks { get; private set; }

		public SettingEntry<bool> HideInCombat { get; private set; }

		public SettingEntry<bool> HideOnOpenMap { get; private set; }

		public SettingEntry<bool> DebugEnabled { get; private set; }

		public SettingEntry<bool> ShowTooltips { get; private set; }

		public SettingEntry<TooltipTimeMode> TooltipTimeMode { get; private set; }

		public SettingEntry<bool> CopyWaypointOnClick { get; private set; }

		public SettingEntry<bool> ShowContextMenuOnClick { get; private set; }

		public SettingEntry<BuildDirection> BuildDirection { get; private set; }

		public SettingEntry<float> Opacity { get; set; }

		public SettingCollection LocationSettings { get; private set; }

		public SettingEntry<int> LocationX { get; private set; }

		public SettingEntry<int> LocationY { get; private set; }

		public SettingEntry<int> Width { get; private set; }

		public SettingCollection EventSettings { get; private set; }

		public SettingEntry<string> EventTimeSpan { get; private set; }

		public SettingEntry<int> EventHistorySplit { get; private set; }

		public SettingEntry<int> EventHeight { get; private set; }

		public SettingEntry<bool> DrawEventBorder { get; private set; }

		public SettingEntry<FontSize> EventFontSize { get; private set; }

		public SettingEntry<bool> UseFiller { get; private set; }

		public SettingEntry<bool> UseFillerEventNames { get; private set; }

		public SettingEntry<Color> TextColor { get; private set; }

		public SettingEntry<Color> FillerTextColor { get; private set; }

		public SettingEntry<WorldbossCompletedAction> WorldbossCompletedAcion { get; private set; }

		public List<SettingEntry<bool>> AllEvents { get; private set; } = new List<SettingEntry<bool>>();


		public event EventHandler<ModuleSettingsChangedEventArgs> ModuleSettingsChanged;

		public ModuleSettings(SettingCollection settings)
		{
			Settings = settings;
			InitializeGlobalSettings(settings);
			InitializeLocationSettings(settings);
		}

		public async Task Load()
		{
			try
			{
				DefaultGW2Color = await ((IBulkExpandableClient<Color, int>)(object)EventTableModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).GetAsync(1, default(CancellationToken));
			}
			catch (Exception ex)
			{
				Logger.Warn("Could not load default gw2 color: " + ex.Message);
			}
		}

		private void InitializeGlobalSettings(SettingCollection settings)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			GlobalSettings = settings.AddSubCollection("event-table-global-settings", false);
			GlobalEnabled = GlobalSettings.DefineSetting<bool>("GlobalEnabled", true, (Func<string>)(() => "Event Table Enabled"), (Func<string>)(() => "Whether the event table should be displayed."));
			GlobalEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			GlobalEnabledHotkey = GlobalSettings.DefineSetting<KeyBinding>("GlobalEnabledHotkey", new KeyBinding((ModifierKeys)2, (Keys)69), (Func<string>)(() => "Event Table Hotkey"), (Func<string>)(() => "The keybinding which will toggle the event table."));
			GlobalEnabledHotkey.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)SettingChanged<KeyBinding>);
			GlobalEnabledHotkey.get_Value().set_Enabled(true);
			GlobalEnabledHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				GlobalEnabled.set_Value(!GlobalEnabled.get_Value());
			});
			GlobalEnabledHotkey.get_Value().set_BlockSequenceFromGw2(true);
			RegisterCornerIcon = GlobalSettings.DefineSetting<bool>("RegisterCornerIcon", true, (Func<string>)(() => "Register Corner Icon"), (Func<string>)(() => "Whether the event table should add it's own corner icon to access settings."));
			RegisterCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			AutomaticallyUpdateEventFile = GlobalSettings.DefineSetting<bool>("AutomaticallyUpdateEventFile", true, (Func<string>)(() => "Automatically Update Event File"), (Func<string>)(() => "Whether the event table should automatically update the exported event file to the newest version."));
			AutomaticallyUpdateEventFile.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideOnOpenMap = GlobalSettings.DefineSetting<bool>("HideOnOpenMap", true, (Func<string>)(() => "Hide on open Map"), (Func<string>)(() => "Whether the event table should hide when the map is open."));
			HideOnOpenMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideOnMissingMumbleTicks = GlobalSettings.DefineSetting<bool>("HideOnMissingMumbleTicks", true, (Func<string>)(() => "Hide on Cutscenes"), (Func<string>)(() => "Whether the event table should hide when cutscenes are played."));
			HideOnMissingMumbleTicks.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInCombat = GlobalSettings.DefineSetting<bool>("HideInCombat", false, (Func<string>)(() => "Hide in Combat"), (Func<string>)(() => "Whether the event table should hide when the player is in combat."));
			HideInCombat.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			BackgroundColor = GlobalSettings.DefineSetting<Color>("BackgroundColor", DefaultGW2Color, (Func<string>)(() => "Background Color"), (Func<string>)(() => "Defines the background color."));
			BackgroundColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			BackgroundColorOpacity = GlobalSettings.DefineSetting<float>("BackgroundColorOpacity", 0f, (Func<string>)(() => "Background Color Opacity"), (Func<string>)(() => "Defines the opacity of the background."));
			SettingComplianceExtensions.SetRange(BackgroundColorOpacity, 0f, 1f);
			BackgroundColorOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingChanged<float>);
			EventTimeSpan = GlobalSettings.DefineSetting<string>("EventTimeSpan", "120", (Func<string>)(() => "Event Timespan"), (Func<string>)(() => "The timespan the event table should cover."));
			EventTimeSpan.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)SettingChanged<string>);
			EventHistorySplit = GlobalSettings.DefineSetting<int>("EventHistorySplit", 50, (Func<string>)(() => "Event History Split"), (Func<string>)(() => "Defines how much history the timespan should contain."));
			SettingComplianceExtensions.SetRange(EventHistorySplit, 0, 75);
			EventHistorySplit.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			EventHeight = GlobalSettings.DefineSetting<int>("EventHeight", 20, (Func<string>)(() => "Event Height"), (Func<string>)(() => "Defines the height of a single event row."));
			SettingComplianceExtensions.SetRange(EventHeight, 5, 50);
			EventHeight.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			EventFontSize = GlobalSettings.DefineSetting<FontSize>("EventFontSize", (FontSize)16, (Func<string>)(() => "Event Font Size"), (Func<string>)(() => "Defines the size of the font used for events."));
			EventFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)SettingChanged<FontSize>);
			DrawEventBorder = GlobalSettings.DefineSetting<bool>("DrawEventBorder", true, (Func<string>)(() => "Draw Event Border"), (Func<string>)(() => "Whether the events should have a small border."));
			DrawEventBorder.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			DebugEnabled = GlobalSettings.DefineSetting<bool>("DebugEnabled", false, (Func<string>)(() => "Debug Enabled"), (Func<string>)(() => "Whether the event table should be running in debug mode."));
			DebugEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			ShowTooltips = GlobalSettings.DefineSetting<bool>("ShowTooltips", true, (Func<string>)(() => "Show Tooltips"), (Func<string>)(() => "Whether the event table should display event information on hover."));
			ShowTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			TooltipTimeMode = GlobalSettings.DefineSetting<TooltipTimeMode>("TooltipTimeMode", Estreya.BlishHUD.EventTable.Models.TooltipTimeMode.Relative, (Func<string>)(() => "Tooltip Time Mode"), (Func<string>)(() => "Defines the mode in which the tooltip times are displayed."));
			TooltipTimeMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<TooltipTimeMode>>)SettingChanged<TooltipTimeMode>);
			CopyWaypointOnClick = GlobalSettings.DefineSetting<bool>("CopyWaypointOnClick", true, (Func<string>)(() => "Copy Waypoints"), (Func<string>)(() => "Whether the event table should copy waypoints to clipboard if event has been left clicked."));
			CopyWaypointOnClick.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			ShowContextMenuOnClick = GlobalSettings.DefineSetting<bool>("ShowContextMenuOnClick", true, (Func<string>)(() => "Show Context Menu"), (Func<string>)(() => "Whether the event table should show a context menu if an event has been right clicked."));
			ShowContextMenuOnClick.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			BuildDirection = GlobalSettings.DefineSetting<BuildDirection>("BuildDirection", Estreya.BlishHUD.EventTable.Models.BuildDirection.Top, (Func<string>)(() => "Build Direction"), (Func<string>)(() => "Whether the event table should be build from the top or the bottom."));
			BuildDirection.add_SettingChanged((EventHandler<ValueChangedEventArgs<BuildDirection>>)SettingChanged<BuildDirection>);
			Opacity = GlobalSettings.DefineSetting<float>("Opacity", 1f, (Func<string>)(() => "Opacity"), (Func<string>)(() => "Defines the opacity of the event table."));
			SettingComplianceExtensions.SetRange(Opacity, 0.1f, 1f);
			Opacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingChanged<float>);
			UseFiller = GlobalSettings.DefineSetting<bool>("UseFiller", false, (Func<string>)(() => "Use Filler Events"), (Func<string>)(() => "Whether the event table should fill empty spaces with filler events."));
			UseFiller.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			UseFillerEventNames = GlobalSettings.DefineSetting<bool>("UseFillerEventNames", false, (Func<string>)(() => "Use Filler Event Names"), (Func<string>)(() => "Whether the event fillers should have names."));
			UseFillerEventNames.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			TextColor = GlobalSettings.DefineSetting<Color>("TextColor", DefaultGW2Color, (Func<string>)(() => "Text Color"), (Func<string>)(() => "Defines the text color of events."));
			TextColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			FillerTextColor = GlobalSettings.DefineSetting<Color>("FillerTextColor", DefaultGW2Color, (Func<string>)(() => "Filler Text Color"), (Func<string>)(() => "Defines the text color of filler events."));
			FillerTextColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			WorldbossCompletedAcion = GlobalSettings.DefineSetting<WorldbossCompletedAction>("WorldbossCompletedAcion", WorldbossCompletedAction.Crossout, (Func<string>)(() => "Worldboss Completed Action"), (Func<string>)(() => "Defines the action when a worldboss has been completed."));
			WorldbossCompletedAcion.add_SettingChanged((EventHandler<ValueChangedEventArgs<WorldbossCompletedAction>>)SettingChanged<WorldbossCompletedAction>);
		}

		private void InitializeLocationSettings(SettingCollection settings)
		{
			LocationSettings = settings.AddSubCollection("event-table-location-settings", false);
			int height = 1080;
			int width = 1920;
			LocationX = LocationSettings.DefineSetting<int>("LocationX", (int)((double)width * 0.1), (Func<string>)(() => "Location X"), (Func<string>)(() => "Where the event table should be displayed on the X axis."));
			SettingComplianceExtensions.SetRange(LocationX, 0, width);
			LocationX.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			LocationY = LocationSettings.DefineSetting<int>("LocationY", (int)((double)height * 0.1), (Func<string>)(() => "Location Y"), (Func<string>)(() => "Where the event table should be displayed on the Y axis."));
			SettingComplianceExtensions.SetRange(LocationY, 0, height);
			LocationY.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			Width = LocationSettings.DefineSetting<int>("Width", (int)((double)width * 0.5), (Func<string>)(() => "Width"), (Func<string>)(() => "The width of the event table."));
			SettingComplianceExtensions.SetRange(Width, 0, width);
			Width.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
		}

		public void InitializeEventSettings(IEnumerable<EventCategory> eventCategories)
		{
			EventSettings = Settings.AddSubCollection("event-table-event-settings", false);
			SettingCollection eventList = EventSettings.AddSubCollection("event-table-event-list-settings", false);
			foreach (EventCategory category in eventCategories)
			{
				IEnumerable<Event> enumerable;
				if (!category.ShowCombined)
				{
					IEnumerable<Event> events = category.Events;
					enumerable = events;
				}
				else
				{
					enumerable = from e in category.Events
						group e by e.Name into eg
						select eg.First();
				}
				foreach (Event e2 in enumerable)
				{
					SettingEntry<bool> setting = eventList.DefineSetting<bool>(e2.Name, true, (Func<string>)null, (Func<string>)null);
					setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
					AllEvents.Add(setting);
				}
			}
		}

		private void SettingChanged<T>(object sender, ValueChangedEventArgs<T> e)
		{
			SettingEntry<T> settingEntry = (SettingEntry<T>)sender;
			string prevValue = JsonConvert.SerializeObject((object)e.get_PreviousValue());
			string newValue = JsonConvert.SerializeObject((object)e.get_NewValue());
			Logger.Debug("Changed setting \"" + ((SettingEntry)settingEntry).get_EntryKey() + "\" from \"" + prevValue + "\" to \"" + newValue + "\"");
			this.ModuleSettingsChanged?.Invoke(this, new ModuleSettingsChangedEventArgs
			{
				Name = ((SettingEntry)settingEntry).get_EntryKey(),
				Value = e.get_NewValue()
			});
		}
	}
}
