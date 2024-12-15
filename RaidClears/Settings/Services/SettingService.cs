using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Features.Raids;
using RaidClears.Localization;
using RaidClears.Settings.Enums;
using RaidClears.Settings.Models;

namespace RaidClears.Settings.Services
{
	public class SettingService
	{
		public SettingEntry<ApiPollPeriod> ApiPollingPeriod { get; }

		public SettingEntry<KeyBinding> SettingsPanelKeyBind { get; }

		public SettingEntry<bool> GlobalCornerIconEnabled { get; }

		public SettingEntry<bool> ScreenClamp { get; }

		public SettingEntry<bool> OrganicGridBoxBackgrounds { get; }

		public SettingEntry<int> CornerIconPriority { get; }

		public RaidSettings RaidSettings { get; }

		public DungeonSettings DungeonSettings { get; }

		public StrikeSettings StrikeSettings { get; }

		public FractalSettings FractalSettings { get; }

		public SettingService(SettingCollection settings)
		{
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			ApiPollingPeriod = settings.DefineSetting<ApiPollPeriod>("RCPoll", ApiPollPeriod.MINUTES_5, (Func<string>)(() => Strings.Setting_APIPoll_Label), (Func<string>)(() => Strings.Setting_APIPoll_Tooltip));
			CornerIconPriority = settings.DefineSetting<int>("RCCornerPriority", 53, (Func<string>)(() => Strings.CornerIconPriority_Label), (Func<string>)(() => Strings.CornerIconPriority_Tooltlp));
			SettingComplianceExtensions.SetRange(CornerIconPriority, 0, 1000);
			SettingsPanelKeyBind = settings.DefineSetting<KeyBinding>("RCsettingsKeybind", new KeyBinding((Keys)0), (Func<string>)(() => Strings.Settings_Keybind_Label), (Func<string>)(() => Strings.Settings_Keybind_tooltip));
			SettingsPanelKeyBind.get_Value().set_Enabled(true);
			GlobalCornerIconEnabled = settings.DefineSetting<bool>("RCGlobalCornerIcon", true, (Func<string>)(() => Strings.Setting_CornerIconEnable), (Func<string>)(() => Strings.Setting_CornerIconEnableTooltip));
			ScreenClamp = settings.DefineSetting<bool>("RCScreenClamp", true, (Func<string>)(() => "Keep overlay windows on screen"), (Func<string>)(() => "When turned on, this will make sure that all overlay windows stay within the visible area of your screen, so they don't go off the edges"));
			OrganicGridBoxBackgrounds = settings.DefineSetting<bool>("RCStylize", true, (Func<string>)(() => "'GW2 Style' background boxes"), (Func<string>)(() => "On: Backgrounds will appear with fuzzy edges more akin to GW2's style,\nOff: Background will be rectangles"));
			RaidSettings = new RaidSettings(settings);
			DungeonSettings = new DungeonSettings(settings);
			StrikeSettings = new StrikeSettings(settings);
			FractalSettings = new FractalSettings(settings);
			StrikeSettings.AnchorToRaidPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue())
				{
					AlignStrikesWithRaidPanel();
				}
			});
			RaidSettings.Generic.Location.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)delegate
			{
				if (StrikeSettings.AnchorToRaidPanel.get_Value())
				{
					AlignStrikesWithRaidPanel();
				}
			});
		}

		public void CopyRaidSettings(DisplayStyle settings)
		{
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			settings.Layout.set_Value(RaidSettings.Style.Layout.get_Value());
			settings.LabelDisplay.set_Value(RaidSettings.Style.LabelDisplay.get_Value());
			settings.LabelOpacity.set_Value(RaidSettings.Style.LabelOpacity.get_Value());
			settings.GridOpacity.set_Value(RaidSettings.Style.GridOpacity.get_Value());
			settings.BgOpacity.set_Value(RaidSettings.Style.BgOpacity.get_Value());
			settings.FontSize.set_Value(RaidSettings.Style.FontSize.get_Value());
			settings.Color.Background.set_Value(RaidSettings.Style.Color.Background.get_Value());
			settings.Color.NotCleared.set_Value(RaidSettings.Style.Color.NotCleared.get_Value());
			settings.Color.Cleared.set_Value(RaidSettings.Style.Color.Cleared.get_Value());
			settings.Color.Text.set_Value(RaidSettings.Style.Color.Text.get_Value());
		}

		public void AlignStrikesWithRaidPanel()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			RaidPanel raidPanel = Service.RaidWindow;
			SettingEntry<Point> strikeLoc = StrikeSettings.Generic.Location;
			Vector2 controlPadding = ((FlowPanel)raidPanel).get_ControlPadding();
			Point padding = ((Vector2)(ref controlPadding)).ToPoint();
			SettingEntry<Point> val = strikeLoc;
			SettingEntry<Layout> layout = RaidSettings.Style.Layout;
			if (layout == null)
			{
				goto IL_00af;
			}
			switch (layout.get_Value())
			{
			case Layout.Horizontal:
			case Layout.SingleRow:
				break;
			case Layout.Vertical:
			case Layout.SingleColumn:
				goto IL_0088;
			default:
				goto IL_00af;
			}
			Point value = ((Control)raidPanel).get_Location() + new Point(((Control)raidPanel).get_Size().X + padding.X, 0);
			goto IL_00b7;
			IL_00af:
			value = strikeLoc.get_Value();
			goto IL_00b7;
			IL_00b7:
			val.set_Value(value);
			return;
			IL_0088:
			value = ((Control)raidPanel).get_Location() + new Point(0, ((Control)raidPanel).get_Size().Y + padding.Y);
			goto IL_00b7;
		}
	}
}
