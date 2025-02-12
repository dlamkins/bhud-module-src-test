using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SL.ChatLinks.Storage.Metadata
{
	public sealed class DataManifestJsonConverter : JsonConverter<DataManifest>
	{
		public override DataManifest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using JsonDocument json = JsonDocument.ParseValue(ref reader);
			if (json.RootElement.ValueKind != JsonValueKind.Object)
			{
				return null;
			}
			if (!json.RootElement.TryGetProperty("version", out var versionElement))
			{
				return null;
			}
			if (versionElement.ValueKind != JsonValueKind.Number)
			{
				return null;
			}
			int version = versionElement.GetInt32();
			if (version != 1)
			{
				return null;
			}
			if (!json.RootElement.TryGetProperty("databases", out var databaseVersionsElement))
			{
				return null;
			}
			if (databaseVersionsElement.ValueKind != JsonValueKind.Object)
			{
				return null;
			}
			Dictionary<string, Database> databases = new Dictionary<string, Database>();
			foreach (JsonProperty database in databaseVersionsElement.EnumerateObject())
			{
				if (database.Value.ValueKind == JsonValueKind.Object && database.Value.TryGetProperty("name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String && database.Value.TryGetProperty("schema_version", out var schemaVersionElement) && schemaVersionElement.ValueKind == JsonValueKind.Number && schemaVersionElement.TryGetInt32(out var schemaVersion))
				{
					string name = nameElement.GetString();
					if (!string.IsNullOrWhiteSpace(name))
					{
						databases.Add(database.Name, new Database
						{
							Name = name,
							SchemaVersion = schemaVersion
						});
					}
				}
			}
			return new DataManifest
			{
				Version = version,
				Databases = databases
			};
		}

		public override void Write(Utf8JsonWriter writer, DataManifest value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("version", value.Version);
			writer.WriteStartObject("databases");
			foreach (KeyValuePair<string, Database> pair in value.Databases)
			{
				writer.WriteStartObject(pair.Key);
				writer.WriteString("name", pair.Value.Name);
				writer.WriteNumber("schema_version", pair.Value.SchemaVersion);
				writer.WriteEndObject();
			}
			writer.WriteEndObject();
			writer.WriteEndObject();
		}
	}
}
