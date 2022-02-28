using System;
using System.Collections.Generic;
using System.Linq;
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

		private TimeSpan TimeSinceDraw { get; set; }

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
				foreach (Event ev in eventCategory.Events)
				{
					if (ev.IsHovered(EventTableModule.ModuleInstance.EventCategories, eventCategory, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin, ((Container)this).get_ContentRegion(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.Debug))
					{
						ev.HandleHover(sender, mouseEventArgs, PixelPerMinute);
					}
					else
					{
						ev.HandleNonHover(sender, mouseEventArgs);
					}
				}
			}
		}

		private void EventTableContainer_Click(object sender, MouseEventArgs e)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			if (!CursorVisible)
			{
				return;
			}
			foreach (EventCategory eventCategory in EventTableModule.ModuleInstance.EventCategories)
			{
				foreach (Event ev in eventCategory.Events)
				{
					if (ev.IsHovered(EventTableModule.ModuleInstance.EventCategories, eventCategory, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin, ((Container)this).get_ContentRegion(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.Debug))
					{
						ev.HandleClick(sender, e);
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
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
			InitializeBaseTexture(((GraphicsResource)spriteBatch).get_GraphicsDevice());
			List<EventCategory> eventCategories = EventTableModule.ModuleInstance.EventCategories;
			Color backgroundColor = Color.get_Transparent();
			if (EventTableModule.ModuleInstance.ModuleSettings.BackgroundColor.get_Value() != null && EventTableModule.ModuleInstance.ModuleSettings.BackgroundColor.get_Value().get_Id() != 1)
			{
				ColorExtensions.ToXnaColor(EventTableModule.ModuleInstance.ModuleSettings.BackgroundColor.get_Value().get_Cloth());
			}
			((Control)this).set_BackgroundColor(backgroundColor * EventTableModule.ModuleInstance.ModuleSettings.BackgroundColorOpacity.get_Value());
			int y = 0;
			bool anyCategoryDrawn = false;
			foreach (EventCategory eventCategory in eventCategories)
			{
				IEnumerable<IGrouping<Event, KeyValuePair<DateTime, Event>>> groups = from ev in eventCategory.GetEventOccurences(EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin, EventTableModule.ModuleInstance.ModuleSettings.UseFiller.get_Value())
					group ev by ev.Value;
				bool anyEventDrawn = false;
				foreach (IGrouping<Event, KeyValuePair<DateTime, Event>> item in groups)
				{
					List<DateTime> starts = item.Select((KeyValuePair<DateTime, Event> g) => g.Key).ToList();
					anyEventDrawn = starts.Count > 0;
					item.Key.Draw(spriteBatch, bounds, (Control)(object)this, Texture, eventCategories.ToList(), eventCategory, PixelPerMinute, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMin, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.Font, starts);
				}
				if (anyEventDrawn)
				{
					anyCategoryDrawn = true;
					y = groups.ElementAt(0).Key.GetYPosition(eventCategories, eventCategory, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.Debug);
				}
			}
			((Control)this).set_Size(new Point(bounds.Width, y + (anyCategoryDrawn ? EventTableModule.ModuleInstance.EventHeight : 0)));
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
			TimeSinceDraw += gameTime.get_ElapsedGameTime();
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
	}
}
