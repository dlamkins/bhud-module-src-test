using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class NecromancerSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture _lifeForceBarBackground = new DetailedTexture(1636710);

		private readonly DetailedTexture _lifeForceBar = new DetailedTexture(2479935);

		private readonly DetailedTexture _lifeForceScourge = new DetailedTexture(1636711);

		private readonly DetailedTexture _lifeForce = new DetailedTexture(156436);

		private readonly DetailedTexture _shades = new DetailedTexture(1636744);

		protected override SkillIcon[] Skills { get; } = new SkillIcon[5]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public NecromancerSpecifics(TemplatePresenter template)
			: base(template)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 80;
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 60:
			{
				_shades.Bounds = new Rectangle(xOffset + 10 + 46, 28, 36, 36);
				_shades.TextureRegion = new Rectangle(0, 2, _shades.Texture.Width, _shades.Texture.Height - 4);
				Skills[0].Bounds = new Rectangle(xOffset + 10, 25, 42, 42);
				for (int i = 1; i < Skills.Length; i++)
				{
					Skills[i].Bounds = new Rectangle(xOffset + 54 + i * 39, 28, 36, 36);
				}
				_lifeForceBarBackground.Bounds = new Rectangle(xOffset + 10, 75, 250, 20);
				_lifeForceScourge.Bounds = new Rectangle(xOffset + 11, 76, 247, 18);
				break;
			}
			case 64:
				_lifeForceBarBackground.Bounds = new Rectangle(xOffset + 10, 70, 205, 20);
				_lifeForceBar.Bounds = new Rectangle(xOffset + 11, 71, 203, 18);
				Skills[0].Bounds = new Rectangle(xOffset + 215, 55, 42, 42);
				break;
			default:
				_lifeForceBarBackground.Bounds = new Rectangle(xOffset + 10, 70, 205, 20);
				_lifeForce.Bounds = new Rectangle(xOffset + 10, 70, 205, 20);
				_lifeForce.TextureRegion = new Rectangle(1, 42, _lifeForce.Texture.Width - 30, _lifeForce.Texture.Height - 49);
				Skills[0].Bounds = new Rectangle(xOffset + 215, 55, 42, 42);
				break;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 60:
			{
				_shades.Draw(this, spriteBatch);
				_lifeForceBarBackground.Draw(this, spriteBatch);
				_lifeForceScourge.Draw(this, spriteBatch, null, Color.get_LightGray() * 0.7f);
				for (int i = 0; i < Skills.Length; i++)
				{
					Skills[i].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				spriteBatch.DrawStringOnCtrl(this, "100%", Control.Content.DefaultFont12, _lifeForceScourge.Bounds, Color.get_White(), wrap: false, HorizontalAlignment.Center);
				break;
			}
			case 64:
				_lifeForceBarBackground.Draw(this, spriteBatch);
				_lifeForceBar.Draw(this, spriteBatch, null, Color.get_LightGray() * 0.7f);
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				spriteBatch.DrawStringOnCtrl(this, "100%", Control.Content.DefaultFont12, _lifeForceBar.Bounds, Color.get_White(), wrap: false, HorizontalAlignment.Center);
				break;
			default:
				_lifeForceBarBackground.Draw(this, spriteBatch);
				_lifeForce.Draw(this, spriteBatch, null, Color.get_LightGray() * 0.7f);
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				spriteBatch.DrawStringOnCtrl(this, "100%", Control.Content.DefaultFont12, _lifeForce.Bounds, Color.get_White(), wrap: false, HorizontalAlignment.Center);
				break;
			}
		}

		protected override void ApplyTemplate()
		{
			if (base.TemplatePresenter?.Template == null)
			{
				return;
			}
			base.ApplyTemplate();
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills = BuildsManager.Data?.Professions?[ProfessionType.Necromancer]?.Skills;
			if (skills == null)
			{
				return;
			}
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 60:
			{
				Skills[0].Skill = GetSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetSkill(SkillSlot.Profession2);
				Skills[2].Skill = GetSkill(SkillSlot.Profession3);
				Skills[3].Skill = GetSkill(SkillSlot.Profession4);
				DetailedTexture shades = _shades;
				Kenedia.Modules.BuildsManager.DataModels.Professions.Trait? grandMaster = base.TemplatePresenter.Template.Specializations.Specialization3.Traits.GrandMaster;
				shades.Texture = ((grandMaster != null && grandMaster!.Id == 2112) ? AsyncTexture2D.FromAssetId(1636742) : AsyncTexture2D.FromAssetId(1636744));
				int masterSkill = (base.TemplatePresenter.Template.Specializations.Specialization3.Traits.Master?.Skills?.FirstOrDefault()).GetValueOrDefault();
				Skills[4].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == masterSkill) ?? GetSkill(SkillSlot.Profession5);
				break;
			}
			case 64:
				Skills[0].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 62567);
				break;
			case 34:
				Skills[0].Skill = skills.Values.FirstOrDefault((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id == 30792);
				break;
			default:
				Skills[0].Skill = GetSkill(SkillSlot.Profession1);
				break;
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? GetSkill(SkillSlot slot)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = null;
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item in skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot))
				{
					if (skill == null)
					{
						skill = ((item.Specialization == base.TemplatePresenter.Template.EliteSpecialization?.Id || item.Specialization == 0) ? item : skill);
					}
					if (item.Specialization == base.TemplatePresenter.Template.EliteSpecialization?.Id && skill.Specialization == 0)
					{
						skill = item;
					}
				}
				return skill;
			}
		}
	}
}
