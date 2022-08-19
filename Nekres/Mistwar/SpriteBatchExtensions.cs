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
		public static void DrawWvwObjectiveOnCtrl(this SpriteBatch spriteBatch, Control control, WvwObjectiveEntity objectiveEntity, Rectangle dest, float opacity = 1f, BitmapFont font = null)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Invalid comparison between Unknown and I4
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Invalid comparison between Unknown and I4
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Invalid comparison between Unknown and I4
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			if (objectiveEntity.Icon == null)
			{
				return;
			}
			if (font == null)
			{
				font = GameService.Content.get_DefaultFont18();
			}
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
						float aWidth = 1.7f * (float)tDest.Width;
						float aHeight = 1.7f * (float)tDest.Height;
						Vector2[] shapeDiamond = (Vector2[])(object)new Vector2[4]
						{
							new Vector2(aWidth / 2f, aWidth),
							new Vector2(0f, aWidth / 2f),
							new Vector2(aWidth / 2f, 0f),
							new Vector2(aWidth, aWidth / 2f)
						};
						ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - aWidth / 2f, (float)tDest.Y - aHeight / 2f), (IReadOnlyList<Vector2>)shapeDiamond, teamColor, (float)dest.Width, 0f);
						ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - aWidth / 2f, (float)tDest.Y - aHeight / 2f), (IReadOnlyList<Vector2>)shapeDiamond, borderColor, 2f, 0f);
					}
					else
					{
						CircleF circleDest = default(CircleF);
						((CircleF)(ref circleDest))._002Ector(new Point2((float)tDest.X, (float)tDest.Y), (float)(int)(0.7 * (double)dest.Width));
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, teamColor, (float)dest.Width, 0f);
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, borderColor, 2f, 0f);
					}
				}
				else
				{
					float aScale = 1.3f;
					RectangleF shapeRectangle = default(RectangleF);
					((RectangleF)(ref shapeRectangle))._002Ector((float)tDest.X - aScale * (float)tDest.Width / 2f, (float)tDest.Y - aScale * (float)tDest.Height / 2f, aScale * (float)tDest.Width, aScale * (float)tDest.Height);
					ShapeExtensions.FillRectangle(spriteBatch, shapeRectangle, teamColor, 0f);
					ShapeExtensions.DrawRectangle(spriteBatch, shapeRectangle, borderColor, 2f, 0f);
				}
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.Icon, new Rectangle(dest.X - dest.Width / 2, dest.Y - dest.Height / 2, dest.Width, dest.Height), (Rectangle?)objectiveEntity.Icon.get_Bounds(), teamColor);
			if (objectiveEntity.HasBuff(out var remainingTime))
			{
				string text = remainingTime.ToString("m\\:ss");
				Size2 size = font.MeasureString(text);
				Point texSize = PointExtensions.ResizeKeepAspect(new Point(objectiveEntity.BuffTexture.get_Width(), objectiveEntity.BuffTexture.get_Height()), (int)size.Width, (int)size.Height, false);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.BuffTexture, new Rectangle(dest.X + texSize.X / 2, dest.Y - texSize.Y + 1, texSize.X, texSize.Y), (Rectangle?)objectiveEntity.BuffTexture.get_Bounds(), whiteColor);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, text, font, new Rectangle(dest.X - (int)size.Width / 2, dest.Y - (int)size.Height - dest.Height / 2 - 10, dest.Width, dest.Height), textColor, false, opacity >= 0.99f, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (objectiveEntity.IsClaimed())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.ClaimedTexture, new Rectangle(dest.X + (int)(0.6 * (double)dest.Width) - objectiveEntity.ClaimedTexture.get_Width() / 2, dest.Y + (int)(0.9 * (double)dest.Height) - dest.Height / 2, objectiveEntity.ClaimedTexture.get_Width(), objectiveEntity.ClaimedTexture.get_Height()), (Rectangle?)objectiveEntity.ClaimedTexture.get_Bounds(), whiteColor);
			}
			if (objectiveEntity.HasUpgraded())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, control, objectiveEntity.UpgradeTexture, new Rectangle(dest.X - objectiveEntity.UpgradeTexture.get_Width() / 2, dest.Y - dest.Height / 2 - (int)(0.7 * (double)objectiveEntity.UpgradeTexture.get_Height()), objectiveEntity.UpgradeTexture.get_Width(), objectiveEntity.UpgradeTexture.get_Height()), (Rectangle?)objectiveEntity.UpgradeTexture.get_Bounds(), whiteColor);
			}
			if (MistwarModule.ModuleInstance.DrawObjectiveNamesSetting.get_Value())
			{
				Size2 nameSize = font.MeasureString(objectiveEntity.Name);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, control, objectiveEntity.Name, font, new Rectangle(dest.X - (int)nameSize.Width / 2, dest.Y + dest.Height / 2 + 3, (int)nameSize.Width, (int)nameSize.Height), textColor, false, opacity >= 0.99f, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
