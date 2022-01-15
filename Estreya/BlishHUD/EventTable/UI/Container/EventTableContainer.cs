using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD._Extensions;
using Estreya.BlishHUD.EventTable.Input;
using Estreya.BlishHUD.EventTable.Models;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Container
{
	public class EventTableContainer : Container
	{
		public enum FontSize
		{
			Size8,
			Size11,
			Size12,
			Size14,
			Size16,
			Size18,
			Size20,
			Size22,
			Size24,
			Size32,
			Size34,
			Size36
		}

		private BitmapFont _font;

		private IEnumerable<EventCategory> _eventCategories;

		private TimeSpan TimeSinceDraw { get; set; }

		private BitmapFont Font
		{
			get
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				if (_font == null)
				{
					if (!Enum.TryParse<FontSize>(Enum.GetName(typeof(FontSize), Settings.EventFontSize.get_Value()), out FontSize size))
					{
						size = (FontSize)16;
					}
					_font = GameService.Content.GetFont((FontFace)0, size, (FontStyle)0);
				}
				return _font;
			}
		}

		private double PixelPerMinute => (double)((Control)this).get_Size().X / EventTableModule.ModuleInstance.EventTimeSpan.TotalMinutes;

		private IEnumerable<EventCategory> EventCategories
		{
			get
			{
				return _eventCategories;
			}
			set
			{
				_eventCategories = value;
			}
		}

		private Tween CurrentVisibilityAnimation { get; set; }

		private ModuleSettings Settings { get; set; }

		private Texture2D Texture { get; set; }

		public EventTableContainer(IEnumerable<EventCategory> eventCategories, ModuleSettings settings)
			: this()
		{
			EventCategories = eventCategories;
			Settings = settings;
			Settings.ModuleSettingsChanged += Settings_ModuleSettingsChanged;
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)EventTableContainer_Click);
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)EventTableContainer_MouseMoved);
		}

		private void EventTableContainer_MouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			MouseEventArgs mouseEventArgs = new MouseEventArgs(((Control)this).get_RelativeMousePosition(), e.get_IsDoubleClick(), e.get_EventType());
			foreach (EventCategory eventCategory in EventCategories)
			{
				foreach (Event ev in eventCategory.Events)
				{
					if (ev.IsHovered(EventCategories, eventCategory, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin, ((Container)this).get_ContentRegion(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.Debug))
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

		private void Settings_ModuleSettingsChanged(object sender, ModuleSettings.ModuleSettingsChangedEventArgs e)
		{
			if (e.Name == "EventFontSize")
			{
				_font = null;
			}
		}

		private void EventTableContainer_Click(object sender, MouseEventArgs e)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			foreach (EventCategory eventCategory in EventCategories)
			{
				foreach (Event ev in eventCategory.Events)
				{
					if (ev.IsHovered(EventCategories, eventCategory, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin, ((Container)this).get_ContentRegion(), ((Control)this).get_RelativeMousePosition(), PixelPerMinute, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.Debug))
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
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
			InitializeBaseTexture(((GraphicsResource)spriteBatch).get_GraphicsDevice());
			IEnumerable<EventCategory> eventCategories = EventCategories;
			Color backgroundColor = ((Settings.BackgroundColor.get_Value().get_Id() == 1) ? Color.get_Transparent() : ColorExtensions.ToXnaColor(Settings.BackgroundColor.get_Value().get_Cloth()));
			((Control)this).set_BackgroundColor(backgroundColor * Settings.BackgroundColorOpacity.get_Value());
			int y = 0;
			foreach (EventCategory eventCategory in eventCategories)
			{
				IEnumerable<IGrouping<Event, KeyValuePair<DateTime, Event>>> groups = from ev in eventCategory.GetEventOccurences(Settings.AllEvents, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMax, EventTableModule.ModuleInstance.EventTimeMin, Settings.UseFiller.get_Value())
					group ev by ev.Value;
				bool anyEventDrawn = false;
				foreach (IGrouping<Event, KeyValuePair<DateTime, Event>> item in groups)
				{
					List<DateTime> starts = item.Select((KeyValuePair<DateTime, Event> g) => g.Key).ToList();
					anyEventDrawn = starts.Count > 0;
					item.Key.Draw(spriteBatch, bounds, (Control)(object)this, Texture, eventCategories.ToList(), eventCategory, PixelPerMinute, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.DateTimeNow, EventTableModule.ModuleInstance.EventTimeMin, EventTableModule.ModuleInstance.EventTimeMax, Font, starts);
				}
				if (anyEventDrawn)
				{
					y = groups.ElementAt(0).Key.GetYPosition(eventCategories, eventCategory, EventTableModule.ModuleInstance.EventHeight, EventTableModule.ModuleInstance.Debug);
				}
			}
			if (Settings.SnapHeight.get_Value())
			{
				((Control)this).set_Size(new Point(bounds.Width, y + EventTableModule.ModuleInstance.EventHeight));
			}
			DrawLine(spriteBatch, new Rectangle(((Control)this).get_Size().X / 2, 0, 2, ((Control)this).get_Size().Y), Color.get_LightGray());
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
		}

		public void Show()
		{
			if (!((Control)this).get_Visible() || CurrentVisibilityAnimation != null)
			{
				if (CurrentVisibilityAnimation != null)
				{
					CurrentVisibilityAnimation.Cancel();
				}
				((Control)this).set_Visible(true);
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
			if (((Control)this).get_Visible() || CurrentVisibilityAnimation != null)
			{
				if (CurrentVisibilityAnimation != null)
				{
					CurrentVisibilityAnimation.Cancel();
				}
				CurrentVisibilityAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<EventTableContainer>(this, (object)new
				{
					Opacity = 0f
				}, 0.2f, 0f, true);
				CurrentVisibilityAnimation.OnComplete((Action)delegate
				{
					((Control)this).set_Visible(false);
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
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(width, (Settings.SnapHeight.get_Value() && !overrideHeight) ? ((Control)this).get_Size().Y : height));
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

		private void DrawLine(SpriteBatch spriteBatch, Rectangle coords, Color color)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			InitializeBaseTexture(((GraphicsResource)spriteBatch).get_GraphicsDevice());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Texture, coords, color);
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
