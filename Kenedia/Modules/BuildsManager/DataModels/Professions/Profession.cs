using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Profession : IDisposable, IDataMember
	{
		private bool _isDisposed;

		private AsyncTexture2D _icon;

		private AsyncTexture2D _iconBig;

		[DataMember]
		public ProfessionType Id { get; set; }

		public AsyncTexture2D Icon
		{
			get
			{
				if (_icon != null)
				{
					return _icon;
				}
				if (IconAssetId != 0)
				{
					_icon = AsyncTexture2D.FromAssetId(IconAssetId);
				}
				return _icon;
			}
		}

		public AsyncTexture2D IconBig
		{
			get
			{
				if (_iconBig != null)
				{
					return _iconBig;
				}
				if (IconBigAssetId != 0)
				{
					_iconBig = AsyncTexture2D.FromAssetId(IconBigAssetId);
				}
				return _iconBig;
			}
		}

		[DataMember]
		public int IconAssetId { get; set; }

		[DataMember]
		public int IconBigAssetId { get; set; }

		[DataMember]
		public Dictionary<int, Specialization> Specializations { get; set; } = new Dictionary<int, Specialization>();


		[DataMember]
		public Dictionary<Weapon.WeaponType, Weapon> Weapons { get; set; } = new Dictionary<Weapon.WeaponType, Weapon>();


		public string Name
		{
			get
			{
				return Names.Text;
			}
			set
			{
				Names.Text = value;
			}
		}

		[DataMember]
		public LocalizedString Names { get; protected set; } = new LocalizedString();


		[DataMember]
		public Dictionary<int, Skill> Skills { get; set; } = new Dictionary<int, Skill>();


		[DataMember]
		public Dictionary<int, Legend> Legends { get; set; }

		[DataMember]
		public Dictionary<int, int> SkillsByPalette { get; set; }

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_icon = null;
				_iconBig = null;
				Skills?.Values.DisposeAll();
				Skills?.Clear();
				Legends?.Values.DisposeAll();
				Legends?.Clear();
				Specializations?.Values.DisposeAll();
				Specializations?.Clear();
				Weapons?.Values.DisposeAll();
				Weapons?.Clear();
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Profession prof, Dictionary<int, Specialization> specializations, Dictionary<int, Trait> traits, Dictionary<int, Skill> skills, Dictionary<int, Legend> legends, Dictionary<Races, Race> races)
		{
			Dictionary<int, Skill> skills2 = skills;
			if (!Enum.TryParse<ProfessionType>(prof.Id, out var professionType))
			{
				return;
			}
			Id = professionType;
			Name = prof.Name;
			IconAssetId = prof.Icon.GetAssetIdFromRenderUrl();
			IconBigAssetId = prof.IconBig.GetAssetIdFromRenderUrl();
			SkillsByPalette = prof.SkillsByPalette.ToDictionary((KeyValuePair<int, int> e) => e.Key, (KeyValuePair<int, int> e) => e.Value);
			foreach (KeyValuePair<string, ProfessionWeapon> apiWeapon in prof.Weapons)
			{
				if (Enum.TryParse<Weapon.WeaponType>(apiWeapon.Key, out var weaponType))
				{
					Weapon weapon;
					bool num = Weapons.TryGetValue(weaponType, out weapon);
					if (weapon == null)
					{
						weapon = new Weapon(apiWeapon, skills2);
					}
					if (professionType == ProfessionType.Guardian && weaponType == Weapon.WeaponType.Sword)
					{
						weapon.Specialization = 65;
						weapon.SpecializationWielded = Weapon.WieldingFlag.Offhand;
					}
					else if (professionType == ProfessionType.Ranger && weaponType == Weapon.WeaponType.Dagger)
					{
						weapon.Specialization = 55;
						weapon.SpecializationWielded = Weapon.WieldingFlag.Mainhand;
					}
					if (!num)
					{
						Weapons.Add(weapon.Type, weapon);
					}
					else
					{
						weapon.ApplyLanguage(skills2);
					}
				}
			}
			List<int> profSkillIds = prof.Skills.Select((ProfessionSkill e) => e.Id).ToList();
			List<int> traitedSkillsIds = new List<int>();
			foreach (Specialization spec2 in specializations.Values.Where((Specialization e) => e.Profession == professionType))
			{
				foreach (List<int> traited2 in from e in spec2.MajorTraits.Values
					where e.Skills.Count > 0
					select e.Skills)
				{
					traitedSkillsIds.AddRange(traited2);
				}
				foreach (List<int> traited in from e in spec2.MinorTraits.Values
					where e.Skills.Count > 0
					select e.Skills)
				{
					traitedSkillsIds.AddRange(traited);
				}
			}
			profSkillIds.AddRange(traitedSkillsIds);
			List<int> tSkillids = (from e in skills2
				where e.Value.Professions.Count <= 2 && e.Value.Professions.Contains(professionType)
				select e.Value.Id).Except(profSkillIds).ToList();
			List<int> raceSkills = (from e in races.SelectMany<KeyValuePair<Races, Race>, KeyValuePair<int, Skill>>((KeyValuePair<Races, Race> e) => e.Value.Skills)
				select e.Value.Id).ToList();
			profSkillIds.AddRange(tSkillids);
			profSkillIds.RemoveAll((int e) => raceSkills.Contains(e));
			List<int> list = prof.Weapons.SelectMany<KeyValuePair<string, ProfessionWeapon>, ProfessionWeaponSkill, int>(delegate(KeyValuePair<string, ProfessionWeapon> pp)
			{
				KeyValuePair<string, ProfessionWeapon> keyValuePair = pp;
				return keyValuePair.Value.Skills;
			}, (KeyValuePair<string, ProfessionWeapon> pp, ProfessionWeaponSkill pskills) => pskills.Id).ToList();
			List<int> ids = new List<int>();
			foreach (int id6 in list)
			{
				if (skills2.TryGetValue(id6, out var _))
				{
					ids.AddRange(getIds(getSkillById(id6, skills2)));
				}
			}
			foreach (int id5 in profSkillIds.Distinct())
			{
				if (skills2.TryGetValue(id5, out var _))
				{
					ids.AddRange(getIds(getSkillById(id5, skills2)));
				}
			}
			foreach (int s in prof.Specializations)
			{
				if (!specializations.TryGetValue(s, out var spec))
				{
					continue;
				}
				if (!Specializations.TryGetValue(spec.Id, out var specialization))
				{
					Specializations.Add(spec.Id, spec);
					continue;
				}
				specialization.Name = spec.Name;
				Skill value;
				foreach (KeyValuePair<int, Trait> t2 in specialization.MajorTraits)
				{
					if (!traits.TryGetValue(t2.Key, out var trait2))
					{
						continue;
					}
					t2.Value.Name = trait2.Name;
					t2.Value.Description = trait2.Description;
					foreach (int id4 in trait2.Skills)
					{
						if (skills2.TryGetValue(id4, out value))
						{
							ids.AddRange(getIds(getSkillById(id4, skills2)));
						}
					}
				}
				foreach (KeyValuePair<int, Trait> t in specialization.MinorTraits)
				{
					if (!traits.TryGetValue(t.Key, out var trait))
					{
						continue;
					}
					t.Value.Name = trait.Name;
					t.Value.Description = trait.Description;
					foreach (int id3 in trait.Skills)
					{
						if (skills2.TryGetValue(id3, out value))
						{
							ids.AddRange(getIds(getSkillById(id3, skills2)));
						}
					}
				}
			}
			foreach (int id2 in ids)
			{
				AddOrUpdateLocale(id2);
			}
			if (professionType != ProfessionType.Revenant)
			{
				return;
			}
			if (Legends == null)
			{
				Dictionary<int, Legend> dictionary2 = (Legends = new Dictionary<int, Legend>());
			}
			foreach (KeyValuePair<int, Legend> leg in legends)
			{
				if (!Legends.TryGetValue(leg.Key, out var legend))
				{
					Legends.Add(leg.Key, leg.Value);
				}
				else
				{
					legend.ApplyLanguage(leg);
				}
			}
			void AddOrUpdateLocale(int id)
			{
				Skill skill2 = getSkillById(id, skills2);
				Skill existingSkill = getSkillById(id, Skills);
				if (skill2 != null)
				{
					if (existingSkill != null)
					{
						existingSkill.Name = skill2.Name;
						existingSkill.Description = skill2.Description;
					}
					else
					{
						Skills.Add(id, skill2);
					}
				}
			}
			List<int> getIds(Skill skill, List<int> result = null)
			{
				if (result == null)
				{
					result = new List<int>();
				}
				if (skill == null)
				{
					return result;
				}
				result.Add(skill.Id);
				if (skill.BundleSkills != null)
				{
					result.AddRange(skill.BundleSkills);
					foreach (int bundleskill in skill.BundleSkills)
					{
						result.AddRange(getIds(getSkillById(bundleskill, skills2)));
					}
				}
				if (skill.FlipSkill.HasValue)
				{
					result.Add(skill.FlipSkill.Value);
					result.AddRange(getIds(getSkillById(skill.FlipSkill.Value, skills2)));
				}
				if (skill.NextChain.HasValue)
				{
					result.Add(skill.NextChain.Value);
				}
				if (skill.PrevChain.HasValue)
				{
					result.Add(skill.PrevChain.Value);
				}
				if (skill.ToolbeltSkill.HasValue)
				{
					result.Add(skill.ToolbeltSkill.Value);
				}
				return result;
			}
			static Skill getSkillById(int id, Dictionary<int, Skill> skills)
			{
				if (!skills.TryGetValue(id, out var skill3))
				{
					return null;
				}
				return skill3;
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Profession prof, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Specialization> apiSpecializations, IEnumerable<Gw2Sharp.WebApi.V2.Models.Legend> apiLegends, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Trait> apiTraits, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Skill> apiSkills)
		{
			Gw2Sharp.WebApi.V2.Models.Profession prof2 = prof;
			if (!Enum.TryParse<ProfessionType>(prof2.Id, out var professionType))
			{
				return;
			}
			Id = professionType;
			Name = prof2.Name;
			IconAssetId = prof2.Icon.GetAssetIdFromRenderUrl();
			IconBigAssetId = prof2.IconBig.GetAssetIdFromRenderUrl();
			SkillsByPalette = prof2.SkillsByPalette.ToDictionary((KeyValuePair<int, int> e) => e.Key, (KeyValuePair<int, int> e) => e.Value);
			foreach (KeyValuePair<string, ProfessionWeapon> apiWeapon in prof2.Weapons)
			{
				if (Enum.TryParse<Weapon.WeaponType>(apiWeapon.Key, out var weaponType2))
				{
					Weapon weapon2;
					bool num = Weapons.TryGetValue(weaponType2, out weapon2);
					if (weapon2 == null)
					{
						weapon2 = new Weapon(apiWeapon);
					}
					if (professionType == ProfessionType.Guardian && weaponType2 == Weapon.WeaponType.Sword)
					{
						weapon2.Specialization = 65;
						weapon2.SpecializationWielded = Weapon.WieldingFlag.Offhand;
					}
					else if (professionType == ProfessionType.Ranger && weaponType2 == Weapon.WeaponType.Dagger)
					{
						weapon2.Specialization = 55;
						weapon2.SpecializationWielded = Weapon.WieldingFlag.Mainhand;
					}
					if (!num)
					{
						Weapons.Add(weapon2.Type, weapon2);
					}
				}
			}
			List<ProfessionWeaponSkill> weaponSkills = prof2.Weapons.SelectMany<KeyValuePair<string, ProfessionWeapon>, ProfessionWeaponSkill>((KeyValuePair<string, ProfessionWeapon> weapon) => weapon.Value.Skills).ToList();
			IEnumerable<Gw2Sharp.WebApi.V2.Models.Skill> skills = apiSkills.Where((Gw2Sharp.WebApi.V2.Models.Skill skill) => prof2.Skills.FirstOrDefault((ProfessionSkill e) => e.Id == skill.Id) != null || weaponSkills.FirstOrDefault((ProfessionWeaponSkill e) => e.Id == skill.Id) != null);
			skills = skills.Concat(apiSkills.Where((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Professions.Count <= 2 && e.Professions.Contains<string>($"{professionType}")).ToList()).Distinct();
			IEnumerable<int> traitIds = apiSpecializations.Where((Gw2Sharp.WebApi.V2.Models.Specialization e) => e.Profession == $"{professionType}").SelectMany((Gw2Sharp.WebApi.V2.Models.Specialization x) => x.MajorTraits);
			if (traitIds.Any())
			{
				List<int> traitSkills = (from x in apiTraits.Where((Gw2Sharp.WebApi.V2.Models.Trait e) => traitIds.Contains(e.Id) && e.Skills != null).SelectMany((Gw2Sharp.WebApi.V2.Models.Trait e) => e.Skills)
					select x.Id).ToList();
				if (traitSkills.Count > 0)
				{
					List<Gw2Sharp.WebApi.V2.Models.Skill> traitedSkills = apiSkills.Where((Gw2Sharp.WebApi.V2.Models.Skill e) => traitSkills.Contains(e.Id)).ToList();
					if (traitedSkills.Count > 0)
					{
						skills = skills.Concat(traitedSkills).Distinct();
					}
				}
			}
			foreach (Gw2Sharp.WebApi.V2.Models.Skill apiSkill in skills)
			{
				Skill skill2;
				bool num2 = Skills.TryGetValue(apiSkill.Id, out skill2);
				if (skill2 == null)
				{
					skill2 = new Skill();
				}
				skill2.Apply(apiSkill);
				skill2.PaletteId = prof2.SkillsByPalette.FirstOrDefault((KeyValuePair<int, int> e) => e.Value == apiSkill.Id).Key;
				ProfessionWeaponSkill weaponSkill = weaponSkills.FirstOrDefault((ProfessionWeaponSkill e) => e.Id == apiSkill.Id);
				if (weaponSkill != null)
				{
					skill2.Offhand = ((weaponSkill.Offhand == null) ? null : (Enum.TryParse<Weapon.WeaponType>(weaponSkill.Offhand, out var weaponType) ? new Weapon.WeaponType?(weaponType) : null));
				}
				if (!num2)
				{
					Skills.Add(skill2.Id, skill2);
				}
			}
			foreach (Gw2Sharp.WebApi.V2.Models.Specialization apiSpecialization in apiSpecializations.Where((Gw2Sharp.WebApi.V2.Models.Specialization e) => e.Profession == $"{professionType}"))
			{
				Specialization specialization;
				bool num3 = Specializations.TryGetValue(apiSpecialization.Id, out specialization);
				if (specialization == null)
				{
					specialization = new Specialization();
				}
				specialization.Apply(apiSpecialization, apiTraits);
				if (!num3)
				{
					Specializations.Add(specialization.Id, specialization);
				}
			}
			if (professionType != ProfessionType.Revenant)
			{
				return;
			}
			if (Legends == null)
			{
				Dictionary<int, Legend> dictionary2 = (Legends = new Dictionary<int, Legend>());
			}
			foreach (Gw2Sharp.WebApi.V2.Models.Legend apiLegend in apiLegends)
			{
				if (int.TryParse(apiLegend.Id.Replace("Legend", ""), out var id))
				{
					Legend legend;
					bool num4 = Legends.TryGetValue(id, out legend);
					if (legend == null)
					{
						legend = new Legend();
					}
					legend.Apply(apiLegend, apiSkills);
					if (!num4)
					{
						Legends.Add(legend.Id, legend);
					}
				}
			}
		}
	}
}
