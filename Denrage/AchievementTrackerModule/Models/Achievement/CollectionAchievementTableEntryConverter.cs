using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	public class CollectionAchievementTableEntryConverter : JsonConverter<CollectionAchievementTable.CollectionAchievementTableEntry>
	{
		private enum TypeDiscriminator
		{
			Number,
			Coin,
			Item,
			Link,
			Map,
			String,
			Empty
		}

		private const string TypeValuePropertyName = "TypeValue";

		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(CollectionAchievementTable.CollectionAchievementTableEntry).IsAssignableFrom(typeToConvert);
		}

		public override CollectionAchievementTable.CollectionAchievementTableEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Invalid comparison between Unknown and I4
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
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
			CollectionAchievementTable.CollectionAchievementTableEntry entry = ((Utf8JsonReader)(ref reader)).GetInt32() switch
			{
				0 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableNumberEntry>(ref reader), 
				1 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableCoinEntry>(ref reader), 
				2 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableItemEntry>(ref reader), 
				3 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableLinkEntry>(ref reader), 
				4 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableMapEntry>(ref reader), 
				5 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableStringEntry>(ref reader), 
				6 => ParseEntry<CollectionAchievementTable.CollectionAchievementTableEmptyEntry>(ref reader), 
				_ => null, 
			};
			if (((Utf8JsonReader)(ref reader)).Read() && (int)((Utf8JsonReader)(ref reader)).get_TokenType() == 2)
			{
				return entry;
			}
			throw new JsonException();
			static CollectionAchievementTable.CollectionAchievementTableEntry ParseEntry<T>(ref Utf8JsonReader jsonReader) where T : CollectionAchievementTable.CollectionAchievementTableEntry
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Invalid comparison between Unknown and I4
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				if (!((Utf8JsonReader)(ref jsonReader)).Read() || ((Utf8JsonReader)(ref jsonReader)).GetString() != "TypeValue")
				{
					throw new JsonException();
				}
				if (!((Utf8JsonReader)(ref jsonReader)).Read() || (int)((Utf8JsonReader)(ref jsonReader)).get_TokenType() != 1)
				{
					throw new JsonException();
				}
				T result = (T)JsonSerializer.Deserialize(ref jsonReader, typeof(T), (JsonSerializerOptions)null);
				if (result != null)
				{
					return result;
				}
				throw new JsonException();
			}
		}

		public override void Write(Utf8JsonWriter writer, CollectionAchievementTable.CollectionAchievementTableEntry value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			CollectionAchievementTable.CollectionAchievementTableNumberEntry numberEntry = value as CollectionAchievementTable.CollectionAchievementTableNumberEntry;
			if (numberEntry == null)
			{
				CollectionAchievementTable.CollectionAchievementTableCoinEntry coinEntry = value as CollectionAchievementTable.CollectionAchievementTableCoinEntry;
				if (coinEntry == null)
				{
					CollectionAchievementTable.CollectionAchievementTableItemEntry itemEntry = value as CollectionAchievementTable.CollectionAchievementTableItemEntry;
					if (itemEntry == null)
					{
						CollectionAchievementTable.CollectionAchievementTableLinkEntry linkEntry = value as CollectionAchievementTable.CollectionAchievementTableLinkEntry;
						if (linkEntry == null)
						{
							CollectionAchievementTable.CollectionAchievementTableMapEntry mapEntry = value as CollectionAchievementTable.CollectionAchievementTableMapEntry;
							if (mapEntry == null)
							{
								CollectionAchievementTable.CollectionAchievementTableStringEntry stringEntry = value as CollectionAchievementTable.CollectionAchievementTableStringEntry;
								if (stringEntry == null)
								{
									CollectionAchievementTable.CollectionAchievementTableEmptyEntry empyEntry = value as CollectionAchievementTable.CollectionAchievementTableEmptyEntry;
									if (empyEntry != null)
									{
										WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableEmptyEntry>(writer, empyEntry, TypeDiscriminator.Empty);
									}
								}
								else
								{
									WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableStringEntry>(writer, stringEntry, TypeDiscriminator.String);
								}
							}
							else
							{
								WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableMapEntry>(writer, mapEntry, TypeDiscriminator.Map);
							}
						}
						else
						{
							WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableLinkEntry>(writer, linkEntry, TypeDiscriminator.Link);
						}
					}
					else
					{
						WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableItemEntry>(writer, itemEntry, TypeDiscriminator.Item);
					}
				}
				else
				{
					WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableCoinEntry>(writer, coinEntry, TypeDiscriminator.Coin);
				}
			}
			else
			{
				WriteTypeDiscriminator<CollectionAchievementTable.CollectionAchievementTableNumberEntry>(writer, numberEntry, TypeDiscriminator.Number);
			}
			writer.WriteEndObject();
			static void WriteTypeDiscriminator<T>(Utf8JsonWriter jsonWriter, T reward, TypeDiscriminator typeDiscriminator) where T : CollectionAchievementTable.CollectionAchievementTableEntry
			{
				jsonWriter.WriteNumber("TypeDiscriminator", (int)typeDiscriminator);
				jsonWriter.WritePropertyName("TypeValue");
				JsonSerializer.Serialize<T>(jsonWriter, reward, (JsonSerializerOptions)null);
			}
		}
	}
}
