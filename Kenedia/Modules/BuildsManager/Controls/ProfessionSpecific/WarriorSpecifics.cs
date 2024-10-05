using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
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

		protected override SkillIcon[] Skills { get; } = new SkillIcon[2]
		{
			new SkillIcon(),
			new SkillIcon()
		};


		public WarriorSpecifics(TemplatePresenter template)
			: base(template)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 90;
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
			case 68:
			{
				for (int i = 0; i < _charges.Length; i++)
				{
					_charges[i].Bounds = new Rectangle(xOffset + 90 + 1 + i * 17, 57, 12, 24);
				}
				Skills[0].Bounds = new Rectangle(xOffset, 56, 42, 42);
				Skills[1].Bounds = new Rectangle(xOffset + 44, 56, 42, 42);
				_barBackground.Bounds = new Rectangle(xOffset + 90, 83, 165, 14);
				break;
			}
			case 18:
				_emptyAdrenalin.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_emptyAdrenalin.TextureRegion = new Rectangle(0, 25, 217, 14);
				_adrenalin1.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin1.TextureRegion = new Rectangle(0, 25, 217, 14);
				_adrenalin2.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin2.TextureRegion = new Rectangle(0, 25, 217, 14);
				_adrenalin3.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin3.TextureRegion = new Rectangle(0, 25, 217, 14);
				Skills[0].Bounds = new Rectangle(xOffset + 211, 56, 42, 42);
				Skills[1].Bounds = new Rectangle(xOffset + 211 - 45, 56, 42, 42);
				break;
			case 61:
				_emptyAdrenalin.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_emptyAdrenalin.TextureRegion = new Rectangle(0, 25, 145, 14);
				_adrenalin1.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin1.TextureRegion = new Rectangle(0, 25, 145, 14);
				_adrenalin2.Bounds = new Rectangle(xOffset + 3, 80, 163, 14);
				_adrenalin2.TextureRegion = new Rectangle(0, 25, 145, 14);
				Skills[0].Bounds = new Rectangle(xOffset + 211, 56, 42, 42);
				Skills[1].Bounds = new Rectangle(xOffset + 211 - 45, 56, 42, 42);
				break;
			default:
				_emptyAdrenalin.Bounds = new Rectangle(xOffset + 3, 80, 208, 14);
				_emptyAdrenalin.TextureRegion = new Rectangle(0, 25, 217, 14);
				_adrenalin1.Bounds = new Rectangle(xOffset + 3, 80, 208, 14);
				_adrenalin1.TextureRegion = new Rectangle(0, 25, 217, 14);
				_adrenalin2.Bounds = new Rectangle(xOffset + 3, 80, 208, 14);
				_adrenalin2.TextureRegion = new Rectangle(0, 25, 217, 14);
				_adrenalin3.Bounds = new Rectangle(xOffset + 3, 80, 208, 14);
				_adrenalin3.TextureRegion = new Rectangle(0, 25, 217, 14);
				Skills[0].Bounds = new Rectangle(xOffset + 211, 56, 42, 42);
				break;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
			{
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
			if (base.TemplatePresenter?.Template == null)
			{
				return;
			}
			base.ApplyTemplate();
			Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills = BuildsManager.Data?.Professions?[ProfessionType.Warrior]?.Skills;
			if (skills != null)
			{
				bool flag;
				switch (base.TemplatePresenter.Template.EliteSpecialization?.Id)
				{
				case 18:
				case 61:
					flag = true;
					break;
				default:
					flag = false;
					break;
				}
				if (flag)
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession2);
					Skills[1].Skill = GetSkill(SkillSlot.Profession1);
				}
				else
				{
					Skills[0].Skill = GetSkill(SkillSlot.Profession1);
					Skills[1].Skill = GetSkill(SkillSlot.Profession2);
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
					return skills[44165];
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
						foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Skill item2 in from x in slotSkills
							where x.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || x.Specialization == 0
							where !x.WeaponType.HasValue || x.WeaponType == weapon
							orderby x.Specialization == 0
							select x)
						{
							if (skill == null)
							{
								skill = ((item2.Specialization == base.TemplatePresenter?.Template?.EliteSpecialization?.Id || item2.Specialization == 0) ? item2 : skill);
							}
						}
						return skill;
					}
				}
				return skill;
			}
		}
	}
}
