using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter
{
	public class ClearConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Clear);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken firstPath = ((JToken)JObject.Load(reader)).get_First();
			if (firstPath == null)
			{
				return null;
			}
			string displayName = firstPath.get_Path().Trim('[', '\'', ']');
			IJEnumerable<JToken> bosses = Extensions.Values((IEnumerable<JToken>)firstPath);
			return new Clear
			{
				Name = displayName,
				Encounters = ((IEnumerable<JToken>)bosses).Select((JToken boss) => serializer.Deserialize<Boss>(boss.CreateReader())).ToList()
			};
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public ClearConverter()
			: this()
		{
		}
	}
}
