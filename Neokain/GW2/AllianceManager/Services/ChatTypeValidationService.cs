using System;
using System.Linq;
using System.Text.RegularExpressions;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Services
{
	public static class ChatTypeValidationService
	{
		private static readonly Regex ChannelNameRegex = new Regex("^/[a-zA-Z0-9]+$", RegexOptions.Compiled);

		public static bool RequiresExtraInfo(ChatType type)
		{
			if ((uint)type <= 1u || (uint)(type - 7) <= 1u)
			{
				return true;
			}
			return false;
		}

		public static bool IsAllowedInContext(ChatType type, SpamContextType context)
		{
			if (type == ChatType.Alliance && context == SpamContextType.Guild)
			{
				return false;
			}
			return true;
		}

		public static string GetDisallowedReason(ChatType type, SpamContextType context)
		{
			if (type == ChatType.Alliance && context == SpamContextType.Guild)
			{
				return "Alliance chat is not available for guild spams";
			}
			return null;
		}

		public static ValidationResult ValidateTargetInfo1(ChatType type, string targetInfo1, SpamContext context)
		{
			return type switch
			{
				ChatType.Guild => ValidateGuildTarget(targetInfo1, context), 
				ChatType.Alliance => ValidateAllianceTarget(targetInfo1, context), 
				ChatType.Whisper => ValidateWhisperTarget(targetInfo1), 
				ChatType.Channel => ValidateChannelTarget(targetInfo1), 
				_ => new ValidationResult
				{
					Status = ValidationStatus.Ok,
					Message = "OK"
				}, 
			};
		}

		public static string GetChatCommand(ChatType type, string targetInfo1, SpamContext context)
		{
			switch (type)
			{
			case ChatType.Alliance:
				return null;
			case ChatType.Guild:
			{
				if (!Guid.TryParse(targetInfo1, out var guildId))
				{
					return null;
				}
				GuildMembershipDto guild = context.AvailableGuilds?.FirstOrDefault((GuildMembershipDto g) => g.GuildId == guildId);
				if (guild == null)
				{
					return null;
				}
				return $"/g{guild.GuildIndex + 1}";
			}
			case ChatType.Squad:
			case ChatType.Party:
			case ChatType.Raid:
				return "/d";
			case ChatType.Map:
				return "/m";
			case ChatType.Team:
				return "/t";
			case ChatType.Whisper:
				if (string.IsNullOrWhiteSpace(targetInfo1))
				{
					return null;
				}
				return "/w " + targetInfo1;
			case ChatType.Channel:
				if (string.IsNullOrWhiteSpace(targetInfo1))
				{
					return null;
				}
				return targetInfo1;
			default:
				return null;
			}
		}

		public static int GetChatCommandPrefixLength(ChatType type, string targetInfo1, SpamContext context)
		{
			string cmd = GetChatCommand(type, targetInfo1, context);
			if (cmd == null)
			{
				if (type == ChatType.Alliance)
				{
					return 4;
				}
				return 0;
			}
			return cmd.Length + 1;
		}

		private static ValidationResult ValidateGuildTarget(string targetInfo1, SpamContext context)
		{
			if (string.IsNullOrWhiteSpace(targetInfo1))
			{
				if (context != null && context.ContextType == SpamContextType.Guild)
				{
					return new ValidationResult
					{
						Status = ValidationStatus.Ok,
						Message = "OK"
					};
				}
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Guild must be selected"
				};
			}
			if (!Guid.TryParse(targetInfo1, out var guildId))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Invalid guild ID"
				};
			}
			if (context?.AvailableGuilds == null || !context.AvailableGuilds.Any())
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "No guilds available"
				};
			}
			GuildMembershipDto guild = context.AvailableGuilds.FirstOrDefault((GuildMembershipDto g) => g.GuildId == guildId);
			if (guild == null)
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Guild not found in available guilds"
				};
			}
			return new ValidationResult
			{
				Status = ValidationStatus.Ok,
				Message = "[" + guild.GuildTag + "] " + guild.GuildName
			};
		}

		private static ValidationResult ValidateAllianceTarget(string targetInfo1, SpamContext context)
		{
			if (context.ContextType != SpamContextType.Account)
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Ok,
					Message = "OK"
				};
			}
			if (string.IsNullOrWhiteSpace(targetInfo1))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Alliance must be selected"
				};
			}
			if (!Guid.TryParse(targetInfo1, out var allianceId))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Invalid alliance ID"
				};
			}
			if (context?.AvailableAlliances == null || !context.AvailableAlliances.Any())
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "No alliances available"
				};
			}
			AllianceMembershipDto alliance = context.AvailableAlliances.FirstOrDefault((AllianceMembershipDto a) => a.AllianceId == allianceId);
			if (alliance == null)
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Alliance not found in available alliances"
				};
			}
			return new ValidationResult
			{
				Status = ValidationStatus.Ok,
				Message = "[" + alliance.AllianceTag + "] " + alliance.AllianceName
			};
		}

		private static ValidationResult ValidateWhisperTarget(string targetInfo1)
		{
			if (string.IsNullOrWhiteSpace(targetInfo1))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Player name required"
				};
			}
			if (targetInfo1.Length > 50)
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Player name too long"
				};
			}
			return new ValidationResult
			{
				Status = ValidationStatus.Ok,
				Message = "Whisper to: " + targetInfo1
			};
		}

		private static ValidationResult ValidateChannelTarget(string targetInfo1)
		{
			if (string.IsNullOrWhiteSpace(targetInfo1))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Channel name required (e.g., /mychannel)"
				};
			}
			if (!targetInfo1.StartsWith("/"))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Channel must start with /"
				};
			}
			if (!ChannelNameRegex.IsMatch(targetInfo1))
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = "Channel: only letters and numbers allowed"
				};
			}
			return new ValidationResult
			{
				Status = ValidationStatus.Ok,
				Message = "Channel: " + targetInfo1
			};
		}
	}
}
