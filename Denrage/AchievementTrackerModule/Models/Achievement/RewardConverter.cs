using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Denrage.AchievementTrackerModule.Models.Achievement
{
	public class RewardConverter : JsonConverter<Reward>
	{
		private enum TypeDiscriminator
		{
			EmptyReward,
			ItemReward,
			MultiTierReward
		}

		private const string TypeValuePropertyName = "TypeValue";

		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(Reward).IsAssignableFrom(typeToConvert);
		}

		public override Reward Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
			Reward reward = ((Utf8JsonReader)(ref reader)).GetInt32() switch
			{
				0 => ParseReward<EmptyReward>(ref reader), 
				2 => ParseReward<MultiTierReward>(ref reader), 
				1 => ParseReward<ItemReward>(ref reader), 
				_ => throw new NotSupportedException(), 
			};
			if (!((Utf8JsonReader)(ref reader)).Read() || (int)((Utf8JsonReader)(ref reader)).get_TokenType() != 2)
			{
				throw new JsonException();
			}
			return reward;
			static Reward ParseReward<T>(ref Utf8JsonReader jsonReader) where T : Reward
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

		public override void Write(Utf8JsonWriter writer, Reward value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			MultiTierReward multiTierReward = value as MultiTierReward;
			if (multiTierReward == null)
			{
				ItemReward itemReward = value as ItemReward;
				if (itemReward == null)
				{
					EmptyReward emptyReward = value as EmptyReward;
					if (emptyReward == null)
					{
						throw new NotSupportedException();
					}
					WriteTypeDiscriminator<EmptyReward>(writer, emptyReward, TypeDiscriminator.EmptyReward);
				}
				else
				{
					WriteTypeDiscriminator<ItemReward>(writer, itemReward, TypeDiscriminator.ItemReward);
				}
			}
			else
			{
				WriteTypeDiscriminator<MultiTierReward>(writer, multiTierReward, TypeDiscriminator.MultiTierReward);
			}
			writer.WriteEndObject();
			static void WriteTypeDiscriminator<T>(Utf8JsonWriter jsonWriter, T reward, TypeDiscriminator typeDiscriminator) where T : Reward
			{
				jsonWriter.WriteNumber("TypeDiscriminator", (int)typeDiscriminator);
				jsonWriter.WritePropertyName("TypeValue");
				JsonSerializer.Serialize<T>(jsonWriter, reward, (JsonSerializerOptions)null);
			}
		}
	}
}
