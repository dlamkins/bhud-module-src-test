using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser
{
	public static class TutorialWorldProjection
	{
		public static bool TryProject(Vector3 worldMeters, out WorldProjectionResult result)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			result = default(WorldProjectionResult);
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return false;
			}
			int screenWidth = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
			int screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			if (screenWidth <= 0 || screenHeight <= 0)
			{
				return false;
			}
			PlayerCamera camera = GameService.Gw2Mumble.get_PlayerCamera();
			Viewport viewport = default(Viewport);
			((Viewport)(ref viewport))._002Ector(0, 0, screenWidth, screenHeight);
			Vector3 projected = ((Viewport)(ref viewport)).Project(worldMeters, camera.get_Projection(), camera.get_View(), Matrix.get_Identity());
			Vector2 screen = default(Vector2);
			((Vector2)(ref screen))._002Ector(projected.X, projected.Y);
			float z = projected.Z;
			bool inFront = z >= 0f && z <= 1f;
			bool onScreen = inFront && screen.X >= 0f && screen.X <= (float)screenWidth && screen.Y >= 0f && screen.Y <= (float)screenHeight;
			Vector2 center = default(Vector2);
			((Vector2)(ref center))._002Ector((float)screenWidth / 2f, (float)screenHeight / 2f);
			Vector2 direction = screen - center;
			if (!inFront)
			{
				direction = center - screen;
			}
			if (((Vector2)(ref direction)).LengthSquared() < 0.001f)
			{
				((Vector2)(ref direction))._002Ector(0f, -1f);
			}
			else
			{
				((Vector2)(ref direction)).Normalize();
			}
			float edgeRadius = (float)screenHeight / 3f;
			Vector2 edgeScreen = center + direction * edgeRadius;
			float arrowAngle = (float)Math.Atan2(direction.Y, direction.X);
			result = new WorldProjectionResult(screen, inFront, onScreen, edgeScreen, arrowAngle);
			return true;
		}
	}
}
