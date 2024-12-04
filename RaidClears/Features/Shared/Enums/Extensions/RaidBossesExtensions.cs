using System;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Enums.Extensions
{
	public static class RaidBossesExtensions
	{
		public static string GetLabel(this Encounters.RaidBosses value)
		{
			return value switch
			{
				Encounters.RaidBosses.ValeGuardian => Strings.Raid_Wing_1_1_Name, 
				Encounters.RaidBosses.SpiritWoods => Strings.Raid_Wing_1_2_Name, 
				Encounters.RaidBosses.Gorseval => Strings.Raid_Wing_1_3_Name, 
				Encounters.RaidBosses.Sabetha => Strings.Raid_Wing_1_4_Name, 
				Encounters.RaidBosses.Slothasor => Strings.Raid_Wing_2_1_Name, 
				Encounters.RaidBosses.BanditTrio => Strings.Raid_Wing_2_2_Name, 
				Encounters.RaidBosses.Matthias => Strings.Raid_Wing_2_3_Name, 
				Encounters.RaidBosses.Escort => Strings.Raid_Wing_3_1_Name, 
				Encounters.RaidBosses.KeepConstruct => Strings.Raid_Wing_3_2_Name, 
				Encounters.RaidBosses.TwistedCastle => Strings.Raid_Wing_3_3_Name, 
				Encounters.RaidBosses.Xera => Strings.Raid_Wing_3_4_Name, 
				Encounters.RaidBosses.Cairn => Strings.Raid_Wing_4_1_Name, 
				Encounters.RaidBosses.MursaatOverseer => Strings.Raid_Wing_4_2_Name, 
				Encounters.RaidBosses.Samarog => Strings.Raid_Wing_4_3_Name, 
				Encounters.RaidBosses.Deimos => Strings.Raid_Wing_4_4_Name, 
				Encounters.RaidBosses.SoulessHorror => Strings.Raid_Wing_5_1_Name, 
				Encounters.RaidBosses.RiverOfSouls => Strings.Raid_Wing_5_2_Name, 
				Encounters.RaidBosses.StatuesOfGrenth => Strings.Raid_Wing_5_3_Name, 
				Encounters.RaidBosses.VoiceInTheVoid => Strings.Raid_Wing_5_4_Name, 
				Encounters.RaidBosses.ConjuredAmalgamate => Strings.Raid_Wing_6_1_Name, 
				Encounters.RaidBosses.TwinLargos => Strings.Raid_Wing_6_2_Name, 
				Encounters.RaidBosses.Qadim => Strings.Raid_Wing_6_3_Name, 
				Encounters.RaidBosses.Gate => Strings.Raid_Wing_7_1_Name, 
				Encounters.RaidBosses.Adina => Strings.Raid_Wing_7_2_Name, 
				Encounters.RaidBosses.Sabir => Strings.Raid_Wing_7_3_Name, 
				Encounters.RaidBosses.QadimThePeerless => Strings.Raid_Wing_7_4_Name, 
				Encounters.RaidBosses.Camp => Strings.Raid_Wing_8_1_Name, 
				Encounters.RaidBosses.Decima => Strings.Raid_Wing_8_2_Name, 
				Encounters.RaidBosses.Greer => Strings.Raid_Wing_8_3_Name, 
				Encounters.RaidBosses.Ura => Strings.Raid_Wing_8_4_Name, 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}

		public static string GetLabelShort(this Encounters.RaidBosses value)
		{
			return value switch
			{
				Encounters.RaidBosses.ValeGuardian => Strings.Raid_Wing_1_1_Short, 
				Encounters.RaidBosses.SpiritWoods => Strings.Raid_Wing_1_2_Short, 
				Encounters.RaidBosses.Gorseval => Strings.Raid_Wing_1_3_Short, 
				Encounters.RaidBosses.Sabetha => Strings.Raid_Wing_1_4_Short, 
				Encounters.RaidBosses.Slothasor => Strings.Raid_Wing_2_1_Short, 
				Encounters.RaidBosses.BanditTrio => Strings.Raid_Wing_2_2_Short, 
				Encounters.RaidBosses.Matthias => Strings.Raid_Wing_2_3_Short, 
				Encounters.RaidBosses.Escort => Strings.Raid_Wing_3_1_Short, 
				Encounters.RaidBosses.KeepConstruct => Strings.Raid_Wing_3_2_Short, 
				Encounters.RaidBosses.TwistedCastle => Strings.Raid_Wing_3_3_Short, 
				Encounters.RaidBosses.Xera => Strings.Raid_Wing_3_4_Short, 
				Encounters.RaidBosses.Cairn => Strings.Raid_Wing_4_1_Short, 
				Encounters.RaidBosses.MursaatOverseer => Strings.Raid_Wing_4_2_Short, 
				Encounters.RaidBosses.Samarog => Strings.Raid_Wing_4_3_Short, 
				Encounters.RaidBosses.Deimos => Strings.Raid_Wing_4_4_Short, 
				Encounters.RaidBosses.SoulessHorror => Strings.Raid_Wing_5_1_Short, 
				Encounters.RaidBosses.RiverOfSouls => Strings.Raid_Wing_5_2_Short, 
				Encounters.RaidBosses.StatuesOfGrenth => Strings.Raid_Wing_5_3_Short, 
				Encounters.RaidBosses.VoiceInTheVoid => Strings.Raid_Wing_5_4_Short, 
				Encounters.RaidBosses.ConjuredAmalgamate => Strings.Raid_Wing_6_1_Short, 
				Encounters.RaidBosses.TwinLargos => Strings.Raid_Wing_6_2_Short, 
				Encounters.RaidBosses.Qadim => Strings.Raid_Wing_6_3_Short, 
				Encounters.RaidBosses.Gate => Strings.Raid_Wing_7_1_Short, 
				Encounters.RaidBosses.Adina => Strings.Raid_Wing_7_2_Short, 
				Encounters.RaidBosses.Sabir => Strings.Raid_Wing_7_3_Short, 
				Encounters.RaidBosses.QadimThePeerless => Strings.Raid_Wing_7_4_Short, 
				Encounters.RaidBosses.Camp => Strings.Raid_Wing_8_1_Short, 
				Encounters.RaidBosses.Decima => Strings.Raid_Wing_8_2_Short, 
				Encounters.RaidBosses.Greer => Strings.Raid_Wing_8_3_Short, 
				Encounters.RaidBosses.Ura => Strings.Raid_Wing_8_4_Short, 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}

		public static string GetApiLabel(this Encounters.RaidBosses value)
		{
			return value switch
			{
				Encounters.RaidBosses.ValeGuardian => "vale_guardian", 
				Encounters.RaidBosses.SpiritWoods => "spirit_woods", 
				Encounters.RaidBosses.Gorseval => "gorseval", 
				Encounters.RaidBosses.Sabetha => "sabetha", 
				Encounters.RaidBosses.Slothasor => "slothasor", 
				Encounters.RaidBosses.BanditTrio => "bandit_trio", 
				Encounters.RaidBosses.Matthias => "matthias", 
				Encounters.RaidBosses.Escort => "escort", 
				Encounters.RaidBosses.KeepConstruct => "keep_construct", 
				Encounters.RaidBosses.TwistedCastle => "twisted_castle", 
				Encounters.RaidBosses.Xera => "xera", 
				Encounters.RaidBosses.Cairn => "cairn", 
				Encounters.RaidBosses.MursaatOverseer => "mursaat_overseer", 
				Encounters.RaidBosses.Samarog => "samarog", 
				Encounters.RaidBosses.Deimos => "deimos", 
				Encounters.RaidBosses.SoulessHorror => "soulless_horror", 
				Encounters.RaidBosses.RiverOfSouls => "river_of_souls", 
				Encounters.RaidBosses.StatuesOfGrenth => "statues_of_grenth", 
				Encounters.RaidBosses.VoiceInTheVoid => "voice_in_the_void", 
				Encounters.RaidBosses.ConjuredAmalgamate => "conjured_amalgamate", 
				Encounters.RaidBosses.TwinLargos => "twin_largos", 
				Encounters.RaidBosses.Qadim => "qadim", 
				Encounters.RaidBosses.Gate => "gate", 
				Encounters.RaidBosses.Adina => "adina", 
				Encounters.RaidBosses.Sabir => "sabir", 
				Encounters.RaidBosses.QadimThePeerless => "qadim_the_peerless", 
				Encounters.RaidBosses.Camp => "camp", 
				Encounters.RaidBosses.Decima => "decima", 
				Encounters.RaidBosses.Greer => "greer", 
				Encounters.RaidBosses.Ura => "ura", 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}
	}
}
