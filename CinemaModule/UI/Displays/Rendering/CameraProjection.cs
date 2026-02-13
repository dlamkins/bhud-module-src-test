using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Displays.Rendering
{
	public static class CameraProjection
	{
		private const float NearPlane = 0.1f;

		private const float FarPlane = 1000f;

		private const float ClipSpaceEpsilon = 0.01f;

		private static readonly Vector3 WorldUp = new Vector3(0f, 0f, 1f);

		public static void GetCameraOrientation(Vector3 cameraForward, out Vector3 right, out Vector3 up)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			right = Vector3.Cross(cameraForward, WorldUp);
			if (((Vector3)(ref right)).LengthSquared() < 0.0001f)
			{
				right = new Vector3(1f, 0f, 0f);
			}
			else
			{
				((Vector3)(ref right)).Normalize();
			}
			up = Vector3.Cross(right, cameraForward);
			((Vector3)(ref up)).Normalize();
		}

		public static void CreateViewProjectionMatrices(Vector3 cameraPosition, Vector3 cameraForward, float fieldOfView, float aspectRatio, out Matrix viewMatrix, out Matrix projectionMatrix)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			GetCameraOrientation(cameraForward, out var _, out var cameraUp);
			viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraForward, cameraUp);
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 0.1f, 1000f);
		}

		public static Vector2 WorldToScreen(Vector3 worldPosition, Vector3 cameraPosition, Vector3 cameraForward, float fieldOfView)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Screen spriteScreen = GameService.Graphics.get_SpriteScreen();
			float aspectRatio = (float)((Control)spriteScreen).get_Width() / (float)((Control)spriteScreen).get_Height();
			CreateViewProjectionMatrices(cameraPosition, cameraForward, fieldOfView, aspectRatio, out var viewMatrix, out var projectionMatrix);
			return ProjectToScreen(worldPosition, viewMatrix * projectionMatrix, ((Control)spriteScreen).get_Width(), ((Control)spriteScreen).get_Height());
		}

		private static Vector2 ProjectToScreen(Vector3 worldPosition, Matrix viewProjection, int screenWidth, int screenHeight)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			Vector4 clipSpace = Vector4.Transform(new Vector4(worldPosition, 1f), viewProjection);
			if (clipSpace.W <= 0.01f)
			{
				return new Vector2(-10000f, -10000f);
			}
			float inverseW = 1f / clipSpace.W;
			Vector3 normalizedDeviceCoordinates = default(Vector3);
			((Vector3)(ref normalizedDeviceCoordinates))._002Ector(clipSpace.X * inverseW, clipSpace.Y * inverseW, clipSpace.Z * inverseW);
			return new Vector2((normalizedDeviceCoordinates.X + 1f) * 0.5f * (float)screenWidth, (1f - normalizedDeviceCoordinates.Y) * 0.5f * (float)screenHeight);
		}

		public static Vector3 SafeNormalize(Vector3 vector)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (((Vector3)(ref vector)).LengthSquared() > 0.0001f)
			{
				((Vector3)(ref vector)).Normalize();
			}
			return vector;
		}
	}
}
