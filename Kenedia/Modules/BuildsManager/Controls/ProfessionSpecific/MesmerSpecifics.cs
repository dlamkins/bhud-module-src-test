using System;
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

		private readonly DetailedTexture _notesRegionBackground = new DetailedTexture(3680685);

		private readonly DetailedTexture[] _notes = new DetailedTexture[3]
		{
			new DetailedTexture(3680686),
			new DetailedTexture(3680688),
			new DetailedTexture(3680690)
		};

		private readonly DetailedTexture[] _notesBackground = new DetailedTexture[3]
		{
			new DetailedTexture(3680687),
			new DetailedTexture(3680689),
			new DetailedTexture(3680691)
		};

		private Microsoft.Xna.Framework.Color _notesColor = new Microsoft.Xna.Framework.Color(198, 100, 231);

		private Microsoft.Xna.Framework.Rectangle _separatorBounds;

		protected override SkillIcon[] Skills { get; } = new SkillIcon[5]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public MesmerSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int xOffset = 70;
			switch (base.TemplatePresenter?.Template?.EliteSpecialization?.Id)
			{
			case 73:
				_notesRegionBackground.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 85, 0, 125, 42);
				_notesBackground[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 95, 6, 36, 36);
				_notes[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 95, 6, 36, 36);
				_notesBackground[1].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 135, 0, 36, 36);
				_notes[1].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 135, 0, 36, 36);
				_notesBackground[2].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 165, 2, 36, 36);
				_notes[2].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 165, 2, 36, 36);
				break;
			case 66:
			{
				for (int j = 0; j < _clones.Length; j++)
				{
					_clones[j].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 90 + j * 24, 24, 30, 30);
				}
				break;
			}
			default:
			{
				for (int k = 0; k < 3; k++)
				{
					_clones[k].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 80 + k * 32, 12, 42, 42);
				}
				break;
			}
			}
			for (int i = 0; i < Skills.Length; i++)
			{
				Skills[i].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + ((i == 4) ? 10 : 0) + 42 + i * 46, 52, 42, 42);
			}
			Microsoft.Xna.Framework.Rectangle p = Skills[3].Bounds;
			_separatorBounds = new Microsoft.Xna.Framework.Rectangle(p.Right + 6, p.Y, 2, p.Height);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 73:
			{
				_notesRegionBackground.Draw(this, spriteBatch);
				DetailedTexture[] notesBackground = _notesBackground;
				foreach (DetailedTexture obj in notesBackground)
				{
					Microsoft.Xna.Framework.Color? color = Microsoft.Xna.Framework.Color.Black;
					obj.Draw(this, spriteBatch, null, color);
				}
				notesBackground = _notes;
				foreach (DetailedTexture obj2 in notesBackground)
				{
					Microsoft.Xna.Framework.Color? color = _notesColor;
					obj2.Draw(this, spriteBatch, null, color);
				}
				for (int i2 = 0; i2 < 5; i2++)
				{
					Skills[i2].Draw(this, spriteBatch, base.RelativeMousePosition);
				}
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Microsoft.Xna.Framework.Color.Black);
				break;
			}
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
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Microsoft.Xna.Framework.Color.Black);
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
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Microsoft.Xna.Framework.Color.Black);
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
			if (base.TemplatePresenter?.Template == null || !base.Data.IsLoaded)
			{
				return;
			}
			base.ApplyTemplate();
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Mesmer]?.Skills;
			if (skills != null)
			{
				Skills[0].Skill = GetSkill(SkillSlot.Profession1);
				Skills[1].Skill = GetSkill(SkillSlot.Profession2);
				Skills[2].Skill = GetSkill(SkillSlot.Profession3);
				Skills[3].Skill = GetSkill(SkillSlot.Profession4);
				Skills[4].Skill = GetSkill(SkillSlot.Profession5);
				int? num = base.TemplatePresenter.Template.EliteSpecialization?.Id;
				if (num.HasValue && num.GetValueOrDefault() == 73)
				{
					Skills[4].Skill = skills.Get(76931);
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
