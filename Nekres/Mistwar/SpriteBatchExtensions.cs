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
		public static void DrawWvwObjectiveOnCtrl(this SpriteBatch spriteBatch, Control control, WvwObjectiveEntity objectiveEntity, Rectangle dest, float opacity = 1f, float scale = 1f, bool drawName = true, bool drawDistance = false)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Invalid comparison between Unknown and I4
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Invalid comparison between Unknown and I4
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Invalid comparison between Unknown and I4
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0628: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_066d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0673: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			if (objectiveEntity.Icon == null)
			{
				return;
			}
			BitmapFont font = ((scale < 0.25f) ? GameService.Content.get_DefaultFont12() : ((scale < 0.5f) ? GameService.Content.get_DefaultFont14() : ((scale < 0.75f) ? GameService.Content.get_DefaultFont16() : GameService.Content.get_DefaultFont18())));
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
			bool doStroke = opacity > 0.99f;
			if (objectiveEntity.HasBuff(out var remainingTime))
			{
				string text2 = remainingTime.ToString("m\\:ss");
				Size2 size2 = font.MeasureString(text2);
				Point texSize = PointExtensions.ResizeKeepAspect(new Point(objectiveEntity.BuffTexture.get_Width(), objectiveEntity.BuffTexture.get_Height()), (int)(scale * size2.Width), (int)(scale * size2.Height), false);
				Rectangle iconBnds = default(Rectangle);
				((Rectangle)(ref iconBnds))._002Ector(dest.X - 20, (int)((float)dest.Y - scale * 80f), texSize.X, texSize.Y);
				Rectangle textBnds = default(Rectangle);
				((Rectangle)(ref textBnds))._002Ector(((Rectangle)(ref iconBnds)).get_Right() + 3, iconBnds.Y, iconBnds.Width, iconBnds.Height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.BuffTexture, iconBnds, (Rectangle?)objectiveEntity.BuffTexture.get_Bounds(), whiteColor);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, text2, font, textBnds, textColor, false, doStroke, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
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
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, objectiveEntity.Name, font, new Rectangle(dest.X - (int)nameSize.Width / 2, dest.Y + dest.Height / 2 + (int)(scale * 12f), (int)nameSize.Width, (int)nameSize.Height), textColor, false, doStroke, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (drawDistance)
			{
				float distance = objectiveEntity.GetDistance() - 25f;
				if (distance > 1f)
				{
					string text = ((distance >= 1000f) ? $"{distance / 1000f:N2}km" : $"{distance:N0}m");
					Size2 size = font.MeasureString(text);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, text, font, new Rectangle(dest.X - (int)size.Width / 2, dest.Y - (int)size.Height - dest.Height / 2 - (int)(scale * 80f), dest.Width, dest.Height), textColor, false, doStroke, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}
	}
}
