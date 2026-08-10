using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Utils
{
	public static class SpamUtil
	{
		private static readonly KeyBinding DefaultChatKeyBinding = new KeyBinding((Keys)13);

		private static int GetMessageDelay(Module module)
		{
			return (int)(module?.SpamMessageDelay?.get_Value()).GetValueOrDefault(MessageDelayPreset.Normal);
		}

		public static async Task<bool> SpamToAllianceAsync(Guid allianceId, string message, Gw2WebClient webClient, Module module)
		{
			if (webClient == null || allianceId == Guid.Empty)
			{
				return false;
			}
			try
			{
				List<GuildMembershipDto> memberships = await webClient.GetAccountGuildMemberships((await webClient.GetMyAccount()).Id);
				HashSet<Guid> allianceGuildIds = new HashSet<Guid>((await webClient.GetAllianceDetail(allianceId))?.GuildIds ?? new List<Guid>());
				List<GuildMembershipDto> allianceGuilds = memberships?.Where((GuildMembershipDto m) => allianceGuildIds.Contains(m.GuildId)).ToList() ?? new List<GuildMembershipDto>();
				if (allianceGuilds.Count == 0)
				{
					return false;
				}
				string interpolated = TextInterpolationService.Interpolate(message, DateTime.Now);
				int messageDelay = GetMessageDelay(module);
				foreach (GuildMembershipDto guild in allianceGuilds)
				{
					await SendChatLineAsync($"/g{guild.GuildIndex + 1}", interpolated, messageDelay);
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static async Task<bool> SpamToGuildAsync(Guid guildId, string message, Gw2WebClient webClient, Module module)
		{
			if (webClient == null || guildId == Guid.Empty)
			{
				return false;
			}
			try
			{
				GuildMembershipDto guild = (await webClient.GetAccountGuildMemberships((await webClient.GetMyAccount()).Id))?.FirstOrDefault((GuildMembershipDto m) => m.GuildId == guildId);
				if (guild == null)
				{
					return false;
				}
				string interpolated = TextInterpolationService.Interpolate(message, DateTime.Now);
				int messageDelay = GetMessageDelay(module);
				await SendChatLineAsync($"/g{guild.GuildIndex + 1}", interpolated, messageDelay);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
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
