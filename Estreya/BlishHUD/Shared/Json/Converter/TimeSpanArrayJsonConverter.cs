using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Json.Converter
{
	public class TimeSpanArrayJsonConverter : JsonConverter<TimeSpan[]>
	{
		private IEnumerable<string> Formats { get; set; } = new string[2] { "dd\\.hh\\:mm", "hh\\:mm" };


		private string ToStringFormat { get; set; }

		private bool KeepExistingIfEmpty { get; }

		public TimeSpanArrayJsonConverter()
		{
		}

		public TimeSpanArrayJsonConverter(string toStringFormat, IEnumerable<string> parseFormats)
			: this()
		{
			ToStringFormat = toStringFormat;
			Formats = parseFormats;
		}

		public TimeSpanArrayJsonConverter(string toStringFormat, IEnumerable<string> parseFormats, bool keepExistingIfEmpty)
			: this(toStringFormat, parseFormats)
		{
			KeepExistingIfEmpty = keepExistingIfEmpty;
		}

		public override void WriteJson(JsonWriter writer, TimeSpan[] value, JsonSerializer serializer)
		{
			if (string.IsNullOrWhiteSpace(ToStringFormat))
			{
				throw new ArgumentNullException("ToStringFormat", "Format has not been specified.");
			}
			string[] stringValues = value.Select((TimeSpan v) => v.ToString(ToStringFormat)).ToArray();
			serializer.Serialize(writer, stringValues);
		}

		public override TimeSpan[] ReadJson(JsonReader reader, Type objectType, TimeSpan[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (objectType != typeof(TimeSpan[]))
			{
				return new TimeSpan[0];
			}
			List<TimeSpan> timespans = new List<TimeSpan>();
			if (reader.TokenType != JsonToken.Null && reader.TokenType == JsonToken.StartArray)
			{
				string[] tempValues = serializer.Deserialize<string[]>(reader);
				timespans.AddRange((from ts in tempValues.Select(delegate(string tv)
					{
						TimeSpan? result = null;
						foreach (string current in Formats)
						{
							if (TimeSpan.TryParseExact(tv, current, CultureInfo.InvariantCulture, out var result2))
							{
								return result2;
							}
						}
						return result;
					})
					where ts.HasValue
					select ts.Value).ToList());
			}
			if (timespans.Count == 0 && KeepExistingIfEmpty && hasExistingValue)
			{
				timespans.AddRange(existingValue);
			}
			return timespans.ToArray();
		}
	}
}
