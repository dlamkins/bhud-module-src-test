using System;
using System.Collections.Generic;
using System.Linq;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TitleConverter : JsonConverter<Title>
{
	public override bool CanWrite => false;

	public override bool CanRead => true;

	public override Title ReadJson(JsonReader reader, Type objectType, Title existingValue, bool hasExistingValue, JsonSerializer serializer)
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
		string originStr = Extensions.Value<string>((IEnumerable<JToken>)prop.get_Value());
		if (string.IsNullOrEmpty(originStr))
		{
			return null;
		}
		Title.TitleMode result;
		return new Title
		{
			Name = displayName,
			Mode = (Enum.TryParse<Title.TitleMode>(originStr, ignoreCase: true, out result) ? result : Title.TitleMode.Unknown)
		};
	}

	public override void WriteJson(JsonWriter writer, Title value, JsonSerializer serializer)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		val.Add(value.Name, JToken.op_Implicit(value.Mode.ToString().ToLower()));
		((JToken)val).WriteTo(writer, Array.Empty<JsonConverter>());
	}
}
