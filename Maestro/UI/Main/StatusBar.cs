using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Main
{
	public class StatusBar : Panel
	{
		private class InstrumentSelectorPanel : Control
		{
			private const int ITEM_HEIGHT = 28;

			private const int PADDING_X = 10;

			private static readonly IReadOnlyList<InstrumentInfo> Instruments = InstrumentCatalog.Pickable.ToList();

			private readonly StatusBar _owner;

			private int _highlightedIndex = -1;

			public InstrumentSelectorPanel(StatusBar owner)
				: this()
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				_owner = owner;
				int height = Instruments.Count * 28 + 8;
				base._size = new Point(150, height);
				base._location = GetPanelLocation();
				base._zIndex = 2147483615;
				((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				Control.get_Input().get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
			}

			private Point GetPanelLocation()
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				Rectangle absoluteBounds = ((Control)_owner._createButton).get_AbsoluteBounds();
				return ((Rectangle)(ref absoluteBounds)).get_Location() + new Point(0, ((Control)_owner._createButton).get_Height() - 1);
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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				int y = ((Control)this).get_RelativeMousePosition().Y - 4;
				_highlightedIndex = ((y >= 0 && y < Instruments.Count * 28) ? (y / 28) : (-1));
				((Control)this).OnMouseMoved(e);
			}

			protected override void OnClick(MouseEventArgs e)
			{
				if (_highlightedIndex >= 0 && _highlightedIndex < Instruments.Count)
				{
					InstrumentType instrument = Instruments[_highlightedIndex].Type;
					((Control)this).Dispose();
					_owner.OnInstrumentSelected(instrument);
				}
				((Control)this).OnClick(e);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(Point.get_Zero(), base._size), Color.get_Black());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, base._size.X, 1), MaestroTheme.MediumGray);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, base._size.Y - 1, base._size.X, 1), MaestroTheme.MediumGray);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 1, base._size.Y), MaestroTheme.MediumGray);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(base._size.X - 1, 0, 1, base._size.Y), MaestroTheme.MediumGray);
				int y = 4;
				for (int i = 0; i < Instruments.Count; i++)
				{
					bool num = _highlightedIndex == i;
					if (num)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, y, base._size.X - 4, 28), new Color(45, 37, 25, 255));
					}
					Color textColor = (num ? Colors.Chardonnay : Color.FromNonPremultiplied(239, 240, 239, 255));
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Instruments[i].DisplayName, Control.get_Content().get_DefaultFont14(), new Rectangle(10, y, base._size.X - 20, 28), textColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
					y += 28;
				}
			}

			protected override void DisposeControl()
			{
				if (_owner != null)
				{
					_owner._instrumentPanel = null;
				}
				Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				Control.get_Input().get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseButtonPressed);
				((Control)this).DisposeControl();
			}
		}

		public static class Layout
		{
			public static int Height => 26;
		}

		private readonly Label _statusLabel;

		private readonly StandardButton _communityButton;

		private readonly StandardButton _createButton;

		private readonly StandardButton _importButton;

		private InstrumentSelectorPanel _instrumentPanel;

		private bool _hadPanel;

		private int _visibleCount;

		private int _totalCount;

		public int VisibleCount
		{
			get
			{
				return _visibleCount;
			}
			set
			{
				_visibleCount = value;
				UpdateText();
			}
		}

		public int TotalCount
		{
			get
			{
				return _totalCount;
			}
			set
			{
				_totalCount = value;
				UpdateText();
			}
		}

		public event EventHandler ImportClicked;

		public event EventHandler CommunityClicked;

		public event EventHandler<InstrumentType> CreateClicked;

		public StatusBar(int width)
			: this()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(width, Layout.Height));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			int buttonsWidth = 130;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(width - buttonsWidth - 10);
			((Control)val).set_Height(((Control)this).get_Height());
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.LightGray);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			_statusLabel = val;
			int x = width - 40;
			IconButton iconButton = new IconButton(MaestroIcons.Import, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_Location(new Point(x, 0));
			((Control)iconButton).set_Size(new Point(40, 26));
			((Control)iconButton).set_BasicTooltipText("Import songs");
			_importButton = (StandardButton)(object)iconButton;
			((Control)_importButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ImportClicked?.Invoke(this, EventArgs.Empty);
			});
			x -= 45;
			IconButton iconButton2 = new IconButton(MaestroIcons.Create, MaestroTheme.IconGlyph);
			((Control)iconButton2).set_Parent((Container)(object)this);
			((Control)iconButton2).set_Location(new Point(x, 0));
			((Control)iconButton2).set_Size(new Point(40, 26));
			((Control)iconButton2).set_BasicTooltipText("Create a song");
			_createButton = (StandardButton)(object)iconButton2;
			((Control)_createButton).add_Click((EventHandler<MouseEventArgs>)OnCreateButtonClick);
			x -= 45;
			IconButton iconButton3 = new IconButton(MaestroIcons.Community, MaestroTheme.IconGlyph);
			((Control)iconButton3).set_Parent((Container)(object)this);
			((Control)iconButton3).set_Location(new Point(x, 0));
			((Control)iconButton3).set_Size(new Point(40, 26));
			((Control)iconButton3).set_BasicTooltipText("Browse & upload community songs");
			_communityButton = (StandardButton)(object)iconButton3;
			((Control)_communityButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.CommunityClicked?.Invoke(this, EventArgs.Empty);
			});
		}

		private void OnCreateButtonClick(object sender, MouseEventArgs e)
		{
			if (_instrumentPanel == null && !_hadPanel)
			{
				_instrumentPanel = new InstrumentSelectorPanel(this);
			}
			else
			{
				_hadPanel = false;
			}
		}

		private void OnInstrumentSelected(InstrumentType instrument)
		{
			_hadPanel = ((Control)_createButton).get_MouseOver();
			this.CreateClicked?.Invoke(this, instrument);
		}

		public void SetCreateButtonEnabled(bool enabled)
		{
			((Control)_createButton).set_Enabled(enabled);
			((Control)_createButton).set_BasicTooltipText(enabled ? "Create a song" : "Close the Creator window first");
		}

		private void UpdateText()
		{
			_statusLabel.set_Text((_visibleCount == _totalCount) ? $"  {_totalCount} songs" : $"  {_visibleCount} of {_totalCount} songs");
		}

		protected override void DisposeControl()
		{
			InstrumentSelectorPanel instrumentPanel = _instrumentPanel;
			if (instrumentPanel != null)
			{
				((Control)instrumentPanel).Dispose();
			}
			Label statusLabel = _statusLabel;
			if (statusLabel != null)
			{
				((Control)statusLabel).Dispose();
			}
			StandardButton communityButton = _communityButton;
			if (communityButton != null)
			{
				((Control)communityButton).Dispose();
			}
			StandardButton createButton = _createButton;
			if (createButton != null)
			{
				((Control)createButton).Dispose();
			}
			StandardButton importButton = _importButton;
			if (importButton != null)
			{
				((Control)importButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
