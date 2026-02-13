using System;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Displays.Rendering
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

		private readonly ScreenCornerCalculator _corners;

		private readonly short[] _quadIndices = new short[6] { 0, 1, 2, 0, 2, 3 };

		private VertexPositionColorTexture[] _quadVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _backVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _sideVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[16];

		private VertexPositionColorTexture[] _innerFrameVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[16];

		private VertexPositionColorTexture[] _logoVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

		private VertexPositionColorTexture[] _textVertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[4];

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
			_corners = corners ?? throw new ArgumentNullException("corners");
		}

		public void Initialize(GraphicsDevice graphicsDevice)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			if (!_initialized && graphicsDevice != null)
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
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (!_initialized || !_corners.IsValid || graphicsDevice == null)
			{
				return;
			}
			(RasterizerState, SamplerState, BlendState, DepthStencilState) savedState = SaveGraphicsState(graphicsDevice);
			try
			{
				BuildVertices(opacity);
				SetRenderState(graphicsDevice);
				_basicEffect.set_World(Matrix.get_Identity());
				_basicEffect.set_View(viewMatrix);
				_basicEffect.set_Projection(projectionMatrix);
				RenderBackPanel(graphicsDevice);
				RenderBackLogo(graphicsDevice, opacity);
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

		private void BuildVertices(float opacity)
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
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			byte alpha = CalculateAlpha(opacity);
			Color sideColorWithAlpha = ApplyAlpha(InnerFrameSideColor, alpha);
			Color topBottomColorWithAlpha = ApplyAlpha(InnerFrameTopBottomColor, alpha);
			Color texturedColor = CreateOpaqueColorWithAlpha(alpha);
			Vector2[] texCoords = (Vector2[])(object)new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 1f)
			};
			BuildQuadVertices(_quadVertices, _corners.WorldCorners, texturedColor, texCoords);
			BuildBackVertices(texturedColor, texCoords);
			BuildSideVertices(texturedColor, texturedColor);
			BuildInnerFrameVertices(sideColorWithAlpha, topBottomColorWithAlpha);
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

		private void BuildQuadVertices(VertexPositionColorTexture[] vertices, Vector3[] corners, Color color, Vector2[] texCoords)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < 4; i++)
			{
				vertices[i] = new VertexPositionColorTexture(corners[i], color, texCoords[i]);
			}
		}

		private void BuildBackVertices(Color color, Vector2[] texCoords)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			_backVertices[0] = new VertexPositionColorTexture(_corners.BackCorners[0], color, texCoords[0]);
			_backVertices[1] = new VertexPositionColorTexture(_corners.BackCorners[3], color, texCoords[3]);
			_backVertices[2] = new VertexPositionColorTexture(_corners.BackCorners[2], color, texCoords[2]);
			_backVertices[3] = new VertexPositionColorTexture(_corners.BackCorners[1], color, texCoords[1]);
		}

		private void BuildSideVertices(Color sideColor, Color topBottomColor)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			_sideVertices[0] = new VertexPositionColorTexture(_corners.BorderCorners[0], sideColor, new Vector2(0f, 0f));
			_sideVertices[1] = new VertexPositionColorTexture(_corners.BorderCorners[3], sideColor, new Vector2(0f, 1f));
			_sideVertices[2] = new VertexPositionColorTexture(_corners.BackCorners[3], sideColor, new Vector2(1f, 1f));
			_sideVertices[3] = new VertexPositionColorTexture(_corners.BackCorners[0], sideColor, new Vector2(1f, 0f));
			_sideVertices[4] = new VertexPositionColorTexture(_corners.BorderCorners[1], sideColor, new Vector2(0f, 0f));
			_sideVertices[5] = new VertexPositionColorTexture(_corners.BackCorners[1], sideColor, new Vector2(1f, 0f));
			_sideVertices[6] = new VertexPositionColorTexture(_corners.BackCorners[2], sideColor, new Vector2(1f, 1f));
			_sideVertices[7] = new VertexPositionColorTexture(_corners.BorderCorners[2], sideColor, new Vector2(0f, 1f));
			_sideVertices[8] = new VertexPositionColorTexture(_corners.BorderCorners[0], topBottomColor, new Vector2(0f, 0f));
			_sideVertices[9] = new VertexPositionColorTexture(_corners.BackCorners[0], topBottomColor, new Vector2(0f, 1f));
			_sideVertices[10] = new VertexPositionColorTexture(_corners.BackCorners[1], topBottomColor, new Vector2(1f, 1f));
			_sideVertices[11] = new VertexPositionColorTexture(_corners.BorderCorners[1], topBottomColor, new Vector2(1f, 0f));
			_sideVertices[12] = new VertexPositionColorTexture(_corners.BorderCorners[3], topBottomColor, new Vector2(0f, 0f));
			_sideVertices[13] = new VertexPositionColorTexture(_corners.BorderCorners[2], topBottomColor, new Vector2(1f, 0f));
			_sideVertices[14] = new VertexPositionColorTexture(_corners.BackCorners[2], topBottomColor, new Vector2(1f, 1f));
			_sideVertices[15] = new VertexPositionColorTexture(_corners.BackCorners[3], topBottomColor, new Vector2(0f, 1f));
		}

		private void BuildInnerFrameVertices(Color sideColor, Color topBottomColor)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			_innerFrameVertices[0] = new VertexPositionColorTexture(_corners.BorderCorners[0], topBottomColor, new Vector2(0f, 0f));
			_innerFrameVertices[1] = new VertexPositionColorTexture(_corners.BorderCorners[1], topBottomColor, new Vector2(1f, 0f));
			_innerFrameVertices[2] = new VertexPositionColorTexture(_corners.InnerFrameCorners[1], topBottomColor, new Vector2(1f, 1f));
			_innerFrameVertices[3] = new VertexPositionColorTexture(_corners.InnerFrameCorners[0], topBottomColor, new Vector2(0f, 1f));
			_innerFrameVertices[4] = new VertexPositionColorTexture(_corners.InnerFrameCorners[3], topBottomColor, new Vector2(0f, 0f));
			_innerFrameVertices[5] = new VertexPositionColorTexture(_corners.InnerFrameCorners[2], topBottomColor, new Vector2(1f, 0f));
			_innerFrameVertices[6] = new VertexPositionColorTexture(_corners.BorderCorners[2], topBottomColor, new Vector2(1f, 1f));
			_innerFrameVertices[7] = new VertexPositionColorTexture(_corners.BorderCorners[3], topBottomColor, new Vector2(0f, 1f));
			_innerFrameVertices[8] = new VertexPositionColorTexture(_corners.BorderCorners[0], sideColor, new Vector2(0f, 0f));
			_innerFrameVertices[9] = new VertexPositionColorTexture(_corners.InnerFrameCorners[0], sideColor, new Vector2(1f, 0f));
			_innerFrameVertices[10] = new VertexPositionColorTexture(_corners.InnerFrameCorners[3], sideColor, new Vector2(1f, 1f));
			_innerFrameVertices[11] = new VertexPositionColorTexture(_corners.BorderCorners[3], sideColor, new Vector2(0f, 1f));
			_innerFrameVertices[12] = new VertexPositionColorTexture(_corners.InnerFrameCorners[1], sideColor, new Vector2(0f, 0f));
			_innerFrameVertices[13] = new VertexPositionColorTexture(_corners.BorderCorners[1], sideColor, new Vector2(1f, 0f));
			_innerFrameVertices[14] = new VertexPositionColorTexture(_corners.BorderCorners[2], sideColor, new Vector2(1f, 1f));
			_innerFrameVertices[15] = new VertexPositionColorTexture(_corners.InnerFrameCorners[2], sideColor, new Vector2(0f, 1f));
		}

		private void RenderBackPanel(GraphicsDevice graphicsDevice)
		{
			_basicEffect.set_Texture(GetTextureOrFallback(_backTexture));
			ApplyEffectAndDraw(graphicsDevice, delegate
			{
				DrawQuad(graphicsDevice, _backVertices);
			});
		}

		private void RenderSidePanels(GraphicsDevice graphicsDevice)
		{
			Texture2D sideTex = GetTextureOrFallback(_sideTexture);
			Texture2D topBottomTex = GetTextureOrFallback(_topBottomTexture);
			for (int sideIndex = 0; sideIndex < 4; sideIndex++)
			{
				bool isLeftOrRight = sideIndex < 2;
				_basicEffect.set_Texture(isLeftOrRight ? sideTex : topBottomTex);
				int vertexOffset = sideIndex * 4;
				ApplyEffectAndDraw(graphicsDevice, delegate
				{
					DrawIndexedQuad(graphicsDevice, _sideVertices, vertexOffset);
				});
			}
		}

		private void RenderInnerFrame(GraphicsDevice graphicsDevice)
		{
			_basicEffect.set_Texture(CinemaModule.Instance.TextureService.GetWhitePixel());
			ApplyEffectAndDraw(graphicsDevice, delegate
			{
				for (int i = 0; i < 4; i++)
				{
					int vertexOffset = i * 4;
					DrawIndexedQuad(graphicsDevice, _innerFrameVertices, vertexOffset);
				}
			});
		}

		private void RenderBackLogo(GraphicsDevice graphicsDevice, float opacity)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Color videoColor = CreateOpaqueColorWithAlpha(CalculateAlpha(opacity));
			RenderLogoTexture(graphicsDevice, videoColor);
			RenderTextTexture(graphicsDevice, videoColor);
		}

		private void RenderLogoTexture(GraphicsDevice graphicsDevice, Color color)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D logoTexture = _logoTexture;
			Texture2D logoTex = ((logoTexture != null) ? logoTexture.get_Texture() : null);
			if (CinemaModule.Instance.TextureService.IsTextureReady(logoTex))
			{
				_logoVertices[0] = new VertexPositionColorTexture(_corners.LogoCorners[0], color, new Vector2(0f, 0f));
				_logoVertices[1] = new VertexPositionColorTexture(_corners.LogoCorners[3], color, new Vector2(0f, 1f));
				_logoVertices[2] = new VertexPositionColorTexture(_corners.LogoCorners[2], color, new Vector2(1f, 1f));
				_logoVertices[3] = new VertexPositionColorTexture(_corners.LogoCorners[1], color, new Vector2(1f, 0f));
				_basicEffect.set_Texture(logoTex);
				ApplyEffectAndDraw(graphicsDevice, delegate
				{
					DrawQuad(graphicsDevice, _logoVertices);
				});
			}
		}

		private void RenderTextTexture(GraphicsDevice graphicsDevice, Color color)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D logoTextTexture = _logoTextTexture;
			Texture2D textTex = ((logoTextTexture != null) ? logoTextTexture.get_Texture() : null);
			if (CinemaModule.Instance.TextureService.IsTextureReady(textTex))
			{
				_textVertices[0] = new VertexPositionColorTexture(_corners.TextCorners[0], color, new Vector2(1f, 0f));
				_textVertices[1] = new VertexPositionColorTexture(_corners.TextCorners[3], color, new Vector2(1f, 1f));
				_textVertices[2] = new VertexPositionColorTexture(_corners.TextCorners[2], color, new Vector2(0f, 1f));
				_textVertices[3] = new VertexPositionColorTexture(_corners.TextCorners[1], color, new Vector2(0f, 0f));
				_basicEffect.set_Texture(textTex);
				ApplyEffectAndDraw(graphicsDevice, delegate
				{
					DrawQuad(graphicsDevice, _textVertices);
				});
			}
		}

		private void RenderVideoQuad(GraphicsDevice graphicsDevice, Texture2D videoTexture)
		{
			_basicEffect.set_Texture(CinemaModule.Instance.TextureService.IsTextureReady(videoTexture) ? videoTexture : GetTextureOrFallback(_screenOffTexture));
			ApplyEffectAndDraw(graphicsDevice, delegate
			{
				DrawQuad(graphicsDevice, _quadVertices);
			});
		}

		private void DrawQuad(GraphicsDevice graphicsDevice, VertexPositionColorTexture[] vertices)
		{
			DrawIndexedQuad(graphicsDevice, vertices, 0);
		}

		private void DrawIndexedQuad(GraphicsDevice graphicsDevice, VertexPositionColorTexture[] vertices, int vertexOffset)
		{
			graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>((PrimitiveType)0, vertices, vertexOffset, 4, _quadIndices, 0, 2);
		}

		private Texture2D GetTextureOrFallback(AsyncTexture2D asyncTexture)
		{
			return ((asyncTexture != null) ? asyncTexture.get_Texture() : null) ?? CinemaModule.Instance.TextureService.GetWhitePixel();
		}

		private void ApplyEffectAndDraw(GraphicsDevice graphicsDevice, Action drawAction)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Enumerator enumerator = ((Effect)_basicEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					drawAction();
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
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
