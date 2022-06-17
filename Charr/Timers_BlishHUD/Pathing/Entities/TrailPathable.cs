using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Pathing.Entities
{
	public class TrailPathable : Entity
	{
		private VertexPositionColorTexture[] _vertexData;

		private VertexBuffer _vertexBuffer;

		private static readonly TrailEffect _sharedTrailEffect;

		private static readonly Texture2D _fadeTexture;

		public Vector3 PointA { get; set; }

		public Vector3 PointB { get; set; }

		public AsyncTexture2D TrailTexture { get; set; }

		public float AnimationSpeed { get; set; } = 1f;


		public float FadeNear { get; set; } = 5000f;


		public float FadeFar { get; set; } = 5000f;


		public int TrailResolution { get; set; } = 20;


		public Color TintColor { get; set; } = Color.get_White();


		public float TrailWidth { get; set; } = 0.508f;


		public float FadeRadius { get; set; } = 1f;


		public bool ShouldShow { get; set; }

		public float TrailLength => Vector3.Distance(PointA, PointB);

		static TrailPathable()
		{
			_sharedTrailEffect = new TrailEffect(GameService.Content.ContentManager.Load<Effect>("effects\\trail"));
			_fadeTexture = TimersModule.ModuleInstance.Resources.TextureFade;
		}

		public override void HandleRebuild(GraphicsDevice graphicsDevice)
		{
		}

		private static List<Vector3> BuildTrailWithResolution(List<Vector3> points, float pointResolution)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (!points.Any())
			{
				return new List<Vector3>(0);
			}
			List<Vector3> tempPoints = new List<Vector3>();
			Vector3 lstPoint = points[0];
			for (int i = 0; i < points.Count; i++)
			{
				float s = Vector3.Distance(lstPoint, points[i]) / pointResolution;
				float inc = 1f / s;
				for (float v = inc; v < s - inc; v += inc)
				{
					Vector3 nPoint = Vector3.Lerp(lstPoint, points[i], v / s);
					tempPoints.Add(nPoint);
				}
				tempPoints.Add(points[i]);
				lstPoint = points[i];
			}
			return tempPoints;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Expected O, but got Unknown
			if (Visible && ShouldShow)
			{
				List<Vector3> trailPoints = BuildTrailWithResolution(new List<Vector3>((IEnumerable<Vector3>)(object)new Vector3[2] { PointA, PointB }), TrailResolution);
				_vertexData = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[trailPoints.Count * 2];
				float pastDistance = TrailLength;
				Vector3 offsetDirection = default(Vector3);
				((Vector3)(ref offsetDirection))._002Ector(0f, 0f, -1f);
				Vector3 currPoint = trailPoints[0];
				Vector3 offset = Vector3.get_Zero();
				for (int i = 0; i < trailPoints.Count - 1; i++)
				{
					Vector3 nextPoint = trailPoints[i + 1];
					offset = Vector3.Cross(nextPoint - currPoint, offsetDirection);
					((Vector3)(ref offset)).Normalize();
					Vector3 leftPoint = currPoint + offset * TrailWidth;
					Vector3 rightPoint = currPoint + offset * (0f - TrailWidth);
					_ = pastDistance / (TrailWidth * 2f);
					_vertexData[i * 2 + 1] = new VertexPositionColorTexture(leftPoint, Color.get_White(), new Vector2(0f, pastDistance / (TrailWidth * 2f) - 1f));
					_vertexData[i * 2] = new VertexPositionColorTexture(rightPoint, Color.get_White(), new Vector2(1f, pastDistance / (TrailWidth * 2f) - 1f));
					pastDistance -= Vector3.Distance(currPoint, nextPoint);
					currPoint = nextPoint;
				}
				Vector3 fleftPoint = currPoint + offset * TrailWidth;
				Vector3 frightPoint = currPoint + offset * (0f - TrailWidth);
				_vertexData[trailPoints.Count * 2 - 1] = new VertexPositionColorTexture(fleftPoint, Color.get_White(), new Vector2(0f, pastDistance / (TrailWidth * 4f) - 1f));
				_vertexData[trailPoints.Count * 2 - 2] = new VertexPositionColorTexture(frightPoint, Color.get_White(), new Vector2(1f, pastDistance / (TrailWidth * 4f) - 1f));
				VertexBuffer vertexBuffer = _vertexBuffer;
				if (vertexBuffer != null)
				{
					((GraphicsResource)vertexBuffer).Dispose();
				}
				_vertexBuffer = new VertexBuffer(GameService.Graphics.GraphicsDevice, VertexPositionColorTexture.VertexDeclaration, _vertexData.Length, (BufferUsage)1);
				_vertexBuffer.SetData<VertexPositionColorTexture>(_vertexData);
			}
		}

		public override void Draw(GraphicsDevice graphicsDevice)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			if (!Visible || !ShouldShow || TrailTexture == null || _vertexData == null || _vertexData.Length < 3)
			{
				return;
			}
			_sharedTrailEffect.SetEntityState(TrailTexture, AnimationSpeed, FadeNear, FadeFar, Opacity, FadeRadius, fadeCenter: true, _fadeTexture, TintColor);
			graphicsDevice.SetVertexBuffer(_vertexBuffer, 0);
			Enumerator enumerator = ((Effect)_sharedTrailEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawPrimitives((PrimitiveType)1, 0, _vertexBuffer.get_VertexCount() - 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}
	}
}
