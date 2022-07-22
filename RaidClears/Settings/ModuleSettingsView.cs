using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace RaidClears.Settings
{
	public class ModuleSettingsView : View
	{
		private Module _m;

		private Label _apiPollLabel;

		private readonly SettingService _settingService;

		private FlowPanel _rootFlowPanel;

		public ModuleSettingsView(SettingService settingService, Module m)
			: this()
		{
			_m = m;
			_settingService = settingService;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_OuterControlPadding(new Vector2(10f, 20f));
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val).set_Width(((Control)buildPanel).get_Width() - 10);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			int singleColumnWidth = ((Control)buildPanel).get_Width() - (int)_rootFlowPanel.get_OuterControlPadding().X * 2;
			int doubleColWidth = singleColumnWidth / 2 - 100;
			FlowPanel generalSettingFlowPanel = CreateSettingsGroupFlowPanel("General Options", (Container)(object)_rootFlowPanel);
			FlowPanel col2 = CreateTwoColPanel((Container)(object)generalSettingFlowPanel);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelApiPollingPeriod, (Container)(object)col2, doubleColWidth);
			_apiPollLabel = CreateApiPollRemainingLabel((Container)(object)col2, doubleColWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelIsVisibleKeyBind, (Container)(object)generalSettingFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.ShowRaidsCornerIconSetting, (Container)(object)generalSettingFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelIsVisible, (Container)(object)generalSettingFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelAllowTooltipsSetting, (Container)(object)generalSettingFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelDragWithMouseIsEnabledSetting, (Container)(object)generalSettingFlowPanel, singleColumnWidth);
			FlowPanel layoutFlowPanel = CreateSettingsGroupFlowPanel("Layout and Visuals", (Container)(object)_rootFlowPanel);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelOrientationSetting, (Container)(object)layoutFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelFontSizeSetting, (Container)(object)layoutFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelWingLabelsSetting, (Container)(object)layoutFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelWingLabelOpacity, (Container)(object)layoutFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.RaidPanelEncounterOpacity, (Container)(object)layoutFlowPanel, singleColumnWidth);
			FlowPanel wingSelectionFlowPanel = CreateSettingsGroupFlowPanel("Wing Selection", (Container)(object)_rootFlowPanel);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W1IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W2IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W3IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W4IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W5IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W6IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ShowSettingWithViewContainer((SettingEntry)(object)_settingService.W7IsVisibleSetting, (Container)(object)wingSelectionFlowPanel, singleColumnWidth);
			ReloadApiPollLabelText();
			_settingService.RaidPanelApiPollingPeriod.add_SettingChanged((EventHandler<ValueChangedEventArgs<ApiPollPeriod>>)delegate
			{
				ReloadApiPollLabelText();
			});
		}

		private static FlowPanel CreateTwoColPanel(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val).set_ShowBorder(false);
			((Control)val).set_Width(((Control)parent).get_Width() - 20);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			return val;
		}

		private static FlowPanel CreateSettingsGroupFlowPanel(string title, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title(title);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Width(((Control)parent).get_Width() - 20);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			return val;
		}

		private static ViewContainer ShowSettingWithViewContainer(SettingEntry settingEntry, Container parent, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			val.Show(SettingView.FromType(settingEntry, ((Control)parent).get_Width()));
			return val;
		}

		private Label CreateApiPollRemainingLabel(Container parent, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			Label val = new Label();
			val.set_AutoSizeHeight(true);
			val.set_Text("");
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(width);
			return val;
		}

		private void ReloadApiPollLabelText()
		{
			if (_apiPollLabel != null)
			{
				int secondsRemaining = _m.GetTimeoutSecondsRemaining();
				string labelText = "Next Api call in ~" + secondsRemaining + " seconds";
				if (secondsRemaining < 0)
				{
					labelText = "Waiting for a valid API token";
				}
				_apiPollLabel.set_Text(labelText);
			}
		}
	}
}
