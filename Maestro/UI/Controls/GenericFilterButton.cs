using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Controls
{
	public class GenericFilterButton : Control
	{
		private class FilterPanel : Control
		{
			private const int ITEM_HEIGHT = 24;

			private const int SEPARATOR_HEIGHT = 8;

			private const int PADDING_X = 8;

			private readonly GenericFilterButton _owner;

			private int _highlightedIndex = -1;

			public FilterPanel(GenericFilterButton owner)
				: this()
			{
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				_owner = owner;
				int totalItems = _owner._section1.Items.Length + _owner._section2.Items.Length;
				int separatorCount = 1;
				if (_owner._section3 != null)
				{
					totalItems += _owner._section3.Items.Length;
					separatorCount = 2;
				}
				int height = totalItems * 24 + separatorCount * 8;
				base._size = new Point(((Control)_owner).get_Width(), height);
				base._location = GetPanelLocation();
				base._zIndex = 2147483615;
				((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				Control.get_Input().get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
			}

			private Point GetPanelLocation()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				Rectangle absoluteBounds = ((Control)_owner).get_AbsoluteBounds();
				return ((Rectangle)(ref absoluteBounds)).get_Location() + new Point(0, ((Control)_owner).get_Height() - 1);
			}

			private void OnMouseButtonPressed(object sender, MouseEventArgs e)
			{
				if (!((Control)this).get_MouseOver())
				{
					((Control)this).Dispose();
				}
			}

			protected override void OnMouseMoved(MouseEventArgs e)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_highlightedIndex = GetItemIndexAt(((Control)this).get_RelativeMousePosition().Y);
				((Control)this).OnMouseMoved(e);
			}

			private int GetItemIndexAt(int y)
			{
				int section1Height = _owner._section1.Items.Length * 24;
				if (y < section1Height)
				{
					return y / 24;
				}
				if (y < section1Height + 8)
				{
					return -1;
				}
				int section2Start = section1Height + 8;
				int section2Height = _owner._section2.Items.Length * 24;
				if (y < section2Start + section2Height)
				{
					int section2Y = y - section2Start;
					return _owner._section1.Items.Length + section2Y / 24;
				}
				if (_owner._section3 == null)
				{
					return -1;
				}
				int section3Start = section2Start + section2Height + 8;
				if (y < section2Start + section2Height + 8)
				{
					return -1;
				}
				int section3Y = y - section3Start;
				return _owner._section1.Items.Length + _owner._section2.Items.Length + section3Y / 24;
			}

			protected override void OnClick(MouseEventArgs e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				int index = GetItemIndexAt(((Control)this).get_RelativeMousePosition().Y);
				if (index >= 0 && index < _owner._section1.Items.Length)
				{
					_owner.SelectedValue1 = _owner._section1.Items[index];
				}
				else if (index >= _owner._section1.Items.Length && index < _owner._section1.Items.Length + _owner._section2.Items.Length)
				{
					int section2Index = index - _owner._section1.Items.Length;
					_owner.SelectedValue2 = _owner._section2.Items[section2Index];
				}
				else if (_owner._section3 != null && index >= _owner._section1.Items.Length + _owner._section2.Items.Length)
				{
					int section3Index = index - _owner._section1.Items.Length - _owner._section2.Items.Length;
					if (section3Index < _owner._section3.Items.Length)
					{
						_owner.SelectedValue3 = _owner._section3.Items[section3Index];
					}
				}
				((Control)this).OnClick(e);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_0158: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), base._size), Color.get_Black());
				int y = 0;
				for (int k = 0; k < _owner._section1.Items.Length; k++)
				{
					string item = _owner._section1.Items[k];
					bool isSelected = item == _owner.SelectedValue1;
					bool isHighlighted = _highlightedIndex == k;
					DrawItem(spriteBatch, item, y, isSelected, isHighlighted);
					y += 24;
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(8, y + 4 - 1, base._size.X - 16, 2), MaestroTheme.MediumGray);
				y += 8;
				for (int j = 0; j < _owner._section2.Items.Length; j++)
				{
					string item2 = _owner._section2.Items[j];
					bool isSelected2 = item2 == _owner.SelectedValue2;
					bool isHighlighted2 = _highlightedIndex == _owner._section1.Items.Length + j;
					DrawItem(spriteBatch, item2, y, isSelected2, isHighlighted2);
					y += 24;
				}
				if (_owner._section3 != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(8, y + 4 - 1, base._size.X - 16, 2), MaestroTheme.MediumGray);
					y += 8;
					for (int i = 0; i < _owner._section3.Items.Length; i++)
					{
						string item3 = _owner._section3.Items[i];
						bool isSelected3 = item3 == _owner.SelectedValue3;
						bool isHighlighted3 = _highlightedIndex == _owner._section1.Items.Length + _owner._section2.Items.Length + i;
						DrawItem(spriteBatch, item3, y, isSelected3, isHighlighted3);
						y += 24;
					}
				}
			}

			private void DrawItem(SpriteBatch spriteBatch, string text, int y, bool isSelected, bool isHighlighted)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				if (isHighlighted)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, y + 2, base._size.X - 4, 20), new Color(45, 37, 25, 255));
				}
				int radioX = 8;
				int radioY = y + 12 - 4;
				Color radioColor = (isSelected ? MaestroTheme.AmberGold : MaestroTheme.MediumGray);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(radioX, radioY, 8, 8), radioColor);
				if (isSelected)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(radioX + 2, radioY + 2, 4, 4), MaestroTheme.CreamWhite);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(radioX + 2, radioY + 2, 4, 4), Color.get_Black());
				}
				Color textColor = (isHighlighted ? Colors.Chardonnay : Color.FromNonPremultiplied(239, 240, 239, 255));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Control.get_Content().get_DefaultFont14(), new Rectangle(22, y, base._size.X - 8 - 14, 24), textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}

			protected override void DisposeControl()
			{
				if (_owner != null)
				{
					_owner._panel = null;
				}
				Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				Control.get_Input().get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				((Control)this).DisposeControl();
			}
		}

		private static readonly Texture2D TextureInputBox = Control.get_Content().GetTexture("input-box");

		private FilterPanel _panel;

		private bool _hadPanel;

		private readonly FilterSection _section1;

		private readonly FilterSection _section2;

		private readonly FilterSection _section3;

		private string _selectedValue1;

		private string _selectedValue2;

		private string _selectedValue3;

		public string SelectedValue1
		{
			get
			{
				return _selectedValue1;
			}
			set
			{
				if (_selectedValue1 != value)
				{
					_selectedValue1 = value;
					this.FilterChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public string SelectedValue2
		{
			get
			{
				return _selectedValue2;
			}
			set
			{
				if (_selectedValue2 != value)
				{
					_selectedValue2 = value;
					this.FilterChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public string SelectedValue3
		{
			get
			{
				return _selectedValue3;
			}
			set
			{
				if (_selectedValue3 != value)
				{
					_selectedValue3 = value;
					this.FilterChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public bool PanelOpen => _panel != null;

		private string DisplayText
		{
			get
			{
				if (_section3 == null)
				{
					return _selectedValue1 + " / " + _selectedValue2;
				}
				return _selectedValue1 + " / " + _selectedValue2 + " / " + _selectedValue3;
			}
		}

		public event EventHandler FilterChanged;

		public GenericFilterButton(FilterSection section1, FilterSection section2, FilterSection section3 = null)
			: this()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			_section1 = section1;
			_section2 = section2;
			_section3 = section3;
			_selectedValue1 = section1.DefaultValue;
			_selectedValue2 = section2.DefaultValue;
			_selectedValue3 = section3?.DefaultValue;
			((Control)this).set_Size(new Point(180, 27));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			if (_panel == null && !_hadPanel)
			{
				_panel = new FilterPanel(this);
			}
			else
			{
				_hadPanel = false;
			}
		}

		public void HidePanel()
		{
			_hadPanel = base._mouseOver;
			FilterPanel panel = _panel;
			if (panel != null)
			{
				((Control)panel).Dispose();
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
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TextureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), base._size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(TextureInputBox.get_Width() - 5, ((Control)this).get_Width() - 5), TextureInputBox.get_Height()));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, TextureInputBox, new Rectangle(base._size.X - 5, 0, 5, base._size.Y), (Rectangle?)new Rectangle(TextureInputBox.get_Width() - 5, 0, 5, TextureInputBox.get_Height()));
			Color arrowColor = ((((Control)this).get_Enabled() && ((Control)this).get_MouseOver()) ? Colors.Chardonnay : MaestroTheme.MutedCream);
			int arrowX = base._size.X - 18;
			int arrowY = base._size.Y / 2 - 2;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(arrowX, arrowY, 8, 2), arrowColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(arrowX + 2, arrowY + 2, 4, 2), arrowColor);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, DisplayText, Control.get_Content().get_DefaultFont14(), new Rectangle(5, 0, base._size.X - 25, base._size.Y), ((Control)this).get_Enabled() ? Color.FromNonPremultiplied(239, 240, 239, 255) : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
