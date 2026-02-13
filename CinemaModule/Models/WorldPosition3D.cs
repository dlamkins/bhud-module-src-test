using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class WorldPosition3D
	{
		[JsonProperty("x")]
		public float X { get; set; }

		[JsonProperty("y")]
		public float Y { get; set; }

		[JsonProperty("z")]
		public float Z { get; set; }

		[JsonProperty("yaw")]
		public float Yaw { get; set; }

		[JsonProperty("pitch")]
		public float Pitch { get; set; }

		[JsonProperty("mapId")]
		public int MapId { get; set; }

		public WorldPosition3D()
			: this(0f, 0f, 0f, 0f, 0f, 0)
		{
		}

		public WorldPosition3D(float x, float y, float z, int mapId)
			: this(x, y, z, 0f, 0f, mapId)
		{
		}

		public WorldPosition3D(float x, float y, float z, float yaw, float pitch, int mapId)
		{
			X = x;
			Y = y;
			Z = z;
			Yaw = yaw;
			Pitch = pitch;
			MapId = mapId;
		}

		public Vector3 ToVector3()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(X, Y, Z);
		}

		public Vector3 GetRightDirection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Vector3 normal = GetNormalDirection();
			Vector3 right = Vector3.Cross(GetUpDirection(), normal);
			if (((Vector3)(ref right)).LengthSquared() > 0.0001f)
			{
				((Vector3)(ref right)).Normalize();
			}
			return right;
		}

		public Vector3 GetUpDirection()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			float num = MathHelper.ToRadians(Pitch);
			float yawRad = MathHelper.ToRadians(Yaw);
			float cosP = (float)Math.Cos(num);
			float sinP = (float)Math.Sin(num);
			return new Vector3((0f - sinP) * (float)Math.Sin(yawRad), (0f - sinP) * (float)Math.Cos(yawRad), cosP);
		}

		public Vector3 GetNormalDirection()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			float num = MathHelper.ToRadians(Pitch);
			float yawRad = MathHelper.ToRadians(Yaw);
			float cosP = (float)Math.Cos(num);
			float sinP = (float)Math.Sin(num);
			return new Vector3(cosP * (float)Math.Sin(yawRad), cosP * (float)Math.Cos(yawRad), sinP);
		}

		public static float NormalizeYaw(float yaw)
		{
			while (yaw < 0f)
			{
				yaw += 360f;
			}
			while (yaw >= 360f)
			{
				yaw -= 360f;
			}
			return yaw;
		}
	}
}
