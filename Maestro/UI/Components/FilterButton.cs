using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Components
{
	public class FilterButton : Control
	{
		private class FilterPanel : Control
		{
			private const int ItemHeight = 24;

			private const int SeparatorHeight = 8;

			private const int PaddingX = 8;

			private static readonly string[] SourceItems = new string[3] { "All", "Bundled", "Imported" };

			private static readonly string[] InstrumentItems = new string[5] { "All", "Piano", "Harp", "Lute", "Bass" };

			private readonly FilterButton _owner;

			private int _highlightedIndex = -1;

			public FilterPanel(FilterButton owner)
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				_owner = owner;
				int height = (SourceItems.Length + InstrumentItems.Length) * 24 + 8;
				_size = new Point(_owner.Width, height);
				_location = GetPanelLocation();
				_zIndex = 2147483615;
				base.Parent = GameService.Graphics.SpriteScreen;
				Control.Input.Mouse.LeftMouseButtonPressed += OnMouseButtonPressed;
				Control.Input.Mouse.RightMouseButtonPressed += OnMouseButtonPressed;
			}

			private Point GetPanelLocation()
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				Rectangle absoluteBounds = _owner.AbsoluteBounds;
				return ((Rectangle)(ref absoluteBounds)).get_Location() + new Point(0, _owner.Height - 1);
			}

			private void OnMouseButtonPressed(object sender, MouseEventArgs e)
			{
				if (!base.MouseOver)
				{
					Dispose();
				}
			}

			protected override void OnMouseMoved(MouseEventArgs e)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_highlightedIndex = GetItemIndexAt(base.RelativeMousePosition.Y);
				base.OnMouseMoved(e);
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
				int index = GetItemIndexAt(base.RelativeMousePosition.Y);
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
				base.OnClick(e);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(Point.get_Zero(), _size), Color.get_Black());
				int y = 0;
				for (int j = 0; j < SourceItems.Length; j++)
				{
					string item = SourceItems[j];
					bool isSelected = item == _owner.SelectedSource;
					bool isHighlighted = _highlightedIndex == j;
					DrawItem(spriteBatch, item, y, isSelected, isHighlighted);
					y += 24;
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(8, y + 4 - 1, _size.X - 16, 2), MaestroTheme.MediumGray);
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
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(2, y + 2, _size.X - 4, 20), new Color(45, 37, 25, 255));
				}
				int radioX = 8;
				int radioY = y + 12 - 4;
				Color radioColor = (isSelected ? MaestroTheme.AmberGold : MaestroTheme.MediumGray);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(radioX, radioY, 8, 8), radioColor);
				if (isSelected)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(radioX + 2, radioY + 2, 4, 4), MaestroTheme.CreamWhite);
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(radioX + 2, radioY + 2, 4, 4), Color.get_Black());
				}
				Color textColor = (isHighlighted ? ContentService.Colors.Chardonnay : Color.FromNonPremultiplied(239, 240, 239, 255));
				spriteBatch.DrawStringOnCtrl(this, text, Control.Content.DefaultFont14, new Rectangle(22, y, _size.X - 8 - 14, 24), textColor);
			}

			protected override void DisposeControl()
			{
				if (_owner != null)
				{
					_owner._panel = null;
				}
				Control.Input.Mouse.LeftMouseButtonPressed -= OnMouseButtonPressed;
				Control.Input.Mouse.RightMouseButtonPressed -= OnMouseButtonPressed;
				base.DisposeControl();
			}
		}

		private static readonly Texture2D TextureInputBox = Control.Content.GetTexture("input-box");

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
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(180, 27);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
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
			_hadPanel = _mouseOver;
			_panel?.Dispose();
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
			spriteBatch.DrawOnCtrl((Control)this, TextureInputBox, RectangleExtension.Subtract(new Rectangle(Point.get_Zero(), _size), new Rectangle(0, 0, 5, 0)), (Rectangle?)new Rectangle(0, 0, Math.Min(TextureInputBox.get_Width() - 5, base.Width - 5), TextureInputBox.get_Height()));
			spriteBatch.DrawOnCtrl((Control)this, TextureInputBox, new Rectangle(_size.X - 5, 0, 5, _size.Y), (Rectangle?)new Rectangle(TextureInputBox.get_Width() - 5, 0, 5, TextureInputBox.get_Height()));
			Color arrowColor = ((base.Enabled && base.MouseOver) ? ContentService.Colors.Chardonnay : MaestroTheme.MutedCream);
			int arrowX = _size.X - 18;
			int arrowY = _size.Y / 2 - 2;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(arrowX, arrowY, 8, 2), arrowColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(arrowX + 2, arrowY + 2, 4, 2), arrowColor);
			spriteBatch.DrawStringOnCtrl(this, DisplayText, Control.Content.DefaultFont14, new Rectangle(5, 0, _size.X - 25, _size.Y), base.Enabled ? Color.FromNonPremultiplied(239, 240, 239, 255) : StandardColors.DisabledText);
		}
	}
}
