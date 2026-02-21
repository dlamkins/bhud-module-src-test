using System;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.VideoDisplays.Rendering
{
	public class WorldScreenRenderer : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<WorldScreenRenderer>();

		private const int VerticesPerQuad = 4;

		private const int QuadCount = 4;

		private const int SideVerticesCount = 16;

		private const byte MaxAlpha = byte.MaxValue;

		private static readonly Color InnerFrameSideColor = new Color(31, 31, 31);

		private static readonly Color InnerFrameTopBottomColor = new Color(35, 35, 36);

		private static readonly Vector2[] StandardTexCoords = (Vector2[])(object)new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};

		private readonly ScreenCornerCalculator _corners;

		private readonly short[] _quadIndices = new short[6] { 0, 1, 2, 0, 2, 3 };

		private VertexPositionColorTexture[] _quadVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _backVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _sideVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[16];

		private VertexPositionColorTexture[] _innerFrameVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[16];

		private VertexPositionColorTexture[] _logoVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _textVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _overlayVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private BasicEffect _basicEffect;

		private AsyncTexture2D _logoTexture;

		private AsyncTexture2D _logoTextTexture;

		private AsyncTexture2D _sideTexture;

		private AsyncTexture2D _topBottomTexture;

		private AsyncTexture2D _backTexture;

		private AsyncTexture2D _screenOffTexture;

		private bool _initialized;

		public WorldScreenRenderer(ScreenCornerCalculator corners)
		{
			_corners = corners;
		}

		public void Initialize(GraphicsDevice graphicsDevice)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			if (!_initialized)
			{
				try
				{
					BasicEffect val = new BasicEffect(graphicsDevice);
					val.set_TextureEnabled(true);
					val.set_VertexColorEnabled(true);
					val.set_LightingEnabled(false);
					val.set_FogEnabled(false);
					_basicEffect = val;
					_logoTexture = CinemaModule.Instance.TextureService.GetLogo();
					_logoTextTexture = CinemaModule.Instance.TextureService.GetLogoText();
					_sideTexture = CinemaModule.Instance.TextureService.GetTvSide();
					_topBottomTexture = CinemaModule.Instance.TextureService.GetTvTopBottom();
					_backTexture = CinemaModule.Instance.TextureService.GetTvBack();
					_screenOffTexture = CinemaModule.Instance.TextureService.GetTvScreenOff();
					_initialized = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to initialize WorldScreenRenderer");
				}
			}
		}

		public void Render(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix, Texture2D videoTexture, float opacity)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (!_initialized || !_corners.IsValid)
			{
				return;
			}
			(RasterizerState, SamplerState, BlendState, DepthStencilState) savedState = SaveGraphicsState(graphicsDevice);
			try
			{
				Color texturedColor = BuildVertices(opacity);
				SetRenderState(graphicsDevice);
				_basicEffect.set_World(Matrix.get_Identity());
				_basicEffect.set_View(viewMatrix);
				_basicEffect.set_Projection(projectionMatrix);
				RenderBackPanel(graphicsDevice);
				RenderBackLogo(graphicsDevice, texturedColor);
				RenderVideoQuad(graphicsDevice, videoTexture);
				RenderInnerFrame(graphicsDevice);
				RenderSidePanels(graphicsDevice);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Error during world screen rendering");
			}
			finally
			{
				RestoreGraphicsState(graphicsDevice, savedState);
			}
		}

		private Color BuildVertices(float opacity)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			byte alpha = CalculateAlpha(opacity);
			Color sideColorWithAlpha = ApplyAlpha(InnerFrameSideColor, alpha);
			Color topBottomColorWithAlpha = ApplyAlpha(InnerFrameTopBottomColor, alpha);
			Color texturedColor = CreateOpaqueColorWithAlpha(alpha);
			BuildQuadVertices(_quadVertices, _corners.WorldCorners, texturedColor);
			BuildBackVertices(texturedColor);
			BuildSideVertices(texturedColor);
			BuildInnerFrameVertices(sideColorWithAlpha, topBottomColorWithAlpha);
			return texturedColor;
		}

		private static byte CalculateAlpha(float opacity)
		{
			return (byte)(Math.Max(0f, Math.Min(1f, opacity)) * 255f);
		}

		private static Color ApplyAlpha(Color color, byte alpha)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Color(((Color)(ref color)).get_R(), ((Color)(ref color)).get_G(), ((Color)(ref color)).get_B(), alpha);
		}

		private static Color CreateOpaqueColorWithAlpha(byte alpha)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, alpha);
		}

		private void BuildQuadVertices(VertexPositionColorTexture[] vertices, Vector3[] corners, Color color)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < 4; i++)
			{
				vertices[i] = new VertexPositionColorTexture(corners[i], color, StandardTexCoords[i]);
			}
		}

		private void BuildBackVertices(Color color)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			SetQuadVertex(ref _backVertices[0], _corners.BackCorners[0], color, 0);
			SetQuadVertex(ref _backVertices[1], _corners.BackCorners[3], color, 3);
			SetQuadVertex(ref _backVertices[2], _corners.BackCorners[2], color, 2);
			SetQuadVertex(ref _backVertices[3], _corners.BackCorners[1], color, 1);
		}

		private static void SetQuadVertex(ref VertexPositionColorTexture vertex, Vector3 position, Color color, int texCoordIndex)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			vertex = new VertexPositionColorTexture(position, color, StandardTexCoords[texCoordIndex]);
		}

		private void BuildSideVertices(Color color)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			BuildSideQuad(0, _corners.BorderCorners[0], _corners.BorderCorners[3], _corners.BackCorners[3], _corners.BackCorners[0], color);
			BuildSideQuad(4, _corners.BorderCorners[1], _corners.BackCorners[1], _corners.BackCorners[2], _corners.BorderCorners[2], color);
			BuildSideQuad(8, _corners.BorderCorners[0], _corners.BackCorners[0], _corners.BackCorners[1], _corners.BorderCorners[1], color);
			BuildSideQuad(12, _corners.BorderCorners[3], _corners.BorderCorners[2], _corners.BackCorners[2], _corners.BackCorners[3], color);
		}

		private void BuildSideQuad(int startIndex, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			SetQuadVertex(ref _sideVertices[startIndex], v0, color, 0);
			SetQuadVertex(ref _sideVertices[startIndex + 1], v1, color, 3);
			SetQuadVertex(ref _sideVertices[startIndex + 2], v2, color, 2);
			SetQuadVertex(ref _sideVertices[startIndex + 3], v3, color, 1);
		}

		private void BuildInnerFrameVertices(Color sideColor, Color topBottomColor)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			BuildInnerFrameQuad(0, _corners.BorderCorners[0], _corners.BorderCorners[1], _corners.InnerFrameCorners[1], _corners.InnerFrameCorners[0], topBottomColor);
			BuildInnerFrameQuad(4, _corners.InnerFrameCorners[3], _corners.InnerFrameCorners[2], _corners.BorderCorners[2], _corners.BorderCorners[3], topBottomColor);
			BuildInnerFrameQuad(8, _corners.BorderCorners[0], _corners.InnerFrameCorners[0], _corners.InnerFrameCorners[3], _corners.BorderCorners[3], sideColor);
			BuildInnerFrameQuad(12, _corners.InnerFrameCorners[1], _corners.BorderCorners[1], _corners.BorderCorners[2], _corners.InnerFrameCorners[2], sideColor);
		}

		private void BuildInnerFrameQuad(int startIndex, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color color)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			SetQuadVertex(ref _innerFrameVertices[startIndex], v0, color, 0);
			SetQuadVertex(ref _innerFrameVertices[startIndex + 1], v1, color, 1);
			SetQuadVertex(ref _innerFrameVertices[startIndex + 2], v2, color, 2);
			SetQuadVertex(ref _innerFrameVertices[startIndex + 3], v3, color, 3);
		}

		private void RenderBackPanel(GraphicsDevice graphicsDevice)
		{
			_basicEffect.set_Texture(GetTextureOrFallback(_backTexture));
			ApplyEffectAndDrawQuad(graphicsDevice, _backVertices, 0);
		}

		private void RenderSidePanels(GraphicsDevice graphicsDevice)
		{
			Texture2D sideTex = GetTextureOrFallback(_sideTexture);
			Texture2D topBottomTex = GetTextureOrFallback(_topBottomTexture);
			for (int sideIndex = 0; sideIndex < 4; sideIndex++)
			{
				_basicEffect.set_Texture((sideIndex < 2) ? sideTex : topBottomTex);
				ApplyEffectAndDrawQuad(graphicsDevice, _sideVertices, sideIndex * 4);
			}
		}

		private void RenderInnerFrame(GraphicsDevice graphicsDevice)
		{
			_basicEffect.set_Texture(CinemaModule.Instance.TextureService.GetWhitePixel());
			ApplyEffect();
			for (int bevelIndex = 0; bevelIndex < 4; bevelIndex++)
			{
				DrawIndexedQuad(graphicsDevice, _innerFrameVertices, bevelIndex * 4);
			}
		}

		private void RenderBackLogo(GraphicsDevice graphicsDevice, Color color)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RenderLogoTexture(graphicsDevice, color);
			RenderTextTexture(graphicsDevice, color);
		}

		private void RenderLogoTexture(GraphicsDevice graphicsDevice, Color color)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D logoTexture = _logoTexture;
			Texture2D logoTex = ((logoTexture != null) ? logoTexture.get_Texture() : null);
			if (CinemaModule.Instance.TextureService.IsTextureReady(logoTex))
			{
				SetQuadVertex(ref _logoVertices[0], _corners.LogoCorners[0], color, 0);
				SetQuadVertex(ref _logoVertices[1], _corners.LogoCorners[3], color, 3);
				SetQuadVertex(ref _logoVertices[2], _corners.LogoCorners[2], color, 2);
				SetQuadVertex(ref _logoVertices[3], _corners.LogoCorners[1], color, 1);
				_basicEffect.set_Texture(logoTex);
				ApplyEffectAndDrawQuad(graphicsDevice, _logoVertices, 0);
			}
		}

		private void RenderTextTexture(GraphicsDevice graphicsDevice, Color color)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D logoTextTexture = _logoTextTexture;
			Texture2D textTex = ((logoTextTexture != null) ? logoTextTexture.get_Texture() : null);
			if (CinemaModule.Instance.TextureService.IsTextureReady(textTex))
			{
				SetQuadVertex(ref _textVertices[0], _corners.TextCorners[0], color, 1);
				SetQuadVertex(ref _textVertices[1], _corners.TextCorners[3], color, 2);
				SetQuadVertex(ref _textVertices[2], _corners.TextCorners[2], color, 3);
				SetQuadVertex(ref _textVertices[3], _corners.TextCorners[1], color, 0);
				_basicEffect.set_Texture(textTex);
				ApplyEffectAndDrawQuad(graphicsDevice, _textVertices, 0);
			}
		}

		private void RenderVideoQuad(GraphicsDevice graphicsDevice, Texture2D videoTexture)
		{
			_basicEffect.set_Texture(CinemaModule.Instance.TextureService.IsTextureReady(videoTexture) ? videoTexture : GetTextureOrFallback(_screenOffTexture));
			ApplyEffectAndDrawQuad(graphicsDevice, _quadVertices, 0);
		}

		public void RenderOverlay(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix, Texture2D overlayTexture, float opacity)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			if (!_initialized || !_corners.IsValid || !CinemaModule.Instance.TextureService.IsTextureReady(overlayTexture))
			{
				return;
			}
			(RasterizerState, SamplerState, BlendState, DepthStencilState) savedState = SaveGraphicsState(graphicsDevice);
			try
			{
				SetRenderState(graphicsDevice);
				_basicEffect.set_World(Matrix.get_Identity());
				_basicEffect.set_View(viewMatrix);
				_basicEffect.set_Projection(projectionMatrix);
				Color color = CreateOpaqueColorWithAlpha(CalculateAlpha(opacity));
				Vector3[] corners = _corners.WorldCorners;
				Vector3 right = corners[1] - corners[0];
				Vector3 down = corners[3] - corners[0];
				Vector3 overlayOffset = Vector3.Normalize(Vector3.Cross(down, right)) * 0.02f;
				float textureAspect = (float)overlayTexture.get_Width() / (float)overlayTexture.get_Height();
				float quadWidth = ((Vector3)(ref right)).Length();
				float quadHeight = ((Vector3)(ref down)).Length();
				float boxLeftRatio = 0.42f;
				float boxRightRatio = 0.92f;
				float boxTopRatio = 0.07f;
				float boxBottomRatio = 0.27f;
				float num = quadWidth * (boxRightRatio - boxLeftRatio);
				float boxHeight = quadHeight * (boxBottomRatio - boxTopRatio);
				float fitWidth = num;
				float fitHeight = fitWidth / textureAspect;
				if (fitHeight > boxHeight)
				{
					fitHeight = boxHeight;
					fitWidth = fitHeight * textureAspect;
				}
				float boxCenterX = boxLeftRatio + (boxRightRatio - boxLeftRatio) / 2f;
				float boxCenterY = boxTopRatio + (boxBottomRatio - boxTopRatio) / 2f;
				Vector3 rightNorm = Vector3.Normalize(right);
				Vector3 downNorm = Vector3.Normalize(down);
				Vector3 boxCenter = corners[0] + rightNorm * (quadWidth * boxCenterX) + downNorm * (quadHeight * boxCenterY) + overlayOffset;
				Vector3 halfRight = rightNorm * (fitWidth / 2f);
				Vector3 halfDown = downNorm * (fitHeight / 2f);
				_overlayVertices[0] = new VertexPositionColorTexture(boxCenter - halfRight - halfDown, color, new Vector2(0f, 0f));
				_overlayVertices[1] = new VertexPositionColorTexture(boxCenter + halfRight - halfDown, color, new Vector2(1f, 0f));
				_overlayVertices[2] = new VertexPositionColorTexture(boxCenter + halfRight + halfDown, color, new Vector2(1f, 1f));
				_overlayVertices[3] = new VertexPositionColorTexture(boxCenter - halfRight + halfDown, color, new Vector2(0f, 1f));
				_basicEffect.set_Texture(overlayTexture);
				ApplyEffectAndDrawQuad(graphicsDevice, _overlayVertices, 0);
			}
			finally
			{
				RestoreGraphicsState(graphicsDevice, savedState);
			}
		}

		private void DrawIndexedQuad(GraphicsDevice graphicsDevice, VertexPositionColorTexture[] vertices, int vertexOffset)
		{
			graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>((PrimitiveType)0, vertices, vertexOffset, 4, _quadIndices, 0, 2);
		}

		private Texture2D GetTextureOrFallback(AsyncTexture2D asyncTexture)
		{
			return ((asyncTexture != null) ? asyncTexture.get_Texture() : null) ?? CinemaModule.Instance.TextureService.GetWhitePixel();
		}

		private void ApplyEffect()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator enumerator = ((Effect)_basicEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private void ApplyEffectAndDrawQuad(GraphicsDevice graphicsDevice, VertexPositionColorTexture[] vertices, int vertexOffset)
		{
			ApplyEffect();
			DrawIndexedQuad(graphicsDevice, vertices, vertexOffset);
		}

		private (RasterizerState rasterizer, SamplerState sampler, BlendState blend, DepthStencilState depth) SaveGraphicsState(GraphicsDevice graphicsDevice)
		{
			return (graphicsDevice.get_RasterizerState(), graphicsDevice.get_SamplerStates().get_Item(0), graphicsDevice.get_BlendState(), graphicsDevice.get_DepthStencilState());
		}

		private void SetRenderState(GraphicsDevice graphicsDevice)
		{
			graphicsDevice.set_RasterizerState(RasterizerState.CullNone);
			graphicsDevice.get_SamplerStates().set_Item(0, SamplerState.LinearClamp);
			graphicsDevice.set_BlendState(BlendState.AlphaBlend);
			graphicsDevice.set_DepthStencilState(DepthStencilState.Default);
		}

		private void RestoreGraphicsState(GraphicsDevice graphicsDevice, (RasterizerState rasterizer, SamplerState sampler, BlendState blend, DepthStencilState depth) state)
		{
			graphicsDevice.set_RasterizerState(state.rasterizer);
			graphicsDevice.get_SamplerStates().set_Item(0, state.sampler);
			graphicsDevice.set_BlendState(state.blend);
			graphicsDevice.set_DepthStencilState(state.depth);
		}

		public void Dispose()
		{
			BasicEffect basicEffect = _basicEffect;
			if (basicEffect != null)
			{
				((GraphicsResource)basicEffect).Dispose();
			}
			_basicEffect = null;
			_initialized = false;
		}
	}
}
