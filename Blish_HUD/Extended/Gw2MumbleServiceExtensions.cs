using System;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended
{
	public static class Gw2MumbleServiceExtensions
	{
		private const int MAPWIDTH_MAX = 362;

		private const int MAPHEIGHT_MAX = 338;

		private const int MAPWIDTH_MIN = 170;

		private const int MAPHEIGHT_MIN = 170;

		private const int MAPOFFSET_MIN = 19;

		public static Vector3 Position(this PlayerCharacter playerCharacter, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCharacter.get_Position();
			}
			return playerCharacter.get_Position().SwapYZ();
		}

		public static Vector3 Position(this PlayerCamera playerCamera, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCamera.get_Position();
			}
			return playerCamera.get_Position().SwapYZ();
		}

		public static Vector3 Forward(this PlayerCharacter playerCharacter, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCharacter.get_Forward();
			}
			return playerCharacter.get_Forward().SwapYZ();
		}

		public static Vector3 Forward(this PlayerCamera playerCamera, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCamera.get_Forward();
			}
			return playerCamera.get_Forward().SwapYZ();
		}

		private static int GetOffset(float curr, float max, float min, float val)
		{
			return (int)Math.Round((curr - min) / (max - min) * (val - 19f) + 19f, 0);
		}

		public static Rectangle CompassBounds(this UI ui)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			Size compassSize = ui.get_CompassSize();
			int offsetWidth = GetOffset(((Size)(ref compassSize)).get_Width(), 362f, 170f, 40f);
			compassSize = ui.get_CompassSize();
			int offsetHeight = GetOffset(((Size)(ref compassSize)).get_Height(), 338f, 170f, 40f);
			compassSize = ui.get_CompassSize();
			int width = ((Size)(ref compassSize)).get_Width() + offsetWidth;
			compassSize = ui.get_CompassSize();
			int height = ((Size)(ref compassSize)).get_Height() + offsetHeight;
			int num = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Width - width;
			int y = 0;
			if (!ui.get_IsCompassTopRight())
			{
				y += ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Height - height - 40;
			}
			return new Rectangle(num, y, width, height);
		}

		private static Vector3 SwapYZ(this Vector3 vec)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(vec.X, vec.Z, vec.Y);
		}
	}
}
