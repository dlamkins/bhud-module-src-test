using System.Collections.Generic;

namespace Torlando.SquadTracker
{
	public static class Specialization
	{
		public static readonly IReadOnlyCollection<int> EliteCodes = (IReadOnlyCollection<int>)(object)new int[27]
		{
			18, 61, 68, 27, 62, 65, 52, 63, 69, 5,
			55, 72, 7, 58, 71, 43, 57, 70, 34, 60,
			64, 48, 56, 67, 40, 59, 66
		};

		public static string GetEliteName(uint elite, uint core)
		{
			return elite switch
			{
				0u => GetCoreName(core), 
				18u => "Berserker", 
				61u => "Spellbreaker", 
				68u => "Bladesworn", 
				27u => "Dragonhunter", 
				62u => "Firebrand", 
				65u => "Willbender", 
				52u => "Herald", 
				63u => "Renegade", 
				69u => "Vindicator", 
				5u => "Druid", 
				55u => "Soulbeast", 
				72u => "Untamed", 
				7u => "Daredevil", 
				58u => "Deadeye", 
				71u => "Specter", 
				43u => "Scrapper", 
				57u => "Holosmith", 
				70u => "Mechanist", 
				34u => "Reaper", 
				60u => "Scourge", 
				64u => "Harbinger", 
				48u => "Tempest", 
				56u => "Weaver", 
				67u => "Catalyst", 
				40u => "Chronomancer", 
				59u => "Mirage", 
				66u => "Virtuoso", 
				_ => "Unknown", 
			};
		}

		public static string GetCoreName(uint core)
		{
			return core switch
			{
				1u => "Guardian", 
				2u => "Warrior", 
				3u => "Engineer", 
				4u => "Ranger", 
				5u => "Thief", 
				6u => "Elementalist", 
				7u => "Mesmer", 
				8u => "Necromancer", 
				9u => "Revenant", 
				_ => "Unknown", 
			};
		}
	}
}
