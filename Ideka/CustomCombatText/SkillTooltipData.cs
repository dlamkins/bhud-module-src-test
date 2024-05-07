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
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			Skill skill2 = skill;
			TooltipContext context2 = context;
			base._002Ector();
			if (context2 == null)
			{
				context2 = TooltipContext.Default;
			}
			IconId = iconId ?? skill2.Icon;
			Title = TooltipUtils.ResolveInflections(skill2.Name, context2.CharacterGender);
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
				TooltipContext tooltipContext = context2;
				if (!tooltipContext.Profession.HasValue)
				{
					ProfessionId? professionId = (tooltipContext.Profession = group3.Profession);
				}
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
			double weaponStrength = 690.5;
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
				double num;
				if (palette.Type != SkillPaletteType.Equipment)
				{
					WeaponType? weaponType = palette.WeaponType;
					SkillPaletteType type = palette.Type;
					if (!weaponType.HasValue)
					{
						if (type != SkillPaletteType.Bundle)
						{
							goto IL_04ed;
						}
						num = 922.5;
					}
					else
					{
						switch (weaponType.GetValueOrDefault())
						{
						case WeaponType.BundleLarge:
							break;
						case WeaponType.Focus:
							goto IL_03cb;
						case WeaponType.Shield:
							goto IL_03db;
						case WeaponType.Torch:
							goto IL_03eb;
						case WeaponType.Warhorn:
							goto IL_03fb;
						case WeaponType.Greatsword:
							goto IL_040b;
						case WeaponType.Hammer:
							goto IL_041b;
						case WeaponType.Staff:
							goto IL_042b;
						case WeaponType.BowLong:
							goto IL_043b;
						case WeaponType.Rifle:
							goto IL_044b;
						case WeaponType.BowShort:
							goto IL_045b;
						case WeaponType.Axe:
							goto IL_046b;
						case WeaponType.Sword:
							goto IL_0478;
						case WeaponType.Dagger:
							goto IL_0485;
						case WeaponType.Pistol:
							goto IL_0492;
						case WeaponType.Scepter:
							goto IL_049f;
						case WeaponType.Mace:
							goto IL_04ac;
						case WeaponType.Spear:
							goto IL_04b9;
						case WeaponType.Speargun:
							goto IL_04c6;
						case WeaponType.Trident:
							goto IL_04d3;
						case WeaponType.Standard:
							goto IL_04e0;
						default:
							goto IL_04ed;
						}
						num = 0.0;
					}
					goto IL_04f5;
				}
				i++;
				continue;
				IL_045b:
				num = 1000.0;
				goto IL_04f5;
				IL_043b:
				num = 1050.0;
				goto IL_04f5;
				IL_042b:
				num = 1100.0;
				goto IL_04f5;
				IL_044b:
				num = 1150.0;
				goto IL_04f5;
				IL_040b:
				num = 1100.0;
				goto IL_04f5;
				IL_03fb:
				num = 900.0;
				goto IL_04f5;
				IL_041b:
				num = 1100.0;
				goto IL_04f5;
				IL_03eb:
				num = 900.0;
				goto IL_04f5;
				IL_03cb:
				num = 900.0;
				goto IL_04f5;
				IL_04f5:
				weaponStrength = num;
				break;
				IL_03db:
				num = 900.0;
				goto IL_04f5;
				IL_04ed:
				num = weaponStrength;
				goto IL_04f5;
				IL_04e0:
				num = 690.5;
				goto IL_04f5;
				IL_04d3:
				num = 1000.0;
				goto IL_04f5;
				IL_04c6:
				num = 1000.0;
				goto IL_04f5;
				IL_04b9:
				num = 1000.0;
				goto IL_04f5;
				IL_04ac:
				num = 1000.0;
				goto IL_04f5;
				IL_049f:
				num = 1000.0;
				goto IL_04f5;
				IL_0492:
				num = 1000.0;
				goto IL_04f5;
				IL_0485:
				num = 1000.0;
				goto IL_04f5;
				IL_0478:
				num = 1000.0;
				goto IL_04f5;
				IL_046b:
				num = 1000.0;
				goto IL_04f5;
			}
			Blocks = (from x in skill2.Blocks
				where !x.TraitRequirements.Any()
				select new BlockTooltipData(x, weaponStrength, context2)).ToList();
		}
	}
}
