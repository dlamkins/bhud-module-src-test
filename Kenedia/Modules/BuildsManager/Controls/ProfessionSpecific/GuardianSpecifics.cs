using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
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

		private Microsoft.Xna.Framework.Rectangle _separatorBounds;

		protected override SkillIcon[] Skills { get; } = new SkillIcon[4]
		{
			new SkillIcon(),
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
			base.RecalculateLayout();
			int xOffset = 90;
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 81:
			{
				for (int j = 0; j < Skills.Length; j++)
				{
					Skills[j].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset - 10 + j * 44 + ((j == 3) ? 18 : 0), 56, 42, 42);
					if (j == 3)
					{
						_separatorBounds = new Microsoft.Xna.Framework.Rectangle(Skills[j].Bounds.Left - 10, Skills[j].Bounds.Top - 2, 2, Skills[j].Bounds.Height + 4);
					}
				}
				break;
			}
			case 62:
			{
				_pagesBackground.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 10, 50, 256, 64);
				_pages.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 125, 50, 140, 44);
				for (int k = 0; k < Skills.Length; k++)
				{
					Skills[k].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3 + k * 40, 53, 38, 38);
				}
				break;
			}
			default:
			{
				for (int i = 0; i < Skills.Length; i++)
				{
					Skills[i].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 100 + i * 42, 56, 42, 42);
				}
				break;
			}
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 81:
			{
				for (int k = 0; k < Skills.Length; k++)
				{
					Skills[k].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Microsoft.Xna.Framework.Color.White);
				break;
			}
			case 62:
			{
				_pagesBackground.Draw(this, spriteBatch);
				_pages.Draw(this, spriteBatch);
				for (int j = 0; j < 4; j++)
				{
					Skills[j].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
			default:
			{
				for (int i = 0; i < 4; i++)
				{
					Skills[i].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				break;
			}
			}
		}

		protected override void ApplyTemplate()
		{
			if (base.TemplatePresenter?.Template == null || !base.Data.IsLoaded)
			{
				return;
			}
			base.ApplyTemplate();
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Guardian]?.Skills;
			if (skills != null)
			{
				int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
				if (num.HasValue && num.GetValueOrDefault() == 81)
				{
					Skills[0].Skill = skills.Get(78837);
					Skills[1].Skill = skills.Get(78604);
					Skills[2].Skill = skills.Get(78358);
					Skills[3].Skill = skills.Get(77073);
				}
				else
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = GetSkill(SkillSlot.Profession2);
					Skills[2].Skill = GetSkill(SkillSlot.Profession3);
					Skills[3].Skill = null;
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
