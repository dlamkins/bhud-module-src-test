using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class TutorialInstructionsConverter : JsonConverter<List<TutorialInstructionStep>>
	{
		public override void WriteJson(JsonWriter writer, List<TutorialInstructionStep>? value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteStartArray();
			foreach (TutorialInstructionStep step in value!)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("type");
				writer.WriteValue(NormalizeType(step.Type));
				if (NeedsText(NormalizeType(step.Type)))
				{
					writer.WritePropertyName("text");
					writer.WriteValue(step.Text ?? "");
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}

		public override List<TutorialInstructionStep> ReadJson(JsonReader reader, Type objectType, List<TutorialInstructionStep>? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			List<TutorialInstructionStep> steps = new List<TutorialInstructionStep>();
			if (reader.TokenType == JsonToken.Null)
			{
				return steps;
			}
			foreach (JToken token in JArray.Load(reader))
			{
				switch (token.Type)
				{
				case JTokenType.String:
					steps.Add(new TutorialInstructionStep
					{
						Type = "text",
						Text = (token.Value<string>() ?? "")
					});
					break;
				case JTokenType.Object:
				{
					JObject obj = (JObject)token;
					string type = NormalizeType(obj.Value<string>("type"));
					steps.Add(new TutorialInstructionStep
					{
						Type = type,
						Text = (NeedsText(type) ? (obj.Value<string>("text") ?? "") : "")
					});
					break;
				}
				}
			}
			return steps;
		}

		private static string NormalizeType(string? type)
		{
			if (string.IsNullOrWhiteSpace(type))
			{
				return "text";
			}
			return type!.Trim().ToLowerInvariant() switch
			{
				"hint" => "hint", 
				"waypoint" => "waypoint", 
				"location" => "location", 
				"scoring_rings" => "scoring_rings", 
				_ => "text", 
			};
		}

		private static bool NeedsText(string type)
		{
			if (type == "text" || type == "hint")
			{
				return true;
			}
			return false;
		}
	}
}
