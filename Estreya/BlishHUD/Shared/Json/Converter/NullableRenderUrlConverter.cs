using System;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Json.Converter
{
	public class NullableRenderUrlConverter : JsonConverter<RenderUrl?>
	{
		private readonly IConnection _connection;

		public NullableRenderUrlConverter(IConnection connection)
		{
			_connection = connection;
		}

		public override RenderUrl? ReadJson(JsonReader reader, Type objectType, RenderUrl? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			if (reader.TokenType != JsonToken.String)
			{
				throw new JsonException("Expected a string value");
			}
			return Gw2SharpHelper.CreateRenderUrl(_connection, reader.Value as string);
		}

		public override void WriteJson(JsonWriter writer, RenderUrl? value, JsonSerializer serializer)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			object value2;
			if (!value.HasValue)
			{
				value2 = null;
			}
			else
			{
				RenderUrl valueOrDefault = value.GetValueOrDefault();
				value2 = ((RenderUrl)(ref valueOrDefault)).get_Url()?.AbsoluteUri;
			}
			writer.WriteValue((string?)value2);
		}
	}
}
