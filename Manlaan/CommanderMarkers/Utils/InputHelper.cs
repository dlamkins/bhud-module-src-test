using System;
using System.Threading;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class InputHelper
	{
		public static void DoHotKey(KeyBinding key)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
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

		private static VirtualKeyShort ToVirtualKey(Keys key)
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
