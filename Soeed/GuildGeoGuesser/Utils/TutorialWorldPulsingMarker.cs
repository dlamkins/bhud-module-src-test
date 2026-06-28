using System;
using Blish_HUD;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser.Utils
{
	public static class TutorialWorldPulsingMarker
	{
		private const int CircleSegments = 64;

		private const float OuterRadiusMin = 1.4f;

		private const float OuterRadiusPulse = 0.6f;

		private const float InnerRadiusRatio = 0.55f;

		private static BasicEffect? _lineEffect;

		private static readonly VertexPositionColor[] RingVertices = (VertexPositionColor[])(object)new VertexPositionColor[65];

		public static float GetInnerRingDiameter(float pulse)
		{
			return GetInnerRingRadius(pulse) * 2f;
		}

		public static float GetInnerRingRadius(float pulse)
		{
			return GetOuterRingRadius(pulse) * 0.55f;
		}

		public static float GetOuterRingRadius(float pulse)
		{
			return 1.4f + pulse * 0.6f;
		}

		public static float GetDistanceOpacity(float distanceMeters, float nearDistance = 2f, float nearOpacity = 0.3f, float farOpacity = 0.8f)
		{
			if (!(distanceMeters <= nearDistance))
			{
				return farOpacity;
			}
			return nearOpacity;
		}

		public static void DrawRings(GraphicsDevice graphicsDevice, Vector3 worldCenter, float pulse, Color ringColor, float distanceMeters)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			float distanceOpacity = GetDistanceOpacity(distanceMeters);
			float outerRadius = GetOuterRingRadius(pulse);
			float innerRadius = GetInnerRingRadius(pulse);
			PlayerCamera camera = GameService.Gw2Mumble.get_PlayerCamera();
			Vector3 toCamera = camera.get_Position() - worldCenter;
			if (((Vector3)(ref toCamera)).LengthSquared() < 0.0001f)
			{
				toCamera = camera.get_Forward();
			}
			((Vector3)(ref toCamera)).Normalize();
			Vector3 right = Vector3.Cross(Vector3.get_UnitZ(), toCamera);
			if (((Vector3)(ref right)).LengthSquared() < 0.0001f)
			{
				right = Vector3.get_UnitX();
			}
			((Vector3)(ref right)).Normalize();
			Vector3 up = Vector3.Cross(toCamera, right);
			((Vector3)(ref up)).Normalize();
			if (_lineEffect == null)
			{
				_lineEffect = CreateLineEffect(graphicsDevice);
			}
			_lineEffect!.set_View(camera.get_View());
			_lineEffect!.set_Projection(camera.get_Projection());
			_lineEffect!.set_World(Matrix.get_Identity());
			BlendState previousBlend = graphicsDevice.get_BlendState();
			DepthStencilState previousDepth = graphicsDevice.get_DepthStencilState();
			RasterizerState previousRaster = graphicsDevice.get_RasterizerState();
			graphicsDevice.set_BlendState(BlendState.AlphaBlend);
			graphicsDevice.set_DepthStencilState(DepthStencilState.None);
			graphicsDevice.set_RasterizerState(RasterizerState.CullNone);
			DrawRing(graphicsDevice, worldCenter, right, up, outerRadius, ringColor * distanceOpacity);
			DrawRing(graphicsDevice, worldCenter, right, up, innerRadius, Color.get_White() * distanceOpacity);
			graphicsDevice.set_BlendState(previousBlend);
			graphicsDevice.set_DepthStencilState(previousDepth);
			graphicsDevice.set_RasterizerState(previousRaster);
		}

		private static void DrawRing(GraphicsDevice graphicsDevice, Vector3 center, Vector3 right, Vector3 up, float radius, Color color)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			for (int segment = 0; segment <= 64; segment++)
			{
				double angle = (double)segment * Math.PI * 2.0 / 64.0;
				Vector3 offset = right * (float)(Math.Cos(angle) * (double)radius) + up * (float)(Math.Sin(angle) * (double)radius);
				RingVertices[segment] = new VertexPositionColor(center + offset, color);
			}
			Enumerator enumerator = ((Effect)_lineEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, RingVertices, 0, 64);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private static BasicEffect CreateLineEffect(GraphicsDevice graphicsDevice)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			BasicEffect val = new BasicEffect(graphicsDevice);
			val.set_VertexColorEnabled(true);
			val.set_TextureEnabled(false);
			return val;
		}
	}
}
