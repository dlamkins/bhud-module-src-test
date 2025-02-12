using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SL.ChatLinks.StaticFiles
{
	public sealed class SeedDatabaseJsonConverter : JsonConverter<SeedDatabase>
	{
		public override SeedDatabase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
			JsonElement root = jsonDocument.RootElement;
			if (root.ValueKind != JsonValueKind.Object)
			{
				return null;
			}
			JsonElement schemaVersionElement = default(JsonElement);
			JsonElement languageElement = default(JsonElement);
			JsonElement nameElement = default(JsonElement);
			JsonElement urlElement = default(JsonElement);
			JsonElement sha256Element = default(JsonElement);
			foreach (JsonProperty property in root.EnumerateObject())
			{
				if (property.NameEquals("schema_version"))
				{
					schemaVersionElement = property.Value;
				}
				else if (property.NameEquals("lang"))
				{
					languageElement = property.Value;
				}
				else if (property.NameEquals("name"))
				{
					nameElement = property.Value;
				}
				else if (property.NameEquals("url"))
				{
					urlElement = property.Value;
				}
				else if (property.NameEquals("sha256"))
				{
					sha256Element = property.Value;
				}
			}
			if (schemaVersionElement.ValueKind != JsonValueKind.Number)
			{
				return null;
			}
			if (!schemaVersionElement.TryGetInt32(out var schemaVersion))
			{
				return null;
			}
			if (languageElement.ValueKind != JsonValueKind.String)
			{
				return null;
			}
			string language = languageElement.GetString();
			if (string.IsNullOrWhiteSpace(language))
			{
				return null;
			}
			if (nameElement.ValueKind != JsonValueKind.String)
			{
				return null;
			}
			string name = nameElement.GetString();
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			if (urlElement.ValueKind != JsonValueKind.String)
			{
				return null;
			}
			string url = urlElement.GetString();
			if (string.IsNullOrWhiteSpace(url))
			{
				return null;
			}
			if (sha256Element.ValueKind != JsonValueKind.String)
			{
				return null;
			}
			string sha256 = sha256Element.GetString();
			if (string.IsNullOrWhiteSpace(sha256))
			{
				return null;
			}
			return new SeedDatabase
			{
				SchemaVersion = schemaVersion,
				Language = language,
				Name = name,
				Url = url,
				SHA256 = sha256
			};
		}

		public override void Write(Utf8JsonWriter writer, SeedDatabase value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("schema_version", value.SchemaVersion);
			writer.WriteString("lang", value.Language);
			writer.WriteString("name", value.Name);
			writer.WriteString("url", value.Url);
			writer.WriteString("sha256", value.SHA256);
			writer.WriteEndObject();
		}
	}
}
