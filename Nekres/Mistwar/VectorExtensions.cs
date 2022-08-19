using System;
using System.Numerics;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	internal static class VectorExtensions
	{
		public static Vector2 XY(this Vector3 vector)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(vector.X, vector.Y);
		}

		public static Vector2 XY(this Vector3 vector)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(vector.X, vector.Y);
		}

		public static Vector3 ToScreenSpace(this Vector3 position, Matrix view, Matrix projection)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			int screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			position = Vector3.Transform(position, view);
			position = Vector3.Transform(position, projection);
			float num = position.X / position.Z;
			float y = position.Y / (0f - position.Z);
			float num2 = (num + 1f) * (float)screenWidth / 2f;
			y = (y + 1f) * (float)screenHeight / 2f;
			return new Vector3(num2, y, position.Z);
		}

		public static Vector2 Flatten(this Vector3 v)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((v.X / v.Z + 1f) / 2f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), (1f - v.Y / v.Z) / 2f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
		}

		public static Vector2 ToXnaVector2(this Coordinates2 coords)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)((Coordinates2)(ref coords)).get_X(), (float)((Coordinates2)(ref coords)).get_Y());
		}

		public static Vector2 ToVector2(this Vector3 vector)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(vector.X, vector.Y);
		}

		public static float Distance(this Vector3 v1, Vector3 v2)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = v1 - v2;
			return ((Vector3)(ref val)).Length();
		}

		public static double Angle(this Vector3 v, Vector3 u)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Math.Acos(Vector3.Dot(v, u) / (((Vector3)(ref v)).Length() * ((Vector3)(ref u)).Length()));
		}
	}
}
