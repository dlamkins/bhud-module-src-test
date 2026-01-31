using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Maestro.Models;
using Maestro.UI.Main;
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
				if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
				{
					return !SongFilterBar.IsTextInputFocused;
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
					SendKeyBindingDown(sharpSetting.get_Value());
				}
				else if (_keyRemappings.TryGetValue(key, out setting))
				{
					_heldKeys.Add(key);
					Keyboard.Press((VirtualKeyShort)(short)setting.get_Value().get_PrimaryKey(), true);
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
				SendKeyBindingUp(sharpSetting.get_Value());
			}
			else if (_keyRemappings.TryGetValue(key, out setting))
			{
				_heldKeys.Remove(key);
				Keyboard.Release((VirtualKeyShort)(short)setting.get_Value().get_PrimaryKey(), true);
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
					Keyboard.Release((VirtualKeyShort)(short)setting.get_Value().get_PrimaryKey(), true);
				}
			}
			_heldKeys.Clear();
			foreach (Keys key in _activeSharpKeys)
			{
				if (_sharpRemappings.TryGetValue(key, out var sharpSetting))
				{
					SendKeyBindingUp(sharpSetting.get_Value());
				}
			}
			_activeSharpKeys.Clear();
			_altHeld = false;
		}

		public void PlayNote(Keys key, bool isSharp = false)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldSendKeys)
			{
				if (isSharp && _sharpRemappings.TryGetValue(key, out var sharpSetting))
				{
					SendKeyBindingDown(sharpSetting.get_Value());
					SendKeyBindingUp(sharpSetting.get_Value());
				}
				else
				{
					KeyDown(key);
					KeyUp(key);
				}
			}
		}

		public void PlayNoteByName(string note, bool isSharp = false, bool isHighC = false)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (!ShouldSendKeys)
			{
				return;
			}
			if (isHighC)
			{
				PlayNote((Keys)104);
			}
			else
			{
				if (!NoteMapping.TryParse(note, out var noteName))
				{
					return;
				}
				if (isSharp)
				{
					Keys? sharpKey = NoteMapping.GetSharpKey(noteName);
					if (sharpKey.HasValue)
					{
						PlayNote(sharpKey.Value, isSharp: true);
					}
				}
				else
				{
					Keys? naturalKey = NoteMapping.GetNaturalKey(noteName);
					if (naturalKey.HasValue)
					{
						PlayNote(naturalKey.Value);
					}
				}
			}
		}

		public void PlayOctaveChange(bool up)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldSendKeys)
			{
				Keys key = (Keys)(up ? 105 : 96);
				KeyDown(key);
				KeyUp(key);
			}
		}

		public void ResetToMiddleOctave()
		{
			if (ShouldSendKeys)
			{
				for (int i = 0; i < 5; i++)
				{
					KeyDown((Keys)96);
					KeyUp((Keys)96);
					Thread.Sleep(100);
				}
				KeyDown((Keys)105);
				KeyUp((Keys)105);
				Thread.Sleep(100);
			}
		}

		private static void SendKeyBindingDown(KeyBinding binding)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				Keyboard.Press((VirtualKeyShort)164, true);
			}
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				Keyboard.Press((VirtualKeyShort)162, true);
			}
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				Keyboard.Press((VirtualKeyShort)160, true);
			}
			Keyboard.Press((VirtualKeyShort)(short)binding.get_PrimaryKey(), true);
		}

		private static void SendKeyBindingUp(KeyBinding binding)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Keyboard.Release((VirtualKeyShort)(short)binding.get_PrimaryKey(), true);
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				Keyboard.Release((VirtualKeyShort)160, true);
			}
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				Keyboard.Release((VirtualKeyShort)162, true);
			}
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				Keyboard.Release((VirtualKeyShort)164, true);
			}
		}
	}
}
