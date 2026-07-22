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
	public class WarriorSpecifics : ProfessionSpecifics
	{
		private readonly DetailedTexture _emptyAdrenalin = new DetailedTexture(156441);

		private readonly DetailedTexture _adrenalin1 = new DetailedTexture(156442);

		private readonly DetailedTexture _adrenalin2 = new DetailedTexture(156443);

		private readonly DetailedTexture _adrenalin3 = new DetailedTexture(156444);

		private Microsoft.Xna.Framework.Rectangle _separatorBounds;

		private readonly DetailedTexture _motivationBackground = new DetailedTexture(3680713);

		private readonly DetailedTexture _motivation = new DetailedTexture(3680713, 3680717);

		private readonly DetailedTexture _barBackground = new DetailedTexture(1636710);

		private readonly DetailedTexture _bladeswornCharges = new DetailedTexture(2492047, 2492048);

		private readonly DetailedTexture[] _charges = new DetailedTexture[10]
		{
			new DetailedTexture(2492048),
			new DetailedTexture(2492048),
			new DetailedTexture(2492048),
			new DetailedTexture(2492048),
			new DetailedTexture(2492048),
			new DetailedTexture(2492047),
			new DetailedTexture(2492047),
			new DetailedTexture(2492047),
			new DetailedTexture(2492047),
			new DetailedTexture(2492047)
		};

		protected override SkillIcon[] Skills { get; } = new SkillIcon[4]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public WarriorSpecifics(TemplatePresenter template, Data data)
			: base(template, data)
		{
			template.GearCodeChanged += new EventHandler(Template_GearCodeChanged);
		}

		private void Template_GearCodeChanged(object sender, EventArgs e)
		{
			ApplyTemplate();
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int xOffset = 90;
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 74:
			{
				xOffset = 68;
				int adrenalin_size = 280;
				_emptyAdrenalin.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset, 80, adrenalin_size, 14);
				_emptyAdrenalin.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin1.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset, 80, adrenalin_size, 14);
				_adrenalin1.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin2.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset, 80, adrenalin_size, 14);
				_adrenalin2.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin3.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset, 80, adrenalin_size, 14);
				_adrenalin3.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				Skills[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset, 30, 42, 42);
				_separatorBounds = new Microsoft.Xna.Framework.Rectangle(Skills[0].Bounds.Right + 10, Skills[1].Bounds.Top + 3, 2, Skills[1].Bounds.Height - 6);
				_motivation.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 210, 20, 64, 64);
				for (int i = 1; i < 4; i++)
				{
					Skills[i].Bounds = new Microsoft.Xna.Framework.Rectangle(_separatorBounds.Right + 10 + (i - 1) * 48, 30, 42, 42);
				}
				break;
			}
			case 68:
			{
				for (int j = 0; j < _charges.Length; j++)
				{
					_charges[j].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 90 + 1 + j * 17, 57, 12, 24);
				}
				Skills[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset, 56, 42, 42);
				Skills[1].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 44, 56, 42, 42);
				_barBackground.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 90, 83, 165, 14);
				break;
			}
			case 18:
				_emptyAdrenalin.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_emptyAdrenalin.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin1.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin1.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin2.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin2.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin3.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin3.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				Skills[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 211, 56, 42, 42);
				Skills[1].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 211 - 45, 56, 42, 42);
				break;
			case 61:
				_emptyAdrenalin.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_emptyAdrenalin.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 145, 14);
				_adrenalin1.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin1.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 145, 14);
				_adrenalin2.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin2.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 145, 14);
				Skills[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 211, 56, 42, 42);
				Skills[1].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 211 - 45, 56, 42, 42);
				break;
			default:
				_emptyAdrenalin.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 208, 14);
				_emptyAdrenalin.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin1.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 208, 14);
				_adrenalin1.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin2.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 208, 14);
				_adrenalin2.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				_adrenalin3.Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 3, 80, 208, 14);
				_adrenalin3.TextureRegion = new Microsoft.Xna.Framework.Rectangle(0, 25, 217, 14);
				Skills[0].Bounds = new Microsoft.Xna.Framework.Rectangle(xOffset + 211, 56, 42, 42);
				break;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			RecalculateLayout();
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 74:
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				Skills[1].Draw(this, spriteBatch, base.RelativeMousePosition);
				Skills[2].Draw(this, spriteBatch, base.RelativeMousePosition);
				Skills[3].Draw(this, spriteBatch, base.RelativeMousePosition);
				_emptyAdrenalin.Draw(this, spriteBatch);
				_adrenalin1.Draw(this, spriteBatch);
				_adrenalin2.Draw(this, spriteBatch);
				_adrenalin3.Draw(this, spriteBatch);
				_motivation.Draw(this, spriteBatch, base.RelativeMousePosition, _motivation.Bounds.Contains(base.RelativeMousePosition) ? new Microsoft.Xna.Framework.Color?(new Microsoft.Xna.Framework.Color(32, 32, 32, 180)) : new Microsoft.Xna.Framework.Color?(new Microsoft.Xna.Framework.Color(32, 32, 32, 120)));
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _separatorBounds, Microsoft.Xna.Framework.Color.Black);
				break;
			case 68:
			{
				_barBackground.Draw(this, spriteBatch);
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				Skills[1].Draw(this, spriteBatch, base.RelativeMousePosition);
				for (int i = 0; i < _charges.Length; i++)
				{
					_charges[i].Draw(this, spriteBatch);
				}
				break;
			}
			case 61:
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				Skills[1].Draw(this, spriteBatch, base.RelativeMousePosition);
				_emptyAdrenalin.Draw(this, spriteBatch);
				_adrenalin1.Draw(this, spriteBatch);
				_adrenalin2.Draw(this, spriteBatch);
				break;
			case 18:
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				Skills[1].Draw(this, spriteBatch, base.RelativeMousePosition);
				_emptyAdrenalin.Draw(this, spriteBatch);
				_adrenalin1.Draw(this, spriteBatch);
				_adrenalin2.Draw(this, spriteBatch);
				_adrenalin3.Draw(this, spriteBatch);
				break;
			default:
				Skills[0].Draw(this, spriteBatch, base.RelativeMousePosition);
				_emptyAdrenalin.Draw(this, spriteBatch);
				_adrenalin1.Draw(this, spriteBatch);
				_adrenalin2.Draw(this, spriteBatch);
				_adrenalin3.Draw(this, spriteBatch);
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
			SkillDictionary skills = base.Data?.Professions?[ProfessionType.Warrior]?.Skills;
			if (skills != null)
			{
				switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
				{
				case 18:
				case 61:
					Skills[0].Skill = GetSkill(SkillSlot.Profession2);
					Skills[1].Skill = GetSkill(SkillSlot.Profession1);
					break;
				default:
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = GetSkill(SkillSlot.Profession2);
					Skills[2].Skill = GetSkill(SkillSlot.Profession3);
					Skills[3].Skill = GetSkill(SkillSlot.Profession4);
					break;
				}
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? GetSkill(SkillSlot slot)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = null;
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization = base.TemplatePresenter.Template.EliteSpecialization;
				bool num = eliteSpecialization != null && eliteSpecialization!.Id == 61;
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? eliteSpecialization2 = base.TemplatePresenter.Template.EliteSpecialization;
				bool bladesworn = eliteSpecialization2 != null && eliteSpecialization2!.Id == 68;
				if (num && slot == SkillSlot.Profession2)
				{
					return skills.Get(44165);
				}
				IEnumerable<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> slotSkills = skills.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Slot == slot);
				if (bladesworn || slot != SkillSlot.Profession1)
				{
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
					{
						foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item in enumerable2)
						{
							if (skill == null)
							{
								skill = ((item.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || item.Specialization == 0) ? item : skill);
							}
						}
						return skill;
					}
				}
				if (!string.IsNullOrEmpty(base.TemplatePresenter.Template?.MainHand?.Weapon?.WeaponType.ToString()))
				{
					Weapon.WeaponType weapon = (Weapon.WeaponType)Enum.Parse(typeof(Weapon.WeaponType), base.TemplatePresenter.Template?.MainHand?.Weapon?.WeaponType.ToString());
					if (weapon != 0)
					{
						IOrderedEnumerable<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> orderedEnumerable = from x in slotSkills
							where x.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || x.Specialization == 0
							where !x.WeaponType.HasValue || x.WeaponType == weapon
							orderby x.Specialization == 0
							select x;
						List<int> underwater_skills = new List<int>(1) { 14443 };
						{
							foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item2 in orderedEnumerable)
							{
								if (!underwater_skills.Contains(item2.Id) && skill == null)
								{
									skill = ((item2.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || item2.Specialization == 0) ? item2 : skill);
								}
							}
							return skill;
						}
					}
				}
				return skill;
			}
		}

		protected override void DisposeControl()
		{
			base.TemplatePresenter.GearCodeChanged -= new EventHandler(Template_GearCodeChanged);
			base.DisposeControl();
		}
	}
}
