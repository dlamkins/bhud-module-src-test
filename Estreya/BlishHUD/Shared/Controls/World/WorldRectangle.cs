using System;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldRectangle : WorldEntity
	{
		private readonly Color _color;

		private static DynamicVertexBuffer _sharedVertexBuffer;

		private static readonly Vector3[] _faceVerts = (Vector3[])(object)new Vector3[4]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f)
		};

		public WorldRectangle(Vector3 position, Color color, float scale = 1f)
			: base(position, scale)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			_color = color;
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

		private RenderTarget2D CreateTexture(SpriteBatch spriteBatch)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture = Textures.get_Pixel();
			RenderTarget2D target = new RenderTarget2D(((GraphicsResource)spriteBatch).get_GraphicsDevice(), texture.get_Width(), texture.get_Height());
			((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget(target);
			try
			{
				spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().Clear(_color);
				spriteBatch.End();
			}
			catch (Exception)
			{
			}
			((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
			return target;
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
			RenderTarget2D renderTarget2D = CreateTexture(spriteBatch);
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
			((GraphicsResource)renderTarget2D).Dispose();
			((GraphicsResource)spriteBatch).Dispose();
		}
	}
}
