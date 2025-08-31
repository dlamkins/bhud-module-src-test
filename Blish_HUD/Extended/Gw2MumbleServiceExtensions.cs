using System;
using Blish_HUD.Gw2Mumble;
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
				return playerCharacter.Position;
			}
			return playerCharacter.Position.SwapYZ();
		}

		public static Vector3 Position(this PlayerCamera playerCamera, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCamera.Position;
			}
			return playerCamera.Position.SwapYZ();
		}

		public static Vector3 Forward(this PlayerCharacter playerCharacter, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCharacter.Forward;
			}
			return playerCharacter.Forward.SwapYZ();
		}

		public static Vector3 Forward(this PlayerCamera playerCamera, bool swapYZ)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (!swapYZ)
			{
				return playerCamera.Forward;
			}
			return playerCamera.Forward.SwapYZ();
		}

		private static int GetOffset(float curr, float max, float min, float val)
		{
			return (int)Math.Round((curr - min) / (max - min) * (val - 19f) + 19f, 0);
		}

		public static Rectangle CompassBounds(this UI ui)
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			int offsetWidth = GetOffset(ui.CompassSize.Width, 362f, 170f, 40f);
			int offsetHeight = GetOffset(ui.CompassSize.Height, 338f, 170f, 40f);
			int width = ui.CompassSize.Width + offsetWidth;
			int height = ui.CompassSize.Height + offsetHeight;
			int num = GameService.Graphics.SpriteScreen.ContentRegion.Width - width;
			int y = 0;
			if (!ui.IsCompassTopRight)
			{
				y += GameService.Graphics.SpriteScreen.ContentRegion.Height - height - 40;
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
