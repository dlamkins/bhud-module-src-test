using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser.Utils
{
	public static class TutorialMarkerDraw
	{
		public static void DrawPulsingMarker(SpriteBatch spriteBatch, AsyncTexture2D icon, Vector2 screenPosition, float pulse, Color ringColor)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			if (icon.get_HasTexture())
			{
				Rectangle iconRect = default(Rectangle);
				((Rectangle)(ref iconRect))._002Ector((int)screenPosition.X - 16, (int)screenPosition.Y - 16, 32, 32);
				spriteBatch.Draw(icon.get_Texture(), iconRect, Color.get_White());
			}
			float ringRadius = 14f + pulse * 10f;
			float ringOpacity = 0.65f + pulse * 0.35f;
			int ringSides = SpriteBatchExtensions.GetSidesFor(ringRadius, Math.PI * 2.0);
			spriteBatch.DrawArc(screenPosition, ringRadius, ringRadius, 0.0, 2.0, ringSides, ringColor * ringOpacity, 3f);
			spriteBatch.DrawArc(screenPosition, ringRadius * 0.55f, ringRadius * 0.55f, 0.0, 2.0, ringSides, Color.get_White() * (0.55f + pulse * 0.35f), 2f);
		}

		public static void DrawOffScreenPointer(SpriteBatch spriteBatch, Vector2 screenPosition, float angleRadians, float pulse, Color color)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			float ringRadius = 14f + pulse * 10f;
			Vector2 forward = default(Vector2);
			((Vector2)(ref forward))._002Ector((float)Math.Cos(angleRadians), (float)Math.Sin(angleRadians));
			Vector2 perpendicular = default(Vector2);
			((Vector2)(ref perpendicular))._002Ector(0f - forward.Y, forward.X);
			float opacity = 0.85f + pulse * 0.15f;
			Color lineColor = color * opacity;
			Vector2 tip = screenPosition + forward * (ringRadius + 16f + pulse * 4f);
			float wingSpread = 9f + pulse * 2f;
			float wingInset = 10f + pulse * 2f;
			Vector2 val = screenPosition + forward * (ringRadius + 2f);
			Vector2 left = val + perpendicular * wingSpread - forward * wingInset;
			Vector2 right = val - perpendicular * wingSpread - forward * wingInset;
			spriteBatch.DrawPolygon(Vector2.get_Zero(), (IEnumerable<Vector2>)(object)new Vector2[2] { tip, left }, lineColor, 3f, 0f, open: true);
			spriteBatch.DrawPolygon(Vector2.get_Zero(), (IEnumerable<Vector2>)(object)new Vector2[2] { tip, right }, lineColor, 3f, 0f, open: true);
		}
	}
}
