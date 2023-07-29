using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter
{
	public class BossConverter : JsonConverter
	{
		public override bool CanRead => true;

		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Boss);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)reader.get_TokenType() == 11)
			{
				return null;
			}
			JProperty property = JObject.Load(reader).Properties().FirstOrDefault();
			if (property == null)
			{
				return null;
			}
			return new Boss
			{
				Name = property.get_Name(),
				Cleared = Convert.ToBoolean(Extensions.Value<int>((IEnumerable<JToken>)property.get_Value()))
			};
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public BossConverter()
			: this()
		{
		}
	}
}
