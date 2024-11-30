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

		public static async Task<bool> Send(string text, KeyBinding messageKey, Logger logger = null)
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
				return false;
			}
			bool flag = !IsTextValid(text, logger);
			if (!flag)
			{
				flag = !(await Focus(messageKey));
			}
			if (flag)
			{
				return false;
			}
			try
			{
				bool result;
				if (!KeyboardUtil.Paste() || !KeyboardUtil.Stroke(13))
				{
					logger.Info("Failed to send text to chat: " + text);
					await Unfocus();
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
			finally
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
			}
		}

		public static async Task<bool> SendWhisper(string recipient, string cmdAndMessage, KeyBinding messageKey, Logger logger = null)
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
				return false;
			}
			bool flag = !IsTextValid(cmdAndMessage, logger);
			if (!flag)
			{
				flag = !(await Focus(messageKey));
			}
			if (flag)
			{
				return false;
			}
			try
			{
				bool result;
				if (!KeyboardUtil.Paste())
				{
					logger.Info("Failed to paste whisper message: " + cmdAndMessage);
					await Unfocus();
					result = false;
				}
				else if (!(await SetTextAsync(recipient.Trim(), logger)))
				{
					await Unfocus();
					result = false;
				}
				else if (!KeyboardUtil.Paste())
				{
					logger.Info("Failed to paste whisper recipient: " + recipient);
					await Unfocus();
					result = false;
				}
				else
				{
					await Task.Delay(1);
					bool success2 = KeyboardUtil.Stroke(9);
					await Task.Delay(1);
					success2 = success2 && KeyboardUtil.Stroke(13);
					await Task.Delay(50);
					await Unfocus();
					result = success2;
				}
				return result;
			}
			finally
			{
				await SetUnicodeBytesAsync(prevClipboardContent, logger);
			}
		}

		public static async Task<bool> Insert(string text, KeyBinding messageKey, Logger logger = null)
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
				return false;
			}
			bool flag = !IsTextValid(text, logger);
			if (!flag)
			{
				flag = !(await Focus(messageKey));
			}
			if (flag)
			{
				return false;
			}
			bool success = KeyboardUtil.Paste();
			await SetUnicodeBytesAsync(prevClipboardContent, logger);
			return success;
		}

		private static async Task<bool> Focus(KeyBinding messageKey)
		{
			return await Task.Run(delegate
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				if (GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
				{
					return true;
				}
				if (messageKey == null || ((int)messageKey.get_PrimaryKey() == 0 && (int)messageKey.get_ModifierKeys() == 0))
				{
					return GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();
				}
				List<Func<bool>> list = new List<Func<bool>>
				{
					() => KeyboardUtil.Release(160),
					() => KeyboardUtil.Release(161)
				};
				int modifierKey;
				bool num = _modifierLookUp.TryGetValue(messageKey.get_ModifierKeys(), out modifierKey);
				if (num)
				{
					list.Add(() => KeyboardUtil.Press(modifierKey));
				}
				if ((int)messageKey.get_PrimaryKey() != 0)
				{
					list.Add(() => KeyboardUtil.Stroke((int)messageKey.get_PrimaryKey()));
				}
				if (num)
				{
					list.Add(() => KeyboardUtil.Release(modifierKey));
				}
				foreach (Func<bool> item in list)
				{
					if (!item())
					{
						return false;
					}
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
				if (!KeyboardUtil.Stroke(27))
				{
					return false;
				}
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
