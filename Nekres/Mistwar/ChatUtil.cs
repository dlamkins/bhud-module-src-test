using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Input;

namespace Nekres.Mistwar
{
	internal static class ChatUtil
	{
		private static readonly IReadOnlyDictionary<ModifierKeys, VirtualKeyShort> ModifierLookUp = new Dictionary<ModifierKeys, VirtualKeyShort>
		{
			{
				(ModifierKeys)2,
				(VirtualKeyShort)18
			},
			{
				(ModifierKeys)1,
				(VirtualKeyShort)17
			},
			{
				(ModifierKeys)4,
				(VirtualKeyShort)16
			}
		};

		public static async Task PastText(string text)
		{
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text))
			{
				Focus();
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)65, true);
				Thread.Sleep(1);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)46, true);
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)86, true);
				Thread.Sleep(1);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)13, false);
				UnFocus();
				if (prevClipboardContent != null)
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
				}
			}
		}

		public static async Task InsertText(string text)
		{
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text))
			{
				Focus();
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)86, true);
				Thread.Sleep(50);
				Keyboard.Release((VirtualKeyShort)162, true);
				if (prevClipboardContent != null)
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
				}
			}
		}

		private static void Focus()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			UnFocus();
			if ((int)MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_ModifierKeys() != 0)
			{
				Keyboard.Press(ModifierLookUp[MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_ModifierKeys()], false);
			}
			if ((int)MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_PrimaryKey() != 0)
			{
				Keyboard.Press((VirtualKeyShort)(short)MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_PrimaryKey(), false);
				Keyboard.Release((VirtualKeyShort)(short)MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_PrimaryKey(), false);
			}
			if ((int)MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_ModifierKeys() != 0)
			{
				Keyboard.Release(ModifierLookUp[MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value().get_ModifierKeys()], false);
			}
		}

		private static void UnFocus()
		{
			Mouse.Click((MouseButton)0, GameService.Graphics.get_WindowWidth() - 1, -1, false);
		}
	}
}
