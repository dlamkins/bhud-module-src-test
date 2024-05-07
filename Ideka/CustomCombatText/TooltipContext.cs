using System;
using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using HsAPI;

namespace Ideka.CustomCombatText
{
	public class TooltipContext
	{
		public static readonly TooltipContext Default = new TooltipContext();

		public GameMode GameMode { get; set; }

		public int TargetArmor { get; set; } = 2597;


		public int CharacterLevel { get; set; } = 80;


		public Gender CharacterGender { get; set; }

		public ProfessionId? Profession { get; set; }

		public Dictionary<BaseAttribute, double> Stats { get; set; } = new Dictionary<BaseAttribute, double>
		{
			[BaseAttribute.Power] = 1000.0,
			[BaseAttribute.Toughness] = 1000.0,
			[BaseAttribute.Vitality] = 1000.0,
			[BaseAttribute.Precision] = 1000.0,
			[BaseAttribute.Ferocity] = 1000.0,
			[BaseAttribute.ConditionDamage] = 0.0,
			[BaseAttribute.Expertise] = 0.0,
			[BaseAttribute.Concentration] = 0.0,
			[BaseAttribute.HealingPower] = 0.0,
			[BaseAttribute.AgonyResistance] = 0.0
		};


		public int BaseHealth => Profession switch
		{
			ProfessionId.Guardian => 1645, 
			ProfessionId.Thief => 1645, 
			ProfessionId.Elementalist => 1645, 
			ProfessionId.Engineer => 5922, 
			ProfessionId.Ranger => 5922, 
			ProfessionId.Mesmer => 5922, 
			ProfessionId.Revenant => 5922, 
			ProfessionId.Necromancer => 9212, 
			ProfessionId.Warrior => 9212, 
			_ => 5922, 
		};

		public int Health => BaseHealth + (int)Math.Round(Stats[BaseAttribute.Vitality] * 10.0);
	}
}
