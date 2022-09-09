using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Blish_HUD.Extended
{
	public static class ChatUtil
	{
		private static readonly IReadOnlyDictionary<ModifierKeys, int> ModifierLookUp = new Dictionary<ModifierKeys, int>
		{
			{
				(ModifierKeys)2,
				18
			},
			{
				(ModifierKeys)1,
				17
			},
			{
				(ModifierKeys)4,
				16
			}
		};

		public static async Task Send(string text, KeyBinding messageKey)
		{
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text) && Focus(messageKey))
			{
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(65, sendToSystem: true);
				KeyboardUtil.Release(162, sendToSystem: true);
				Thread.Sleep(25);
				KeyboardUtil.Stroke(46, sendToSystem: true);
				Thread.Sleep(25);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(86, sendToSystem: true);
				KeyboardUtil.Release(162, sendToSystem: true);
				Thread.Sleep(25);
				KeyboardUtil.Stroke(13, sendToSystem: true);
				if (prevClipboardContent != null)
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
				}
			}
		}

		public static async Task Insert(string text, KeyBinding messageKey)
		{
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text) && Focus(messageKey))
			{
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(86, sendToSystem: true);
				KeyboardUtil.Release(162, sendToSystem: true);
				if (prevClipboardContent != null)
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
				}
			}
		}

		private static bool Focus(KeyBinding messageKey)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected I4, but got Unknown
			if (!GameService.Gw2Mumble.get_IsAvailable() || GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				return false;
			}
			KeyboardUtil.Release(160);
			KeyboardUtil.Release(161);
			int modifierKey;
			bool num = ModifierLookUp.TryGetValue(messageKey.get_ModifierKeys(), out modifierKey);
			if (num)
			{
				KeyboardUtil.Press(modifierKey, sendToSystem: true);
			}
			if ((int)messageKey.get_PrimaryKey() != 0)
			{
				KeyboardUtil.Stroke((int)messageKey.get_PrimaryKey(), sendToSystem: true);
			}
			if (num)
			{
				KeyboardUtil.Release(modifierKey, sendToSystem: true);
			}
			return true;
		}
	}
}
