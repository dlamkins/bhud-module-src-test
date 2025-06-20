using System;
using Blish_HUD;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public class LocationConverter : JsonConverter<Location>
	{
		public override void WriteJson(JsonWriter writer, Location value, JsonSerializer serializer)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			JArray jArray = new JArray();
			jArray.Add(value.MapId);
			jArray.Add(value.MapCoord.ToJArray());
			jArray.Add(value.AvatarPosition.ToJArray());
			jArray.Add(value.AvatarDirection.ToJArray());
			jArray.Add(value.CameraPosition.ToJArray());
			jArray.Add(value.CameraDirection.ToJArray());
			jArray.WriteTo(writer);
		}

		public override Location ReadJson(JsonReader reader, Type objectType, Location existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JArray array = JArray.Load(reader);
				if (array.Count < 6)
				{
					Logger.GetLogger<Module>().Warn($"Location data has insufficient elements: {array.Count}, expected 6. Data: {array}");
					return new Location();
				}
				if (!array[0].Type.HasFlag(JTokenType.Integer) && !array[0].Type.HasFlag(JTokenType.Float))
				{
					Logger.GetLogger<Module>().Warn($"Location MapId is not a number: {array[0]}. Data: {array}");
					return new Location();
				}
				return new Location
				{
					MapId = array[0].Value<int>(),
					MapCoord = array[1].ToCoords2(),
					AvatarPosition = array[2].ToVector3(),
					AvatarDirection = array[3].ToVector3(),
					CameraPosition = array[4].ToVector3(),
					CameraDirection = array[5].ToVector3()
				};
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Error(ex, "Failed to parse Location data");
				return new Location();
			}
		}
	}
}
