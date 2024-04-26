using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Chat;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Guild;
using Estreya.BlishHUD.Shared.Windows.API;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Services.GameIntegration
{
	public class ChatService : ManagedService
	{
		private const uint MAPVK_VK_TO_VSC = 0u;

		private const uint MAPVK_VSC_TO_VK = 1u;

		private const uint MAPVK_VK_TO_CHAR = 2u;

		private const uint MAPVK_VSC_TO_VK_EX = 3u;

		private const uint MAPVK_VK_TO_VSC_EX = 4u;

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, [In][MarshalAs(UnmanagedType.LPArray)] Input[] pInputs, int cbSize);

		public ChatService(ServiceConfiguration configuration)
			: base(configuration)
		{
		}

		public async Task ChangeChannel(ChatChannel channel, GuildNumber guildNumber = GuildNumber.Guild_1, string wispherRecipient = null)
		{
			if (await IsBusy())
			{
				throw new InvalidOperationException("The chat can't be used at the moment.");
			}
			await Paste(channel switch
			{
				ChatChannel.Say => "/s", 
				ChatChannel.Map => "/m", 
				ChatChannel.Party => "/p", 
				ChatChannel.Squad => "/d", 
				ChatChannel.Team => "/t", 
				ChatChannel.Private => "/w", 
				ChatChannel.RepresentedGuild => "/g", 
				ChatChannel.Guild1_5 => $"/g{(int)guildNumber}", 
				_ => throw new ArgumentException($"Invalid chat channel: {channel}"), 
			});
			await Task.Delay(100);
			await Type((VirtualKeyShort)32);
			await Task.Delay(100);
			if (channel == ChatChannel.Private)
			{
				if (string.IsNullOrWhiteSpace(wispherRecipient))
				{
					throw new ArgumentNullException("wispherRecipient", "wispher recipient can't be null or empty.");
				}
				await Paste(wispherRecipient);
				await Task.Delay(100);
				await Type((VirtualKeyShort)9);
				await Task.Delay(100);
			}
		}

		public async Task Type(VirtualKeyShort virtualKey)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (await IsBusy())
			{
				throw new InvalidOperationException("The chat can't be used at the moment.");
			}
			Keyboard.Stroke(virtualKey, true);
		}

		public async Task Send(string message)
		{
			if (await IsBusy())
			{
				throw new InvalidOperationException("The chat can't be used at the moment.");
			}
			await Paste(message);
			Keyboard.Stroke((VirtualKeyShort)13, false);
		}

		public async Task Paste(string text)
		{
			if (await IsBusy())
			{
				throw new InvalidOperationException("The chat can't be used at the moment.");
			}
			if (!(await IsTextValid(text)))
			{
				throw new ArgumentException("The text is invalid.", "text");
			}
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			try
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
				await Focus();
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)86, true);
				Thread.Sleep(50);
				Keyboard.Release((VirtualKeyShort)162, true);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to paste {text}", new object[1] { text });
				throw;
			}
			if (prevClipboardContent != null)
			{
				await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
			}
		}

		public async Task<string> GetInputText()
		{
			if (await IsBusy())
			{
				return string.Empty;
			}
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			await Focus();
			Keyboard.Press((VirtualKeyShort)162, true);
			Keyboard.Stroke((VirtualKeyShort)65, true);
			Keyboard.Stroke((VirtualKeyShort)67, true);
			Thread.Sleep(50);
			Keyboard.Release((VirtualKeyShort)162, true);
			await Unfocus();
			string text = await ClipboardUtil.get_WindowsClipboardService().GetTextAsync();
			if (prevClipboardContent != null)
			{
				await ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
			}
			return text;
		}

		public new async Task Clear()
		{
			if (!(await IsBusy()))
			{
				await Focus();
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)65, true);
				Thread.Sleep(50);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)8, false);
				await Unfocus();
			}
		}

		private async Task Focus()
		{
			if (!(await IsFocused()))
			{
				await Unfocus();
				Keyboard.Stroke((VirtualKeyShort)13, false);
			}
		}

		private async Task Unfocus()
		{
			if (await IsFocused())
			{
				Mouse.Click((MouseButton)0, GameService.Graphics.get_WindowWidth() / 2, 0, false);
			}
		}

		private Task<bool> IsTextValid(string text)
		{
			return Task.FromResult(text != null && text.Length < 200);
		}

		private Task<bool> IsFocused()
		{
			return Task.FromResult(GameService.Gw2Mumble.get_UI().get_IsTextInputFocused());
		}

		private Task<bool> IsBusy()
		{
			return Task.FromResult(!GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() || !GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() || !GameService.GameIntegration.get_Gw2Instance().get_IsInGame());
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override void InternalUnload()
		{
		}
	}
}
