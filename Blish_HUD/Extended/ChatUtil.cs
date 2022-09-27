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
				Thread.Sleep(1);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(65, sendToSystem: true);
				Thread.Sleep(1);
				KeyboardUtil.Release(162, sendToSystem: true);
				KeyboardUtil.Stroke(46, sendToSystem: true);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(86, sendToSystem: true);
				Thread.Sleep(50);
				KeyboardUtil.Release(162, sendToSystem: true);
				Thread.Sleep(1);
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
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text) && Focus(messageKey))
			{
				Thread.Sleep(1);
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

		private static bool Focus(KeyBinding messageKey)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected I4, but got Unknown
			if (IsBusy())
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

		private static bool IsBusy()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();
			}
			return true;
		}
	}
}
