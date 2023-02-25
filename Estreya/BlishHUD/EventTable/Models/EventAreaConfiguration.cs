using System.Collections.Generic;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Models.Drawers;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class EventAreaConfiguration : DrawerConfiguration
	{
		public SettingEntry<List<string>> DisabledEventKeys { get; set; }

		public SettingEntry<EventCompletedAction> CompletionAcion { get; set; }

		public SettingEntry<bool> ShowTooltips { get; set; }

		public SettingEntry<LeftClickAction> LeftClickAction { get; set; }

		public SettingEntry<bool> AcceptWaypointPrompt { get; set; }

		public SettingEntry<int> TimeSpan { get; set; }

		public SettingEntry<int> HistorySplit { get; set; }

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
	}
}
