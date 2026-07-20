using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldTexture : WorldEntity
	{
		private static DynamicVertexBuffer _sharedVertexBuffer;

		private static readonly Vector3[] _faceVerts = (Vector3[])(object)new Vector3[4]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, 1f, 0f)
		};

		private readonly AsyncTexture2D _asyncTexture;

		private RenderTarget2D? _renderTarget;

		private SpriteBatch _spriteBatch;

		private int _lastTextureHashcode = -1;

		public int ResizeWidth { get; set; } = -1;


		public int ResizeHeight { get; set; } = -1;


		public WorldTexture(AsyncTexture2D asyncTexture, Vector3 position, float scale)
			: base(position, scale)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			_asyncTexture = asyncTexture;
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

		private RenderTarget2D CreateTexture(GraphicsDevice graphicsDevice)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			if (_spriteBatch == null)
			{
				_spriteBatch = new SpriteBatch(graphicsDevice);
			}
			Texture2D texture = _asyncTexture.get_Texture();
			bool doResize = ResizeWidth != -1 || ResizeHeight != -1;
			Texture2D resizedTexture = ((!doResize) ? texture : ImageUtil.ResizeImage(texture.ToImage(), (ResizeWidth == -1) ? texture.get_Width() : ResizeWidth, (ResizeHeight == -1) ? texture.get_Height() : ResizeHeight).ToTexture2D(((GraphicsResource)_spriteBatch).get_GraphicsDevice()));
			Size textureSize = default(Size);
			((Size)(ref textureSize))._002Ector(resizedTexture.get_Width(), resizedTexture.get_Height());
			textureSize = PadSizesToPowerOfTwo(textureSize);
			if (NeedsRenderTargetRecreate(_renderTarget, textureSize))
			{
				CreateRenderTarget(graphicsDevice, textureSize);
				_lastTextureHashcode = -1;
			}
			int hashcode = ((object)resizedTexture).GetHashCode();
			if (hashcode == _lastTextureHashcode)
			{
				return _renderTarget;
			}
			_lastTextureHashcode = hashcode;
			((GraphicsResource)_spriteBatch).get_GraphicsDevice().get_PresentationParameters().set_RenderTargetUsage((RenderTargetUsage)1);
			((GraphicsResource)_spriteBatch).get_GraphicsDevice().SetRenderTarget(_renderTarget);
			try
			{
				_spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
				((GraphicsResource)_spriteBatch).get_GraphicsDevice().Clear(Color.get_Transparent());
				_spriteBatch.Draw(resizedTexture, Vector2.get_Zero(), Color.get_White());
				_spriteBatch.End();
			}
			catch (Exception)
			{
			}
			((GraphicsResource)_spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
			if (doResize)
			{
				((GraphicsResource)resizedTexture).Dispose();
			}
			return _renderTarget;
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			RenderTarget2D renderTarget2D = CreateTexture(graphicsDevice);
			Matrix modelMatrix = GetMatrix(graphicsDevice, world, camera);
			base.RenderEffect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			base.RenderEffect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			base.RenderEffect.set_World(modelMatrix);
			base.RenderEffect.set_Texture((Texture2D)(object)renderTarget2D);
			base.RenderEffect.set_TextureEnabled(true);
			base.RenderEffect.set_VertexColorEnabled(false);
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
			SpriteBatch spriteBatch = _spriteBatch;
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
