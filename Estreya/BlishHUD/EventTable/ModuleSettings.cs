using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Resources;
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

		public class EventSettingsChangedEventArgs
		{
			public string Name { get; set; }

			public bool Enabled { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		private Color _defaultColor;

		private const string GLOBAL_SETTINGS = "event-table-global-settings";

		private const string LOCATION_SETTINGS = "event-table-location-settings";

		private const string EVENT_SETTINGS = "event-table-event-settings";

		private const string EVENT_LIST_SETTINGS = "event-table-event-list-settings";

		public Color DefaultGW2Color
		{
			get
			{
				return _defaultColor;
			}
			private set
			{
				_defaultColor = value;
			}
		}

		private SettingCollection Settings { get; set; }

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalEnabled { get; private set; }

		public SettingEntry<KeyBinding> GlobalEnabledHotkey { get; private set; }

		public SettingEntry<bool> RegisterCornerIcon { get; private set; }

		public SettingEntry<int> RefreshRateDelay { get; private set; }

		public SettingEntry<bool> AutomaticallyUpdateEventFile { get; private set; }

		public SettingEntry<Color> BackgroundColor { get; private set; }

		public SettingEntry<float> BackgroundColorOpacity { get; private set; }

		public SettingEntry<bool> HideOnMissingMumbleTicks { get; private set; }

		public SettingEntry<bool> HideInCombat { get; private set; }

		public SettingEntry<bool> HideOnOpenMap { get; private set; }

		public SettingEntry<bool> DebugEnabled { get; private set; }

		public SettingEntry<bool> ShowTooltips { get; private set; }

		public SettingEntry<TooltipTimeMode> TooltipTimeMode { get; private set; }

		public SettingEntry<bool> HandleLeftClick { get; private set; }

		public SettingEntry<LeftClickAction> LeftClickAction { get; private set; }

		public SettingEntry<bool> ShowContextMenuOnClick { get; private set; }

		public SettingEntry<BuildDirection> BuildDirection { get; private set; }

		public SettingEntry<float> Opacity { get; private set; }

		public SettingEntry<bool> DirectlyTeleportToWaypoint { get; private set; }

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

		public SettingEntry<EventCompletedAction> EventCompletedAcion { get; private set; }

		public SettingEntry<bool> UseEventTranslation { get; private set; }

		public List<SettingEntry<bool>> AllEvents { get; private set; } = new List<SettingEntry<bool>>();


		public event EventHandler<ModuleSettingsChangedEventArgs> ModuleSettingsChanged;

		public event EventHandler<EventSettingsChangedEventArgs> EventSettingChanged;

		public ModuleSettings(SettingCollection settings)
		{
			Settings = settings;
			BuildDefaultColor();
			InitializeGlobalSettings(settings);
			InitializeLocationSettings(settings);
		}

		private void BuildDefaultColor()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01ee: Expected O, but got Unknown
			Color val = new Color();
			val.set_Name("Dye Remover");
			val.set_Id(1);
			val.set_BaseRgb((IReadOnlyList<int>)new List<int> { 128, 26, 26 });
			ColorMaterial val2 = new ColorMaterial();
			val2.set_Brightness(15);
			val2.set_Contrast(1.25);
			val2.set_Hue(38);
			val2.set_Saturation(9.0 / 32.0);
			val2.set_Lightness(1.44531);
			val2.set_Rgb((IReadOnlyList<int>)new List<int> { 124, 108, 83 });
			val.set_Cloth(val2);
			ColorMaterial val3 = new ColorMaterial();
			val3.set_Brightness(-8);
			val3.set_Contrast(1.0);
			val3.set_Hue(34);
			val3.set_Saturation(0.3125);
			val3.set_Lightness(1.09375);
			val3.set_Rgb((IReadOnlyList<int>)new List<int> { 65, 49, 29 });
			val.set_Leather(val3);
			ColorMaterial val4 = new ColorMaterial();
			val4.set_Brightness(5);
			val4.set_Contrast(1.05469);
			val4.set_Hue(38);
			val4.set_Saturation(0.101563);
			val4.set_Lightness(1.36719);
			val4.set_Rgb((IReadOnlyList<int>)new List<int> { 96, 91, 83 });
			val.set_Metal(val4);
			ColorMaterial val5 = new ColorMaterial();
			val5.set_Brightness(15);
			val5.set_Contrast(1.25);
			val5.set_Hue(38);
			val5.set_Saturation(9.0 / 32.0);
			val5.set_Lightness(1.44531);
			val5.set_Rgb((IReadOnlyList<int>)new List<int> { 124, 108, 83 });
			val.set_Fur(val5);
			_defaultColor = val;
		}

		public async Task LoadAsync()
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
			GlobalEnabled = GlobalSettings.DefineSetting<bool>("GlobalEnabled", true, (Func<string>)(() => Strings.Setting_GlobalEnabled_Name), (Func<string>)(() => Strings.Setting_GlobalEnabled_Description));
			GlobalEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			GlobalEnabledHotkey = GlobalSettings.DefineSetting<KeyBinding>("GlobalEnabledHotkey", new KeyBinding((ModifierKeys)2, (Keys)69), (Func<string>)(() => Strings.Setting_GlobalEnabledHotkey_Name), (Func<string>)(() => Strings.Setting_GlobalEnabledHotkey_Description));
			GlobalEnabledHotkey.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)SettingChanged<KeyBinding>);
			GlobalEnabledHotkey.get_Value().set_Enabled(true);
			GlobalEnabledHotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				GlobalEnabled.set_Value(!GlobalEnabled.get_Value());
			});
			GlobalEnabledHotkey.get_Value().set_BlockSequenceFromGw2(true);
			RegisterCornerIcon = GlobalSettings.DefineSetting<bool>("RegisterCornerIcon", true, (Func<string>)(() => Strings.Setting_RegisterCornerIcon_Name), (Func<string>)(() => Strings.Setting_RegisterCornerIcon_Description));
			RegisterCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			RefreshRateDelay = GlobalSettings.DefineSetting<int>("RefreshRateDelay", 900, (Func<string>)(() => Strings.Setting_RefreshRateDelay_Title), (Func<string>)(() => string.Format(Strings.Setting_RefreshRateDelay_Description, RefreshRateDelay.GetRange<int>().Value.Min, RefreshRateDelay.GetRange<int>().Value.Max)));
			RefreshRateDelay.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			SettingComplianceExtensions.SetRange(RefreshRateDelay, 0, 900);
			AutomaticallyUpdateEventFile = GlobalSettings.DefineSetting<bool>("AutomaticallyUpdateEventFile", true, (Func<string>)(() => Strings.Setting_AutomaticallyUpdateEventFile_Name), (Func<string>)(() => Strings.Setting_AutomaticallyUpdateEventFile_Description));
			AutomaticallyUpdateEventFile.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideOnOpenMap = GlobalSettings.DefineSetting<bool>("HideOnOpenMap", true, (Func<string>)(() => Strings.Setting_HideOnMap_Name), (Func<string>)(() => Strings.Setting_HideOnMap_Description));
			HideOnOpenMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideOnMissingMumbleTicks = GlobalSettings.DefineSetting<bool>("HideOnMissingMumbleTicks", true, (Func<string>)(() => Strings.Setting_HideOnMissingMumbleTicks_Name), (Func<string>)(() => Strings.Setting_HideOnMissingMumbleTicks_Description));
			HideOnMissingMumbleTicks.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInCombat = GlobalSettings.DefineSetting<bool>("HideInCombat", false, (Func<string>)(() => Strings.Setting_HideInCombat_Name), (Func<string>)(() => Strings.Setting_HideInCombat_Description));
			HideInCombat.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			BackgroundColor = GlobalSettings.DefineSetting<Color>("BackgroundColor", DefaultGW2Color, (Func<string>)(() => Strings.Setting_BackgroundColor_Name), (Func<string>)(() => Strings.Setting_BackgroundColor_Description));
			BackgroundColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			BackgroundColorOpacity = GlobalSettings.DefineSetting<float>("BackgroundColorOpacity", 0f, (Func<string>)(() => Strings.Setting_BackgroundColorOpacity_Name), (Func<string>)(() => Strings.Setting_BackgroundColorOpacity_Description));
			SettingComplianceExtensions.SetRange(BackgroundColorOpacity, 0f, 1f);
			BackgroundColorOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingChanged<float>);
			EventTimeSpan = GlobalSettings.DefineSetting<string>("EventTimeSpan", "120", (Func<string>)(() => Strings.Setting_EventTimeSpan_Name), (Func<string>)(() => Strings.Setting_EventTimeSpan_Description));
			EventTimeSpan.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)SettingChanged<string>);
			SettingComplianceExtensions.SetValidation<string>(EventTimeSpan, (Func<string, SettingValidationResult>)delegate(string val)
			{
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				bool flag = true;
				string text = null;
				double num = 1440.0;
				if (double.TryParse(val, out var result))
				{
					if (result > num)
					{
						flag = false;
						text = string.Format(Strings.Setting_EventTimeSpan_Validation_OverLimit, num);
					}
				}
				else
				{
					flag = false;
					text = string.Format(Strings.Setting_EventTimeSpan_Validation_NoDouble, val);
				}
				return new SettingValidationResult(flag, text);
			});
			EventHistorySplit = GlobalSettings.DefineSetting<int>("EventHistorySplit", 50, (Func<string>)(() => Strings.Setting_EventHistorySplit_Name), (Func<string>)(() => Strings.Setting_EventHistorySplit_Description));
			SettingComplianceExtensions.SetRange(EventHistorySplit, 0, 75);
			EventHistorySplit.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			EventHeight = GlobalSettings.DefineSetting<int>("EventHeight", 20, (Func<string>)(() => Strings.Setting_EventHeight_Name), (Func<string>)(() => Strings.Setting_EventHeight_Description));
			SettingComplianceExtensions.SetRange(EventHeight, 5, 50);
			EventHeight.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			EventFontSize = GlobalSettings.DefineSetting<FontSize>("EventFontSize", (FontSize)16, (Func<string>)(() => Strings.Setting_EventFontSize_Name), (Func<string>)(() => Strings.Setting_EventFontSize_Description));
			EventFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)SettingChanged<FontSize>);
			DrawEventBorder = GlobalSettings.DefineSetting<bool>("DrawEventBorder", true, (Func<string>)(() => Strings.Setting_DrawEventBorder_Name), (Func<string>)(() => Strings.Setting_DrawEventBorder_Description));
			DrawEventBorder.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			DebugEnabled = GlobalSettings.DefineSetting<bool>("DebugEnabled", false, (Func<string>)(() => Strings.Setting_DebugEnabled_Name), (Func<string>)(() => Strings.Setting_DebugEnabled_Description));
			DebugEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			ShowTooltips = GlobalSettings.DefineSetting<bool>("ShowTooltips", true, (Func<string>)(() => Strings.Setting_ShowTooltips_Name), (Func<string>)(() => Strings.Setting_ShowTooltips_Description));
			ShowTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			TooltipTimeMode = GlobalSettings.DefineSetting<TooltipTimeMode>("TooltipTimeMode", Estreya.BlishHUD.EventTable.Models.TooltipTimeMode.Relative, (Func<string>)(() => Strings.Setting_TooltipTimeMode_Name), (Func<string>)(() => Strings.Setting_TooltipTimeMode_Description));
			TooltipTimeMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<TooltipTimeMode>>)SettingChanged<TooltipTimeMode>);
			HandleLeftClick = GlobalSettings.DefineSetting<bool>("HandleLeftClick", true, (Func<string>)(() => Strings.Setting_HandleLeftClick_Name), (Func<string>)(() => Strings.Setting_HandleLeftClick_Description));
			HandleLeftClick.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			LeftClickAction = GlobalSettings.DefineSetting<LeftClickAction>("LeftClickAction", Estreya.BlishHUD.EventTable.Models.LeftClickAction.CopyWaypoint, (Func<string>)(() => Strings.Setting_LeftClickAction_Title), (Func<string>)(() => Strings.Setting_LeftClickAction_Description));
			LeftClickAction.add_SettingChanged((EventHandler<ValueChangedEventArgs<LeftClickAction>>)SettingChanged<LeftClickAction>);
			DirectlyTeleportToWaypoint = GlobalSettings.DefineSetting<bool>("DirectlyTeleportToWaypoint", false, (Func<string>)(() => Strings.Setting_DirectlyTeleportToWaypoint_Title), (Func<string>)(() => Strings.Setting_DirectlyTeleportToWaypoint_Description));
			DirectlyTeleportToWaypoint.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			ShowContextMenuOnClick = GlobalSettings.DefineSetting<bool>("ShowContextMenuOnClick", true, (Func<string>)(() => Strings.Setting_ShowContextMenuOnClick_Name), (Func<string>)(() => Strings.Setting_ShowContextMenuOnClick_Description));
			ShowContextMenuOnClick.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			BuildDirection = GlobalSettings.DefineSetting<BuildDirection>("BuildDirection", Estreya.BlishHUD.EventTable.Models.BuildDirection.Top, (Func<string>)(() => Strings.Setting_BuildDirection_Name), (Func<string>)(() => Strings.Setting_BuildDirection_Description));
			BuildDirection.add_SettingChanged((EventHandler<ValueChangedEventArgs<BuildDirection>>)SettingChanged<BuildDirection>);
			Opacity = GlobalSettings.DefineSetting<float>("Opacity", 1f, (Func<string>)(() => Strings.Setting_Opacity_Name), (Func<string>)(() => Strings.Setting_Opacity_Description));
			SettingComplianceExtensions.SetRange(Opacity, 0.1f, 1f);
			Opacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingChanged<float>);
			UseFiller = GlobalSettings.DefineSetting<bool>("UseFiller", false, (Func<string>)(() => Strings.Setting_UseFiller_Name), (Func<string>)(() => Strings.Setting_UseFiller_Description));
			UseFiller.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			UseFillerEventNames = GlobalSettings.DefineSetting<bool>("UseFillerEventNames", false, (Func<string>)(() => Strings.Setting_UseFillerEventNames_Name), (Func<string>)(() => Strings.Setting_UseFillerEventNames_Description));
			UseFillerEventNames.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			TextColor = GlobalSettings.DefineSetting<Color>("TextColor", DefaultGW2Color, (Func<string>)(() => Strings.Setting_TextColor_Name), (Func<string>)(() => Strings.Setting_TextColor_Description));
			TextColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			FillerTextColor = GlobalSettings.DefineSetting<Color>("FillerTextColor", DefaultGW2Color, (Func<string>)(() => Strings.Setting_FillerTextColor_Name), (Func<string>)(() => Strings.Setting_FillerTextColor_Description));
			FillerTextColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<Color>>)SettingChanged<Color>);
			EventCompletedAcion = GlobalSettings.DefineSetting<EventCompletedAction>("EventCompletedAcion", EventCompletedAction.Crossout, (Func<string>)(() => Strings.Setting_EventCompletedAction_Name), (Func<string>)(() => Strings.Setting_EventCompletedAction_Description));
			EventCompletedAcion.add_SettingChanged((EventHandler<ValueChangedEventArgs<EventCompletedAction>>)SettingChanged<EventCompletedAction>);
			UseEventTranslation = GlobalSettings.DefineSetting<bool>("UseEventTranslation", true, (Func<string>)(() => Strings.Setting_UseEventTranslation_Name), (Func<string>)(() => Strings.Setting_UseEventTranslation_Description));
			UseEventTranslation.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
		}

		private void InitializeLocationSettings(SettingCollection settings)
		{
			LocationSettings = settings.AddSubCollection("event-table-location-settings", false);
			int height = 1080;
			int width = 1920;
			LocationX = LocationSettings.DefineSetting<int>("LocationX", (int)((double)width * 0.1), (Func<string>)(() => Strings.Setting_LocationX_Name), (Func<string>)(() => Strings.Setting_LocationX_Description));
			SettingComplianceExtensions.SetRange(LocationX, 0, width);
			LocationX.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			LocationY = LocationSettings.DefineSetting<int>("LocationY", (int)((double)height * 0.1), (Func<string>)(() => Strings.Setting_LocationY_Name), (Func<string>)(() => Strings.Setting_LocationY_Description));
			SettingComplianceExtensions.SetRange(LocationY, 0, height);
			LocationY.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged<int>);
			Width = LocationSettings.DefineSetting<int>("Width", (int)((double)width * 0.5), (Func<string>)(() => Strings.Setting_Width_Name), (Func<string>)(() => Strings.Setting_Width_Description));
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
						group e by e.Key into eg
						select eg.First();
				}
				foreach (Event e2 in enumerable)
				{
					SettingEntry<bool> setting = eventList.DefineSetting<bool>(e2.SettingKey, true, (Func<string>)null, (Func<string>)null);
					setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
					{
						SettingEntry<bool> val = (SettingEntry<bool>)s;
						this.EventSettingChanged?.Invoke(s, new EventSettingsChangedEventArgs
						{
							Name = ((SettingEntry)val).get_EntryKey(),
							Enabled = e.get_NewValue()
						});
						SettingChanged<bool>(s, e);
					});
					AllEvents.Add(setting);
				}
			}
		}

		private void SettingChanged<T>(object sender, ValueChangedEventArgs<T> e)
		{
			SettingEntry<T> settingEntry = (SettingEntry<T>)sender;
			string prevValue = ((e.get_PreviousValue().GetType() == typeof(string)) ? e.get_PreviousValue().ToString() : JsonConvert.SerializeObject((object)e.get_PreviousValue()));
			string newValue = ((e.get_NewValue().GetType() == typeof(string)) ? e.get_NewValue().ToString() : JsonConvert.SerializeObject((object)e.get_NewValue()));
			Logger.Debug("Changed setting \"" + ((SettingEntry)settingEntry).get_EntryKey() + "\" from \"" + prevValue + "\" to \"" + newValue + "\"");
			this.ModuleSettingsChanged?.Invoke(this, new ModuleSettingsChangedEventArgs
			{
				Name = ((SettingEntry)settingEntry).get_EntryKey(),
				Value = e.get_NewValue()
			});
		}
	}
}
