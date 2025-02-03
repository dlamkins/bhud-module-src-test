using System;
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
		public static VirtualKeyShort[] ModKeyMapping { get; } = new VirtualKeyShort[5]
		{
			(VirtualKeyShort)0,
			VirtualKeyShort.CONTROL,
			VirtualKeyShort.MENU,
			(VirtualKeyShort)0,
			VirtualKeyShort.LSHIFT
		};


		public static async Task<bool> PerformPress(this KeyBinding keybinding, int keyDelay = 0, bool triggerSystem = true, CancellationToken? cancellationToken = null)
		{
			ModifierKeys mods = keybinding.ModifierKeys;
			foreach (ModifierKeys mod2 in Enum.GetValues(typeof(ModifierKeys)))
			{
				if (mod2 != 0 && mods.HasFlag(mod2))
				{
					Keyboard.Press(ModKeyMapping[(int)mod2]);
					if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
					{
						return false;
					}
				}
			}
			Keyboard.Stroke((VirtualKeyShort)keybinding.PrimaryKey);
			if (triggerSystem)
			{
				Keyboard.Stroke((VirtualKeyShort)keybinding.PrimaryKey, sendToSystem: true);
			}
			if (cancellationToken.HasValue)
			{
				await Task.Delay(keyDelay, cancellationToken.Value);
			}
			foreach (ModifierKeys mod in Enum.GetValues(typeof(ModifierKeys)))
			{
				if (mod != 0 && mods.HasFlag(mod))
				{
					Keyboard.Release(ModKeyMapping[(int)mod]);
					if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
