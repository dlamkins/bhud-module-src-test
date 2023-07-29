using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter
{
	public class DictionaryConverter<T> : JsonConverter
	{
		public override bool CanWrite => false;

		public override bool CanRead => true;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Dictionary<string, T>);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if ((int)reader.get_TokenType() == 11)
			{
				return Enumerable.Empty<T>().ToList();
			}
			JObject obj = JObject.Load(reader);
			List<T> list = new List<T>();
			foreach (JProperty property in obj.Properties())
			{
				JObject val = new JObject();
				string name = property.get_Name();
				val.set_Item(name, property.get_Value());
				T value = ((JToken)val).ToObject<T>(serializer);
				list.Add(value);
			}
			return list;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public DictionaryConverter()
			: this()
		{
		}
	}
}
