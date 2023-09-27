using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.QoL.SubModules.GameResets
{
	public class GameResets : SubModule
	{
		private readonly FlowPanel _container;

		private readonly IconLabel _serverTime;

		private readonly IconLabel _serverReset;

		private readonly IconLabel _weeklyReset;

		private SettingEntry<bool> _showTooltips;

		private SettingEntry<bool> _showServerTime;

		private SettingEntry<bool> _showDailyReset;

		private SettingEntry<bool> _showWeeklyReset;

		private SettingEntry<bool> _showIcons;

		private SettingEntry<DateDisplayType> _dateDisplay;

		public override SubModuleType SubModuleType => SubModuleType.GameResets;

		public GameResets(SettingCollection settings)
			: base(settings)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			SubModuleUI uI_Elements = UI_Elements;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)flowPanel).set_Visible(base.Enabled);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)1);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)0);
			((Control)flowPanel).set_Height(GameService.Content.get_DefaultFont14().get_LineHeight() * 3 + 6);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)7);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(0f, 2f));
			Rectangle localBounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_LocalBounds();
			((Control)flowPanel).set_Location(((Rectangle)(ref localBounds)).get_Center());
			flowPanel.CaptureInput = false;
			FlowPanel item = flowPanel;
			_container = flowPanel;
			uI_Elements.Add((Control)(object)item);
			IconLabel iconLabel = new IconLabel();
			((Control)iconLabel).set_Parent((Container)(object)_container);
			iconLabel.Texture = new DetailedTexture(156692)
			{
				Size = new Point(GameService.Content.get_DefaultFont14().get_LineHeight())
			};
			iconLabel.Text = "0 days 00:00:00";
			((Control)iconLabel).set_BasicTooltipText("Weekly Reset");
			iconLabel.AutoSize = true;
			_weeklyReset = iconLabel;
			IconLabel iconLabel2 = new IconLabel();
			((Control)iconLabel2).set_Parent((Container)(object)_container);
			iconLabel2.Texture = new DetailedTexture(943979)
			{
				Size = new Point(GameService.Content.get_DefaultFont14().get_LineHeight())
			};
			iconLabel2.Text = "00:00:00";
			((Control)iconLabel2).set_BasicTooltipText("Server Reset");
			iconLabel2.AutoSize = true;
			_serverReset = iconLabel2;
			IconLabel iconLabel3 = new IconLabel();
			((Control)iconLabel3).set_Parent((Container)(object)_container);
			iconLabel3.Texture = new DetailedTexture(517180)
			{
				Size = new Point(GameService.Content.get_DefaultFont14().get_LineHeight()),
				TextureRegion = new Rectangle(4, 4, 24, 24)
			};
			iconLabel3.Text = "00:00:00";
			((Control)iconLabel3).set_BasicTooltipText("Server Time");
			iconLabel3.AutoSize = true;
			iconLabel3.ShowIcon = (_serverReset.ShowIcon = (_weeklyReset.ShowIcon = _showIcons.get_Value()));
			_serverTime = iconLabel3;
			SetPositions();
		}

		public override void Load()
		{
			base.Load();
			GameService.Gw2Mumble.get_UI().add_CompassSizeChanged((EventHandler<ValueEventArgs<Size>>)UI_CompassSizeChanged);
			GameService.Gw2Mumble.get_UI().add_IsCompassTopRightChanged((EventHandler<ValueEventArgs<bool>>)UI_IsCompassTopRightChanged);
			BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.ClientWindowService.ResolutionChanged += ClientWindowService_ResolutionChanged;
		}

		private void ClientWindowService_ResolutionChanged(object sender, ValueChangedEventArgs<Point> e)
		{
			SetPositions();
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Gw2Mumble.get_UI().remove_CompassSizeChanged((EventHandler<ValueEventArgs<Size>>)UI_CompassSizeChanged);
			GameService.Gw2Mumble.get_UI().remove_IsCompassTopRightChanged((EventHandler<ValueEventArgs<bool>>)UI_IsCompassTopRightChanged);
			if (BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance != null)
			{
				BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.ClientWindowService.ResolutionChanged -= ClientWindowService_ResolutionChanged;
			}
			_showServerTime.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeServerTimeVisibility);
			_showDailyReset.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeServerResetVisibility);
			_showWeeklyReset.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeWeeklyResetVisibility);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_showTooltips = settings.DefineSetting<bool>("_showTooltips", true, (Func<string>)null, (Func<string>)null);
			_showServerTime = settings.DefineSetting<bool>("_showServerTime", true, (Func<string>)null, (Func<string>)null);
			_showDailyReset = settings.DefineSetting<bool>("_showDailyReset", true, (Func<string>)null, (Func<string>)null);
			_showWeeklyReset = settings.DefineSetting<bool>("_showWeeklyReset", true, (Func<string>)null, (Func<string>)null);
			_showIcons = settings.DefineSetting<bool>("_showIcons", true, (Func<string>)null, (Func<string>)null);
			_dateDisplay = settings.DefineSetting<DateDisplayType>("_dateDisplay", DateDisplayType.Long, (Func<string>)null, (Func<string>)null);
			_showServerTime.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeServerTimeVisibility);
			_showDailyReset.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeServerResetVisibility);
			_showWeeklyReset.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeWeeklyResetVisibility);
			_showIcons.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ChangeShowIcons);
			_dateDisplay.add_SettingChanged((EventHandler<ValueChangedEventArgs<DateDisplayType>>)DateDisplay_SettingChanged);
		}

		private void DateDisplay_SettingChanged(object sender, ValueChangedEventArgs<DateDisplayType> e)
		{
			SetTexts();
		}

		private void ChangeShowIcons(object sender, ValueChangedEventArgs<bool> e)
		{
			_serverTime.ShowIcon = e.get_NewValue();
			_serverReset.ShowIcon = e.get_NewValue();
			_weeklyReset.ShowIcon = e.get_NewValue();
		}

		private void ChangeServerTimeVisibility(object sender, ValueChangedEventArgs<bool> e)
		{
			((Control)_serverTime).set_Visible(e.get_NewValue());
			Container parent = ((Control)_serverTime).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		private void ChangeServerResetVisibility(object sender, ValueChangedEventArgs<bool> e)
		{
			((Control)_serverReset).set_Visible(e.get_NewValue());
			Container parent = ((Control)_serverReset).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		private void ChangeWeeklyResetVisibility(object sender, ValueChangedEventArgs<bool> e)
		{
			((Control)_weeklyReset).set_Visible(e.get_NewValue());
			Container parent = ((Control)_weeklyReset).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		private void UI_IsCompassTopRightChanged(object sender, ValueEventArgs<bool> e)
		{
			SetPositions();
		}

		private void UI_CompassSizeChanged(object sender, ValueEventArgs<Size> e)
		{
			SetPositions();
		}

		protected override void Enable()
		{
			base.Enable();
			((Control)_container).set_Visible(true);
		}

		protected override void Disable()
		{
			base.Disable();
			((Control)_container).set_Visible(false);
		}

		public override void Update(GameTime gameTime)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			if (base.Enabled)
			{
				((Control)_container).set_Visible(base.Enabled && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen());
				IconLabel serverTime = _serverTime;
				IconLabel serverReset = _serverReset;
				IconLabel weeklyReset = _weeklyReset;
				FlowPanel container = _container;
				int num;
				if (!_showTooltips.get_Value())
				{
					num = 0;
				}
				else
				{
					Rectangle absoluteBounds = ((Control)_container).get_AbsoluteBounds();
					num = (((Rectangle)(ref absoluteBounds)).Contains(GameService.Input.get_Mouse().get_Position()) ? 22 : 0);
				}
				CaptureType? val = (CaptureType)num;
				container.Capture = val;
				CaptureType? val3 = (weeklyReset.Capture = val);
				CaptureType? val6 = (serverTime.Capture = (serverReset.Capture = val3));
				SetTexts();
				SetPositions();
			}
		}

		private void SetPositions()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			Size s = GameService.Gw2Mumble.get_UI().get_CompassSize();
			float scale = (float)((Size)(ref s)).get_Height() / 362f;
			scale = (((double)scale < 0.5) ? (scale - 0.3f) : scale);
			int y = (GameService.Gw2Mumble.get_UI().get_IsCompassTopRight() ? (((Size)(ref s)).get_Height() - ((Control)_container).get_Height() + (int)(24f * scale)) : (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_container).get_Height() - 60));
			FlowPanel container = _container;
			int num = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Size)(ref s)).get_Width();
			Size compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			((Control)container).set_Location(new Point(num - (int)((double)((Size)(ref compassSize)).get_Width() * 0.1), y));
		}

		private void SetTexts()
		{
			DateTime now = DateTime.UtcNow;
			DateTime nextDay = DateTime.UtcNow.AddDays(1.0);
			DateTime nextWeek = DateTime.UtcNow;
			for (int i = 0; i < 8; i++)
			{
				nextWeek = DateTime.UtcNow.AddDays(i);
				if (nextWeek.DayOfWeek == DayOfWeek.Monday && (nextWeek.Day != now.Day || now.Hour < 7 || (now.Hour == 7 && now.Minute < 30)))
				{
					break;
				}
			}
			DateTime t = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
			DateTime w = new DateTime(nextWeek.Year, nextWeek.Month, nextWeek.Day, 7, 30, 0);
			_serverTime.Text = $"{now.Hour}:{now.Minute:00}";
			TimeSpan weeklyReset = w.Subtract(now);
			_weeklyReset.Text = ((_dateDisplay.get_Value() == DateDisplayType.Long) ? string.Format("{1:0} {0} {2:00}:{3:00}:{4:00}", strings.Days, weeklyReset.Days, weeklyReset.Hours, weeklyReset.Minutes, weeklyReset.Seconds) : ((_dateDisplay.get_Value() == DateDisplayType.ShortDays) ? string.Format("{1:0}{0} {2:00}:{3:00}:{4:00}", strings.Days.Substring(0, 1), weeklyReset.Days, weeklyReset.Hours, weeklyReset.Minutes, weeklyReset.Seconds) : ((weeklyReset.Days > 0) ? string.Format("{1:0} {0}", strings.Days, weeklyReset.Days) : $"{weeklyReset.Hours:00}:{weeklyReset.Minutes:00}:{weeklyReset.Seconds:00}")));
			TimeSpan serverReset = t.Subtract(now);
			_serverReset.Text = $"{serverReset.Hours:00}:{serverReset.Minutes:00}:{serverReset.Seconds:00}";
		}

		public override void CreateSettingsPanel(FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)flowPanel);
			((Control)panel).set_Width(width);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = base.Icon.Texture;
			((Panel)panel).set_Title(SubModuleType.ToString());
			Panel headerPanel = panel;
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)3);
			flowPanel2.ContentPadding = new RectangleDimensions(5, 2);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel contentFlowPanel = flowPanel2;
			Func<string> localizedLabelContent = () => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}");
			Func<string> localizedTooltip = () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}");
			int width2 = width - 16;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Height(20);
			((Checkbox)checkbox).set_Checked(base.ShowInHotbar.get_Value());
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				base.ShowInHotbar.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width2, (Control)(object)checkbox);
			KeybindingAssigner keybindingAssigner = new KeybindingAssigner();
			((Control)keybindingAssigner).set_Parent((Container)(object)contentFlowPanel);
			((Control)keybindingAssigner).set_Width(width - 16);
			((KeybindingAssigner)keybindingAssigner).set_KeyBinding(base.HotKey.get_Value());
			keybindingAssigner.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Expected O, but got Unknown
				SettingEntry<KeyBinding> hotKey = base.HotKey;
				KeyBinding val = new KeyBinding();
				val.set_ModifierKeys(kb.get_ModifierKeys());
				val.set_PrimaryKey(kb.get_PrimaryKey());
				val.set_Enabled(kb.get_Enabled());
				val.set_IgnoreWhenInTextField(true);
				hotKey.set_Value(val);
			};
			keybindingAssigner.SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}");
			keybindingAssigner.SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}");
			Func<string> localizedLabelContent2 = () => strings.ShowServerTime_Name;
			Func<string> localizedTooltip2 = () => strings.ShowServerTime_Tooltip;
			int width3 = width - 16;
			Checkbox checkbox2 = new Checkbox();
			((Control)checkbox2).set_Height(20);
			((Checkbox)checkbox2).set_Checked(_showServerTime.get_Value());
			checkbox2.CheckedChangedAction = delegate(bool b)
			{
				_showServerTime.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent2, localizedTooltip2, (Container)(object)contentFlowPanel, width3, (Control)(object)checkbox2);
			Func<string> localizedLabelContent3 = () => strings.ShowDailyReset_Name;
			Func<string> localizedTooltip3 = () => strings.ShowDailyReset_Tooltip;
			int width4 = width - 16;
			Checkbox checkbox3 = new Checkbox();
			((Control)checkbox3).set_Height(20);
			((Checkbox)checkbox3).set_Checked(_showDailyReset.get_Value());
			checkbox3.CheckedChangedAction = delegate(bool b)
			{
				_showDailyReset.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent3, localizedTooltip3, (Container)(object)contentFlowPanel, width4, (Control)(object)checkbox3);
			Func<string> localizedLabelContent4 = () => strings.ShowWeeklyReset_Name;
			Func<string> localizedTooltip4 = () => strings.ShowWeeklyReset_Tooltip;
			int width5 = width - 16;
			Checkbox checkbox4 = new Checkbox();
			((Control)checkbox4).set_Height(20);
			((Checkbox)checkbox4).set_Checked(_showWeeklyReset.get_Value());
			checkbox4.CheckedChangedAction = delegate(bool b)
			{
				_showWeeklyReset.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent4, localizedTooltip4, (Container)(object)contentFlowPanel, width5, (Control)(object)checkbox4);
			Func<string> localizedLabelContent5 = () => strings.ShowIcons_Name;
			Func<string> localizedTooltip5 = () => strings.ShowIcons_Tooltip;
			int width6 = width - 16;
			Checkbox checkbox5 = new Checkbox();
			((Control)checkbox5).set_Height(20);
			((Checkbox)checkbox5).set_Checked(_showIcons.get_Value());
			checkbox5.CheckedChangedAction = delegate(bool b)
			{
				_showIcons.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent5, localizedTooltip5, (Container)(object)contentFlowPanel, width6, (Control)(object)checkbox5);
			Func<string> localizedLabelContent6 = () => strings.ShowTooltips_Name;
			Func<string> localizedTooltip6 = () => strings.ShowTooltips_Tooltip;
			int width7 = width - 16;
			Checkbox checkbox6 = new Checkbox();
			((Control)checkbox6).set_Height(20);
			((Checkbox)checkbox6).set_Checked(_showTooltips.get_Value());
			checkbox6.CheckedChangedAction = delegate(bool b)
			{
				_showTooltips.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent6, localizedTooltip6, (Container)(object)contentFlowPanel, width7, (Control)(object)checkbox6);
			Func<string> localizedLabelContent7 = () => strings.KeyboardLayout_Name;
			Func<string> localizedTooltip7 = () => strings.KeyboardLayout_Tooltip;
			int width8 = width - 16;
			Dropdown dropdown = new Dropdown();
			((Control)dropdown).set_Location(new Point(250, 0));
			((Control)dropdown).set_Parent((Container)(object)contentFlowPanel);
			dropdown.SetLocalizedItems = () => new List<string>
			{
				$"{DateDisplayType.Short}".SplitStringOnUppercase(),
				$"{DateDisplayType.ShortDays}".SplitStringOnUppercase(),
				$"{DateDisplayType.Long}".SplitStringOnUppercase()
			};
			((Dropdown)dropdown).set_SelectedItem($"{_dateDisplay.get_Value()}".SplitStringOnUppercase());
			dropdown.ValueChangedAction = delegate(string b)
			{
				_dateDisplay.set_Value(Enum.TryParse<DateDisplayType>(b.RemoveSpaces(), out var result) ? result : _dateDisplay.get_Value());
			};
			UI.WrapWithLabel(localizedLabelContent7, localizedTooltip7, (Container)(object)contentFlowPanel, width8, (Control)(object)dropdown);
		}
	}
}
