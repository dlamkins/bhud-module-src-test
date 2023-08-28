using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.Controls
{
	public class CollapseContainer : Container
	{
		protected bool _canCollapse = true;

		private Tween _collapseAnim;

		private string _title;

		protected bool _showBorder;

		protected bool _collapsed;

		private readonly AsyncTexture2D _texturePanelHeader = AsyncTexture2D.FromAssetId(1032325);

		private readonly AsyncTexture2D _texturePanelHeaderActive = AsyncTexture2D.FromAssetId(1032324);

		private readonly AsyncTexture2D _textureCornerAccent = AsyncTexture2D.FromAssetId(1002144);

		private readonly AsyncTexture2D _textureLeftSideAccent = AsyncTexture2D.FromAssetId(605025);

		private readonly AsyncTexture2D _textureAccordionArrow = AsyncTexture2D.FromAssetId(155953);

		private readonly BasicTooltip _tooltip;

		private Vector2 _layoutAccordionArrowOrigin;

		private Rectangle _layoutTopLeftAccentBounds;

		private Rectangle _layoutBottomRightAccentBounds;

		private Rectangle _layoutCornerAccentSrc;

		private Rectangle _layoutLeftAccentBounds;

		private Rectangle _layoutLeftAccentSrc;

		private Rectangle _layoutHeaderBounds;

		private Rectangle _layoutHeaderTextBounds;

		private Rectangle _layoutHeaderIconBounds;

		private Rectangle _layoutAccordionArrowBounds;

		private RectangleDimensions _contentPadding;

		private RectangleDimensions _titleIconPadding;

		private int _titleBarHeight;

		private Func<string> _setLocalizedTitleTooltip;

		private Func<string> _setLocalizedTitle;

		private Func<string> _setLocalizedTooltip;

		private Control _sizeDeterminingChild;

		private Rectangle _panelBounds;

		[JsonIgnore]
		public float ArrowRotation { get; set; }

		[JsonIgnore]
		public float AccentOpacity { get; set; }

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
				Title = value?.Invoke();
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				Common.SetProperty(ref _title, value, ((Control)this).RecalculateLayout);
			}
		}

		public AsyncTexture2D TitleIcon { get; set; }

		public Point MaxSize { get; set; }

		[JsonIgnore]
		public bool Collapsed
		{
			get
			{
				return _collapsed;
			}
			set
			{
				if (value)
				{
					Collapse();
				}
				else
				{
					Expand();
				}
			}
		}

		public bool CanCollapse
		{
			get
			{
				return _canCollapse;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canCollapse, value, true, "CanCollapse");
			}
		}

		public bool ShowBorder
		{
			get
			{
				return _showBorder;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _showBorder, value, true, "ShowBorder");
			}
		}

		public Control SizeDeterminingChild
		{
			get
			{
				return _sizeDeterminingChild;
			}
			set
			{
				Control temp = _sizeDeterminingChild;
				if (Common.SetProperty(ref _sizeDeterminingChild, value, OnChildSet) && temp != null)
				{
					temp.remove_Resized((EventHandler<ResizedEventArgs>)SizeDeterminingChild_Resized);
				}
			}
		}

		public CollapseContainer()
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			BasicTooltip basicTooltip = new BasicTooltip();
			((Control)basicTooltip).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)basicTooltip).set_ZIndex(1073741823);
			((Control)basicTooltip).set_Visible(false);
			_tooltip = basicTooltip;
			_contentPadding = new RectangleDimensions(0);
			_titleIconPadding = new RectangleDimensions(3, 3, 5, 3);
			_titleBarHeight = 36;
			AccentOpacity = 1f;
			MaxSize = Point.get_Zero();
			((Container)this)._002Ector();
		}

		public void Expand()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			if (_collapsed)
			{
				Tween collapseAnim = _collapseAnim;
				if (collapseAnim != null)
				{
					collapseAnim.CancelAndComplete();
				}
				((Control)this).SetProperty<bool>(ref _collapsed, false, false, "Expand");
				Point bounds = ((SizeDeterminingChild != null) ? ControlUtil.GetControlBounds((Control[])(object)new Control[1] { SizeDeterminingChild }) : Point.get_Zero());
				int height = ((MaxSize != Point.get_Zero()) ? Math.Min(MaxSize.Y, bounds.Y + ContentPadding.Vertical + _titleBarHeight) : (bounds.Y + ContentPadding.Vertical + _titleBarHeight));
				_collapseAnim = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<CollapseContainer>(this, (object)new
				{
					Height = height,
					ArrowRotation = 0f,
					AccentOpacity = 1f
				}, 0.15f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
			}
		}

		public void Collapse()
		{
			if (!_collapsed)
			{
				if (_collapseAnim != null && _collapseAnim.get_Completion() < 1f)
				{
					_collapseAnim.CancelAndComplete();
				}
				((Control)this).SetProperty<bool>(ref _canCollapse, true, false, "Collapse");
				((Control)this).SetProperty<bool>(ref _collapsed, true, false, "Collapse");
				_collapseAnim = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<CollapseContainer>(this, (object)new
				{
					Height = _titleBarHeight,
					ArrowRotation = -(float)Math.PI / 2f,
					AccentOpacity = 0f
				}, 0.15f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
			}
		}

		private void OnChildSet()
		{
			if (_sizeDeterminingChild != null)
			{
				_sizeDeterminingChild.add_Resized((EventHandler<ResizedEventArgs>)SizeDeterminingChild_Resized);
				_sizeDeterminingChild.add_Hidden((EventHandler<EventArgs>)SizeDeterminingChild_Hidden);
			}
		}

		private void SizeDeterminingChild_Hidden(object sender, EventArgs e)
		{
			((Control)this).Hide();
		}

		private void SizeDeterminingChild_Resized(object sender, ResizedEventArgs e)
		{
			SetHeight();
		}

		private void SetHeight()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			Point bounds = ((_sizeDeterminingChild != null) ? ControlUtil.GetControlBounds((Control[])(object)new Control[1] { _sizeDeterminingChild }) : Point.get_Zero());
			int height = ((MaxSize != Point.get_Zero()) ? Math.Min(MaxSize.Y, bounds.Y + ContentPadding.Vertical + _titleBarHeight) : (bounds.Y + ContentPadding.Vertical + _titleBarHeight));
			((Control)this).set_Height(Collapsed ? _titleBarHeight : height);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (_canCollapse && ((Rectangle)(ref _layoutHeaderBounds)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				ToggleAccordionState();
			}
			((Control)this).OnClick(e);
		}

		private bool ToggleAccordionState()
		{
			Collapsed = !_collapsed;
			return _collapsed;
		}

		public override void RecalculateLayout()
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			int num = ((!string.IsNullOrEmpty(_title) || TitleIcon != null) ? _titleBarHeight : 0);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (ShowBorder)
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
				_layoutLeftAccentSrc = new Rectangle(0, 0, _textureLeftSideAccent.get_Width(), _layoutLeftAccentBounds.Height);
			}
			_panelBounds = new Rectangle(num4, num, ((Control)this)._size.X - num4 - num2, ((Control)this)._size.Y - num - num3);
			((Container)this).set_ContentRegion(new Rectangle(num4 + ContentPadding.Left, num + ContentPadding.Top, ((Control)this)._size.X - num4 - num2 - ContentPadding.Horizontal, ((Control)this)._size.Y - num - num3 - ContentPadding.Vertical));
			_layoutHeaderBounds = new Rectangle(0, 0, ((Control)this).get_Width(), num);
			_layoutHeaderIconBounds = (Rectangle)((TitleIcon != null) ? new Rectangle(((Rectangle)(ref _layoutHeaderBounds)).get_Left() + _titleIconPadding.Left, _titleIconPadding.Top, num - _titleIconPadding.Vertical, num - _titleIconPadding.Vertical) : Rectangle.get_Empty());
			_layoutHeaderTextBounds = new Rectangle(((Rectangle)(ref _layoutHeaderIconBounds)).get_Right() + _titleIconPadding.Right, 0, _layoutHeaderBounds.Width - _layoutHeaderIconBounds.Width, num);
			int arrowSize = num - 4;
			_layoutAccordionArrowOrigin = new Vector2(16f, 16f);
			_layoutAccordionArrowBounds = RectangleExtension.OffsetBy(new Rectangle(((Rectangle)(ref _layoutHeaderBounds)).get_Right() - arrowSize, (num - arrowSize) / 2, arrowSize, arrowSize), new Point(arrowSize / 2, arrowSize / 2));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			((Control)_tooltip).set_Visible(false);
			if (!string.IsNullOrEmpty(_title))
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeader), _layoutHeaderBounds);
				if (_canCollapse && ((Control)this)._mouseOver && ((Control)this).get_RelativeMousePosition().Y <= 36)
				{
					((Control)_tooltip).set_Visible(true);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeaderActive), _layoutHeaderBounds);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeader), _layoutHeaderBounds);
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _title, Control.get_Content().get_DefaultFont16(), _layoutHeaderTextBounds, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (TitleIcon != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(TitleIcon), _layoutHeaderIconBounds, (Rectangle?)TitleIcon.get_Bounds(), Color.get_White());
				}
				if (_canCollapse)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureAccordionArrow), _layoutAccordionArrowBounds, (Rectangle?)null, Color.get_White(), ArrowRotation, _layoutAccordionArrowOrigin, (SpriteEffects)0);
				}
			}
			if (ShowBorder)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _panelBounds, Color.get_Black() * (0.1f * AccentOpacity));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureCornerAccent), _layoutTopLeftAccentBounds, (Rectangle?)_layoutCornerAccentSrc, Color.get_White() * AccentOpacity, 0f, Vector2.get_Zero(), (SpriteEffects)1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureCornerAccent), _layoutBottomRightAccentBounds, (Rectangle?)_layoutCornerAccentSrc, Color.get_White() * AccentOpacity, 0f, Vector2.get_Zero(), (SpriteEffects)2);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureLeftSideAccent), _layoutLeftAccentBounds, (Rectangle?)_layoutLeftAccentSrc, Color.get_Black() * AccentOpacity, 0f, Vector2.get_Zero(), (SpriteEffects)2);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _panelBounds, Color.get_Black() * (0.3f * AccentOpacity));
		}

		protected override void DisposeControl()
		{
			((Container)this).DisposeControl();
			_sizeDeterminingChild.remove_Resized((EventHandler<ResizedEventArgs>)SizeDeterminingChild_Resized);
			_sizeDeterminingChild = null;
		}
	}
}
