using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class LineMeter : AnchoredRectMeter
	{
		public bool Horizontal { get; set; }

		public bool Positive { get; set; }

		public Vector2 Padding { get; set; }

		public override double MinProjected { get; protected set; }

		public override double FullPortion { get; protected set; }

		public Vector2 Origin { get; private set; }

		public Vector2 Full { get; private set; }

		public Vector2 Breadth { get; private set; }

		public Vector2 GetBreadth(float thickness)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((Breadth.X == 0f) ? thickness : Breadth.X, (Breadth.Y == 0f) ? thickness : Breadth.Y);
		}

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			base.EarlyDraw(target);
			MinProjected = (Horizontal ? Padding.X : Padding.Y) * (float)(Positive ? 1 : (-1));
			FullPortion = (Horizontal ? (target.Rect.Width - Padding.X * 2f) : (target.Rect.Height - Padding.Y * 2f)) * (float)(Positive ? 1 : (-1));
			RectangleF rect;
			Vector2 origin;
			if (!Horizontal)
			{
				if (!Positive)
				{
					rect = target.Rect;
					float num = ((RectangleF)(ref rect)).get_Left() + target.Rect.Width / 2f;
					rect = target.Rect;
					origin = new Vector2(num, ((RectangleF)(ref rect)).get_Bottom());
				}
				else
				{
					rect = target.Rect;
					float num2 = ((RectangleF)(ref rect)).get_Left() + target.Rect.Width / 2f;
					rect = target.Rect;
					origin = new Vector2(num2, ((RectangleF)(ref rect)).get_Top());
				}
			}
			else if (!Positive)
			{
				rect = target.Rect;
				float right = ((RectangleF)(ref rect)).get_Right();
				rect = target.Rect;
				origin = new Vector2(right, ((RectangleF)(ref rect)).get_Top() + target.Rect.Height / 2f);
			}
			else
			{
				rect = target.Rect;
				float left = ((RectangleF)(ref rect)).get_Left();
				rect = target.Rect;
				origin = new Vector2(left, ((RectangleF)(ref rect)).get_Top() + target.Rect.Height / 2f);
			}
			Origin = origin;
			Full = (Horizontal ? new Vector2(1f, 0f) : new Vector2(0f, 1f));
			Breadth = (Horizontal ? new Vector2(0f, target.Rect.Height - Padding.Y * 2f) : new Vector2(target.Rect.Width - Padding.X * 2f, 0f));
		}
	}
}
