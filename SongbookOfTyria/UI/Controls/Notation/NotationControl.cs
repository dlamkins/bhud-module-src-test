using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace SongbookOfTyria.UI.Controls.Notation
{
	public class NotationControl : Control
	{
		private readonly struct TextSegment
		{
			public readonly string Text;

			public readonly BitmapFont Font;

			public readonly Color Color;

			public readonly int X;

			public readonly int Y;

			public readonly int CharWidth;

			public readonly int LineHeight;

			public readonly Color? BackgroundColor;

			public TextSegment(string text, BitmapFont font, Color color, int x, int y, int charWidth = 0, int lineHeight = 0, Color? backgroundColor = null)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Text = text;
				Font = font;
				Color = color;
				X = x;
				Y = y;
				CharWidth = charWidth;
				LineHeight = lineHeight;
				BackgroundColor = backgroundColor;
			}
		}

		private const float BoldOffset = 0.5f;

		private const bool ShowDebugBounds = false;

		private static readonly Color[] DebugColors = (Color[])(object)new Color[6]
		{
			Color.get_Red() * 0.3f,
			Color.get_Green() * 0.3f,
			Color.get_Blue() * 0.3f,
			Color.get_Yellow() * 0.3f,
			Color.get_Cyan() * 0.3f,
			Color.get_Magenta() * 0.3f
		};

		private readonly List<TextSegment> _segments = new List<TextSegment>();

		public bool SmoothScrolling { get; set; }

		public void AddSegment(string text, BitmapFont font, Color color, int x, int y, int charWidth = 0, int lineHeight = 0, Color? backgroundColor = null)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_segments.Add(new TextSegment(text, font, color, x, y, charWidth, lineHeight, backgroundColor));
		}

		public void ClearSegments()
		{
			_segments.Clear();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			float uiScale = Control.get_Graphics().get_UIScaleMultiplier();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			float opacity = ((Control)this).AbsoluteOpacity();
			int colorIdx = 0;
			Rectangle bgRect = default(Rectangle);
			Vector2 position = default(Vector2);
			foreach (TextSegment segment in _segments)
			{
				float num = segment.X + absoluteBounds.X;
				float absY = segment.Y + absoluteBounds.Y;
				float alignedX = (float)(int)(num * uiScale) / uiScale;
				float finalY = (SmoothScrolling ? absY : ((float)(int)(absY * uiScale) / uiScale));
				if (segment.BackgroundColor.HasValue && segment.CharWidth > 0 && segment.LineHeight > 0)
				{
					((Rectangle)(ref bgRect))._002Ector(segment.X, segment.Y, segment.CharWidth, segment.LineHeight);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bgRect, segment.BackgroundColor.Value * opacity);
				}
				((Vector2)(ref position))._002Ector(alignedX, finalY);
				Color color = segment.Color * opacity;
				BitmapFontExtensions.DrawString(spriteBatch, segment.Font, segment.Text, position + new Vector2(0.5f, 0f), color, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, segment.Font, segment.Text, position, color, (Rectangle?)null);
			}
		}

		public NotationControl()
			: this()
		{
		}
	}
}
