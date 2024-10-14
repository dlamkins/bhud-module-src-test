using System.Collections.Generic;
using System.Linq;
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
	public class GuardianSpecifics : ProfessionSpecifics
	{
		private readonly Dictionary<int, DetailedTexture> _fivePages = new Dictionary<int, DetailedTexture>
		{
			{
				0,
				new DetailedTexture(1636723)
			},
			{
				1,
				new DetailedTexture(1636724)
			},
			{
				2,
				new DetailedTexture(1636725)
			},
			{
				3,
				new DetailedTexture(1636726)
			},
			{
				4,
				new DetailedTexture(1636727)
			},
			{
				5,
				new DetailedTexture(1636728)
			}
		};

		private readonly Dictionary<int, DetailedTexture> _eightPages = new Dictionary<int, DetailedTexture>
		{
			{
				0,
				new DetailedTexture(1636729)
			},
			{
				1,
				new DetailedTexture(1636730)
			},
			{
				2,
				new DetailedTexture(1636731)
			},
			{
				3,
				new DetailedTexture(1636732)
			},
			{
				4,
				new DetailedTexture(1636733)
			},
			{
				5,
				new DetailedTexture(1636734)
			},
			{
				6,
				new DetailedTexture(1636735)
			},
			{
				7,
				new DetailedTexture(1636736)
			},
			{
				8,
				new DetailedTexture(1636737)
			}
		};

		private readonly DetailedTexture _pagesBackground = new DetailedTexture(1636722);

		private readonly DetailedTexture _pages = new DetailedTexture(1636728);

		protected override SkillIcon[] Skills { get; } = new SkillIcon[3]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public GuardianSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 90;
			int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
			if (num.HasValue && num.GetValueOrDefault() == 62)
			{
				_pagesBackground.Bounds = new Rectangle(xOffset + 10, 50, 256, 64);
				_pages.Bounds = new Rectangle(xOffset + 125, 50, 140, 44);
				for (int j = 0; j < Skills.Length; j++)
				{
					Skills[j].Bounds = new Rectangle(xOffset + 3 + j * 40, 53, 38, 38);
				}
			}
			else
			{
				for (int i = 0; i < Skills.Length; i++)
				{
					Skills[i].Bounds = new Rectangle(xOffset + 100 + i * 42, 56, 42, 42);
				}
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
			if (num.HasValue && num.GetValueOrDefault() == 62)
			{
				_pagesBackground.Draw(this, spriteBatch);
				_pages.Draw(this, spriteBatch);
			}
			for (int i = 0; i < Skills.Length; i++)
			{
				Skills[i].Draw(this, spriteBatch, base.RelativeMousePosition);
			}
		}

		protected override void ApplyTemplate()
		{
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills;
			if (base.TemplatePresenter?.Template != null && base.Data.IsLoaded)
			{
				base.ApplyTemplate();
				skills = base.Data?.Professions?[ProfessionType.Guardian]?.Skills;
				if (skills != null)
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = GetSkill(SkillSlot.Profession2);
					Skills[2].Skill = GetSkill(SkillSlot.Profession3);
				}
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? GetSkill(SkillSlot slot)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = null;
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item in skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot))
				{
					if (item.Id != 41380)
					{
						if (skill == null)
						{
							skill = ((item.Specialization == base.TemplatePresenter?.Template.EliteSpecialization?.Id || item.Specialization == 0) ? item : skill);
						}
						if (item.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id && skill.Specialization == 0)
						{
							skill = item;
						}
					}
				}
				return skill;
			}
		}
	}
}
