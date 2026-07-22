using System;
using System.Collections.Generic;
using System.Linq;
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
		public static VirtualKeyShort[] ModKeyMapping { get; } = new VirtualKeyShort[5]
		{
			(VirtualKeyShort)0,
			VirtualKeyShort.LCONTROL,
			VirtualKeyShort.LMENU,
			(VirtualKeyShort)0,
			VirtualKeyShort.LSHIFT
		};


		public static async Task ClickMouse(MouseButton mouseButton, Point pos, int clicks = 1, bool sendToSystem = false, bool moveMouse = false)
		{
			await ClickMouse(mouseButton, pos.X, pos.Y, clicks, sendToSystem, moveMouse);
		}

		public static async Task ClickMouse(MouseButton mouseButton = MouseButton.LEFT, int xPos = -1, int yPos = -1, int clicks = 1, bool sendToSystem = false, bool moveMouse = false)
		{
			if (moveMouse)
			{
				Blish_HUD.Controls.Intern.Mouse.SetPosition(xPos, yPos, sendToSystem);
				await Task.Delay(25);
			}
			for (int i = 0; i < clicks; i++)
			{
				Blish_HUD.Controls.Intern.Mouse.Press(mouseButton, xPos, yPos, sendToSystem);
				await Task.Delay(25);
			}
		}

		public static async Task SendKey(Keys key, bool sendToSystem = false)
		{
			Blish_HUD.Controls.Intern.Keyboard.Stroke((VirtualKeyShort)key, sendToSystem);
		}

		public static async Task SendKey(KeyBinding keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding.PrimaryKey, keybinding.ModifierKeys, sendToSystem, delay);
		}

		public static async Task SendKey(ModifierKeys modifier, Keys key, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(key, modifier, sendToSystem, delay);
		}

		public static async Task SendKey(Keys key, ModifierKeys modifier, bool sendToSystem = false, int delay = 25)
		{
			IEnumerable<Enum> modifiers = modifier.GetFlags();
			foreach (ModifierKeys mod2 in modifiers.Select((Enum v) => (ModifierKeys)(object)v))
			{
				Blish_HUD.Controls.Intern.Keyboard.Press(ModKeyMapping[(int)mod2], sendToSystem);
			}
			await Task.Delay(delay);
			Blish_HUD.Controls.Intern.Keyboard.Stroke((VirtualKeyShort)key, sendToSystem);
			await Task.Delay(delay);
			foreach (ModifierKeys mod in modifiers.Select((Enum v) => (ModifierKeys)(object)v))
			{
				Blish_HUD.Controls.Intern.Keyboard.Release(ModKeyMapping[(int)mod], sendToSystem);
			}
		}

		public static async Task SendKey(Keys[] modifiers, Keys key, bool sendToSystem = false, int delay = 25)
		{
			Keys[] array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				Blish_HUD.Controls.Intern.Keyboard.Press((VirtualKeyShort)array[i], sendToSystem);
			}
			await Task.Delay(delay);
			Blish_HUD.Controls.Intern.Keyboard.Stroke((VirtualKeyShort)key, sendToSystem);
			await Task.Delay(delay);
			array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				Blish_HUD.Controls.Intern.Keyboard.Release((VirtualKeyShort)array[i], sendToSystem);
			}
		}

		public static async Task Press(this KeyBinding keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding, sendToSystem, delay);
		}

		public static async Task Press(this SettingEntry<KeyBinding> keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding.Value, sendToSystem, delay);
		}
	}
}
