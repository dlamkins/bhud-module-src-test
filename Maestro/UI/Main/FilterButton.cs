using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Main
{
	public class FilterButton : Control
	{
		private class FilterPanel : Control
		{
			private const int ITEM_HEIGHT = 24;

			private const int SEPARATOR_HEIGHT = 8;

			private const int PADDING_X = 8;

			private static readonly string[] SourceItems = new string[3] { "All", "Bundled", "Imported" };

			private static readonly string[] InstrumentItems = new string[5] { "All", "Piano", "Harp", "Lute", "Bass" };

			private readonly FilterButton _owner;

			private int _highlightedIndex = -1;

			public FilterPanel(FilterButton owner)
				: this()
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				_owner = owner;
				int height = (SourceItems.Length + InstrumentItems.Length) * 24 + 8;
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
				int sourceHeight = SourceItems.Length * 24;
				if (y < sourceHeight)
				{
					return y / 24;
				}
				if (y < sourceHeight + 8)
				{
					return -1;
				}
				int instrumentY = y - sourceHeight - 8;
				return SourceItems.Length + instrumentY / 24;
			}

			protected override void OnClick(MouseEventArgs e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				int index = GetItemIndexAt(((Control)this).get_RelativeMousePosition().Y);
				if (index >= 0 && index < SourceItems.Length)
				{
					_owner.SelectedSource = SourceItems[index];
				}
				else if (index >= SourceItems.Length)
				{
					int instrumentIndex = index - SourceItems.Length;
					if (instrumentIndex < InstrumentItems.Length)
					{
						_owner.SelectedInstrument = InstrumentItems[instrumentIndex];
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
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), base._size), Color.get_Black());
				int y = 0;
				for (int j = 0; j < SourceItems.Length; j++)
				{
					string item = SourceItems[j];
					bool isSelected = item == _owner.SelectedSource;
					bool isHighlighted = _highlightedIndex == j;
					DrawItem(spriteBatch, item, y, isSelected, isHighlighted);
					y += 24;
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(8, y + 4 - 1, base._size.X - 16, 2), MaestroTheme.MediumGray);
				y += 8;
				for (int i = 0; i < InstrumentItems.Length; i++)
				{
					string item2 = InstrumentItems[i];
					bool isSelected2 = item2 == _owner.SelectedInstrument;
					bool isHighlighted2 = _highlightedIndex == SourceItems.Length + i;
					DrawItem(spriteBatch, item2, y, isSelected2, isHighlighted2);
					y += 24;
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

		private string _selectedSource = "All";

		private string _selectedInstrument = "All";

		public string SelectedSource
		{
			get
			{
				return _selectedSource;
			}
			set
			{
				if (_selectedSource != value)
				{
					_selectedSource = value;
					this.FilterChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public string SelectedInstrument
		{
			get
			{
				return _selectedInstrument;
			}
			set
			{
				if (_selectedInstrument != value)
				{
					_selectedInstrument = value;
					this.FilterChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public bool PanelOpen => _panel != null;

		private string DisplayText => _selectedSource + " / " + _selectedInstrument;

		public event EventHandler FilterChanged;

		public FilterButton()
			: this()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
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
