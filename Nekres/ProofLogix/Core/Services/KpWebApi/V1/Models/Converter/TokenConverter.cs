using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter
{
	public class TokenConverter : JsonConverter<Token>
	{
		public override bool CanWrite => false;

		public override bool CanRead => true;

		public override Token ReadJson(JsonReader reader, Type objectType, Token existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)reader.get_TokenType() == 11)
			{
				return null;
			}
			JProperty prop = JObject.Load(reader).Properties().FirstOrDefault();
			if (prop == null)
			{
				return null;
			}
			string displayName = prop.get_Name();
			if (string.IsNullOrEmpty(displayName))
			{
				return null;
			}
			int quantity = Convert.ToInt32(Extensions.Value<int>((IEnumerable<JToken>)prop.get_Value()));
			return new Token
			{
				Name = displayName,
				Quantity = quantity
			};
		}

		public override void WriteJson(JsonWriter writer, Token value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
