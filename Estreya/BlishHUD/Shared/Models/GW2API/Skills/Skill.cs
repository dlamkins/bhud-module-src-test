using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Estreya.BlishHUD.Shared.State;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.Shared.Models.GW2API.Skills
{
	public class Skill : IDisposable
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public SkillCategory Category { get; set; }

		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public string Icon { get; set; }

		public int Specialization { get; set; }

		public string ChatLink { get; set; } = string.Empty;


		[JsonConverter(typeof(StringEnumConverter))]
		public SkillType Type { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public SkillWeaponType WeaponType { get; set; }

		public List<string> Professions { get; set; } = new List<string>();


		[JsonConverter(typeof(StringEnumConverter))]
		public SkillSlot Slot { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public Attunement DualAttunement { get; set; }

		public List<SkillFlag> Flags { get; set; }

		public List<string>? Categories { get; set; }

		[JsonProperty("subskills")]
		public List<SkillSubSkill> SubSkills { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public Attunement Attunement { get; set; }

		public int Cost { get; set; }

		public string DualWield { get; set; }

		public int FlipSkill { get; set; }

		public int Initiative { get; set; }

		public int NextChain { get; set; }

		public int PrevChain { get; set; }

		public List<int> TransformSkills { get; set; }

		public List<int> BundleSkills { get; set; }

		public int ToolbeltSkill { get; set; }

		[JsonIgnore]
		public AsyncTexture2D IconTexture { get; set; }

		public static Skill FromAPISkill(Skill skill)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			Skill obj = new Skill
			{
				Category = SkillCategory.Skill,
				Id = skill.get_Id(),
				Name = skill.get_Name(),
				Description = skill.get_Description()
			};
			RenderUrl? icon = skill.get_Icon();
			object icon2;
			if (!icon.HasValue)
			{
				icon2 = null;
			}
			else
			{
				RenderUrl valueOrDefault = icon.GetValueOrDefault();
				icon2 = ((RenderUrl)(ref valueOrDefault)).get_Url()?.AbsoluteUri;
			}
			obj.Icon = (string)icon2;
			obj.Specialization = skill.get_Specialization().GetValueOrDefault();
			obj.ChatLink = skill.get_ChatLink();
			obj.Type = (SkillType)((!(skill.get_Type()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<SkillType>.op_Implicit(skill.get_Type())) : 0);
			obj.WeaponType = (SkillWeaponType)((!(skill.get_WeaponType()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<SkillWeaponType>.op_Implicit(skill.get_WeaponType())) : 0);
			obj.Professions = skill.get_Professions().ToList();
			obj.Slot = (SkillSlot)((!(skill.get_Slot()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<SkillSlot>.op_Implicit(skill.get_Slot())) : 0);
			obj.DualAttunement = (Attunement)((!(skill.get_DualAttunement()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<Attunement>.op_Implicit(skill.get_DualAttunement())) : 0);
			obj.Flags = (from flag in skill.get_Flags()?.get_List()
				select flag.get_Value()).ToList();
			obj.Categories = skill.get_Categories()?.ToList();
			obj.SubSkills = skill.get_SubSkills()?.ToList();
			obj.Attunement = (Attunement)((!(skill.get_Attunement()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<Attunement>.op_Implicit(skill.get_Attunement())) : 0);
			obj.Cost = skill.get_Cost().GetValueOrDefault();
			obj.DualWield = skill.get_DualWield();
			obj.FlipSkill = skill.get_FlipSkill().GetValueOrDefault();
			obj.Initiative = skill.get_Initiative().GetValueOrDefault();
			obj.NextChain = skill.get_NextChain().GetValueOrDefault();
			obj.PrevChain = skill.get_PrevChain().GetValueOrDefault();
			obj.TransformSkills = skill.get_TransformSkills()?.ToList();
			obj.BundleSkills = skill.get_BundleSkills()?.ToList();
			obj.ToolbeltSkill = skill.get_ToolbeltSkill().GetValueOrDefault();
			return obj;
		}

		public static Skill FromAPITrait(Trait trait)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			return new Skill
			{
				Category = SkillCategory.Trait,
				Id = trait.get_Id(),
				Name = trait.get_Name(),
				Description = trait.get_Description(),
				Icon = RenderUrl.op_Implicit(trait.get_Icon()),
				Specialization = trait.get_Specialization()
			};
		}

		public static Skill FromAPITraitSkill(TraitSkill skill)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			return new Skill
			{
				Category = SkillCategory.TraitSkill,
				Id = skill.get_Id(),
				Name = skill.get_Name(),
				Description = skill.get_Description(),
				Icon = RenderUrl.op_Implicit(skill.get_Icon()),
				ChatLink = skill.get_ChatLink(),
				Flags = (from flag in skill.get_Flags()?.get_List()
					select flag.get_Value()).ToList(),
				Categories = skill.get_Categories()?.ToList()
			};
		}

		public static Skill FromAPIUpgradeComponent(ItemUpgradeComponent upgradeComponent)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			ItemUpgradeComponentDetails details = upgradeComponent.get_Details();
			if (((details != null) ? details.get_InfixUpgrade() : null) == null)
			{
				return null;
			}
			Skill obj = new Skill
			{
				Category = SkillCategory.UpgradeComponent
			};
			ItemUpgradeComponentDetails details2 = upgradeComponent.get_Details();
			obj.Id = ((details2 != null) ? details2.get_InfixUpgrade().get_Buff().get_SkillId() : 0);
			obj.Name = ((Item)upgradeComponent).get_Name();
			ItemUpgradeComponentDetails details3 = upgradeComponent.get_Details();
			obj.Description = ((details3 != null) ? details3.get_InfixUpgrade().get_Buff().get_Description() : null);
			obj.Icon = RenderUrl.op_Implicit(((Item)upgradeComponent).get_Icon());
			obj.ChatLink = ((Item)upgradeComponent).get_ChatLink();
			return obj;
		}

		public void Dispose()
		{
			IconTexture = null;
		}

		public void LoadTexture(IconState iconState)
		{
			if (!string.IsNullOrWhiteSpace(Icon))
			{
				IconTexture = iconState.GetIcon(Icon);
			}
		}
	}
}
