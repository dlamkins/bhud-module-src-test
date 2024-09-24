using System.Collections.Generic;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Models.Drawers;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Chat;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Guild;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class EventAreaConfiguration : DrawerConfiguration
	{
		public SettingEntry<List<string>> DisabledEventKeys { get; set; }

		public SettingEntry<EventCompletedAction> CompletionAction { get; set; }

		public SettingEntry<bool> EnableLinkedCompletion { get; set; }

		public SettingEntry<bool> ShowTooltips { get; set; }

		public SettingEntry<LeftClickAction> LeftClickAction { get; set; }

		public SettingEntry<bool> AcceptWaypointPrompt { get; set; }

		public SettingEntry<bool> HideAfterWaypointNavigation { get; set; }

		public SettingEntry<ChatChannel> WaypointSendingChannel { get; set; }

		public SettingEntry<GuildNumber> WaypointSendingGuild { get; set; }

		public SettingEntry<EventChatFormat> EventChatFormat { get; set; }

		public SettingEntry<int> TimeSpan { get; set; }

		public SettingEntry<int> HistorySplit { get; set; }

		public SettingEntry<bool> EnableHistorySplitScrolling { get; set; }

		public SettingEntry<int> HistorySplitScrollingSpeed { get; set; }

		public SettingEntry<bool> DrawBorders { get; set; }

		public SettingEntry<bool> UseFiller { get; set; }

		public SettingEntry<Color> FillerTextColor { get; set; }

		public SettingEntry<float> FillerTextOpacity { get; set; }

		public SettingEntry<bool> DrawShadowsForFiller { get; set; }

		public SettingEntry<Color> FillerShadowColor { get; set; }

		public SettingEntry<float> FillerShadowOpacity { get; set; }

		public SettingEntry<int> EventHeight { get; set; }

		public SettingEntry<List<string>> EventOrder { get; set; }

		public SettingEntry<float> EventBackgroundOpacity { get; set; }

		public SettingEntry<float> EventTextOpacity { get; set; }

		public SettingEntry<bool> DrawShadows { get; set; }

		public SettingEntry<Color> ShadowColor { get; set; }

		public SettingEntry<float> ShadowOpacity { get; set; }

		public SettingEntry<DrawInterval> DrawInterval { get; set; }

		public SettingEntry<bool> LimitToCurrentMap { get; set; }

		public SettingEntry<bool> AllowUnspecifiedMap { get; set; }

		public SettingEntry<float> TimeLineOpacity { get; set; }

		public SettingEntry<float> CompletedEventsBackgroundOpacity { get; set; }

		public SettingEntry<float> CompletedEventsTextOpacity { get; set; }

		public SettingEntry<bool> CompletedEventsInvertTextColor { get; set; }

		public SettingEntry<bool> HideOnMissingMumbleTicks { get; set; }

		public SettingEntry<bool> HideInCombat { get; set; }

		public SettingEntry<bool> HideOnOpenMap { get; set; }

		public SettingEntry<bool> HideInPvE_OpenWorld { get; set; }

		public SettingEntry<bool> HideInPvE_Competetive { get; set; }

		public SettingEntry<bool> HideInWvW { get; set; }

		public SettingEntry<bool> HideInPvP { get; set; }

		public SettingEntry<bool> ShowCategoryNames { get; set; }

		public SettingEntry<Color> CategoryNameColor { get; set; }

		public SettingEntry<bool> EnableColorGradients { get; set; }

		public SettingEntry<string> EventAbsoluteTimeFormatString { get; set; }

		public SettingEntry<string> EventTimespanDaysFormatString { get; set; }

		public SettingEntry<string> EventTimespanHoursFormatString { get; set; }

		public SettingEntry<string> EventTimespanMinutesFormatString { get; set; }

		public SettingEntry<bool> ShowTopTimeline { get; set; }

		public SettingEntry<string> TopTimelineTimeFormatString { get; set; }

		public SettingEntry<Color> TopTimelineBackgroundColor { get; set; }

		public SettingEntry<Color> TopTimelineLineColor { get; set; }

		public SettingEntry<Color> TopTimelineTimeColor { get; set; }

		public SettingEntry<float> TopTimelineBackgroundOpacity { get; set; }

		public SettingEntry<float> TopTimelineLineOpacity { get; set; }

		public SettingEntry<float> TopTimelineTimeOpacity { get; set; }

		public SettingEntry<bool> TopTimelineLinesOverWholeHeight { get; set; }

		public SettingEntry<bool> TopTimelineLinesInBackground { get; set; }

		public void CopyTo(EventAreaConfiguration other)
		{
			CopyTo((DrawerConfiguration)other);
			other.DisabledEventKeys.set_Value(DisabledEventKeys.get_Value());
			other.CompletionAction.set_Value(CompletionAction.get_Value());
			other.EnableLinkedCompletion.set_Value(EnableLinkedCompletion.get_Value());
			other.ShowTooltips.set_Value(ShowTooltips.get_Value());
			other.LeftClickAction.set_Value(LeftClickAction.get_Value());
			other.AcceptWaypointPrompt.set_Value(AcceptWaypointPrompt.get_Value());
			other.HideAfterWaypointNavigation.set_Value(HideAfterWaypointNavigation.get_Value());
			other.WaypointSendingChannel.set_Value(WaypointSendingChannel.get_Value());
			other.WaypointSendingGuild.set_Value(WaypointSendingGuild.get_Value());
			other.EventChatFormat.set_Value(EventChatFormat.get_Value());
			other.TimeSpan.set_Value(TimeSpan.get_Value());
			other.HistorySplit.set_Value(HistorySplit.get_Value());
			other.EnableHistorySplitScrolling.set_Value(EnableHistorySplitScrolling.get_Value());
			other.HistorySplitScrollingSpeed.set_Value(HistorySplitScrollingSpeed.get_Value());
			other.DrawBorders.set_Value(DrawBorders.get_Value());
			other.UseFiller.set_Value(UseFiller.get_Value());
			other.FillerTextColor.set_Value(FillerTextColor.get_Value());
			other.FillerTextOpacity.set_Value(FillerTextOpacity.get_Value());
			other.DrawShadowsForFiller.set_Value(DrawShadowsForFiller.get_Value());
			other.FillerShadowColor.set_Value(FillerShadowColor.get_Value());
			other.FillerShadowOpacity.set_Value(FillerShadowOpacity.get_Value());
			other.EventHeight.set_Value(EventHeight.get_Value());
			other.EventOrder.set_Value(EventOrder.get_Value());
			other.EventBackgroundOpacity.set_Value(EventBackgroundOpacity.get_Value());
			other.EventTextOpacity.set_Value(EventTextOpacity.get_Value());
			other.DrawShadows.set_Value(DrawShadows.get_Value());
			other.ShadowColor.set_Value(ShadowColor.get_Value());
			other.DrawInterval.set_Value(DrawInterval.get_Value());
			other.LimitToCurrentMap.set_Value(LimitToCurrentMap.get_Value());
			other.AllowUnspecifiedMap.set_Value(AllowUnspecifiedMap.get_Value());
			other.TimeLineOpacity.set_Value(TimeLineOpacity.get_Value());
			other.CompletedEventsBackgroundOpacity.set_Value(CompletedEventsBackgroundOpacity.get_Value());
			other.CompletedEventsTextOpacity.set_Value(CompletedEventsTextOpacity.get_Value());
			other.CompletedEventsInvertTextColor.set_Value(CompletedEventsInvertTextColor.get_Value());
			other.HideOnMissingMumbleTicks.set_Value(HideOnMissingMumbleTicks.get_Value());
			other.HideInCombat.set_Value(HideInCombat.get_Value());
			other.HideOnOpenMap.set_Value(HideOnOpenMap.get_Value());
			other.HideInPvE_OpenWorld.set_Value(HideInPvE_OpenWorld.get_Value());
			other.HideInPvE_Competetive.set_Value(HideInPvE_Competetive.get_Value());
			other.HideInWvW.set_Value(HideInWvW.get_Value());
			other.HideInPvP.set_Value(HideInPvP.get_Value());
			other.ShowCategoryNames.set_Value(ShowCategoryNames.get_Value());
			other.CategoryNameColor.set_Value(CategoryNameColor.get_Value());
			other.EnableColorGradients.set_Value(EnableColorGradients.get_Value());
			other.EventAbsoluteTimeFormatString.set_Value(EventAbsoluteTimeFormatString.get_Value());
			other.EventTimespanDaysFormatString.set_Value(EventTimespanDaysFormatString.get_Value());
			other.EventTimespanHoursFormatString.set_Value(EventTimespanHoursFormatString.get_Value());
			other.EventTimespanMinutesFormatString.set_Value(EventTimespanMinutesFormatString.get_Value());
			other.ShowTopTimeline.set_Value(ShowTopTimeline.get_Value());
			other.TopTimelineTimeFormatString.set_Value(TopTimelineTimeFormatString.get_Value());
			other.TopTimelineBackgroundColor.set_Value(TopTimelineBackgroundColor.get_Value());
			other.TopTimelineLineColor.set_Value(TopTimelineLineColor.get_Value());
			other.TopTimelineTimeColor.set_Value(TopTimelineTimeColor.get_Value());
			other.TopTimelineBackgroundOpacity.set_Value(TopTimelineBackgroundOpacity.get_Value());
			other.TopTimelineLineOpacity.set_Value(TopTimelineLineOpacity.get_Value());
			other.TopTimelineTimeOpacity.set_Value(TopTimelineTimeOpacity.get_Value());
			other.TopTimelineLinesOverWholeHeight.set_Value(TopTimelineLinesOverWholeHeight.get_Value());
			other.TopTimelineLinesInBackground.set_Value(TopTimelineLinesInBackground.get_Value());
		}
	}
}
