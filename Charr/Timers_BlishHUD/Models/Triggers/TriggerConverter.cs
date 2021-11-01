using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Charr.Timers_BlishHUD.Models.Triggers
{
	public class TriggerConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public override bool CanRead => true;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Trigger);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new InvalidOperationException("Use default serialization.");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jsonObject = JObject.Load(reader);
			Trigger trigger = null;
			string type = ((JToken)jsonObject).Value<string>((object)"type") ?? "location";
			if (!(type == "key"))
			{
				if (!(type == "location"))
				{
				}
				trigger = new LocationTrigger();
			}
			else
			{
				trigger = new KeyTrigger();
			}
			serializer.Populate(((JToken)jsonObject).CreateReader(), (object)trigger);
			return trigger;
		}

		public TriggerConverter()
			: this()
		{
		}
	}
}
