using System;
using System.Collections.Generic;
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
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Controls
{
	public class FlowPanel : FlowPanel, ILocalizable
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

		private readonly BasicTooltip _tooltip;

		private Func<string> _setLocalizedTitleTooltip;

		private Func<string> _setLocalizedTooltip;

		private Func<string> _setLocalizedTitle;

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

		private RectangleDimensions _contentPadding;

		private RectangleDimensions _borderWidth;

		private Rectangle _backgroundBounds;

		private RectangleDimensions _titleIconPadding;

		private int _titleBarHeight;

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
				((Control)this).RecalculateLayout();
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
				((Control)this).RecalculateLayout();
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
				((Control)this).RecalculateLayout();
			}
		}

		public int TitleBarHeight
		{
			get
			{
				return _titleBarHeight;
			}
			set
			{
				_titleBarHeight = value;
				((Control)this).RecalculateLayout();
			}
		}

		public Color? BorderColor { get; set; }

		public Color? HoveredBorderColor { get; set; }

		public AsyncTexture2D BackgroundImage { get; set; }

		public AsyncTexture2D TitleIcon { get; set; }

		public Color? BackgroundImageColor { get; set; }

		public Color? BackgroundImageHoveredColor { get; set; }

		public Color? BackgroundColor { get; set; }

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
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				((Control)this).set_BasicTooltipText(value?.Invoke());
			}
		}

		public Func<string> SetLocalizedTitleTooltip
		{
			get
			{
				return _setLocalizedTitleTooltip;
			}
			set
			{
				_setLocalizedTitleTooltip = value;
				TitleTooltipText = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedTitle
		{
			get
			{
				return _setLocalizedTitle;
			}
			set
			{
				_setLocalizedTitle = value;
				((Panel)this).set_Title(value?.Invoke());
			}
		}

		public Action OnCollapse { get; set; }

		public Action OnExpand { get; set; }

		public bool CaptureInput { get; set; }

		public CaptureType? Capture { get; set; }

		public bool CanDrag { get; set; }

		public FlowPanel()
		{
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			BasicTooltip basicTooltip = new BasicTooltip();
			((Control)basicTooltip).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)basicTooltip).set_ZIndex(1073741823);
			((Control)basicTooltip).set_Visible(false);
			_tooltip = basicTooltip;
			_contentPadding = new RectangleDimensions(0);
			_borderWidth = new RectangleDimensions(0);
			_titleIconPadding = new RectangleDimensions(3, 3, 5, 3);
			_titleBarHeight = 36;
			BackgroundImageColor = Color.get_White();
			CaptureInput = true;
			((FlowPanel)this)._002Ector();
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			bool collapsed = ((Panel)this).get_Collapsed();
			((Panel)this).OnClick(e);
			if (collapsed != ((Panel)this).get_Collapsed())
			{
				if (((Panel)this).get_Collapsed())
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
				MouseState state = GameService.Input.get_Mouse().get_State();
				((MouseState)(ref state)).get_LeftButton();
				_ = 1;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			_backgroundBounds = new Rectangle(Math.Max(BorderWidth.Left - 2, 0), Math.Max(BorderWidth.Top - 2, 0), ((Control)this).get_Width() - Math.Max(BorderWidth.Horizontal - 4, 0), ((Control)this).get_Height() - Math.Max(BorderWidth.Vertical - 4, 0));
			int num = ((!string.IsNullOrEmpty(((Panel)this)._title)) ? _titleBarHeight : 0);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (((Panel)this).get_ShowBorder())
			{
				num = Math.Max(7, num);
				num2 = 4;
				num3 = 7;
				num4 = 4;
				int num5 = Math.Min(((Control)this)._size.X, 256);
				_layoutTopLeftAccentBounds = new Rectangle(-2, num - 12, num5, _textureCornerAccent.get_Height());
				_layoutBottomRightAccentBounds = new Rectangle(((Control)this)._size.X - num5 + 2, ((Control)this)._size.Y - 59, num5, _textureCornerAccent.get_Height());
				_layoutCornerAccentSrc = new Rectangle(256 - num5, 0, num5, _textureCornerAccent.get_Height());
				_layoutLeftAccentBounds = new Rectangle(num4 - 7, num, _textureLeftSideAccent.get_Width(), Math.Min(((Control)this)._size.Y - num - num3, _textureLeftSideAccent.get_Height()));
				_layoutRightAccentBounds = new Rectangle(((Control)this)._size.X - 12, Math.Max(0, ((Control)this)._size.Y - _layoutLeftAccentBounds.Height), _textureLeftSideAccent.get_Width(), Math.Min(((Control)this)._size.Y - num - num3 - 10, _textureLeftSideAccent.get_Height() - 10));
				_layoutLeftAccentSrc = new Rectangle(0, 0, _textureLeftSideAccent.get_Width(), _layoutLeftAccentBounds.Height);
			}
			((Container)this).set_ContentRegion(new Rectangle(_contentPadding.Left + num4, _contentPadding.Top + num, ((Control)this)._size.X - num4 - num2 - _contentPadding.Horizontal, ((Control)this)._size.Y - num - num3 - _contentPadding.Vertical));
			LayoutHeaderBounds = new Rectangle(num4, 0, ((Control)this).get_Width(), num);
			_layoutHeaderIconBounds = (Rectangle)((TitleIcon != null) ? new Rectangle(((Rectangle)(ref LayoutHeaderBounds)).get_Left() + _titleIconPadding.Left, _titleIconPadding.Top, num - _titleIconPadding.Vertical, num - _titleIconPadding.Vertical) : Rectangle.get_Empty());
			_layoutHeaderTextBounds = new Rectangle(((Rectangle)(ref _layoutHeaderIconBounds)).get_Right() + _titleIconPadding.Right, 0, LayoutHeaderBounds.Width - _layoutHeaderIconBounds.Width, num);
			int arrowSize = num - 4;
			_layoutAccordionArrowOrigin = new Vector2(16f, 16f);
			_layoutAccordionArrowBounds = RectangleExtension.OffsetBy(new Rectangle(((Rectangle)(ref LayoutHeaderBounds)).get_Right() - arrowSize, (num - arrowSize) / 2, arrowSize, arrowSize), new Point(arrowSize / 2, arrowSize / 2));
			if (((Panel)this).get_Collapsed() && ((Control)this)._size.Y > LayoutHeaderBounds.Height && !_resized)
			{
				_resized = true;
				Task.Run(async delegate
				{
					await Task.Delay(125);
					if (!((Panel)this).get_Collapsed())
					{
						_resized = false;
					}
					else
					{
						((Control)this).set_Size(new Point(((Control)this).get_Width(), LayoutHeaderBounds.Height));
						_resized = false;
					}
				});
			}
			CalculateBorders();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			((Control)_tooltip).set_Visible(false);
			if (((Panel)this)._backgroundTexture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Panel)this)._backgroundTexture), bounds);
			}
			if (((Panel)this)._showTint)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), ((Container)this).get_ContentRegion(), Color.get_Black() * 0.4f);
			}
			if (!string.IsNullOrEmpty(((Panel)this)._title))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeader), LayoutHeaderBounds);
				if (((Panel)this)._canCollapse && ((Control)this)._mouseOver && ((Control)this).get_RelativeMousePosition().Y <= 36)
				{
					((Control)_tooltip).set_Visible(true);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeaderActive), LayoutHeaderBounds);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeader), LayoutHeaderBounds);
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((Panel)this)._title, Control.get_Content().get_DefaultFont16(), _layoutHeaderTextBounds, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (TitleIcon != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(TitleIcon), _layoutHeaderIconBounds, (Rectangle?)TitleIcon.get_Bounds(), Color.get_White());
				}
				if (((Panel)this)._canCollapse)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureAccordionArrow), _layoutAccordionArrowBounds, (Rectangle?)null, Color.get_White(), ((Panel)this).get_ArrowRotation(), _layoutAccordionArrowOrigin, (SpriteEffects)0);
				}
			}
			if (((Panel)this).get_ShowBorder())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), ((Container)this).get_ContentRegion(), Color.get_Black() * (0.1f * ((Panel)this).get_AccentOpacity()));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureLeftSideAccent), _layoutLeftAccentBounds, (Rectangle?)_layoutLeftAccentSrc, Color.get_Black() * ((Panel)this).get_AccentOpacity(), 0f, Vector2.get_Zero(), (SpriteEffects)2);
				if (ShowRightBorder)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureLeftSideAccent), _layoutRightAccentBounds, (Rectangle?)_layoutLeftAccentSrc, Color.get_Black() * ((Panel)this).get_AccentOpacity(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureCornerAccent), _layoutTopLeftAccentBounds, (Rectangle?)_layoutCornerAccentSrc, Color.get_White() * ((Panel)this).get_AccentOpacity(), 0f, Vector2.get_Zero(), (SpriteEffects)1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureCornerAccent), _layoutBottomRightAccentBounds, (Rectangle?)_layoutCornerAccentSrc, Color.get_White() * ((Panel)this).get_AccentOpacity(), 0f, Vector2.get_Zero(), (SpriteEffects)2);
			}
			Color? backgroundColor = ((BackgroundHoveredColor.HasValue && ((Control)this).get_MouseOver()) ? BackgroundHoveredColor : BackgroundColor);
			if (backgroundColor.HasValue)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _backgroundBounds, (Rectangle?)Rectangle.get_Empty(), backgroundColor.Value);
			}
			Color? backgroundImageColor = ((BackgroundImageHoveredColor.HasValue && ((Control)this).get_MouseOver()) ? BackgroundImageHoveredColor : BackgroundImageColor);
			if (BackgroundImage != null && backgroundImageColor.HasValue)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(BackgroundImage), _backgroundBounds, (Rectangle?)(Rectangle)(((_003F?)TextureRectangle) ?? BackgroundImage.get_Bounds()), backgroundImageColor.Value, 0f, default(Vector2), (SpriteEffects)0);
			}
			if (((HoveredBorderColor.HasValue && ((Control)this).get_MouseOver()) ? HoveredBorderColor : BorderColor).HasValue)
			{
				DrawBorders(spriteBatch);
			}
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
			}
		}

		protected override void DisposeControl()
		{
			((FlowPanel)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}

		private void CalculateBorders()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			_topBorders.Clear();
			_leftBorders.Clear();
			_bottomBorders.Clear();
			_rightBorders.Clear();
			Rectangle r = default(Rectangle);
			((Rectangle)(ref r))._002Ector(-1, 0, ((Control)this).get_Width() + 2, 0);
			int strength = BorderWidth.Top;
			int fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> topBorders = _topBorders;
				((Rectangle)(ref r))._002Ector(0, 0, ((Control)this).get_Width(), 1);
				topBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> topBorders2 = _topBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 2, 1);
				topBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> topBorders3 = _topBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 1, strength - fadeLines);
			topBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> topBorders4 = _topBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 2, 1);
				topBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_topBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Bottom(), r.Width - 2, 1), 0.5f));
			}
			((Rectangle)(ref r))._002Ector(-1, -1, 0, ((Control)this).get_Height() + 2);
			strength = BorderWidth.Left;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> leftBorders = _leftBorders;
				((Rectangle)(ref r))._002Ector(0, 0, 1, ((Control)this).get_Height());
				leftBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> leftBorders2 = _leftBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				leftBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> leftBorders3 = _leftBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, strength - fadeLines, r.Height - 2);
			leftBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> leftBorders4 = _leftBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				leftBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_leftBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Right(), ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2), 0.5f));
			}
			((Rectangle)(ref r))._002Ector(((Control)this).get_Width(), -1, 0, ((Control)this).get_Height() + 2);
			strength = BorderWidth.Right;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> rightBorders = _rightBorders;
				((Rectangle)(ref r))._002Ector(((Control)this).get_Width() - 1, 0, 1, ((Control)this).get_Height());
				rightBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> rightBorders2 = _rightBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() - 1, ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				rightBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> rightBorders3 = _rightBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() - (strength - fadeLines), ((Rectangle)(ref r)).get_Top() + 1, strength - fadeLines, r.Height - 2);
			rightBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> rightBorders4 = _rightBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() - 1, ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2);
				rightBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_rightBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Left() - 1, ((Rectangle)(ref r)).get_Top() + 1, 1, r.Height - 2), 0.5f));
			}
			((Rectangle)(ref r))._002Ector(-1, ((Control)this).get_Height(), ((Control)this).get_Width() + 2, 2);
			strength = BorderWidth.Bottom;
			fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			if (fadeLines >= 1)
			{
				List<(Rectangle, float)> bottomBorders = _bottomBorders;
				((Rectangle)(ref r))._002Ector(0, ((Control)this).get_Height() - 1, ((Control)this).get_Width(), 1);
				bottomBorders.Add((r, 0.5f));
			}
			if (fadeLines >= 3)
			{
				List<(Rectangle, float)> bottomBorders2 = _bottomBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - 1, r.Width - 2, 1);
				bottomBorders2.Add((r, 0.7f));
			}
			List<(Rectangle, float)> bottomBorders3 = _bottomBorders;
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - (strength - fadeLines), r.Width - 2, strength - fadeLines);
			bottomBorders3.Add((r, 1f));
			if (fadeLines >= 4)
			{
				List<(Rectangle, float)> bottomBorders4 = _bottomBorders;
				((Rectangle)(ref r))._002Ector(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - 1, r.Width - 2, 1);
				bottomBorders4.Add((r, 0.7f));
			}
			if (fadeLines >= 2)
			{
				_bottomBorders.Add((new Rectangle(((Rectangle)(ref r)).get_Left() + 1, ((Rectangle)(ref r)).get_Top() - 1, r.Width - 2, 1), 0.5f));
			}
		}

		private void DrawBorders(SpriteBatch spriteBatch)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			Color? borderColor = ((HoveredBorderColor.HasValue && ((Control)this).get_MouseOver()) ? HoveredBorderColor : BorderColor);
			if (!borderColor.HasValue)
			{
				return;
			}
			foreach (var r4 in new List<(Rectangle, float)>(_topBorders))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), r4.Item1, (Rectangle?)Rectangle.get_Empty(), borderColor.Value * r4.Item2);
			}
			foreach (var r3 in new List<(Rectangle, float)>(_leftBorders))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), r3.Item1, (Rectangle?)Rectangle.get_Empty(), borderColor.Value * r3.Item2);
			}
			foreach (var r2 in new List<(Rectangle, float)>(_bottomBorders))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), r2.Item1, (Rectangle?)Rectangle.get_Empty(), borderColor.Value * r2.Item2);
			}
			foreach (var r in new List<(Rectangle, float)>(_rightBorders))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), r.Item1, (Rectangle?)Rectangle.get_Empty(), borderColor.Value * r.Item2);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			_dragging = CanDrag && _dragging && ((Control)this).get_MouseOver();
			if (_dragging)
			{
				((Control)this).set_Location(GameService.Input.get_Mouse().get_Position().Add(new Point(-_draggingStart.X, -_draggingStart.Y)));
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			_dragging = false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			_dragging = CanDrag;
			_draggingStart = (_dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
		}

		protected override CaptureType CapturesInput()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			CaptureType? capture = Capture;
			if (!capture.HasValue)
			{
				if (!CaptureInput)
				{
					return (CaptureType)0;
				}
				return ((Container)this).CapturesInput();
			}
			return capture.GetValueOrDefault();
		}
	}
}
