using System;
using System.Collections.Generic;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
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
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			int widthLimit = ((Control)this).get_Width() - (ShowShadow ? ShadowDistance.X : 0);
			_final = new List<Fragment>();
			float spaceWidth = Font.MeasureStringFixed(" ").Width;
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
						pos.Y += Font.get_LineHeight();
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
							Size2 wordSize = Font.MeasureStringFixed(word);
							if (pos.X + wordSize.Width > (float)widthLimit)
							{
								pos.Y += Font.get_LineHeight();
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
			((Control)this).set_Height((int)Math.Ceiling(pos.Y + (float)Font.get_LineHeight() + (float)(ShowShadow ? ShadowDistance.Y : 0)));
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
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(text))
			{
				if (ShowShadow)
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, RectangleExtension.OffsetBy(bounds, ShadowDistance), ShadowColor, false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, bounds, color, false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
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
