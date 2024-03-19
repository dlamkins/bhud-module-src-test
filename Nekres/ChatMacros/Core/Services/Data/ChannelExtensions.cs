using Microsoft.Xna.Framework;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.Services.Data
{
	public static class ChannelExtensions
	{
		public static string ToShortChatCommand(this ChatChannel channel)
		{
			return channel switch
			{
				ChatChannel.Current => string.Empty, 
				ChatChannel.Emote => Resources._e, 
				ChatChannel.Say => Resources._s, 
				ChatChannel.Map => Resources._m, 
				ChatChannel.Party => Resources._p, 
				ChatChannel.Squad => Resources._d, 
				ChatChannel.Team => Resources._t, 
				ChatChannel.Reply => Resources._r, 
				ChatChannel.Whisper => Resources._w, 
				ChatChannel.Guild => Resources._g, 
				ChatChannel.Guild1 => string.Format(Resources._g_0_, 1), 
				ChatChannel.Guild2 => string.Format(Resources._g_0_, 2), 
				ChatChannel.Guild3 => string.Format(Resources._g_0_, 3), 
				ChatChannel.Guild4 => string.Format(Resources._g_0_, 4), 
				ChatChannel.Guild5 => string.Format(Resources._g_0_, 5), 
				_ => string.Empty, 
			};
		}

		public static string ToChatCommand(this ChatChannel channel)
		{
			return channel switch
			{
				ChatChannel.Current => string.Empty, 
				ChatChannel.Emote => Resources._emote, 
				ChatChannel.Say => Resources._say, 
				ChatChannel.Map => Resources._map, 
				ChatChannel.Party => Resources._party, 
				ChatChannel.Squad => Resources._squad, 
				ChatChannel.Team => Resources._team, 
				ChatChannel.Reply => Resources._reply, 
				ChatChannel.Whisper => Resources._whisper, 
				ChatChannel.Guild => Resources._guild, 
				ChatChannel.Guild1 => string.Format(Resources._guild_0_, 1), 
				ChatChannel.Guild2 => string.Format(Resources._guild_0_, 2), 
				ChatChannel.Guild3 => string.Format(Resources._guild_0_, 3), 
				ChatChannel.Guild4 => string.Format(Resources._guild_0_, 4), 
				ChatChannel.Guild5 => string.Format(Resources._guild_0_, 5), 
				_ => string.Empty, 
			};
		}

		public static string ToDisplayName(this ChatChannel channel, bool brackets = true)
		{
			string name = channel switch
			{
				ChatChannel.Current => Resources.Whichever, 
				ChatChannel.Emote => Resources.Emote, 
				ChatChannel.Say => Resources.Say, 
				ChatChannel.Map => Resources.Map, 
				ChatChannel.Party => Resources.Party, 
				ChatChannel.Squad => Resources.Squad, 
				ChatChannel.Team => Resources.Team, 
				ChatChannel.Reply => Resources.Reply, 
				ChatChannel.Whisper => Resources.Whisper, 
				ChatChannel.Guild => Resources.Guild, 
				ChatChannel.Guild1 => string.Format(Resources.G_0_, 1), 
				ChatChannel.Guild2 => string.Format(Resources.G_0_, 2), 
				ChatChannel.Guild3 => string.Format(Resources.G_0_, 3), 
				ChatChannel.Guild4 => string.Format(Resources.G_0_, 4), 
				ChatChannel.Guild5 => string.Format(Resources.G_0_, 5), 
				_ => string.Empty, 
			};
			if (!string.IsNullOrEmpty(name) && brackets)
			{
				return "[" + name + "]";
			}
			return name;
		}

		public static Color GetHeadingColor(this ChatChannel channel)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(channel switch
			{
				ChatChannel.Current => Color.get_White(), 
				ChatChannel.Emote => new Color(136, 136, 136), 
				ChatChannel.Say => new Color(118, 217, 140), 
				ChatChannel.Map => new Color(170, 79, 68), 
				ChatChannel.Party => new Color(63, 155, 229), 
				ChatChannel.Squad => new Color(173, 218, 91), 
				ChatChannel.Team => new Color(221, 48, 49), 
				ChatChannel.Reply => new Color(177, 78, 169), 
				ChatChannel.Whisper => new Color(177, 78, 169), 
				ChatChannel.Guild => new Color(204, 150, 42), 
				ChatChannel.Guild1 => new Color(141, 129, 86), 
				ChatChannel.Guild2 => new Color(141, 129, 86), 
				ChatChannel.Guild3 => new Color(141, 129, 86), 
				ChatChannel.Guild4 => new Color(141, 129, 86), 
				ChatChannel.Guild5 => new Color(141, 129, 86), 
				_ => Color.get_White(), 
			}) * 1.25f;
		}

		public static Color GetMessageColor(this ChatChannel channel)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(channel switch
			{
				ChatChannel.Current => Color.get_White(), 
				ChatChannel.Emote => new Color(136, 136, 136), 
				ChatChannel.Say => new Color(220, 224, 233), 
				ChatChannel.Map => new Color(200, 168, 164), 
				ChatChannel.Party => new Color(147, 192, 223), 
				ChatChannel.Squad => new Color(191, 240, 230), 
				ChatChannel.Team => new Color(212, 212, 212), 
				ChatChannel.Reply => new Color(222, 135, 208), 
				ChatChannel.Whisper => new Color(222, 135, 208), 
				ChatChannel.Guild => new Color(208, 192, 138), 
				ChatChannel.Guild1 => new Color(227, 217, 164), 
				ChatChannel.Guild2 => new Color(227, 217, 164), 
				ChatChannel.Guild3 => new Color(227, 217, 164), 
				ChatChannel.Guild4 => new Color(227, 217, 164), 
				ChatChannel.Guild5 => new Color(227, 217, 164), 
				_ => Color.get_White(), 
			});
		}
	}
}
