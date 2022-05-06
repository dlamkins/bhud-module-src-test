using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Input;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Utils;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class EventTableDrawer : Control
	{
		private bool _currentVisibilityDirection;

		private TimeSpan _lastDraw = TimeSpan.Zero;

		private RenderTarget2D _renderTarget;

		private bool _renderTargetIsEmpty = true;

		private static bool CursorVisible => GameService.Input.get_Mouse().get_CursorIsVisible();

		public bool Visible
		{
			get
			{
				if (_currentVisibilityDirection && CurrentVisibilityAnimation != null)
				{
					return true;
				}
				if (!_currentVisibilityDirection && CurrentVisibilityAnimation != null)
				{
					return false;
				}
				return ((Control)this).get_Visible();
			}
			set
			{
				((Control)this).set_Visible(value);
			}
		}

		private double PixelPerMinute => (double)((Control)this).get_Size().X / EventTableModule.ModuleInstance.EventTimeSpan.TotalMinutes;

		private Tween CurrentVisibilityAnimation { get; set; }

		public EventTableDrawer()
			: this()
		{
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)EventTableContainer_MouseMoved);
			CreateRenderTarget();
		}

		private void EventTableContainer_MouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (!CursorVisible)
			{
				return;
			}
			MouseEventArgs mouseEventArgs = new MouseEventArgs(((Control)this).get_RelativeMousePosition(), e.get_IsDoubleClick(), e.get_EventType());
			foreach (EventCategory eventCategory in EventTableModule.ModuleInstance.EventCategories)
			{
				foreach (Event ev2 in eventCategory.Events.Where((Event ev) => !ev.IsDisabled))
				{
					if (ev2.IsHovered(EventTableModule.ModuleInstance.EventTimeMin, ((Control)this).get_AbsoluteBounds(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute))
					{
						ev2.HandleHover(sender, mouseEventArgs, PixelPerMinute);
					}
					else
					{
						ev2.HandleNonHover(sender, mouseEventArgs);
					}
				}
			}
		}

		private void EventTableContainer_Click(object sender, MouseEventArgs e)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			if (!CursorVisible)
			{
				return;
			}
			foreach (EventCategory eventCategory in EventTableModule.ModuleInstance.EventCategories)
			{
				foreach (Event ev2 in eventCategory.Events.Where((Event ev) => !ev.IsDisabled))
				{
					if (ev2.IsHovered(EventTableModule.ModuleInstance.EventTimeMin, ((Control)this).get_AbsoluteBounds(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute))
					{
						ev2.HandleClick(sender, e);
						return;
					}
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		private void CreateRenderTarget()
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			int width = Math.Max(((Control)this).get_Width(), 1);
			int height = Math.Max(((Control)this).get_Height(), 1);
			if (_renderTarget != null && (((Texture2D)_renderTarget).get_Width() != width || ((Texture2D)_renderTarget).get_Height() != height))
			{
				((GraphicsResource)_renderTarget).Dispose();
				_renderTarget = null;
			}
			if (_renderTarget == null)
			{
				_renderTarget = new RenderTarget2D(GameService.Graphics.get_GraphicsDevice(), width, height, false, GameService.Graphics.get_GraphicsDevice().get_PresentationParameters().get_BackBufferFormat(), (DepthFormat)2, 1, (RenderTargetUsage)1);
				_renderTargetIsEmpty = true;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			((GraphicsResource)spriteBatch).get_GraphicsDevice().get_PresentationParameters().set_RenderTargetUsage((RenderTargetUsage)1);
			spriteBatch.End();
			int refreshInterval = EventTableModule.ModuleInstance.ModuleSettings.RefreshRateDelay.get_Value();
			if (_renderTargetIsEmpty || _lastDraw.TotalMilliseconds > (double)refreshInterval)
			{
				((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget(_renderTarget);
				spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().Clear(Color.get_Transparent());
				List<EventCategory> eventCategories = EventTableModule.ModuleInstance.EventCategories;
				int y = 0;
				DateTime now = EventTableModule.ModuleInstance.DateTimeNow;
				DateTime min = EventTableModule.ModuleInstance.EventTimeMin;
				DateTime max = EventTableModule.ModuleInstance.EventTimeMax;
				foreach (EventCategory item in eventCategories)
				{
					bool categoryHasEvents = false;
					foreach (Event ev2 in item.Events.Where((Event ev) => !ev.IsDisabled))
					{
						categoryHasEvents = true;
						if (EventTableModule.ModuleInstance.ModuleSettings.UseFiller.get_Value() || !ev2.Filler)
						{
							ev2.Draw(spriteBatch, bounds, Textures.get_Pixel(), y, PixelPerMinute, now, min, max, EventTableModule.ModuleInstance.Font);
						}
					}
					if (categoryHasEvents)
					{
						y += EventTableModule.ModuleInstance.EventHeight;
					}
				}
				UpdateSize(bounds.Width, y, overrideHeight: true);
				float middleLineX = (float)((Control)this).get_Size().X * EventTableModule.ModuleInstance.EventTimeSpanRatio;
				SpriteBatchUtil.DrawLine(spriteBatch, Textures.get_Pixel(), new RectangleF(middleLineX, 0f, 2f, (float)((Control)this).get_Size().Y), Color.get_LightGray());
				spriteBatch.End();
				((GraphicsResource)spriteBatch).get_GraphicsDevice().SetRenderTarget((RenderTarget2D)null);
				_renderTargetIsEmpty = false;
				_lastDraw = TimeSpan.Zero;
			}
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Texture2D)(object)_renderTarget, bounds, Color.get_White());
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
		}

		public void Show()
		{
			if (!Visible || CurrentVisibilityAnimation != null)
			{
				if (CurrentVisibilityAnimation != null)
				{
					CurrentVisibilityAnimation.Cancel();
				}
				_currentVisibilityDirection = true;
				Visible = true;
				CurrentVisibilityAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<EventTableDrawer>(this, (object)new
				{
					Opacity = 1f
				}, 0.2f, 0f, true);
				CurrentVisibilityAnimation.OnComplete((Action)delegate
				{
					CurrentVisibilityAnimation = null;
				});
			}
		}

		public void Hide()
		{
			if (Visible || CurrentVisibilityAnimation != null)
			{
				if (CurrentVisibilityAnimation != null)
				{
					CurrentVisibilityAnimation.Cancel();
				}
				_currentVisibilityDirection = false;
				CurrentVisibilityAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<EventTableDrawer>(this, (object)new
				{
					Opacity = 0f
				}, 0.2f, 0f, true);
				CurrentVisibilityAnimation.OnComplete((Action)delegate
				{
					Visible = false;
					CurrentVisibilityAnimation = null;
				});
			}
		}

		public void UpdatePosition(int x, int y)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			bool buildFromBottom = EventTableModule.ModuleInstance.ModuleSettings.BuildDirection.get_Value() == BuildDirection.Bottom;
			((Control)this).set_Location(buildFromBottom ? new Point(x, y - ((Control)this).get_Height()) : new Point(x, y));
		}

		public void UpdateSize(int width, int height, bool overrideHeight = false)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (height == -1)
			{
				height = ((Control)this).get_Size().Y;
			}
			((Control)this).set_Size(new Point(width, (!overrideHeight) ? ((Control)this).get_Size().Y : height));
			CreateRenderTarget();
		}

		protected override void DisposeControl()
		{
			Hide();
			((Control)this).remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).remove_MouseMoved((EventHandler<MouseEventArgs>)EventTableContainer_MouseMoved);
			((GraphicsResource)_renderTarget).Dispose();
			((Control)this).DisposeControl();
		}

		public override void DoUpdate(GameTime gameTime)
		{
			_lastDraw += gameTime.get_ElapsedGameTime();
		}

		public void UpdateBackgroundColor()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			Color backgroundColor = Color.get_Transparent();
			if (EventTableModule.ModuleInstance.ModuleSettings.BackgroundColor.get_Value() != null && EventTableModule.ModuleInstance.ModuleSettings.BackgroundColor.get_Value().get_Id() != 1)
			{
				backgroundColor = ColorExtensions.ToXnaColor(EventTableModule.ModuleInstance.ModuleSettings.BackgroundColor.get_Value().get_Cloth());
			}
			((Control)this).set_BackgroundColor(backgroundColor * EventTableModule.ModuleInstance.ModuleSettings.BackgroundColorOpacity.get_Value());
		}

		public Task LoadAsync()
		{
			UpdateBackgroundColor();
			return Task.CompletedTask;
		}
	}
}
