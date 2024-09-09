using System;
using System.Threading.Tasks;
using Blish_HUD;
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

		private Label _drfConnectionStatusValueLabel;

		private AutomaticResetSettingsPanel _automaticResetSettingsPanel;

		private const string DRF_CONNECTION_LABEL_TEXT = "DRF Server Connection";

		private const int LABEL_WIDTH = 480;

		public SettingsTabView(Services services)
			: this()
		{
			_services = services;
		}

		protected override void Unload()
		{
			AutomaticResetSettingsPanel automaticResetSettingsPanel = _automaticResetSettingsPanel;
			if (automaticResetSettingsPanel != null)
			{
				((Control)automaticResetSettingsPanel).Dispose();
			}
			_services.Drf.DrfConnectionStatusChanged -= OnDrfConnectionStatusChanged;
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
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 20f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			FlowPanel rootFlowPanel = val;
			BitmapFont font = _services.FontService.Fonts[(FontSize)16];
			CreateDrfConnectionStatusLabel(font, rootFlowPanel);
			await Task.Delay(1);
			CreateSetupDrfTokenPanel(font, (Container)(object)rootFlowPanel);
			SettingsFlowPanel miscSettingsFlowPanel = new SettingsFlowPanel((Container)(object)rootFlowPanel, "Misc");
			new SettingControl((Container)(object)miscSettingsFlowPanel, (SettingEntry)(object)_services.SettingService.WindowVisibilityKeyBindingSetting);
			_automaticResetSettingsPanel = new AutomaticResetSettingsPanel((Container)(object)miscSettingsFlowPanel, _services);
			SettingsFlowPanel parent = new SettingsFlowPanel((Container)(object)rootFlowPanel, "Count");
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.CountBackgroundOpacitySetting);
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.CountBackgroundColorSetting);
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.PositiveCountTextColorSetting);
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.NegativeCountTextColorSetting);
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.CountFontSizeSetting);
			new SettingControl((Container)(object)parent, (SettingEntry)(object)_services.SettingService.CountHoritzontalAlignmentSetting);
			SettingsFlowPanel iconSettingsFlowPanel = new SettingsFlowPanel((Container)(object)rootFlowPanel, "Icon");
			CreateIconSizeDropdown((Container)(object)iconSettingsFlowPanel, _services);
			new SettingControl((Container)(object)iconSettingsFlowPanel, (SettingEntry)(object)_services.SettingService.NegativeCountIconOpacitySetting);
			new SettingControl((Container)(object)iconSettingsFlowPanel, (SettingEntry)(object)_services.SettingService.RarityIconBorderIsVisibleSetting);
			SettingsFlowPanel parent2 = new SettingsFlowPanel((Container)(object)rootFlowPanel, "Profit window");
			new FixedWidthHintLabel((Container)(object)parent2, 480, "A small window which shows the profit. It is permanently visible even when the main farming tracker window is not visible.");
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.IsProfitWindowVisibleSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.DragProfitWindowWithMouseIsEnabledSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.ProfitWindowCanBeClickedThroughSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.WindowAnchorSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.ProfitWindowBackgroundOpacitySetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.ProfitWindowDisplayModeSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.ProfitLabelTextSetting);
			new SettingControl((Container)(object)parent2, (SettingEntry)(object)_services.SettingService.ProfitPerHourLabelTextSetting);
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
			string settingTooltipText = ((SettingEntry)services.SettingService.StatIconSizeSetting).get_GetDescriptionFunc()();
			Panel val = new Panel();
			((Control)val).set_BasicTooltipText(settingTooltipText);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel iconSizePanel = val;
			Label val2 = new Label();
			val2.set_Text(((SettingEntry)services.SettingService.StatIconSizeSetting).get_GetDisplayNameFunc()());
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
			iconSizeDropDown.set_SelectedItem(services.SettingService.StatIconSizeSetting.get_Value().ToString());
			iconSizeDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				services.UpdateLoop.TriggerUpdateUi();
			});
			iconSizeDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				services.SettingService.StatIconSizeSetting.set_Value((StatIconSize)Enum.Parse(typeof(StatIconSize), iconSizeDropDown.get_SelectedItem()));
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
			_services.Drf.DrfConnectionStatusChanged += OnDrfConnectionStatusChanged;
			OnDrfConnectionStatusChanged();
		}

		private void CreateSetupDrfTokenPanel(BitmapFont font, Container parent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Expected O, but got Unknown
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Expected O, but got Unknown
			AutoSizeContainer setupDrfWrapperContainer = new AutoSizeContainer(parent);
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title(" ");
			val.set_FlowDirection((ControlFlowDirection)3);
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
			((Control)clickThroughLabel).set_Left(10);
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
			string buttonTooltip = "Open DRF website in your default web browser.";
			BitmapFont headerFont = _services.FontService.Fonts[(FontSize)20];
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "DRF SETUP INSTRUCTIONS", headerFont);
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Prerequisite:", font);
			new HintLabel((Container)(object)addDrfTokenFlowPanel, "- Windows 8 or newer because DRF requires websocket technolgy.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Setup DRF DLL and DRF account:", font);
			new HintLabel((Container)(object)addDrfTokenFlowPanel, "1. Click the button below and follow the instructions to setup the drf.dll.\n2. Create a drf account on the website and link it with\nyour GW2 Account(s).");
			new OpenUrlInBrowserButton("https://drf.rs/getting-started", "Open drf.dll setup instructions", buttonTooltip, _services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			string testDrfHeader = "Test DRF DLL and DRF account";
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, testDrfHeader + ":", font);
			new HintLabel((Container)(object)addDrfTokenFlowPanel, "1. Click the button below to open the DRF web live tracker.\n2. Use this web live tracker to check if the tracking is working.\ne.g. by opening an unidentified gear.\nThe items should appear almost instantly in the web live tracker.");
			new OpenUrlInBrowserButton("https://drf.rs/dashboard/livetracker", "Open DRF web live tracker", buttonTooltip, _services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Does NOT work? :-( Try this:", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- After a GW2 patch, you will have to wait until a fixed arcdps version is released if you use arcdps to load the drf.dll.\n- If you installed drf.dll a while ago, check the drf website whether an updated version of drf.dll is available.\n- If none of this applies, the DRF Discord can help:");
			new OpenUrlInBrowserButton("https://discord.gg/VSgehyHkrD", "Open DRF Discord", "Open DRF discord in your default web browser.", _services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Is working? :-) Get the DRF Token:", font);
			new HintLabel((Container)(object)addDrfTokenFlowPanel, "1. Click the button below to open the drf.rs settings page.\n2. Click on 'Regenerate Token'.\n3. Copy the 'DRF Token' by clicking on the copy icon.\n4. Paste the DRF Token with CTRL + V into the DRF token input above.\n5. Done! Open the first tab again to see the tracked items/currencies :-)");
			new OpenUrlInBrowserButton("https://drf.rs/dashboard/user/settings", "Open DRF web settings", buttonTooltip, _services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "TROUBLESHOOTING", headerFont);
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Module shows 'Add GW2 API key!' but BlishHUD already has API key", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "Sometimes BlishHUD fails to give a module access to the GW2 API key. That can be caused by a GW2 API timeout when BlishHUD is starting or for other unknown reasons. Possible workarounds:\n- Restart BlishHUD.\n- disable the module, wait a few seconds, then enable the module again.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "'DRF Server Connection' shows 'Authentication failed'", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- Make sure you copied the DRF token into the module with the copy button and CTRL+V as explained above. Otherwise you may accidentally copy only part of the token. In this case the DRF token input above will show you that the format is incomplete/invalid.\n- After you have clicked on 'Regenerate Token' on the DRF website, any old DRF token you may have used previously will become invalid.You must add the new token to the module.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "'DRF Server Connection' shows 'Connected' but does not track changes", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- Currencies and items changes will be shown after the 'Updating...' or 'Resetting...' hint disappears. While those hints are shown the module normally waits for the GW2 API.If the GW2 API is slow or has a timeout, this can unfortunately take a while.\n- The DRF DLL sends data to the DRF Server. Then the DRF Server sends data to this module. If 'DRF Server Connection' shows 'Connected', this only means that the module is connected to the DRF Server and the DRF account is probably set up correctly. But it does not mean that the DRF DLL is set up correctly. Follow the steps from '" + testDrfHeader + "' to test that.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Why is the GW2 API needed?", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- DRF offers only raw data. To get further details like item/currency name, description, icon and profits the GW2 API is still needed.\n- The GW2 API is the reason why the module cannot display changes to your account immediately but somtimes takes several second because it has to wait for the GW2 API responses.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Red bug images appear", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- When the bug image is used for an item/currency:\nhover with the mouse over the bug icon to read the tooltip. In most cases the tooltip should mention that those are items missing in the GW2 API. E.g. lvl-80-boost item or some reknown heart items.\n\n- If the bug images appears somewhere else in the module's UI or the item tooltip is not mentioning an missing item:\nReason 1: The item is new and BlishHUD's texture cache does not know the icon yet.\nOR\nReason 2: You ran BlishHUD as admin at one point and later stopped running BlishHUD as admin. This causes file permission issues for software like BlishHUD that has to create cache or config data.\nYou can try to fix 'Reason 2' by closing BlishHUD and then deleting the 'Blish HUD' folder at 'C:\\ProgramData\\Blish HUD'.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Known DRF issues", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "These issues cannot be fixed or might be fixed in a future release.\n\n- Bank Slot Expansion Crash:\nThe DRF.dll will crash your game when you use a Bank Slot Expansion.\n\n- Equipment changes are tracked:\nOnly none-legendary equipment is affected. Equipping an item counts as losing the item. Unequipping an item counts as gaining the item. This applies to runes and regular gathing tools too. It only somtimes applies to infinite gathering tools. Swapping equipment templates is not tracked. This issue only affects you when you swap equipment by using your bank/inventory. As a workaround you can add equipment items that you swap often to the ignored items.\n\n- Bouncy Chests:\nIf you have more than 4 bouncy chests and swap map, the game will automatically consume all but 4 of them. DRF is currently not noticing this change.\n\n- whole wallet is tracked\nSometimes the whole wallet is accidentely interpreted as a drop. You should not notice this bug, because the module will ignore drops that include more than 10 currencies at once. But you might be affected by this on accounts that have less than 10 currencies");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Known MODULE issues", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- The 'GW2 API error' hint constantly appears\nReason 1: GW2 API is down or instable. The GW2 API can be very instable in the evening. This results in frequent GW2 API timeouts.\nReason 2: A bug in the GW2 API libary used by this module. This can only be fixed by restarting Blish HUD.");
			AddVerticalSpacing(_services, addDrfTokenFlowPanel);
		}

		private static void AddVerticalSpacing(Services services, FlowPanel addDrfTokenFlowPanel)
		{
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "", services.FontService.Fonts[(FontSize)8]);
		}

		private void OnDrfConnectionStatusChanged(object sender = null, EventArgs e = null)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			DrfConnectionStatus drfConnectionStatus = _services.Drf.DrfConnectionStatus;
			_drfConnectionStatusValueLabel.set_TextColor(DrfConnectionStatusService.GetDrfConnectionStatusTextColor(drfConnectionStatus));
			_drfConnectionStatusValueLabel.set_Text(DrfConnectionStatusService.GetSettingTabDrfConnectionStatusText(drfConnectionStatus, _services.Drf.ReconnectTriesCounter, _services.Drf.ReconnectDelaySeconds));
		}
	}
}
