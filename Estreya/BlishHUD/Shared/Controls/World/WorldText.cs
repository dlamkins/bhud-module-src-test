using System;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
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

		private readonly Color _color;

		private static DynamicVertexBuffer _sharedVertexBuffer;

		private static readonly Vector3[] _faceVerts = (Vector3[])(object)new Vector3[4]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f)
		};

		public int TextureWidth { get; set; }

		public int TextureHeight { get; set; }

		public WorldText(Func<string> getText, BitmapFont font, Vector3 position, float scale, Color color)
			: base(position, scale)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_getText = getText;
			_font = font;
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

		private RenderTarget2D CreateTexture(GraphicsDevice graphicsDevice, string text, Size2 textSizes, Size2 textureSizes)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
			try
			{
				RenderTarget2D target = new RenderTarget2D(graphicsDevice, (int)Math.Ceiling(textureSizes.Width), (int)Math.Ceiling(textureSizes.Height + 2f), false, graphicsDevice.get_PresentationParameters().get_BackBufferFormat(), graphicsDevice.get_PresentationParameters().get_DepthStencilFormat(), 1, (RenderTargetUsage)1);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().get_PresentationParameters().set_RenderTargetUsage((RenderTargetUsage)1);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget(target);
				try
				{
					spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
					((GraphicsResource)spriteBatch).get_GraphicsDevice().Clear(Color.get_Transparent());
					BitmapFontExtensions.DrawString(spriteBatch, _font, text, new Vector2(textureSizes.Width / 2f - textSizes.Width / 2f, textureSizes.Height / 2f - textSizes.Height / 2f), _color, (Rectangle?)null);
					spriteBatch.End();
				}
				catch (Exception)
				{
				}
				((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
				return target;
			}
			finally
			{
				((IDisposable)spriteBatch)?.Dispose();
			}
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			string text = _getText();
			Size2 textSizes = _font.MeasureString(text);
			Size2 textureSizes = default(Size2);
			((Size2)(ref textureSizes))._002Ector(textSizes.Width, textSizes.Height);
			if ((float)TextureWidth > textureSizes.Width)
			{
				textureSizes.Width = TextureWidth;
			}
			if ((float)TextureHeight > textureSizes.Height)
			{
				textureSizes.Height = TextureHeight;
			}
			RenderTarget2D renderTarget2D = CreateTexture(graphicsDevice, text, textSizes, textureSizes);
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
