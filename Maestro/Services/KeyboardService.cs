using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services
{
	public class KeyboardService
	{
		private static readonly Logger Logger = Logger.GetLogger<KeyboardService>();

		private static readonly Dictionary<Keys, VirtualKeyShort> KeyMapping = new Dictionary<Keys, VirtualKeyShort>
		{
			{
				(Keys)96,
				(VirtualKeyShort)96
			},
			{
				(Keys)97,
				(VirtualKeyShort)97
			},
			{
				(Keys)98,
				(VirtualKeyShort)98
			},
			{
				(Keys)99,
				(VirtualKeyShort)99
			},
			{
				(Keys)100,
				(VirtualKeyShort)100
			},
			{
				(Keys)101,
				(VirtualKeyShort)101
			},
			{
				(Keys)102,
				(VirtualKeyShort)102
			},
			{
				(Keys)103,
				(VirtualKeyShort)103
			},
			{
				(Keys)104,
				(VirtualKeyShort)104
			},
			{
				(Keys)105,
				(VirtualKeyShort)105
			}
		};

		public void KeyDown(Keys key)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (!KeyMapping.TryGetValue(key, out var virtualKey))
			{
				Logger.Warn($"Unknown key: {key}");
			}
			else
			{
				Keyboard.Press(virtualKey, true);
			}
		}

		public void KeyUp(Keys key)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (!KeyMapping.TryGetValue(key, out var virtualKey))
			{
				Logger.Warn($"Unknown key: {key}");
			}
			else
			{
				Keyboard.Release(virtualKey, true);
			}
		}
	}
}
