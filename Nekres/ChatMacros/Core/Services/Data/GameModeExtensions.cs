using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.Services.Data
{
	internal static class GameModeExtensions
	{
		public static string ToShortDisplayString(this GameMode mode)
		{
			return mode switch
			{
				GameMode.PvE => Resources.PvE, 
				GameMode.WvW => Resources.WvW, 
				GameMode.PvP => Resources.PvP, 
				_ => string.Empty, 
			};
		}

		public static string ToDisplayString(this GameMode mode)
		{
			return mode switch
			{
				GameMode.PvE => Resources.Player_vs__Environment, 
				GameMode.WvW => Resources.World_vs__World, 
				GameMode.PvP => Resources.Player_vs__Player, 
				_ => string.Empty, 
			};
		}
	}
}
