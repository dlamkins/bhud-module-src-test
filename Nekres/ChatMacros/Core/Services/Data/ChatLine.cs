using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Gw2Sharp.WebApi;
using LiteDB;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.Services.Data
{
	internal class ChatLine
	{
		private static Regex _whisperRecipientPattern = new Regex("^\\[(?<name>.*)\\]", RegexOptions.Compiled);

		private static Regex _squadBroadcastPattern = new Regex("^!", RegexOptions.Compiled);

		[BsonId(true)]
		public ObjectId Id { get; set; }

		[BsonField("channel")]
		public ChatChannel Channel { get; set; }

		[BsonField("whisper_to")]
		public string WhisperTo { get; set; }

		[BsonField("squad_broadcast")]
		public bool SquadBroadcast { get; set; }

		[BsonField("message")]
		public string Message { get; set; }

		public string ToChatMessage()
		{
			return (Channel.ToShortChatCommand() + " ").TrimStart() + Message;
		}

		public static ChatLine Parse(string input)
		{
			ChatLine line = new ChatLine
			{
				Id = new ObjectId()
			};
			if (input.IsNullOrEmpty())
			{
				return line;
			}
			ChatChannel channel = (line.Channel = ParseChannel(ref input));
			if (channel == ChatChannel.Current)
			{
				line.Message = input.TrimEnd();
				return line;
			}
			string message = input.TrimStart(1);
			switch (channel)
			{
			case ChatChannel.Whisper:
			{
				Match recipientMatch = _whisperRecipientPattern.Match(message);
				if (recipientMatch.Success)
				{
					line.WhisperTo = recipientMatch.Groups["name"].Value;
					message = message.Remove(0, recipientMatch.Index + recipientMatch.Length);
				}
				break;
			}
			case ChatChannel.Squad:
			{
				Match broadcastMatch = _squadBroadcastPattern.Match(message);
				if (broadcastMatch.Success)
				{
					line.SquadBroadcast = true;
					message = message.Remove(0, broadcastMatch.Index + broadcastMatch.Length);
				}
				break;
			}
			}
			line.Message = message.TrimEnd().TrimStart(1);
			return line;
		}

		private static ChatChannel ParseChannel(ref string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return ChatChannel.Current;
			}
			CultureInfo currentCulture = Resources.Culture;
			ChatChannel channel = ChatChannel.Current;
			foreach (CultureInfo item in from Locale l in Enum.GetValues(typeof(Locale))
				select l.GetCulture())
			{
				Resources.Culture = item;
				foreach (ChatChannel chatChannel in Enum.GetValues(typeof(ChatChannel)).Cast<ChatChannel>().Skip(1))
				{
					string shortcmd = chatChannel.ToShortChatCommand();
					string cmd = chatChannel.ToChatCommand();
					string command = input.Split(' ')[0];
					if (chatChannel == ChatChannel.Whisper)
					{
						command = command.Split('[')[0];
					}
					if (chatChannel == ChatChannel.Squad)
					{
						command = command.Split('!')[0];
					}
					if (command.Equals(shortcmd, StringComparison.InvariantCultureIgnoreCase))
					{
						channel = chatChannel;
						input = input.Replace(shortcmd, string.Empty, 1);
						Resources.Culture = currentCulture;
						return channel;
					}
					if (command.Equals(cmd, StringComparison.InvariantCultureIgnoreCase))
					{
						channel = chatChannel;
						input = input.Replace(cmd, string.Empty, 1);
						Resources.Culture = currentCulture;
						return channel;
					}
				}
			}
			Resources.Culture = currentCulture;
			return channel;
		}

		public string Serialize()
		{
			string param = string.Empty;
			if (Channel == ChatChannel.Whisper && !WhisperTo.IsNullOrEmpty())
			{
				param = "[" + WhisperTo + "]";
			}
			else if (Channel == ChatChannel.Squad && SquadBroadcast)
			{
				param = "!";
			}
			string channel = Channel.ToShortChatCommand();
			if (channel.IsNullOrEmpty())
			{
				return Message;
			}
			if (param.IsNullOrEmpty())
			{
				return channel + " " + Message;
			}
			return channel + " " + param + " " + Message;
		}
	}
}
