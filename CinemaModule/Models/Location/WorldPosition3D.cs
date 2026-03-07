using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace CinemaModule.Models.Location
{
	public class WorldPosition3D
	{
		private const float MinLengthSquared = 0.0001f;

		private const float FullRotation = 360f;

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

		public void GetDirections(out Vector3 normal, out Vector3 up, out Vector3 right)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			float num = MathHelper.ToRadians(Pitch);
			float yawRad = MathHelper.ToRadians(Yaw);
			float cosP = (float)Math.Cos(num);
			float sinP = (float)Math.Sin(num);
			float sinY = (float)Math.Sin(yawRad);
			float cosY = (float)Math.Cos(yawRad);
			normal = new Vector3(cosP * sinY, cosP * cosY, sinP);
			up = new Vector3((0f - sinP) * sinY, (0f - sinP) * cosY, cosP);
			right = Vector3.Cross(up, normal);
			if (((Vector3)(ref right)).LengthSquared() > 0.0001f)
			{
				((Vector3)(ref right)).Normalize();
			}
		}

		public Vector3 GetNormalDirection()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			GetDirections(out var normal, out var _, out var _);
			return normal;
		}

		public Vector3 GetUpDirection()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			GetDirections(out var _, out var up, out var _);
			return up;
		}

		public Vector3 GetRightDirection()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			GetDirections(out var _, out var _, out var right);
			return right;
		}

		public static float NormalizeYaw(float yaw)
		{
			yaw %= 360f;
			if (!(yaw < 0f))
			{
				return yaw;
			}
			return yaw + 360f;
		}
	}
}
