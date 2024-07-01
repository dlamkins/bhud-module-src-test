using System;
using Blish_HUD;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public abstract class MeterMaker
	{
		public static readonly Color DefaultColor = Color.get_White();

		public static readonly Color Default3DColor = Color.get_SkyBlue();

		public static readonly Color MarginColor = Color.get_Black();

		public static readonly Color SoftMaxColor = Color.get_DarkRed();

		public static readonly Color ForwardOrbColor = Color.get_CornflowerBlue();

		public static readonly Color WaterColor = Color.get_LightBlue();

		public static readonly Color UpSpeedColor = Color.get_DarkSlateBlue();

		public static readonly Color DownSpeedColor = Color.get_SaddleBrown();

		public const int DefaultThickness = 2;

		public const float NeedleLength = 20f;

		public const int AngleMeterDistance = 15;

		public const double AngleGuide = 45.0;

		public const double WaterLevel = -40.0;

		public IMeasurer Measurer { get; }

		protected MeterMaker(IMeasurer measurer)
		{
			Measurer = measurer;
			base._002Ector();
		}

		public static int RoundedDegrees(double angle)
		{
			int r = (int)Math.Round(angle * 180.0);
			if (r % 180 != 0)
			{
				return r;
			}
			return Math.Abs(r);
		}
	}
	public abstract class MeterMaker<T> : MeterMaker where T : AnchoredRect, IMeter, new()
	{
		public T Meter { get; } = new T();


		protected MeterMaker(IMeasurer measurer)
			: base(measurer)
		{
		}

		public SizedTextLabel AddText(Vector2 anchor, Vector2 pivot, BitmapFont? font = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			return Meter.AddChild(new SizedTextLabel
			{
				Anchor = anchor,
				Pivot = pivot,
				Font = (font ?? GameService.Content.get_DefaultFont16()),
				Color = MeterMaker.DefaultColor,
				Stroke = true,
				StrokeDistance = 1
			});
		}
	}
}
