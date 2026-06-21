using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Presets.Services
{
	public static class SyncBaselineHash
	{
		public static string Compute(MarkerSet markerSet)
		{
			string json = JsonConvert.SerializeObject(CanonicalPayloadJson(markerSet));
			using SHA256 sha = SHA256.Create();
			return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(json))).Replace("-", "").ToLowerInvariant();
		}

		private static JObject CanonicalPayloadJson(MarkerSet markerSet)
		{
			List<MarkerCoord> list = markerSet.marks.OrderBy((MarkerCoord m) => m.icon).ToList();
			JArray markers = new JArray();
			foreach (MarkerCoord marker in list)
			{
				JObject row = new JObject
				{
					["i"] = (JToken)marker.icon,
					["x"] = (JToken)marker.x,
					["y"] = (JToken)marker.y,
					["z"] = (JToken)marker.z
				};
				if (!string.IsNullOrEmpty(marker.name))
				{
					row["d"] = (JToken)marker.name;
				}
				markers.Add(row);
			}
			WorldCoord trigger = markerSet.trigger ?? new WorldCoord();
			return new JObject
			{
				["name"] = (JToken)markerSet.name,
				["description"] = (JToken)markerSet.description,
				["mapId"] = (JToken)markerSet.mapId,
				["trigger"] = new JObject
				{
					["x"] = (JToken)trigger.x,
					["y"] = (JToken)trigger.y,
					["z"] = (JToken)trigger.z
				},
				["markers"] = markers
			};
		}
	}
}
