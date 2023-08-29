using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers
{
	public static class MumbleUtils
	{
		private const int MinCompassWidth = 170;

		private const int MaxCompassWidth = 362;

		private const int MinCompassHeight = 170;

		private const int MaxCompassHeight = 338;

		private const int MinCompassOffset = 19;

		private const int MaxCompassOffset = 40;

		private const int CompassSeparation = 40;

		private static int GetCompassOffset(float curr, float min, float max)
		{
			return (int)Math.Round(MathUtils.Scale(curr, min, max, 19.0, 40.0));
		}

		public static Rectangle GetMapBounds()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			Size compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			if (((Size)(ref compassSize)).get_Width() >= 1)
			{
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				if (((Size)(ref compassSize)).get_Height() >= 1)
				{
					if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
					{
						return new Rectangle(Point.get_Zero(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
					}
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					int offsetWidth = GetCompassOffset(((Size)(ref compassSize)).get_Width(), 170f, 362f);
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					int offsetHeight = GetCompassOffset(((Size)(ref compassSize)).get_Height(), 170f, 338f);
					int width = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width();
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					int num = width - ((Size)(ref compassSize)).get_Width() - offsetWidth;
					int num2;
					if (!GameService.Gw2Mumble.get_UI().get_IsCompassTopRight())
					{
						int height = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
						compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
						num2 = height - ((Size)(ref compassSize)).get_Height() - offsetHeight - 40;
					}
					else
					{
						num2 = 0;
					}
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					int num3 = ((Size)(ref compassSize)).get_Width() + offsetWidth;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					return new Rectangle(num, num2, num3, ((Size)(ref compassSize)).get_Height() + offsetHeight);
				}
			}
			return default(Rectangle);
		}
	}
}
