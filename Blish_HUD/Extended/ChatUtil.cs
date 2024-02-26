using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace Blish_HUD.Extended
{
	public static class ChatUtil
	{
		public const int MAX_MESSAGE_LENGTH = 199;

		private const int WAIT_MS = 250;

		private static readonly IReadOnlyDictionary<ModifierKeys, int> _modifierLookUp = new Dictionary<ModifierKeys, int>
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

		public static async Task<bool> Clear(KeyBinding messageKey)
		{
			return await Focus(messageKey) && KeyboardUtil.Clear();
		}

		public static async Task Send(string text, KeyBinding messageKey, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger(typeof(ChatUtil));
			}
			byte[] prevClipboardContent = null;
			try
			{
				prevClipboardContent = ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync().Result;
			}
			catch (Exception e)
			{
				logger.Debug(e, e.Message);
			}
			if (!(await SetTextAsync(text, logger)))
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
				return;
			}
			bool flag = !IsTextValid(text, logger);
			if (!flag)
			{
				flag = !(await Focus(messageKey));
			}
			if (flag)
			{
				return;
			}
			try
			{
				if (!KeyboardUtil.Paste() || !KeyboardUtil.Stroke(13))
				{
					logger.Info("Failed to send text to chat: " + text);
					await Unfocus();
				}
			}
			finally
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
			}
		}

		public static async Task SendWhisper(string recipient, string cmdAndMessage, KeyBinding messageKey, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger(typeof(ChatUtil));
			}
			byte[] prevClipboardContent = null;
			try
			{
				prevClipboardContent = ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync().Result;
			}
			catch (Exception e)
			{
				logger.Debug(e, e.Message);
			}
			if (!(await SetTextAsync(cmdAndMessage, logger)))
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
				return;
			}
			bool flag = !IsTextValid(cmdAndMessage, logger);
			if (!flag)
			{
				flag = !(await Focus(messageKey));
			}
			if (flag)
			{
				return;
			}
			try
			{
				if (!KeyboardUtil.Paste())
				{
					logger.Info("Failed to send text to chat: " + cmdAndMessage);
					await Unfocus();
					return;
				}
				if (!(await SetTextAsync(recipient.Trim(), logger)))
				{
					await Unfocus();
					return;
				}
				if (!KeyboardUtil.Paste())
				{
					logger.Info("Failed to paste recipient: " + recipient);
					await Unfocus();
					return;
				}
				await Task.Delay(1);
				KeyboardUtil.Stroke(9);
				await Task.Delay(1);
				KeyboardUtil.Stroke(13);
				await Task.Delay(50);
				await Unfocus();
			}
			finally
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
			}
		}

		public static async Task Insert(string text, KeyBinding messageKey, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger(typeof(ChatUtil));
			}
			byte[] prevClipboardContent = null;
			try
			{
				prevClipboardContent = ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync().Result;
			}
			catch (Exception e)
			{
				logger.Debug(e, e.Message);
			}
			if (!(await SetTextAsync(text, logger)))
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
				return;
			}
			bool flag = !IsTextValid(text, logger);
			if (!flag)
			{
				flag = !(await Focus(messageKey));
			}
			if (!flag)
			{
				KeyboardUtil.Paste();
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
			}
		}

		private static async Task<bool> Focus(KeyBinding messageKey)
		{
			return await Task.Run(delegate
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Expected I4, but got Unknown
				if (GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
				{
					return true;
				}
				if (messageKey == null || ((int)messageKey.get_PrimaryKey() == 0 && (int)messageKey.get_ModifierKeys() == 0))
				{
					return GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();
				}
				KeyboardUtil.Release(160);
				KeyboardUtil.Release(161);
				int value;
				bool num = _modifierLookUp.TryGetValue(messageKey.get_ModifierKeys(), out value);
				if (num)
				{
					KeyboardUtil.Press(value);
				}
				if ((int)messageKey.get_PrimaryKey() != 0)
				{
					KeyboardUtil.Stroke((int)messageKey.get_PrimaryKey());
				}
				if (num)
				{
					KeyboardUtil.Release(value);
				}
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(250.0);
				while (DateTime.UtcNow < dateTime)
				{
					if (GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
					{
						return true;
					}
				}
				return GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();
			});
		}

		private static async Task<bool> Unfocus()
		{
			return await Task.Run(delegate
			{
				if (!GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
				{
					return true;
				}
				KeyboardUtil.Stroke(27);
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(250.0);
				while (DateTime.UtcNow < dateTime)
				{
					if (!GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
					{
						return true;
					}
				}
				return !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();
			});
		}

		private static async Task<bool> SetUnicodeBytesAsync(byte[] clipboardContent, Logger logger)
		{
			if (clipboardContent == null)
			{
				return true;
			}
			try
			{
				return await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(clipboardContent);
			}
			catch (Exception e)
			{
				logger.Debug(e, e.Message);
			}
			return false;
		}

		private static async Task<bool> SetTextAsync(string text, Logger logger, int retries = 5)
		{
			try
			{
				do
				{
					if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text))
					{
						return true;
					}
					retries--;
					await Task.Delay(250);
				}
				while (retries > 0);
			}
			catch (Exception e)
			{
				if (retries > 0)
				{
					return await SetTextAsync(text, logger, retries - 1);
				}
				logger.Debug(e, e.Message);
			}
			return false;
		}

		private static bool IsTextValid(string text, Logger logger)
		{
			if (string.IsNullOrEmpty(text))
			{
				logger.Info("Invalid chat message. Argument 'text' was null or empty.");
				return false;
			}
			if (text.Length > 199)
			{
				logger.Info(string.Format("Invalid chat message. Argument '{0}' exceeds limit of {1} characters. Value: \"{2}[..+{3}]\"", "text", 199, text.Substring(0, 25), 174));
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
