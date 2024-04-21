using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard.Exceptions;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Blish_HUD.Extended
{
	public static class ChatUtil
	{
		public const int MAX_MESSAGE_LENGTH = 199;

		private static Logger _logger = Logger.GetLogger(typeof(ChatUtil));

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

		public static void Send(string text, KeyBinding messageKey)
		{
			if (!IsTextValid(text) || !Focus(messageKey))
			{
				return;
			}
			try
			{
				byte[] prevClipboardContent = ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync().Result;
				if (!ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text).Result)
				{
					SetUnicodeBytesAsync(prevClipboardContent);
					return;
				}
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
				SetUnicodeBytesAsync(prevClipboardContent);
			}
			catch (Exception ex) when (((ex is ClipboardWindowsApiException || ex is ClipboardTimeoutException) ? 1 : 0) != 0)
			{
				_logger.Info(ex, ex.Message);
			}
		}

		public static void Insert(string text, KeyBinding messageKey)
		{
			if (!IsTextValid(text) || !Focus(messageKey))
			{
				return;
			}
			try
			{
				byte[] prevClipboardContent = ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync().Result;
				if (!ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text).Result)
				{
					SetUnicodeBytesAsync(prevClipboardContent);
					return;
				}
				Thread.Sleep(1);
				KeyboardUtil.Press(162, sendToSystem: true);
				KeyboardUtil.Stroke(86, sendToSystem: true);
				Thread.Sleep(1);
				KeyboardUtil.Release(162, sendToSystem: true);
				SetUnicodeBytesAsync(prevClipboardContent);
			}
			catch (Exception ex) when (((ex is ClipboardWindowsApiException || ex is ClipboardTimeoutException) ? 1 : 0) != 0)
			{
				_logger.Info(ex, ex.Message);
			}
		}

		private static bool Focus(KeyBinding messageKey)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected I4, but got Unknown
			if (IsBusy() || messageKey == null || ((int)messageKey.get_PrimaryKey() == 0 && (int)messageKey.get_ModifierKeys() == 0))
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

		private static async Task SetUnicodeBytesAsync(byte[] clipboardContent)
		{
			if (clipboardContent != null)
			{
				try
				{
					await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(clipboardContent);
				}
				catch (Exception ex) when (((ex is ClipboardWindowsApiException || ex is ClipboardTimeoutException) ? 1 : 0) != 0)
				{
					_logger.Info(ex, ex.Message);
				}
			}
		}

		private static bool IsBusy()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();
			}
			return true;
		}

		private static bool IsTextValid(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				_logger.Info("Invalid chat message. Argument 'text' was null or empty.");
				return false;
			}
			if (text.Length > 199)
			{
				_logger.Info(string.Format("Invalid chat message. Argument '{0}' exceeds limit of {1} characters. Value: \"{2}[..+{3}]\"", "text", 199, text.Substring(0, 25), 174));
				return false;
			}
			return true;
		}

		public static bool IsLengthValid(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				return message.Length <= 199;
			}
			return true;
		}
	}
}
