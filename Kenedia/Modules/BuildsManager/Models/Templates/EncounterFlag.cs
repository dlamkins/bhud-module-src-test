using System;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	[Flags]
	public enum EncounterFlag : long
	{
		None = 0x0L,
		NormalMode = 0x2L,
		ChallengeMode = 0x4L,
		ValeGuardian = 0x8L,
		Gorseval = 0x10L,
		Sabetha = 0x20L,
		Slothasor = 0x40L,
		BanditTrio = 0x80L,
		Matthias = 0x100L,
		Escort = 0x200L,
		KeepConstruct = 0x400L,
		Xera = 0x800L,
		Cairn = 0x1000L,
		MursaatOverseer = 0x2000L,
		Samarog = 0x4000L,
		Deimos = 0x8000L,
		SoullessHorror = 0x10000L,
		River = 0x20000L,
		Statues = 0x40000L,
		Dhuum = 0x80000L,
		ConjuredAmalgamate = 0x100000L,
		TwinLargos = 0x200000L,
		Qadim1 = 0x400000L,
		Sabir = 0x800000L,
		Adina = 0x1000000L,
		Qadim2 = 0x2000000L,
		Shiverpeaks = 0x4000000L,
		KodanTwins = 0x8000000L,
		Fraenir = 0x10000000L,
		Boneskinner = 0x20000000L,
		WhisperOfJormag = 0x40000000L,
		ForgingSteel = 0x80000000L,
		ColdWar = 0x100000000L,
		OldLionsCourt = 0x200000000L,
		Aetherblade = 0x400000000L,
		Junkyard = 0x800000000L,
		KainengOverlook = 0x1000000000L,
		HarvestTemple = 0x2000000000L
	}
}
