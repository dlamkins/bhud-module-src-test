using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids
{
	public class RaidPanel : GridPanel
	{
		private readonly IEnumerable<Wing> Wings = new List<Wing>();

		private static RaidSettings Settings => Service.Settings.RaidSettings;

		public RaidPanel()
			: base(Settings.Generic, (Container)(object)GameService.Graphics.get_SpriteScreen())
		{
			Wings = WingFactory.Create(this);
			Service.ApiPollingService!.ApiPollingTrigger += delegate
			{
				Task.Run(async delegate
				{
					List<string> weeklyClears = await GetCurrentClearsService.GetClearsFromApi();
					foreach (Wing wing in Wings)
					{
						foreach (BoxModel encounter in wing.boxes)
						{
							encounter.SetCleared(weeklyClears.Contains(encounter.id));
						}
					}
					ApplyEncounterBackgroundColors();
					if (Settings.RaidPanelMentorProgress.get_Value())
					{
						await Service.MentorAchievementProgress.RefreshFromApiAsync();
					}
					((Control)this).Invalidate();
				});
			};
			Settings.Style.Color.Cleared.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.Style.Color.NotCleared.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.RaidPanelColorNonWeeklyBounty.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.RaidPanelHighlightNonWeeklyBounty.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.RaidPanelOmitEventEncounters.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
		}

		private void ApplyEncounterBackgroundColors()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			Color clearedColor = Settings.Style.Color.Cleared.get_Value().HexToXnaColor();
			Color notClearedColor = Settings.Style.Color.NotCleared.get_Value().HexToXnaColor();
			Color nonWeeklyColor = Settings.RaidPanelColorNonWeeklyBounty.get_Value().HexToXnaColor();
			bool highlightNonWeekly = Settings.RaidPanelHighlightNonWeeklyBounty.get_Value();
			WeeklyBountyEncountersService weeklyBounties = Service.WeeklyBountyEncounters;
			foreach (Wing wing in Wings)
			{
				foreach (BoxModel encounter in wing.boxes)
				{
					if (encounter.IsCleared)
					{
						encounter.Box.BackgroundColor = clearedColor;
					}
					else if (highlightNonWeekly && weeklyBounties != null && !weeklyBounties.IsWeeklyBounty(encounter.id) && (!Settings.RaidPanelOmitEventEncounters.get_Value() || !Service.RaidData.IsEventEncounter(encounter.id)))
					{
						encounter.Box.BackgroundColor = nonWeeklyColor;
					}
					else
					{
						encounter.Box.BackgroundColor = notClearedColor;
					}
				}
			}
			((Control)this).Invalidate();
		}

		public void UpdateEncounterLabel(string encounterApiId, string newLabel)
		{
			foreach (Wing wing in Wings)
			{
				if (wing.id == encounterApiId)
				{
					((Label)wing.GroupLabel).set_Text(newLabel);
					((Control)wing.GroupLabel).Invalidate();
					((Control)this).Invalidate();
					break;
				}
				foreach (BoxModel encounter in wing.boxes)
				{
					if (encounter.id == encounterApiId)
					{
						encounter.SetLabel(newLabel);
						((Control)this).Invalidate();
						return;
					}
				}
			}
		}
	}
}
