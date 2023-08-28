using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Extensions
{
	public static class KeybindExtension
	{
		public static VirtualKeyShort[] ModKeyMapping { get; }

		public static async Task<bool> PerformPress(this KeyBinding keybinding, int keyDelay = 0, bool triggerSystem = true, CancellationToken? cancellationToken = null)
		{
			ModifierKeys mods = keybinding.get_ModifierKeys();
			foreach (ModifierKeys mod2 in Enum.GetValues(typeof(ModifierKeys)))
			{
				if ((int)mod2 != 0 && ((Enum)mods).HasFlag((Enum)(object)mod2))
				{
					Keyboard.Press(ModKeyMapping[mod2], false);
					if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
					{
						return false;
					}
				}
			}
			Keyboard.Stroke((VirtualKeyShort)(short)keybinding.get_PrimaryKey(), false);
			if (triggerSystem)
			{
				Keyboard.Stroke((VirtualKeyShort)(short)keybinding.get_PrimaryKey(), true);
			}
			if (cancellationToken.HasValue)
			{
				await Task.Delay(keyDelay, cancellationToken.Value);
			}
			foreach (ModifierKeys mod in Enum.GetValues(typeof(ModifierKeys)))
			{
				if ((int)mod != 0 && ((Enum)mods).HasFlag((Enum)(object)mod))
				{
					Keyboard.Release(ModKeyMapping[mod], false);
					if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
					{
						return false;
					}
				}
			}
			return true;
		}

		static KeybindExtension()
		{
			VirtualKeyShort[] array = new VirtualKeyShort[5];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			ModKeyMapping = (VirtualKeyShort[])(object)array;
		}
	}
}
