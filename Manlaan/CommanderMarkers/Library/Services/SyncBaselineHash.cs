using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Library.Services
{
	public static class SyncBaselineHash
	{
		public static string Compute(MarkerSet markerSet)
		{
			List<JObject> markers = markerSet.marks.OrderBy((MarkerCoord m) => m.icon).Select(delegate(MarkerCoord m)
			{
				JObject jObject = new JObject
				{
					["i"] = (JToken)m.icon,
					["x"] = (JToken)m.x,
					["y"] = (JToken)m.y,
					["z"] = (JToken)m.z
				};
				if (!string.IsNullOrWhiteSpace(m.name))
				{
					jObject["d"] = (JToken)m.name;
				}
				return jObject;
			}).ToList();
			string json = new JObject
			{
				["name"] = (JToken)(markerSet.name ?? ""),
				["description"] = (JToken)(markerSet.description ?? ""),
				["mapId"] = (JToken)markerSet.mapId.GetValueOrDefault(),
				["trigger"] = JObject.FromObject(markerSet.Trigger),
				["markers"] = new JArray(markers)
			}.ToString(Formatting.None);
			using SHA256 sha = SHA256.Create();
			return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(json))).Replace("-", "").ToLowerInvariant();
		}
	}
}
