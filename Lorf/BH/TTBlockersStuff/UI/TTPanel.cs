using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorf.BH.TTBlockersStuff.UI
{
	internal class TTPanel : Container
	{
		private const int ACCENT_LEFT_MARGIN = 4;

		private const int ACCENT_BOTTOM_MARGIN = 5;

		private const int ACCENT_RIGHT_MARGIN = 4;

		private const int RIGHT_PADDING = 15;

		private const int LEFT_PADDING = 7;

		private const int HEADER_HEIGHT = 36;

		private static readonly Texture2D _texturePanelHeader = Control.Content.GetTexture("controls/panel/1032325");

		private static readonly Texture2D _texturePanelHeaderActive = Control.Content.GetTexture("controls/panel/1032324");

		private static readonly Texture2D armorIcon = Module.Instance.ContentsManager.GetTexture("Armor_(attribute).png");

		private static readonly Texture2D markerIcon = Module.Instance.ContentsManager.GetTexture("marker_icon.png");

		private static readonly Texture2D impossibleIcon = Module.Instance.ContentsManager.GetTexture("Closed.png");

		private static readonly Texture2D resizeCornerInactive = Module.Instance.ContentsManager.GetTexture("resize_corner_inactive.png");

		private static readonly Texture2D resizeCornerActive = Module.Instance.ContentsManager.GetTexture("resize_corner_active.png");

		private static readonly Texture2D cornerAccent = Module.Instance.ContentsManager.GetTexture("corner_accent.png");

		private static readonly Texture2D borderAccent = Module.Instance.ContentsManager.GetTexture("border_accent.png");

		private static readonly SettingCollection windowSettings = Module.Instance.SettingsManager.ModuleSettings.AddSubCollection("TTPanel");

		private Rectangle layoutHeaderBounds;

		private Rectangle layoutHeaderTextBounds;

		private Rectangle layoutTopLeftAccentBounds;

		private Rectangle layoutBottomRightAccentBounds;

		private Rectangle layoutCornerAccentSrc;

		private Rectangle layoutLeftAccentBounds;

		private Rectangle layoutInternalBounds;

		private Rectangle resizeHandleBounds;

		private Point dragStart;

		private Point resizeStart;

		private bool dragging;

		private bool isBlocking;

		private bool isMounted;

		private float blockerIconRotation;

		private bool blockerIconVisible;

		private Color blockerIconTint;

		private bool resizing;

		protected string title;

		protected bool targetVisibility;

		public bool Dragging
		{
			get
			{
				return dragging;
			}
			private set
			{
				SetProperty(ref dragging, value, invalidateLayout: false, "Dragging");
			}
		}

		public bool IsBlocking
		{
			get
			{
				return isBlocking;
			}
			set
			{
				SetProperty(ref isBlocking, value, invalidateLayout: true, "IsBlocking");
			}
		}

		public bool IsMounted
		{
			get
			{
				return isMounted;
			}
			set
			{
				SetProperty(ref isMounted, value, invalidateLayout: true, "IsMounted");
			}
		}

		public float BlockerIconRotation
		{
			get
			{
				return blockerIconRotation;
			}
			set
			{
				SetProperty(ref blockerIconRotation, value, invalidateLayout: false, "BlockerIconRotation");
			}
		}

		public bool BlockerIconVisible
		{
			get
			{
				return blockerIconVisible;
			}
			set
			{
				SetProperty(ref blockerIconVisible, value, invalidateLayout: true, "BlockerIconVisible");
			}
		}

		public Color BlockerIconTint
		{
			get
			{
				return blockerIconTint;
			}
			set
			{
				SetProperty(ref blockerIconTint, value, invalidateLayout: false, "BlockerIconTint");
			}
		}

		public bool Resizing
		{
			get
			{
				return resizing;
			}
			private set
			{
				SetProperty(ref resizing, value, invalidateLayout: false, "Resizing");
			}
		}

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				SetProperty(ref title, value, invalidateLayout: false, "Title");
			}
		}

		public bool TargetVisibility
		{
			get
			{
				return targetVisibility;
			}
			set
			{
				SetProperty(ref targetVisibility, value, invalidateLayout: false, "TargetVisibility");
			}
		}

		protected int InternalWidth => base.Width - 4 - 4;

		protected int InternalHeight => base.Height - 5;

		public TTPanel()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased += OnGlobalMouseRelease;
			resizeHandleBounds = Rectangle.Empty;
			dragStart = Point.Zero;
			resizeStart = Point.Zero;
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			return new Point(MathHelper.Clamp(newSize.X, 250, (int)((float)GameService.Graphics.Resolution.X / GameService.Graphics.UIScaleMultiplier)), MathHelper.Clamp(newSize.Y, 41, (int)((float)GameService.Graphics.Resolution.Y / GameService.Graphics.UIScaleMultiplier)));
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			if (Dragging)
			{
				base.Location += Control.Input.Mouse.Position - dragStart;
				dragStart = Control.Input.Mouse.Position;
			}
			else if (Resizing)
			{
				Point nOffset = Control.Input.Mouse.Position - dragStart;
				base.Size = HandleWindowResize(resizeStart + nOffset);
			}
			if (base.Visible != TargetVisibility || _opacity != 0f || _opacity != 1f)
			{
				if (TargetVisibility)
				{
					base.Opacity = MathHelper.Clamp(base.Opacity + (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 200.0), 0f, 1f);
				}
				else
				{
					base.Opacity = MathHelper.Clamp(base.Opacity - (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 200.0), 0f, 1f);
				}
				base.Visible = _opacity > 0f;
			}
			base.UpdateContainer(gameTime);
		}

		public override void RecalculateLayout()
		{
			layoutInternalBounds = new Rectangle(4, 0, InternalWidth, InternalHeight);
			layoutHeaderBounds = new Rectangle(4, 0, InternalWidth, 36);
			layoutHeaderTextBounds = new Rectangle(layoutHeaderBounds.X + 7 + (BlockerIconVisible ? (markerIcon.Width + 3) : 0), layoutHeaderBounds.Y, layoutHeaderBounds.Width - 15 - 7, layoutHeaderBounds.Height);
			resizeHandleBounds = new Rectangle(base.Width - resizeCornerInactive.Width, base.Height - resizeCornerInactive.Height, resizeCornerInactive.Width, resizeCornerInactive.Height);
			int cornerAccentWidth = Math.Min(base.Width, cornerAccent.Width);
			layoutTopLeftAccentBounds = new Rectangle(0, 30, cornerAccentWidth, cornerAccent.Height);
			layoutBottomRightAccentBounds = new Rectangle(base.Width - cornerAccentWidth, base.Height - cornerAccent.Height, cornerAccentWidth, cornerAccent.Height);
			layoutCornerAccentSrc = new Rectangle(0, 0, cornerAccentWidth, cornerAccent.Height);
			layoutLeftAccentBounds = new Rectangle(4, 36, borderAccent.Width, Math.Min(InternalHeight - 36, borderAccent.Height));
			base.ContentRegion = new Rectangle(4, 36, InternalWidth, InternalHeight - 36);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, layoutInternalBounds, Color.Black * 0.1f);
			spriteBatch.DrawOnCtrl(this, (_mouseOver && base.RelativeMousePosition.Y <= 36) ? _texturePanelHeaderActive : _texturePanelHeader, layoutHeaderBounds);
			spriteBatch.DrawStringOnCtrl(this, title, Control.Content.DefaultFont16, layoutHeaderTextBounds, Color.White);
			spriteBatch.DrawOnCtrl(this, cornerAccent, layoutTopLeftAccentBounds, layoutCornerAccentSrc, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally);
			spriteBatch.DrawOnCtrl(this, cornerAccent, layoutBottomRightAccentBounds, layoutCornerAccentSrc, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically);
			spriteBatch.DrawOnCtrl(this, borderAccent, layoutLeftAccentBounds, layoutLeftAccentBounds, Color.Black, 0f, Vector2.Zero);
			if (blockerIconVisible)
			{
				Texture2D icon = (isMounted ? impossibleIcon : (isBlocking ? armorIcon : markerIcon));
				spriteBatch.DrawOnCtrl(this, icon, new Rectangle(22, 18, 26, 26), icon.Bounds, isMounted ? Color.White : blockerIconTint, isMounted ? 0f : blockerIconRotation, new Vector2(icon.Width / 2, icon.Height / 2));
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			Texture2D iconTexture = ((resizeHandleBounds.Contains(base.RelativeMousePosition) || Resizing) ? resizeCornerActive : resizeCornerInactive);
			spriteBatch.DrawOnCtrl(this, iconTexture, resizeHandleBounds);
			base.PaintAfterChildren(spriteBatch, bounds);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (resizeHandleBounds.Contains(base.RelativeMousePosition))
			{
				Resizing = true;
				resizeStart = base.Size;
				dragStart = Control.Input.Mouse.Position;
			}
			else if (_mouseOver && base.RelativeMousePosition.Y <= 36)
			{
				Dragging = true;
				dragStart = Control.Input.Mouse.Position;
			}
			base.OnLeftMouseButtonPressed(e);
		}

		protected void OnGlobalMouseRelease(object sender, MouseEventArgs e)
		{
			if (!base.Visible)
			{
				return;
			}
			if (Dragging || Resizing)
			{
				((windowSettings["location"] as SettingEntry<Point>) ?? windowSettings.DefineSetting("location", dragStart)).Value = base.Location;
				((windowSettings["size"] as SettingEntry<Point>) ?? windowSettings.DefineSetting("size", resizeStart)).Value = base.Size;
				if (Resizing)
				{
					OnResized(new ResizedEventArgs(resizeStart, base.Size));
				}
				Dragging = false;
				Resizing = false;
				return;
			}
			foreach (Control child in base.Children)
			{
				TimerBar pb = child as TimerBar;
				if (pb != null && pb.LocalBounds.OffsetBy(base.ContentRegion.Location).Contains(base.RelativeMousePosition))
				{
					pb.OnInternalClick(this, e);
				}
			}
		}

		public override void Hide()
		{
			if (TargetVisibility)
			{
				TargetVisibility = false;
				Dragging = false;
			}
		}

		public override void Show()
		{
			if (TargetVisibility)
			{
				return;
			}
			TargetVisibility = true;
			if (windowSettings.TryGetSetting("size", out var windowSize))
			{
				SettingEntry<Point> setting2 = windowSize as SettingEntry<Point>;
				if (setting2 == null)
				{
					setting2 = new SettingEntry<Point>();
					setting2.Value = new Point(400, 120);
				}
				base.Size = setting2.Value;
			}
			else
			{
				base.Size = new Point(400, 120);
			}
			if (windowSettings.TryGetSetting("location", out var windowPosition))
			{
				SettingEntry<Point> setting = windowPosition as SettingEntry<Point>;
				if (setting == null)
				{
					setting = new SettingEntry<Point>();
					setting.Value = GetDefaultLocation();
				}
				base.Location = setting.Value;
			}
			else
			{
				base.Location = GetDefaultLocation();
			}
			base.Show();
		}

		private Point GetDefaultLocation()
		{
			return new Point((int)((float)GameService.Graphics.Resolution.X / GameService.Graphics.UIScaleMultiplier / 2f) - base.Width / 2, (int)((float)GameService.Graphics.Resolution.Y / GameService.Graphics.UIScaleMultiplier / 2f) - base.Height / 2);
		}

		protected override void DisposeControl()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased -= OnGlobalMouseRelease;
			_texturePanelHeader?.Dispose();
			_texturePanelHeaderActive?.Dispose();
			armorIcon?.Dispose();
			markerIcon?.Dispose();
			impossibleIcon?.Dispose();
			resizeCornerInactive?.Dispose();
			resizeCornerActive?.Dispose();
			cornerAccent?.Dispose();
			borderAccent?.Dispose();
			base.DisposeControl();
		}
	}
}
