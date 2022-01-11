using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Json
{
	public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
	{
		private IEnumerable<string> Formats { get; set; } = new string[2] { "dd\\.hh\\:mm", "hh\\:mm" };


		private string ToStringFormat { get; set; }

		public TimeSpanJsonConverter()
		{
		}

		public TimeSpanJsonConverter(string toStringFormat, IEnumerable<string> parseFormats)
			: this()
		{
			ToStringFormat = toStringFormat;
			Formats = parseFormats;
		}

		public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
		{
			if (string.IsNullOrWhiteSpace(ToStringFormat))
			{
				throw new ArgumentNullException("ToStringFormat", "Format has not been specified.");
			}
			writer.WriteValue(value.ToString(ToStringFormat));
		}

		public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (objectType != typeof(TimeSpan))
			{
				return TimeSpan.Zero;
			}
			string value = (string)reader.get_Value();
			foreach (string format in Formats)
			{
				if (TimeSpan.TryParseExact(value, format, CultureInfo.InvariantCulture, out var result))
				{
					return result;
				}
			}
			return TimeSpan.Zero;
		}
	}
}
