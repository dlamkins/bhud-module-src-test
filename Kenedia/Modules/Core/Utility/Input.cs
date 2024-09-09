using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Utility
{
	public static class Input
	{
		public static VirtualKeyShort[] ModKeyMapping { get; }

		public static async Task ClickMouse(MouseButton mouseButton, Point pos, int clicks = 1, bool sendToSystem = false, bool moveMouse = false)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			await ClickMouse(mouseButton, pos.X, pos.Y, clicks, sendToSystem, moveMouse);
		}

		public static async Task ClickMouse(MouseButton mouseButton = 0, int xPos = -1, int yPos = -1, int clicks = 1, bool sendToSystem = false, bool moveMouse = false)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (moveMouse)
			{
				Mouse.SetPosition(xPos, yPos, sendToSystem);
				await Task.Delay(25);
			}
			for (int i = 0; i < clicks; i++)
			{
				Mouse.Press(mouseButton, xPos, yPos, sendToSystem);
				await Task.Delay(25);
			}
		}

		public static async Task SendKey(Keys key, bool sendToSystem = false)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Keyboard.Stroke((VirtualKeyShort)(short)key, sendToSystem);
		}

		public static async Task SendKey(KeyBinding keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding.get_PrimaryKey(), keybinding.get_ModifierKeys(), sendToSystem, delay);
		}

		public static async Task SendKey(ModifierKeys modifier, Keys key, bool sendToSystem = false, int delay = 25)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			await SendKey(key, modifier, sendToSystem, delay);
		}

		public static async Task SendKey(Keys key, ModifierKeys modifier, bool sendToSystem = false, int delay = 25)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<Enum> modifiers = ((Enum)(object)modifier).GetFlags();
			foreach (ModifierKeys mod2 in modifiers.Select((Func<Enum, ModifierKeys>)((Enum v) => (ModifierKeys)(object)v)))
			{
				Keyboard.Press(ModKeyMapping[mod2], sendToSystem);
			}
			await Task.Delay(delay);
			Keyboard.Stroke((VirtualKeyShort)(short)key, sendToSystem);
			await Task.Delay(delay);
			foreach (ModifierKeys mod in modifiers.Select((Func<Enum, ModifierKeys>)((Enum v) => (ModifierKeys)(object)v)))
			{
				Keyboard.Release(ModKeyMapping[mod], sendToSystem);
			}
		}

		public static async Task SendKey(Keys[] modifiers, Keys key, bool sendToSystem = false, int delay = 25)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Keys[] array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				Keyboard.Press((VirtualKeyShort)(short)array[i], sendToSystem);
			}
			await Task.Delay(delay);
			Keyboard.Stroke((VirtualKeyShort)(short)key, sendToSystem);
			await Task.Delay(delay);
			array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				Keyboard.Release((VirtualKeyShort)(short)array[i], sendToSystem);
			}
		}

		public static async Task Press(this KeyBinding keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding, sendToSystem, delay);
		}

		public static async Task Press(this SettingEntry<KeyBinding> keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding.get_Value(), sendToSystem, delay);
		}

		static Input()
		{
			VirtualKeyShort[] array = new VirtualKeyShort[5];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			ModKeyMapping = (VirtualKeyShort[])(object)array;
		}
	}
}
