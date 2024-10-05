using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Skill : IDisposable, IBaseApiData, IDataMember
	{
		private bool _isDisposed;

		private AsyncTexture2D _icon;

		[DataMember]
		public LocalizedString Names { get; protected set; } = new LocalizedString();


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
		public int Id { get; set; }

		[DataMember]
		public int? Parent { get; set; }

		[DataMember]
		public int Specialization { get; set; }

		[DataMember]
		public List<ProfessionType> Professions { get; set; } = new List<ProfessionType>();


		[DataMember]
		public int PaletteId { get; set; }

		[DataMember]
		public int IconAssetId { get; set; }

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

		[DataMember]
		public string ChatLink { get; set; }

		[DataMember]
		public LocalizedString Descriptions { get; protected set; } = new LocalizedString();


		public string Description
		{
			get
			{
				return Descriptions.Text;
			}
			set
			{
				Descriptions.Text = value;
			}
		}

		[DataMember]
		public SkillSlot? Slot { get; set; }

		[DataMember]
		public Weapon.WeaponType? WeaponType { get; set; }

		[DataMember]
		public Weapon.WeaponType? Offhand { get; set; }

		[DataMember]
		public SkillFlag Flags { get; set; }

		[DataMember]
		public SkillCategoryType Categories { get; set; }

		[DataMember]
		public int? FlipSkill { get; set; }

		[DataMember]
		public int? NextChain { get; set; }

		[DataMember]
		public int? PrevChain { get; set; }

		[DataMember]
		public int? ToolbeltSkill { get; set; }

		[DataMember]
		public List<int> BundleSkills { get; set; }

		public List<Gw2Sharp.WebApi.V2.Models.SkillFact> Facts { get; set; }

		public List<Gw2Sharp.WebApi.V2.Models.SkillFact> TraitedFacts { get; set; }

		public Skill()
		{
		}

		public Skill(Gw2Sharp.WebApi.V2.Models.Skill skill)
		{
			Apply(skill);
		}

		public Skill(BaseSkill baseSkill)
		{
			Names = baseSkill.Names;
			Descriptions = baseSkill.Descriptions;
			Id = baseSkill.Id;
			IconAssetId = baseSkill.AssetId.GetValueOrDefault();
			Slot = baseSkill.Slot;
			foreach (string profession in baseSkill.Professions)
			{
				if (Enum.TryParse<ProfessionType>(profession, out var profType))
				{
					Professions.Add(profType);
				}
			}
		}

		public void SetLiveAPI(Gw2Sharp.WebApi.V2.Models.Skill skill)
		{
			Facts = skill.Facts?.ToList();
			TraitedFacts = skill.TraitedFacts?.ToList();
		}

		internal static Skill FromUShort(ushort id, ProfessionType profession)
		{
			foreach (KeyValuePair<Races, Race> item in BuildsManager.Data.Races.Items)
			{
				KeyValuePair<int, Skill> skill = item.Value.Skills.Where<KeyValuePair<int, Skill>>((KeyValuePair<int, Skill> e) => e.Value.PaletteId == id).FirstOrDefault();
				if (skill.Value != null)
				{
					return skill.Value;
				}
			}
			int skillid = 0;
			ProfessionDataEntry professions = BuildsManager.Data.Professions;
			if (professions == null || professions[profession]?.SkillsByPalette.TryGetValue(id, out skillid) != true)
			{
				return null;
			}
			return BuildsManager.Data.Professions?[profession]?.Skills[skillid];
		}

		public static int GetRevPaletteId(int id)
		{
			foreach (KeyValuePair<int, List<int>> pair in new Dictionary<int, List<int>>
			{
				{
					4572,
					new List<int> { 26937, 27220, 27372, 28219, 28427, 29148, 45686, 62719, 62749 }
				},
				{
					4614,
					new List<int> { 26821, 27322, 28379, 28516, 29209, 29310, 42949, 62832, 62702 }
				},
				{
					4651,
					new List<int> { 26679, 27014, 27025, 27505, 28231, 29082, 40485, 62962, 62941 }
				},
				{
					4564,
					new List<int> { 26557, 26644, 27107, 27715, 27917, 29197, 41220, 62878, 62796 }
				},
				{
					4554,
					new List<int> { 27356, 27760, 27975, 28287, 28406, 29114, 45773, 62942, 62687 }
				}
			})
			{
				if (pair.Value.Contains(id))
				{
					return pair.Key;
				}
			}
			return 0;
		}

		public static int GetRevPaletteId(Skill skill)
		{
			return GetRevPaletteId(skill.Id);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_icon = null;
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Skill skill)
		{
			Gw2Sharp.WebApi.V2.Models.Skill skill2 = skill;
			Id = skill2.Id;
			IconAssetId = skill2.Icon.GetAssetIdFromRenderUrl();
			Name = skill2.Name;
			Description = skill2.Description;
			Specialization = (skill2.Specialization.HasValue ? skill2.Specialization.Value : 0);
			ChatLink = skill2.ChatLink;
			Flags = ((skill2.Flags.Count() > 0) ? ((SkillFlag)skill2.Flags.Aggregate((ApiEnum<SkillFlag> x, ApiEnum<SkillFlag> y) => x = (ApiEnum<SkillFlag>)((SkillFlag)x | y.ToEnum()))) : SkillFlag.Unknown);
			Slot = skill2.Slot?.ToEnum();
			WeaponType = (((object)skill2.WeaponType != null) ? new Weapon.WeaponType?((Weapon.WeaponType)(skill2.WeaponType?.ToEnum()).Value) : null);
			Dictionary<SkillCategoryType, List<int>> missinCategories = new Dictionary<SkillCategoryType, List<int>> { 
			{
				SkillCategoryType.Preparation,
				new List<int> { 13057, 13026, 13038, 13056 }
			} };
			if (Categories == SkillCategoryType.None)
			{
				foreach (List<int> value in missinCategories.Values)
				{
					if (value.Contains(skill2.Id))
					{
						Categories |= missinCategories.FirstOrDefault((KeyValuePair<SkillCategoryType, List<int>> x) => x.Value.Contains(skill2.Id)).Key;
					}
				}
				if ((skill2.Specialization ?? 0) != 0)
				{
					if (!Categories.HasFlag(SkillCategoryType.Specialization))
					{
						Categories |= SkillCategoryType.Specialization;
					}
				}
				else if (skill2.Professions.Count == 1 && skill2.Professions.Contains<string>("Engineer") && skill2.BundleSkills != null)
				{
					if (!Categories.HasFlag(SkillCategoryType.Kit))
					{
						Categories |= SkillCategoryType.Kit;
					}
				}
				else if ((skill2.Categories != null && skill2.Categories!.Count > 0) || skill2.Name.Contains('"'))
				{
					if (skill2.Name.Contains('"') && !Categories.HasFlag(SkillCategoryType.Shout))
					{
						Categories |= SkillCategoryType.Shout;
					}
					if (skill2.Categories != null)
					{
						foreach (string item in skill2.Categories!)
						{
							if (Enum.TryParse<SkillCategoryType>(item, out var category) && !Categories.HasFlag(category))
							{
								Categories |= category;
							}
						}
					}
				}
			}
			BundleSkills = ((skill2.BundleSkills != null && skill2.BundleSkills!.Count > 0) ? skill2.BundleSkills.ToList() : null);
			FlipSkill = (skill2.FlipSkill.HasValue ? skill2.FlipSkill : null);
			ToolbeltSkill = (skill2.ToolbeltSkill.HasValue ? skill2.ToolbeltSkill : null);
			PrevChain = (skill2.PrevChain.HasValue ? skill2.PrevChain : null);
			NextChain = (skill2.NextChain.HasValue ? skill2.NextChain : null);
			foreach (string profession2 in skill2.Professions)
			{
				if (Enum.TryParse<ProfessionType>(profession2, out var profession))
				{
					Professions.Add(profession);
				}
			}
			Facts = skill2.Facts?.ToList();
			TraitedFacts = skill2.TraitedFacts?.ToList();
		}

		internal static int GetRevPaletteId(Gw2Sharp.WebApi.V2.Models.Skill skill)
		{
			return GetRevPaletteId(skill.Id);
		}
	}
}
