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

		private Texture2D _texturePanelHeader = Module.Instance.ContentsManager.GetTexture("1032325.png");

		private Texture2D _texturePanelHeaderActive = Module.Instance.ContentsManager.GetTexture("1032324.png");

		private Texture2D armorIcon = Module.Instance.ContentsManager.GetTexture("Armor_(attribute).png");

		private Texture2D markerIcon = Module.Instance.ContentsManager.GetTexture("marker_icon.png");

		private Texture2D impossibleIcon = Module.Instance.ContentsManager.GetTexture("Closed.png");

		private Texture2D resizeCornerInactive = Module.Instance.ContentsManager.GetTexture("resize_corner_inactive.png");

		private Texture2D resizeCornerActive = Module.Instance.ContentsManager.GetTexture("resize_corner_active.png");

		private Texture2D cornerAccent = Module.Instance.ContentsManager.GetTexture("corner_accent.png");

		private Texture2D borderAccent = Module.Instance.ContentsManager.GetTexture("border_accent.png");

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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return blockerIconTint;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			GameService.Input.Mouse.LeftMouseButtonReleased += OnGlobalMouseRelease;
			resizeHandleBounds = Rectangle.get_Empty();
			dragStart = Point.get_Zero();
			resizeStart = Point.get_Zero();
		}

		protected virtual Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, 250, (int)((float)GameService.Graphics.Resolution.X / GameService.Graphics.UIScaleMultiplier)), MathHelper.Clamp(newSize.Y, 41, (int)((float)GameService.Graphics.Resolution.Y / GameService.Graphics.UIScaleMultiplier)));
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
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
					base.Opacity = MathHelper.Clamp(base.Opacity + (float)(gameTime.get_ElapsedGameTime().TotalMilliseconds / 200.0), 0f, 1f);
				}
				else
				{
					base.Opacity = MathHelper.Clamp(base.Opacity - (float)(gameTime.get_ElapsedGameTime().TotalMilliseconds / 200.0), 0f, 1f);
				}
				base.Visible = _opacity > 0f;
			}
			base.UpdateContainer(gameTime);
		}

		public override void RecalculateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			layoutInternalBounds = new Rectangle(4, 0, InternalWidth, InternalHeight);
			layoutHeaderBounds = new Rectangle(4, 0, InternalWidth, 36);
			layoutHeaderTextBounds = new Rectangle(layoutHeaderBounds.X + 7 + (BlockerIconVisible ? (markerIcon.get_Width() + 3) : 0), layoutHeaderBounds.Y, layoutHeaderBounds.Width - 15 - 7, layoutHeaderBounds.Height);
			resizeHandleBounds = new Rectangle(base.Width - resizeCornerInactive.get_Width(), base.Height - resizeCornerInactive.get_Height(), resizeCornerInactive.get_Width(), resizeCornerInactive.get_Height());
			int cornerAccentWidth = Math.Min(base.Width, cornerAccent.get_Width());
			layoutTopLeftAccentBounds = new Rectangle(0, 30, cornerAccentWidth, cornerAccent.get_Height());
			layoutBottomRightAccentBounds = new Rectangle(base.Width - cornerAccentWidth, base.Height - cornerAccent.get_Height(), cornerAccentWidth, cornerAccent.get_Height());
			layoutCornerAccentSrc = new Rectangle(0, 0, cornerAccentWidth, cornerAccent.get_Height());
			layoutLeftAccentBounds = new Rectangle(4, 36, borderAccent.get_Width(), Math.Min(InternalHeight - 36, borderAccent.get_Height()));
			base.ContentRegion = new Rectangle(4, 36, InternalWidth, InternalHeight - 36);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, layoutInternalBounds, Color.get_Black() * 0.1f);
			spriteBatch.DrawOnCtrl(this, (_mouseOver && base.RelativeMousePosition.Y <= 36) ? _texturePanelHeaderActive : _texturePanelHeader, layoutHeaderBounds);
			spriteBatch.DrawStringOnCtrl(this, title, Control.Content.DefaultFont16, layoutHeaderTextBounds, Color.get_White());
			spriteBatch.DrawOnCtrl(this, cornerAccent, layoutTopLeftAccentBounds, layoutCornerAccentSrc, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)1);
			spriteBatch.DrawOnCtrl(this, cornerAccent, layoutBottomRightAccentBounds, layoutCornerAccentSrc, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)2);
			spriteBatch.DrawOnCtrl(this, borderAccent, layoutLeftAccentBounds, layoutLeftAccentBounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (blockerIconVisible)
			{
				Texture2D icon = (isMounted ? impossibleIcon : (isBlocking ? armorIcon : markerIcon));
				spriteBatch.DrawOnCtrl(this, icon, new Rectangle(22, 18, 26, 26), icon.get_Bounds(), isMounted ? Color.get_White() : blockerIconTint, isMounted ? 0f : blockerIconRotation, new Vector2((float)(icon.get_Width() / 2), (float)(icon.get_Height() / 2)), (SpriteEffects)0);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Texture2D iconTexture = ((((Rectangle)(ref resizeHandleBounds)).Contains(base.RelativeMousePosition) || Resizing) ? resizeCornerActive : resizeCornerInactive);
			spriteBatch.DrawOnCtrl(this, iconTexture, resizeHandleBounds);
			base.PaintAfterChildren(spriteBatch, bounds);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (((Rectangle)(ref resizeHandleBounds)).Contains(base.RelativeMousePosition))
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
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Visible)
			{
				return;
			}
			if (Dragging || Resizing)
			{
				((windowSettings["location"] as SettingEntry<Point>) ?? windowSettings.DefineSetting<Point>("location", dragStart)).Value = base.Location;
				((windowSettings["size"] as SettingEntry<Point>) ?? windowSettings.DefineSetting<Point>("size", resizeStart)).Value = base.Size;
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
				if (pb != null)
				{
					Rectangle localBounds = pb.LocalBounds;
					Rectangle val = base.ContentRegion;
					val = localBounds.OffsetBy(((Rectangle)(ref val)).get_Location());
					if (((Rectangle)(ref val)).Contains(base.RelativeMousePosition))
					{
						pb.OnInternalClick(this, e);
					}
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
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			return new Point((int)((float)GameService.Graphics.Resolution.X / GameService.Graphics.UIScaleMultiplier / 2f) - base.Width / 2, (int)((float)GameService.Graphics.Resolution.Y / GameService.Graphics.UIScaleMultiplier / 2f) - base.Height / 2);
		}

		protected override void DisposeControl()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased -= OnGlobalMouseRelease;
			Texture2D texturePanelHeader = _texturePanelHeader;
			if (texturePanelHeader != null)
			{
				((GraphicsResource)texturePanelHeader).Dispose();
			}
			Texture2D texturePanelHeaderActive = _texturePanelHeaderActive;
			if (texturePanelHeaderActive != null)
			{
				((GraphicsResource)texturePanelHeaderActive).Dispose();
			}
			Texture2D obj = armorIcon;
			if (obj != null)
			{
				((GraphicsResource)obj).Dispose();
			}
			Texture2D obj2 = markerIcon;
			if (obj2 != null)
			{
				((GraphicsResource)obj2).Dispose();
			}
			Texture2D obj3 = impossibleIcon;
			if (obj3 != null)
			{
				((GraphicsResource)obj3).Dispose();
			}
			Texture2D obj4 = resizeCornerInactive;
			if (obj4 != null)
			{
				((GraphicsResource)obj4).Dispose();
			}
			Texture2D obj5 = resizeCornerActive;
			if (obj5 != null)
			{
				((GraphicsResource)obj5).Dispose();
			}
			Texture2D obj6 = cornerAccent;
			if (obj6 != null)
			{
				((GraphicsResource)obj6).Dispose();
			}
			Texture2D obj7 = borderAccent;
			if (obj7 != null)
			{
				((GraphicsResource)obj7).Dispose();
			}
			base.DisposeControl();
		}
	}
}
