using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Characters.Extensions;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Views
{
	public class SettingsWindow : BaseSettingsWindow
	{
		private Label _customFontSizeLabel;

		private Dropdown _customFontSize;

		private Label _customNameFontSizeLabel;

		private Dropdown _customNameFontSize;

		private readonly FlowPanel _contentPanel;

		private readonly SharedSettingsView _sharedSettingsView;

		private readonly OCR _ocr;

		private readonly SettingsModel _settings;

		private double _tick;

		public SettingsWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, SharedSettingsView sharedSettingsView, OCR ocr, SettingsModel settings)
			: base(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			_sharedSettingsView = sharedSettingsView;
			_ocr = ocr;
			_settings = settings;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((Control)flowPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)flowPanel).set_Height(((Container)this).get_ContentRegion().Height);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(0f, 10f));
			((Panel)flowPanel).set_CanScroll(true);
			_contentPanel = flowPanel;
			base.SubWindowEmblem = AsyncTexture2D.FromAssetId(156027);
			base.MainWindowEmblem = AsyncTexture2D.FromAssetId(156015);
			base.Name = string.Format(strings.ItemSettings, BaseModule<Characters, MainWindow, SettingsModel>.ModuleName ?? "");
			CreateOCR();
			CreateAppearance();
			CreateBehavior();
			CreateRadial();
			CreateDelays();
			CreateGeneral();
			CreateKeybinds();
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLanguageChanged);
			OnLanguageChanged();
		}

		private void CreateRadial()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_0762: Unknown result type (might be due to invalid IL or missing references)
			//IL_0778: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_0872: Unknown result type (might be due to invalid IL or missing references)
			//IL_087f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0895: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = AsyncTexture2D.FromAssetId(157122);
			panel.SetLocalizedTitle = () => strings.RadialMenuSettings;
			panel.SetLocalizedTitleTooltip = () => strings.RadialMenuSettings_Tooltip;
			Panel headerPanel = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(10f));
			FlowPanel contentFlowPanel = flowPanel;
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel2).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(3f, 3f));
			((FlowPanel)flowPanel2).set_OuterControlPadding(new Vector2(5f));
			FlowPanel settingsFlowPanel = flowPanel2;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Parent((Container)(object)settingsFlowPanel);
			((Checkbox)checkbox).set_Checked(_settings.EnableRadialMenu.get_Value());
			checkbox.SetLocalizedText = () => strings.EnableRadialMenu;
			checkbox.SetLocalizedTooltip = () => strings.EnableRadialMenu_Tooltip;
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				_settings.EnableRadialMenu.set_Value(b);
			};
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Parent((Container)(object)settingsFlowPanel);
			checkbox2.SetLocalizedText = () => strings.Radial_ShowAdvancedTooltip;
			checkbox2.SetLocalizedTooltip = () => strings.Radial_ShowAdvancedTooltip_Tooltip;
			((Checkbox)checkbox2).set_Checked(_settings.Radial_ShowAdvancedTooltip.get_Value());
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_settings.Radial_ShowAdvancedTooltip.set_Value(b);
			};
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Parent((Container)(object)settingsFlowPanel);
			checkbox3.SetLocalizedText = () => strings.Radial_UseProfessionColor;
			checkbox3.SetLocalizedTooltip = () => strings.Radial_UseProfessionColor_Tooltip;
			((Checkbox)checkbox3).set_Checked(_settings.Radial_UseProfessionColor.get_Value());
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_settings.Radial_UseProfessionColor.set_Value(b);
			};
			Checkbox checkbox4 = new Checkbox();
			((Control)checkbox4).set_Parent((Container)(object)settingsFlowPanel);
			checkbox4.SetLocalizedText = () => strings.Radial_UseProfessionIcons;
			checkbox4.SetLocalizedTooltip = () => strings.Radial_UseProfessionIcons_Tooltip;
			((Checkbox)checkbox4).set_Checked(_settings.Radial_UseProfessionIcons.get_Value());
			checkbox4.CheckedChangedAction = delegate(bool b)
			{
				_settings.Radial_UseProfessionIcons.set_Value(b);
			};
			Checkbox checkbox5 = new Checkbox();
			((Control)checkbox5).set_Parent((Container)(object)settingsFlowPanel);
			checkbox5.SetLocalizedText = () => strings.Radial_UseProfessionIconsColor;
			checkbox5.SetLocalizedTooltip = () => strings.Radial_UseProfessionIconsColor_Tooltip;
			((Checkbox)checkbox5).set_Checked(_settings.Radial_UseProfessionIconsColor.get_Value());
			checkbox5.CheckedChangedAction = delegate(bool b)
			{
				_settings.Radial_UseProfessionIconsColor.set_Value(b);
			};
			Panel panel2 = new Panel();
			((Control)panel2).set_Parent((Container)(object)settingsFlowPanel);
			((Control)panel2).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel2).set_HeightSizingMode((SizingMode)1);
			Panel subP = panel2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)subP);
			((Label)label).set_AutoSizeWidth(true);
			label.SetLocalizedText = () => string.Format(strings.Radial_Scale + " {0}%", _settings.Radial_Scale.get_Value() * 100f);
			label.SetLocalizedTooltip = () => strings.Radial_Scale_Tooltip;
			Label scaleLabel = label;
			TrackBar trackBar = new TrackBar();
			((Control)trackBar).set_Parent((Container)(object)subP);
			trackBar.SetLocalizedTooltip = () => strings.Radial_Scale_Tooltip;
			((TrackBar)trackBar).set_Value(_settings.Radial_Scale.get_Value() * 100f);
			trackBar.ValueChangedAction = delegate(int v)
			{
				_settings.Radial_Scale.set_Value((float)v / 100f);
				scaleLabel.UserLocale_SettingChanged(_settings.Radial_Scale.get_Value(), null);
			};
			((TrackBar)trackBar).set_MinValue(0f);
			((TrackBar)trackBar).set_MaxValue(100f);
			((Control)trackBar).set_Location(new Point(250, 0));
			Panel panel3 = new Panel();
			((Control)panel3).set_Parent((Container)(object)settingsFlowPanel);
			((Control)panel3).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel3).set_HeightSizingMode((SizingMode)1);
			subP = panel3;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)subP);
			((Label)label2).set_AutoSizeWidth(true);
			((Control)label2).set_Location(new Point(30, 0));
			label2.SetLocalizedText = () => strings.Radial_IdleBackgroundColor;
			Panel panel4 = new Panel();
			((Control)panel4).set_Parent((Container)(object)subP);
			((Control)panel4).set_Location(new Point(0, 0));
			((Control)panel4).set_Size(new Point(20));
			panel4.BackgroundColor = _settings.Radial_IdleColor.get_Value();
			Panel idleBackgroundPreview = panel4;
			TextBox textBox = new TextBox();
			((Control)textBox).set_Parent((Container)(object)subP);
			((Control)textBox).set_Location(new Point(250, 0));
			((TextInputBase)textBox).set_Text(_settings.Radial_IdleColor.get_Value().ToHex());
			((Control)textBox).set_Width(((Container)this).get_ContentRegion().Width - 20 - 250);
			textBox.TextChangedAction = delegate(string t)
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if (t.ColorFromHex(out var outColor4))
				{
					_settings.Radial_IdleColor.set_Value(outColor4);
					idleBackgroundPreview.BackgroundColor = outColor4;
				}
			};
			Panel panel5 = new Panel();
			((Control)panel5).set_Parent((Container)(object)settingsFlowPanel);
			((Control)panel5).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel5).set_HeightSizingMode((SizingMode)1);
			subP = panel5;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)subP);
			((Label)label3).set_AutoSizeWidth(true);
			((Control)label3).set_Location(new Point(30, 0));
			label3.SetLocalizedText = () => strings.Radial_IdleBorderColor;
			Panel panel6 = new Panel();
			((Control)panel6).set_Parent((Container)(object)subP);
			((Control)panel6).set_Location(new Point(0, 0));
			((Control)panel6).set_Size(new Point(20));
			panel6.BackgroundColor = _settings.Radial_IdleBorderColor.get_Value();
			Panel idleBorderPreview = panel6;
			TextBox textBox2 = new TextBox();
			((Control)textBox2).set_Parent((Container)(object)subP);
			((Control)textBox2).set_Location(new Point(250, 0));
			((TextInputBase)textBox2).set_Text(_settings.Radial_IdleBorderColor.get_Value().ToHex());
			((Control)textBox2).set_Width(((Container)this).get_ContentRegion().Width - 20 - 250);
			textBox2.TextChangedAction = delegate(string t)
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if (t.ColorFromHex(out var outColor3))
				{
					_settings.Radial_IdleBorderColor.set_Value(outColor3);
					idleBorderPreview.BackgroundColor = outColor3;
				}
			};
			Panel panel7 = new Panel();
			((Control)panel7).set_Parent((Container)(object)settingsFlowPanel);
			((Control)panel7).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel7).set_HeightSizingMode((SizingMode)1);
			subP = panel7;
			Label label4 = new Label();
			((Control)label4).set_Parent((Container)(object)subP);
			((Label)label4).set_AutoSizeWidth(true);
			((Control)label4).set_Location(new Point(30, 0));
			label4.SetLocalizedText = () => strings.Radial_HoveredBackgroundColor;
			Panel panel8 = new Panel();
			((Control)panel8).set_Parent((Container)(object)subP);
			((Control)panel8).set_Location(new Point(0, 0));
			((Control)panel8).set_Size(new Point(20));
			panel8.BackgroundColor = _settings.Radial_HoveredColor.get_Value();
			Panel activeBackgroundPreview = panel8;
			TextBox textBox3 = new TextBox();
			((Control)textBox3).set_Parent((Container)(object)subP);
			((Control)textBox3).set_Location(new Point(250, 0));
			((TextInputBase)textBox3).set_Text(_settings.Radial_HoveredColor.get_Value().ToHex());
			((Control)textBox3).set_Width(((Container)this).get_ContentRegion().Width - 20 - 250);
			textBox3.TextChangedAction = delegate(string t)
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if (t.ColorFromHex(out var outColor2))
				{
					_settings.Radial_HoveredColor.set_Value(outColor2);
					activeBackgroundPreview.BackgroundColor = outColor2;
				}
			};
			Panel panel9 = new Panel();
			((Control)panel9).set_Parent((Container)(object)settingsFlowPanel);
			((Control)panel9).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel9).set_HeightSizingMode((SizingMode)1);
			subP = panel9;
			Label label5 = new Label();
			((Control)label5).set_Parent((Container)(object)subP);
			((Label)label5).set_AutoSizeWidth(true);
			((Control)label5).set_Location(new Point(30, 0));
			label5.SetLocalizedText = () => strings.Radial_HoveredBorderColor;
			Panel panel10 = new Panel();
			((Control)panel10).set_Parent((Container)(object)subP);
			((Control)panel10).set_Location(new Point(0, 0));
			((Control)panel10).set_Size(new Point(20));
			panel10.BackgroundColor = _settings.Radial_HoveredBorderColor.get_Value();
			Panel activeBorderPreview = panel10;
			TextBox textBox4 = new TextBox();
			((Control)textBox4).set_Parent((Container)(object)subP);
			((Control)textBox4).set_Location(new Point(250, 0));
			((TextInputBase)textBox4).set_Text(_settings.Radial_HoveredBorderColor.get_Value().ToHex());
			((Control)textBox4).set_Width(((Container)this).get_ContentRegion().Width - 20 - 250);
			textBox4.TextChangedAction = delegate(string t)
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if (t.ColorFromHex(out var outColor))
				{
					_settings.Radial_HoveredBorderColor.set_Value(outColor);
					activeBorderPreview.BackgroundColor = outColor;
				}
			};
		}

		private void CreateOCR()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = AsyncTexture2D.FromAssetId(759447);
			panel.SetLocalizedTitle = () => strings.OCRAndImageRecognition;
			panel.SetLocalizedTitleTooltip = () => strings.OCRAndImageRecognition_Tooltip;
			Panel headerPanel = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(10f));
			FlowPanel contentFlowPanel = flowPanel;
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel2).set_Width((((Container)this).get_ContentRegion().Width - 20) / 2);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(3f, 3f));
			((FlowPanel)flowPanel2).set_OuterControlPadding(new Vector2(5f));
			FlowPanel settingsFlowPanel = flowPanel2;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Parent((Container)(object)settingsFlowPanel);
			((Checkbox)checkbox).set_Checked(_settings.UseOCR.get_Value());
			checkbox.SetLocalizedText = () => strings.UseOCR;
			checkbox.SetLocalizedTooltip = () => strings.UseOCR_Tooltip;
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				_settings.UseOCR.set_Value(b);
			};
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Parent((Container)(object)settingsFlowPanel);
			((Checkbox)checkbox2).set_Checked(_settings.UseBetaGamestate.get_Value());
			checkbox2.SetLocalizedText = () => strings.UseBetaGameState;
			checkbox2.SetLocalizedTooltip = () => strings.UseBetaGameState_Tooltip;
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_settings.UseBetaGamestate.set_Value(b);
			};
			FlowPanel flowPanel3 = new FlowPanel();
			((Control)flowPanel3).set_Parent((Container)(object)headerPanel);
			((Control)flowPanel3).set_Location(new Point(((Control)settingsFlowPanel).get_Right(), 0));
			((Container)flowPanel3).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel3).set_Width((((Container)this).get_ContentRegion().Width - 20) / 2);
			((FlowPanel)flowPanel3).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel3).set_ControlPadding(new Vector2(3f, 3f));
			((FlowPanel)flowPanel3).set_OuterControlPadding(new Vector2(5f));
			FlowPanel buttonFlowPanel = flowPanel3;
			Button button = new Button();
			((Control)button).set_Parent((Container)(object)buttonFlowPanel);
			button.SetLocalizedText = () => strings.EditOCR;
			button.SetLocalizedTooltip = () => strings.EditOCR_Tooltip;
			((Control)button).set_Width(((Control)buttonFlowPanel).get_Width() - 15);
			((Control)button).set_Height(40);
			button.ClickAction = _ocr.ToggleContainer;
			_sharedSettingsView.CreateLayout((Container)(object)contentFlowPanel, ((Container)this).get_ContentRegion().Width - 20);
		}

		private void CreateKeybinds()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.SetLocalizedTitle = () => strings.Keybinds;
			panel.TitleIcon = AsyncTexture2D.FromAssetId(156734);
			Panel p = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)p);
			((Control)flowPanel).set_Location(new Point(5));
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel cP = flowPanel;
			KeybindingAssigner keybindingAssigner = new KeybindingAssigner();
			((Control)keybindingAssigner).set_Parent((Container)(object)cP);
			((Control)keybindingAssigner).set_Width(((Container)this).get_ContentRegion().Width - 35);
			((KeybindingAssigner)keybindingAssigner).set_KeyBinding(_settings.LogoutKey.get_Value());
			keybindingAssigner.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				SettingEntry<KeyBinding> logoutKey = _settings.LogoutKey;
				KeyBinding val5 = new KeyBinding();
				val5.set_ModifierKeys(kb.get_ModifierKeys());
				val5.set_PrimaryKey(kb.get_PrimaryKey());
				val5.set_Enabled(kb.get_Enabled());
				val5.set_IgnoreWhenInTextField(true);
				logoutKey.set_Value(val5);
			};
			keybindingAssigner.SetLocalizedKeyBindingName = () => strings.Logout;
			keybindingAssigner.SetLocalizedTooltip = () => strings.LogoutDescription;
			KeybindingAssigner keybindingAssigner2 = new KeybindingAssigner();
			((Control)keybindingAssigner2).set_Parent((Container)(object)cP);
			((Control)keybindingAssigner2).set_Width(((Container)this).get_ContentRegion().Width - 35);
			((KeybindingAssigner)keybindingAssigner2).set_KeyBinding(_settings.ShortcutKey.get_Value());
			keybindingAssigner2.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				SettingEntry<KeyBinding> shortcutKey = _settings.ShortcutKey;
				KeyBinding val4 = new KeyBinding();
				val4.set_ModifierKeys(kb.get_ModifierKeys());
				val4.set_PrimaryKey(kb.get_PrimaryKey());
				val4.set_Enabled(kb.get_Enabled());
				val4.set_IgnoreWhenInTextField(true);
				shortcutKey.set_Value(val4);
			};
			keybindingAssigner2.SetLocalizedKeyBindingName = () => strings.ShortcutToggle;
			keybindingAssigner2.SetLocalizedTooltip = () => strings.ShortcutToggle_Tooltip;
			KeybindingAssigner keybindingAssigner3 = new KeybindingAssigner();
			((Control)keybindingAssigner3).set_Parent((Container)(object)cP);
			((Control)keybindingAssigner3).set_Width(((Container)this).get_ContentRegion().Width - 35);
			((KeybindingAssigner)keybindingAssigner3).set_KeyBinding(_settings.RadialKey.get_Value());
			keybindingAssigner3.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				SettingEntry<KeyBinding> radialKey = _settings.RadialKey;
				KeyBinding val3 = new KeyBinding();
				val3.set_ModifierKeys(kb.get_ModifierKeys());
				val3.set_PrimaryKey(kb.get_PrimaryKey());
				val3.set_Enabled(kb.get_Enabled());
				val3.set_IgnoreWhenInTextField(true);
				radialKey.set_Value(val3);
			};
			keybindingAssigner3.SetLocalizedKeyBindingName = () => strings.RadialMenuKey;
			keybindingAssigner3.SetLocalizedTooltip = () => strings.RadialMenuKey_Tooltip;
			KeybindingAssigner keybindingAssigner4 = new KeybindingAssigner();
			((Control)keybindingAssigner4).set_Parent((Container)(object)cP);
			((Control)keybindingAssigner4).set_Width(((Container)this).get_ContentRegion().Width - 35);
			((KeybindingAssigner)keybindingAssigner4).set_KeyBinding(_settings.InventoryKey.get_Value());
			keybindingAssigner4.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				SettingEntry<KeyBinding> inventoryKey = _settings.InventoryKey;
				KeyBinding val2 = new KeyBinding();
				val2.set_ModifierKeys(kb.get_ModifierKeys());
				val2.set_PrimaryKey(kb.get_PrimaryKey());
				val2.set_Enabled(kb.get_Enabled());
				val2.set_IgnoreWhenInTextField(true);
				inventoryKey.set_Value(val2);
			};
			keybindingAssigner4.SetLocalizedKeyBindingName = () => strings.InventoryKey;
			keybindingAssigner4.SetLocalizedTooltip = () => strings.InventoryKey_Tooltip;
			KeybindingAssigner keybindingAssigner5 = new KeybindingAssigner();
			((Control)keybindingAssigner5).set_Parent((Container)(object)cP);
			((Control)keybindingAssigner5).set_Width(((Container)this).get_ContentRegion().Width - 35);
			((KeybindingAssigner)keybindingAssigner5).set_KeyBinding(_settings.MailKey.get_Value());
			keybindingAssigner5.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				SettingEntry<KeyBinding> mailKey = _settings.MailKey;
				KeyBinding val = new KeyBinding();
				val.set_ModifierKeys(kb.get_ModifierKeys());
				val.set_PrimaryKey(kb.get_PrimaryKey());
				val.set_Enabled(kb.get_Enabled());
				val.set_IgnoreWhenInTextField(true);
				mailKey.set_Value(val);
			};
			keybindingAssigner5.SetLocalizedKeyBindingName = () => strings.MailKey;
			keybindingAssigner5.SetLocalizedTooltip = () => strings.MailKey_Tooltip;
		}

		private void CreateBehavior()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.SetLocalizedTitle = () => strings.ModuleBehavior;
			panel.SetLocalizedTitleTooltip = () => strings.ModuleBehavior_Tooltip;
			panel.TitleIcon = AsyncTexture2D.FromAssetId(60968);
			Panel p = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)p);
			((Control)flowPanel).set_Location(new Point(5, 5));
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel cP = flowPanel;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Parent((Container)(object)cP);
			((Checkbox)checkbox).set_Checked(_settings.OnlyEnterOnExact.get_Value());
			checkbox.SetLocalizedText = () => strings.OnlyEnterOnExact;
			checkbox.SetLocalizedTooltip = () => strings.OnlyEnterOnExact_Tooltip;
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				_settings.OnlyEnterOnExact.set_Value(b);
			};
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Parent((Container)(object)cP);
			((Checkbox)checkbox2).set_Checked(_settings.EnterOnSwap.get_Value());
			checkbox2.SetLocalizedText = () => strings.EnterOnSwap;
			checkbox2.SetLocalizedTooltip = () => strings.EnterOnSwap_Tooltip;
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_settings.EnterOnSwap.set_Value(b);
			};
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Parent((Container)(object)cP);
			((Checkbox)checkbox3).set_Checked(_settings.OpenInventoryOnEnter.get_Value());
			checkbox3.SetLocalizedText = () => strings.OpenInventoryOnEnter;
			checkbox3.SetLocalizedTooltip = () => strings.OpenInventoryOnEnter_Tooltip;
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_settings.OpenInventoryOnEnter.set_Value(b);
			};
			Checkbox checkbox4 = new Checkbox();
			((Control)checkbox4).set_Parent((Container)(object)cP);
			((Checkbox)checkbox4).set_Checked(_settings.CloseWindowOnSwap.get_Value());
			checkbox4.SetLocalizedText = () => strings.CloseWindowOnSwap;
			checkbox4.SetLocalizedTooltip = () => strings.CloseWindowOnSwap_Tooltip;
			checkbox4.CheckedChangedAction = delegate(bool b)
			{
				_settings.CloseWindowOnSwap.set_Value(b);
			};
			Checkbox checkbox5 = new Checkbox();
			((Control)checkbox5).set_Parent((Container)(object)cP);
			((Checkbox)checkbox5).set_Checked(_settings.DoubleClickToEnter.get_Value());
			checkbox5.SetLocalizedText = () => strings.DoubleClickToEnter;
			checkbox5.SetLocalizedTooltip = () => strings.DoubleClickToEnter_Tooltip;
			checkbox5.CheckedChangedAction = delegate(bool b)
			{
				_settings.DoubleClickToEnter.set_Value(b);
			};
			Checkbox checkbox6 = new Checkbox();
			((Control)checkbox6).set_Parent((Container)(object)cP);
			((Checkbox)checkbox6).set_Checked(_settings.EnterToLogin.get_Value());
			checkbox6.SetLocalizedText = () => strings.EnterToLogin;
			checkbox6.SetLocalizedTooltip = () => strings.EnterToLogin_Tooltip;
			checkbox6.CheckedChangedAction = delegate(bool b)
			{
				_settings.EnterToLogin.set_Value(b);
			};
			Checkbox checkbox7 = new Checkbox();
			((Control)checkbox7).set_Parent((Container)(object)cP);
			((Checkbox)checkbox7).set_Checked(_settings.AutoSortCharacters.get_Value());
			checkbox7.SetLocalizedText = () => strings.AutoFix;
			checkbox7.SetLocalizedTooltip = () => strings.AutoFix_Tooltip;
			checkbox7.CheckedChangedAction = delegate(bool b)
			{
				_settings.AutoSortCharacters.set_Value(b);
			};
			Checkbox checkbox8 = new Checkbox();
			((Control)checkbox8).set_Parent((Container)(object)cP);
			((Checkbox)checkbox8).set_Checked(_settings.FilterDiacriticsInsensitive.get_Value());
			checkbox8.SetLocalizedText = () => strings.FilterDiacriticsInsensitive;
			checkbox8.SetLocalizedTooltip = () => strings.FilterDiacriticsInsensitive_Tooltip;
			checkbox8.CheckedChangedAction = delegate(bool b)
			{
				_settings.FilterDiacriticsInsensitive.set_Value(b);
			};
			Checkbox checkbox9 = new Checkbox();
			((Control)checkbox9).set_Parent((Container)(object)cP);
			((Checkbox)checkbox9).set_Checked(_settings.FilterAsOne.get_Value());
			checkbox9.SetLocalizedText = () => strings.FilterAsOne;
			checkbox9.SetLocalizedTooltip = () => strings.FilterAsOne_Tooltip;
			checkbox9.CheckedChangedAction = delegate(bool b)
			{
				_settings.FilterAsOne.set_Value(b);
			};
			Panel panel2 = new Panel();
			((Control)panel2).set_Parent((Container)(object)cP);
			((Container)panel2).set_WidthSizingMode((SizingMode)2);
			((Container)panel2).set_HeightSizingMode((SizingMode)1);
			Panel subP = panel2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)subP);
			((Label)label).set_AutoSizeWidth(true);
			((Control)label).set_Height(20);
			label.SetLocalizedText = () => string.Format(strings.CheckDistance, _settings.CheckDistance.get_Value());
			label.SetLocalizedTooltip = () => strings.CheckDistance_Tooltip;
			Label checkDistanceLabel = label;
			TrackBar trackBar = new TrackBar();
			((Control)trackBar).set_Location(new Point(225, 2));
			((Control)trackBar).set_Parent((Container)(object)subP);
			((TrackBar)trackBar).set_MinValue(0f);
			((TrackBar)trackBar).set_MaxValue(72f);
			((TrackBar)trackBar).set_Value((float)_settings.CheckDistance.get_Value());
			((Control)trackBar).set_Width(((Container)this).get_ContentRegion().Width - 35 - 225);
			trackBar.SetLocalizedTooltip = () => strings.CheckDistance_Tooltip;
			trackBar.ValueChangedAction = delegate(int num)
			{
				_settings.CheckDistance.set_Value(num);
				checkDistanceLabel.UserLocale_SettingChanged(null, null);
			};
		}

		private void CreateAppearance()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_0753: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = AsyncTexture2D.FromAssetId(156740);
			panel.SetLocalizedTitle = () => strings.Appearance;
			Panel p = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)p);
			((Control)flowPanel).set_Location(new Point(5, 5));
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel cP = flowPanel;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Parent((Container)(object)cP);
			((Checkbox)checkbox).set_Checked(_settings.ShowCornerIcon.get_Value());
			checkbox.SetLocalizedText = () => strings.ShowCorner_Name;
			checkbox.SetLocalizedTooltip = () => string.Format(strings.ShowCorner_Tooltip, BaseModule<Characters, MainWindow, SettingsModel>.ModuleName);
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				_settings.ShowCornerIcon.set_Value(b);
			};
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Parent((Container)(object)cP);
			((Checkbox)checkbox2).set_Checked(_settings.ShowRandomButton.get_Value());
			checkbox2.SetLocalizedText = () => strings.ShowRandomButton_Name;
			checkbox2.SetLocalizedTooltip = () => strings.ShowRandomButton_Tooltip;
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_settings.ShowRandomButton.set_Value(b);
			};
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Parent((Container)(object)cP);
			((Checkbox)checkbox3).set_Checked(_settings.ShowLastButton.get_Value());
			checkbox3.SetLocalizedText = () => strings.ShowLastButton;
			checkbox3.SetLocalizedTooltip = () => strings.ShowLastButton_Tooltip;
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_settings.ShowLastButton.set_Value(b);
			};
			Checkbox checkbox4 = new Checkbox();
			((Control)checkbox4).set_Parent((Container)(object)cP);
			((Checkbox)checkbox4).set_Checked(_settings.ShowDetailedTooltip.get_Value());
			checkbox4.SetLocalizedText = () => string.Format(strings.ShowItem, strings.DetailedTooltip);
			checkbox4.SetLocalizedTooltip = () => strings.DetailedTooltip_Tooltip;
			checkbox4.CheckedChangedAction = delegate(bool b)
			{
				_settings.ShowDetailedTooltip.set_Value(b);
			};
			Panel panel2 = new Panel();
			((Control)panel2).set_Parent((Container)(object)cP);
			((Container)panel2).set_WidthSizingMode((SizingMode)2);
			((Control)panel2).set_Height(30);
			Panel subP = panel2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)subP);
			((Label)label).set_AutoSizeWidth(true);
			((Control)label).set_Height(30);
			label.SetLocalizedText = () => strings.CharacterDisplaySize;
			Dropdown dropdown = new Dropdown();
			((Control)dropdown).set_Location(new Point(250, 0));
			((Control)dropdown).set_Parent((Container)(object)subP);
			dropdown.SetLocalizedItems = () => new List<string>
			{
				strings.Small,
				strings.Normal,
				strings.Large,
				strings.Custom
			};
			((Dropdown)dropdown).set_SelectedItem(_settings.PanelSize.get_Value().GetPanelSize());
			dropdown.ValueChangedAction = delegate(string b)
			{
				_settings.PanelSize.set_Value(b.GetPanelSize());
			};
			Panel panel3 = new Panel();
			((Control)panel3).set_Parent((Container)(object)cP);
			((Container)panel3).set_WidthSizingMode((SizingMode)2);
			((Control)panel3).set_Height(30);
			subP = panel3;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)subP);
			((Label)label2).set_AutoSizeWidth(true);
			((Control)label2).set_Height(30);
			label2.SetLocalizedText = () => strings.CharacterDisplayOption;
			Dropdown dropdown2 = new Dropdown();
			((Control)dropdown2).set_Parent((Container)(object)subP);
			((Control)dropdown2).set_Location(new Point(250, 0));
			dropdown2.SetLocalizedItems = () => new List<string>
			{
				strings.OnlyText,
				strings.OnlyIcons,
				strings.TextAndIcon
			};
			((Dropdown)dropdown2).set_SelectedItem(_settings.PanelLayout.get_Value().GetPanelLayout());
			dropdown2.ValueChangedAction = delegate(string b)
			{
				_settings.PanelLayout.set_Value(b.GetPanelLayout());
			};
			Panel panel4 = new Panel();
			((Control)panel4).set_Parent((Container)(object)cP);
			((Container)panel4).set_WidthSizingMode((SizingMode)2);
			((Control)panel4).set_Height(30);
			subP = panel4;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)subP);
			((Label)label3).set_AutoSizeWidth(true);
			label3.SetLocalizedText = () => string.Format(strings.FontSize, _settings.CustomCharacterFontSize.get_Value());
			_customFontSizeLabel = label3;
			Dropdown dropdown3 = new Dropdown();
			((Control)dropdown3).set_Parent((Container)(object)subP);
			((Control)dropdown3).set_Location(new Point(250, 0));
			((Dropdown)dropdown3).set_SelectedItem(_settings.CustomCharacterFontSize.get_Value().ToString());
			dropdown3.ValueChangedAction = delegate(string str)
			{
				GetFontSize(_settings.CustomCharacterFontSize, str);
				_customFontSizeLabel.UserLocale_SettingChanged(null, null);
			};
			_customFontSize = dropdown3;
			Panel panel5 = new Panel();
			((Control)panel5).set_Parent((Container)(object)cP);
			((Container)panel5).set_WidthSizingMode((SizingMode)2);
			((Control)panel5).set_Height(30);
			subP = panel5;
			Label label4 = new Label();
			((Control)label4).set_Parent((Container)(object)subP);
			((Label)label4).set_AutoSizeWidth(true);
			label4.SetLocalizedText = () => string.Format(strings.NameFontSize, _settings.CustomCharacterNameFontSize.get_Value());
			_customNameFontSizeLabel = label4;
			Dropdown dropdown4 = new Dropdown();
			((Control)dropdown4).set_Parent((Container)(object)subP);
			((Control)dropdown4).set_Location(new Point(250, 0));
			((Dropdown)dropdown4).set_SelectedItem(_settings.CustomCharacterNameFontSize.get_Value().ToString());
			dropdown4.ValueChangedAction = delegate(string str)
			{
				GetFontSize(_settings.CustomCharacterNameFontSize, str);
				_customNameFontSizeLabel.UserLocale_SettingChanged(null, null);
			};
			_customNameFontSize = dropdown4;
			foreach (object i in Enum.GetValues(typeof(FontSize)))
			{
				((Dropdown)_customFontSize).get_Items().Add($"{(int)i}");
				((Dropdown)_customNameFontSize).get_Items().Add($"{(int)i}");
			}
			Panel panel6 = new Panel();
			((Control)panel6).set_Parent((Container)(object)cP);
			((Container)panel6).set_WidthSizingMode((SizingMode)2);
			((Control)panel6).set_Height(30);
			subP = panel6;
			Label label5 = new Label();
			((Control)label5).set_Parent((Container)(object)subP);
			((Label)label5).set_AutoSizeWidth(true);
			label5.SetLocalizedText = () => string.Format(strings.IconSize, _settings.CustomCharacterIconSize.get_Value());
			Label iconSizeLabel = label5;
			TrackBar trackBar = new TrackBar();
			((Control)trackBar).set_Parent((Container)(object)subP);
			((Control)trackBar).set_Location(new Point(250, 0));
			((TrackBar)trackBar).set_MinValue(8f);
			((TrackBar)trackBar).set_MaxValue(256f);
			((Control)trackBar).set_Width(((Container)this).get_ContentRegion().Width - 35 - 250);
			((TrackBar)trackBar).set_Value((float)_settings.CustomCharacterIconSize.get_Value());
			trackBar.ValueChangedAction = delegate(int num)
			{
				_settings.CustomCharacterIconSize.set_Value(num);
				iconSizeLabel.UserLocale_SettingChanged(num, null);
			};
			Panel panel7 = new Panel();
			((Control)panel7).set_Parent((Container)(object)cP);
			((Container)panel7).set_WidthSizingMode((SizingMode)2);
			((Control)panel7).set_Height(30);
			subP = panel7;
			Checkbox checkbox5 = new Checkbox();
			((Control)checkbox5).set_Parent((Container)(object)subP);
			((Checkbox)checkbox5).set_Checked(_settings.CharacterPanelFixedWidth.get_Value());
			checkbox5.CheckedChangedAction = delegate(bool b)
			{
				_settings.CharacterPanelFixedWidth.set_Value(b);
			};
			checkbox5.SetLocalizedText = () => string.Format(strings.CardWith, _settings.CharacterPanelWidth.get_Value());
			checkbox5.SetLocalizedTooltip = () => string.Format(strings.CardWidth_Tooltip, _settings.CharacterPanelWidth.get_Value());
			Checkbox cardWidthCheckbox = checkbox5;
			TrackBar trackBar2 = new TrackBar();
			((Control)trackBar2).set_Parent((Container)(object)subP);
			((Control)trackBar2).set_Location(new Point(250, 0));
			((TrackBar)trackBar2).set_MinValue(25f);
			((TrackBar)trackBar2).set_MaxValue(750f);
			((Control)trackBar2).set_Width(((Container)this).get_ContentRegion().Width - 35 - 250);
			((TrackBar)trackBar2).set_Value((float)_settings.CharacterPanelWidth.get_Value());
			trackBar2.ValueChangedAction = delegate(int num)
			{
				_settings.CharacterPanelWidth.set_Value(num);
				cardWidthCheckbox.UserLocale_SettingChanged(null, null);
			};
		}

		private void CreateDelays()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.SetLocalizedTitle = () => strings.Delays;
			panel.TitleIcon = AsyncTexture2D.FromAssetId(155035);
			Panel p = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)p);
			((Control)flowPanel).set_Location(new Point(5));
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel cP = flowPanel;
			Panel panel2 = new Panel();
			((Control)panel2).set_Parent((Container)(object)cP);
			((Container)panel2).set_WidthSizingMode((SizingMode)2);
			((Container)panel2).set_HeightSizingMode((SizingMode)1);
			Panel subP = panel2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)subP);
			((Label)label).set_AutoSizeWidth(true);
			((Control)label).set_Height(20);
			label.SetLocalizedText = () => string.Format(strings.KeyDelay, _settings.KeyDelay.get_Value());
			label.SetLocalizedTooltip = () => strings.KeyDelay_Tooltip;
			Label keyDelayLabel = label;
			TrackBar trackBar = new TrackBar();
			((Control)trackBar).set_Location(new Point(225, 2));
			((Control)trackBar).set_Parent((Container)(object)subP);
			((TrackBar)trackBar).set_MinValue(0f);
			((TrackBar)trackBar).set_MaxValue(500f);
			((TrackBar)trackBar).set_Value((float)_settings.KeyDelay.get_Value());
			((Control)trackBar).set_Width(((Container)this).get_ContentRegion().Width - 35 - 225);
			trackBar.ValueChangedAction = delegate(int num)
			{
				_settings.KeyDelay.set_Value(num);
				keyDelayLabel.UserLocale_SettingChanged(null, null);
			};
			Panel panel3 = new Panel();
			((Control)panel3).set_Parent((Container)(object)cP);
			((Container)panel3).set_WidthSizingMode((SizingMode)2);
			((Container)panel3).set_HeightSizingMode((SizingMode)1);
			subP = panel3;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)subP);
			((Label)label2).set_AutoSizeWidth(true);
			((Control)label2).set_Height(20);
			label2.SetLocalizedText = () => string.Format(strings.FilterDelay, _settings.FilterDelay.get_Value());
			label2.SetLocalizedTooltip = () => strings.FilterDelay_Tooltip;
			Label filterDelayLabel = label2;
			TrackBar trackBar2 = new TrackBar();
			((Control)trackBar2).set_Location(new Point(225, 2));
			((Control)trackBar2).set_Parent((Container)(object)subP);
			((TrackBar)trackBar2).set_MinValue(0f);
			((TrackBar)trackBar2).set_MaxValue(500f);
			((TrackBar)trackBar2).set_Value((float)_settings.FilterDelay.get_Value());
			((Control)trackBar2).set_Width(((Container)this).get_ContentRegion().Width - 35 - 225);
			trackBar2.ValueChangedAction = delegate(int num)
			{
				_settings.FilterDelay.set_Value(num);
				filterDelayLabel.UserLocale_SettingChanged(num, null);
			};
			Panel panel4 = new Panel();
			((Control)panel4).set_Parent((Container)(object)cP);
			((Container)panel4).set_WidthSizingMode((SizingMode)2);
			((Container)panel4).set_HeightSizingMode((SizingMode)1);
			subP = panel4;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)subP);
			((Label)label3).set_AutoSizeWidth(true);
			((Control)label3).set_Height(20);
			label3.SetLocalizedText = () => string.Format(strings.SwapDelay, _settings.SwapDelay.get_Value());
			label3.SetLocalizedTooltip = () => strings.SwapDelay_Tooltip;
			Label swapDelayLabel = label3;
			TrackBar trackBar3 = new TrackBar();
			((Control)trackBar3).set_Location(new Point(225, 2));
			((Control)trackBar3).set_Parent((Container)(object)subP);
			((TrackBar)trackBar3).set_MinValue(0f);
			((TrackBar)trackBar3).set_MaxValue(60000f);
			((TrackBar)trackBar3).set_Value((float)_settings.SwapDelay.get_Value());
			((Control)trackBar3).set_Width(((Container)this).get_ContentRegion().Width - 35 - 225);
			trackBar3.ValueChangedAction = delegate(int num)
			{
				_settings.SwapDelay.set_Value(num);
				swapDelayLabel.UserLocale_SettingChanged(null, null);
			};
		}

		private void CreateGeneral()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.SetLocalizedTitle = () => strings.GeneralAndWindows;
			panel.TitleIcon = AsyncTexture2D.FromAssetId(157109);
			Panel p = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)p);
			((Control)flowPanel).set_Location(new Point(5));
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel cP = flowPanel;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Parent((Container)(object)cP);
			((Checkbox)checkbox).set_Checked(_settings.LoadCachedAccounts.get_Value());
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				_settings.LoadCachedAccounts.set_Value(b);
			};
			checkbox.SetLocalizedText = () => strings.LoadCachedAccounts;
			checkbox.SetLocalizedTooltip = () => strings.LoadCachedAccounts_Tooltip;
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Parent((Container)(object)cP);
			((Checkbox)checkbox2).set_Checked(_settings.ShowStatusWindow.get_Value());
			checkbox2.SetLocalizedText = () => strings.ShowStatusWindow_Name;
			checkbox2.SetLocalizedTooltip = () => strings.ShowStatusWindow_Tooltip;
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_settings.ShowStatusWindow.set_Value(b);
			};
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Parent((Container)(object)cP);
			((Checkbox)checkbox3).set_Checked(_settings.ShowChoyaSpinner.get_Value());
			checkbox3.SetLocalizedText = () => strings.ShowChoyaSpinner;
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_settings.ShowChoyaSpinner.set_Value(b);
			};
			Checkbox checkbox4 = new Checkbox();
			((Control)checkbox4).set_Parent((Container)(object)cP);
			((Checkbox)checkbox4).set_Checked(_settings.OpenSidemenuOnSearch.get_Value());
			checkbox4.CheckedChangedAction = delegate(bool b)
			{
				_settings.OpenSidemenuOnSearch.set_Value(b);
			};
			checkbox4.SetLocalizedText = () => strings.OpenSidemenuOnSearch;
			checkbox4.SetLocalizedTooltip = () => strings.OpenSidemenuOnSearch_Tooltip;
			Checkbox checkbox5 = new Checkbox();
			((Control)checkbox5).set_Parent((Container)(object)cP);
			((Checkbox)checkbox5).set_Checked(_settings.FocusSearchOnShow.get_Value());
			checkbox5.CheckedChangedAction = delegate(bool b)
			{
				_settings.FocusSearchOnShow.set_Value(b);
			};
			checkbox5.SetLocalizedText = () => strings.FocusSearchOnShow;
			checkbox5.SetLocalizedTooltip = () => strings.FocusSearchOnShow_Tooltip;
		}

		private void GetFontSize(SettingEntry<int> setting, string item)
		{
			if (int.TryParse(item, out var fontSize))
			{
				setting.set_Value(fontSize);
			}
		}

		public void OnLanguageChanged(object s = null, EventArgs e = null)
		{
			base.Name = string.Format(strings.ItemSettings, BaseModule<Characters, MainWindow, SettingsModel>.ModuleName ?? "");
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _tick >= 1000.0)
			{
				_tick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
				{
					_sharedSettingsView?.UpdateOffset();
				}
			}
		}
	}
}
