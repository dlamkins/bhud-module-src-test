using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class MarkerSetSubmission
	{
		public static JObject ToSubmissionPayload(MarkerSet markerSet, string suggestedCategory)
		{
			JArray markers = new JArray();
			foreach (MarkerCoord mark in markerSet.marks)
			{
				JObject row = new JObject
				{
					["i"] = (JToken)mark.icon,
					["x"] = (JToken)mark.x,
					["y"] = (JToken)mark.y,
					["z"] = (JToken)mark.z
				};
				if (!string.IsNullOrWhiteSpace(mark.name))
				{
					row["d"] = (JToken)mark.name;
				}
				markers.Add(row);
			}
			return new JObject
			{
				["name"] = (JToken)(markerSet.name ?? ""),
				["description"] = (JToken)(markerSet.description ?? ""),
				["mapId"] = (JToken)markerSet.mapId.GetValueOrDefault(),
				["enabled"] = (JToken)markerSet.enabled,
				["trigger"] = ((markerSet.trigger != null) ? JObject.FromObject(markerSet.trigger) : new JObject
				{
					["x"] = (JToken)0,
					["y"] = (JToken)0,
					["z"] = (JToken)0
				}),
				["markers"] = markers,
				["suggestedCategory"] = (JToken)suggestedCategory
			};
		}
	}
}
