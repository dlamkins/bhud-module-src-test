using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldTexture : WorldEntity
	{
		private static DynamicVertexBuffer _sharedVertexBuffer;

		private static readonly Vector3[] _faceVerts = (Vector3[])(object)new Vector3[4]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f)
		};

		private readonly AsyncTexture2D _asyncTexture;

		public int ResizeWidth { get; set; } = -1;


		public int ResizeHeight { get; set; } = -1;


		public WorldTexture(AsyncTexture2D asyncTexture, Vector3 position, float scale)
			: base(position, scale)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
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

		private RenderTarget2D CreateTexture(GraphicsDevice graphicsDevice)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
			try
			{
				Texture2D texture = _asyncTexture.get_Texture();
				bool doResize = ResizeWidth != -1 || ResizeHeight != -1;
				Texture2D resizedTexture = ((!doResize) ? texture : ImageUtil.ResizeImage(texture.ToImage(), (ResizeWidth == -1) ? texture.get_Width() : ResizeWidth, (ResizeHeight == -1) ? texture.get_Height() : ResizeHeight).ToTexture2D(((GraphicsResource)spriteBatch).get_GraphicsDevice()));
				RenderTarget2D target = new RenderTarget2D(((GraphicsResource)spriteBatch).get_GraphicsDevice(), resizedTexture.get_Width(), resizedTexture.get_Height(), false, graphicsDevice.get_PresentationParameters().get_BackBufferFormat(), graphicsDevice.get_PresentationParameters().get_DepthStencilFormat(), 1, (RenderTargetUsage)1);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().get_PresentationParameters().set_RenderTargetUsage((RenderTargetUsage)1);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget(target);
				try
				{
					spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
					((GraphicsResource)spriteBatch).get_GraphicsDevice().Clear(Color.get_Transparent());
					spriteBatch.Draw(resizedTexture, Vector2.get_Zero(), Color.get_White());
					spriteBatch.End();
				}
				catch (Exception)
				{
				}
				((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
				if (doResize)
				{
					((GraphicsResource)resizedTexture).Dispose();
				}
				return target;
			}
			finally
			{
				((IDisposable)spriteBatch)?.Dispose();
			}
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
			try
			{
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
			finally
			{
				((IDisposable)renderTarget2D)?.Dispose();
			}
		}
	}
}
