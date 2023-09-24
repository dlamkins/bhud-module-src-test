using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Presets.Model
{
	[Serializable]
	public class MarkerSet
	{
		[JsonProperty("enabled")]
		public bool enabled = true;

		[JsonProperty("name")]
		public string? name { get; set; }

		[JsonProperty("description")]
		public string? description { get; set; }

		[JsonProperty("mapId")]
		public int? mapId { get; set; }

		[JsonProperty("trigger")]
		public WorldCoord? trigger { get; set; }

		[JsonProperty("markers")]
		public List<MarkerCoord> marks { get; set; } = new List<MarkerCoord>();


		[JsonIgnore]
		public WorldCoord Trigger => trigger ?? new WorldCoord();

		[JsonIgnore]
		public int MapId => mapId.GetValueOrDefault();

		[JsonIgnore]
		public string MapName => Service.MapDataCache.Describe(MapId);

		public string DescribeMarkers()
		{
			return string.Join("\n", marks);
		}

		public void CloneFromMarkerSet(MarkerSet otherSet)
		{
			name = otherSet.name;
			description = otherSet.description;
			mapId = otherSet.mapId;
			trigger = otherSet.trigger;
			marks = new List<MarkerCoord>();
			enabled = otherSet.enabled;
			for (int i = 0; i < otherSet.marks.Count && i < 8; i++)
			{
				marks.Add(otherSet.marks[i]);
			}
		}
	}
}
