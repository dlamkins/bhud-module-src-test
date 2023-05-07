using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Models.Drawers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Input;

namespace Estreya.BlishHUD.EventTable
{
	public class ModuleSettings : BaseModuleSettings
	{
		private const string EVENT_AREA_SETTINGS = "event-area-settings";

		private SettingCollection EventAreaSettings { get; set; }

		public SettingEntry<List<string>> EventAreaNames { get; private set; }

		public SettingEntry<KeyBinding> MapKeybinding { get; private set; }

		public SettingEntry<bool> RemindersEnabled { get; private set; }

		public EventReminderPositition ReminderPosition { get; private set; }

		public SettingEntry<float> ReminderDuration { get; private set; }

		public SettingEntry<float> ReminderOpacity { get; private set; }

		public SettingEntry<List<string>> ReminderDisabledForEvents { get; private set; }

		public SettingEntry<Dictionary<string, List<TimeSpan>>> ReminderTimesOverride { get; private set; }

		public SettingEntry<bool> ShowDynamicEventsOnMap { get; private set; }

		public SettingEntry<bool> ShowDynamicEventInWorld { get; private set; }

		public SettingEntry<bool> ShowDynamicEventsInWorldOnlyWhenInside { get; private set; }

		public SettingEntry<bool> IgnoreZAxisOnDynamicEventsInWorld { get; private set; }

		public SettingEntry<int> DynamicEventsRenderDistance { get; private set; }

		public SettingEntry<List<string>> DisabledDynamicEventIds { get; private set; }

		public SettingEntry<MenuEventSortMode> MenuEventSortMode { get; private set; }

		public SettingEntry<bool> HideRemindersOnMissingMumbleTicks { get; private set; }

		public SettingEntry<bool> HideRemindersInCombat { get; private set; }

		public SettingEntry<bool> HideRemindersOnOpenMap { get; private set; }

		public SettingEntry<bool> HideRemindersInPvE_OpenWorld { get; private set; }

		public SettingEntry<bool> HideRemindersInPvE_Competetive { get; private set; }

		public SettingEntry<bool> HideRemindersInWvW { get; private set; }

		public SettingEntry<bool> HideRemindersInPvP { get; private set; }

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
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			MapKeybinding = base.GlobalSettings.DefineSetting<KeyBinding>("MapKeybinding", new KeyBinding((Keys)77), (Func<string>)(() => "Open Map Hotkey"), (Func<string>)(() => "Defines the key used to open the fullscreen map."));
			MapKeybinding.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)SettingChanged<KeyBinding>);
			MapKeybinding.get_Value().set_Enabled(true);
			MapKeybinding.get_Value().set_BlockSequenceFromGw2(false);
			RemindersEnabled = base.GlobalSettings.DefineSetting<bool>("RemindersEnabled", true, (Func<string>)(() => "Reminders Enabled"), (Func<string>)(() => "Whether the module should display alerts before an event starts."));
			ReminderPosition = new EventReminderPositition
			{
				X = base.GlobalSettings.DefineSetting<int>("ReminderPositionX", 200, (Func<string>)(() => "Location X"), (Func<string>)(() => "Defines the position of reminders on the x axis.")),
				Y = base.GlobalSettings.DefineSetting<int>("ReminderPositionY", 200, (Func<string>)(() => "Location Y"), (Func<string>)(() => "Defines the position of reminders on the y axis."))
			};
			int reminderDurationMin = 1;
			int reminderDurationMax = 15;
			ReminderDuration = base.GlobalSettings.DefineSetting<float>("ReminderDuration", 5f, (Func<string>)(() => "Reminder Duration"), (Func<string>)(() => "Defines the reminder duration."));
			SettingComplianceExtensions.SetRange(ReminderDuration, (float)reminderDurationMin, (float)reminderDurationMax);
			ReminderDisabledForEvents = base.GlobalSettings.DefineSetting<List<string>>("ReminderDisabledForEvents", new List<string>(), (Func<string>)(() => "Reminder disabled for Events"), (Func<string>)(() => "Defines the events for which NO reminder should be displayed."));
			ReminderTimesOverride = base.GlobalSettings.DefineSetting<Dictionary<string, List<TimeSpan>>>("ReminderTimesOverride", new Dictionary<string, List<TimeSpan>>(), (Func<string>)(() => "Reminder Times Override"), (Func<string>)(() => "Defines the overridden times for reminders per event."));
			ReminderOpacity = base.GlobalSettings.DefineSetting<float>("ReminderOpacity", 0.5f, (Func<string>)(() => "Reminder Opacity"), (Func<string>)(() => "Defines the background opacity for reminders."));
			SettingComplianceExtensions.SetRange(ReminderOpacity, 0.1f, 1f);
			ShowDynamicEventsOnMap = base.GlobalSettings.DefineSetting<bool>("ShowDynamicEventsOnMap", false, (Func<string>)(() => "Show Dynamic Events on Map"), (Func<string>)(() => "Whether the dynamic events of the map should be shown."));
			ShowDynamicEventInWorld = base.GlobalSettings.DefineSetting<bool>("ShowDynamicEventInWorld", false, (Func<string>)(() => "Show Dynamic Events in World"), (Func<string>)(() => "Whether dynamic events should be shown inside the world."));
			ShowDynamicEventInWorld.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventInWorld_SettingChanged);
			ShowDynamicEventsInWorldOnlyWhenInside = base.GlobalSettings.DefineSetting<bool>("ShowDynamicEventsInWorldOnlyWhenInside", true, (Func<string>)(() => "Show only when inside"), (Func<string>)(() => "Whether the dynamic events inside the world should only show up when the player is inside."));
			ShowDynamicEventsInWorldOnlyWhenInside.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventsInWorldOnlyWhenInside_SettingChanged);
			IgnoreZAxisOnDynamicEventsInWorld = base.GlobalSettings.DefineSetting<bool>("IgnoreZAxisOnDynamicEventsInWorld", true, (Func<string>)(() => "Ignore Z Axis"), (Func<string>)(() => "Defines whether the z axis should be ignored when calculating the visibility of in world events."));
			DynamicEventsRenderDistance = base.GlobalSettings.DefineSetting<int>("DynamicEventsRenderDistance", 300, (Func<string>)(() => "Dynamic Event Render Distance"), (Func<string>)(() => "Defines the distance in which dynamic events should be rendered."));
			SettingComplianceExtensions.SetRange(DynamicEventsRenderDistance, 50, 500);
			DisabledDynamicEventIds = base.GlobalSettings.DefineSetting<List<string>>("DisabledDynamicEventIds", new List<string>(), (Func<string>)(() => "Disabled Dynamic Events"), (Func<string>)(() => "Defines which dynamic events are disabled."));
			MenuEventSortMode = base.GlobalSettings.DefineSetting<MenuEventSortMode>("MenuEventSortMode", Estreya.BlishHUD.EventTable.Models.MenuEventSortMode.Default, (Func<string>)(() => "Menu Event Sort Mode"), (Func<string>)(() => "Defines the mode by which the events in menu views are sorted by."));
			HideRemindersOnOpenMap = base.GlobalSettings.DefineSetting<bool>("HideRemindersOnOpenMap", false, (Func<string>)(() => "Hide Reminders on open Map"), (Func<string>)(() => "Whether the reminders should hide when the map is open."));
			HideRemindersOnMissingMumbleTicks = base.GlobalSettings.DefineSetting<bool>("HideRemindersOnMissingMumbleTicks", true, (Func<string>)(() => "Hide Reminders on Cutscenes"), (Func<string>)(() => "Whether the reminders should hide when cutscenes are played."));
			HideRemindersInCombat = base.GlobalSettings.DefineSetting<bool>("HideRemindersInCombat", false, (Func<string>)(() => "Hide Reminders in Combat"), (Func<string>)(() => "Whether the reminders should hide when in combat."));
			HideRemindersInPvE_OpenWorld = base.GlobalSettings.DefineSetting<bool>("HideRemindersInPvE_OpenWorld", false, (Func<string>)(() => "Hide Reminders in PvE (Open World)"), (Func<string>)(() => "Whether the reminders should hide when in PvE (Open World)."));
			HideRemindersInPvE_Competetive = base.GlobalSettings.DefineSetting<bool>("HideRemindersInPvE_Competetive", false, (Func<string>)(() => "Hide Reminders in PvE (Competetive)"), (Func<string>)(() => "Whether the reminders should hide when in PvE (Competetive)."));
			HideRemindersInWvW = base.GlobalSettings.DefineSetting<bool>("HideRemindersInWvW", false, (Func<string>)(() => "Hide Reminders in WvW"), (Func<string>)(() => "Whether the reminders should hide when in world vs. world."));
			HideRemindersInPvP = base.GlobalSettings.DefineSetting<bool>("HideRemindersInPvP", false, (Func<string>)(() => "Hide Reminders in PvP"), (Func<string>)(() => "Whether the reminders should hide when in player vs. player."));
			HandleEnabledStates();
		}

		private void ShowDynamicEventInWorld_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			HandleEnabledStates();
		}

		private void ShowDynamicEventsInWorldOnlyWhenInside_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			HandleEnabledStates();
		}

		private void HandleEnabledStates()
		{
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)ShowDynamicEventsInWorldOnlyWhenInside, !ShowDynamicEventInWorld.get_Value());
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)IgnoreZAxisOnDynamicEventsInWorld, !ShowDynamicEventInWorld.get_Value() || !ShowDynamicEventsInWorldOnlyWhenInside.get_Value());
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)DynamicEventsRenderDistance, !ShowDynamicEventInWorld.get_Value() || ShowDynamicEventsInWorldOnlyWhenInside.get_Value());
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
			int num = (int)((float)GameService.Graphics.get_Resolution().Y / GameService.Graphics.get_UIScaleMultiplier());
			int minLocationX = 0;
			int maxLocationX = maxResX - 350;
			int minLocationY = 0;
			int maxLocationY = num - 96;
			EventReminderPositition reminderPosition = ReminderPosition;
			if (reminderPosition != null)
			{
				SettingComplianceExtensions.SetRange(reminderPosition.X, minLocationX, maxLocationX);
			}
			EventReminderPositition reminderPosition2 = ReminderPosition;
			if (reminderPosition2 != null)
			{
				SettingComplianceExtensions.SetRange(reminderPosition2.Y, minLocationY, maxLocationY);
			}
		}

		public EventAreaConfiguration AddDrawer(string name, List<EventCategory> eventCategories)
		{
			DrawerConfiguration drawer = AddDrawer(name);
			SettingEntry<LeftClickAction> leftClickAction = base.DrawerSettings.DefineSetting<LeftClickAction>(name + "-leftClickAction", LeftClickAction.CopyWaypoint, (Func<string>)(() => "Left Click Action"), (Func<string>)(() => "Defines the action which is executed when left clicking."));
			SettingEntry<bool> showTooltips = base.DrawerSettings.DefineSetting<bool>(name + "-showTooltips", true, (Func<string>)(() => "Show Tooltips"), (Func<string>)(() => "Whether a tooltip should be displayed when hovering."));
			SettingEntry<int> timespan = base.DrawerSettings.DefineSetting<int>(name + "-timespan", 120, (Func<string>)(() => "Timespan"), (Func<string>)(() => "Defines the timespan the event drawer covers."));
			SettingComplianceExtensions.SetRange(timespan, 60, 240);
			SettingEntry<int> historySplit = base.DrawerSettings.DefineSetting<int>(name + "-historySplit", 50, (Func<string>)(() => "History Split"), (Func<string>)(() => "Defines how much history the timespan should contain."));
			SettingComplianceExtensions.SetRange(historySplit, 0, 75);
			SettingEntry<bool> enableHistorySplitScrolling = base.DrawerSettings.DefineSetting<bool>(name + "-enableHistorySplitScrolling", false, (Func<string>)(() => "Enable History Split Scrolling"), (Func<string>)(() => "Defines if scrolling inside the event area temporary moves the history split until the mouse leaves the area."));
			SettingEntry<int> historySplitScrollingSpeed = base.DrawerSettings.DefineSetting<int>(name + "-historySplitScrollingSpeed", 1, (Func<string>)(() => "History Split Scrolling Speed"), (Func<string>)(() => "Defines the speed when scrolling inside the event area."));
			SettingComplianceExtensions.SetRange(historySplitScrollingSpeed, 1, 10);
			SettingEntry<bool> drawBorders = base.DrawerSettings.DefineSetting<bool>(name + "-drawBorders", false, (Func<string>)(() => "Draw Borders"), (Func<string>)(() => "Whether the events should be rendered with borders."));
			SettingEntry<bool> useFillers = base.DrawerSettings.DefineSetting<bool>(name + "-useFillers", true, (Func<string>)(() => "Use Filler Events"), (Func<string>)(() => "Whether the empty spaces should be filled by filler events."));
			SettingEntry<Color> fillerTextColor = base.DrawerSettings.DefineSetting<Color>(name + "-fillerTextColor", base.DefaultGW2Color, (Func<string>)(() => "Filler Text Color"), (Func<string>)(() => "Defines the text color used by filler events."));
			SettingEntry<bool> acceptWaypointPrompt = base.DrawerSettings.DefineSetting<bool>(name + "-acceptWaypointPrompt", true, (Func<string>)(() => "Accept Waypoint Prompt"), (Func<string>)(() => "Whether the waypoint prompt should be accepted automatically when performing an automated teleport."));
			SettingEntry<EventCompletedAction> completionAction = base.DrawerSettings.DefineSetting<EventCompletedAction>(name + "-completionAction", EventCompletedAction.Crossout, (Func<string>)(() => "Completion Action"), (Func<string>)(() => "Defines the action to perform if an event has been completed."));
			SettingEntry<List<string>> disabledEventKeys = base.DrawerSettings.DefineSetting<List<string>>(name + "-disabledEventKeys", new List<string>(), (Func<string>)(() => "Active Event Keys"), (Func<string>)(() => "Defines the active event keys."));
			SettingEntry<int> eventHeight = base.DrawerSettings.DefineSetting<int>(name + "-eventHeight", 30, (Func<string>)(() => "Event Height"), (Func<string>)(() => "Defines the height of the individual event rows."));
			SettingComplianceExtensions.SetRange(eventHeight, 5, 30);
			SettingEntry<List<string>> eventOrder = base.DrawerSettings.DefineSetting<List<string>>(name + "-eventOrder", new List<string>(eventCategories.Select((EventCategory x) => x.Key)), (Func<string>)(() => "Event Order"), (Func<string>)(() => "Defines the order of events."));
			SettingEntry<float> eventBackgroundOpacity = base.DrawerSettings.DefineSetting<float>(name + "-eventBackgroundOpacity", 1f, (Func<string>)(() => "Event Background Opacity"), (Func<string>)(() => "Defines the opacity of the individual event backgrounds."));
			SettingComplianceExtensions.SetRange(eventBackgroundOpacity, 0.1f, 1f);
			SettingEntry<bool> drawShadows = base.DrawerSettings.DefineSetting<bool>(name + "-drawShadows", false, (Func<string>)(() => "Draw Shadows"), (Func<string>)(() => "Whether the text should have shadows"));
			SettingEntry<Color> shadowColor = base.DrawerSettings.DefineSetting<Color>(name + "-shadowColor", base.DefaultGW2Color, (Func<string>)(() => "Shadow Color"), (Func<string>)(() => "Defines the color of the shadows"));
			SettingEntry<bool> drawShadowsForFiller = base.DrawerSettings.DefineSetting<bool>(name + "-drawShadowsForFiller", false, (Func<string>)(() => "Draw Shadows for Filler"), (Func<string>)(() => "Whether the filler text should have shadows"));
			SettingEntry<Color> fillerShadowColor = base.DrawerSettings.DefineSetting<Color>(name + "-fillerShadowColor", base.DefaultGW2Color, (Func<string>)(() => "Filler Shadow Color"), (Func<string>)(() => "Defines the color of the shadows for fillers"));
			SettingEntry<DrawInterval> drawInterval = base.DrawerSettings.DefineSetting<DrawInterval>(name + "-drawInterval", DrawInterval.FAST, (Func<string>)(() => "Draw Interval"), (Func<string>)(() => "Defines the refresh rate of the drawer."));
			SettingEntry<bool> limitToCurrentMap = base.DrawerSettings.DefineSetting<bool>(name + "-limitToCurrentMap", false, (Func<string>)(() => "Limit to current Map"), (Func<string>)(() => "Whether the drawer should only show events from the current map."));
			SettingEntry<bool> allowUnspecifiedMap = base.DrawerSettings.DefineSetting<bool>(name + "-allowUnspecifiedMap", true, (Func<string>)(() => "Allow from unspecified Maps"), (Func<string>)(() => "Whether the table should show events which do not have a map id specified."));
			SettingEntry<float> timeLineOpacity = base.DrawerSettings.DefineSetting<float>(name + "-timeLineOpacity", 1f, (Func<string>)(() => "Timeline Opacity"), (Func<string>)(() => "Defines the opacity of the time line bar."));
			SettingComplianceExtensions.SetRange(timeLineOpacity, 0.1f, 1f);
			SettingEntry<float> eventTextOpacity = base.DrawerSettings.DefineSetting<float>(name + "-eventTextOpacity", 1f, (Func<string>)(() => "Event Text Opacity"), (Func<string>)(() => "Defines the opacity of the event text."));
			SettingComplianceExtensions.SetRange(eventTextOpacity, 0.1f, 1f);
			SettingEntry<float> fillerTextOpacity = base.DrawerSettings.DefineSetting<float>(name + "-fillerTextOpacity", 1f, (Func<string>)(() => "Filler Text Opacity"), (Func<string>)(() => "Defines the opacity of filler event text."));
			SettingComplianceExtensions.SetRange(fillerTextOpacity, 0.1f, 1f);
			SettingEntry<float> shadowOpacity = base.DrawerSettings.DefineSetting<float>(name + "-shadowOpacity", 1f, (Func<string>)(() => "Shadow Opacity"), (Func<string>)(() => "Defines the opacity for shadows."));
			SettingComplianceExtensions.SetRange(shadowOpacity, 0.1f, 1f);
			SettingEntry<float> fillerShadowOpacity = base.DrawerSettings.DefineSetting<float>(name + "-fillerShadowOpacity", 1f, (Func<string>)(() => "Filler Shadow Opacity"), (Func<string>)(() => "Defines the opacity for filler shadows."));
			SettingComplianceExtensions.SetRange(fillerShadowOpacity, 0.1f, 1f);
			SettingEntry<float> completedEventsBackgroundOpacity = base.DrawerSettings.DefineSetting<float>(name + "-completedEventsBackgroundOpacity", 0.5f, (Func<string>)(() => "Completed Events Background Opacity"), (Func<string>)(() => "Defines the background opacity of completed events. Only works in combination with CompletionAction = Change Opacity"));
			SettingComplianceExtensions.SetRange(completedEventsBackgroundOpacity, 0.1f, 0.9f);
			SettingEntry<float> completedEventsTextOpacity = base.DrawerSettings.DefineSetting<float>(name + "-completedEventsTextOpacity", 1f, (Func<string>)(() => "Completed Events Text Opacity"), (Func<string>)(() => "Defines the text opacity of completed events. Only works in combination with CompletionAction = Change Opacity"));
			SettingComplianceExtensions.SetRange(completedEventsBackgroundOpacity, 0f, 1f);
			SettingEntry<bool> completedEventsInvertTextColor = base.DrawerSettings.DefineSetting<bool>(name + "-completedEventsInvertTextColor", true, (Func<string>)(() => "Completed Events Invert Textcolor"), (Func<string>)(() => "Specified if completed events should have their text color inverted. Only works in combination with CompletionAction = Change Opacity"));
			SettingEntry<bool> hideOnOpenMap = base.DrawerSettings.DefineSetting<bool>(name + "-hideOnOpenMap", true, (Func<string>)(() => "Hide on open Map"), (Func<string>)(() => "Whether the area should hide when the map is open."));
			SettingEntry<bool> hideOnMissingMumbleTicks = base.DrawerSettings.DefineSetting<bool>(name + "-hideOnMissingMumbleTicks", true, (Func<string>)(() => "Hide on Cutscenes"), (Func<string>)(() => "Whether the area should hide when cutscenes are played."));
			SettingEntry<bool> hideInCombat = base.DrawerSettings.DefineSetting<bool>(name + "-hideInCombat", false, (Func<string>)(() => "Hide in Combat"), (Func<string>)(() => "Whether the area should hide when in combat."));
			SettingEntry<bool> hideInPvE_OpenWorld = base.DrawerSettings.DefineSetting<bool>(name + "-hideInPvE_OpenWorld", false, (Func<string>)(() => "Hide in PvE (Open World)"), (Func<string>)(() => "Whether the area should hide when in PvE (Open World)."));
			SettingEntry<bool> hideInPvE_Competetive = base.DrawerSettings.DefineSetting<bool>(name + "-hideInPvE_Competetive", false, (Func<string>)(() => "Hide in PvE (Competetive)"), (Func<string>)(() => "Whether the area should hide when in PvE (Competetive)."));
			SettingEntry<bool> hideInWvW = base.DrawerSettings.DefineSetting<bool>(name + "-hideInWvW", false, (Func<string>)(() => "Hide in WvW"), (Func<string>)(() => "Whether the area should hide when in world vs. world."));
			SettingEntry<bool> hideInPvP = base.DrawerSettings.DefineSetting<bool>(name + "-hideInPvP", false, (Func<string>)(() => "Hide in PvP"), (Func<string>)(() => "Whether the area should hide when in player vs. player."));
			SettingEntry<bool> showCategoryNames = base.DrawerSettings.DefineSetting<bool>(name + "-showCategoryNames", false, (Func<string>)(() => "Show Category Names"), (Func<string>)(() => "Defines if the category names should be shown before the event bars."));
			SettingEntry<Color> categoryNameColor = base.DrawerSettings.DefineSetting<Color>(name + "-categoryNameColor", base.DefaultGW2Color, (Func<string>)(() => "Category Name Color"), (Func<string>)(() => "Defines the color of the category names."));
			SettingEntry<bool> enableColorGradients = base.DrawerSettings.DefineSetting<bool>(name + "-enableColorGradients", false, (Func<string>)(() => "Enable Color Gradients"), (Func<string>)(() => "Defines if supported events should have a smoother color gradient from and to the next event."));
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
				EnableHistorySplitScrolling = enableHistorySplitScrolling,
				HistorySplitScrollingSpeed = historySplitScrollingSpeed,
				TimeSpan = timespan,
				UseFiller = useFillers,
				FillerTextColor = fillerTextColor,
				AcceptWaypointPrompt = acceptWaypointPrompt,
				DisabledEventKeys = disabledEventKeys,
				CompletionAction = completionAction,
				EventHeight = eventHeight,
				EventOrder = eventOrder,
				EventBackgroundOpacity = eventBackgroundOpacity,
				DrawShadows = drawShadows,
				ShadowColor = shadowColor,
				DrawShadowsForFiller = drawShadowsForFiller,
				FillerShadowColor = fillerShadowColor,
				DrawInterval = drawInterval,
				LimitToCurrentMap = limitToCurrentMap,
				AllowUnspecifiedMap = allowUnspecifiedMap,
				TimeLineOpacity = timeLineOpacity,
				EventTextOpacity = eventTextOpacity,
				FillerTextOpacity = fillerTextOpacity,
				ShadowOpacity = shadowOpacity,
				FillerShadowOpacity = fillerShadowOpacity,
				CompletedEventsBackgroundOpacity = completedEventsBackgroundOpacity,
				CompletedEventsTextOpacity = completedEventsTextOpacity,
				CompletedEventsInvertTextColor = completedEventsInvertTextColor,
				HideInCombat = hideInCombat,
				HideOnMissingMumbleTicks = hideOnMissingMumbleTicks,
				HideOnOpenMap = hideOnOpenMap,
				HideInPvE_Competetive = hideInPvE_Competetive,
				HideInPvE_OpenWorld = hideInPvE_OpenWorld,
				HideInPvP = hideInPvP,
				HideInWvW = hideInWvW,
				ShowCategoryNames = showCategoryNames,
				CategoryNameColor = categoryNameColor,
				EnableColorGradients = enableColorGradients
			};
		}

		public void CheckDrawerSettings(EventAreaConfiguration configuration, List<EventCategory> categories)
		{
			Dictionary<int, EventCategory> notOrderedEventCategories = categories.Where((EventCategory ec) => !configuration.EventOrder.get_Value().Contains(ec.Key)).ToDictionary((EventCategory ec) => categories.IndexOf(ec), (EventCategory ec) => ec);
			foreach (KeyValuePair<int, EventCategory> notOrderedEventCategory in notOrderedEventCategories)
			{
				configuration.EventOrder.get_Value().Insert(notOrderedEventCategory.Key, notOrderedEventCategory.Value.Key);
			}
			if (notOrderedEventCategories.Count > 0)
			{
				configuration.EventOrder.set_Value(new List<string>(configuration.EventOrder.get_Value()));
			}
		}

		public new void RemoveDrawer(string name)
		{
			base.RemoveDrawer(name);
			base.DrawerSettings.UndefineSetting(name + "-leftClickAction");
			base.DrawerSettings.UndefineSetting(name + "-showTooltips");
			base.DrawerSettings.UndefineSetting(name + "-timespan");
			base.DrawerSettings.UndefineSetting(name + "-historySplit");
			base.DrawerSettings.UndefineSetting(name + "-enableHistorySplitScrolling");
			base.DrawerSettings.UndefineSetting(name + "-historySplitScrollingSpeed");
			base.DrawerSettings.UndefineSetting(name + "-drawBorders");
			base.DrawerSettings.UndefineSetting(name + "-useFillers");
			base.DrawerSettings.UndefineSetting(name + "-fillerTextColor");
			base.DrawerSettings.UndefineSetting(name + "-acceptWaypointPrompt");
			base.DrawerSettings.UndefineSetting(name + "-completionAction");
			base.DrawerSettings.UndefineSetting(name + "-disabledEventKeys");
			base.DrawerSettings.UndefineSetting(name + "-eventHeight");
			base.DrawerSettings.UndefineSetting(name + "-eventOrder");
			base.DrawerSettings.UndefineSetting(name + "-eventBackgroundOpacity");
			base.DrawerSettings.UndefineSetting(name + "-drawShadows");
			base.DrawerSettings.UndefineSetting(name + "-shadowColor");
			base.DrawerSettings.UndefineSetting(name + "-drawShadowsForFiller");
			base.DrawerSettings.UndefineSetting(name + "-fillerShadowColor");
			base.DrawerSettings.UndefineSetting(name + "-drawInterval");
			base.DrawerSettings.UndefineSetting(name + "-limitToCurrentMap");
			base.DrawerSettings.UndefineSetting(name + "-allowUnspecifiedMap");
			base.DrawerSettings.UndefineSetting(name + "-timeLineOpacity");
			base.DrawerSettings.UndefineSetting(name + "-eventTextOpacity");
			base.DrawerSettings.UndefineSetting(name + "-fillerTextOpacity");
			base.DrawerSettings.UndefineSetting(name + "-shadowOpacity");
			base.DrawerSettings.UndefineSetting(name + "-fillerShadowOpacity");
			base.DrawerSettings.UndefineSetting(name + "-completedEventsBackgroundOpacity");
			base.DrawerSettings.UndefineSetting(name + "-completedEventsTextOpacity");
			base.DrawerSettings.UndefineSetting(name + "-completedEventsInvertTextColor");
			base.DrawerSettings.UndefineSetting(name + "-hideOnOpenMap");
			base.DrawerSettings.UndefineSetting(name + "-hideOnMissingMumbleTicks");
			base.DrawerSettings.UndefineSetting(name + "-hideInCombat");
			base.DrawerSettings.UndefineSetting(name + "-hideInPvE_OpenWorld");
			base.DrawerSettings.UndefineSetting(name + "-hideInPvE_Competetive");
			base.DrawerSettings.UndefineSetting(name + "-hideInWvW");
			base.DrawerSettings.UndefineSetting(name + "-hideInPvP");
			base.DrawerSettings.UndefineSetting(name + "-showCategoryNames");
			base.DrawerSettings.UndefineSetting(name + "-categoryNameColor");
			base.DrawerSettings.UndefineSetting(name + "-enableColorGradients");
		}

		public override void UpdateLocalization(TranslationService translationService)
		{
			base.UpdateLocalization(translationService);
			string mapKeybindingDisplayNameDefault = ((SettingEntry)MapKeybinding).get_DisplayName();
			string mapKeybindingDescriptionDefault = ((SettingEntry)MapKeybinding).get_Description();
			((SettingEntry)MapKeybinding).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-mapKeybinding-name", mapKeybindingDisplayNameDefault)));
			((SettingEntry)MapKeybinding).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-mapKeybinding-description", mapKeybindingDescriptionDefault)));
			string remindersEnabledDisplayNameDefault = ((SettingEntry)RemindersEnabled).get_DisplayName();
			string remindersEnabledDescriptionDefault = ((SettingEntry)RemindersEnabled).get_Description();
			((SettingEntry)RemindersEnabled).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-remindersEnabled-name", remindersEnabledDisplayNameDefault)));
			((SettingEntry)RemindersEnabled).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-remindersEnabled-description", remindersEnabledDescriptionDefault)));
			string reminderPositionXDisplayNameDefault = ((SettingEntry)ReminderPosition.X).get_DisplayName();
			string reminderPositionXDescriptionDefault = ((SettingEntry)ReminderPosition.X).get_Description();
			((SettingEntry)ReminderPosition.X).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderPositionX-name", reminderPositionXDisplayNameDefault)));
			((SettingEntry)ReminderPosition.X).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderPositionX-description", reminderPositionXDescriptionDefault)));
			string reminderPositionYDisplayNameDefault = ((SettingEntry)ReminderPosition.Y).get_DisplayName();
			string reminderPositionYDescriptionDefault = ((SettingEntry)ReminderPosition.Y).get_Description();
			((SettingEntry)ReminderPosition.Y).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderPositionY-name", reminderPositionYDisplayNameDefault)));
			((SettingEntry)ReminderPosition.Y).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderPositionY-description", reminderPositionYDescriptionDefault)));
			string reminderDurationDisplayNameDefault = ((SettingEntry)ReminderDuration).get_DisplayName();
			string reminderDurationDescriptionDefault = ((SettingEntry)ReminderDuration).get_Description();
			((SettingEntry)ReminderDuration).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderDuration-name", reminderDurationDisplayNameDefault)));
			((SettingEntry)ReminderDuration).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderDuration-description", reminderDurationDescriptionDefault)));
			string reminderOpacityDisplayNameDefault = ((SettingEntry)ReminderOpacity).get_DisplayName();
			string reminderOpacityDescriptionDefault = ((SettingEntry)ReminderOpacity).get_Description();
			((SettingEntry)ReminderOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderOpacity-name", reminderOpacityDisplayNameDefault)));
			((SettingEntry)ReminderOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-reminderOpacity-description", reminderOpacityDescriptionDefault)));
			string showDynamicEventsOnMapDisplayNameDefault = ((SettingEntry)ShowDynamicEventsOnMap).get_DisplayName();
			string showDynamicEventsOnMapDescriptionDefault = ((SettingEntry)ShowDynamicEventsOnMap).get_Description();
			((SettingEntry)ShowDynamicEventsOnMap).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-showDynamicEventsOnMap-name", showDynamicEventsOnMapDisplayNameDefault)));
			((SettingEntry)ShowDynamicEventsOnMap).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-showDynamicEventsOnMap-description", showDynamicEventsOnMapDescriptionDefault)));
			string showDynamicEventInWorldDisplayNameDefault = ((SettingEntry)ShowDynamicEventInWorld).get_DisplayName();
			string showDynamicEventInWorldDescriptionDefault = ((SettingEntry)ShowDynamicEventInWorld).get_Description();
			((SettingEntry)ShowDynamicEventInWorld).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-showDynamicEventInWorld-name", showDynamicEventInWorldDisplayNameDefault)));
			((SettingEntry)ShowDynamicEventInWorld).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-showDynamicEventInWorld-description", showDynamicEventInWorldDescriptionDefault)));
			string showDynamicEventsInWorldOnlyWhenInsideDisplayNameDefault = ((SettingEntry)ShowDynamicEventsInWorldOnlyWhenInside).get_DisplayName();
			string showDynamicEventsInWorldOnlyWhenInsideDescriptionDefault = ((SettingEntry)ShowDynamicEventsInWorldOnlyWhenInside).get_Description();
			((SettingEntry)ShowDynamicEventsInWorldOnlyWhenInside).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-showDynamicEventsInWorldOnlyWhenInside-name", showDynamicEventsInWorldOnlyWhenInsideDisplayNameDefault)));
			((SettingEntry)ShowDynamicEventsInWorldOnlyWhenInside).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-showDynamicEventsInWorldOnlyWhenInside-description", showDynamicEventsInWorldOnlyWhenInsideDescriptionDefault)));
			string ignoreZAxisOnDynamicEventsInWorldDisplayNameDefault = ((SettingEntry)IgnoreZAxisOnDynamicEventsInWorld).get_DisplayName();
			string ignoreZAxisOnDynamicEventsInWorldDescriptionDefault = ((SettingEntry)IgnoreZAxisOnDynamicEventsInWorld).get_Description();
			((SettingEntry)IgnoreZAxisOnDynamicEventsInWorld).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-ignoreZAxisOnDynamicEventsInWorld-name", ignoreZAxisOnDynamicEventsInWorldDisplayNameDefault)));
			((SettingEntry)IgnoreZAxisOnDynamicEventsInWorld).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-ignoreZAxisOnDynamicEventsInWorld-description", ignoreZAxisOnDynamicEventsInWorldDescriptionDefault)));
			string dynamicEventsRenderDistanceDisplayNameDefault = ((SettingEntry)DynamicEventsRenderDistance).get_DisplayName();
			string dynamicEventsRenderDistanceDescriptionDefault = ((SettingEntry)DynamicEventsRenderDistance).get_Description();
			((SettingEntry)DynamicEventsRenderDistance).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-dynamicEventsRenderDistance-name", dynamicEventsRenderDistanceDisplayNameDefault)));
			((SettingEntry)DynamicEventsRenderDistance).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-dynamicEventsRenderDistance-description", dynamicEventsRenderDistanceDescriptionDefault)));
			string menuEventSortModeDisplayNameDefault = ((SettingEntry)MenuEventSortMode).get_DisplayName();
			string menuEventSortModeDescriptionDefault = ((SettingEntry)MenuEventSortMode).get_Description();
			((SettingEntry)MenuEventSortMode).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-menuEventSortMode-name", menuEventSortModeDisplayNameDefault)));
			((SettingEntry)MenuEventSortMode).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-menuEventSortMode-description", menuEventSortModeDescriptionDefault)));
			string hideRemindersOnOpenMapDisplayNameDefault = ((SettingEntry)HideRemindersOnOpenMap).get_DisplayName();
			string hideRemindersOnOpenMapDescriptionDefault = ((SettingEntry)HideRemindersOnOpenMap).get_Description();
			((SettingEntry)HideRemindersOnOpenMap).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersOnOpenMap-name", hideRemindersOnOpenMapDisplayNameDefault)));
			((SettingEntry)HideRemindersOnOpenMap).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersOnOpenMap-description", hideRemindersOnOpenMapDescriptionDefault)));
			string hideRemindersOnMissingMumbleTicksDisplayNameDefault = ((SettingEntry)HideRemindersOnMissingMumbleTicks).get_DisplayName();
			string hideRemindersOnMissingMumbleTicksDescriptionDefault = ((SettingEntry)HideRemindersOnMissingMumbleTicks).get_Description();
			((SettingEntry)HideRemindersOnMissingMumbleTicks).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersOnMissingMumbleTicks-name", hideRemindersOnMissingMumbleTicksDisplayNameDefault)));
			((SettingEntry)HideRemindersOnMissingMumbleTicks).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersOnMissingMumbleTicks-description", hideRemindersOnMissingMumbleTicksDescriptionDefault)));
			string hideRemindersInCombatDisplayNameDefault = ((SettingEntry)HideRemindersInCombat).get_DisplayName();
			string hideRemindersInCombatDescriptionDefault = ((SettingEntry)HideRemindersInCombat).get_Description();
			((SettingEntry)HideRemindersInCombat).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInCombat-name", hideRemindersInCombatDisplayNameDefault)));
			((SettingEntry)HideRemindersInCombat).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInCombat-description", hideRemindersInCombatDescriptionDefault)));
			string hideRemindersInPvE_OpenWorldDisplayNameDefault = ((SettingEntry)HideRemindersInPvE_OpenWorld).get_DisplayName();
			string hideRemindersInPvE_OpenWorldDescriptionDefault = ((SettingEntry)HideRemindersInPvE_OpenWorld).get_Description();
			((SettingEntry)HideRemindersInPvE_OpenWorld).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInPvE_OpenWorld-name", hideRemindersInPvE_OpenWorldDisplayNameDefault)));
			((SettingEntry)HideRemindersInPvE_OpenWorld).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInPvE_OpenWorld-description", hideRemindersInPvE_OpenWorldDescriptionDefault)));
			string hideRemindersInPvE_CompetetiveDisplayNameDefault = ((SettingEntry)HideRemindersInPvE_Competetive).get_DisplayName();
			string hideRemindersInPvE_CompetetiveDescriptionDefault = ((SettingEntry)HideRemindersInPvE_Competetive).get_Description();
			((SettingEntry)HideRemindersInPvE_Competetive).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInPvE_Competetive-name", hideRemindersInPvE_CompetetiveDisplayNameDefault)));
			((SettingEntry)HideRemindersInPvE_Competetive).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInPvE_Competetive-description", hideRemindersInPvE_CompetetiveDescriptionDefault)));
			string hideRemindersInWvWDisplayNameDefault = ((SettingEntry)HideRemindersInWvW).get_DisplayName();
			string hideRemindersInWvWDescriptionDefault = ((SettingEntry)HideRemindersInWvW).get_Description();
			((SettingEntry)HideRemindersInWvW).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInWvW-name", hideRemindersInWvWDisplayNameDefault)));
			((SettingEntry)HideRemindersInWvW).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInWvW-description", hideRemindersInWvWDescriptionDefault)));
			string hideRemindersInPvPDisplayNameDefault = ((SettingEntry)HideRemindersInPvP).get_DisplayName();
			string hideRemindersInPvPDescriptionDefault = ((SettingEntry)HideRemindersInPvP).get_Description();
			((SettingEntry)HideRemindersInPvP).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInPvP-name", hideRemindersInPvPDisplayNameDefault)));
			((SettingEntry)HideRemindersInPvP).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-hideRemindersInPvP-description", hideRemindersInPvPDescriptionDefault)));
		}

		public void UpdateDrawerLocalization(EventAreaConfiguration drawerConfiguration, TranslationService translationService)
		{
			UpdateDrawerLocalization((DrawerConfiguration)drawerConfiguration, translationService);
			string leftClickActionDisplayNameDefault = ((SettingEntry)drawerConfiguration.LeftClickAction).get_DisplayName();
			string leftClickActionDescriptionDefault = ((SettingEntry)drawerConfiguration.LeftClickAction).get_Description();
			((SettingEntry)drawerConfiguration.LeftClickAction).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerLeftClickAction-name", leftClickActionDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.LeftClickAction).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerLeftClickAction-description", leftClickActionDescriptionDefault)));
			string showTooltipsDisplayNameDefault = ((SettingEntry)drawerConfiguration.ShowTooltips).get_DisplayName();
			string showTooltipsDescriptionDefault = ((SettingEntry)drawerConfiguration.ShowTooltips).get_Description();
			((SettingEntry)drawerConfiguration.ShowTooltips).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShowTooltips-name", showTooltipsDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.ShowTooltips).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShowTooltips-description", showTooltipsDescriptionDefault)));
			string timespanDisplayNameDefault = ((SettingEntry)drawerConfiguration.TimeSpan).get_DisplayName();
			string timespanDescriptionDefault = ((SettingEntry)drawerConfiguration.TimeSpan).get_Description();
			((SettingEntry)drawerConfiguration.TimeSpan).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerTimespan-name", timespanDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.TimeSpan).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerTimespan-description", timespanDescriptionDefault)));
			string historySplitDisplayNameDefault = ((SettingEntry)drawerConfiguration.HistorySplit).get_DisplayName();
			string historySplitDescriptionDefault = ((SettingEntry)drawerConfiguration.HistorySplit).get_Description();
			((SettingEntry)drawerConfiguration.HistorySplit).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHistorySplit-name", historySplitDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HistorySplit).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHistorySplit-description", historySplitDescriptionDefault)));
			string enableHistorySplitScrollingDisplayNameDefault = ((SettingEntry)drawerConfiguration.EnableHistorySplitScrolling).get_DisplayName();
			string enableHistorySplitScrollingDescriptionDefault = ((SettingEntry)drawerConfiguration.EnableHistorySplitScrolling).get_Description();
			((SettingEntry)drawerConfiguration.EnableHistorySplitScrolling).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEnableHistorySplitScrolling-name", enableHistorySplitScrollingDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EnableHistorySplitScrolling).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEnableHistorySplitScrolling-description", enableHistorySplitScrollingDescriptionDefault)));
			string historySplitScrollingSpeedDisplayNameDefault = ((SettingEntry)drawerConfiguration.HistorySplitScrollingSpeed).get_DisplayName();
			string historySplitScrollingSpeedDescriptionDefault = ((SettingEntry)drawerConfiguration.HistorySplitScrollingSpeed).get_Description();
			((SettingEntry)drawerConfiguration.HistorySplitScrollingSpeed).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHistorySplitScrollingSpeed-name", historySplitScrollingSpeedDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HistorySplitScrollingSpeed).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHistorySplitScrollingSpeed-description", historySplitScrollingSpeedDescriptionDefault)));
			string drawBordersDisplayNameDefault = ((SettingEntry)drawerConfiguration.DrawBorders).get_DisplayName();
			string drawBordersDescriptionDefault = ((SettingEntry)drawerConfiguration.DrawBorders).get_Description();
			((SettingEntry)drawerConfiguration.DrawBorders).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawBorders-name", drawBordersDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.DrawBorders).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawBorders-description", drawBordersDescriptionDefault)));
			string useFillersDisplayNameDefault = ((SettingEntry)drawerConfiguration.UseFiller).get_DisplayName();
			string useFillersDescriptionDefault = ((SettingEntry)drawerConfiguration.UseFiller).get_Description();
			((SettingEntry)drawerConfiguration.UseFiller).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerUseFillers-name", useFillersDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.UseFiller).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerUseFillers-description", useFillersDescriptionDefault)));
			string fillerTextColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.FillerTextColor).get_DisplayName();
			string fillerTextColorDescriptionDefault = ((SettingEntry)drawerConfiguration.FillerTextColor).get_Description();
			((SettingEntry)drawerConfiguration.FillerTextColor).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerTextColor-name", fillerTextColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.FillerTextColor).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerTextColor-description", fillerTextColorDescriptionDefault)));
			string acceptWaypointPromptDisplayNameDefault = ((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).get_DisplayName();
			string acceptWaypointPromptDescriptionDefault = ((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).get_Description();
			((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerAcceptWaypointPrompt-name", acceptWaypointPromptDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.AcceptWaypointPrompt).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerAcceptWaypointPrompt-description", acceptWaypointPromptDescriptionDefault)));
			string completionActionDisplayNameDefault = ((SettingEntry)drawerConfiguration.CompletionAction).get_DisplayName();
			string completionActionDescriptionDefault = ((SettingEntry)drawerConfiguration.CompletionAction).get_Description();
			((SettingEntry)drawerConfiguration.CompletionAction).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletionAction-name", completionActionDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.CompletionAction).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletionAction-description", completionActionDescriptionDefault)));
			string eventHeightDisplayNameDefault = ((SettingEntry)drawerConfiguration.EventHeight).get_DisplayName();
			string eventHeightDescriptionDefault = ((SettingEntry)drawerConfiguration.EventHeight).get_Description();
			((SettingEntry)drawerConfiguration.EventHeight).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEventHeight-name", eventHeightDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EventHeight).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEventHeight-description", eventHeightDescriptionDefault)));
			string eventBackgroundOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.EventBackgroundOpacity).get_DisplayName();
			string eventBackgroundOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.EventBackgroundOpacity).get_Description();
			((SettingEntry)drawerConfiguration.EventBackgroundOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEventBackgroundOpacity-name", eventBackgroundOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EventBackgroundOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEventBackgroundOpacity-description", eventBackgroundOpacityDescriptionDefault)));
			string drawShadowsDisplayNameDefault = ((SettingEntry)drawerConfiguration.DrawShadows).get_DisplayName();
			string drawShadowsDescriptionDefault = ((SettingEntry)drawerConfiguration.DrawShadows).get_Description();
			((SettingEntry)drawerConfiguration.DrawShadows).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawShadows-name", drawShadowsDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.DrawShadows).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawShadows-description", drawShadowsDescriptionDefault)));
			string shadowColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.ShadowColor).get_DisplayName();
			string shadowColorDescriptionDefault = ((SettingEntry)drawerConfiguration.ShadowColor).get_Description();
			((SettingEntry)drawerConfiguration.ShadowColor).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShadowColor-name", shadowColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.ShadowColor).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShadowColor-description", shadowColorDescriptionDefault)));
			string drawShadowsForFillerDisplayNameDefault = ((SettingEntry)drawerConfiguration.DrawShadowsForFiller).get_DisplayName();
			string drawShadowsForFillerDescriptionDefault = ((SettingEntry)drawerConfiguration.DrawShadowsForFiller).get_Description();
			((SettingEntry)drawerConfiguration.DrawShadowsForFiller).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawShadowsForFiller-name", drawShadowsForFillerDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.DrawShadowsForFiller).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawShadowsForFiller-description", drawShadowsForFillerDescriptionDefault)));
			string fillerShadowColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.FillerShadowColor).get_DisplayName();
			string fillerShadowColorDescriptionDefault = ((SettingEntry)drawerConfiguration.FillerShadowColor).get_Description();
			((SettingEntry)drawerConfiguration.FillerShadowColor).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerShadowColor-name", fillerShadowColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.FillerShadowColor).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerShadowColor-description", fillerShadowColorDescriptionDefault)));
			string drawIntervalDisplayNameDefault = ((SettingEntry)drawerConfiguration.DrawInterval).get_DisplayName();
			string drawIntervalDescriptionDefault = ((SettingEntry)drawerConfiguration.DrawInterval).get_Description();
			((SettingEntry)drawerConfiguration.DrawInterval).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawInterval-name", drawIntervalDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.DrawInterval).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerDrawInterval-description", drawIntervalDescriptionDefault)));
			string limitToCurrentMapDisplayNameDefault = ((SettingEntry)drawerConfiguration.LimitToCurrentMap).get_DisplayName();
			string limitToCurrentMapDescriptionDefault = ((SettingEntry)drawerConfiguration.LimitToCurrentMap).get_Description();
			((SettingEntry)drawerConfiguration.LimitToCurrentMap).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerLimitToCurrentMap-name", limitToCurrentMapDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.LimitToCurrentMap).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerLimitToCurrentMap-description", limitToCurrentMapDescriptionDefault)));
			string allowUnspecifiedMapDisplayNameDefault = ((SettingEntry)drawerConfiguration.AllowUnspecifiedMap).get_DisplayName();
			string allowUnspecifiedMapDescriptionDefault = ((SettingEntry)drawerConfiguration.AllowUnspecifiedMap).get_Description();
			((SettingEntry)drawerConfiguration.AllowUnspecifiedMap).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerAllowUnspecifiedMap-name", allowUnspecifiedMapDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.AllowUnspecifiedMap).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerAllowUnspecifiedMap-description", allowUnspecifiedMapDescriptionDefault)));
			string timeLineOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.TimeLineOpacity).get_DisplayName();
			string timeLineOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.TimeLineOpacity).get_Description();
			((SettingEntry)drawerConfiguration.TimeLineOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerTimeLineOpacity-name", timeLineOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.TimeLineOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerTimeLineOpacity-description", timeLineOpacityDescriptionDefault)));
			string eventTextOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.EventTextOpacity).get_DisplayName();
			string eventTextOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.EventTextOpacity).get_Description();
			((SettingEntry)drawerConfiguration.EventTextOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEventTextOpacity-name", eventTextOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EventTextOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEventTextOpacity-description", eventTextOpacityDescriptionDefault)));
			string fillerTextOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.FillerTextOpacity).get_DisplayName();
			string fillerTextOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.FillerTextOpacity).get_Description();
			((SettingEntry)drawerConfiguration.FillerTextOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerTextOpacity-name", fillerTextOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.FillerTextOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerTextOpacity-description", fillerTextOpacityDescriptionDefault)));
			string shadowOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.ShadowOpacity).get_DisplayName();
			string shadowOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.ShadowOpacity).get_Description();
			((SettingEntry)drawerConfiguration.ShadowOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShadowOpacity-name", shadowOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.ShadowOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShadowOpacity-description", shadowOpacityDescriptionDefault)));
			string fillerShadowOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.FillerShadowOpacity).get_DisplayName();
			string fillerShadowOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.FillerShadowOpacity).get_Description();
			((SettingEntry)drawerConfiguration.FillerShadowOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerShadowOpacity-name", fillerShadowOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.FillerShadowOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerFillerShadowOpacity-description", fillerShadowOpacityDescriptionDefault)));
			string completedEventsBackgroundOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.CompletedEventsBackgroundOpacity).get_DisplayName();
			string completedEventsBackgroundOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.CompletedEventsBackgroundOpacity).get_Description();
			((SettingEntry)drawerConfiguration.CompletedEventsBackgroundOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletedEventsBackgroundOpacity-name", completedEventsBackgroundOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.CompletedEventsBackgroundOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletedEventsBackgroundOpacity-description", completedEventsBackgroundOpacityDescriptionDefault)));
			string completedEventsTextOpacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.CompletedEventsTextOpacity).get_DisplayName();
			string completedEventsTextOpacityDescriptionDefault = ((SettingEntry)drawerConfiguration.CompletedEventsTextOpacity).get_Description();
			((SettingEntry)drawerConfiguration.CompletedEventsTextOpacity).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletedEventsTextOpacity-name", completedEventsTextOpacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.CompletedEventsTextOpacity).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletedEventsTextOpacity-description", completedEventsTextOpacityDescriptionDefault)));
			string completedEventsInvertTextColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.CompletedEventsInvertTextColor).get_DisplayName();
			string completedEventsInvertTextColorDescriptionDefault = ((SettingEntry)drawerConfiguration.CompletedEventsInvertTextColor).get_Description();
			((SettingEntry)drawerConfiguration.CompletedEventsInvertTextColor).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletedEventsInvertTextColor-name", completedEventsInvertTextColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.CompletedEventsInvertTextColor).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCompletedEventsInvertTextColor-description", completedEventsInvertTextColorDescriptionDefault)));
			string hideOnOpenMapDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideOnOpenMap).get_DisplayName();
			string hideOnOpenMapDescriptionDefault = ((SettingEntry)drawerConfiguration.HideOnOpenMap).get_Description();
			((SettingEntry)drawerConfiguration.HideOnOpenMap).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideOnOpenMap-name", hideOnOpenMapDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideOnOpenMap).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideOnOpenMap-description", hideOnOpenMapDescriptionDefault)));
			string hideOnMissingMumbleTicksDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideOnMissingMumbleTicks).get_DisplayName();
			string hideOnMissingMumbleTicksDescriptionDefault = ((SettingEntry)drawerConfiguration.HideOnMissingMumbleTicks).get_Description();
			((SettingEntry)drawerConfiguration.HideOnMissingMumbleTicks).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideOnMissingMumbleTicks-name", hideOnMissingMumbleTicksDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideOnMissingMumbleTicks).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideOnMissingMumbleTicks-description", hideOnMissingMumbleTicksDescriptionDefault)));
			string hideInCombatDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideInCombat).get_DisplayName();
			string hideInCombatDescriptionDefault = ((SettingEntry)drawerConfiguration.HideInCombat).get_Description();
			((SettingEntry)drawerConfiguration.HideInCombat).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInCombat-name", hideInCombatDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideInCombat).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInCombat-description", hideInCombatDescriptionDefault)));
			string hideInPvE_OpenWorldDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideInPvE_OpenWorld).get_DisplayName();
			string hideInPvE_OpenWorldDescriptionDefault = ((SettingEntry)drawerConfiguration.HideInPvE_OpenWorld).get_Description();
			((SettingEntry)drawerConfiguration.HideInPvE_OpenWorld).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInPvE_OpenWorld-name", hideInPvE_OpenWorldDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideInPvE_OpenWorld).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInPvE_OpenWorld-description", hideInPvE_OpenWorldDescriptionDefault)));
			string hideInPvE_CompetetiveDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideInPvE_Competetive).get_DisplayName();
			string hideInPvE_CompetetiveDescriptionDefault = ((SettingEntry)drawerConfiguration.HideInPvE_Competetive).get_Description();
			((SettingEntry)drawerConfiguration.HideInPvE_Competetive).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInPvE_Competetive-name", hideInPvE_CompetetiveDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideInPvE_Competetive).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInPvE_Competetive-description", hideInPvE_CompetetiveDescriptionDefault)));
			string hideInWvWDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideInWvW).get_DisplayName();
			string hideInWvWDescriptionDefault = ((SettingEntry)drawerConfiguration.HideInWvW).get_Description();
			((SettingEntry)drawerConfiguration.HideInWvW).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInWvW-name", hideInWvWDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideInWvW).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInWvW-description", hideInWvWDescriptionDefault)));
			string hideInPvPDisplayNameDefault = ((SettingEntry)drawerConfiguration.HideInPvP).get_DisplayName();
			string hideInPvPDescriptionDefault = ((SettingEntry)drawerConfiguration.HideInPvP).get_Description();
			((SettingEntry)drawerConfiguration.HideInPvP).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInPvP-name", hideInPvPDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.HideInPvP).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerHideInPvP-description", hideInPvPDescriptionDefault)));
			string showCategoryNamesDisplayNameDefault = ((SettingEntry)drawerConfiguration.ShowCategoryNames).get_DisplayName();
			string showCategoryNamesDescriptionDefault = ((SettingEntry)drawerConfiguration.ShowCategoryNames).get_Description();
			((SettingEntry)drawerConfiguration.ShowCategoryNames).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShowCategoryNames-name", showCategoryNamesDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.ShowCategoryNames).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerShowCategoryNames-description", showCategoryNamesDescriptionDefault)));
			string categoryNameColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.CategoryNameColor).get_DisplayName();
			string categoryNameColorDescriptionDefault = ((SettingEntry)drawerConfiguration.CategoryNameColor).get_Description();
			((SettingEntry)drawerConfiguration.CategoryNameColor).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCategoryNameColor-name", categoryNameColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.CategoryNameColor).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerCategoryNameColor-description", categoryNameColorDescriptionDefault)));
			string enableColorGradientsDisplayNameDefault = ((SettingEntry)drawerConfiguration.EnableColorGradients).get_DisplayName();
			string enableColorGradientsDescriptionDefault = ((SettingEntry)drawerConfiguration.EnableColorGradients).get_Description();
			((SettingEntry)drawerConfiguration.EnableColorGradients).set_GetDisplayNameFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEnableColorGradients-name", enableColorGradientsDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EnableColorGradients).set_GetDescriptionFunc((Func<string>)(() => translationService.GetTranslation("setting-drawerEnableColorGradients-description", enableColorGradientsDescriptionDefault)));
		}

		public override void Unload()
		{
			base.Unload();
			ShowDynamicEventInWorld.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventInWorld_SettingChanged);
			ShowDynamicEventsInWorldOnlyWhenInside.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowDynamicEventsInWorldOnlyWhenInside_SettingChanged);
		}
	}
}
