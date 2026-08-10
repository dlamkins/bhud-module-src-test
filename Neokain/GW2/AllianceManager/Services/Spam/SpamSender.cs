using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Spams;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.AllianceManager.Services.Spam
{
	public class SpamSender : ISpamSender
	{
		private class PreparedSpamLine
		{
			public SpamLineDto Line { get; set; }

			public string InterpolatedText { get; set; }
		}

		private static KeyBinding _defaultChatKeyBinding;

		private static KeyBinding DefaultChatKeyBinding
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				object obj = _defaultChatKeyBinding;
				if (obj == null)
				{
					KeyBinding val = new KeyBinding((Keys)13);
					obj = (object)val;
					_defaultChatKeyBinding = val;
				}
				return (KeyBinding)obj;
			}
		}

		Task<bool> ISpamSender.SendAsync(SpamDetailDto spam, SpamContext context, int messageDelay, IGw2WebClient webClient)
		{
			return SendAsync(spam, context, messageDelay, webClient);
		}

		public static List<GuildMembershipDto> ResolveAllianceTargets(IReadOnlyCollection<Guid> allianceGuildIds, IEnumerable<GuildMembershipDto> available)
		{
			if (allianceGuildIds == null || allianceGuildIds.Count == 0 || available == null)
			{
				return new List<GuildMembershipDto>();
			}
			return available.Where((GuildMembershipDto g) => allianceGuildIds.Contains(g.GuildId)).ToList();
		}

		public static bool HasSendableLines(SpamDetailDto spam, SpamContext context)
		{
			if (spam?.SpamLines == null || spam.SpamLines.Count == 0)
			{
				return false;
			}
			foreach (SpamLineDto line in spam.SpamLines)
			{
				if (ChatTypeValidationService.IsAllowedInContext(line.Target, context.ContextType) && (!ChatTypeValidationService.RequiresExtraInfo(line.Target) || ChatTypeValidationService.ValidateTargetInfo1(line.Target, line.TargetInfo1, context).Status != ValidationStatus.Error))
				{
					return true;
				}
			}
			return false;
		}

		public static int GetMessageDelay(Module module)
		{
			return (int)(module?.SpamMessageDelay?.get_Value()).GetValueOrDefault(MessageDelayPreset.Normal);
		}

		public static async Task<bool> SendAsync(SpamDetailDto spam, SpamContext context, int messageDelay, IGw2WebClient webClient)
		{
			if (spam?.SpamLines == null || spam.SpamLines.Count == 0)
			{
				ScreenNotification.ShowNotification("This spam has no lines to send", (NotificationType)1, (Texture2D)null, 4);
				return false;
			}
			DateTime referenceTime = DateTime.Now;
			List<PreparedSpamLine> preparedLines = new List<PreparedSpamLine>();
			List<string> validationErrors = new List<string>();
			foreach (SpamLineDto line in spam.SpamLines.OrderBy((SpamLineDto l) => l.Order))
			{
				string interpolatedText = TextInterpolationService.Interpolate(line.LineText, referenceTime);
				if (!ChatTypeValidationService.IsAllowedInContext(line.Target, context.ContextType))
				{
					validationErrors.Add($"ChatType {line.Target} not allowed in {context.ContextType} context");
					continue;
				}
				if (ChatTypeValidationService.RequiresExtraInfo(line.Target))
				{
					ValidationResult validation = ChatTypeValidationService.ValidateTargetInfo1(line.Target, line.TargetInfo1, context);
					if (validation.Status == ValidationStatus.Error)
					{
						validationErrors.Add("Line validation failed: " + validation.Message);
						continue;
					}
				}
				preparedLines.Add(new PreparedSpamLine
				{
					Line = line,
					InterpolatedText = interpolatedText
				});
			}
			if (validationErrors.Count > 0)
			{
				ScreenNotification.ShowNotification((validationErrors.Count == 1) ? validationErrors[0] : $"{validationErrors.Count} validation errors", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			bool anySent = false;
			foreach (PreparedSpamLine prepared in preparedLines)
			{
				anySent |= await SendSingleLineAsync(prepared.Line, prepared.InterpolatedText, context, messageDelay, webClient);
			}
			if (!anySent)
			{
				ScreenNotification.ShowNotification("Nothing was sent — the spam has no valid targets", (NotificationType)1, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		private static async Task<bool> SendSingleLineAsync(SpamLineDto line, string interpolatedText, SpamContext context, int messageDelay, IGw2WebClient webClient)
		{
			await ChatUtil.Clear(DefaultChatKeyBinding);
			await Task.Delay(messageDelay);
			switch (line.Target)
			{
			case ChatType.Alliance:
				return await SendAllianceLineAsync(interpolatedText, line.TargetInfo1, context, messageDelay, webClient);
			case ChatType.Guild:
				return await SendGuildLineAsync(interpolatedText, line.TargetInfo1, context, messageDelay);
			case ChatType.Party:
				await SendChatLineAsync("/p", interpolatedText, messageDelay);
				return true;
			case ChatType.Squad:
			case ChatType.Raid:
				await SendChatLineAsync("/d", interpolatedText, messageDelay);
				return true;
			case ChatType.Map:
				await SendChatLineAsync("/m", interpolatedText, messageDelay);
				return true;
			case ChatType.Team:
				await SendChatLineAsync("/t", interpolatedText, messageDelay);
				return true;
			case ChatType.Whisper:
				return await SendWhisperLineAsync(interpolatedText, line.TargetInfo1, messageDelay);
			case ChatType.Channel:
				return await SendChannelLineAsync(interpolatedText, line.TargetInfo1, messageDelay);
			default:
				return false;
			}
		}

		private static async Task<bool> SendAllianceLineAsync(string text, string targetInfo1, SpamContext context, int messageDelay, IGw2WebClient webClient)
		{
			Guid allianceGuid;
			List<GuildMembershipDto> targets = ((!Guid.TryParse(targetInfo1, out allianceGuid) || webClient == null) ? (context?.AvailableGuilds ?? new List<GuildMembershipDto>()) : ResolveAllianceTargets((await webClient.GetAllianceDetail(allianceGuid))?.GuildIds, context.AvailableGuilds));
			if (targets == null || targets.Count == 0)
			{
				return false;
			}
			foreach (GuildMembershipDto guild in targets)
			{
				await SendChatLineAsync($"/g{guild.GuildIndex + 1}", text, messageDelay);
			}
			return true;
		}

		private static async Task<bool> SendGuildLineAsync(string text, string targetInfo1, SpamContext context, int messageDelay)
		{
			if (Guid.TryParse(targetInfo1, out var guildId))
			{
				GuildMembershipDto guild2 = context?.AvailableGuilds?.FirstOrDefault((GuildMembershipDto g) => g.GuildId == guildId);
				if (guild2 == null)
				{
					return false;
				}
				await SendChatLineAsync($"/g{guild2.GuildIndex + 1}", text, messageDelay);
				return true;
			}
			List<GuildMembershipDto> targets = context?.AvailableGuilds;
			if (targets == null || targets.Count == 0)
			{
				return false;
			}
			foreach (GuildMembershipDto guild in targets)
			{
				await SendChatLineAsync($"/g{guild.GuildIndex + 1}", text, messageDelay);
			}
			return true;
		}

		private static async Task<bool> SendWhisperLineAsync(string text, string playerName, int messageDelay)
		{
			if (string.IsNullOrWhiteSpace(playerName))
			{
				return false;
			}
			await SendChatLineAsync("/w " + playerName, text, messageDelay);
			return true;
		}

		private static async Task<bool> SendChannelLineAsync(string text, string channelCommand, int messageDelay)
		{
			if (string.IsNullOrWhiteSpace(channelCommand))
			{
				return false;
			}
			await SendChatLineAsync(channelCommand, text, messageDelay);
			return true;
		}

		private static async Task SendChatLineAsync(string chatCommand, string text, int messageDelay)
		{
			string combinedMessage = chatCommand + " " + text;
			if (combinedMessage.Length < 200)
			{
				await ChatUtil.Send(combinedMessage, DefaultChatKeyBinding);
			}
			else
			{
				int maxTextLength = 199;
				string sizedText = ((text.Length <= maxTextLength) ? text : text.Substring(0, maxTextLength));
				await ChatUtil.Send(chatCommand + " ", DefaultChatKeyBinding);
				await Task.Delay(messageDelay);
				await ChatUtil.Send(sizedText, DefaultChatKeyBinding);
			}
			await Task.Delay(messageDelay);
		}
	}
}
