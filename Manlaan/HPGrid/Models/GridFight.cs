using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Manlaan.HPGrid.Models
{
	public class GridFight
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("position")]
		public List<float> Position { get; set; }

		[JsonPropertyName("radius")]
		public float Radius { get; set; }

		[JsonPropertyName("phase")]
		public List<GridPhase> Phase { get; set; }

		public bool InRadius(Vector3 point)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = new Vector3(Position[0], Position[1], Position[2]) - point;
			float distance = ((Vector3)(ref val)).Length();
			if (distance <= Radius)
			{
				return true;
			}
			return false;
		}
	}
}
