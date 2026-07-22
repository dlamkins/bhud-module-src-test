using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class FlowPanel : Blish_HUD.Controls.FlowPanel, ILocalizable
	{
		private readonly List<(Rectangle, float)> _leftBorders = new List<(Rectangle, float)>();

		private readonly List<(Rectangle, float)> _topBorders = new List<(Rectangle, float)>();

		private readonly List<(Rectangle, float)> _rightBorders = new List<(Rectangle, float)>();

		private readonly List<(Rectangle, float)> _bottomBorders = new List<(Rectangle, float)>();

		private readonly AsyncTexture2D _texturePanelHeader = AsyncTexture2D.FromAssetId(1032325);

		private readonly AsyncTexture2D _texturePanelHeaderActive = AsyncTexture2D.FromAssetId(1032324);

		private readonly AsyncTexture2D _textureCornerAccent = AsyncTexture2D.FromAssetId(1002144);

		private readonly AsyncTexture2D _textureLeftSideAccent = AsyncTexture2D.FromAssetId(605025);

		private readonly AsyncTexture2D _textureAccordionArrow = AsyncTexture2D.FromAssetId(155953);

		private readonly BasicTooltip _tooltip = new BasicTooltip
		{
			Parent = Control.Graphics.SpriteScreen,
			ZIndex = 1073741823,
			Visible = false
		};

		private Vector2 _layoutAccordionArrowOrigin;

		private Rectangle _layoutTopLeftAccentBounds;

		private Rectangle _layoutBottomRightAccentBounds;

		private Rectangle _layoutCornerAccentSrc;

		private Rectangle _layoutLeftAccentBounds;

		private Rectangle _layoutRightAccentBounds;

		private Rectangle _layoutLeftAccentSrc;

		private Rectangle _layoutHeaderTextBounds;

		private Rectangle _layoutHeaderIconBounds;

		private Rectangle _layoutAccordionArrowBounds;

		private RectangleDimensions _contentPadding = new RectangleDimensions(0);

		private RectangleDimensions _borderWidth = new RectangleDimensions(0);

		private Rectangle _backgroundBounds;

		private RectangleDimensions _titleIconPadding = new RectangleDimensions(3, 3, 5, 3);

		private bool _resized;

		protected Rectangle LayoutHeaderBounds;

		private Point _dragStart;

		private Point _draggingStart;

		private bool _dragging;

		public bool ShowRightBorder { get; set; }

		public RectangleDimensions BorderWidth
		{
			get
			{
				return _borderWidth;
			}
			set
			{
				_borderWidth = value;
				RecalculateLayout();
			}
		}

		public RectangleDimensions ContentPadding
		{
			get
			{
				return _contentPadding;
			}
			set
			{
				_contentPadding = value;
				RecalculateLayout();
			}
		}

		public RectangleDimensions TitleIconPadding
		{
			get
			{
				return _titleIconPadding;
			}
			set
			{
				_titleIconPadding = value;
				RecalculateLayout();
			}
		}

		public int TitleBarHeight
		{
			[CompilerGenerated]
			get
			{
				return _003CTitleBarHeight_003Ek__BackingField;
			}
			set
			{
				_003CTitleBarHeight_003Ek__BackingField = value;
				RecalculateLayout();
			}
		}

		public Color? BorderColor { get; set; }

		public Color? HoveredBorderColor { get; set; }

		public AsyncTexture2D BackgroundImage { get; set; }

		public AsyncTexture2D TitleIcon { get; set; }

		public Color? BackgroundImageColor { get; set; }

		public Color? BackgroundImageHoveredColor { get; set; }

		public new Color? BackgroundColor { get; set; }

		public Color? BackgroundHoveredColor { get; set; }

		public Rectangle? TextureRectangle { get; set; }

		public string TitleTooltipText
		{
			get
			{
				return _tooltip.Text;
			}
			set
			{
				_tooltip.Text = value;
			}
		}

		public Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedTitleTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTitleTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTitleTooltip_003Ek__BackingField = value;
				TitleTooltipText = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedTitle
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTitle_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTitle_003Ek__BackingField = value;
				base.Title = value?.Invoke();
			}
		}

		public Action OnCollapse { get; set; }

		public Action OnExpand { get; set; }

		public bool CaptureInput { get; set; }

		public CaptureType? Capture { get; set; }

		public bool CanDrag { get; set; }

		public FlowPanel()
		{
			_003CTitleBarHeight_003Ek__BackingField = 36;
			BackgroundImageColor = Color.White;
			CaptureInput = true;
			base._002Ector();
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			bool collapsed = base.Collapsed;
			base.OnClick(e);
			if (collapsed != base.Collapsed)
			{
				if (base.Collapsed)
				{
					OnCollapse?.Invoke();
				}
				else
				{
					OnExpand?.Invoke();
				}
			}
			if (CanDrag)
			{
				_ = GameService.Input.Mouse.State.LeftButton;
				_ = 1;
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_backgroundBounds = new Rectangle(Math.Max(BorderWidth.Left - 2, 0), Math.Max(BorderWidth.Top - 2, 0), base.Width - Math.Max(BorderWidth.Horizontal - 4, 0), base.Height - Math.Max(BorderWidth.Vertical - 4, 0));
			int num = ((!string.IsNullOrEmpty(_title)) ? TitleBarHeight : 0);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (base.ShowBorder)
			{
				num = Math.Max(7, num);
				num2 = 4;
				num3 = 7;
				num4 = 4;
				int num5 = Math.Min(_size.X, 256);
				_layoutTopLeftAccentBounds = new Rectangle(-2, num - 12, num5, _textureCornerAccent.Height);
				_layoutBottomRightAccentBounds = new Rectangle(_size.X - num5 + 2, _size.Y - 59, num5, _textureCornerAccent.Height);
				_layoutCornerAccentSrc = new Rectangle(256 - num5, 0, num5, _textureCornerAccent.Height);
				_layoutLeftAccentBounds = new Rectangle(num4 - 7, num, _textureLeftSideAccent.Width, Math.Min(_size.Y - num - num3, _textureLeftSideAccent.Height));
				_layoutRightAccentBounds = new Rectangle(_size.X - 12, Math.Max(0, _size.Y - _layoutLeftAccentBounds.Height), _textureLeftSideAccent.Width, Math.Min(_size.Y - num - num3 - 10, _textureLeftSideAccent.Height - 10));
				_layoutLeftAccentSrc = new Rectangle(0, 0, _textureLeftSideAccent.Width, _layoutLeftAccentBounds.Height);
			}
			base.ContentRegion = new Rectangle(_contentPadding.Left + num4, _contentPadding.Top + num, _size.X - num4 - num2 - _contentPadding.Horizontal, _size.Y - num - num3 - _contentPadding.Vertical);
			LayoutHeaderBounds = new Rectangle(num4, 0, base.Width, num);
			_layoutHeaderIconBounds = ((TitleIcon != null) ? new Rectangle(LayoutHeaderBounds.Left + _titleIconPadding.Left, _titleIconPadding.Top, num - _titleIconPadding.Vertical, num - _titleIconPadding.Vertical) : Rectangle.Empty);
			_layoutHeaderTextBounds = new Rectangle(_layoutHeaderIconBounds.Right + _titleIconPadding.Right, 0, LayoutHeaderBounds.Width - _layoutHeaderIconBounds.Width, num);
			int arrowSize = num - 4;
			_layoutAccordionArrowOrigin = new Vector2(16f, 16f);
			_layoutAccordionArrowBounds = new Rectangle(LayoutHeaderBounds.Right - arrowSize, (num - arrowSize) / 2, arrowSize, arrowSize).OffsetBy(new Point(arrowSize / 2, arrowSize / 2));
			if (base.Collapsed && _size.Y > LayoutHeaderBounds.Height && !_resized)
			{
				_resized = true;
				Task.Run(async delegate
				{
					await Task.Delay(125);
					if (!base.Collapsed)
					{
						_resized = false;
					}
					else
					{
						base.Size = new Point(base.Width, LayoutHeaderBounds.Height);
						_resized = false;
					}
				});
			}
			CalculateBorders();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_tooltip.Visible = false;
			if (_backgroundTexture != null)
			{
				spriteBatch.DrawOnCtrl(this, _backgroundTexture, bounds);
			}
			if (_showTint)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, base.ContentRegion, Color.Black * 0.4f);
			}
			if (!string.IsNullOrEmpty(_title))
			{
				spriteBatch.DrawOnCtrl(this, _texturePanelHeader, LayoutHeaderBounds);
				if (_canCollapse && _mouseOver && base.RelativeMousePosition.Y <= 36)
				{
					_tooltip.Visible = true;
					spriteBatch.DrawOnCtrl(this, _texturePanelHeaderActive, LayoutHeaderBounds);
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, _texturePanelHeader, LayoutHeaderBounds);
				}
				spriteBatch.DrawStringOnCtrl(this, _title, Control.Content.DefaultFont16, _layoutHeaderTextBounds, Color.White);
				if (TitleIcon != null)
				{
					spriteBatch.DrawOnCtrl(this, TitleIcon, _layoutHeaderIconBounds, TitleIcon.Bounds, Color.White);
				}
				if (_canCollapse)
				{
					spriteBatch.DrawOnCtrl(this, _textureAccordionArrow, _layoutAccordionArrowBounds, null, Color.White, base.ArrowRotation, _layoutAccordionArrowOrigin);
				}
			}
			if (base.ShowBorder)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, base.ContentRegion, Color.Black * (0.1f * base.AccentOpacity));
				spriteBatch.DrawOnCtrl(this, _textureLeftSideAccent, _layoutLeftAccentBounds, _layoutLeftAccentSrc, Color.Black * base.AccentOpacity, 0f, Vector2.Zero, SpriteEffects.FlipVertically);
				if (ShowRightBorder)
				{
					spriteBatch.DrawOnCtrl(this, _textureLeftSideAccent, _layoutRightAccentBounds, _layoutLeftAccentSrc, Color.Black * base.AccentOpacity, 0f, Vector2.Zero);
				}
				spriteBatch.DrawOnCtrl(this, _textureCornerAccent, _layoutTopLeftAccentBounds, _layoutCornerAccentSrc, Color.White * base.AccentOpacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally);
				spriteBatch.DrawOnCtrl(this, _textureCornerAccent, _layoutBottomRightAccentBounds, _layoutCornerAccentSrc, Color.White * base.AccentOpacity, 0f, Vector2.Zero, SpriteEffects.FlipVertically);
			}
			Color? backgroundColor = ((BackgroundHoveredColor.HasValue && base.MouseOver) ? BackgroundHoveredColor : BackgroundColor);
			if (backgroundColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backgroundBounds, Rectangle.Empty, backgroundColor.Value);
			}
			Color? backgroundImageColor = ((BackgroundImageHoveredColor.HasValue && base.MouseOver) ? BackgroundImageHoveredColor : BackgroundImageColor);
			if (BackgroundImage != null && backgroundImageColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, BackgroundImage, _backgroundBounds, TextureRectangle ?? BackgroundImage.Bounds, backgroundImageColor.Value, 0f, default(Vector2));
			}
			if (((HoveredBorderColor.HasValue && base.MouseOver) ? HoveredBorderColor : BorderColor).HasValue)
			{
				DrawBorders(spriteBatch);
			}
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}

		private void CalculateBorders()
		{
			_topBorders.Clear();
			_leftBorders.Clear();
			_bottomBorders.Clear();
			_rightBorders.Clear();
			Rectangle r = new Rectangle(-1, 0, base.Width + 2, 0);
			int strength = BorderWidth.Top;
			int fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> topBorders = _topBorders;
				r = new Rectangle(0, 0, base.Width, 1);
				topBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> topBorders2 = _topBorders;
				r = new Rectangle(r.Left + 1, r.Bottom, r.Width - 2, 1);
				topBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> topBorders3 = _topBorders;
			r = new Rectangle(r.Left + 1, r.Bottom, r.Width - 1, strength - fadeLines);
			topBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> topBorders4 = _topBorders;
				r = new Rectangle(r.Left + 1, r.Bottom, r.Width - 2, 1);
				topBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_topBorders.Add((new Rectangle(r.Left + 1, r.Bottom, r.Width - 2, 1), 0.5f));
			}
			r = new Rectangle(-1, -1, 0, base.Height + 2);
			strength = BorderWidth.Left;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> leftBorders = _leftBorders;
				r = new Rectangle(0, 0, 1, base.Height);
				leftBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> leftBorders2 = _leftBorders;
				r = new Rectangle(r.Right, r.Top + 1, 1, r.Height - 2);
				leftBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> leftBorders3 = _leftBorders;
			r = new Rectangle(r.Right, r.Top + 1, strength - fadeLines, r.Height - 2);
			leftBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> leftBorders4 = _leftBorders;
				r = new Rectangle(r.Right, r.Top + 1, 1, r.Height - 2);
				leftBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_leftBorders.Add((new Rectangle(r.Right, r.Top + 1, 1, r.Height - 2), 0.5f));
			}
			r = new Rectangle(base.Width, -1, 0, base.Height + 2);
			strength = BorderWidth.Right;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> rightBorders = _rightBorders;
				r = new Rectangle(base.Width - 1, 0, 1, base.Height);
				rightBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> rightBorders2 = _rightBorders;
				r = new Rectangle(r.Left - 1, r.Top + 1, 1, r.Height - 2);
				rightBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> rightBorders3 = _rightBorders;
			r = new Rectangle(r.Left - (strength - fadeLines), r.Top + 1, strength - fadeLines, r.Height - 2);
			rightBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> rightBorders4 = _rightBorders;
				r = new Rectangle(r.Left - 1, r.Top + 1, 1, r.Height - 2);
				rightBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_rightBorders.Add((new Rectangle(r.Left - 1, r.Top + 1, 1, r.Height - 2), 0.5f));
			}
			r = new Rectangle(-1, base.Height, base.Width + 2, 2);
			strength = BorderWidth.Bottom;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> bottomBorders = _bottomBorders;
				r = new Rectangle(0, base.Height - 1, base.Width, 1);
				bottomBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> bottomBorders2 = _bottomBorders;
				r = new Rectangle(r.Left + 1, r.Top - 1, r.Width - 2, 1);
				bottomBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> bottomBorders3 = _bottomBorders;
			r = new Rectangle(r.Left + 1, r.Top - (strength - fadeLines), r.Width - 2, strength - fadeLines);
			bottomBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> bottomBorders4 = _bottomBorders;
				r = new Rectangle(r.Left + 1, r.Top - 1, r.Width - 2, 1);
				bottomBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_bottomBorders.Add((new Rectangle(r.Left + 1, r.Top - 1, r.Width - 2, 1), 0.5f));
			}
		}

		private void DrawBorders(SpriteBatch spriteBatch)
		{
			Color? borderColor = ((HoveredBorderColor.HasValue && base.MouseOver) ? HoveredBorderColor : BorderColor);
			if (!borderColor.HasValue)
			{
				return;
			}
			foreach (var r4 in new List<(Rectangle, float)>(_topBorders))
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r4.Item1, Rectangle.Empty, borderColor.Value * r4.Item2);
			}
			foreach (var r3 in new List<(Rectangle, float)>(_leftBorders))
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r3.Item1, Rectangle.Empty, borderColor.Value * r3.Item2);
			}
			foreach (var r2 in new List<(Rectangle, float)>(_bottomBorders))
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r2.Item1, Rectangle.Empty, borderColor.Value * r2.Item2);
			}
			foreach (var r in new List<(Rectangle, float)>(_rightBorders))
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, r.Item1, Rectangle.Empty, borderColor.Value * r.Item2);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			_dragging = CanDrag && _dragging && base.MouseOver;
			if (_dragging)
			{
				base.Location = GameService.Input.Mouse.Position.Add(new Point(-_draggingStart.X, -_draggingStart.Y));
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			_dragging = false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			_dragging = CanDrag;
			_draggingStart = (_dragging ? base.RelativeMousePosition : Point.Zero);
		}

		protected override CaptureType CapturesInput()
		{
			CaptureType? capture = Capture;
			if (!capture.HasValue)
			{
				if (!CaptureInput)
				{
					return CaptureType.None;
				}
				return base.CapturesInput();
			}
			return capture.GetValueOrDefault();
		}
	}
}
