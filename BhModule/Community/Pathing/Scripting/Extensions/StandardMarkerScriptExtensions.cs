using System;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Scripting.Lib;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Scripting.Extensions
{
	internal static class StandardMarkerScriptExtensions
	{
		private static PackInitiator _packInitiator;

		internal static void SetPackInitiator(PackInitiator packInitiator)
		{
			_packInitiator = packInitiator;
		}

		public static void SetPos(this StandardMarker marker, float x, float y, float z)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			marker.Position = new Vector3(x, y, z);
		}

		public static void SetPos(this StandardMarker marker, Vector3 position)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			marker.Position = position;
		}

		public static void SetPosX(this StandardMarker marker, float x)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			marker.SetPos(x, marker.Position.Y, marker.Position.Z);
		}

		public static void SetPosY(this StandardMarker marker, float y)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			marker.SetPos(marker.Position.X, y, marker.Position.Z);
		}

		public static void SetPosZ(this StandardMarker marker, float z)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			marker.SetPos(marker.Position.X, marker.Position.Y, z);
		}

		public static void SetRot(this StandardMarker marker, float x, float y, float z)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			marker.RotationXyz = new Vector3(x, y, z);
		}

		public static void SetRot(this StandardMarker marker, Vector3 rotation)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			marker.RotationXyz = rotation;
		}

		public static void SetRotX(this StandardMarker marker, float x)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			marker.SetRot(x, marker.RotationXyz.GetValueOrDefault(Vector3.get_Zero()).Y, marker.RotationXyz.GetValueOrDefault(Vector3.get_Zero()).Z);
		}

		public static void SetRotY(this StandardMarker marker, float y)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			marker.SetRot(marker.RotationXyz.GetValueOrDefault(Vector3.get_Zero()).X, y, marker.RotationXyz.GetValueOrDefault(Vector3.get_Zero()).Z);
		}

		public static void SetRotZ(this StandardMarker marker, float z)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			marker.SetRot(marker.RotationXyz.GetValueOrDefault(Vector3.get_Zero()).X, marker.RotationXyz.GetValueOrDefault(Vector3.get_Zero()).Y, z);
		}

		public static void Remove(this StandardMarker marker)
		{
			marker.Unload();
			_packInitiator.PackState.RemovePathingEntity(marker);
		}

		public static void SetTexture(this StandardMarker marker, string texturePath)
		{
			marker.Texture = Instance.Texture(marker.TextureResourceManager, texturePath);
		}

		public static void SetTexture(this StandardMarker marker, int textureId)
		{
			marker.Texture = AsyncTexture2D.FromAssetId(textureId) ?? AsyncTexture2D.op_Implicit(Textures.get_Error());
		}

		public static IBehavior GetBehavior(this StandardMarker marker, string behaviorName)
		{
			foreach (IBehavior behavior in marker.Behaviors)
			{
				if (string.Equals(behavior.GetType().Name, behaviorName, StringComparison.InvariantCultureIgnoreCase))
				{
					return behavior;
				}
			}
			return null;
		}
	}
}
