using System;
using Blish_HUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ideka.CustomCombatText
{
	public class AreaModelTypeConverter : JsonConverter<IAreaModelType>
	{
		private static readonly Logger Logger = Logger.GetLogger<AreaModelTypeConverter>();

		public override bool CanRead => true;

		public override bool CanWrite => false;

		private static IAreaModelType? Convert(JToken t, JsonSerializer s, AreaType type)
		{
			return type switch
			{
				AreaType.Container => t.ToObject<ModelTypeContainer>(s), 
				AreaType.Scroll => t.ToObject<ModelTypeScroll>(s), 
				AreaType.Top => t.ToObject<ModelTypeTop>(s), 
				_ => null, 
			};
		}

		public override IAreaModelType? ReadJson(JsonReader reader, Type objectType, IAreaModelType? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JToken token = JToken.ReadFrom(reader);
			AreaType? areaType = token["Type"]?.ToObject<AreaType>(serializer);
			if (areaType.HasValue)
			{
				AreaType type = areaType.GetValueOrDefault();
				IAreaModelType area = Convert(token, serializer, type);
				if (area == null)
				{
					Logger.Warn(string.Format("Failed to deserialize {0} as type: {1}.", "IAreaModelType", type));
					return null;
				}
				return area;
			}
			Logger.Warn("Failed to determine IAreaModelType type.");
			return null;
		}

		public override void WriteJson(JsonWriter writer, IAreaModelType? value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
