using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldText : WorldEntity
	{
		private readonly Func<string> _getText;

		private readonly BitmapFont _font;

		private readonly Func<Color> _getColor;

		private static DynamicVertexBuffer _sharedVertexBuffer;

		private static readonly Vector3[] _faceVerts = (Vector3[])(object)new Vector3[4]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, 1f, 0f)
		};

		private string _lastText;

		private SpriteBatch? _spriteBatch;

		private RenderTarget2D? _renderTarget;

		public Func<Size>? TextureSizeCallback { get; set; }

		public bool UseTextSizeAsMinimum { get; set; }

		protected Effect? Effect { get; set; }

		public WorldText(Func<string> getText, BitmapFont font, Vector3 position, float scale, Func<Color> getColor)
			: base(position, scale)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_getText = getText;
			_font = font;
			_getColor = getColor;
			CreateSharedVertexBuffer();
		}

		private void CreateSharedVertexBuffer()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext gdctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				_sharedVertexBuffer = new DynamicVertexBuffer(((GraphicsDeviceContext)(ref gdctx)).get_GraphicsDevice(), typeof(VertexPositionTexture), 4, (BufferUsage)1);
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdctx)).Dispose();
			}
			VertexPositionTexture[] verts = (VertexPositionTexture[])(object)new VertexPositionTexture[_faceVerts.Length];
			for (int i = 0; i < _faceVerts.Length; i++)
			{
				ref Vector3 vert = ref _faceVerts[i];
				verts[i] = new VertexPositionTexture(vert, new Vector2((float)((vert.X < 0f) ? 1 : 0), (float)((vert.Y < 0f) ? 1 : 0)));
			}
			((VertexBuffer)_sharedVertexBuffer).SetData<VertexPositionTexture>(verts);
		}

		public override bool IsPlayerInside(bool includeZAxis = true)
		{
			return false;
		}

		private void CreateRenderTarget(GraphicsDevice graphicsDevice, Size textureSize)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			RenderTarget2D? renderTarget = _renderTarget;
			if (renderTarget != null)
			{
				((GraphicsResource)renderTarget).Dispose();
			}
			_renderTarget = new RenderTarget2D(graphicsDevice, textureSize.Width, textureSize.Height, false, graphicsDevice.get_PresentationParameters().get_BackBufferFormat(), graphicsDevice.get_PresentationParameters().get_DepthStencilFormat(), 1, (RenderTargetUsage)1);
		}

		private static bool NeedsRenderTargetRecreate(RenderTarget2D renderTarget, Size textureSize)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (renderTarget != null && ((Texture2D)renderTarget).get_Width() == textureSize.Width)
			{
				return ((Texture2D)renderTarget).get_Height() != textureSize.Height;
			}
			return true;
		}

		private RenderTarget2D CreateTexture(GraphicsDevice graphicsDevice, string text, Size textureSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			if (NeedsRenderTargetRecreate(_renderTarget, textureSize))
			{
				CreateRenderTarget(graphicsDevice, textureSize);
				_lastText = null;
			}
			if (text == _lastText)
			{
				return _renderTarget;
			}
			_lastText = text;
			if (_spriteBatch == null)
			{
				_spriteBatch = new SpriteBatch(graphicsDevice);
			}
			((GraphicsResource)_spriteBatch).get_GraphicsDevice().get_PresentationParameters().set_RenderTargetUsage((RenderTargetUsage)1);
			((GraphicsResource)_spriteBatch).get_GraphicsDevice().SetRenderTarget(_renderTarget);
			try
			{
				_spriteBatch!.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, Effect, (Matrix?)null);
				((GraphicsResource)_spriteBatch).get_GraphicsDevice().Clear(Color.get_Transparent());
				_spriteBatch.DrawString(text, _font, new RectangleF(0f, 0f, (float)((Texture2D)_renderTarget).get_Width(), (float)((Texture2D)_renderTarget).get_Height()), _getColor(), wrap: false, stroke: false, 1, 1f, (HorizontalAlignment)1, (VerticalAlignment)1);
				_spriteBatch!.End();
			}
			catch (Exception)
			{
			}
			((GraphicsResource)_spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
			return _renderTarget;
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			string text = _getText();
			Size2 textSizes = _font.MeasureString(text);
			Size textureSizes = default(Size);
			((Size)(ref textureSizes))._002Ector((int)Math.Ceiling(textSizes.Width), (int)Math.Ceiling(textSizes.Height));
			if (TextureSizeCallback != null)
			{
				Size size = TextureSizeCallback!();
				textureSizes.Width = ((!UseTextSizeAsMinimum) ? size.Width : Math.Max(textureSizes.Width, size.Width));
				textureSizes.Height = ((!UseTextSizeAsMinimum) ? size.Height : Math.Max(textureSizes.Height, size.Height));
			}
			textureSizes = PadSizesToPowerOfTwo(textureSizes);
			RenderTarget2D renderTarget2D = CreateTexture(graphicsDevice, text, textureSizes);
			Matrix modelMatrix = GetMatrix(graphicsDevice, world, camera);
			base.RenderEffect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			base.RenderEffect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			base.RenderEffect.set_World(modelMatrix);
			base.RenderEffect.set_Texture((Texture2D)(object)renderTarget2D);
			base.RenderEffect.set_TextureEnabled(true);
			base.RenderEffect.set_VertexColorEnabled(false);
			graphicsDevice.set_RasterizerState(RasterizerState.CullCounterClockwise);
			graphicsDevice.SetVertexBuffer((VertexBuffer)(object)_sharedVertexBuffer);
			Enumerator enumerator = ((Effect)base.RenderEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawPrimitives((PrimitiveType)1, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private static Size PadSizesToPowerOfTwo(Size size)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			while (size.Height % 2 != 0)
			{
				size.Height++;
			}
			while (size.Width % 2 != 0)
			{
				size.Width++;
			}
			return size;
		}

		public override void Dispose()
		{
			SpriteBatch? spriteBatch = _spriteBatch;
			if (spriteBatch != null)
			{
				((GraphicsResource)spriteBatch).Dispose();
			}
			RenderTarget2D? renderTarget = _renderTarget;
			if (renderTarget != null)
			{
				((GraphicsResource)renderTarget).Dispose();
			}
			base.Dispose();
		}
	}
}
