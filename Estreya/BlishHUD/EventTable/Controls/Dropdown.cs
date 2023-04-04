using System;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public class Dropdown : Control
	{
		private class DropdownPanel : FlowPanel
		{
			private const int SCROLL_CLOSE_THRESHOLD = 20;

			private Dropdown _assocDropdown;

			private int _startTop;

			private DropdownPanel(Dropdown assocDropdown)
				: this()
			{
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				_assocDropdown = assocDropdown;
				((Control)this)._size = new Point(((Control)_assocDropdown).get_Width(), ((Control)_assocDropdown).get_Height() * _assocDropdown.Items.Count);
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
				foreach (string item in _assocDropdown.Items)
				{
					DropdownPanelItem dropdownPanelItem = new DropdownPanelItem(item);
					((Control)dropdownPanelItem).set_Parent((Container)(object)this);
					((Control)dropdownPanelItem).set_Height(((Control)_assocDropdown).get_Height());
					((Control)dropdownPanelItem).set_Width(((Control)_assocDropdown).get_Width());
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

			public static DropdownPanel ShowPanel(Dropdown assocDropdown)
			{
				return new DropdownPanel(assocDropdown);
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

			private void UpdateDropdownLocation()
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

			public string Value { get; }

			public DropdownPanelItem(string value)
				: this()
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
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
				((Control)this).set_BasicTooltipText((_hoverTime > 800.0) ? Value : string.Empty);
			}

			public override void DoUpdate(GameTime gameTime)
			{
				UpdateHoverTimer(gameTime.get_ElapsedGameTime().TotalMilliseconds);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).get_MouseOver())
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, 2, base._size.X - 12 - _textureArrow.get_Width(), ((Control)this).get_Height() - 4), new Color(45, 37, 25, 255));
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Value, Control.get_Content().get_DefaultFont14(), new Rectangle(8, 0, bounds.Width - 13 - _textureArrow.get_Width(), ((Control)this).get_Height()), Colors.Chardonnay, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				else
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Value, Control.get_Content().get_DefaultFont14(), new Rectangle(8, 0, bounds.Width - 13 - _textureArrow.get_Width(), ((Control)this).get_Height()), Color.FromNonPremultiplied(239, 240, 239, 255), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		public static readonly DesignStandard Standard = new DesignStandard(new Point(250, 27), new Point(5, 2), ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset());

		private static readonly Texture2D _textureInputBox = Control.get_Content().GetTexture("input-box");

		private static readonly TextureRegion2D _textureArrow = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow");

		private static readonly TextureRegion2D _textureArrowActive = Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow-active");

		private string _selectedItem;

		private DropdownPanel _lastPanel;

		private bool _hadPanel;

		public ObservableCollection<string> Items { get; }

		public string SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Expected O, but got Unknown
				string previousValue = _selectedItem;
				if (((Control)this).SetProperty<string>(ref _selectedItem, value, false, "SelectedItem"))
				{
					OnValueChanged(new ValueChangedEventArgs(previousValue, _selectedItem));
				}
			}
		}

		public bool PanelOpen => _lastPanel != null;

		public int PanelHeight { get; set; } = -1;


		public event EventHandler<ValueChangedEventArgs> ValueChanged;

		protected virtual void OnValueChanged(ValueChangedEventArgs e)
		{
			this.ValueChanged?.Invoke(this, e);
		}

		public Dropdown()
			: this()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Items = new ObservableCollection<string>();
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
				_lastPanel = DropdownPanel.ShowPanel(this);
				if (PanelHeight != -1)
				{
					((Control)_lastPanel).set_Height(PanelHeight);
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
			if (string.IsNullOrEmpty(SelectedItem))
			{
				SelectedItem = Items.FirstOrDefault();
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
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), base._size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(_textureInputBox.get_Width() - 5, ((Control)this).get_Width() - 5), _textureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureInputBox, new Rectangle(base._size.X - 5, 0, 5, base._size.Y), (Rectangle?)new Rectangle(_textureInputBox.get_Width() - 5, 0, 5, _textureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (((Control)this).get_Enabled() && ((Control)this).get_MouseOver()) ? _textureArrowActive : _textureArrow, new Rectangle(base._size.X - _textureArrow.get_Width() - 5, base._size.Y / 2 - _textureArrow.get_Height() / 2, _textureArrow.get_Width(), _textureArrow.get_Height()));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _selectedItem, Control.get_Content().get_DefaultFont14(), new Rectangle(5, 0, base._size.X - 10 - _textureArrow.get_Width(), base._size.Y), ((Control)this).get_Enabled() ? Color.FromNonPremultiplied(239, 240, 239, 255) : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
