using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class SettingsTabView : View
	{
		private readonly Services _services;

		private Label? _drfConnectionStatusValueLabel;

		private FlowPanel? _rootFlowPanel;

		public SettingsTabView(Services services)
			: this()
		{
			_services = services;
		}

		protected override void Unload()
		{
			FlowPanel? rootFlowPanel = _rootFlowPanel;
			if (rootFlowPanel != null)
			{
				((Control)rootFlowPanel).Dispose();
			}
			_rootFlowPanel = null;
			_services.Drf.DrfConnectionStatusChanged -= new EventHandler(OnDrfConnectionStatusChanged);
			_services.SettingService.CountBackgroundOpacitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged<int>);
			_services.SettingService.CountBackgroundColorSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ColorType>>)OnSettingChanged<ColorType>);
			_services.SettingService.PositiveCountTextColorSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ColorType>>)OnSettingChanged<ColorType>);
			_services.SettingService.NegativeCountTextColorSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ColorType>>)OnSettingChanged<ColorType>);
			_services.SettingService.CountFontSizeSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)OnSettingChanged<FontSize>);
			_services.SettingService.CountHoritzontalAlignmentSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<HorizontalAlignment>>)OnSettingChanged<HorizontalAlignment>);
			_services.SettingService.StatIconSizeSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<StatIconSize>>)OnSettingChanged<StatIconSize>);
			_services.SettingService.NegativeCountIconOpacitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged<int>);
			_services.SettingService.RarityIconBorderIsVisibleSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged<bool>);
			_drfConnectionStatusValueLabel = null;
		}

		protected override async void Build(Container buildPanel)
		{
			SettingsTabView settingsTabView = this;
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 20f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			settingsTabView._rootFlowPanel = val;
			BitmapFont font = _services.FontService.Fonts[(FontSize)16];
			CreateDrfConnectionStatusLabel(font, _rootFlowPanel);
			await Task.Delay(1);
			CreateSetupDrfTokenPanel(font, _services, (Container)(object)_rootFlowPanel);
			SettingsFlowPanel parent = new SettingsFlowPanel((Container)(object)_rootFlowPanel, "Misc");
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.WindowVisibilityKeyBindingSetting);
			new AutomaticResetSettingsPanel((Container)(object)parent, _services);
			SettingsFlowPanel parent2 = new SettingsFlowPanel((Container)(object)_rootFlowPanel, "Count");
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.CountBackgroundOpacitySetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.CountBackgroundColorSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.PositiveCountTextColorSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.NegativeCountTextColorSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.CountFontSizeSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.CountHoritzontalAlignmentSetting);
			SettingsFlowPanel iconSettingsFlowPanel = new SettingsFlowPanel((Container)(object)_rootFlowPanel, "Icon");
			CreateIconSizeDropdown((Container)(object)iconSettingsFlowPanel, _services);
			new SettingControl((Container)(object)iconSettingsFlowPanel, (SettingEntry)(object)_services.SettingService.NegativeCountIconOpacitySetting);
			new SettingControl((Container)(object)iconSettingsFlowPanel, (SettingEntry)(object)_services.SettingService.RarityIconBorderIsVisibleSetting);
			SettingsFlowPanel parent3 = new SettingsFlowPanel((Container)(object)_rootFlowPanel, "Profit window");
			new FixedWidthHintLabel((Container)(object)parent3, 480, "A small window which shows the profit. It is permanently visible even when the main farming tracker window is not visible.");
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.IsProfitWindowVisibleSetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.DragProfitWindowWithMouseIsEnabledSetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.ProfitWindowCanBeClickedThroughSetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.WindowAnchorSetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.ProfitWindowBackgroundOpacitySetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.ProfitWindowDisplayModeSetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.ProfitLabelTextSetting);
			new SettingControl((Container)(object)parent3, (SettingEntry)(object)_services.SettingService.ProfitPerHourLabelTextSetting);
			_services.SettingService.CountBackgroundOpacitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged<int>);
			_services.SettingService.CountBackgroundColorSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ColorType>>)OnSettingChanged<ColorType>);
			_services.SettingService.PositiveCountTextColorSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ColorType>>)OnSettingChanged<ColorType>);
			_services.SettingService.NegativeCountTextColorSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ColorType>>)OnSettingChanged<ColorType>);
			_services.SettingService.CountFontSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)OnSettingChanged<FontSize>);
			_services.SettingService.StatIconSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<StatIconSize>>)OnSettingChanged<StatIconSize>);
			_services.SettingService.CountHoritzontalAlignmentSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<HorizontalAlignment>>)OnSettingChanged<HorizontalAlignment>);
			_services.SettingService.NegativeCountIconOpacitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged<int>);
			_services.SettingService.RarityIconBorderIsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged<bool>);
		}

		private void OnSettingChanged<T>(object sender, ValueChangedEventArgs<T> e)
		{
			_services.UpdateLoop.TriggerUpdateUi();
		}

		private void CreateIconSizeDropdown(Container parent, Services services)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			Services services2 = services;
			string settingTooltipText = ((SettingEntry)services2.SettingService.StatIconSizeSetting).get_GetDescriptionFunc()();
			Panel val = new Panel();
			((Control)val).set_BasicTooltipText(settingTooltipText);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel iconSizePanel = val;
			Label val2 = new Label();
			val2.set_Text(((SettingEntry)services2.SettingService.StatIconSizeSetting).get_GetDisplayNameFunc()());
			((Control)val2).set_BasicTooltipText(settingTooltipText);
			((Control)val2).set_Top(4);
			((Control)val2).set_Left(5);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)iconSizePanel);
			Label iconSizeLabel = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_BasicTooltipText(settingTooltipText);
			((Control)val3).set_Left(((Control)iconSizeLabel).get_Right() + 5);
			((Control)val3).set_Width(60);
			((Control)val3).set_Parent((Container)(object)iconSizePanel);
			Dropdown iconSizeDropDown = val3;
			string[] names = Enum.GetNames(typeof(StatIconSize));
			foreach (string dropDownValue in names)
			{
				iconSizeDropDown.get_Items().Add(dropDownValue);
			}
			iconSizeDropDown.set_SelectedItem(services2.SettingService.StatIconSizeSetting.get_Value().ToString());
			iconSizeDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				services2.UpdateLoop.TriggerUpdateUi();
			});
			iconSizeDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				services2.SettingService.StatIconSizeSetting.set_Value((StatIconSize)Enum.Parse(typeof(StatIconSize), iconSizeDropDown.get_SelectedItem()));
			});
		}

		private void CreateDrfConnectionStatusLabel(BitmapFont font, FlowPanel rootFlowPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)rootFlowPanel);
			Panel drfConnectionStatusPanel = val;
			Label val2 = new Label();
			val2.set_Text("DRF Server Connection:");
			val2.set_Font(font);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(0, 10));
			((Control)val2).set_Parent((Container)(object)drfConnectionStatusPanel);
			Label drfConnectionStatusTitleLabel = val2;
			Label val3 = new Label();
			val3.set_Text("");
			val3.set_Font(font);
			val3.set_StrokeText(true);
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(((Control)drfConnectionStatusTitleLabel).get_Right() + 5, 10));
			((Control)val3).set_Parent((Container)(object)drfConnectionStatusPanel);
			_drfConnectionStatusValueLabel = val3;
			_services.Drf.DrfConnectionStatusChanged += new EventHandler(OnDrfConnectionStatusChanged);
			OnDrfConnectionStatusChanged();
		}

		private void CreateSetupDrfTokenPanel(BitmapFont font, Services services, Container parent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Expected O, but got Unknown
			AutoSizeContainer setupDrfWrapperContainer = new AutoSizeContainer(parent);
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title(" ");
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_Icon(AsyncTexture2D.op_Implicit(services.TextureService.DrfTexture));
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.5f);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			val.set_OuterControlPadding(new Vector2(5f, 5f));
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val).set_Width(500);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)setupDrfWrapperContainer);
			FlowPanel addDrfTokenFlowPanel = val;
			ClickThroughLabel clickThroughLabel = new ClickThroughLabel();
			((Label)clickThroughLabel).set_Text("Setup DRF (click)");
			((Label)clickThroughLabel).set_TextColor(Color.get_Yellow());
			((Label)clickThroughLabel).set_Font(_services.FontService.Fonts[(FontSize)20]);
			((Label)clickThroughLabel).set_AutoSizeHeight(true);
			((Label)clickThroughLabel).set_AutoSizeWidth(true);
			((Control)clickThroughLabel).set_Top(6);
			((Control)clickThroughLabel).set_Left(35);
			((Control)clickThroughLabel).set_Parent((Container)(object)setupDrfWrapperContainer);
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)addDrfTokenFlowPanel);
			Panel drfTokenInputPanel = val2;
			string drfTokenTooltip = "Add DRF token from DRF website here. How generate this token is described below.";
			Label val3 = new Label();
			val3.set_Text("DRF Token:");
			((Control)val3).set_BasicTooltipText(drfTokenTooltip);
			val3.set_Font(font);
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(0, 10));
			((Control)val3).set_Parent((Container)(object)drfTokenInputPanel);
			Label drfTokenLabel = val3;
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Container)val4).set_WidthSizingMode((SizingMode)1);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Location(new Point(((Control)drfTokenLabel).get_Right() + 10, 0));
			((Control)val4).set_Parent((Container)(object)drfTokenInputPanel);
			FlowPanel drfTokenTextBoxFlowPanel = val4;
			DrfTokenTextBox drfTokenTextBox = new DrfTokenTextBox(_services.SettingService.DrfTokenSetting.get_Value(), drfTokenTooltip, font, (Container)(object)drfTokenTextBoxFlowPanel);
			Label val5 = new Label();
			val5.set_Text(DrfToken.CreateDrfTokenHintText(_services.SettingService.DrfTokenSetting.get_Value()));
			val5.set_TextColor(Color.get_Yellow());
			val5.set_Font(font);
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Parent((Container)(object)drfTokenTextBoxFlowPanel);
			Label drfTokenValidationLabel = val5;
			drfTokenTextBox.SanitizedTextChanged += delegate
			{
				_services.SettingService.DrfTokenSetting.set_Value(((TextInputBase)drfTokenTextBox).get_Text());
				drfTokenValidationLabel.set_Text(DrfToken.CreateDrfTokenHintText(((TextInputBase)drfTokenTextBox).get_Text()));
			};
			SetupInstructions.CreateSetupInstructions(font, addDrfTokenFlowPanel, _services);
		}

		private void OnDrfConnectionStatusChanged(object? sender = null, EventArgs? e = null)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (_drfConnectionStatusValueLabel == null)
			{
				Module.Logger.Error("DRF status label missing.");
				return;
			}
			DrfConnectionStatus drfConnectionStatus = _services.Drf.DrfConnectionStatus;
			_drfConnectionStatusValueLabel!.set_TextColor(DrfConnectionStatusService.GetDrfConnectionStatusTextColor(drfConnectionStatus));
			_drfConnectionStatusValueLabel!.set_Text(DrfConnectionStatusService.GetSettingTabDrfConnectionStatusText(drfConnectionStatus, _services.Drf.ReconnectTriesCounter, _services.Drf.ReconnectDelaySeconds));
		}
	}
}
