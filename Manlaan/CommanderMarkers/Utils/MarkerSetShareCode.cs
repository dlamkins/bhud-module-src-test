using System;
using System.Text;
using Manlaan.CommanderMarkers.Presets.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manlaan.CommanderMarkers.Utils
{
	public static class MarkerSetShareCode
	{
		public static string Export(MarkerSet markerSet)
		{
			return Encode(ToPortablePayload(markerSet).ToString(Formatting.None));
		}

		public static MarkerSet? Import(string text, Func<string, string, MarkerSet?>? resolveCommunity = null)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			string json;
			try
			{
				json = Decode(text.Trim());
			}
			catch (Exception)
			{
				json = text.Trim();
			}
			try
			{
				JObject i = JObject.Parse(json);
				if (IsCommunityShareRef(i))
				{
					string communitySetId = i.Value<string>("communitySetId") ?? "";
					string name = i.Value<string>("name") ?? "";
					if (string.IsNullOrEmpty(communitySetId) || resolveCommunity == null)
					{
						return null;
					}
					return resolveCommunity!(communitySetId, name);
				}
				return i.ToObject<MarkerSet>();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static JObject ToPortablePayload(MarkerSet markerSet)
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
				["mapId"] = (JToken)markerSet.MapId,
				["enabled"] = (JToken)markerSet.enabled,
				["trigger"] = ((markerSet.trigger != null) ? JObject.FromObject(markerSet.trigger) : new JObject
				{
					["x"] = (JToken)0,
					["y"] = (JToken)0,
					["z"] = (JToken)0
				}),
				["markers"] = markers
			};
		}

		private static bool IsCommunityShareRef(JObject j)
		{
			if (string.Equals(j.Value<string>("shareType"), "community", StringComparison.Ordinal))
			{
				return true;
			}
			if (string.IsNullOrWhiteSpace(j.Value<string>("communitySetId")))
			{
				return false;
			}
			JToken mapIdToken = j["mapId"];
			bool hasMapPayload = mapIdToken != null && mapIdToken.Type != JTokenType.Null;
			if (!hasMapPayload)
			{
				JArray markers = j["markers"] as JArray;
				if (markers != null && markers.Count > 0)
				{
					hasMapPayload = true;
				}
			}
			if (!hasMapPayload)
			{
				JArray marks = j["marks"] as JArray;
				if (marks != null && marks.Count > 0)
				{
					hasMapPayload = true;
				}
			}
			return !hasMapPayload;
		}

		private static string Encode(string json)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
		}

		private static string Decode(string text)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(text));
		}
	}
}
