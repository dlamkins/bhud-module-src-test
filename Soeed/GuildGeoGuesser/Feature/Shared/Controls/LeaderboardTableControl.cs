using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class LeaderboardTableControl : Control
	{
		private List<string> _headers = new List<string>();

		private List<List<string>> _rows = new List<List<string>>();

		private List<HorizontalAlignment> _columnAlignments = new List<HorizontalAlignment>();

		private int _rowHeight = 28;

		private int _headerHeight = 32;

		private int _padding = 8;

		private Color _headerTextColor = Color.get_LightGoldenrodYellow();

		private Color _rowTextColor = Color.get_WhiteSmoke();

		private string _title = string.Empty;

		private string _subtitle = string.Empty;

		private string _footer = string.Empty;

		private int _titleHeight = 32;

		private int _subtitleHeight = 18;

		private int _footerHeight = 24;

		private Color _titleTextColor = Color.get_LightGoldenrodYellow();

		private Color _subtitleTextColor = Color.get_LightGray();

		private Color _footerTextColor = Color.get_LightGray();

		private bool _showTrophyIcons;

		private AsyncTexture2D _gradientRowTexture = Service.Textures.DatAsset(156044);

		private AsyncTexture2D _headerTexture = Service.Textures.DatAsset(1032325);

		public bool ShowTrophyIcons
		{
			get
			{
				return _showTrophyIcons;
			}
			set
			{
				_showTrophyIcons = value;
				((Control)this).Invalidate();
			}
		}

		public AsyncTexture2D GoldTrophy { get; set; }

		public AsyncTexture2D SilverTrophy { get; set; }

		public AsyncTexture2D BronzeTrophy { get; set; }

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
				((Control)this).Invalidate();
			}
		}

		public string Subtitle
		{
			get
			{
				return _subtitle;
			}
			set
			{
				_subtitle = value;
				((Control)this).Invalidate();
			}
		}

		public string Footer
		{
			get
			{
				return _footer;
			}
			set
			{
				_footer = value;
				((Control)this).Invalidate();
			}
		}

		public void SetHeaders(params string[] headers)
		{
			_headers = headers.ToList();
			((Control)this).Invalidate();
		}

		public void SetRows(List<List<string>> rows)
		{
			_rows = rows;
			((Control)this).Invalidate();
		}

		public void SetColumnAlignments(params HorizontalAlignment[] alignments)
		{
			_columnAlignments = alignments.ToList();
			((Control)this).Invalidate();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			if (_headers.Count == 0)
			{
				return;
			}
			int colCount = _headers.Count;
			int[] colWidths = new int[colCount];
			int totalWidth = bounds.Width - 2 * _padding;
			int colWidth = totalWidth / colCount;
			for (int j = 0; j < colCount; j++)
			{
				colWidths[j] = colWidth;
			}
			int y = bounds.Y + _padding;
			if (!string.IsNullOrEmpty(_title))
			{
				DrawText(spriteBatch, _title, bounds.X + _padding, y, totalWidth, _titleTextColor, bold: true, (HorizontalAlignment)0, _titleHeight);
				y += _titleHeight;
			}
			if (!string.IsNullOrEmpty(_subtitle))
			{
				DrawText(spriteBatch, _subtitle, bounds.X + _padding, y, totalWidth, _subtitleTextColor, bold: true, (HorizontalAlignment)0, _subtitleHeight);
				y += _subtitleHeight;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_headerTexture), new Rectangle(bounds.X + _padding, y, totalWidth, _headerHeight), Color.get_White());
			int i = 0;
			int x2 = bounds.X + _padding;
			for (; i < colCount; i++)
			{
				string text = _headers[i];
				HorizontalAlignment align = (HorizontalAlignment)((_columnAlignments.Count > i) ? ((int)_columnAlignments[i]) : 0);
				DrawText(spriteBatch, text, x2 + 8, y + 6, colWidths[i] - 16, _headerTextColor, bold: true, align);
				x2 += colWidths[i];
			}
			y += _headerHeight;
			for (int row = 0; row < _rows.Count; row++)
			{
				if (row % 2 == 1)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_gradientRowTexture), new Rectangle(bounds.X + _padding, y, totalWidth, _rowHeight), Color.get_White());
				}
				int col = 0;
				int x = bounds.X + _padding;
				for (; col < colCount; col++)
				{
					string text2 = ((col < _rows[row].Count) ? _rows[row][col] : string.Empty);
					HorizontalAlignment align2 = (HorizontalAlignment)((_columnAlignments.Count > col) ? ((int)_columnAlignments[col]) : 0);
					int iconOffset = 0;
					if (_showTrophyIcons && col == 0 && row < 3)
					{
						AsyncTexture2D icon = null;
						switch (row)
						{
						case 0:
							icon = GoldTrophy;
							break;
						case 1:
							icon = SilverTrophy;
							break;
						case 2:
							icon = BronzeTrophy;
							break;
						}
						if (icon != null)
						{
							int iconSize = 24;
							SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(icon), new Rectangle(x + 8, y + (_rowHeight - iconSize) / 2, iconSize, iconSize), Color.get_White());
							iconOffset = iconSize + 6;
						}
					}
					DrawText(spriteBatch, text2, x + 8 + iconOffset, y + 4, colWidths[col] - 16 - iconOffset, _rowTextColor, bold: false, align2);
					x += colWidths[col];
				}
				y += _rowHeight;
			}
			if (!string.IsNullOrEmpty(_footer))
			{
				y += 4;
				DrawText(spriteBatch, _footer, bounds.X + _padding, y, totalWidth, _footerTextColor, bold: false, (HorizontalAlignment)0, _footerHeight);
			}
		}

		private void DrawText(SpriteBatch spriteBatch, string text, int x, int y, int width, Color color, bool bold, HorizontalAlignment align, int? overrideHeight = null)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont14();
			if (bold)
			{
				font = GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2);
			}
			Size2 size = font.MeasureString(text);
			int drawX = x;
			if ((int)align == 1)
			{
				drawX = x + (width - (int)size.Width) / 2;
			}
			else if ((int)align == 2)
			{
				drawX = x + width - (int)size.Width;
			}
			int height = overrideHeight ?? ((int)size.Height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, font, new Rectangle(drawX, y, width, height), color, false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		public override void RecalculateLayout()
		{
			((Control)this).RecalculateLayout();
			int height = _headerHeight + _rows.Count * _rowHeight + 2 * _padding;
			if (!string.IsNullOrEmpty(_title))
			{
				height += _titleHeight;
			}
			if (!string.IsNullOrEmpty(_subtitle))
			{
				height += _subtitleHeight;
			}
			if (!string.IsNullOrEmpty(_footer))
			{
				height += _footerHeight + 4;
			}
			((Control)this).set_Height(height);
		}

		public LeaderboardTableControl()
			: this()
		{
		}//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)

	}
}
