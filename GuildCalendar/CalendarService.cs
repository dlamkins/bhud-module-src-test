using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GuildCalendar
{
	public class CalendarService
	{
		private static readonly HttpClient _http = new HttpClient();

		private readonly TimeZoneInfo _etZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

		public async Task<List<GuildEvent>> FetchEvents(string icalUrl)
		{
			if (string.IsNullOrWhiteSpace(icalUrl))
			{
				return new List<GuildEvent>();
			}
			try
			{
				_http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
				string iCalData = await _http.GetStringAsync(icalUrl);
				if (iCalData.Contains("<!DOCTYPE html>") || iCalData.Contains("<html"))
				{
					throw new Exception("Link is a Login Page. Check permissions.");
				}
				string[] array = iCalData.Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
				List<string> unfoldedLines = new List<string>();
				StringBuilder sb = new StringBuilder();
				string[] array2 = array;
				foreach (string rawLine in array2)
				{
					if (rawLine.StartsWith(" ") || rawLine.StartsWith("\t"))
					{
						sb.Append(rawLine.Substring(1));
						continue;
					}
					if (sb.Length > 0)
					{
						unfoldedLines.Add(sb.ToString());
						sb.Clear();
					}
					sb.Append(rawLine);
				}
				if (sb.Length > 0)
				{
					unfoldedLines.Add(sb.ToString());
				}
				List<GuildEvent> events = new List<GuildEvent>();
				string currentSummary = null;
				string currentDesc = null;
				DateTime? currentDate = null;
				bool inEvent = false;
				bool isRecurring = false;
				string repeatFreq = "";
				int repeatInterval = 1;
				foreach (string item in unfoldedLines)
				{
					string k = item.Trim();
					if (k == "BEGIN:VEVENT")
					{
						inEvent = true;
						currentSummary = null;
						currentDesc = null;
						currentDate = null;
						isRecurring = false;
						repeatFreq = "";
						repeatInterval = 1;
					}
					else
					{
						if (!inEvent)
						{
							continue;
						}
						if (k == "END:VEVENT")
						{
							if (currentSummary != null && currentDate.HasValue)
							{
								string cleanDesc = (currentDesc ?? "").Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t")
									.Replace("\\,", ",")
									.Replace("\\;", ";");
								events.Add(new GuildEvent
								{
									Title = currentSummary,
									Date = currentDate.Value,
									Description = cleanDesc
								});
								if (isRecurring && repeatFreq == "WEEKLY")
								{
									DateTime nextDate2 = currentDate.Value;
									for (int j = 0; j < 52; j++)
									{
										nextDate2 = nextDate2.AddDays(7 * repeatInterval);
										events.Add(new GuildEvent
										{
											Title = currentSummary,
											Date = nextDate2,
											Description = cleanDesc
										});
									}
								}
								else if (isRecurring && repeatFreq == "DAILY")
								{
									DateTime nextDate = currentDate.Value;
									for (int i = 0; i < 60; i++)
									{
										nextDate = nextDate.AddDays(repeatInterval);
										events.Add(new GuildEvent
										{
											Title = currentSummary,
											Date = nextDate,
											Description = cleanDesc
										});
									}
								}
							}
							inEvent = false;
							continue;
						}
						if (k.StartsWith("SUMMARY:"))
						{
							currentSummary = k.Substring(k.IndexOf(':') + 1).Trim();
						}
						if (k.StartsWith("DESCRIPTION:"))
						{
							currentDesc = k.Substring(k.IndexOf(':') + 1);
						}
						if (k.StartsWith("DTSTART"))
						{
							try
							{
								string datePart = k.Substring(k.IndexOf(':') + 1).Trim();
								if (datePart.Length == 8 && !datePart.Contains("T"))
								{
									int year2 = int.Parse(datePart.Substring(0, 4));
									int month2 = int.Parse(datePart.Substring(4, 2));
									int day2 = int.Parse(datePart.Substring(6, 2));
									currentDate = new DateTime(year2, month2, day2, 0, 0, 0, DateTimeKind.Unspecified);
								}
								else if (datePart.Contains("T"))
								{
									bool num = datePart.EndsWith("Z");
									string cleanDate = datePart.Replace("Z", "");
									int year = int.Parse(cleanDate.Substring(0, 4));
									int month = int.Parse(cleanDate.Substring(4, 2));
									int day = int.Parse(cleanDate.Substring(6, 2));
									int hour = int.Parse(cleanDate.Substring(9, 2));
									int min = int.Parse(cleanDate.Substring(11, 2));
									int sec = ((cleanDate.Length > 13) ? int.Parse(cleanDate.Substring(13, 2)) : 0);
									if (num)
									{
										DateTime etTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(year, month, day, hour, min, sec, DateTimeKind.Utc), _etZone);
										currentDate = new DateTime(etTime.Year, etTime.Month, etTime.Day, etTime.Hour, etTime.Minute, etTime.Second, DateTimeKind.Unspecified);
									}
									else
									{
										currentDate = new DateTime(year, month, day, hour, min, sec, DateTimeKind.Unspecified);
									}
								}
							}
							catch
							{
							}
						}
						if (k.StartsWith("RRULE:"))
						{
							isRecurring = true;
							if (k.Contains("FREQ=WEEKLY"))
							{
								repeatFreq = "WEEKLY";
							}
							if (k.Contains("FREQ=DAILY"))
							{
								repeatFreq = "DAILY";
							}
							Match intervalMatch = Regex.Match(k, "INTERVAL=([0-9]+)");
							if (intervalMatch.Success)
							{
								int.TryParse(intervalMatch.Groups[1].Value, out repeatInterval);
							}
						}
					}
				}
				DateTime minDate = DateTime.Now.AddYears(-2);
				DateTime maxDate = DateTime.Now.AddYears(2);
				return (from e in events
					where e.Date > minDate && e.Date < maxDate
					orderby e.Date
					select e).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to fetch/parse iCal: " + ex.Message);
			}
		}
	}
}
