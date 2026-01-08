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
			(Palette, SlotGroup, SkillInfo)[] infos = skill2.Palettes.SelectMany((int paletteId) => (!CTextModule.HsPaletteData.Items.TryGetValue(paletteId, out palette2)) ? Array.Empty<(Palette, SlotGroup, SkillInfo)>() : palette2.Groups.SelectMany((SlotGroup group) => from info in @group.Candidates
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
				if (group3.Profession.GetValueOrDefault() == ProfessionId.Thief)
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
							goto IL_04df;
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
							goto IL_03bd;
						case WeaponType.Shield:
							goto IL_03cd;
						case WeaponType.Torch:
							goto IL_03dd;
						case WeaponType.Warhorn:
							goto IL_03ed;
						case WeaponType.Greatsword:
							goto IL_03fd;
						case WeaponType.Hammer:
							goto IL_040d;
						case WeaponType.Staff:
							goto IL_041d;
						case WeaponType.Longbow:
							goto IL_042d;
						case WeaponType.Rifle:
							goto IL_043d;
						case WeaponType.Shortbow:
							goto IL_044d;
						case WeaponType.Axe:
							goto IL_045d;
						case WeaponType.Sword:
							goto IL_046a;
						case WeaponType.Dagger:
							goto IL_0477;
						case WeaponType.Pistol:
							goto IL_0484;
						case WeaponType.Scepter:
							goto IL_0491;
						case WeaponType.Mace:
							goto IL_049e;
						case WeaponType.Spear:
							goto IL_04ab;
						case WeaponType.Speargun:
							goto IL_04b8;
						case WeaponType.Trident:
							goto IL_04c5;
						case WeaponType.Standard:
							goto IL_04d2;
						default:
							goto IL_04df;
						}
						num = 0.0;
					}
					goto IL_04e7;
				}
				i++;
				continue;
				IL_044d:
				num = 1000.0;
				goto IL_04e7;
				IL_042d:
				num = 1050.0;
				goto IL_04e7;
				IL_041d:
				num = 1100.0;
				goto IL_04e7;
				IL_043d:
				num = 1150.0;
				goto IL_04e7;
				IL_03fd:
				num = 1100.0;
				goto IL_04e7;
				IL_03ed:
				num = 900.0;
				goto IL_04e7;
				IL_040d:
				num = 1100.0;
				goto IL_04e7;
				IL_03dd:
				num = 900.0;
				goto IL_04e7;
				IL_03bd:
				num = 900.0;
				goto IL_04e7;
				IL_04e7:
				weaponStrength = num;
				break;
				IL_03cd:
				num = 900.0;
				goto IL_04e7;
				IL_04df:
				num = weaponStrength;
				goto IL_04e7;
				IL_04d2:
				num = 690.5;
				goto IL_04e7;
				IL_04c5:
				num = 1000.0;
				goto IL_04e7;
				IL_04b8:
				num = 1000.0;
				goto IL_04e7;
				IL_04ab:
				num = 1000.0;
				goto IL_04e7;
				IL_049e:
				num = 1000.0;
				goto IL_04e7;
				IL_0491:
				num = 1000.0;
				goto IL_04e7;
				IL_0484:
				num = 1000.0;
				goto IL_04e7;
				IL_0477:
				num = 1000.0;
				goto IL_04e7;
				IL_046a:
				num = 1000.0;
				goto IL_04e7;
				IL_045d:
				num = 1000.0;
				goto IL_04e7;
			}
			Blocks = (from x in skill2.Blocks
				where !x.TraitRequirements.Any()
				select new BlockTooltipData(x, weaponStrength, context2)).ToList();
		}
	}
}
