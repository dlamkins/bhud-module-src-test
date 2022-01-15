using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.UI.Container;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Input;

namespace Estreya.BlishHUD.EventTable
{
	public class ModuleSettings
	{
		public class ModuleSettingsChangedEventArgs
		{
			public string Name { get; set; }

			public object Value { get; set; }
		}

		private const string GLOBAL_SETTINGS = "event-table-global-settings";

		private const string LOCATION_SETTINGS = "event-table-location-settings";

		private const string EVENT_SETTINGS = "event-table-event-settings";

		private const string EVENT_LIST_SETTINGS = "event-table-event-list-settings";

		public SettingCollection Settings { get; private set; }

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalEnabled { get; private set; }

		public SettingEntry<KeyBinding> GlobalEnabledHotkey { get; private set; }

		public SettingEntry<Color> BackgroundColor { get; private set; }

		public SettingEntry<float> BackgroundColorOpacity { get; private set; }

		public SettingEntry<bool> HideOnMissingMumbleTicks { get; private set; }

		public SettingEntry<bool> DebugEnabled { get; private set; }

		public SettingEntry<bool> ShowTooltips { get; private set; }

		public SettingEntry<bool> CopyWaypointOnClick { get; private set; }

		public SettingEntry<bool> ShowContextMenuOnClick { get; private set; }

		public SettingEntry<BuildDirection> BuildDirection { get; private set; }

		public SettingEntry<float> Opacity { get; set; }

		public SettingCollection LocationSettings { get; private set; }

		public SettingEntry<int> LocationX { get; private set; }

		public SettingEntry<int> LocationY { get; private set; }

		public SettingEntry<int> Height { get; private set; }

		public SettingEntry<bool> SnapHeight { get; private set; }

		public SettingEntry<int> Width { get; private set; }

		public SettingCollection EventSettings { get; private set; }

		public SettingEntry<int> EventTimeSpan { get; private set; }

		public SettingEntry<int> EventHeight { get; private set; }

		public SettingEntry<bool> DrawEventBorder { get; private set; }

		public SettingEntry<EventTableContainer.FontSize> EventFontSize { get; private set; }

		public SettingEntry<bool> UseFiller { get; private set; }

		public SettingEntry<bool> UseFillerEventNames { get; private set; }

		public SettingEntry<Color> TextColor { get; private set; }

		public SettingEntry<Color> FillerTextColor { get; private set; }

		public List<SettingEntry<bool>> AllEvents { get; private set; } = new List<SettingEntry<bool>>();


		public event EventHandler<ModuleSettingsChangedEventArgs> ModuleSettingsChanged;

		public ModuleSettings(SettingCollection settings)
		{
			Settings = settings;
			InitializeGlobalSettings(settings);
			InitializeLocationSettings(settings);
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
			HideOnMissingMumbleTicks = GlobalSettings.DefineSetting<bool>("HideOnMissingMumbleTicks", true, (Func<string>)(() => "Hide on missing Mumble Tick"), (Func<string>)(() => "Whether the event table should hide when mumble ticks are missing."));
			HideOnMissingMumbleTicks.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			BackgroundColor = GlobalSettings.DefineSetting<Color>("BackgroundColor", ((IBulkExpandableClient<Color, int>)(object)EventTableModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).GetAsync(1, default(CancellationToken)).Result, (Func<string>)(() => "Background Color"), (Func<string>)(() => "Defines the background color."));
			BackgroundColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			BackgroundColorOpacity = GlobalSettings.DefineSetting<float>("BackgroundColorOpacity", 0f, (Func<string>)(() => "Background Color Opacity"), (Func<string>)(() => "Defines the opacity of the background."));
			SettingComplianceExtensions.SetRange(BackgroundColorOpacity, 0f, 1f);
			BackgroundColorOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingChanged<float>);
			EventTimeSpan = GlobalSettings.DefineSetting<int>("EventTimeSpan", 120, (Func<string>)(() => "Event Timespan"), (Func<string>)(() => "The timespan the event table should cover."));
			SettingComplianceExtensions.SetRange(EventTimeSpan, 30, 300);
			EventTimeSpan.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			EventHeight = GlobalSettings.DefineSetting<int>("EventHeight", 20, (Func<string>)(() => "Event Height"), (Func<string>)(() => "Defines the height of a single event row."));
			SettingComplianceExtensions.SetRange(EventHeight, 5, 50);
			EventHeight.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			EventFontSize = GlobalSettings.DefineSetting<EventTableContainer.FontSize>("EventFontSize", EventTableContainer.FontSize.Size16, (Func<string>)(() => "Event Font Size"), (Func<string>)(() => "Defines the size of the font used for events."));
			EventFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<EventTableContainer.FontSize>>)SettingChanged<EventTableContainer.FontSize>);
			DrawEventBorder = GlobalSettings.DefineSetting<bool>("DrawEventBorder", true, (Func<string>)(() => "Draw Event Border"), (Func<string>)(() => "Whether the events should have a small border."));
			DrawEventBorder.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			DebugEnabled = GlobalSettings.DefineSetting<bool>("DebugEnabled", false, (Func<string>)(() => "Debug Enabled"), (Func<string>)(() => "Whether the event table should be running in debug mode."));
			DebugEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			ShowTooltips = GlobalSettings.DefineSetting<bool>("ShowTooltips", true, (Func<string>)(() => "Show Tooltips"), (Func<string>)(() => "Whether the event table should display event information on hover."));
			DebugEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
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
			TextColor = GlobalSettings.DefineSetting<Color>("TextColor", ((IBulkExpandableClient<Color, int>)(object)EventTableModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).GetAsync(1, default(CancellationToken)).Result, (Func<string>)(() => "Text Color"), (Func<string>)(() => "Defines the text color of events."));
			TextColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			FillerTextColor = GlobalSettings.DefineSetting<Color>("FillerTextColor", ((IBulkExpandableClient<Color, int>)(object)EventTableModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).GetAsync(1, default(CancellationToken)).Result, (Func<string>)(() => "Filler Text Color"), (Func<string>)(() => "Defines the text color of filler events."));
			FillerTextColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
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
			Height = LocationSettings.DefineSetting<int>("Height", (int)((double)height * 0.2), (Func<string>)(() => "Height"), (Func<string>)(() => "The height of the event table."));
			SettingComplianceExtensions.SetRange(Height, 0, height);
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)Height, true);
			Height.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			SnapHeight = LocationSettings.DefineSetting<bool>("SnapHeight", true, (Func<string>)(() => "Snap Height"), (Func<string>)(() => "Whether the event table should auto resize height to content."));
			SnapHeight.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				SettingComplianceExtensions.SetDisabled((SettingEntry)(object)Height, e.get_NewValue());
				SettingChanged<bool>(s, e);
			});
			Width = LocationSettings.DefineSetting<int>("Width", (int)((double)width * 0.5), (Func<string>)(() => "Width"), (Func<string>)(() => "The width of the event table."));
			SettingComplianceExtensions.SetRange(Width, 0, width);
			Width.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
		}

		private void SettingChanged<T>(object sender, ValueChangedEventArgs<T> e)
		{
			SettingEntry<T> settingEntry = (SettingEntry<T>)sender;
			this.ModuleSettingsChanged?.Invoke(this, new ModuleSettingsChangedEventArgs
			{
				Name = ((SettingEntry)settingEntry).get_EntryKey(),
				Value = e.get_NewValue()
			});
		}
	}
}
