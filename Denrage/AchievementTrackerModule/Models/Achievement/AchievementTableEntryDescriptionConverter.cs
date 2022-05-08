using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	public class AchievementTableEntryDescriptionConverter : JsonConverter<AchievementTableEntryDescription>
	{
		private enum TypeDiscriminator
		{
			String,
			Objective,
			Collection
		}

		private const string TypeValuePropertyName = "TypeValue";

		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(AchievementTableEntryDescription).IsAssignableFrom(typeToConvert);
		}

		public override AchievementTableEntryDescription Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Invalid comparison between Unknown and I4
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((Utf8JsonReader)(ref reader)).get_TokenType() != 1)
			{
				throw new JsonException();
			}
			if (!((Utf8JsonReader)(ref reader)).Read() || (int)((Utf8JsonReader)(ref reader)).get_TokenType() != 5 || ((Utf8JsonReader)(ref reader)).GetString() != "TypeDiscriminator")
			{
				throw new JsonException();
			}
			if (!((Utf8JsonReader)(ref reader)).Read() || (int)((Utf8JsonReader)(ref reader)).get_TokenType() != 8)
			{
				throw new JsonException();
			}
			AchievementTableEntryDescription description = ((Utf8JsonReader)(ref reader)).GetInt32() switch
			{
				0 => ParseDescription<StringDescription>(ref reader), 
				1 => ParseDescription<ObjectivesDescription>(ref reader), 
				2 => ParseDescription<CollectionDescription>(ref reader), 
				_ => throw new NotSupportedException(), 
			};
			if (!((Utf8JsonReader)(ref reader)).Read() || (int)((Utf8JsonReader)(ref reader)).get_TokenType() != 2)
			{
				throw new JsonException();
			}
			return description;
			static AchievementTableEntryDescription ParseDescription<T>(ref Utf8JsonReader jsonReader) where T : AchievementTableEntryDescription
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Invalid comparison between Unknown and I4
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				if (!((Utf8JsonReader)(ref jsonReader)).Read() || ((Utf8JsonReader)(ref jsonReader)).GetString() != "TypeValue")
				{
					throw new JsonException();
				}
				if (!((Utf8JsonReader)(ref jsonReader)).Read() || (int)((Utf8JsonReader)(ref jsonReader)).get_TokenType() != 1)
				{
					throw new JsonException();
				}
				return ((T)JsonSerializer.Deserialize(ref jsonReader, typeof(T), (JsonSerializerOptions)null)) ?? throw new JsonException();
			}
		}

		public override void Write(Utf8JsonWriter writer, AchievementTableEntryDescription value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			StringDescription stringDescription = value as StringDescription;
			if (stringDescription == null)
			{
				ObjectivesDescription objectivesDescription = value as ObjectivesDescription;
				if (objectivesDescription == null)
				{
					CollectionDescription collectionDescription = value as CollectionDescription;
					if (collectionDescription == null)
					{
						throw new NotSupportedException();
					}
					WriteTypeDiscriminator<CollectionDescription>(writer, collectionDescription, TypeDiscriminator.Collection);
				}
				else
				{
					WriteTypeDiscriminator<ObjectivesDescription>(writer, objectivesDescription, TypeDiscriminator.Objective);
				}
			}
			else
			{
				WriteTypeDiscriminator<StringDescription>(writer, stringDescription, TypeDiscriminator.String);
			}
			writer.WriteEndObject();
			static void WriteTypeDiscriminator<T>(Utf8JsonWriter jsonWriter, T description, TypeDiscriminator typeDiscriminator) where T : AchievementTableEntryDescription
			{
				jsonWriter.WriteNumber("TypeDiscriminator", (int)typeDiscriminator);
				jsonWriter.WritePropertyName("TypeValue");
				JsonSerializer.Serialize<T>(jsonWriter, description, (JsonSerializerOptions)null);
			}
		}
	}
}
