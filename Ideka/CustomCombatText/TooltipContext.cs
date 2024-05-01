using System;
using System.Collections.Generic;
using HsAPI;

namespace Ideka.CustomCombatText
{
	public class TooltipContext
	{
		public static readonly TooltipContext Default = new TooltipContext();

		public GameMode GameMode { get; set; }

		public int TargetArmor { get; set; } = 2597;


		public int CharacterLevel { get; set; } = 80;


		public ProfessionId? Profession { get; set; }

		public Dictionary<BaseAttribute, float> Stats { get; set; } = new Dictionary<BaseAttribute, float>
		{
			[BaseAttribute.Power] = 1000f,
			[BaseAttribute.Toughness] = 1000f,
			[BaseAttribute.Vitality] = 1000f,
			[BaseAttribute.Precision] = 1000f,
			[BaseAttribute.Ferocity] = 1000f,
			[BaseAttribute.ConditionDamage] = 0f,
			[BaseAttribute.Expertise] = 0f,
			[BaseAttribute.Concentration] = 0f,
			[BaseAttribute.HealingPower] = 0f,
			[BaseAttribute.AgonyResistance] = 0f
		};


		public int BaseHealth => Profession switch
		{
			null => 1000, 
			ProfessionId.Guardian => 1645, 
			ProfessionId.Thief => 1645, 
			ProfessionId.Elementalist => 1645, 
			ProfessionId.Engineer => 5922, 
			ProfessionId.Ranger => 5922, 
			ProfessionId.Mesmer => 5922, 
			ProfessionId.Revenant => 5922, 
			ProfessionId.Necromancer => 9212, 
			ProfessionId.Warrior => 9212, 
			_ => 1000, 
		};

		public int Health => BaseHealth + (int)Math.Round(Stats[BaseAttribute.Vitality] * 10f);
	}
}
