using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillIcon : DetailedTexture
	{
		private readonly AsyncTexture2D _noAquaticFlagTexture = AsyncTexture2D.FromAssetId(157145);

		private Rectangle _noAquaticFlagTextureRegion;

		private Skill _skill;

		public Skill Skill
		{
			get
			{
				return _skill;
			}
			set
			{
				Common.SetProperty(ref _skill, value, new Action(ApplySkill));
			}
		}

		public AsyncTexture2D HoveredFrameTexture { get; private set; }

		public AsyncTexture2D AutoCastTexture { get; set; }

		public Rectangle HoveredFrameTextureRegion { get; }

		public Rectangle AutoCastTextureRegion { get; }

		public SkillSlotType Slot { get; set; }

		public bool ShowSelector { get; set; }

		public DetailedTexture Selector { get; } = new DetailedTexture(157138, 157140);


		public SkillIcon()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			base.FallBackTexture = AsyncTexture2D.FromAssetId(157154);
			HoveredFrameTexture = AsyncTexture2D.FromAssetId(157143);
			base.TextureRegion = new Rectangle(14, 14, 100, 100);
			HoveredFrameTextureRegion = new Rectangle(8, 8, 112, 112);
			AutoCastTextureRegion = new Rectangle(6, 6, 52, 52);
			_noAquaticFlagTextureRegion = new Rectangle(16, 16, 96, 96);
		}

		private void ApplySkill()
		{
			base.Texture = Skill?.Icon;
		}

		public void Draw(Control ctrl, SpriteBatch spriteBatch, bool terrestrial = true, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			base.Draw(ctrl, spriteBatch, mousePos, color, bgColor, forceHover, rotation, origin);
			Color valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.get_White();
				color = valueOrDefault;
			}
			Vector2 valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.get_Zero();
				origin = valueOrDefault2;
			}
			float valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			Color borderColor = Color.get_Black();
			Texture2D pixel = ContentService.Textures.Pixel;
			Rectangle bounds = base.Bounds;
			int left = ((Rectangle)(ref bounds)).get_Left();
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel, new Rectangle(left, ((Rectangle)(ref bounds)).get_Top(), base.Bounds.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel2 = ContentService.Textures.Pixel;
			bounds = base.Bounds;
			int left2 = ((Rectangle)(ref bounds)).get_Left();
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel2, new Rectangle(left2, ((Rectangle)(ref bounds)).get_Bottom() - 1, base.Bounds.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel3 = ContentService.Textures.Pixel;
			bounds = base.Bounds;
			int left3 = ((Rectangle)(ref bounds)).get_Left();
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel3, new Rectangle(left3, ((Rectangle)(ref bounds)).get_Top(), 1, base.Bounds.Height), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel4 = ContentService.Textures.Pixel;
			bounds = base.Bounds;
			int num = ((Rectangle)(ref bounds)).get_Right() - 1;
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel4, new Rectangle(num, ((Rectangle)(ref bounds)).get_Top(), 1, base.Bounds.Height), Rectangle.get_Empty(), borderColor * 0.6f);
			valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.get_White();
				color = valueOrDefault;
			}
			valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.get_Zero();
				origin = valueOrDefault2;
			}
			valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			if (!terrestrial)
			{
				Skill skill = Skill;
				if (skill != null && skill.Flags.HasFlag(SkillFlag.NoUnderwater))
				{
					spriteBatch.DrawOnCtrl(ctrl, _noAquaticFlagTexture, base.Bounds, _noAquaticFlagTextureRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
					goto IL_02bd;
				}
			}
			if (base.Hovered)
			{
				spriteBatch.DrawOnCtrl(ctrl, HoveredFrameTexture, base.Bounds, HoveredFrameTextureRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
			}
			goto IL_02bd;
			IL_02bd:
			if (ShowSelector)
			{
				Selector.Draw(ctrl, spriteBatch, mousePos);
			}
		}

		public override void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			base.Draw(ctrl, spriteBatch, mousePos, color, bgColor, forceHover, rotation, origin);
			Color valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.get_White();
				color = valueOrDefault;
			}
			Vector2 valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.get_Zero();
				origin = valueOrDefault2;
			}
			float valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			Color borderColor = Color.get_Black();
			Texture2D pixel = ContentService.Textures.Pixel;
			Rectangle bounds = base.Bounds;
			int left = ((Rectangle)(ref bounds)).get_Left();
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel, new Rectangle(left, ((Rectangle)(ref bounds)).get_Top(), base.Bounds.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel2 = ContentService.Textures.Pixel;
			bounds = base.Bounds;
			int left2 = ((Rectangle)(ref bounds)).get_Left();
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel2, new Rectangle(left2, ((Rectangle)(ref bounds)).get_Bottom() - 1, base.Bounds.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel3 = ContentService.Textures.Pixel;
			bounds = base.Bounds;
			int left3 = ((Rectangle)(ref bounds)).get_Left();
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel3, new Rectangle(left3, ((Rectangle)(ref bounds)).get_Top(), 1, base.Bounds.Height), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel4 = ContentService.Textures.Pixel;
			bounds = base.Bounds;
			int num = ((Rectangle)(ref bounds)).get_Right() - 1;
			bounds = base.Bounds;
			spriteBatch.DrawOnCtrl(ctrl, pixel4, new Rectangle(num, ((Rectangle)(ref bounds)).get_Top(), 1, base.Bounds.Height), Rectangle.get_Empty(), borderColor * 0.6f);
			if (AutoCastTexture != null)
			{
				spriteBatch.DrawOnCtrl(ctrl, AutoCastTexture, base.Bounds.Add(-4, -4, 8, 8), AutoCastTextureRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
			}
			if (base.Hovered)
			{
				spriteBatch.DrawOnCtrl(ctrl, HoveredFrameTexture, base.Bounds, HoveredFrameTextureRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
			}
			if (ShowSelector)
			{
				Selector.Draw(ctrl, spriteBatch, mousePos);
			}
		}
	}
}
