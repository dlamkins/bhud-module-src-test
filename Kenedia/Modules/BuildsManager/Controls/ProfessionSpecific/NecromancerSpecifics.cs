using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
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


		public NecromancerSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
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
			case 76:
			{
				_lifeForceBarBackground.Bounds = new Rectangle(xOffset - 10, 75, 255, 20);
				_lifeForce.Bounds = new Rectangle(xOffset - 10, 75, 255, 20);
				_lifeForce.TextureRegion = new Rectangle(1, 42, _lifeForce.Texture.Width - 30, _lifeForce.Texture.Height - 49);
				for (int j = 1; j < 4; j++)
				{
					Skills[j].Bounds = new Rectangle(xOffset + 54 + j * 39, 28, 36, 36);
				}
				SkillIcon obj = Skills[0];
				Rectangle bounds = _lifeForce.Bounds;
				obj.Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Left() - 1, 22, 48, 48);
				break;
			}
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
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 60:
			{
				_shades.Draw(this, spriteBatch);
				_lifeForceBarBackground.Draw(this, spriteBatch);
				_lifeForceScourge.Draw(this, spriteBatch, null, Color.get_LightGray() * 0.7f);
				for (int j = 0; j < Skills.Length; j++)
				{
					Skills[j].Draw(this, spriteBatch, base.RelativeMousePosition);
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
			case 76:
			{
				_lifeForceBarBackground.Draw(this, spriteBatch);
				_lifeForce.Draw(this, spriteBatch, null, Color.get_LightGray() * 0.7f);
				spriteBatch.DrawStringOnCtrl(this, "100%", Control.Content.DefaultFont12, _lifeForce.Bounds, Color.get_White(), wrap: false, HorizontalAlignment.Center);
				for (int i = 0; i < 4; i++)
				{
					Skills[i].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
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
			if (base.TemplatePresenter?.Template == null || !base.Data.IsLoaded)
			{
				return;
			}
			base.ApplyTemplate();
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Necromancer]?.Skills;
			if (skills != null)
			{
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
					Skills[4].Skill = (skills.TryGetValue(masterSkill, out var scourgeMasterSkill) ? scourgeMasterSkill : GetSkill(SkillSlot.Profession5));
					break;
				}
				case 64:
				{
					Skills[0].Skill = (skills.TryGetValue(62567, out var harbingerSkill) ? harbingerSkill : null);
					break;
				}
				case 34:
				{
					Skills[0].Skill = (skills.TryGetValue(30792, out var reaperSkill) ? reaperSkill : null);
					break;
				}
				case 76:
				{
					Skills[0].Skill = (skills.TryGetValue(77238, out var ritualistSkill) ? ritualistSkill : null);
					Skills[1].Skill = (skills.TryGetValue(77003, out var ritualistSkill2) ? ritualistSkill2 : null);
					Skills[2].Skill = (skills.TryGetValue(76732, out var ritualistSkill3) ? ritualistSkill3 : null);
					Skills[3].Skill = (skills.TryGetValue(76602, out var ritualistSkill4) ? ritualistSkill4 : null);
					break;
				}
				default:
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					break;
				}
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
