using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SongbookOfTyria.Serialization
{
	public class StringOrArrayConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(List<string>);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			JToken token = JToken.Load(reader);
			if ((int)token.get_Type() == 2)
			{
				return token.ToObject<List<string>>();
			}
			if ((int)token.get_Type() == 8)
			{
				return new List<string> { ((object)token).ToString() };
			}
			token.get_Type();
			_ = 10;
			return new List<string>();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value);
		}

		public StringOrArrayConverter()
			: this()
		{
		}
	}
}
