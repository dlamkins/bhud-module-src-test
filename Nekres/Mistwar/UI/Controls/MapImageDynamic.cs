using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.UI.Controls
{
	internal class MapImageDynamic : Control
	{
		private BitmapFont _font;

		private SpriteEffects _spriteEffects;

		private Color _tint = Color.get_White();

		private IEnumerable<WvwObjectiveEntity> WvwObjectives => ((MapImage)(object)((Control)this).get_Parent()).WvwObjectives;

		private Rectangle SourceRectangle => ((MapImage)(object)((Control)this).get_Parent()).SourceRectangle;

		private float ScaleRatio => ((MapImage)(object)((Control)this).get_Parent()).ScaleRatio;

		private float TextureOpacity => ((MapImage)(object)((Control)this).get_Parent()).TextureOpacity;

		public SpriteEffects SpriteEffects
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _spriteEffects;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<SpriteEffects>(ref _spriteEffects, value, false, "SpriteEffects");
			}
		}

		public Color Tint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tint;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _tint, value, false, "Tint");
			}
		}

		public MapImageDynamic(MapImage parent)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Size(((Control)((Control)this).get_Parent()).get_Size());
			((Control)this).set_Visible(((Control)((Control)this).get_Parent()).get_Visible());
			_font = Control.get_Content().GetFont((FontFace)0, (FontSize)24, (FontStyle)0);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Invalid comparison between Unknown and I4
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Invalid comparison between Unknown and I4
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0525: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0578: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_061a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_0665: Unknown result type (might be due to invalid IL or missing references)
			//IL_0698: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_078b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0797: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07da: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0802: Unknown result type (might be due to invalid IL or missing references)
			//IL_080a: Unknown result type (might be due to invalid IL or missing references)
			//IL_080f: Unknown result type (might be due to invalid IL or missing references)
			if (WvwObjectives == null)
			{
				return;
			}
			float widthRatio = (float)bounds.Width / (float)SourceRectangle.Width;
			float heightRatio = (float)bounds.Height / (float)SourceRectangle.Height;
			if (MistwarModule.ModuleInstance.DrawSectorsSetting.get_Value())
			{
				foreach (WvwObjectiveEntity wvwObjective in WvwObjectives)
				{
					Color teamColor2 = wvwObjective.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value(), (int)(TextureOpacity * 255f));
					Vector2[] sectorBounds = wvwObjective.Bounds.Select((Func<Point, Vector2>)delegate(Point p)
					{
						//IL_0020: Unknown result type (might be due to invalid IL or missing references)
						//IL_002b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0030: Unknown result type (might be due to invalid IL or missing references)
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
						//IL_0036: Unknown result type (might be due to invalid IL or missing references)
						//IL_003d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0044: Unknown result type (might be due to invalid IL or missing references)
						Point val = PointExtensions.ToBounds(new Point((int)(widthRatio * (float)p.X), (int)(heightRatio * (float)p.Y)), ((Control)this).get_AbsoluteBounds());
						return new Vector2((float)val.X, (float)val.Y);
					}).ToArray();
					ShapeExtensions.DrawPolygon(spriteBatch, new Vector2(0f, 0f), (IReadOnlyList<Vector2>)sectorBounds, teamColor2, 3f, 0f);
				}
			}
			Rectangle dest = default(Rectangle);
			CircleF circleDest = default(CircleF);
			RectangleF shapeRectangle = default(RectangleF);
			foreach (WvwObjectiveEntity objectiveEntity in WvwObjectives)
			{
				Color teamColor = objectiveEntity.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value());
				if (objectiveEntity.Icon == null)
				{
					continue;
				}
				int width = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Width());
				int height = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Height());
				((Rectangle)(ref dest))._002Ector((int)(widthRatio * (float)objectiveEntity.Center.X), (int)(heightRatio * (float)objectiveEntity.Center.Y), width, height);
				Rectangle tDest = RectangleExtension.ToBounds(dest, ((Control)this).get_AbsoluteBounds());
				WvwOwner owner = objectiveEntity.Owner;
				if ((int)owner != 3)
				{
					if ((int)owner == 4)
					{
						float aWidth = 1.6f * (float)tDest.Width;
						float aHeight = 1.6f * (float)tDest.Height;
						Vector2[] shapeDiamond;
						for (int i = 100; i >= 0; i--)
						{
							float j = (float)i / 100f;
							shapeDiamond = (Vector2[])(object)new Vector2[4]
							{
								new Vector2(j * aWidth, j * (aWidth / 2f)),
								new Vector2(j * (aWidth / 2f), 0f),
								new Vector2(0f, j * (aWidth / 2f)),
								new Vector2(j * (aWidth / 2f), j * aWidth)
							};
							ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - j * (aWidth / 2f), (float)tDest.Y - j * (aHeight / 2f)), (IReadOnlyList<Vector2>)shapeDiamond, teamColor, 1f, 0f);
						}
						shapeDiamond = (Vector2[])(object)new Vector2[4]
						{
							new Vector2(aWidth, aWidth / 2f),
							new Vector2(aWidth / 2f, 0f),
							new Vector2(0f, aWidth / 2f),
							new Vector2(aWidth / 2f, aWidth)
						};
						ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - aWidth / 2f, (float)tDest.Y - aHeight / 2f), (IReadOnlyList<Vector2>)shapeDiamond, Color.get_Black(), 1f, 0f);
					}
					else
					{
						((CircleF)(ref circleDest))._002Ector(new Point2((float)tDest.X, (float)tDest.Y), (float)(int)(0.8 * (double)width));
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, teamColor, (float)width, 0f);
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, Color.get_Black(), 2f, 0f);
					}
				}
				else
				{
					float aScale = 1.4f;
					((RectangleF)(ref shapeRectangle))._002Ector((float)tDest.X - aScale * (float)tDest.Width / 2f, (float)tDest.Y - aScale * (float)tDest.Height / 2f, aScale * (float)tDest.Width, aScale * (float)tDest.Height);
					ShapeExtensions.FillRectangle(spriteBatch, shapeRectangle, teamColor, 0f);
					ShapeExtensions.DrawRectangle(spriteBatch, shapeRectangle, Color.get_Black(), 2f, 0f);
				}
				if (objectiveEntity.HasBuff(out var remainingTime))
				{
					string text = remainingTime.ToString("m\\:ss");
					Size2 size = _font.MeasureString(text);
					Point texSize = PointExtensions.ResizeKeepAspect(new Point(objectiveEntity.BuffTexture.get_Width(), objectiveEntity.BuffTexture.get_Height()), (int)size.Width, (int)size.Height, false);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.BuffTexture, new Rectangle(dest.X + texSize.X / 2, dest.Y - texSize.Y + 1, texSize.X, texSize.Y), (Rectangle?)objectiveEntity.BuffTexture.get_Bounds());
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, new Rectangle(dest.X - (int)size.Width / 2, dest.Y - (int)size.Height - dest.Height / 2 - 10, dest.Width, dest.Height), WvwObjectiveEntity.BrightGold, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.Icon, new Rectangle(dest.X - width / 2, dest.Y - height / 2, width, height), (Rectangle?)objectiveEntity.Icon.get_Bounds(), teamColor);
				double scale = 0.5;
				if (objectiveEntity.IsClaimed())
				{
					if (MistwarModule.ModuleInstance.UseCustomIconsSetting.get_Value())
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.CustomClaimedTexture, new Rectangle(dest.X - width - 5, dest.Y + 10, (int)(scale * (double)objectiveEntity.CustomClaimedTexture.get_Width()), (int)(scale * (double)objectiveEntity.CustomClaimedTexture.get_Height())), (Rectangle?)objectiveEntity.CustomClaimedTexture.get_Bounds());
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.ClaimedTexture, new Rectangle(dest.X + (int)(0.6 * (double)width) - objectiveEntity.ClaimedTexture.get_Width() / 2, dest.Y + (int)(0.9 * (double)height) - height / 2, objectiveEntity.ClaimedTexture.get_Width(), objectiveEntity.ClaimedTexture.get_Height()), (Rectangle?)objectiveEntity.ClaimedTexture.get_Bounds());
					}
				}
				if (objectiveEntity.HasUpgraded())
				{
					if (MistwarModule.ModuleInstance.UseCustomIconsSetting.get_Value())
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.CustomUpgradeTexture, new Rectangle(dest.X + width / 2 - 2, dest.Y + 5, (int)(scale * (double)objectiveEntity.CustomUpgradeTexture.get_Width()), (int)(scale * (double)objectiveEntity.CustomUpgradeTexture.get_Height())), (Rectangle?)objectiveEntity.CustomUpgradeTexture.get_Bounds());
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.UpgradeTexture, new Rectangle(dest.X - objectiveEntity.UpgradeTexture.get_Width() / 2, dest.Y - height / 2 - (int)(0.7 * (double)objectiveEntity.UpgradeTexture.get_Height()), objectiveEntity.UpgradeTexture.get_Width(), objectiveEntity.UpgradeTexture.get_Height()), (Rectangle?)objectiveEntity.UpgradeTexture.get_Bounds());
					}
				}
				if (MistwarModule.ModuleInstance.DrawObjectiveNamesSetting.get_Value())
				{
					Size2 nameSize = _font.MeasureString(objectiveEntity.Name);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity.Name, _font, new Rectangle(dest.X - (int)nameSize.Width / 2, dest.Y + height / 2 + 3, (int)nameSize.Width, (int)nameSize.Height), WvwObjectiveEntity.BrightGold, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}
	}
}
