using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	internal sealed class ParchmentContent : Control
	{
		private static readonly Logger Logger = Logger.GetLogger<ParchmentContent>();

		private readonly TextRenderer _textRenderer;

		private readonly Texture2D _parchment;

		private readonly List<string> _lines = new List<string>();

		private string _text = "";

		private float _fontSize = 18f;

		private int _wrapWidth = 400;

		private const int PadX = 18;

		private const int PadY = 14;

		private static readonly Color InkColor = new Color(48, 36, 20);

		public ParchmentContent(TextRenderer tr, Texture2D parchment)
			: this()
		{
			_textRenderer = tr;
			_parchment = parchment;
		}

		public void SetContent(string text, float fontSize, int wrapWidth)
		{
			_text = text ?? "";
			_fontSize = fontSize;
			_wrapWidth = Math.Max(50, wrapWidth);
			Relayout();
		}

		public void SetWrapWidth(int wrapWidth)
		{
			int w = Math.Max(50, wrapWidth);
			if (w != _wrapWidth)
			{
				_wrapWidth = w;
				Relayout();
			}
		}

		public void SetFontSize(float fontSize)
		{
			if (!(Math.Abs(_fontSize - fontSize) < 0.1f))
			{
				_fontSize = fontSize;
				Relayout();
			}
		}

		private void Relayout()
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_lines.Clear();
				_lines.AddRange(_textRenderer.WrapText(_text, _fontSize, _wrapWidth - 36));
				int height = (int)Math.Ceiling(_textRenderer.LineHeight(_fontSize) * (float)Math.Max(1, _lines.Count)) + 28;
				((Control)this).set_Size(new Point(_wrapWidth, height));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Parchment relayout failed.");
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (_parchment != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _parchment, bounds, (Rectangle?)new Rectangle(0, 0, bounds.Width, bounds.Height), Color.get_White());
				}
				float lh = _textRenderer.LineHeight(_fontSize);
				float y = bounds.Y + 14;
				foreach (string line in _lines)
				{
					if (line.Length > 0)
					{
						Texture2D tex = _textRenderer.RenderLine(line, _fontSize, InkColor);
						if (tex != null)
						{
							SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, new Rectangle(bounds.X + 18, (int)y, tex.get_Width(), tex.get_Height()));
						}
					}
					y += lh;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Parchment paint failed.");
			}
		}
	}
}
