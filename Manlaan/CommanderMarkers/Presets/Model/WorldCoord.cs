using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Presets.Model
{
	[Serializable]
	public class WorldCoord
	{
		[JsonProperty("x")]
		public float x { get; set; }

		[JsonProperty("y")]
		public float y { get; set; }

		[JsonProperty("z")]
		public float z { get; set; }

		public Vector3 ToVector3()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(x, y, z);
		}
	}
}
