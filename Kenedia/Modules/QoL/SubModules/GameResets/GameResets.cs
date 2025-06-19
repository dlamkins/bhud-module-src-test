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
		private readonly Kenedia.Modules.Core.Controls.FlowPanel _container;

		private readonly IconLabel _serverTime;

		private readonly IconLabel _serverReset;

		private readonly IconLabel _weeklyReset;

		private SettingEntry<bool> _showTooltips;

		private SettingEntry<bool> _showServerTime;

		private SettingEntry<bool> _showDailyReset;

		private SettingEntry<bool> _showWeeklyReset;

		private SettingEntry<bool> _showIcons;

		private SettingEntry<bool> _autoPosition;

		private SettingEntry<DateDisplayType> _dateDisplay;

		private SettingEntry<Point> _resetPosition;

		private bool _editPosition;

		public override SubModuleType SubModuleType => SubModuleType.GameResets;

		public GameResets(SettingCollection settings)
			: base(settings)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			SubModuleUI uI_Elements = UI_Elements;
			Kenedia.Modules.Core.Controls.FlowPanel obj = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Visible = base.Enabled,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.Standard,
				Height = GameService.Content.DefaultFont14.get_LineHeight() * 3 + 6,
				FlowDirection = ControlFlowDirection.SingleBottomToTop,
				ControlPadding = new Vector2(0f, 2f),
				Location = _resetPosition.Value,
				CaptureInput = !_autoPosition.Value,
				CanDrag = !_autoPosition.Value
			};
			Kenedia.Modules.Core.Controls.FlowPanel item = obj;
			_container = obj;
			uI_Elements.Add(item);
			_weeklyReset = new IconLabel
			{
				Parent = _container,
				Texture = new DetailedTexture(156692)
				{
					Size = new Point(GameService.Content.DefaultFont14.get_LineHeight())
				},
				Text = "0 days 00:00:00",
				BasicTooltipText = "Weekly Reset",
				AutoSize = true
			};
			_serverReset = new IconLabel
			{
				Parent = _container,
				Texture = new DetailedTexture(943979)
				{
					Size = new Point(GameService.Content.DefaultFont14.get_LineHeight())
				},
				Text = "00:00:00",
				BasicTooltipText = "Server Reset",
				AutoSize = true
			};
			_serverTime = new IconLabel
			{
				Parent = _container,
				Texture = new DetailedTexture(517180)
				{
					Size = new Point(GameService.Content.DefaultFont14.get_LineHeight()),
					TextureRegion = new Rectangle(4, 4, 24, 24)
				},
				Text = "00:00:00",
				BasicTooltipText = "Server Time",
				AutoSize = true,
				ShowIcon = (_serverReset.ShowIcon = (_weeklyReset.ShowIcon = _showIcons.Value))
			};
			SetPositions();
			_container.Moved += Container_Moved;
		}

		private void Container_Moved(object sender, MovedEventArgs e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (!_autoPosition.Value)
			{
				_resetPosition.Value = _container.Location;
			}
		}

		public override void Load()
		{
			base.Load();
			GameService.Gw2Mumble.UI.CompassSizeChanged += UI_CompassSizeChanged;
			GameService.Gw2Mumble.UI.IsCompassTopRightChanged += UI_IsCompassTopRightChanged;
			BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.ClientWindowService.ResolutionChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Point>>(ClientWindowService_ResolutionChanged);
		}

		private void ClientWindowService_ResolutionChanged(object sender, Blish_HUD.ValueChangedEventArgs<Point> e)
		{
			SetPositions();
		}

		public override void Unload()
		{
			base.Unload();
			GameService.Gw2Mumble.UI.CompassSizeChanged -= UI_CompassSizeChanged;
			GameService.Gw2Mumble.UI.IsCompassTopRightChanged -= UI_IsCompassTopRightChanged;
			if (BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance != null)
			{
				BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.ClientWindowService.ResolutionChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Point>>(ClientWindowService_ResolutionChanged);
			}
			_showServerTime.SettingChanged -= ChangeServerTimeVisibility;
			_showDailyReset.SettingChanged -= ChangeServerResetVisibility;
			_showWeeklyReset.SettingChanged -= ChangeWeeklyResetVisibility;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			base.DefineSettings(settings);
			_showTooltips = settings.DefineSetting("_showTooltips", defaultValue: true);
			_showServerTime = settings.DefineSetting("_showServerTime", defaultValue: true);
			_showDailyReset = settings.DefineSetting("_showDailyReset", defaultValue: true);
			_showWeeklyReset = settings.DefineSetting("_showWeeklyReset", defaultValue: true);
			_showIcons = settings.DefineSetting("_showIcons", defaultValue: true);
			_autoPosition = settings.DefineSetting("_autoPosition", defaultValue: true);
			_dateDisplay = settings.DefineSetting("_dateDisplay", DateDisplayType.Long);
			Rectangle localBounds = GameService.Graphics.SpriteScreen.LocalBounds;
			_resetPosition = settings.DefineSetting<Point>("_resetPosition", ((Rectangle)(ref localBounds)).get_Center());
			_showServerTime.SettingChanged += ChangeServerTimeVisibility;
			_showDailyReset.SettingChanged += ChangeServerResetVisibility;
			_showWeeklyReset.SettingChanged += ChangeWeeklyResetVisibility;
			_showIcons.SettingChanged += ChangeShowIcons;
			_autoPosition.SettingChanged += AutoPosition_SettingChanged;
			_dateDisplay.SettingChanged += DateDisplay_SettingChanged;
		}

		private void AutoPosition_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			SetPositions();
		}

		private void DateDisplay_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<DateDisplayType> e)
		{
			SetTexts();
		}

		private void ChangeShowIcons(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_serverTime.ShowIcon = e.NewValue;
			_serverReset.ShowIcon = e.NewValue;
			_weeklyReset.ShowIcon = e.NewValue;
		}

		private void ChangeServerTimeVisibility(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_serverTime.Visible = e.NewValue;
			_serverTime.Parent?.Invalidate();
		}

		private void ChangeServerResetVisibility(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_serverReset.Visible = e.NewValue;
			_serverReset.Parent?.Invalidate();
		}

		private void ChangeWeeklyResetVisibility(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_weeklyReset.Visible = e.NewValue;
			_weeklyReset.Parent?.Invalidate();
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
			_container.Visible = true;
		}

		protected override void Disable()
		{
			base.Disable();
			_container.Visible = false;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (base.Enabled)
			{
				_container.Visible = base.Enabled && GameService.GameIntegration.Gw2Instance.IsInGame && !GameService.Gw2Mumble.UI.IsMapOpen;
				IconLabel serverTime = _serverTime;
				IconLabel serverReset = _serverReset;
				IconLabel weeklyReset = _weeklyReset;
				int value;
				if (!_showTooltips.Value)
				{
					value = 0;
				}
				else
				{
					Rectangle absoluteBounds = _container.AbsoluteBounds;
					value = (((Rectangle)(ref absoluteBounds)).Contains(GameService.Input.Mouse.Position) ? 22 : 0);
				}
				CaptureType? captureType = (CaptureType)value;
				weeklyReset.Capture = captureType;
				CaptureType? captureType4 = (serverTime.Capture = (serverReset.Capture = captureType));
				_container.Capture = (_autoPosition.Value ? _serverTime.Capture : null);
				SetTexts();
				SetPositions();
			}
		}

		private void SetPositions()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			if (_autoPosition.Value)
			{
				Size s = GameService.Gw2Mumble.UI.CompassSize;
				float scale = (float)((Size)(ref s)).get_Height() / 362f;
				scale = (((double)scale < 0.5) ? (scale - 0.3f) : scale);
				int y = (GameService.Gw2Mumble.UI.IsCompassTopRight ? (((Size)(ref s)).get_Height() - _container.Height + (int)(24f * scale)) : (GameService.Graphics.SpriteScreen.Height - _container.Height - 60));
				Kenedia.Modules.Core.Controls.FlowPanel container = _container;
				int num = GameService.Graphics.SpriteScreen.Width - ((Size)(ref s)).get_Width();
				Size compassSize = GameService.Gw2Mumble.UI.CompassSize;
				container.Location = new Point(num - (int)((double)((Size)(ref compassSize)).get_Width() * 0.1), y);
			}
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
			_weeklyReset.Text = ((_dateDisplay.Value == DateDisplayType.Long) ? string.Format("{1:0} {0} {2:00}:{3:00}:{4:00}", strings.Days, weeklyReset.Days, weeklyReset.Hours, weeklyReset.Minutes, weeklyReset.Seconds) : ((_dateDisplay.Value == DateDisplayType.ShortDays) ? string.Format("{1:0}{0} {2:00}:{3:00}:{4:00}", strings.Days.Substring(0, 1), weeklyReset.Days, weeklyReset.Hours, weeklyReset.Minutes, weeklyReset.Seconds) : ((weeklyReset.Days > 0) ? string.Format("{1:0} {0}", strings.Days, weeklyReset.Days) : $"{weeklyReset.Hours:00}:{weeklyReset.Minutes:00}:{weeklyReset.Seconds:00}")));
			TimeSpan serverReset = t.Subtract(now);
			_serverReset.Text = $"{serverReset.Hours:00}:{serverReset.Minutes:00}:{serverReset.Seconds:00}";
		}

		public override void CreateSettingsPanel(Kenedia.Modules.Core.Controls.FlowPanel flowPanel, int width)
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = flowPanel,
				Width = width,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = base.Icon.Texture,
				Title = SubModuleType.ToString()
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5, 2),
				ControlPadding = new Vector2(0f, 2f)
			};
			UI.WrapWithLabel(() => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}"), () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}"), contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = base.ShowInHotbar.Value,
				CheckedChangedAction = delegate(bool b)
				{
					base.ShowInHotbar.Value = b;
				}
			});
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = contentFlowPanel,
				Width = width - 16,
				KeyBinding = base.HotKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					base.HotKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}"),
				SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}")
			};
			UI.WrapWithLabel(() => strings.ShowServerTime_Name, () => strings.ShowServerTime_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _showServerTime.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_showServerTime.Value = b;
				}
			});
			UI.WrapWithLabel(() => strings.ShowDailyReset_Name, () => strings.ShowDailyReset_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _showDailyReset.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_showDailyReset.Value = b;
				}
			});
			UI.WrapWithLabel(() => strings.ShowWeeklyReset_Name, () => strings.ShowWeeklyReset_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _showWeeklyReset.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_showWeeklyReset.Value = b;
				}
			});
			UI.WrapWithLabel(() => strings.ShowIcons_Name, () => strings.ShowIcons_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _showIcons.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_showIcons.Value = b;
				}
			});
			Kenedia.Modules.Core.Controls.Checkbox autoPosCheckbox = null;
			Kenedia.Modules.Core.Controls.Checkbox editPosCheckbox = null;
			UI.WrapWithLabel(() => strings.AutoPosition_Name, () => strings.AutoPosition_Tooltip, contentFlowPanel, width - 16, autoPosCheckbox = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _autoPosition.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_autoPosition.Value = b;
					editPosCheckbox.Checked = !b && editPosCheckbox.Checked;
				}
			});
			UI.WrapWithLabel(() => strings.EditPosition_Name, () => strings.EditPosition_Tooltip, contentFlowPanel, width - 16, editPosCheckbox = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _editPosition,
				CheckedChangedAction = delegate(bool b)
				{
					//IL_003b: Unknown result type (might be due to invalid IL or missing references)
					//IL_004d: Unknown result type (might be due to invalid IL or missing references)
					_container.CaptureInput = b;
					_container.CanDrag = b;
					_container.Location = (b ? _resetPosition.Value : _container.Location);
					autoPosCheckbox.Checked = !b && _autoPosition.Value;
					_editPosition = b;
				}
			});
			UI.WrapWithLabel(() => strings.ShowTooltips_Name, () => strings.ShowTooltips_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = _showTooltips.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_showTooltips.Value = b;
				}
			});
			UI.WrapWithLabel(() => strings.DateFormat_Name, () => strings.DateFormat_Tooltip, contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Location = new Point(250, 0),
				Parent = contentFlowPanel,
				SetLocalizedItems = () => new List<string>
				{
					$"{DateDisplayType.Short}".SplitStringOnUppercase(),
					$"{DateDisplayType.ShortDays}".SplitStringOnUppercase(),
					$"{DateDisplayType.Long}".SplitStringOnUppercase()
				},
				SelectedItem = $"{_dateDisplay.Value}".SplitStringOnUppercase(),
				ValueChangedAction = delegate(string b)
				{
					_dateDisplay.Value = (Enum.TryParse<DateDisplayType>(b.RemoveSpaces(), out var result) ? result : _dateDisplay.Value);
				}
			});
		}
	}
}
