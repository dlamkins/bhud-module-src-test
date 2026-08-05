using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Util
{
	public static class WorldProjection
	{
		public static bool TryProject(Vector3 world, int screenWidth, int screenHeight, out Vector2 screen, out float depth)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			screen = Vector2.get_Zero();
			depth = 0f;
			Matrix wvp = GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection();
			Vector4 clip = Vector4.Transform(new Vector4(world, 1f), wvp);
			if (clip.W <= 0.0001f)
			{
				return false;
			}
			depth = clip.W;
			float ndcX = clip.X / clip.W;
			float ndcY = clip.Y / clip.W;
			screen = new Vector2((ndcX * 0.5f + 0.5f) * (float)screenWidth, (1f - (ndcY * 0.5f + 0.5f)) * (float)screenHeight);
			return true;
		}

		public static Vector2 ContinentDirToWorld(Vector2 continentDir, float axisSignX, float axisSignY)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(continentDir.X * axisSignX, continentDir.Y * axisSignY);
		}
	}
}
