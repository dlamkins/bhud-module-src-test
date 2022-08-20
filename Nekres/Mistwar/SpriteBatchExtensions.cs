using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar
{
	public static class SpriteBatchExtensions
	{
		public static void DrawWvwObjectiveOnCtrl(this SpriteBatch spriteBatch, Control control, WvwObjectiveEntity objectiveEntity, Rectangle dest, float opacity = 1f, float scale = 1f, bool drawName = true)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Invalid comparison between Unknown and I4
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Invalid comparison between Unknown and I4
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			if (objectiveEntity.Icon == null)
			{
				return;
			}
			BitmapFont font = (((double)scale < 0.25) ? GameService.Content.get_DefaultFont12() : ((scale < 0.5f) ? GameService.Content.get_DefaultFont14() : ((scale < 0.75f) ? GameService.Content.get_DefaultFont16() : GameService.Content.get_DefaultFont18())));
			Color teamColor = objectiveEntity.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value()) * opacity;
			Color borderColor = Color.get_Black() * opacity;
			Color textColor = WvwObjectiveEntity.BrightGold * (opacity + 0.2f);
			Color whiteColor = Color.get_White() * opacity;
			Rectangle tDest = RectangleExtension.ToBounds(dest, control.get_AbsoluteBounds());
			if ((int)objectiveEntity.Type != 6)
			{
				WvwOwner shapeType = (WvwOwner)(MistwarModule.ModuleInstance.TeamShapesSetting.get_Value() ? ((int)objectiveEntity.Owner) : 2);
				if ((int)shapeType != 3)
				{
					if ((int)shapeType == 4)
					{
						float aWidth = scale * 1.7f * (float)tDest.Width;
						float aHeight = scale * 1.7f * (float)tDest.Height;
						Vector2[] shapeDiamond = (Vector2[])(object)new Vector2[4]
						{
							new Vector2(aWidth / 2f, aWidth),
							new Vector2(0f, aWidth / 2f),
							new Vector2(aWidth / 2f, 0f),
							new Vector2(aWidth, aWidth / 2f)
						};
						ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - aWidth / 2f, (float)tDest.Y - aHeight / 2f), (IReadOnlyList<Vector2>)shapeDiamond, teamColor, scale * (float)dest.Width, 0f);
						ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - aWidth / 2f, (float)tDest.Y - aHeight / 2f), (IReadOnlyList<Vector2>)shapeDiamond, borderColor, 2f, 0f);
					}
					else
					{
						CircleF circleDest = default(CircleF);
						((CircleF)(ref circleDest))._002Ector(new Point2((float)tDest.X, (float)tDest.Y), (float)(int)((double)scale * 0.7 * (double)dest.Width));
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, teamColor, scale * (float)dest.Width, 0f);
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, borderColor, 2f, 0f);
					}
				}
				else
				{
					float aScale = scale * 1.3f;
					RectangleF shapeRectangle = default(RectangleF);
					((RectangleF)(ref shapeRectangle))._002Ector((float)tDest.X - aScale * (float)tDest.Width / 2f, (float)tDest.Y - aScale * (float)tDest.Height / 2f, aScale * (float)tDest.Width, aScale * (float)tDest.Height);
					ShapeExtensions.FillRectangle(spriteBatch, shapeRectangle, teamColor, 0f);
					ShapeExtensions.DrawRectangle(spriteBatch, shapeRectangle, borderColor, 2f, 0f);
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.Icon, new Rectangle(dest.X - (int)(scale * (float)dest.Width) / 2, dest.Y - (int)(scale * (float)dest.Height) / 2, (int)(scale * (float)dest.Width), (int)(scale * (float)dest.Height)), (Rectangle?)objectiveEntity.Icon.get_Bounds(), teamColor);
			if (objectiveEntity.HasBuff(out var remainingTime))
			{
				string text = remainingTime.ToString("m\\:ss");
				Size2 size = font.MeasureString(text);
				Point texSize = PointExtensions.ResizeKeepAspect(new Point(objectiveEntity.BuffTexture.get_Width(), objectiveEntity.BuffTexture.get_Height()), (int)(scale * size.Width), (int)(scale * size.Height), false);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.BuffTexture, new Rectangle(dest.X + texSize.X / 2, dest.Y - texSize.Y + 1, texSize.X, texSize.Y), (Rectangle?)objectiveEntity.BuffTexture.get_Bounds(), whiteColor);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, text, font, new Rectangle(dest.X - (int)size.Width / 2, dest.Y - (int)size.Height - dest.Height / 2 - 10, dest.Width, dest.Height), textColor, false, opacity >= 0.99f, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (objectiveEntity.IsClaimed())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.ClaimedTexture, new Rectangle(dest.X + (int)((double)scale * 0.6 * (double)dest.Width) - (int)(scale * (float)objectiveEntity.ClaimedTexture.get_Width()) / 2, dest.Y + (int)((double)scale * 0.9 * (double)dest.Height) - dest.Height / 2, (int)(scale * (float)objectiveEntity.ClaimedTexture.get_Width()), (int)(scale * (float)objectiveEntity.ClaimedTexture.get_Height())), (Rectangle?)objectiveEntity.ClaimedTexture.get_Bounds(), whiteColor);
			}
			if (objectiveEntity.HasUpgraded())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.UpgradeTexture, new Rectangle(dest.X - (int)(scale * (float)objectiveEntity.UpgradeTexture.get_Width()) / 2, dest.Y - (int)(scale * (float)dest.Height) / 2 - (int)((double)scale * 0.7 * (double)objectiveEntity.UpgradeTexture.get_Height()), (int)(scale * (float)objectiveEntity.UpgradeTexture.get_Width()), (int)(scale * (float)objectiveEntity.UpgradeTexture.get_Height())), (Rectangle?)objectiveEntity.UpgradeTexture.get_Bounds(), whiteColor);
			}
			if (drawName)
			{
				Size2 nameSize = font.MeasureString(objectiveEntity.Name);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, objectiveEntity.Name, font, new Rectangle(dest.X - (int)nameSize.Width / 2, dest.Y + dest.Height / 2 + 3, (int)nameSize.Width, (int)nameSize.Height), textColor, false, opacity >= 0.99f, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
