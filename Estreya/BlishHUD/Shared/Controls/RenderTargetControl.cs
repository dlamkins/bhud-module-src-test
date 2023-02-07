using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls
{
	public abstract class RenderTargetControl : Control
	{
		private static Logger Logger = Logger.GetLogger<RenderTargetControl>();

		private RenderTarget2D _renderTarget;

		private bool _renderTargetIsEmpty;

		private readonly AsyncLock _renderTargetLock = new AsyncLock();

		private TimeSpan _lastDraw = TimeSpan.Zero;

		public TimeSpan DrawInterval { get; set; } = TimeSpan.FromMilliseconds(500.0);


		public Point Size
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)this).get_Size();
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Size(value);
				CreateRenderTarget();
			}
		}

		public int Height
		{
			get
			{
				return ((Control)this).get_Height();
			}
			set
			{
				((Control)this).set_Height(value);
				CreateRenderTarget();
			}
		}

		public int Width
		{
			get
			{
				return ((Control)this).get_Width();
			}
			set
			{
				((Control)this).set_Width(value);
				CreateRenderTarget();
			}
		}

		public RenderTargetControl()
			: this()
		{
			CreateRenderTarget();
		}

		public override void Invalidate()
		{
			_lastDraw = DrawInterval;
			((Control)this).Invalidate();
		}

		protected sealed override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			((GraphicsResource)spriteBatch).get_GraphicsDevice().get_PresentationParameters().set_RenderTargetUsage((RenderTargetUsage)1);
			spriteBatch.End();
			if (_renderTargetLock.IsFree())
			{
				using (_renderTargetLock.Lock())
				{
					if (_renderTarget != null)
					{
						if (_renderTargetIsEmpty || _lastDraw >= DrawInterval)
						{
							((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget(_renderTarget);
							spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
							((GraphicsResource)spriteBatch).get_GraphicsDevice().Clear(Color.get_Transparent());
							DoPaint(spriteBatch, bounds);
							spriteBatch.End();
							((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
							_renderTargetIsEmpty = false;
							_lastDraw = TimeSpan.Zero;
						}
						SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Texture2D)(object)_renderTarget, bounds, Color.get_White());
						spriteBatch.End();
					}
				}
			}
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
		}

		public sealed override void DoUpdate(GameTime gameTime)
		{
			_lastDraw += gameTime.get_ElapsedGameTime();
			InternalUpdate(gameTime);
		}

		protected virtual void InternalUpdate(GameTime gameTime)
		{
		}

		protected abstract void DoPaint(SpriteBatch spriteBatch, Rectangle bounds);

		private void CreateRenderTarget()
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			int width = Math.Max(Width, 1);
			int height = Math.Max(Height, 1);
			using (_renderTargetLock.Lock())
			{
				if (_renderTarget != null && (((Texture2D)_renderTarget).get_Width() != width || ((Texture2D)_renderTarget).get_Height() != height))
				{
					((GraphicsResource)_renderTarget).Dispose();
					_renderTarget = null;
				}
				if (_renderTarget != null)
				{
					return;
				}
				try
				{
					GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
					try
					{
						_renderTarget = new RenderTarget2D(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), width, height, false, ((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice().get_PresentationParameters().get_BackBufferFormat(), ((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice().get_PresentationParameters().get_DepthStencilFormat(), 1, (RenderTargetUsage)1);
					}
					finally
					{
						((GraphicsDeviceContext)(ref ctx)).Dispose();
					}
					_renderTargetIsEmpty = true;
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to create Render Target");
				}
			}
		}

		protected sealed override void DisposeControl()
		{
			using (_renderTargetLock.Lock())
			{
				if (_renderTarget != null)
				{
					RenderTarget2D renderTarget = _renderTarget;
					if (renderTarget != null)
					{
						((GraphicsResource)renderTarget).Dispose();
					}
					_renderTarget = null;
				}
			}
			((Control)this).DisposeControl();
			InternalDispose();
		}

		protected virtual void InternalDispose()
		{
		}
	}
}
