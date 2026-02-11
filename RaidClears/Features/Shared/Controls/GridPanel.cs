using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Features.Raids;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes;
using RaidClears.Settings.Enums;
using RaidClears.Settings.Models;

namespace RaidClears.Features.Shared.Controls
{
	public class GridPanel : FlowPanel
	{
		private static Vector2 DefaultPadding = new Vector2(2f, 2f);

		private readonly GenericSettings _settings;

		private readonly SettingEntry<bool> _screenClamp;

		private CornerIconService? _cornerIconService;

		private KeyBindHandlerService? _keyBindService;

		private bool _ignoreMouseInput;

		private bool _isDraggedByMouse;

		private Point _dragStart = Point.get_Zero();

		private bool IgnoreMouseInput
		{
			set
			{
				((Control)this).SetProperty<bool>(ref _ignoreMouseInput, value, true, "IgnoreMouseInput");
			}
		}

		protected GridPanel(GenericSettings settings, Container parent)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_screenClamp = Service.Settings.ScreenClamp;
			((FlowPanel)this).set_ControlPadding(DefaultPadding);
			IgnoreMouseInput = ShouldIgnoreMouse();
			((Control)this).set_Location(_settings.Location.get_Value());
			((Control)this).set_Visible(_settings.Visible.get_Value());
			((Control)this).set_Parent(parent);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			AddDragDelegates();
			_settings.Location.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)delegate(object _, ValueChangedEventArgs<Point> e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Location(e.get_NewValue());
			});
			_settings.PositionLock.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				IgnoreMouseInput = ShouldIgnoreMouse();
			});
			_settings.Tooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				IgnoreMouseInput = ShouldIgnoreMouse();
			});
			_screenClamp.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue())
				{
					ClampToSpriteScreen();
				}
			});
		}

		protected override void DisposeControl()
		{
			((FlowPanel)this).DisposeControl();
			_cornerIconService?.Dispose();
			_keyBindService?.Dispose();
		}

		private void DoUpdate()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (_isDraggedByMouse && _settings.PositionLock.get_Value())
			{
				Point nOffset = GameService.Input.get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				_dragStart = GameService.Input.get_Mouse().get_Position();
				SyncAnchoredPanelDuringDrag();
			}
		}

		private void SyncAnchoredPanelDuringDrag()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			if (!Service.Settings.StrikeSettings.AnchorToRaidPanel.get_Value())
			{
				return;
			}
			RaidPanel raidPanel = Service.RaidWindow;
			StrikesPanel strikesPanel = Service.StrikesWindow;
			if (raidPanel != null && strikesPanel != null)
			{
				Vector2 controlPadding = ((FlowPanel)raidPanel).get_ControlPadding();
				Point padding = ((Vector2)(ref controlPadding)).ToPoint();
				Layout layout = Service.Settings.RaidSettings.Style.Layout.get_Value();
				if (this == raidPanel)
				{
					bool flag = (((uint)(layout - 1) <= 1u) ? true : false);
					Point strikeLoc2 = (flag ? (((Control)this).get_Location() + new Point(((Control)this).get_Size().X + padding.X, 0)) : (((Control)this).get_Location() + new Point(0, ((Control)this).get_Size().Y + padding.Y)));
					((Control)strikesPanel).set_Location(strikeLoc2);
				}
				else if (this == strikesPanel)
				{
					bool flag = (((uint)(layout - 1) <= 1u) ? true : false);
					Point raidLoc = (flag ? (((Control)this).get_Location() - new Point(((Control)raidPanel).get_Size().X + padding.X, 0)) : (((Control)this).get_Location() - new Point(0, ((Control)raidPanel).get_Size().Y + padding.Y)));
					((Control)raidPanel).set_Location(raidLoc);
					flag = (((uint)(layout - 1) <= 1u) ? true : false);
					Point strikeLoc = (flag ? (((Control)raidPanel).get_Location() + new Point(((Control)raidPanel).get_Size().X + padding.X, 0)) : (((Control)raidPanel).get_Location() + new Point(0, ((Control)raidPanel).get_Size().Y + padding.Y)));
					((Control)strikesPanel).set_Location(strikeLoc);
				}
			}
		}

		private void AddDragDelegates()
		{
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				if (_settings.PositionLock.get_Value())
				{
					_isDraggedByMouse = true;
					_dragStart = GameService.Input.get_Mouse().get_Position();
				}
			});
			((Control)this).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				if (_settings.PositionLock.get_Value())
				{
					_isDraggedByMouse = false;
					ClampToSpriteScreen();
					if (Service.Settings.StrikeSettings.AnchorToRaidPanel.get_Value() && this == Service.StrikesWindow)
					{
						Service.Settings.RaidSettings.Generic.Location.set_Value(((Control)Service.RaidWindow).get_Location());
					}
				}
			});
		}

		private void ClampToSpriteScreen()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			if (_screenClamp.get_Value())
			{
				Point screenSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				if (((Control)this).get_Location().X < 0)
				{
					((Control)this).set_Location(new Point(0, ((Control)this).get_Location().Y));
				}
				if (((Control)this).get_Location().X + ((Control)this).get_Size().X > screenSize.X)
				{
					((Control)this).set_Location(new Point(screenSize.X - ((Control)this).get_Size().X, ((Control)this).get_Location().Y));
				}
				if (((Control)this).get_Location().Y < 0)
				{
					((Control)this).set_Location(new Point(((Control)this).get_Location().X, 0));
				}
				if (((Control)this).get_Location().Y + ((Control)this).get_Size().Y > screenSize.Y)
				{
					((Control)this).set_Location(new Point(((Control)this).get_Location().X, screenSize.Y - ((Control)this).get_Size().Y));
				}
				SyncAnchoredPanelDuringDrag();
			}
		}

		private bool ShouldIgnoreMouse()
		{
			if (!_settings.PositionLock.get_Value())
			{
				return !_settings.Tooltips.get_Value();
			}
			return false;
		}

		public override Control? TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!_ignoreMouseInput)
			{
				return ((Container)this).TriggerMouseInput(mouseEventType, ms);
			}
			return null;
		}

		public void RegisterCornerIconService(CornerIconService? service)
		{
			_cornerIconService = service;
		}

		public void RegisterKeyBindService(KeyBindHandlerService? service)
		{
			_keyBindService = service;
		}

		public void Update()
		{
			bool shouldBeVisible = _settings.Visible.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			if (shouldBeVisible && _settings.PositionLock.get_Value())
			{
				DoUpdate();
			}
			if (!((Control)this).get_Visible() && shouldBeVisible)
			{
				((Control)this).Show();
			}
			else if (((Control)this).get_Visible() && !shouldBeVisible)
			{
				((Control)this).Hide();
			}
		}
	}
}
