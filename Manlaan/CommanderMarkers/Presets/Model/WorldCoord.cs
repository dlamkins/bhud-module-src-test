using System;
using Blish_HUD;
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

		public WorldCoord FromWorldCoord(WorldCoord coord)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return FromVector3(coord.ToVector3());
		}

		public WorldCoord FromVector3(Vector3 position)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			x = position.X;
			y = position.Y;
			z = position.Z;
			return this;
		}

		public WorldCoord SetFromMumbleLocation()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return FromVector3(GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
		}
	}
}
