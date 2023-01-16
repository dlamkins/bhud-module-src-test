using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Neo.IronLua;

namespace BhModule.Community.Pathing.Scripting.Lib.Std
{
	public class LuaLibraryOS
	{
		private delegate string DateTimeDelegate(DateTime dateTime);

		private static readonly DateTime _unixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

		private static readonly Dictionary<string, DateTimeDelegate> Formats = new Dictionary<string, DateTimeDelegate>
		{
			{
				"a",
				(DateTime dateTime) => dateTime.ToString("ddd", CultureInfo.CurrentCulture)
			},
			{
				"A",
				(DateTime dateTime) => dateTime.ToString("dddd", CultureInfo.CurrentCulture)
			},
			{
				"b",
				(DateTime dateTime) => dateTime.ToString("MMM", CultureInfo.CurrentCulture)
			},
			{
				"B",
				(DateTime dateTime) => dateTime.ToString("MMMM", CultureInfo.CurrentCulture)
			},
			{
				"c",
				(DateTime dateTime) => dateTime.ToString("ddd MMM dd HH:mm:ss yyyy", CultureInfo.CurrentCulture)
			},
			{
				"d",
				(DateTime dateTime) => dateTime.ToString("dd", CultureInfo.CurrentCulture)
			},
			{
				"e",
				(DateTime dateTime) => dateTime.ToString("%d", CultureInfo.CurrentCulture).PadLeft(2, ' ')
			},
			{
				"H",
				(DateTime dateTime) => dateTime.ToString("HH", CultureInfo.CurrentCulture)
			},
			{
				"I",
				(DateTime dateTime) => dateTime.ToString("hh", CultureInfo.CurrentCulture)
			},
			{
				"j",
				(DateTime dateTime) => dateTime.DayOfYear.ToString().PadLeft(3, '0')
			},
			{
				"m",
				(DateTime dateTime) => dateTime.ToString("MM", CultureInfo.CurrentCulture)
			},
			{
				"M",
				(DateTime dateTime) => dateTime.Minute.ToString().PadLeft(2, '0')
			},
			{
				"p",
				(DateTime dateTime) => dateTime.ToString("tt", new CultureInfo("en-US"))
			},
			{
				"S",
				(DateTime dateTime) => dateTime.ToString("ss", CultureInfo.CurrentCulture)
			},
			{
				"U",
				(DateTime dateTime) => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Sunday).ToString().PadLeft(2, '0')
			},
			{
				"W",
				(DateTime dateTime) => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday).ToString().PadLeft(2, '0')
			},
			{
				"w",
				(DateTime dateTime) => ((int)dateTime.DayOfWeek).ToString()
			},
			{
				"x",
				(DateTime dateTime) => dateTime.ToString("d", CultureInfo.CurrentCulture)
			},
			{
				"X",
				(DateTime dateTime) => dateTime.ToString("T", CultureInfo.CurrentCulture)
			},
			{
				"y",
				(DateTime dateTime) => dateTime.ToString("yy", CultureInfo.CurrentCulture)
			},
			{
				"Y",
				(DateTime dateTime) => dateTime.ToString("yyyy", CultureInfo.CurrentCulture)
			},
			{
				"Z",
				(DateTime dateTime) => dateTime.ToString("zzz", CultureInfo.CurrentCulture)
			},
			{
				"%",
				(DateTime dateTime) => "%"
			}
		};

		public static LuaResult clock()
		{
			return new LuaResult(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds);
		}

		public static object date(string format, object time)
		{
			bool toUtc = format != null && format.Length > 0 && format[0] == '!';
			DateTime dt;
			if (time == null)
			{
				dt = (toUtc ? DateTime.UtcNow : DateTime.Now);
			}
			else if (time is DateTime)
			{
				DateTime dt2 = (DateTime)time;
				dt = dt2;
				if (dt.Kind == DateTimeKind.Utc)
				{
					if (!toUtc)
					{
						dt = dt.ToLocalTime();
					}
				}
				else if (toUtc)
				{
					dt = dt.ToUniversalTime();
				}
			}
			else
			{
				dt = _unixStartTime.AddSeconds((long)Lua.RtConvertValue(time, typeof(long)));
				if (toUtc)
				{
					dt = dt.ToUniversalTime();
				}
			}
			if (toUtc)
			{
				format = format.Substring(1);
			}
			if (string.Compare(format, "*t", ignoreCase: false) == 0)
			{
				return new LuaTable
				{
					["year"] = dt.Year,
					["month"] = dt.Month,
					["day"] = dt.Day,
					["hour"] = dt.Hour,
					["min"] = dt.Minute,
					["sec"] = dt.Second,
					["wday"] = (int)dt.DayOfWeek,
					["yday"] = dt.DayOfYear,
					["isdst"] = ((dt.Kind == DateTimeKind.Local) ? true : false)
				};
			}
			return ToStrFTime(dt, format);
		}

		public static LuaResult time(LuaTable table)
		{
			TimeSpan ts;
			if (table == null)
			{
				ts = DateTime.Now.Subtract(_unixStartTime);
			}
			else
			{
				try
				{
					ts = datetime(table).Subtract(_unixStartTime);
				}
				catch (Exception e)
				{
					return new LuaResult(null, e.Message);
				}
			}
			return new LuaResult(Convert.ToInt64(ts.TotalSeconds));
		}

		public static DateTime datetime(object time)
		{
			LuaTable table = time as LuaTable;
			if ((object)table == null)
			{
				if (time is int)
				{
					int i32 = (int)time;
					return _unixStartTime.AddSeconds(i32);
				}
				if (time is long)
				{
					long i33 = (long)time;
					return _unixStartTime.AddSeconds(i33);
				}
				if (time is double)
				{
					double d = (double)time;
					return _unixStartTime.AddSeconds(d);
				}
				throw new ArgumentException();
			}
			return new DateTime((!table.ContainsKey("year")) ? 1970 : (((int)table["year"] < 1970) ? 1970 : ((int)table["year"])), (!table.ContainsKey("month")) ? 1 : ((int)table["month"]), (!table.ContainsKey("day")) ? 1 : ((int)table["day"]), table.ContainsKey("hour") ? ((int)table["hour"]) : 0, table.ContainsKey("min") ? ((int)table["min"]) : 0, table.ContainsKey("sec") ? ((int)table["sec"]) : 0, (!table.ContainsKey("isdst")) ? DateTimeKind.Local : ((!table.ContainsKey("isdst")) ? DateTimeKind.Utc : DateTimeKind.Local));
		}

		public static long difftime(object t2, object t1)
		{
			long num = Convert.ToInt64((t2 is LuaTable) ? time((LuaTable)t2)[0] : t2);
			long time1 = Convert.ToInt64((t1 is LuaTable) ? time((LuaTable)t1)[0] : t1);
			return num - time1;
		}

		private static string ToStrFTime(DateTime dateTime, string pattern)
		{
			string output = "";
			int i = 0;
			if (string.IsNullOrEmpty(pattern))
			{
				return dateTime.ToString();
			}
			for (; i < pattern.Length; i++)
			{
				string s = pattern.Substring(i, 1);
				output = ((i + 1 < pattern.Length) ? (output + ((!(s == "%")) ? s : (Formats.ContainsKey(pattern.Substring(++i, 1)) ? Formats[pattern.Substring(i, 1)](dateTime) : ("%" + pattern.Substring(i, 1))))) : (output + s));
			}
			return output;
		}
	}
}
