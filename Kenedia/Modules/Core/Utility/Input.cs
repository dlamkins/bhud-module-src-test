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
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			await ClickMouse(mouseButton, pos.X, pos.Y, clicks, sendToSystem, moveMouse);
		}

		public static async Task ClickMouse(MouseButton mouseButton = MouseButton.LEFT, int xPos = -1, int yPos = -1, int clicks = 1, bool sendToSystem = false, bool moveMouse = false)
		{
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
			Keyboard.Stroke((VirtualKeyShort)key, sendToSystem);
		}

		public static async Task SendKey(KeyBinding keybinding, bool sendToSystem = false, int delay = 25)
		{
			await SendKey(keybinding.PrimaryKey, keybinding.ModifierKeys, sendToSystem, delay);
		}

		public static async Task SendKey(ModifierKeys modifier, Keys key, bool sendToSystem = false, int delay = 25)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			await SendKey(key, modifier, sendToSystem, delay);
		}

		public static async Task SendKey(Keys key, ModifierKeys modifier, bool sendToSystem = false, int delay = 25)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<Enum> modifiers = modifier.GetFlags();
			foreach (ModifierKeys mod2 in modifiers.Select((Enum v) => (ModifierKeys)(object)v))
			{
				Keyboard.Press(ModKeyMapping[(int)mod2], sendToSystem);
			}
			await Task.Delay(delay);
			Keyboard.Stroke((VirtualKeyShort)key, sendToSystem);
			await Task.Delay(delay);
			foreach (ModifierKeys mod in modifiers.Select((Enum v) => (ModifierKeys)(object)v))
			{
				Keyboard.Release(ModKeyMapping[(int)mod], sendToSystem);
			}
		}

		public static async Task SendKey(Keys[] modifiers, Keys key, bool sendToSystem = false, int delay = 25)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Keys[] array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				Keyboard.Press((VirtualKeyShort)array[i], sendToSystem);
			}
			await Task.Delay(delay);
			Keyboard.Stroke((VirtualKeyShort)key, sendToSystem);
			await Task.Delay(delay);
			array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				Keyboard.Release((VirtualKeyShort)array[i], sendToSystem);
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
