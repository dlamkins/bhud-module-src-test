using System;
using System.Linq;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.CustomCombatText
{
	public class AreaViewBase : AnchoredRect
	{
		private void DrawVisualAids(RectTarget target)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			AreaView selected = CTextModule.Selected;
			if (selected != null)
			{
				RectangleF rect;
				if (selected == this)
				{
					target.SpriteBatch.DrawRectangleFill(target.Rect, Color.get_Black() * 0.4f, 1f);
					ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Black(), 1f, 0f);
					SpriteBatch spriteBatch = target.SpriteBatch;
					rect = target.Rect;
					Point2 position = ((RectangleF)(ref rect)).get_Position();
					rect = target.Rect;
					ShapeExtensions.DrawCircle(spriteBatch, Point2.op_Implicit(position + Size2.op_Implicit(((RectangleF)(ref rect)).get_Size()) * selected.Pivot), 5f, 12, Color.get_Black(), 1f, 0f);
				}
				else if (selected.Parent == this)
				{
					target.SpriteBatch.DrawRectangleFill(target.Rect, Color.get_White() * 0.1f, 1f);
					ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_White() * 0.5f, 1f, 0f);
					rect = target.Rect;
					Point2 position2 = ((RectangleF)(ref rect)).get_Position();
					rect = target.Rect;
					Point2 topLeft = position2 + Size2.op_Implicit(((RectangleF)(ref rect)).get_Size()) * selected.AnchorMin;
					rect = target.Rect;
					Point2 topRight = position2 + Size2.op_Implicit(((RectangleF)(ref rect)).get_Size()) * new Vector2(selected.AnchorMax.X, selected.AnchorMin.Y);
					rect = target.Rect;
					Point2 bottomRight = position2 + Size2.op_Implicit(((RectangleF)(ref rect)).get_Size()) * selected.AnchorMax;
					rect = target.Rect;
					Point2 bottomLeft = position2 + Size2.op_Implicit(((RectangleF)(ref rect)).get_Size()) * new Vector2(selected.AnchorMin.X, selected.AnchorMax.Y);
					drawPincer(Point2.op_Implicit(topLeft), -1, -1);
					drawPincer(Point2.op_Implicit(topRight), 1, -1);
					drawPincer(Point2.op_Implicit(bottomRight), 1, 1);
					drawPincer(Point2.op_Implicit(bottomLeft), -1, 1);
					ShapeExtensions.DrawLine(target.SpriteBatch, Point2.op_Implicit(topLeft), Point2.op_Implicit(topRight), Color.get_White(), 1f, 0f);
					ShapeExtensions.DrawLine(target.SpriteBatch, Point2.op_Implicit(topRight), Point2.op_Implicit(bottomRight), Color.get_White(), 1f, 0f);
					ShapeExtensions.DrawLine(target.SpriteBatch, Point2.op_Implicit(bottomRight), Point2.op_Implicit(bottomLeft), Color.get_White(), 1f, 0f);
					ShapeExtensions.DrawLine(target.SpriteBatch, Point2.op_Implicit(bottomLeft), Point2.op_Implicit(topLeft), Color.get_White(), 1f, 0f);
				}
				else if (selected.Parent?.GetAreaViewChildren().Contains(this) ?? false)
				{
					target.SpriteBatch.DrawRectangleFill(target.Rect, Color.get_Gray() * 0.1f, 1f);
					ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Gray(), 1f, 1f);
				}
				else
				{
					ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Gray() * 0.5f, 1f, 0f);
				}
			}
			void drawPincer(Vector2 pos, int xScale, int yScale)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				double sin = Math.Sin(0.5235987901687622);
				double cos = Math.Cos(0.5235987901687622);
				Vector2 pos2 = default(Vector2);
				((Vector2)(ref pos2))._002Ector((float)((double)pos.X + 20.0 * sin * (double)xScale), (float)((double)pos.Y + 20.0 * cos * (double)yScale));
				Vector2 pos3 = default(Vector2);
				((Vector2)(ref pos3))._002Ector((float)((double)pos.X + 20.0 * cos * (double)xScale), (float)((double)pos.Y + 20.0 * sin * (double)yScale));
				ShapeExtensions.DrawLine(target.SpriteBatch, pos, pos2, Color.get_Black(), 1f, 0f);
				ShapeExtensions.DrawLine(target.SpriteBatch, pos, pos3, Color.get_Black(), 1f, 0f);
				ShapeExtensions.DrawLine(target.SpriteBatch, pos2, pos3, Color.get_Black(), 1f, 0f);
			}
		}

		protected override void EarlyDraw(RectTarget target)
		{
			base.EarlyDraw(target);
			DrawVisualAids(target);
		}
	}
}
