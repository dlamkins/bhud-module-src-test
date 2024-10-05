using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class MesmerSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture[] _clones = new DetailedTexture[5]
		{
			new DetailedTexture(156430),
			new DetailedTexture(156430),
			new DetailedTexture(156430),
			new DetailedTexture(156429),
			new DetailedTexture(156429)
		};

		private Rectangle _separatorBounds;

		protected override SkillIcon[] Skills { get; } = new SkillIcon[5]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public MesmerSpecifics(TemplatePresenter template)
			: base(template)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 70;
			int? num = base.TemplatePresenter?.Template?.EliteSpecialization?.Id;
			if (num.HasValue && num.GetValueOrDefault() == 66)
			{
				for (int k = 0; k < _clones.Length; k++)
				{
					_clones[k].Bounds = new Rectangle(xOffset + 90 + k * 24, 24, 30, 30);
				}
			}
			else
			{
				for (int j = 0; j < 3; j++)
				{
					_clones[j].Bounds = new Rectangle(xOffset + 80 + j * 32, 12, 42, 42);
				}
			}
			for (int i = 0; i < Skills.Length; i++)
			{
				Skills[i].Bounds = new Rectangle(xOffset + ((i == 4) ? 10 : 0) + 42 + i * 46, 52, 42, 42);
			}
			Rectangle p = Skills[3].Bounds;
			_separatorBounds = new Rectangle(((Rectangle)(ref p)).get_Right() + 6, p.Y, 2, p.Height);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 66:
			{
				for (int m = 0; m < _clones.Length; m++)
				{
					_clones[m].Draw(this, spriteBatch);
				}
				for (int n = 0; n < 5; n++)
				{
					Skills[n].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Color.get_Black());
				break;
			}
			case 40:
			{
				for (int k = 0; k < 3; k++)
				{
					_clones[k].Draw(this, spriteBatch);
				}
				for (int l = 0; l < 5; l++)
				{
					Skills[l].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Color.get_Black());
				break;
			}
			default:
			{
				for (int i = 0; i < 3; i++)
				{
					_clones[i].Draw(this, spriteBatch);
				}
				for (int j = 0; j < 4; j++)
				{
					Skills[j].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
			}
		}

		protected override void ApplyTemplate()
		{
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills;
			if (base.TemplatePresenter?.Template != null)
			{
				base.ApplyTemplate();
				skills = BuildsManager.Data?.Professions?[ProfessionType.Mesmer]?.Skills;
				if (skills != null)
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = GetSkill(SkillSlot.Profession2);
					Skills[2].Skill = GetSkill(SkillSlot.Profession3);
					Skills[3].Skill = GetSkill(SkillSlot.Profession4);
					Skills[4].Skill = GetSkill(SkillSlot.Profession5);
				}
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill GetSkill(SkillSlot slot)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = null;
				IEnumerable<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> slotSkills = skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot);
				IEnumerable<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> enumerable2;
				if ((base.TemplatePresenter?.Template?.EliteSpecialization?.Id).GetValueOrDefault() == 0)
				{
					IEnumerable<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> enumerable = Array.Empty<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>();
					enumerable2 = enumerable;
				}
				else
				{
					enumerable2 = slotSkills.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill x) => x.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id);
				}
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item2 in enumerable2)
				{
					if (skill == null)
					{
						skill = ((item2.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || item2.Specialization == 0) ? item2 : skill);
					}
				}
				if (skill != null)
				{
					return skill;
				}
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item in skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot))
				{
					if (skill == null)
					{
						skill = ((item.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || item.Specialization == 0) ? item : skill);
					}
				}
				return skill;
			}
		}
	}
}
