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

namespace Nekres.Mistwar.Controls
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
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Invalid comparison between Unknown and I4
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Invalid comparison between Unknown and I4
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0578: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05de: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0695: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_0772: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0799: Unknown result type (might be due to invalid IL or missing references)
			//IL_07af: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_082c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_0842: Unknown result type (might be due to invalid IL or missing references)
			//IL_0849: Unknown result type (might be due to invalid IL or missing references)
			//IL_0854: Unknown result type (might be due to invalid IL or missing references)
			//IL_0862: Unknown result type (might be due to invalid IL or missing references)
			//IL_086a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0872: Unknown result type (might be due to invalid IL or missing references)
			//IL_0877: Unknown result type (might be due to invalid IL or missing references)
			if (WvwObjectives == null)
			{
				return;
			}
			float widthRatio = (float)bounds.Width / (float)SourceRectangle.Width;
			float heightRatio = (float)bounds.Height / (float)SourceRectangle.Height;
			if (MistwarModule.ModuleInstance.DrawSectorsSetting.get_Value())
			{
				foreach (WvwObjectiveEntity objectiveEntity in WvwObjectives)
				{
					Color teamColor = objectiveEntity.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value(), (int)(TextureOpacity * 255f));
					Vector2[] sectorBounds = objectiveEntity.Bounds.Select((Func<Point, Vector2>)delegate(Point p)
					{
						//IL_0021: Unknown result type (might be due to invalid IL or missing references)
						//IL_002c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0031: Unknown result type (might be due to invalid IL or missing references)
						//IL_0036: Unknown result type (might be due to invalid IL or missing references)
						//IL_0037: Unknown result type (might be due to invalid IL or missing references)
						//IL_003e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0045: Unknown result type (might be due to invalid IL or missing references)
						//IL_004a: Unknown result type (might be due to invalid IL or missing references)
						//IL_004d: Unknown result type (might be due to invalid IL or missing references)
						Point val2 = PointExtensions.ToBounds(new Point((int)(widthRatio * (float)p.X), (int)(heightRatio * (float)p.Y)), ((Control)this).get_AbsoluteBounds());
						return new Vector2((float)val2.X, (float)val2.Y);
					}).ToArray();
					ShapeExtensions.DrawPolygon(spriteBatch, new Vector2(0f, 0f), (IReadOnlyList<Vector2>)sectorBounds, teamColor, 3f, 0f);
				}
			}
			Rectangle dest = default(Rectangle);
			CircleF circleDest = default(CircleF);
			RectangleF shapeRectangle = default(RectangleF);
			foreach (WvwObjectiveEntity objectiveEntity2 in WvwObjectives)
			{
				Color teamColor2 = objectiveEntity2.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value());
				if (objectiveEntity2.Icon == null)
				{
					continue;
				}
				int width = (int)(ScaleRatio * (float)objectiveEntity2.Icon.get_Width());
				int height = (int)(ScaleRatio * (float)objectiveEntity2.Icon.get_Height());
				((Rectangle)(ref dest))._002Ector((int)(widthRatio * (float)objectiveEntity2.Center.X), (int)(heightRatio * (float)objectiveEntity2.Center.Y), width, height);
				Rectangle tDest = RectangleExtension.ToBounds(dest, ((Control)this).get_AbsoluteBounds());
				WvwOwner owner = objectiveEntity2.Owner;
				WvwOwner val = owner;
				if ((int)val != 3)
				{
					if ((int)val == 4)
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
							ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)tDest.X - j * (aWidth / 2f), (float)tDest.Y - j * (aHeight / 2f)), (IReadOnlyList<Vector2>)shapeDiamond, teamColor2, 1f, 0f);
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
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, teamColor2, (float)width, 0f);
						ShapeExtensions.DrawCircle(spriteBatch, circleDest, 360, Color.get_Black(), 2f, 0f);
					}
				}
				else
				{
					float aScale = 1.4f;
					((RectangleF)(ref shapeRectangle))._002Ector((float)tDest.X - aScale * (float)tDest.Width / 2f, (float)tDest.Y - aScale * (float)tDest.Height / 2f, aScale * (float)tDest.Width, aScale * (float)tDest.Height);
					ShapeExtensions.FillRectangle(spriteBatch, shapeRectangle, teamColor2, 0f);
					ShapeExtensions.DrawRectangle(spriteBatch, shapeRectangle, Color.get_Black(), 2f, 0f);
				}
				if (objectiveEntity2.HasBuff(out var remainingTime))
				{
					string text = remainingTime.ToString("m\\:ss");
					Size2 size = _font.MeasureString(text);
					Point texSize = PointExtensions.ResizeKeepAspect(new Point(objectiveEntity2.BuffTexture.get_Width(), objectiveEntity2.BuffTexture.get_Height()), (int)size.Width, (int)size.Height, false);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.BuffTexture, new Rectangle(dest.X + texSize.X / 2, dest.Y - texSize.Y + 1, texSize.X, texSize.Y), (Rectangle?)objectiveEntity2.BuffTexture.get_Bounds());
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, new Rectangle(dest.X - (int)size.Width / 2, dest.Y - (int)size.Height - dest.Height / 2 - 10, dest.Width, dest.Height), WvwObjectiveEntity.BrightGold, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.Icon, new Rectangle(dest.X - width / 2, dest.Y - height / 2, width, height), (Rectangle?)objectiveEntity2.Icon.get_Bounds(), teamColor2);
				double scale = 0.5;
				if (objectiveEntity2.IsClaimed())
				{
					if (MistwarModule.ModuleInstance.UseCustomIconsSetting.get_Value())
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.CustomClaimedTexture, new Rectangle(dest.X - width - 5, dest.Y + 10, (int)(scale * (double)objectiveEntity2.CustomClaimedTexture.get_Width()), (int)(scale * (double)objectiveEntity2.CustomClaimedTexture.get_Height())), (Rectangle?)objectiveEntity2.CustomClaimedTexture.get_Bounds());
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.ClaimedTexture, new Rectangle(dest.X + (int)(0.6 * (double)width) - objectiveEntity2.ClaimedTexture.get_Width() / 2, dest.Y + (int)(0.9 * (double)height) - height / 2, objectiveEntity2.ClaimedTexture.get_Width(), objectiveEntity2.ClaimedTexture.get_Height()), (Rectangle?)objectiveEntity2.ClaimedTexture.get_Bounds());
					}
				}
				if (objectiveEntity2.HasUpgraded())
				{
					if (MistwarModule.ModuleInstance.UseCustomIconsSetting.get_Value())
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.CustomUpgradeTexture, new Rectangle(dest.X + width / 2 - 2, dest.Y + 5, (int)(scale * (double)objectiveEntity2.CustomUpgradeTexture.get_Width()), (int)(scale * (double)objectiveEntity2.CustomUpgradeTexture.get_Height())), (Rectangle?)objectiveEntity2.CustomUpgradeTexture.get_Bounds());
					}
					else
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.UpgradeTexture, new Rectangle(dest.X - objectiveEntity2.UpgradeTexture.get_Width() / 2, dest.Y - height / 2 - (int)(0.7 * (double)objectiveEntity2.UpgradeTexture.get_Height()), objectiveEntity2.UpgradeTexture.get_Width(), objectiveEntity2.UpgradeTexture.get_Height()), (Rectangle?)objectiveEntity2.UpgradeTexture.get_Bounds());
					}
				}
				if (MistwarModule.ModuleInstance.DrawObjectiveNamesSetting.get_Value())
				{
					Size2 nameSize = _font.MeasureString(objectiveEntity2.Name);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, objectiveEntity2.Name, _font, new Rectangle(dest.X - (int)nameSize.Width / 2, dest.Y + height / 2 + 3, (int)nameSize.Width, (int)nameSize.Height), WvwObjectiveEntity.BrightGold, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}
	}
}
