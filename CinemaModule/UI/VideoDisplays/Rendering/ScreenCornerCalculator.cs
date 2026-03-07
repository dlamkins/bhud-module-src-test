using CinemaModule.Models.Location;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.VideoDisplays.Rendering
{
	public class ScreenCornerCalculator
	{
		private const float BorderSize = 0.15f;

		private const float Depth = 0.4f;

		private const float VideoInset = 0.08f;

		private const float BackLogoOffset = 0.03f;

		private const float LogoSizeRatio = 0.75f;

		private const float LogoVerticalOffset = 0.12f;

		private const float TextWidthRatio = 1.1f;

		private const float TextHeightRatio = 0.25f;

		private const float TextVerticalOffset = 0.55f;

		public Vector3[] WorldCorners { get; } = (Vector3[])(object)new Vector3[4];


		public Vector3[] BorderCorners { get; } = (Vector3[])(object)new Vector3[4];


		public Vector3[] InnerFrameCorners => WorldCorners;

		public Vector3[] BackCorners { get; } = (Vector3[])(object)new Vector3[4];


		public Vector3[] LogoCorners { get; } = (Vector3[])(object)new Vector3[4];


		public Vector3[] TextCorners { get; } = (Vector3[])(object)new Vector3[4];


		public bool IsValid { get; private set; }

		public void Recalculate(WorldPosition3D worldPosition, float worldWidth, float aspectRatio)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (worldPosition == null || worldPosition.MapId == 0)
			{
				IsValid = false;
				return;
			}
			Vector3 bottomCenter = worldPosition.ToVector3();
			Vector3 right = worldPosition.GetRightDirection();
			Vector3 up = worldPosition.GetUpDirection();
			Vector3 normal = worldPosition.GetNormalDirection();
			float worldHeight = worldWidth / aspectRatio;
			CalculateBorderCorners(bottomCenter, right, up, worldWidth, worldHeight);
			CalculateVideoCorners(bottomCenter, right, up, normal, worldWidth, worldHeight);
			CalculateBackCorners(normal);
			CalculateBackPanelElements(right, up, normal);
			IsValid = true;
		}

		private void CalculateBorderCorners(Vector3 bottomCenter, Vector3 right, Vector3 up, float worldWidth, float worldHeight)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 halfRight = right * (worldWidth / 2f + 0.15f);
			Vector3 fullUp = up * (worldHeight + 0.15f);
			Vector3 bottomOffset = -up * 0.15f;
			SetQuadCorners(BorderCorners, bottomCenter, halfRight, fullUp, bottomOffset);
		}

		private void CalculateVideoCorners(Vector3 bottomCenter, Vector3 right, Vector3 up, Vector3 normal, float worldWidth, float worldHeight)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			Vector3 halfRight = right * (worldWidth / 2f);
			Vector3 fullUp = up * worldHeight;
			Vector3 insetOffset = -normal * 0.08f;
			SetQuadCorners(WorldCorners, bottomCenter + insetOffset, halfRight, fullUp, Vector3.get_Zero());
		}

		private void CalculateBackCorners(Vector3 normal)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 depthOffset = -normal * 0.4f;
			for (int i = 0; i < 4; i++)
			{
				BackCorners[i] = BorderCorners[i] + depthOffset;
			}
		}

		private void CalculateBackPanelElements(Vector3 right, Vector3 up, Vector3 normal)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = (BackCorners[0] + BackCorners[1] + BackCorners[2] + BackCorners[3]) / 4f;
			Vector3 val2 = BackCorners[0] - BackCorners[3];
			float backHeight = ((Vector3)(ref val2)).Length();
			float logoSize = backHeight * 0.75f;
			Vector3 vertOffset = up * (backHeight * 0.12f);
			Vector3 logoHalfUp = up * (logoSize / 2f);
			SetQuadCorners(center: val + vertOffset - normal * 0.03f, corners: LogoCorners, halfRight: right * (logoSize / 2f), halfUp: logoHalfUp, bottomOffset: -logoHalfUp);
			float textWidth = logoSize * 1.1f;
			float textHeight = textWidth * 0.25f;
			Vector3 textHalfUp = up * (textHeight / 2f);
			Vector3 textCenter = val + vertOffset - up * (logoSize * 0.55f) - normal * 0.04f;
			SetQuadCorners(TextCorners, textCenter, right * (textWidth / 2f), textHalfUp, -textHalfUp);
		}

		private static void SetQuadCorners(Vector3[] corners, Vector3 center, Vector3 halfRight, Vector3 halfUp, Vector3 bottomOffset)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			corners[0] = center - halfRight + halfUp;
			corners[1] = center + halfRight + halfUp;
			corners[2] = center + halfRight + bottomOffset;
			corners[3] = center - halfRight + bottomOffset;
		}
	}
}
