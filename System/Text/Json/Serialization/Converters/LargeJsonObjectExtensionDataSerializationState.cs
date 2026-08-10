using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace System.Text.Json.Serialization.Converters
{
	internal sealed class LargeJsonObjectExtensionDataSerializationState
	{
		public const int LargeObjectThreshold = 25;

		private readonly Dictionary<string, JsonNode> _tempDictionary;

		public JsonObject Destination { get; }

		public LargeJsonObjectExtensionDataSerializationState(JsonObject destination)
		{
			StringComparer comparer = ((destination.Options?.PropertyNameCaseInsensitive ?? false) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
			Destination = destination;
			_tempDictionary = new Dictionary<string, JsonNode>(comparer);
		}

		public void AddProperty(string key, JsonNode value)
		{
			_tempDictionary[key] = value;
		}

		public void Complete()
		{
			foreach (KeyValuePair<string, JsonNode> item in _tempDictionary)
			{
				Destination[item.Key] = item.Value;
			}
		}
	}
}
