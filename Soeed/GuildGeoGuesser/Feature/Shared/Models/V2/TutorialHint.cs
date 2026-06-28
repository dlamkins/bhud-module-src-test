using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	[Serializable]
	public class TutorialHint
	{
		[JsonProperty("mapId")]
		public int MapId { get; set; }

		[JsonProperty("position")]
		public float[] Position { get; set; } = Array.Empty<float>();


		[JsonProperty("continent")]
		public float[] Continent { get; set; } = Array.Empty<float>();


		[JsonProperty("label")]
		public string Label { get; set; } = "";


		[JsonProperty("chatCode")]
		public string ChatCode { get; set; } = "";


		public Vector3 ToVector3()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (Position.Length < 3)
			{
				return Vector3.get_Zero();
			}
			return new Vector3(Position[0], Position[1], Position[2]);
		}

		public bool TryGetContinent(out Vector2 continent)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (Continent.Length >= 2)
			{
				continent = new Vector2(Continent[0], Continent[1]);
				return true;
			}
			continent = default(Vector2);
			return false;
		}
	}
}
