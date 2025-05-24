using System;
using System.Collections.Generic;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	public class MarkupLabel : Control
	{
		private class Fragment
		{
			public Point2 Point { get; set; }

			public string Text { get; set; } = "";


			public Color? Color { get; set; }
		}

		private static readonly Point ShadowDistance = new Point(1, 1);

		private string _rawText = "";

		public BitmapFont _font = Control.get_Content().get_DefaultFont16();

		public SpriteFontBase? _spriteFont;

		private bool _showShadow;

		private readonly MarkupParser.Syntax<MarkupParser.Fragment> _syntax;

		private List<Fragment>? _final;

		public string RawText
		{
			get
			{
				return _rawText;
			}
			set
			{
				_rawText = value;
				_final = null;
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
				_final = null;
			}
		}

		public SpriteFontBase? SpriteFont
		{
			get
			{
				return _spriteFont;
			}
			set
			{
				_spriteFont = value;
				_final = null;
			}
		}

		public bool ShowShadow
		{
			get
			{
				return _showShadow;
			}
			set
			{
				_showShadow = value;
				_final = null;
			}
		}

		public Color BaseColor { get; set; } = Color.get_White();


		public Color ShadowColor { get; set; } = Color.get_Black();


		public MarkupLabel(MarkupParser.Syntax<MarkupParser.Fragment> syntax)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_syntax = syntax;
			((Control)this)._002Ector();
		}

		public override void RecalculateLayout()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			int widthLimit = ((Control)this).get_Width() - (ShowShadow ? ShadowDistance.X : 0);
			_final = new List<Fragment>();
			float spaceWidth = ((SpriteFont == null) ? Font.MeasureStringFixed(" ").Width : SpriteFont!.MeasureString(" ").X);
			float lineHeight = ((SpriteFont == null) ? Font.get_LineHeight() : SpriteFont!.LineHeight);
			Point2 pos = Point2.Zero;
			StringBuilder sb = new StringBuilder();
			Fragment last = new Fragment
			{
				Point = pos
			};
			MarkupParser.Fragment lastFrag = null;
			foreach (MarkupParser.Fragment frag in MarkupParser.Parse(RawText, _syntax))
			{
				string[] array = frag.Text.Split('\n');
				foreach (string line in array)
				{
					if (lastFrag == frag)
					{
						pos.X = 0f;
						pos.Y += lineHeight;
					}
					lastFrag = frag;
					finishFragment(frag.Color);
					if (string.IsNullOrWhiteSpace(line))
					{
						continue;
					}
					string[] array2 = line.Split(' ');
					foreach (string word in array2)
					{
						if (!string.IsNullOrWhiteSpace(word))
						{
							Size2 wordSize = ((SpriteFont == null) ? Font.MeasureStringFixed(word) : Size2.op_Implicit(SpriteFont!.MeasureString(word)));
							if (pos.X + wordSize.Width > (float)widthLimit)
							{
								pos.Y += lineHeight;
								pos.X = 0f;
								finishFragment(frag.Color);
							}
							sb.Append(word + " ");
							pos.X += wordSize.Width + spaceWidth;
						}
					}
					finishFragment(frag.Color);
				}
				finishFragment(frag.Color);
			}
			((Control)this).set_Height((int)Math.Ceiling(pos.Y + lineHeight + (float)(ShowShadow ? ShadowDistance.Y : 0)));
			void finishFragment(Color? color)
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				last.Text = sb.ToString();
				last.Color = color;
				if (!string.IsNullOrWhiteSpace(last.Text))
				{
					_final!.Add(last);
				}
				sb.Clear();
				last = new Fragment
				{
					Point = pos
				};
			}
		}

		private void DrawText(SpriteBatch spriteBatch, Rectangle bounds, string text, Color color)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Point location = ((Rectangle)(ref bounds)).get_Location();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			Point position = location + ((Rectangle)(ref absoluteBounds)).get_Location();
			float opacity = ((Control)this).AbsoluteOpacity();
			if (ShowShadow)
			{
				Point val = position + ShadowDistance;
				Vector2 shadowPosition = ((Point)(ref val)).ToVector2();
				if (SpriteFont == null)
				{
					BitmapFontExtensions.DrawString(spriteBatch, Font, text, shadowPosition, ShadowColor * opacity, (Rectangle?)null);
				}
				else
				{
					spriteBatch.DrawString(SpriteFont, text, shadowPosition, ShadowColor * opacity);
				}
			}
			if (SpriteFont == null)
			{
				BitmapFontExtensions.DrawString(spriteBatch, Font, text, ((Point)(ref position)).ToVector2(), color * opacity, (Rectangle?)null);
			}
			else
			{
				spriteBatch.DrawString(SpriteFont, text, ((Point)(ref position)).ToVector2(), color * opacity);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			if (_final == null)
			{
				((Control)this).RecalculateLayout();
			}
			foreach (Fragment final in _final!)
			{
				if (!string.IsNullOrWhiteSpace(final.Text))
				{
					DrawText(spriteBatch, RectangleExtension.MoveRelativeToBoundsLocation(bounds, new Point((int)final.Point.X, (int)final.Point.Y)), final.Text, (Color)(((_003F?)final.Color) ?? BaseColor));
				}
			}
		}
	}
}
