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
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Invalid comparison between Unknown and I4
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Invalid comparison between Unknown and I4
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_066c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0694: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			if (objectiveEntity.Icon == null)
			{
				return;
			}
			BitmapFont font = ((scale < 0.25f) ? GameService.Content.get_DefaultFont12() : ((scale < 0.5f) ? GameService.Content.get_DefaultFont14() : ((scale < 0.75f) ? GameService.Content.get_DefaultFont16() : GameService.Content.get_DefaultFont18())));
			Color teamColor = objectiveEntity.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value()) * opacity;
			Color borderColor = Color.get_Black() * opacity;
			Color textColor = MistwarModule.ModuleInstance.Resources.BrightGold * (opacity + 0.2f);
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
			Rectangle typeIconBnds = default(Rectangle);
			((Rectangle)(ref typeIconBnds))._002Ector(dest.X - (int)(scale * (float)dest.Width) / 2, dest.Y - (int)(scale * (float)dest.Height) / 2, (int)(scale * (float)dest.Width), (int)(scale * (float)dest.Height));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.Icon, typeIconBnds, (Rectangle?)objectiveEntity.Icon.get_Bounds(), teamColor);
			bool doStroke = opacity > 0.99f;
			if (objectiveEntity.HasBuff(out var remainingTime))
			{
				string text2 = remainingTime.ToString("m\\:ss");
				Size2 size2 = font.MeasureString(text2);
				Point texSize = PointExtensions.ResizeKeepAspect(new Point(objectiveEntity.BuffTexture.get_Width(), objectiveEntity.BuffTexture.get_Height()), (int)(scale * size2.Width), (int)(scale * size2.Height), false);
				Rectangle iconBnds = default(Rectangle);
				((Rectangle)(ref iconBnds))._002Ector(dest.X - (int)(size2.Width + (float)texSize.X) / 2, ((Rectangle)(ref typeIconBnds)).get_Top() - texSize.Y - 10, texSize.X, texSize.Y);
				Rectangle textBnds = default(Rectangle);
				((Rectangle)(ref textBnds))._002Ector(((Rectangle)(ref iconBnds)).get_Right() + 3, iconBnds.Y, (int)size2.Width, iconBnds.Height);
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
