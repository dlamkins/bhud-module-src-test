using System;
using System.Collections.Generic;
using System.Linq;
using HsAPI;

namespace Ideka.CustomCombatText
{
	public class SkillTooltipData
	{
		public Skill Skill { get; }

		public int? IconId { get; }

		public string Title { get; }

		public string Description { get; }

		public bool DisallowUnderwater { get; }

		public int? Activation { get; }

		public int? Recharge { get; }

		public int InitiativeCost { get; }

		public int EnergyCost { get; }

		public int SupplyCost { get; }

		public int UpkeepCost { get; }

		public float EnduranceCost { get; }

		public List<BlockTooltipData> Blocks { get; }

		public SkillTooltipData(Skill skill, int? iconId = null, TooltipContext? context = null)
		{
			Skill skill2 = skill;
			TooltipContext context2 = context;
			base._002Ector();
			if (context2 == null)
			{
				context2 = TooltipContext.Default;
			}
			IconId = iconId ?? skill2.Icon;
			Title = skill2.Name;
			Palette palette2;
			(Palette, SlotGroup, SkillInfo)[] infos = skill2.Palettes.SelectMany((int paletteId) => (!CTextModule.HsPalettes.TryGetValue(paletteId, out palette2)) ? Array.Empty<(Palette, SlotGroup, SkillInfo)>() : palette2.Groups.SelectMany((SlotGroup group) => from info in @group.Candidates
				where info.Skill == skill2.Id
				select (palette2, @group, info))).ToArray() ?? Array.Empty<(Palette, SlotGroup, SkillInfo)>();
			Skill = (skill2 = TooltipUtils.ResolveOverrides(skill2, context2));
			Activation = skill2.Activation;
			Recharge = skill2.Recharge;
			(Palette, SlotGroup, SkillInfo)[] array = infos;
			int i;
			for (i = 0; i < array.Length; i++)
			{
				SlotGroup group3 = array[i].Item2;
				if (group3.Profession == ProfessionId.Thief)
				{
					InitiativeCost = skill2.ResourceCost.GetValueOrDefault();
					break;
				}
				bool flag;
				switch (group3.Profession)
				{
				case ProfessionId.Warrior:
				case ProfessionId.Revenant:
					flag = true;
					break;
				default:
					flag = false;
					break;
				}
				if (flag)
				{
					EnergyCost = skill2.ResourceCost.GetValueOrDefault();
					break;
				}
			}
			SupplyCost = skill2.SupplyCost.GetValueOrDefault();
			UpkeepCost = skill2.UpkeepCost.GetValueOrDefault();
			EnduranceCost = skill2.EnduranceCost.GetValueOrDefault();
			Description = TooltipUtils.FormatText(skill2.Description) ?? "";
			float weaponStrength = 690.5f;
			array = infos;
			i = 0;
			while (i < array.Length)
			{
				(Palette, SlotGroup, SkillInfo) tuple = array[i];
				Palette palette = tuple.Item1;
				SlotGroup group2 = tuple.Item2;
				SkillInfo info2 = tuple.Item3;
				SkillSlotType slot = group2.Slot;
				bool flag = (((uint)(slot - 5) <= 2u) ? true : false);
				DisallowUnderwater = flag && !info2.Usability.Contains(SkillUsability.UsableUnderWater);
				float num;
				if (palette.Type != SkillPaletteType.Equipment)
				{
					WeaponType? weaponType = palette.WeaponType;
					SkillPaletteType type = palette.Type;
					if (!weaponType.HasValue)
					{
						if (type != SkillPaletteType.Bundle)
						{
							goto IL_043f;
						}
						num = 922.5f;
					}
					else
					{
						switch (weaponType.GetValueOrDefault())
						{
						case WeaponType.BundleLarge:
							break;
						case WeaponType.Focus:
							goto IL_0379;
						case WeaponType.Shield:
							goto IL_0385;
						case WeaponType.Torch:
							goto IL_0391;
						case WeaponType.Warhorn:
							goto IL_039d;
						case WeaponType.Greatsword:
							goto IL_03a9;
						case WeaponType.Hammer:
							goto IL_03b5;
						case WeaponType.Staff:
							goto IL_03c1;
						case WeaponType.BowLong:
							goto IL_03ca;
						case WeaponType.Rifle:
							goto IL_03d3;
						case WeaponType.BowShort:
							goto IL_03dc;
						case WeaponType.Axe:
							goto IL_03e5;
						case WeaponType.Sword:
							goto IL_03ee;
						case WeaponType.Dagger:
							goto IL_03f7;
						case WeaponType.Pistol:
							goto IL_0400;
						case WeaponType.Scepter:
							goto IL_0409;
						case WeaponType.Mace:
							goto IL_0412;
						case WeaponType.Spear:
							goto IL_041b;
						case WeaponType.Speargun:
							goto IL_0424;
						case WeaponType.Trident:
							goto IL_042d;
						case WeaponType.Standard:
							goto IL_0436;
						default:
							goto IL_043f;
						}
						num = 0f;
					}
					goto IL_0447;
				}
				i++;
				continue;
				IL_03dc:
				num = 1000f;
				goto IL_0447;
				IL_03ca:
				num = 1050f;
				goto IL_0447;
				IL_03c1:
				num = 1100f;
				goto IL_0447;
				IL_03d3:
				num = 1150f;
				goto IL_0447;
				IL_03a9:
				num = 1100f;
				goto IL_0447;
				IL_039d:
				num = 900f;
				goto IL_0447;
				IL_03b5:
				num = 1100f;
				goto IL_0447;
				IL_0391:
				num = 900f;
				goto IL_0447;
				IL_0379:
				num = 900f;
				goto IL_0447;
				IL_0447:
				weaponStrength = num;
				break;
				IL_0385:
				num = 900f;
				goto IL_0447;
				IL_043f:
				num = weaponStrength;
				goto IL_0447;
				IL_0436:
				num = 690.5f;
				goto IL_0447;
				IL_042d:
				num = 1000f;
				goto IL_0447;
				IL_0424:
				num = 1000f;
				goto IL_0447;
				IL_041b:
				num = 1000f;
				goto IL_0447;
				IL_0412:
				num = 1000f;
				goto IL_0447;
				IL_0409:
				num = 1000f;
				goto IL_0447;
				IL_0400:
				num = 1000f;
				goto IL_0447;
				IL_03f7:
				num = 1000f;
				goto IL_0447;
				IL_03ee:
				num = 1000f;
				goto IL_0447;
				IL_03e5:
				num = 1000f;
				goto IL_0447;
			}
			Blocks = (from x in skill2.Blocks
				where !x.TraitRequirements.Any()
				select new BlockTooltipData(x, weaponStrength, context2)).ToList();
		}
	}
}
