using System;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TargetYourFeet.Settings.Enums;
using TargetYourFeet.Settings.Services;

namespace TargetYourFeet.Features.TargetYourFeet.Services
{
	public class MouseMoveService : IDisposable
	{
		private SettingService _setting;

		private Point savedMousePosition = new Point(0, 0);

		private bool _serviceIsActive;

		private bool _waitingForRelease;

		private bool _waitingForFirstRelease;

		private bool _waitingForSecondRelease;

		private SettingEntry<KeyBinding> _keybind;

		private Keys _primary;

		private ModifierKeys _modifiers;

		private KeybindBehaviour _mode;

		public MouseMoveService(SettingService settings)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_setting = settings;
			_keybind = settings.TargetFeetKeybind;
			_keybind.get_Value().add_BindingChanged((EventHandler<EventArgs>)Value_BindingChanged);
			settings.KeybindBehaviour.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeybindBehaviour>>)KeybindBehaviour_SettingChanged);
			_mode = settings.KeybindBehaviour.get_Value();
			SetKeysFromKeyBinding(_keybind.get_Value());
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
			GameService.Input.get_Keyboard().add_KeyReleased((EventHandler<KeyboardEventArgs>)Keyboard_KeyReleased);
		}

		private void KeybindBehaviour_SettingChanged(object sender, ValueChangedEventArgs<KeybindBehaviour> e)
		{
			_mode = e.get_NewValue();
			_waitingForRelease = false;
			_waitingForFirstRelease = false;
			_waitingForSecondRelease = false;
			_serviceIsActive = false;
		}

		private void Value_BindingChanged(object sender, EventArgs e)
		{
			SetKeysFromKeyBinding(_keybind.get_Value());
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() && CheckTrigger(e.get_Key()))
			{
				LogicHandler(bindActive: true);
			}
		}

		private void Keyboard_KeyReleased(object sender, KeyboardEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() && !CheckTrigger(e.get_Key()))
			{
				LogicHandler(bindActive: false);
			}
		}

		private bool CheckTrigger(Keys key)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if ((int)_modifiers == 0 && (int)_primary == 0)
			{
				return false;
			}
			if ((ModifierKeys)(_modifiers & GameService.Input.get_Keyboard().get_ActiveModifiers()) == _modifiers && ((int)_primary == 0 || GameService.Input.get_Keyboard().get_KeysDown().Contains(_primary)))
			{
				return true;
			}
			return false;
		}

		private void LogicHandler(bool bindActive)
		{
			switch (_mode)
			{
			case KeybindBehaviour.Hold:
				if (bindActive && !_serviceIsActive)
				{
					_serviceIsActive = true;
					DoMoveLogic();
				}
				if (!bindActive && _serviceIsActive)
				{
					_serviceIsActive = false;
					DoRestoreLogic();
				}
				break;
			case KeybindBehaviour.Toggle:
				if (bindActive && !_serviceIsActive && !_waitingForRelease && !_waitingForFirstRelease && !_waitingForSecondRelease)
				{
					_waitingForRelease = true;
					_waitingForFirstRelease = true;
					_waitingForSecondRelease = false;
				}
				else if (!bindActive && !_serviceIsActive && _waitingForRelease && _waitingForFirstRelease && !_waitingForSecondRelease)
				{
					_serviceIsActive = true;
					_waitingForRelease = false;
					_waitingForFirstRelease = false;
					DoMoveLogic();
				}
				else if (bindActive && _serviceIsActive && !_waitingForRelease && !_waitingForFirstRelease && !_waitingForSecondRelease)
				{
					_waitingForRelease = true;
					_waitingForSecondRelease = true;
				}
				else if (!bindActive && _serviceIsActive && _waitingForRelease && !_waitingForFirstRelease && _waitingForSecondRelease)
				{
					_serviceIsActive = false;
					_waitingForRelease = false;
					_waitingForSecondRelease = false;
					DoRestoreLogic();
				}
				break;
			case KeybindBehaviour.MoveOnly:
				if (bindActive && !_serviceIsActive)
				{
					_serviceIsActive = true;
					DoMoveLogic(ignoreActionCam: true);
				}
				if (!bindActive && _serviceIsActive)
				{
					_serviceIsActive = false;
				}
				break;
			}
		}

		private void DoMoveLogic(bool ignoreActionCam = false)
		{
			if (!ignoreActionCam && _setting.ActionCamInUse.get_Value())
			{
				DoHotKey(_setting.ActionCamKeybind.get_Value());
			}
			StoreMouseLocation();
			MoveMouseToHealthGlobe();
		}

		private void DoRestoreLogic()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			MoveMouse(savedMousePosition);
			if (_setting.ActionCamInUse.get_Value())
			{
				DoHotKey(_setting.ActionCamKeybind.get_Value());
			}
		}

		private void StoreMouseLocation()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			MouseState state = Mouse.GetState();
			savedMousePosition = ((MouseState)(ref state)).get_Position();
		}

		private void MoveMouse(Point p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			MoveMouse(p.X, p.Y);
		}

		private void MoveMouse(int x, int y)
		{
			Mouse.SetPosition(x, y);
		}

		private void MoveMouseToHealthGlobe()
		{
			MoveMouse(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() - 50);
		}

		private void SetKeysFromKeyBinding(KeyBinding keybind)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_primary = keybind.get_PrimaryKey();
			_modifiers = keybind.get_ModifierKeys();
		}

		public void Dispose()
		{
			_setting.KeybindBehaviour.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeybindBehaviour>>)KeybindBehaviour_SettingChanged);
			_keybind.get_Value().add_BindingChanged((EventHandler<EventArgs>)Value_BindingChanged);
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
			GameService.Input.get_Keyboard().remove_KeyReleased((EventHandler<KeyboardEventArgs>)Keyboard_KeyReleased);
		}

		protected void DoHotKey(KeyBinding key)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			if (key == null)
			{
				return;
			}
			if ((int)key.get_ModifierKeys() != 0)
			{
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Press((VirtualKeyShort)18, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Press((VirtualKeyShort)17, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Press((VirtualKeyShort)16, true);
				}
			}
			Keyboard.Press(ToVirtualKey(key.get_PrimaryKey()), true);
			Thread.Sleep(50);
			Keyboard.Release(ToVirtualKey(key.get_PrimaryKey()), true);
			if ((int)key.get_ModifierKeys() != 0)
			{
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Release((VirtualKeyShort)16, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Release((VirtualKeyShort)17, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Release((VirtualKeyShort)18, true);
				}
			}
		}

		private VirtualKeyShort ToVirtualKey(Keys key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return (VirtualKeyShort)(short)key;
			}
			catch
			{
				return (VirtualKeyShort)0;
			}
		}
	}
}
