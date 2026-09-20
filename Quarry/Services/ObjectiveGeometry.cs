using System;
using Microsoft.Xna.Framework;
using Quarry.Models.Markers;

namespace Quarry.Services
{
	public static class ObjectiveGeometry
	{
		public static Vector3 ToWorld(AchievementObjective objective)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(objective.X, objective.Z, objective.Y);
		}

		public static float DistanceMetres(Vector3 player, AchievementObjective objective)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Vector3 world = ToWorld(objective);
			if (objective.HeightUnknown)
			{
				float num = player.X - world.X;
				float dy = player.Y - world.Y;
				return (float)Math.Sqrt(num * num + dy * dy);
			}
			return Vector3.Distance(player, world);
		}
	}
}
