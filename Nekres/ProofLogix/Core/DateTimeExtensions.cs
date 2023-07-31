using System;
using System.Globalization;

namespace Nekres.ProofLogix.Core
{
	public static class DateTimeExtensions
	{
		public static string AsTimeAgo(this DateTime dateTime)
		{
			TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);
			double totalSeconds = timeSpan.TotalSeconds;
			if (totalSeconds <= 4.0)
			{
				if (totalSeconds <= 2.0)
				{
					return "just now";
				}
				return "a few seconds ago";
			}
			if (totalSeconds <= 60.0)
			{
				return $"{timeSpan.Seconds} seconds ago";
			}
			double totalMinutes = timeSpan.TotalMinutes;
			string result;
			if (!(totalMinutes <= 2.0))
			{
				if (totalMinutes < 60.0)
				{
					result = $"about {timeSpan.Minutes} minutes ago";
				}
				else
				{
					double totalHours = timeSpan.TotalHours;
					string text;
					if (!(totalHours <= 2.0))
					{
						if (totalHours < 24.0)
						{
							text = $"about {timeSpan.Hours} hours ago";
						}
						else
						{
							double totalDays = timeSpan.TotalDays;
							string text2 = ((totalDays < 365.0) ? ((totalDays <= 30.0) ? ((!(totalDays <= 2.0)) ? $"about {timeSpan.Days} days ago" : "yesterday") : ((!(totalDays <= 60.0)) ? $"about {timeSpan.Days / 30} months ago" : "about a month ago")) : ((!(totalDays <= 730.0)) ? $"about {timeSpan.Days / 365} years ago" : "about a year ago"));
							text = text2;
						}
					}
					else
					{
						text = "about an hour ago";
					}
					result = text;
				}
			}
			else
			{
				result = "about a minute ago";
			}
			return result;
		}

		public static string AsRelativeTime(this DateTime dateTime)
		{
			DateTime now = ((dateTime.Kind == DateTimeKind.Utc) ? DateTime.UtcNow : DateTime.Now);
			string timePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;
			double totalDays = (now - dateTime).TotalDays;
			if (!(totalDays < 1.0))
			{
				if (totalDays < 2.0)
				{
					return ((dateTime.Date > now.Date) ? "Tomorrow" : "Yesterday") + " at " + dateTime.ToString(timePattern);
				}
				return dateTime.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + " " + timePattern);
			}
			return "Today at " + dateTime.ToString(timePattern);
		}
	}
}
