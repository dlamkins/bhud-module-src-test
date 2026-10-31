using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SeaStove.Gw2Tldr.Data
{
	public sealed class MetaSchedule
	{
		public sealed class Occurrence
		{
			public bool Active { get; set; }

			public DateTimeOffset Start { get; set; }

			public DateTimeOffset End { get; set; }
		}

		public static Occurrence Next(JObject data, string name, DateTimeOffset now)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			object obj;
			if (data == null)
			{
				obj = null;
			}
			else
			{
				JToken obj2 = data.get_Item("metaTimerBindings");
				obj = ((obj2 != null) ? obj2.get_Item((object)name) : null);
			}
			JObject binding = (JObject)((obj is JObject) ? obj : null);
			string key = (string)((binding != null) ? binding.get_Item("rowKey") : null);
			object obj5;
			if (key != null)
			{
				object obj3;
				if (data == null)
				{
					obj3 = null;
				}
				else
				{
					JToken obj4 = data.get_Item("metaTimerSchedules");
					obj3 = ((obj4 != null) ? obj4.get_Item((object)key) : null);
				}
				obj5 = ((obj3 is JObject) ? obj3 : null);
			}
			else
			{
				obj5 = null;
			}
			JObject row = (JObject)obj5;
			if (row == null)
			{
				return null;
			}
			JToken obj6 = row.get_Item("sequences");
			JToken obj7 = ((obj6 != null) ? obj6.get_Item((object)"partial") : null);
			JArray prefix = (JArray)(((object)((obj7 is JArray) ? obj7 : null)) ?? ((object)new JArray()));
			JToken obj8 = row.get_Item("sequences");
			JToken obj9 = ((obj8 != null) ? obj8.get_Item((object)"pattern") : null);
			JArray pattern = (JArray)(((object)((obj9 is JArray) ? obj9 : null)) ?? ((object)new JArray()));
			JToken obj10 = binding.get_Item("include");
			JToken obj11 = ((obj10 is JArray) ? obj10 : null);
			List<int> included = ((obj11 != null) ? ((JContainer)obj11).Values<int>().ToList() : null);
			int prefixLength = ((IEnumerable<JToken>)prefix).Sum((JToken block) => ((int?)block.get_Item((object)"d")).GetValueOrDefault());
			int patternLength = ((IEnumerable<JToken>)pattern).Sum((JToken block) => ((int?)block.get_Item((object)"d")).GetValueOrDefault());
			if (prefixLength <= 0 && patternLength <= 0)
			{
				return null;
			}
			if (((IEnumerable<JToken>)prefix).Any((JToken block) => ((int?)block.get_Item((object)"d")).GetValueOrDefault() <= 0) || ((IEnumerable<JToken>)pattern).Any((JToken block) => ((int?)block.get_Item((object)"d")).GetValueOrDefault() <= 0))
			{
				return null;
			}
			now = now.ToUniversalTime();
			DateTimeOffset anchor = new DateTimeOffset(now.UtcDateTime.Date, TimeSpan.Zero).AddMinutes(((int?)row.get_Item("minuteOffset")).GetValueOrDefault());
			double elapsed = (now - anchor).TotalMinutes;
			bool inPrefix = patternLength > 0 && elapsed >= 0.0 && elapsed < (double)prefixLength;
			JArray blocks = (inPrefix ? prefix : ((patternLength > 0) ? pattern : prefix));
			int period = (inPrefix ? prefixLength : ((patternLength > 0) ? patternLength : prefixLength));
			double position = (inPrefix ? elapsed : (((elapsed - (double)((patternLength > 0) ? prefixLength : 0)) % (double)period + (double)period) % (double)period));
			int index = 0;
			double cursor = 0.0;
			while (index < ((JContainer)blocks).get_Count() && position >= cursor + (double)(int)blocks.get_Item(index).get_Item((object)"d"))
			{
				cursor += (double)(int)blocks.get_Item(index++).get_Item((object)"d");
			}
			if (index >= ((JContainer)blocks).get_Count())
			{
				return null;
			}
			DateTimeOffset start = now.AddMinutes(0.0 - (position - cursor));
			int maxBlocks = ((JContainer)prefix).get_Count() + ((JContainer)pattern).get_Count() * 2 + 2;
			for (int checkedBlocks = 0; checkedBlocks < maxBlocks; checkedBlocks++)
			{
				JToken block2 = blocks.get_Item(index);
				DateTimeOffset end = start.AddMinutes((int)block2.get_Item((object)"d"));
				int? segment = (int?)block2.get_Item((object)"r");
				if (included == null || (segment.HasValue && included.Contains(segment.Value)))
				{
					return new Occurrence
					{
						Start = start,
						End = end,
						Active = (start <= now && now < end)
					};
				}
				start = end;
				index++;
				if (index >= ((JContainer)blocks).get_Count())
				{
					if (inPrefix && ((JContainer)pattern).get_Count() > 0)
					{
						blocks = pattern;
						inPrefix = false;
					}
					index = 0;
				}
			}
			return null;
		}
	}
}
