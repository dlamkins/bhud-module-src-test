using System;
using System.Text;
using System.Text.RegularExpressions;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Services.Spam
{
	public static class SpamCooldown
	{
		public static bool CanUseNow(SpamDetailDto spam, DateTime utcNow)
		{
			if (!spam.LastSpammed.HasValue)
			{
				return true;
			}
			TimeSpan? cooldown = ParseCooldown(spam.CooldownFormatted);
			if (!cooldown.HasValue)
			{
				return true;
			}
			return utcNow - spam.LastSpammed.Value.UtcDateTime >= cooldown.Value;
		}

		public static string DescribeRemaining(SpamDetailDto spam, DateTime utcNow)
		{
			if (!spam.LastSpammed.HasValue)
			{
				return string.Empty;
			}
			TimeSpan? cooldown = ParseCooldown(spam.CooldownFormatted);
			if (!cooldown.HasValue)
			{
				return string.Empty;
			}
			TimeSpan elapsed = utcNow - spam.LastSpammed.Value.UtcDateTime;
			TimeSpan remaining = cooldown.Value - elapsed;
			if (remaining <= TimeSpan.Zero)
			{
				return string.Empty;
			}
			return FormatDuration(remaining) + " remaining";
		}

		public static TimeSpan? ParseCooldown(string cooldownFormatted)
		{
			if (string.IsNullOrEmpty(cooldownFormatted))
			{
				return null;
			}
			Match match = Regex.Match(cooldownFormatted, "^(?:(\\d+)h)?(?:(\\d+)m)?(?:(\\d+)s)?$");
			if (!match.Success)
			{
				return null;
			}
			if (!match.Groups[1].Success && !match.Groups[2].Success && !match.Groups[3].Success)
			{
				return null;
			}
			int hours = (match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0);
			int minutes = (match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0);
			int seconds = (match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0);
			return new TimeSpan(hours, minutes, seconds);
		}

		private static string FormatDuration(TimeSpan duration)
		{
			StringBuilder sb = new StringBuilder();
			int num = (int)Math.Ceiling(duration.TotalSeconds);
			int h = num / 3600;
			int i = num % 3600 / 60;
			int s = num % 60;
			if (h > 0)
			{
				sb.Append(h).Append('h');
			}
			if (i > 0)
			{
				sb.Append(i).Append('m');
			}
			if (s > 0 || sb.Length == 0)
			{
				sb.Append(s).Append('s');
			}
			return sb.ToString();
		}
	}
}
