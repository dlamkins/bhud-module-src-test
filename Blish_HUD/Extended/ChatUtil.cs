using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
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
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text))
			{
				Focus(messageKey);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(65, sendToSystem: true);
				Thread.Sleep(1);
				KeyboardUtil.Release(162, sendToSystem: true);
				KeyboardUtil.Stroke(46, sendToSystem: true);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(86, sendToSystem: true);
				Thread.Sleep(1);
				KeyboardUtil.Release(162, sendToSystem: true);
				KeyboardUtil.Stroke(13);
				if (prevClipboardContent != null)
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
				}
			}
		}

		public static async Task Insert(string text, KeyBinding messageKey)
		{
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text))
			{
				Focus(messageKey);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(86, sendToSystem: true);
				Thread.Sleep(1);
				KeyboardUtil.Release(162, sendToSystem: true);
				if (prevClipboardContent != null)
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
				}
			}
		}

		private static void Focus(KeyBinding messageKey)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected I4, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected I4, but got Unknown
			UnFocus();
			KeyboardUtil.Release(160);
			KeyboardUtil.Release(161);
			int modifierKey;
			bool num = ModifierLookUp.TryGetValue(messageKey.get_ModifierKeys(), out modifierKey);
			if (num)
			{
				KeyboardUtil.Press(modifierKey);
			}
			if ((int)messageKey.get_PrimaryKey() != 0)
			{
				KeyboardUtil.Press((int)messageKey.get_PrimaryKey());
				KeyboardUtil.Release((int)messageKey.get_PrimaryKey());
			}
			if (num)
			{
				KeyboardUtil.Release(modifierKey);
			}
		}

		private static void UnFocus()
		{
			if (WindowUtil.GetInnerBounds(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out var bounds))
			{
				MouseUtil.Click(MouseUtil.MouseButton.LEFT, ((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top() + 1);
			}
		}
	}
}
