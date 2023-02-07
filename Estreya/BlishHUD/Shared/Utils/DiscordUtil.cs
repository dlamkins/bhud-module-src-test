using System.Text.RegularExpressions;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class DiscordUtil
	{
		private static readonly Regex _usernameRegEx = new Regex("^.{3,32}#[0-9]{4}$", RegexOptions.Compiled | RegexOptions.Singleline);

		public static bool IsValidUsername(string username)
		{
			return _usernameRegEx.IsMatch(username);
		}
	}
}
