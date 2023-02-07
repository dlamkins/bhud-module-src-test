using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Models.Drawers;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.State;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Input;

namespace Estreya.BlishHUD.EventTable
{
	public class ModuleSettings : BaseModuleSettings
	{
		private const string EVENT_SETTINGS = "event-settings";

		private const string EVENT_AREA_SETTINGS = "event-area-settings";

		public SettingEntry<KeyBinding> MapKeybinding { get; private set; }

		public SettingCollection EventSettings { get; private set; }

		public SettingCollection EventAreaSettings { get; private set; }

		public SettingEntry<List<string>> EventAreaNames { get; private set; }

		public SettingEntry<bool> RemindersEnabled { get; set; }

		public EventReminderPositition ReminderPosition { get; set; }

		public SettingEntry<float> ReminderDuration { get; set; }

		public SettingEntry<List<string>> ReminderDisabledForEvents { get; set; }

		public ModuleSettings(SettingCollection settings)
			: base(settings, new KeyBinding((ModifierKeys)2, (Keys)69))
		{
		}//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown


		protected override void InitializeAdditionalSettings(SettingCollection settings)
		{
			EventAreaSettings = settings.AddSubCollection("event-area-settings", false);
			EventAreaNames = EventAreaSettings.DefineSetting<List<string>>("EventAreaNames", new List<string>(), (Func<string>)(() => "Event Area Names"), (Func<string>)(() => "Defines the event area names."));
		}

		protected override void DoInitializeGlobalSettings(SettingCollection globalSettingCollection)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			MapKeybinding = base.GlobalSettings.DefineSetting<KeyBinding>("MapKeybinding", new KeyBinding((Keys)77), (Func<string>)(() => "Open Map Hotkey"), (Func<string>)(() => "Defines the key used to open the fullscreen map."));
			MapKeybinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)SettingChanged<KeyBinding>);
			MapKeybinding.get_Value().set_Enabled(true);
			MapKeybinding.get_Value().set_BlockSequenceFromGw2(false);
			RemindersEnabled = base.GlobalSettings.DefineSetting<bool>("RemindersEnabled", true, (Func<string>)(() => "Reminders Enabled"), (Func<string>)(() => "Whether the drawer should display alerts before an event starts."));
			ReminderPosition = new EventReminderPositition
			{
				X = base.GlobalSettings.DefineSetting<int>("ReminderPositionX", 200, (Func<string>)(() => "Location X"), (Func<string>)(() => "Defines the position of reminders on the x axis.")),
				Y = base.GlobalSettings.DefineSetting<int>("ReminderPositionY", 200, (Func<string>)(() => "Location Y"), (Func<string>)(() => "Defines the position of reminders on the y axis."))
			};
			int reminderDurationMin = 1;
			int reminderDurationMax = 15;
			ReminderDuration = base.GlobalSettings.DefineSetting<float>("ReminderDuration", 5f, (Func<string>)(() => "Reminder Duration"), (Func<string>)(() => $"Defines the reminder duration. Min: {reminderDurationMin}s - Max: {reminderDurationMax}s"));
			SettingComplianceExtensions.SetRange(ReminderDuration, (float)reminderDurationMin, (float)reminderDurationMax);
			ReminderDisabledForEvents = base.GlobalSettings.DefineSetting<List<string>>("ReminderDisabledForEvents", new List<string>(), (Func<string>)(() => "Reminder disabled for Events"), (Func<string>)(() => "Defines the events for which NO reminder should be displayed."));
		}

		public void CheckDrawerSizeAndPosition(EventAreaConfiguration configuration)
		{
			CheckDrawerSizeAndPosition((DrawerConfiguration)configuration);
		}

		public void CheckGlobalSizeAndPosition()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			int maxResX = (int)((float)GameService.Graphics.get_Resolution().X / GameService.Graphics.get_UIScaleMultiplier());
			int maxResY = (int)((float)GameService.Graphics.get_Resolution().Y / GameService.Graphics.get_UIScaleMultiplier());
			EventReminderPositition reminderPosition = ReminderPosition;
			if (reminderPosition != null)
			{
				SettingComplianceExtensions.SetRange(reminderPosition.X, 0, maxResX);
			}
			EventReminderPositition reminderPosition2 = ReminderPosition;
			if (reminderPosition2 != null)
			{
				SettingComplianceExtensions.SetRange(reminderPosition2.Y, 0, maxResY);
			}
		}

		public EventAreaConfiguration AddDrawer(string name, List<EventCategory> eventCategories)
		{
			DrawerConfiguration drawer = AddDrawer(name);
			SettingEntry<LeftClickAction> leftClickAction = base.DrawerSettings.DefineSetting<LeftClickAction>(name + "-leftClickAction", LeftClickAction.CopyWaypoint, (Func<string>)(() => "Left Click Action"), (Func<string>)(() => "Defines the action which is executed when left clicking."));
			SettingEntry<bool> showTooltips = base.DrawerSettings.DefineSetting<bool>(name + "-showTooltips", true, (Func<string>)(() => "Show Tooltips"), (Func<string>)(() => "Whether a tooltip should be displayed when hovering."));
			SettingEntry<int> timespan = base.DrawerSettings.DefineSetting<int>(name + "-timespan", 120, (Func<string>)(() => "Timespan"), (Func<string>)(() => "Defines the timespan the event drawer covers."));
			SettingEntry<int> historySplit = base.DrawerSettings.DefineSetting<int>(name + "-historySplit", 50, (Func<string>)(() => "History Split"), (Func<string>)(() => "Defines how much history the timespan should contain."));
			SettingComplianceExtensions.SetRange(historySplit, 0, 75);
			SettingEntry<bool> drawBorders = base.DrawerSettings.DefineSetting<bool>(name + "-drawBorders", false, (Func<string>)(() => "Draw Borders"), (Func<string>)(() => "Whether the events should be rendered with borders."));
			SettingEntry<bool> useFillers = base.DrawerSettings.DefineSetting<bool>(name + "-useFillers", true, (Func<string>)(() => "Use Filler Events"), (Func<string>)(() => "Whether the empty spaces should be filled by filler events."));
			SettingEntry<Color> fillerTextColor = base.DrawerSettings.DefineSetting<Color>(name + "-fillerTextColor", base.DefaultGW2Color, (Func<string>)(() => "Filler Text Color"), (Func<string>)(() => "Defines the text color used by filler events."));
			SettingEntry<bool> acceptWaypointPrompt = base.DrawerSettings.DefineSetting<bool>(name + "-acceptWaypointPrompt", true, (Func<string>)(() => "Accept Waypoint Prompt"), (Func<string>)(() => "Whether the waypoint prompt should be accepted automatically when performing an automated teleport."));
			SettingEntry<EventCompletedAction> completionAction = base.DrawerSettings.DefineSetting<EventCompletedAction>(name + "-completionAction", EventCompletedAction.Crossout, (Func<string>)(() => "Completion Action"), (Func<string>)(() => "Defines the action to perform if an event has been completed."));
			SettingEntry<List<string>> disabledEventKeys = base.DrawerSettings.DefineSetting<List<string>>(name + "-disabledEventKeys", new List<string>(), (Func<string>)(() => "Active Event Keys"), (Func<string>)(() => "Defines the active event keys."));
			SettingEntry<int> eventHeight = base.DrawerSettings.DefineSetting<int>(name + "-eventHeight", 30, (Func<string>)(() => "Event Height"), (Func<string>)(() => "Defines the height of the individual event rows."));
			SettingComplianceExtensions.SetRange(eventHeight, 5, 30);
			SettingEntry<List<string>> eventOrder = base.DrawerSettings.DefineSetting<List<string>>(name + "-eventOrder", new List<string>(eventCategories.Select((EventCategory x) => x.Key)), (Func<string>)(() => "Event Order"), (Func<string>)(() => "Defines the order of events."));
			return new EventAreaConfiguration
			{
				Name = drawer.Name,
				Enabled = drawer.Enabled,
				EnabledKeybinding = drawer.EnabledKeybinding,
				BuildDirection = drawer.BuildDirection,
				BackgroundColor = drawer.BackgroundColor,
				FontSize = drawer.FontSize,
				TextColor = drawer.TextColor,
				Location = drawer.Location,
				Opacity = drawer.Opacity,
				Size = drawer.Size,
				LeftClickAction = leftClickAction,
				ShowTooltips = showTooltips,
				DrawBorders = drawBorders,
				HistorySplit = historySplit,
				TimeSpan = timespan,
				UseFiller = useFillers,
				FillerTextColor = fillerTextColor,
				AcceptWaypointPrompt = acceptWaypointPrompt,
				DisabledEventKeys = disabledEventKeys,
				CompletionAcion = completionAction,
				EventHeight = eventHeight,
				EventOrder = eventOrder
			};
		}

		public new void RemoveDrawer(string name)
		{
			base.RemoveDrawer(name);
			base.DrawerSettings.UndefineSetting(name + "-leftClickAction");
			base.DrawerSettings.UndefineSetting(name + "-showTooltips");
			base.DrawerSettings.UndefineSetting(name + "-timespan");
			base.DrawerSettings.UndefineSetting(name + "-historySplit");
			base.DrawerSettings.UndefineSetting(name + "-drawBorders");
			base.DrawerSettings.UndefineSetting(name + "-useFillers");
			base.DrawerSettings.UndefineSetting(name + "-fillerTextColor");
			base.DrawerSettings.UndefineSetting(name + "-acceptWaypointPrompt");
			base.DrawerSettings.UndefineSetting(name + "-completionAction");
			base.DrawerSettings.UndefineSetting(name + "-disabledEventKeys");
			base.DrawerSettings.UndefineSetting(name + "-eventHeight");
			base.DrawerSettings.UndefineSetting(name + "-eventOrder");
		}

		public override void UpdateLocalization(TranslationState translationState)
		{
			base.UpdateLocalization(translationState);
			string mapKeybindingDisplayNameDefault = ((SettingEntry)MapKeybinding).get_DisplayName();
			string mapKeybindingDescriptionDefault = ((SettingEntry)MapKeybinding).get_Description();
			((SettingEntry)MapKeybinding).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-mapKeybinding-name", mapKeybindingDisplayNameDefault)));
			((SettingEntry)MapKeybinding).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-mapKeybinding-description", mapKeybindingDescriptionDefault)));
			string remindersEnabledDisplayNameDefault = ((SettingEntry)RemindersEnabled).get_DisplayName();
			string remindersEnabledDescriptionDefault = ((SettingEntry)RemindersEnabled).get_Description();
			((SettingEntry)RemindersEnabled).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-remindersEnabled-name", remindersEnabledDisplayNameDefault)));
			((SettingEntry)RemindersEnabled).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-remindersEnabled-description", remindersEnabledDescriptionDefault)));
		}

		public void UpdateDrawerLocalization(EventAreaConfiguration drawerConfiguration, TranslationState translationState)
		{
			UpdateDrawerLocalization((DrawerConfiguration)drawerConfiguration, translationState);
			string leftClickActionDisplayNameDefault = ((SettingEntry)drawerConfiguration.LeftClickAction).get_DisplayName();
			string leftClickActionDescriptionDefault = ((SettingEntry)drawerConfiguration.LeftClickAction).get_Description();
			((SettingEntry)drawerConfiguration.LeftClickAction).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerLeftClickAction-name", leftClickActionDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.LeftClickAction).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerLeftClickAction-description", leftClickActionDescriptionDefault)));
			string showTooltipsDisplayNameDefault = ((SettingEntry)drawerConfiguration.ShowTooltips).get_DisplayName();
			string showTooltipsDescriptionDefault = ((SettingEntry)drawerConfiguration.ShowTooltips).get_Description();
			((SettingEntry)drawerConfiguration.ShowTooltips).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerShowTooltips-name", showTooltipsDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.ShowTooltips).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerShowTooltips-description", showTooltipsDescriptionDefault)));
			string timespanDisplayNameDefault = ((SettingEntry)drawerConfiguration.TimeSpan).get_DisplayName();
			string timespanDescriptionDefault = ((SettingEntry)drawerConfiguration.TimeSpan).get_Description();
			((SettingEntry)drawerConfiguration.TimeSpan).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerTimespan-name", timespanDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.TimeSpan).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerTimespan-description", timespanDescriptionDefault)));
			string historySplitDisplayNameDefault = ((SettingEntry)drawerConfiguration.HistorySplit).get_DisplayName();
			string historySplitDescriptionDefault = ((SettingEntry)drawerConfiguration.HistorySplit).get_Description();
			((SettingEntry)drawerConfiguration.HistorySplit).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerHistorySplit-name", historySplitDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HistorySplit).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerHistorySplit-description", historySplitDescriptionDefault)));
			string drawBordersDisplayNameDefault = ((SettingEntry)drawerConfiguration.DrawBorders).get_DisplayName();
			string drawBordersDescriptionDefault = ((SettingEntry)drawerConfiguration.DrawBorders).get_Description();
			((SettingEntry)drawerConfiguration.DrawBorders).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerDrawBorders-name", drawBordersDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.DrawBorders).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerDrawBorders-description", drawBordersDescriptionDefault)));
			string useFillersDisplayNameDefault = ((SettingEntry)drawerConfiguration.UseFiller).get_DisplayName();
			string useFillersDescriptionDefault = ((SettingEntry)drawerConfiguration.UseFiller).get_Description();
			((SettingEntry)drawerConfiguration.UseFiller).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerUseFillers-name", useFillersDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.UseFiller).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerUseFillers-description", useFillersDescriptionDefault)));
			string fillerTextColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.FillerTextColor).get_DisplayName();
			string fillerTextColorDescriptionDefault = ((SettingEntry)drawerConfiguration.FillerTextColor).get_Description();
			((SettingEntry)drawerConfiguration.FillerTextColor).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerFillerTextColor-name", fillerTextColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.FillerTextColor).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerFillerTextColor-description", fillerTextColorDescriptionDefault)));
			string acceptWaypointPromptDisplayNameDefault = ((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).get_DisplayName();
			string acceptWaypointPromptDescriptionDefault = ((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).get_Description();
			((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerAcceptWaypointPrompt-name", acceptWaypointPromptDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerAcceptWaypointPrompt-description", acceptWaypointPromptDescriptionDefault)));
			string completionActionDisplayNameDefault = ((SettingEntry)drawerConfiguration.CompletionAcion).get_DisplayName();
			string completionActionDescriptionDefault = ((SettingEntry)drawerConfiguration.CompletionAcion).get_Description();
			((SettingEntry)drawerConfiguration.CompletionAcion).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerCompletionAction-name", completionActionDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.CompletionAcion).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerCompletionAction-description", completionActionDescriptionDefault)));
			string disabledEventKeysDisplayNameDefault = ((SettingEntry)drawerConfiguration.DisabledEventKeys).get_DisplayName();
			string disabledEventKeysDescriptionDefault = ((SettingEntry)drawerConfiguration.DisabledEventKeys).get_Description();
			((SettingEntry)drawerConfiguration.DisabledEventKeys).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerDisabledEventKeys-name", disabledEventKeysDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.DisabledEventKeys).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerDisabledEventKeys-description", disabledEventKeysDescriptionDefault)));
			string eventHeightDisplayNameDefault = ((SettingEntry)drawerConfiguration.EventHeight).get_DisplayName();
			string eventHeightDescriptionDefault = ((SettingEntry)drawerConfiguration.EventHeight).get_Description();
			((SettingEntry)drawerConfiguration.EventHeight).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEventHeight-name", eventHeightDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EventHeight).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEventHeight-description", eventHeightDescriptionDefault)));
			string eventOrderDisplayNameDefault = ((SettingEntry)drawerConfiguration.EventOrder).get_DisplayName();
			string eventOrderDescriptionDefault = ((SettingEntry)drawerConfiguration.EventOrder).get_Description();
			((SettingEntry)drawerConfiguration.EventOrder).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEventOrder-name", eventOrderDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EventOrder).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEventOrder-description", eventOrderDescriptionDefault)));
		}
	}
}
