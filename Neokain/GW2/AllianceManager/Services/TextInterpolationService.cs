using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Neokain.GW2.AllianceManager.Services
{
	public static class TextInterpolationService
	{
		public const int MaxMessageLength = 199;

		private static readonly Regex PlaceholderRegex = new Regex("\\{(\\w+)([+-]\\d+)?\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Dictionary<string, (int min, int max)> PlaceholderLengths = new Dictionary<string, (int, int)>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"reset",
				(4, 9)
			},
			{
				"today",
				(6, 9)
			},
			{
				"tomorrow",
				(6, 9)
			},
			{
				"todayutc",
				(6, 9)
			},
			{
				"todaylocal",
				(6, 9)
			},
			{
				"tomorrowutc",
				(6, 9)
			},
			{
				"tomorrowlocal",
				(6, 9)
			}
		};

		private static readonly string[] WeekdayNames = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

		public static string Interpolate(string template)
		{
			return Interpolate(template, DateTime.Now);
		}

		public static string Interpolate(string template, DateTime referenceTime)
		{
			if (string.IsNullOrEmpty(template))
			{
				return template;
			}
			return PlaceholderRegex.Replace(template, delegate(Match match)
			{
				string value = match.Groups[1].Value;
				string offsetStr = (match.Groups[2].Success ? match.Groups[2].Value : null);
				return ReplacePlaceholder(value, offsetStr, referenceTime);
			});
		}

		public static (int min, int max) CalculateMinMaxLength(string template)
		{
			if (string.IsNullOrEmpty(template))
			{
				return (0, 0);
			}
			int minLength = 0;
			int maxLength = 0;
			int lastIndex = 0;
			foreach (Match match in PlaceholderRegex.Matches(template))
			{
				int literalLength = match.Index - lastIndex;
				minLength += literalLength;
				maxLength += literalLength;
				string name = match.Groups[1].Value.ToLowerInvariant();
				if (PlaceholderLengths.TryGetValue(name, out var lengths))
				{
					minLength += lengths.Item1;
					maxLength += lengths.Item2;
				}
				else
				{
					minLength += match.Length;
					maxLength += match.Length;
				}
				lastIndex = match.Index + match.Length;
			}
			int remainingLength = template.Length - lastIndex;
			minLength += remainingLength;
			maxLength += remainingLength;
			return (minLength, maxLength);
		}

		public static ValidationResult ValidateLength(string template)
		{
			var (min, max) = CalculateMinMaxLength(template);
			if (min > 199)
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Error,
					Message = $"Message too long: {min}-{max} chars (max {199})",
					MinLength = min,
					MaxLength = max
				};
			}
			if (max > 199)
			{
				return new ValidationResult
				{
					Status = ValidationStatus.Warning,
					Message = $"May exceed limit: {min}-{max} chars (max {199})",
					MinLength = min,
					MaxLength = max
				};
			}
			return new ValidationResult
			{
				Status = ValidationStatus.Ok,
				Message = $"Length: {min}-{max} / {199}",
				MinLength = min,
				MaxLength = max
			};
		}

		private static string ReplacePlaceholder(string name, string offsetStr, DateTime referenceTime)
		{
			int offset = 0;
			if (!string.IsNullOrEmpty(offsetStr))
			{
				int.TryParse(offsetStr, out offset);
			}
			DateTime referenceTimeUtc = ((referenceTime.Kind == DateTimeKind.Utc) ? referenceTime : referenceTime.ToUniversalTime());
			string text = name.ToLowerInvariant();
			if (text != null)
			{
				switch (text.Length)
				{
				case 5:
				{
					char c = text[0];
					if (c != 'r')
					{
						if (c != 't' || !(text == "today"))
						{
							break;
						}
						goto IL_011b;
					}
					if (!(text == "reset"))
					{
						break;
					}
					return FormatReset(offset, referenceTimeUtc);
				}
				case 8:
				{
					char c = text[2];
					if (c != 'd')
					{
						if (c != 'm' || !(text == "tomorrow"))
						{
							break;
						}
						goto IL_012b;
					}
					if (!(text == "todayutc"))
					{
						break;
					}
					goto IL_011b;
				}
				case 10:
					if (!(text == "todaylocal"))
					{
						break;
					}
					return GetWeekday(referenceTime, offset);
				case 11:
					if (!(text == "tomorrowutc"))
					{
						break;
					}
					goto IL_012b;
				case 13:
					{
						if (!(text == "tomorrowlocal"))
						{
							break;
						}
						return GetWeekday(referenceTime.AddDays(1.0), offset);
					}
					IL_011b:
					return GetWeekday(referenceTimeUtc, offset);
					IL_012b:
					return GetWeekday(referenceTimeUtc.AddDays(1.0), offset);
				}
			}
			if (!string.IsNullOrEmpty(offsetStr))
			{
				return "{" + name + offsetStr + "}";
			}
			return "{" + name + "}";
		}

		private static string FormatReset(int offsetHours, DateTime nowUtc)
		{
			TimeSpan timeUntilReset = nowUtc.Date.AddDays(1.0) - nowUtc + TimeSpan.FromHours(offsetHours);
			while (timeUntilReset.TotalHours < 0.0)
			{
				timeUntilReset = timeUntilReset.Add(TimeSpan.FromDays(1.0));
			}
			while (timeUntilReset.TotalHours >= 24.0)
			{
				timeUntilReset = timeUntilReset.Subtract(TimeSpan.FromDays(1.0));
			}
			int hours = (int)timeUntilReset.TotalHours;
			if (hours == 0)
			{
				return $"{timeUntilReset.Minutes}min";
			}
			return $"{hours}h {timeUntilReset.Minutes}min";
		}

		private static string GetWeekday(DateTime baseDate, int offsetDays)
		{
			DateTime targetDate = baseDate.AddDays(offsetDays);
			return WeekdayNames[(int)targetDate.DayOfWeek];
		}
	}
}
