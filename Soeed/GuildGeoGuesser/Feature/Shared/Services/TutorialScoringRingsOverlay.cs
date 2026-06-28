using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public sealed class TutorialScoringRingsOverlay : Control, IDisposable
	{
		internal readonly struct ScoreRing
		{
			public float RadiusMeters { get; }

			public Color Color { get; }

			public string Label { get; }

			public bool IsRainbow { get; }

			public ScoreRing(float radiusMeters, Color color, string label, bool isRainbow)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				RadiusMeters = radiusMeters;
				Color = color;
				Label = label;
				IsRainbow = isRainbow;
			}
		}

		private const int CircleSegments = 72;

		private const float MaxRingRadiusMeters = 250f;

		private const float MinRingRadiusMeters = 0.01f;

		private const float RingOpacity = 0.85f;

		private const float RainbowRotationSpeed = 1.25f;

		private static BasicEffect? _lineEffect;

		private readonly Location _puzzleLocation;

		private readonly IReadOnlyList<ScoreRing> _rings;

		private readonly VertexPositionColor[] _ringVertices = (VertexPositionColor[])(object)new VertexPositionColor[73];

		private readonly VertexPositionColor[] _discVertices = (VertexPositionColor[])(object)new VertexPositionColor[216];

		private double _animationTime;

		public TutorialScoringRingsOverlay(Location puzzleLocation)
			: this()
		{
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			_puzzleLocation = puzzleLocation;
			_rings = BuildScoreRings(Service.Config.Scores);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_ZIndex(2147483635);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			_animationTime += gameTime.get_ElapsedGameTime().TotalSeconds;
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
		}

		private bool ShouldRender()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return false;
			}
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() != _puzzleLocation.MapId)
			{
				return false;
			}
			return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			if (!ShouldRender() || _rings.Count == 0)
			{
				return;
			}
			GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				GraphicsDevice graphicsDevice = ((GraphicsDeviceContext)(ref graphicsDeviceContext)).get_GraphicsDevice();
				PlayerCamera camera = GameService.Gw2Mumble.get_PlayerCamera();
				Vector3 center = _puzzleLocation.AvatarPosition;
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
				foreach (ScoreRing ring in _rings.OrderByDescending((ScoreRing r) => r.RadiusMeters))
				{
					DrawGroundRing(graphicsDevice, center, ring);
				}
				graphicsDevice.set_BlendState(previousBlend);
				graphicsDevice.set_DepthStencilState(previousDepth);
				graphicsDevice.set_RasterizerState(previousRaster);
			}
			finally
			{
				((GraphicsDeviceContext)(ref graphicsDeviceContext)).Dispose();
			}
		}

		private void DrawGroundRing(GraphicsDevice graphicsDevice, Vector3 center, ScoreRing ring)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			if (ring.IsRainbow)
			{
				DrawGroundRainbowDisc(graphicsDevice, center, ring);
				return;
			}
			Color solidColor = ring.Color * 0.85f;
			for (int segment = 0; segment <= 72; segment++)
			{
				double angle = (double)segment * Math.PI * 2.0 / 72.0;
				_ringVertices[segment] = new VertexPositionColor(new Vector3(center.X + (float)((double)ring.RadiusMeters * Math.Cos(angle)), center.Y + (float)((double)ring.RadiusMeters * Math.Sin(angle)), center.Z), solidColor);
			}
			Enumerator enumerator = ((Effect)_lineEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, _ringVertices, 0, 72);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private void DrawGroundRainbowDisc(GraphicsDevice graphicsDevice, Vector3 center, ScoreRing ring)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			double hueOffset = _animationTime * 1.25;
			VertexPositionColor centerVertex = default(VertexPositionColor);
			((VertexPositionColor)(ref centerVertex))._002Ector(center, RainbowColors.ColorAtAngle(0.0, hueOffset) * 0.85f);
			for (int segment = 0; segment < 72; segment++)
			{
				double angle0 = (double)segment * Math.PI * 2.0 / 72.0;
				double angle1 = (double)(segment + 1) * Math.PI * 2.0 / 72.0;
				int baseIndex = segment * 3;
				_discVertices[baseIndex] = centerVertex;
				_discVertices[baseIndex + 1] = new VertexPositionColor(new Vector3(center.X + (float)((double)ring.RadiusMeters * Math.Cos(angle0)), center.Y + (float)((double)ring.RadiusMeters * Math.Sin(angle0)), center.Z), RainbowColors.ColorAtAngle(angle0, hueOffset) * 0.85f);
				_discVertices[baseIndex + 2] = new VertexPositionColor(new Vector3(center.X + (float)((double)ring.RadiusMeters * Math.Cos(angle1)), center.Y + (float)((double)ring.RadiusMeters * Math.Sin(angle1)), center.Z), RainbowColors.ColorAtAngle(angle1, hueOffset) * 0.85f);
			}
			Enumerator enumerator = ((Effect)_lineEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, _discVertices, 0, 72);
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

		internal static IReadOnlyList<ScoreRing> BuildScoreRings(IEnumerable<ScoreModel> scores)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			List<ScoreRing> rings = new List<ScoreRing>();
			foreach (ScoreModel score2 in scores.OrderBy((ScoreModel score) => score.Max))
			{
				if (!(score2.Max < 0.01f) && !(score2.Max > 250f))
				{
					rings.Add(new ScoreRing(score2.Max, score2.IsRainbow ? Color.get_White() : ScoreModel.ParseHexColor(score2.Color), score2.Value, score2.IsRainbow));
				}
			}
			return rings;
		}

		public void Dispose()
		{
			((Control)this).DisposeControl();
		}

		protected override void DisposeControl()
		{
			((Control)this).set_Parent((Container)null);
			((Control)this).DisposeControl();
		}
	}
}
