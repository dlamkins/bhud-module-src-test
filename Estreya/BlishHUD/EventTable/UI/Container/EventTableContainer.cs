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

namespace Estreya.BlishHUD.EventTable.UI.Container
{
	public class EventTableContainer : Container
	{
		private bool _currentVisibilityDirection;

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

		private Texture2D Texture { get; set; }

		public EventTableContainer()
			: this()
		{
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)EventTableContainer_MouseMoved);
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
				foreach (Event ev2 in eventCategory.Events.Where((Event ev) => !ev.IsDisabled()))
				{
					if (ev2.IsHovered(EventTableModule.ModuleInstance.EventTimeMin, ((Container)this).get_ContentRegion(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute))
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
				foreach (Event ev2 in eventCategory.Events.Where((Event ev) => !ev.IsDisabled()))
				{
					if (ev2.IsHovered(EventTableModule.ModuleInstance.EventTimeMin, ((Container)this).get_ContentRegion(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute))
					{
						ev2.HandleClick(sender, e);
						return;
					}
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
			InitializeBaseTexture(((GraphicsResource)spriteBatch).get_GraphicsDevice());
			List<EventCategory> eventCategories = EventTableModule.ModuleInstance.EventCategories;
			int y = 0;
			DateTime now = EventTableModule.ModuleInstance.DateTimeNow;
			DateTime min = EventTableModule.ModuleInstance.EventTimeMin;
			DateTime max = EventTableModule.ModuleInstance.EventTimeMax;
			foreach (EventCategory item in eventCategories)
			{
				bool categoryHasEvents = false;
				foreach (Event ev2 in item.Events.Where((Event ev) => !ev.IsDisabled()))
				{
					categoryHasEvents = true;
					if (EventTableModule.ModuleInstance.ModuleSettings.UseFiller.get_Value() || !ev2.Filler)
					{
						ev2.Draw(spriteBatch, bounds, (Control)(object)this, Texture, y, PixelPerMinute, now, min, max, EventTableModule.ModuleInstance.Font);
					}
				}
				if (categoryHasEvents)
				{
					y += EventTableModule.ModuleInstance.EventHeight;
				}
			}
			((Control)this).set_Size(new Point(bounds.Width, y));
			float middleLineX = (float)((Control)this).get_Size().X * EventTableModule.ModuleInstance.EventTimeSpanRatio;
			DrawLine(spriteBatch, new RectangleF(middleLineX, 0f, 2f, (float)((Control)this).get_Size().Y), Color.get_LightGray());
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
				CurrentVisibilityAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<EventTableContainer>(this, (object)new
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
				CurrentVisibilityAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<EventTableContainer>(this, (object)new
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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (EventTableModule.ModuleInstance.ModuleSettings.BuildDirection.get_Value() == BuildDirection.Bottom)
			{
				((Control)this).set_Location(new Point(x, y - ((Control)this).get_Height()));
			}
			else
			{
				((Control)this).set_Location(new Point(x, y));
			}
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
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
		}

		private void InitializeBaseTexture(GraphicsDevice graphicsDevice)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (Texture == null)
			{
				Texture = new Texture2D(graphicsDevice, 1, 1);
				Texture.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
			}
		}

		private void DrawLine(SpriteBatch spriteBatch, RectangleF coords, Color color)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			InitializeBaseTexture(((GraphicsResource)spriteBatch).get_GraphicsDevice());
			spriteBatch.DrawOnCtrl((Control)(object)this, Texture, coords, color);
		}

		protected override void DisposeControl()
		{
			Hide();
			if (Texture != null)
			{
				((GraphicsResource)Texture).Dispose();
				Texture = null;
			}
			((Container)this).DisposeControl();
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
