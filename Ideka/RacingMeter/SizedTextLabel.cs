using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public class SizedTextLabel : RectAnchor
	{
		public string SuffixModel;

		public override Vector2 SizeDelta => Size2.op_Implicit((SuffixModel != null) ? (Font.MeasureString(Text.Substring(0, Math.Max(0, Text.Length - SuffixModel.Length))) + Font.MeasureString(SuffixModel)) : Font.MeasureString(Text));

		public string Text { get; set; }

		public BitmapFont Font { get; set; }

		public Color Color { get; set; }

		public bool Stroke { get; set; }

		public int StrokeDistance { get; set; }

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			target.DrawString(Text, Font, Color, wrap: false, Stroke, StrokeDistance, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
