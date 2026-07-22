using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillIcon : DetailedTexture
	{
		private readonly AsyncTexture2D _noAquaticFlagTexture = AsyncTexture2D.FromAssetId(157145);

		private Rectangle _noAquaticFlagTextureRegion;

		public Skill Skill
		{
			[CompilerGenerated]
			get
			{
				return _003CSkill_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSkill_003Ek__BackingField, value, delegate(Skill v)
				{
					_003CSkill_003Ek__BackingField = v;
				}, new Action(ApplySkill));
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
			base.FallBackTexture = AsyncTexture2D.FromAssetId(157154);
			HoveredFrameTexture = AsyncTexture2D.FromAssetId(157143);
			base.TextureRegion = new Rectangle(14, 14, 100, 100);
			HoveredFrameTextureRegion = new Rectangle(8, 8, 112, 112);
			AutoCastTextureRegion = new Rectangle(6, 6, 52, 52);
			_noAquaticFlagTextureRegion = new Rectangle(16, 16, 96, 96);
		}

		private void ApplySkill()
		{
			base.Texture = TexturesService.GetAsyncTexture(Skill?.IconAssetId);
		}

		public void Draw(Control ctrl, SpriteBatch spriteBatch, bool terrestrial = true, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			base.Draw(ctrl, spriteBatch, mousePos, color, bgColor, forceHover, rotation, origin);
			Color valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.White;
				color = valueOrDefault;
			}
			Vector2 valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.Zero;
				origin = valueOrDefault2;
			}
			float valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			Color borderColor = Color.Black;
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Left, base.Bounds.Top, base.Bounds.Width, 1), Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Left, base.Bounds.Bottom - 1, base.Bounds.Width, 1), Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Left, base.Bounds.Top, 1, base.Bounds.Height), Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Right - 1, base.Bounds.Top, 1, base.Bounds.Height), Rectangle.Empty, borderColor * 0.6f);
			valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.White;
				color = valueOrDefault;
			}
			valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.Zero;
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
					spriteBatch.DrawOnCtrl(ctrl, _noAquaticFlagTexture, base.Bounds, _noAquaticFlagTextureRegion, color.Value, rotation.Value, origin.Value);
					goto IL_02bd;
				}
			}
			if (base.Hovered)
			{
				spriteBatch.DrawOnCtrl(ctrl, HoveredFrameTexture, base.Bounds, HoveredFrameTextureRegion, color.Value, rotation.Value, origin.Value);
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
			base.Draw(ctrl, spriteBatch, mousePos, color, bgColor, forceHover, rotation, origin);
			Color valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.White;
				color = valueOrDefault;
			}
			Vector2 valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.Zero;
				origin = valueOrDefault2;
			}
			float valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			Color borderColor = Color.Black;
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Left, base.Bounds.Top, base.Bounds.Width, 1), Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Left, base.Bounds.Bottom - 1, base.Bounds.Width, 1), Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Left, base.Bounds.Top, 1, base.Bounds.Height), Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, new Rectangle(base.Bounds.Right - 1, base.Bounds.Top, 1, base.Bounds.Height), Rectangle.Empty, borderColor * 0.6f);
			if (AutoCastTexture != null)
			{
				spriteBatch.DrawOnCtrl(ctrl, AutoCastTexture, base.Bounds.Add(-4, -4, 8, 8), AutoCastTextureRegion, color.Value, rotation.Value, origin.Value);
			}
			if (base.Hovered)
			{
				spriteBatch.DrawOnCtrl(ctrl, HoveredFrameTexture, base.Bounds, HoveredFrameTextureRegion, color.Value, rotation.Value, origin.Value);
			}
			if (ShowSelector)
			{
				Selector.Draw(ctrl, spriteBatch, mousePos);
			}
		}
	}
}
