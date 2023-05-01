using System;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Json.Converter
{
	public class RenderUrlConverter : JsonConverter<RenderUrl>
	{
		private readonly IConnection _connection;

		public RenderUrlConverter(IConnection connection)
		{
			_connection = connection;
		}

		public override RenderUrl ReadJson(JsonReader reader, Type objectType, RenderUrl existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (reader.TokenType == JsonToken.Null)
			{
				return default(RenderUrl);
			}
			if (reader.TokenType != JsonToken.String)
			{
				throw new JsonException("Expected a string value");
			}
			return Gw2SharpHelper.CreateRenderUrl(_connection, reader.Value as string);
		}

		public override void WriteJson(JsonWriter writer, RenderUrl value, JsonSerializer serializer)
		{
			writer.WriteValue(((RenderUrl)(ref value)).get_Url()?.AbsoluteUri);
		}
	}
}
