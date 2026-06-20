using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Maestro.UI.Playlist
{
	public class QueueSongCard : Control
	{
		public static class Layout
		{
			public const int Height = 40;

			public const int DragHandleX = 12;

			public const int TitleX = 32;

			public const int TitleY = 4;

			public const int ArtistY = 21;

			public const int RemoveButtonSize = 24;

			public const int RemoveButtonMargin = 22;
		}

		private readonly Song _song;

		private readonly int _index;

		private bool _isDragging;

		private bool _isHoveringRemove;

		private bool _isHoveringDragHandle;

		private Rectangle _dragHandleBounds;

		private Rectangle _removeButtonBounds;

		private static readonly Color CurrentHighlight = new Color(74, 62, 38);

		private bool _isCurrent;

		public Song Song => _song;

		public int Index => _index;

		public bool IsDragging => _isDragging;

		public bool IsGhost { get; set; }

		public bool IsCurrent
		{
			get
			{
				return _isCurrent;
			}
			set
			{
				if (_isCurrent != value)
				{
					_isCurrent = value;
					((Control)this).Invalidate();
				}
			}
		}

		public event EventHandler RemoveRequested;

		public event EventHandler DragStarted;

		public event EventHandler DragEnded;

		public void EndDrag()
		{
			_isDragging = false;
			_isHoveringDragHandle = false;
		}

		public QueueSongCard(Song song, int index, int width)
			: this()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			_song = song;
			_index = index;
			base._size = new Point(width, 40);
			_dragHandleBounds = new Rectangle(12, 12, 16, 16);
			_removeButtonBounds = new Rectangle(width - 24 - 22, 8, 24, 24);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Point pos = ((Control)this).get_RelativeMousePosition();
			_isHoveringDragHandle = ((Rectangle)(ref _dragHandleBounds)).Contains(pos);
			_isHoveringRemove = ((Rectangle)(ref _removeButtonBounds)).Contains(pos);
			if (_isDragging || IsGhost)
			{
				((Control)this).set_BasicTooltipText((string)null);
			}
			else if (_isHoveringDragHandle)
			{
				((Control)this).set_BasicTooltipText("Drag to reorder");
			}
			else if (_isHoveringRemove)
			{
				((Control)this).set_BasicTooltipText("Remove from queue");
			}
			else
			{
				((Control)this).set_BasicTooltipText((string)null);
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_isHoveringDragHandle = false;
			_isHoveringRemove = false;
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (_isHoveringDragHandle)
			{
				_isDragging = true;
				((Control)this).set_BasicTooltipText((string)null);
				this.DragStarted?.Invoke(this, EventArgs.Empty);
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			if (_isDragging)
			{
				_isDragging = false;
				this.DragEnded?.Invoke(this, EventArgs.Empty);
			}
			((Control)this).OnLeftMouseButtonReleased(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (_isHoveringRemove)
			{
				this.RemoveRequested?.Invoke(this, EventArgs.Empty);
			}
			((Control)this).OnClick(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			float opacity = ((Control)this).get_Opacity();
			Color bgColor = (_isCurrent ? CurrentHighlight : (((Control)this).get_MouseOver() ? MaestroTheme.PanelHover : MaestroTheme.PanelBackground));
			((Color)(ref bgColor))._002Ector((int)((Color)(ref bgColor)).get_R(), (int)((Color)(ref bgColor)).get_G(), (int)((Color)(ref bgColor)).get_B(), (int)((float)(int)((Color)(ref bgColor)).get_A() * opacity));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, bgColor);
			Color instrumentColor = MaestroTheme.GetInstrumentAccent(_song.Instrument);
			((Color)(ref instrumentColor))._002Ector((int)((Color)(ref instrumentColor)).get_R(), (int)((Color)(ref instrumentColor)).get_G(), (int)((Color)(ref instrumentColor)).get_B(), (int)(255f * opacity));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, 40), instrumentColor);
			Color handleColor = ((_isHoveringDragHandle || _isDragging) ? MaestroTheme.AmberGold : MaestroTheme.MutedCream);
			((Color)(ref handleColor))._002Ector((int)((Color)(ref handleColor)).get_R(), (int)((Color)(ref handleColor)).get_G(), (int)((Color)(ref handleColor)).get_B(), (int)(255f * opacity));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MaestroIcons.Queue, _dragHandleBounds, handleColor);
			int titleWidth = base._size.X - 32 - 24 - 22 - 8;
			string title = TruncateText(_song.Name, Control.get_Content().get_DefaultFont14(), titleWidth);
			Color val = MaestroTheme.CreamWhite;
			byte r = ((Color)(ref val)).get_R();
			val = MaestroTheme.CreamWhite;
			byte g = ((Color)(ref val)).get_G();
			val = MaestroTheme.CreamWhite;
			Color titleColor = default(Color);
			((Color)(ref titleColor))._002Ector((int)r, (int)g, (int)((Color)(ref val)).get_B(), (int)(255f * opacity));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, title, Control.get_Content().get_DefaultFont14(), new Rectangle(32, 4, titleWidth, 16), titleColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			string artistTruncated = TruncateText(_song.Artist, Control.get_Content().get_DefaultFont12(), titleWidth);
			val = MaestroTheme.MutedCream;
			byte r2 = ((Color)(ref val)).get_R();
			val = MaestroTheme.MutedCream;
			byte g2 = ((Color)(ref val)).get_G();
			val = MaestroTheme.MutedCream;
			Color artistColor = default(Color);
			((Color)(ref artistColor))._002Ector((int)r2, (int)g2, (int)((Color)(ref val)).get_B(), (int)(255f * opacity));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, artistTruncated, Control.get_Content().get_DefaultFont12(), new Rectangle(32, 21, titleWidth, 14), artistColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			Color removeColor = (_isHoveringRemove ? MaestroTheme.Error : MaestroTheme.MutedCream);
			((Color)(ref removeColor))._002Ector((int)((Color)(ref removeColor)).get_R(), (int)((Color)(ref removeColor)).get_G(), (int)((Color)(ref removeColor)).get_B(), (int)(255f * opacity));
			Rectangle removeIconRect = default(Rectangle);
			((Rectangle)(ref removeIconRect))._002Ector(_removeButtonBounds.X + (_removeButtonBounds.Width - 16) / 2, _removeButtonBounds.Y + (_removeButtonBounds.Height - 16) / 2, 16, 16);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MaestroIcons.Cancel, removeIconRect, removeColor);
		}

		private static string TruncateText(string text, BitmapFont font, int maxWidth)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (font.MeasureString(text).Width <= (float)maxWidth)
			{
				return text;
			}
			string truncated = text;
			while (truncated.Length > 0)
			{
				truncated = truncated.Substring(0, truncated.Length - 1);
				if (font.MeasureString(truncated + "...").Width <= (float)maxWidth)
				{
					return truncated + "...";
				}
			}
			return "...";
		}
	}
}
