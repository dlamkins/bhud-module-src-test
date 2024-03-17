using System;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class Dropdown<TItem> : Control
	{
		private class DropdownPanel : FlowPanel
		{
			private const int SCROLL_CLOSE_THRESHOLD = 20;

			private readonly int _startTop;

			private Dropdown<TItem> _assocDropdown;

			private DropdownPanel(Dropdown<TItem> assocDropdown, int panelHeight = -1)
				: this()
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				_assocDropdown = assocDropdown;
				((Control)this)._size = new Point(((Control)_assocDropdown).get_Width(), (panelHeight != -1) ? panelHeight : (((Control)_assocDropdown).get_Height() * _assocDropdown.Items.Count));
				((Control)this)._location = GetPanelLocation();
				((Control)this)._zIndex = 2147483615;
				((Control)this).set_BackgroundColor(Color.get_Black());
				((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
				_startTop = ((Control)this)._location.Y;
				((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
				Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				Control.get_Input().get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				AddItems();
			}

			private void AddItems()
			{
				foreach (TItem item in _assocDropdown.Items)
				{
					DropdownPanelItem dropdownPanelItem = new DropdownPanelItem(item);
					((Control)dropdownPanelItem).set_Parent((Container)(object)this);
					((Control)dropdownPanelItem).set_Height((_assocDropdown.ItemHeight == -1) ? ((Control)_assocDropdown).get_Height() : _assocDropdown.ItemHeight);
					((Control)dropdownPanelItem).set_Width(((Control)_assocDropdown).get_Width());
					dropdownPanelItem.Font = _assocDropdown.Font;
					((Control)dropdownPanelItem).add_Click((EventHandler<MouseEventArgs>)DropdownPanelItem_Click);
				}
			}

			private void DropdownPanelItem_Click(object sender, MouseEventArgs e)
			{
				DropdownPanelItem panelItem = sender as DropdownPanelItem;
				if (panelItem != null)
				{
					_assocDropdown.SelectedItem = panelItem.Value;
					((Control)this).Dispose();
				}
			}

			private Point GetPanelLocation()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				Rectangle absoluteBounds = ((Control)_assocDropdown).get_AbsoluteBounds();
				Point dropdownLocation = ((Rectangle)(ref absoluteBounds)).get_Location();
				int yUnderDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Bottom() - (dropdownLocation.Y + ((Control)_assocDropdown).get_Height() + ((Control)this)._size.Y);
				int yAboveDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Top() + (dropdownLocation.Y - ((Control)this)._size.Y);
				if (yUnderDef <= 0 && yUnderDef <= yAboveDef)
				{
					return dropdownLocation - new Point(0, ((Control)this)._size.Y + 1);
				}
				return dropdownLocation + new Point(0, ((Control)_assocDropdown).get_Height() - 1);
			}

			public static DropdownPanel ShowPanel(Dropdown<TItem> assocDropdown, int panelHeight = -1)
			{
				return new DropdownPanel(assocDropdown, panelHeight);
			}

			private void InputOnMousedOffDropdownPanel(object sender, MouseEventArgs e)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Invalid comparison between Unknown and I4
				if (!((Control)this).get_MouseOver())
				{
					if ((int)e.get_EventType() == 516)
					{
						_assocDropdown.HideDropdownPanelWithoutDebounce();
					}
					else
					{
						_assocDropdown.HideDropdownPanel();
					}
				}
			}

			public void UpdateDropdownLocation()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this)._location = GetPanelLocation();
				if (Math.Abs(((Control)this)._location.Y - _startTop) > 20)
				{
					((Control)this).Dispose();
				}
			}

			public override void UpdateContainer(GameTime gameTime)
			{
				UpdateDropdownLocation();
			}

			protected override void DisposeControl()
			{
				((Container)this).get_Children()?.ToList().ForEach(delegate(Control child)
				{
					DropdownPanelItem dropdownPanelItem = child as DropdownPanelItem;
					if (dropdownPanelItem != null)
					{
						((Control)dropdownPanelItem).remove_Click((EventHandler<MouseEventArgs>)DropdownPanelItem_Click);
					}
					if (child != null)
					{
						child.Dispose();
					}
				});
				if (_assocDropdown != null)
				{
					_assocDropdown._lastPanel = null;
					_assocDropdown = null;
				}
				Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				Control.get_Input().get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)InputOnMousedOffDropdownPanel);
				((FlowPanel)this).DisposeControl();
			}
		}

		private class DropdownPanelItem : Control
		{
			private const int TOOLTIP_HOVER_DELAY = 800;

			private double _hoverTime;

			public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


			public TItem Value { get; }

			public DropdownPanelItem(TItem value)
				: this()
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				Value = value;
				((Control)this).set_BackgroundColor(Color.get_Black());
			}

			private void UpdateHoverTimer(double elapsedMilliseconds)
			{
				if (base._mouseOver)
				{
					_hoverTime += elapsedMilliseconds;
				}
				else
				{
					_hoverTime = 0.0;
				}
				object basicTooltipText;
				if (!(_hoverTime > 800.0))
				{
					basicTooltipText = string.Empty;
				}
				else
				{
					TItem value = Value;
					basicTooltipText = ((value != null) ? value.ToString() : null);
				}
				((Control)this).set_BasicTooltipText((string)basicTooltipText);
			}

			public override void DoUpdate(GameTime gameTime)
			{
				UpdateHoverTimer(gameTime.get_ElapsedGameTime().TotalMilliseconds);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).get_MouseOver())
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, 2, base._size.X - 12 - Dropdown<TItem>._textureArrow.get_Width(), ((Control)this).get_Height() - 4), new Color(45, 37, 25, 255));
					TItem value = Value;
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, (value != null) ? value.ToString() : null, Font, new Rectangle(8, 0, bounds.Width - 13 - Dropdown<TItem>._textureArrow.get_Width(), ((Control)this).get_Height()), Colors.Chardonnay, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				else
				{
					TItem value = Value;
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, (value != null) ? value.ToString() : null, Font, new Rectangle(8, 0, bounds.Width - 13 - Dropdown<TItem>._textureArrow.get_Width(), ((Control)this).get_Height()), Color.FromNonPremultiplied(239, 240, 239, 255), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		public static readonly DesignStandard Standard = new DesignStandard(new Point(250, 27), new Point(5, 2), ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset());

		private bool _hadPanel;

		private DropdownPanel _lastPanel;

		private TItem _selectedItem;

		private static readonly Texture2D _textureInputBox = Control.get_Content().GetTexture("input-box");

		private static readonly TextureRegion2D _textureArrow = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow");

		private static readonly TextureRegion2D _textureArrowActive = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow-active");

		public ObservableCollection<TItem> Items { get; }

		public TItem SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				TItem previousValue = _selectedItem;
				if (((Control)this).SetProperty<TItem>(ref _selectedItem, value, false, "SelectedItem"))
				{
					OnValueChanged(new ValueChangedEventArgs<TItem>(previousValue, _selectedItem));
				}
			}
		}

		public bool PanelOpen => _lastPanel != null;

		public int PanelHeight { get; set; } = -1;


		public int ItemHeight { get; set; } = -1;


		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


		public event EventHandler<ValueChangedEventArgs<TItem>> ValueChanged;

		public Dropdown()
			: this()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Items = new ObservableCollection<TItem>();
			Items.CollectionChanged += delegate
			{
				ItemsUpdated();
				((Control)this).Invalidate();
			};
			((Control)this).set_Size(((DesignStandard)(ref Standard)).get_Size());
		}

		public void HideDropdownPanel()
		{
			_hadPanel = base._mouseOver;
			DropdownPanel lastPanel = _lastPanel;
			if (lastPanel != null)
			{
				((Control)lastPanel).Dispose();
			}
		}

		private void HideDropdownPanelWithoutDebounce()
		{
			HideDropdownPanel();
			_hadPanel = false;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			if (_lastPanel == null && !_hadPanel)
			{
				_lastPanel = DropdownPanel.ShowPanel(this, Math.Min(PanelHeight, Items.Sum((TItem x) => ((Control)this).get_Height())));
				if (PanelHeight != -1)
				{
					((Panel)_lastPanel).set_CanScroll(true);
				}
			}
			else
			{
				_hadPanel = false;
			}
		}

		private void ItemsUpdated()
		{
			TItem selectedItem = SelectedItem;
			if (selectedItem == null)
			{
				TItem val2 = (SelectedItem = Items.FirstOrDefault());
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), base._size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(_textureInputBox.get_Width() - 5, ((Control)this).get_Width() - 5), _textureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, new Rectangle(base._size.X - 5, 0, 5, base._size.Y), (Rectangle?)new Rectangle(_textureInputBox.get_Width() - 5, 0, 5, _textureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (((Control)this).get_Enabled() && ((Control)this).get_MouseOver()) ? _textureArrowActive : _textureArrow, new Rectangle(base._size.X - _textureArrow.get_Width() - 5, base._size.Y / 2 - _textureArrow.get_Height() / 2, _textureArrow.get_Width(), _textureArrow.get_Height()));
			TItem selectedItem = SelectedItem;
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, (selectedItem != null) ? selectedItem.ToString() : null, Font, new Rectangle(5, 0, base._size.X - 10 - _textureArrow.get_Width(), base._size.Y), ((Control)this).get_Enabled() ? Color.FromNonPremultiplied(239, 240, 239, 255) : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		protected virtual void OnValueChanged(ValueChangedEventArgs<TItem> e)
		{
			this.ValueChanged?.Invoke(this, e);
		}
	}
}
