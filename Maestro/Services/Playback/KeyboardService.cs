using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Playback
{
	public class KeyboardService
	{
		private readonly Dictionary<Keys, SettingEntry<KeyBinding>> _keyRemappings;

		private readonly Dictionary<Keys, SettingEntry<KeyBinding>> _sharpRemappings;

		private readonly HashSet<Keys> _activeSharpKeys;

		private readonly HashSet<Keys> _heldKeys;

		private readonly DebugLogger _debugLogger = new DebugLogger();

		private bool _altHeld;

		private static bool ShouldSendKeys
		{
			get
			{
				if (GameService.GameIntegration.Gw2Instance.Gw2HasFocus)
				{
					return !GameService.Gw2Mumble.UI.IsTextInputFocused;
				}
				return false;
			}
		}

		public KeyboardService(Dictionary<Keys, SettingEntry<KeyBinding>> keyRemappings, Dictionary<Keys, SettingEntry<KeyBinding>> sharpRemappings)
		{
			_keyRemappings = keyRemappings;
			_sharpRemappings = sharpRemappings;
			_activeSharpKeys = new HashSet<Keys>();
			_heldKeys = new HashSet<Keys>();
		}

		public void StartDebugLog(string songName)
		{
		}

		public void StopDebugLog()
		{
		}

		public void KeyDown(Keys key)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldSendKeys)
			{
				SettingEntry<KeyBinding> sharpSetting;
				SettingEntry<KeyBinding> setting;
				if ((int)key == 164)
				{
					_altHeld = true;
				}
				else if (_altHeld && _sharpRemappings.TryGetValue(key, out sharpSetting))
				{
					_activeSharpKeys.Add(key);
					SendKeyBindingDown(sharpSetting.Value);
				}
				else if (_keyRemappings.TryGetValue(key, out setting))
				{
					_heldKeys.Add(key);
					Keyboard.Press((VirtualKeyShort)setting.Value.PrimaryKey, sendToSystem: true);
				}
			}
		}

		public void KeyUp(Keys key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			SettingEntry<KeyBinding> sharpSetting;
			SettingEntry<KeyBinding> setting;
			if ((int)key == 164)
			{
				_altHeld = false;
			}
			else if (_activeSharpKeys.Remove(key) && _sharpRemappings.TryGetValue(key, out sharpSetting))
			{
				SendKeyBindingUp(sharpSetting.Value);
			}
			else if (_keyRemappings.TryGetValue(key, out setting))
			{
				_heldKeys.Remove(key);
				Keyboard.Release((VirtualKeyShort)setting.Value.PrimaryKey, sendToSystem: true);
			}
		}

		public void ReleaseAllKeys()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			foreach (Keys key2 in _heldKeys)
			{
				if (_keyRemappings.TryGetValue(key2, out var setting))
				{
					Keyboard.Release((VirtualKeyShort)setting.Value.PrimaryKey, sendToSystem: true);
				}
			}
			_heldKeys.Clear();
			foreach (Keys key in _activeSharpKeys)
			{
				if (_sharpRemappings.TryGetValue(key, out var sharpSetting))
				{
					SendKeyBindingUp(sharpSetting.Value);
				}
			}
			_activeSharpKeys.Clear();
			_altHeld = false;
		}

		private static void SendKeyBindingDown(KeyBinding binding)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if (binding.ModifierKeys.HasFlag(ModifierKeys.Alt))
			{
				Keyboard.Press(VirtualKeyShort.LMENU, sendToSystem: true);
			}
			if (binding.ModifierKeys.HasFlag(ModifierKeys.Ctrl))
			{
				Keyboard.Press(VirtualKeyShort.LCONTROL, sendToSystem: true);
			}
			if (binding.ModifierKeys.HasFlag(ModifierKeys.Shift))
			{
				Keyboard.Press(VirtualKeyShort.LSHIFT, sendToSystem: true);
			}
			Keyboard.Press((VirtualKeyShort)binding.PrimaryKey, sendToSystem: true);
		}

		private static void SendKeyBindingUp(KeyBinding binding)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			Keyboard.Release((VirtualKeyShort)binding.PrimaryKey, sendToSystem: true);
			if (binding.ModifierKeys.HasFlag(ModifierKeys.Shift))
			{
				Keyboard.Release(VirtualKeyShort.LSHIFT, sendToSystem: true);
			}
			if (binding.ModifierKeys.HasFlag(ModifierKeys.Ctrl))
			{
				Keyboard.Release(VirtualKeyShort.LCONTROL, sendToSystem: true);
			}
			if (binding.ModifierKeys.HasFlag(ModifierKeys.Alt))
			{
				Keyboard.Release(VirtualKeyShort.LMENU, sendToSystem: true);
			}
		}
	}
}
