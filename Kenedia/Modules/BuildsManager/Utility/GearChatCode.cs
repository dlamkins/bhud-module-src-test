using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Utility
{
	public class GearChatCode
	{
		private enum TemplateBytePosition : byte
		{
			MainHandWeaponType,
			MainHandStat,
			MainHandSigil1,
			MainHandPvpSigil,
			MainHandInfusion1,
			OffHandWeaponType,
			OffHandStat,
			OffHandSigil1,
			OffHandPvpSigil,
			OffHandInfusion1,
			AltMainHandWeaponType,
			AltMainHandStat,
			AltMainHandSigil1,
			AltMainHandPvpSigil,
			AltMainHandInfusion1,
			AltOffHandWeaponType,
			AltOffHandStat,
			AltOffHandSigil1,
			AltOffHandPvpSigil,
			AltOffHandInfusion1,
			HeadStat,
			HeadRune,
			HeadInfusion1,
			ShoulderStat,
			ShoulderRune,
			ShoulderInfusion1,
			ChestStat,
			ChestRune,
			ChestInfusion1,
			HandStat,
			HandRune,
			HandInfusion1,
			LegStat,
			LegRune,
			LegInfusion1,
			FootStat,
			FootRune,
			FootInfusion1,
			BackStat,
			BackInfusion1,
			BackInfusion2,
			AmuletStat,
			AmuletEnrichment,
			Accessory1Stat,
			Accessory1Infusion1,
			Accessory2Stat,
			Accessory2Infusion1,
			Ring1Stat,
			Ring1Infusion1,
			Ring1Infusion2,
			Ring1Infusion3,
			Ring2Stat,
			Ring2Infusion1,
			Ring2Infusion2,
			Ring2Infusion3,
			AquaBreatherStat,
			AquaBreatherRune,
			AquaBreatherInfusion1,
			AquaticWeaponType,
			AquaticStat,
			AquaticSigil1,
			AquaticSigil2,
			AquaticInfusion1,
			AquaticInfusion2,
			AltAquaticWeaponType,
			AltAquaticStat,
			AltAquaticSigil1,
			AltAquaticSigil2,
			AltAquaticInfusion1,
			AltAquaticInfusion2,
			PvpAmulet,
			PvpAmuletRune,
			Nourishment,
			Enhancement,
			PowerCore,
			PveRelic,
			PvpRelic
		}

		public static string GetGearChatCode(Template template)
		{
			byte[] codeArray = new byte[77];
			if (template != Template.Empty)
			{
				codeArray[0] = (byte)(template.MainHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown);
				codeArray[1] = template.MainHand.Stat?.MappedId ?? 0;
				codeArray[2] = template.MainHand.Sigil1?.MappedId ?? 0;
				codeArray[3] = template.MainHand.PvpSigil?.MappedId ?? 0;
				codeArray[4] = template.MainHand.Infusion1?.MappedId ?? 0;
				codeArray[5] = (byte)(template.OffHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown);
				codeArray[6] = template.OffHand.Stat?.MappedId ?? 0;
				codeArray[7] = template.OffHand.Sigil1?.MappedId ?? 0;
				codeArray[8] = template.OffHand.PvpSigil?.MappedId ?? 0;
				codeArray[9] = template.OffHand.Infusion1?.MappedId ?? 0;
				codeArray[10] = (byte)(template.AltMainHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown);
				codeArray[11] = template.AltMainHand.Stat?.MappedId ?? 0;
				codeArray[12] = template.AltMainHand.Sigil1?.MappedId ?? 0;
				codeArray[13] = template.AltMainHand.PvpSigil?.MappedId ?? 0;
				codeArray[14] = template.AltMainHand.Infusion1?.MappedId ?? 0;
				codeArray[15] = (byte)(template.AltOffHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown);
				codeArray[16] = template.AltOffHand.Stat?.MappedId ?? 0;
				codeArray[17] = template.AltOffHand.Sigil1?.MappedId ?? 0;
				codeArray[18] = template.AltOffHand.PvpSigil?.MappedId ?? 0;
				codeArray[19] = template.AltOffHand.Infusion1?.MappedId ?? 0;
				codeArray[20] = template.Head.Stat?.MappedId ?? 0;
				codeArray[21] = template.Head.Rune?.MappedId ?? 0;
				codeArray[22] = template.Head.Infusion1?.MappedId ?? 0;
				codeArray[23] = template.Shoulder.Stat?.MappedId ?? 0;
				codeArray[24] = template.Shoulder.Rune?.MappedId ?? 0;
				codeArray[25] = template.Shoulder.Infusion1?.MappedId ?? 0;
				codeArray[26] = template.Chest.Stat?.MappedId ?? 0;
				codeArray[27] = template.Chest.Rune?.MappedId ?? 0;
				codeArray[28] = template.Chest.Infusion1?.MappedId ?? 0;
				codeArray[29] = template.Hand.Stat?.MappedId ?? 0;
				codeArray[30] = template.Hand.Rune?.MappedId ?? 0;
				codeArray[31] = template.Hand.Infusion1?.MappedId ?? 0;
				codeArray[32] = template.Leg.Stat?.MappedId ?? 0;
				codeArray[33] = template.Leg.Rune?.MappedId ?? 0;
				codeArray[34] = template.Leg.Infusion1?.MappedId ?? 0;
				codeArray[35] = template.Foot.Stat?.MappedId ?? 0;
				codeArray[36] = template.Foot.Rune?.MappedId ?? 0;
				codeArray[37] = template.Foot.Infusion1?.MappedId ?? 0;
				codeArray[38] = template.Back.Stat?.MappedId ?? 0;
				codeArray[39] = template.Back.Infusion1?.MappedId ?? 0;
				codeArray[40] = template.Back.Infusion2?.MappedId ?? 0;
				codeArray[41] = template.Amulet.Stat?.MappedId ?? 0;
				codeArray[42] = template.Amulet.Enrichment?.MappedId ?? 0;
				codeArray[43] = template.Accessory_1.Stat?.MappedId ?? 0;
				codeArray[44] = template.Accessory_1.Infusion1?.MappedId ?? 0;
				codeArray[45] = template.Accessory_2.Stat?.MappedId ?? 0;
				codeArray[46] = template.Accessory_2.Infusion1?.MappedId ?? 0;
				codeArray[47] = template.Ring_1.Stat?.MappedId ?? 0;
				codeArray[48] = template.Ring_1.Infusion1?.MappedId ?? 0;
				codeArray[49] = template.Ring_1.Infusion2?.MappedId ?? 0;
				codeArray[50] = template.Ring_1.Infusion3?.MappedId ?? 0;
				codeArray[51] = template.Ring_2.Stat?.MappedId ?? 0;
				codeArray[52] = template.Ring_2.Infusion1?.MappedId ?? 0;
				codeArray[53] = template.Ring_2.Infusion2?.MappedId ?? 0;
				codeArray[54] = template.Ring_2.Infusion3?.MappedId ?? 0;
				codeArray[55] = template.AquaBreather.Stat?.MappedId ?? 0;
				codeArray[56] = template.AquaBreather.Rune?.MappedId ?? 0;
				codeArray[57] = template.AquaBreather.Infusion1?.MappedId ?? 0;
				codeArray[58] = (byte)(template.Aquatic.Weapon?.WeaponType ?? ItemWeaponType.Unknown);
				codeArray[59] = template.Aquatic.Stat?.MappedId ?? 0;
				codeArray[60] = template.Aquatic.Sigil1?.MappedId ?? 0;
				codeArray[61] = template.Aquatic.Sigil2?.MappedId ?? 0;
				codeArray[62] = template.Aquatic.Infusion1?.MappedId ?? 0;
				codeArray[63] = template.Aquatic.Infusion2?.MappedId ?? 0;
				codeArray[64] = (byte)(template.AltAquatic.Weapon?.WeaponType ?? ItemWeaponType.Unknown);
				codeArray[65] = template.AltAquatic.Stat?.MappedId ?? 0;
				codeArray[66] = template.AltAquatic.Sigil1?.MappedId ?? 0;
				codeArray[67] = template.AltAquatic.Sigil2?.MappedId ?? 0;
				codeArray[68] = template.AltAquatic.Infusion1?.MappedId ?? 0;
				codeArray[69] = template.AltAquatic.Infusion2?.MappedId ?? 0;
				codeArray[70] = template.PvpAmulet.PvpAmulet?.MappedId ?? 0;
				codeArray[71] = template.PvpAmulet.Rune?.MappedId ?? 0;
				codeArray[72] = template.Nourishment.Nourishment?.MappedId ?? 0;
				codeArray[73] = template.Enhancement.Enhancement?.MappedId ?? 0;
				codeArray[74] = template.PowerCore.PowerCore?.MappedId ?? 0;
				codeArray[75] = template.PveRelic.Relic?.MappedId ?? 0;
				codeArray[76] = template.PvpRelic.Relic?.MappedId ?? 0;
			}
			return "[&" + Convert.ToBase64String(new System.Span<byte>(codeArray).ToArray()) + "]";
		}

		public static void LoadTemplateFromChatCode(Template template, string? chatCode, Data data)
		{
			if (string.IsNullOrEmpty(chatCode))
			{
				return;
			}
			try
			{
				byte[] array = Convert.FromBase64String(GearTemplateCode.PrepareBase64String(chatCode));
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{GetByte(TemplateBytePosition.MainHandWeaponType)}", out var mainHandWeaponType) ? data.Weapons.Values.Where((Weapon e) => e.WeaponType == mainHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.MainHandStat)).FirstOrDefault().Value);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Sigil1, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.MainHandSigil1)).FirstOrDefault().Value);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.MainHandInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.PvpSigil, data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.MainHandPvpSigil)).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{GetByte(TemplateBytePosition.OffHandWeaponType)}", out var offHandWeaponType) ? data.Weapons.Values.Where((Weapon e) => e.WeaponType == offHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.OffHandStat)).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Sigil1, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.OffHandSigil1)).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.OffHandInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.PvpSigil, data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.OffHandPvpSigil)).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{GetByte(TemplateBytePosition.AltMainHandWeaponType)}", out var altMainHandWeaponType) ? data.Weapons.Values.Where((Weapon e) => e.WeaponType == altMainHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltMainHandStat)).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Sigil1, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltMainHandSigil1)).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltMainHandInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.PvpSigil, data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltMainHandPvpSigil)).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{GetByte(TemplateBytePosition.AltOffHandWeaponType)}", out var altOffHandWeaponType) ? data.Weapons.Values.Where((Weapon e) => e.WeaponType == altOffHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltOffHandStat)).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Sigil1, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltOffHandSigil1)).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltOffHandInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.PvpSigil, data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltOffHandPvpSigil)).FirstOrDefault().Value);
				template.SetItem(template.Head.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.HeadStat)).FirstOrDefault().Value);
				template.SetItem(template.Head.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.HeadRune)).FirstOrDefault().Value);
				template.SetItem(template.Head.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.HeadInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Shoulder.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.ShoulderStat)).FirstOrDefault().Value);
				template.SetItem(template.Shoulder.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.ShoulderRune)).FirstOrDefault().Value);
				template.SetItem(template.Shoulder.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.ShoulderInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Chest.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.ChestStat)).FirstOrDefault().Value);
				template.SetItem(template.Chest.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.ChestRune)).FirstOrDefault().Value);
				template.SetItem(template.Chest.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.ChestInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Hand.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.HandStat)).FirstOrDefault().Value);
				template.SetItem(template.Hand.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.HandRune)).FirstOrDefault().Value);
				template.SetItem(template.Hand.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.HandInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Leg.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.LegStat)).FirstOrDefault().Value);
				template.SetItem(template.Leg.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.LegRune)).FirstOrDefault().Value);
				template.SetItem(template.Leg.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.LegInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Foot.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.FootStat)).FirstOrDefault().Value);
				template.SetItem(template.Foot.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.FootRune)).FirstOrDefault().Value);
				template.SetItem(template.Foot.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.FootInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Back.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.BackStat)).FirstOrDefault().Value);
				template.SetItem(template.Back.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.BackInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Back.Slot, TemplateSubSlotType.Infusion2, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.BackInfusion2)).FirstOrDefault().Value);
				template.SetItem(template.Amulet.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AmuletStat)).FirstOrDefault().Value);
				template.SetItem(template.Amulet.Slot, TemplateSubSlotType.Enrichment, data.Enrichments.Items.Where<KeyValuePair<int, Enrichment>>((KeyValuePair<int, Enrichment> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AmuletEnrichment)).FirstOrDefault().Value);
				template.SetItem(template.Accessory_1.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Accessory1Stat)).FirstOrDefault().Value);
				template.SetItem(template.Accessory_1.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Accessory1Infusion1)).FirstOrDefault().Value);
				template.SetItem(template.Accessory_2.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Accessory2Stat)).FirstOrDefault().Value);
				template.SetItem(template.Accessory_2.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Accessory2Infusion1)).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring1Stat)).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring1Infusion1)).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Infusion2, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring1Infusion2)).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Infusion3, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring1Infusion3)).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring2Stat)).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring2Infusion1)).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Infusion2, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring2Infusion2)).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Infusion3, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Ring2Infusion3)).FirstOrDefault().Value);
				template.SetItem(template.AquaBreather.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaBreatherStat)).FirstOrDefault().Value);
				template.SetItem(template.AquaBreather.Slot, TemplateSubSlotType.Rune, data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaBreatherRune)).FirstOrDefault().Value);
				template.SetItem(template.AquaBreather.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaBreatherInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{GetByte(TemplateBytePosition.AquaticWeaponType)}", out var aquaticWeaponType) ? data.Weapons.Values.Where((Weapon e) => e.WeaponType == aquaticWeaponType).FirstOrDefault() : null);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaticStat)).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Sigil1, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaticSigil1)).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Sigil2, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaticSigil2)).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaticInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Infusion2, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AquaticInfusion2)).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{GetByte(TemplateBytePosition.AltAquaticWeaponType)}", out var altAquaticWeaponType) ? data.Weapons.Values.Where((Weapon e) => e.WeaponType == altAquaticWeaponType).FirstOrDefault() : null);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Stat, data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltAquaticStat)).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Sigil1, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltAquaticSigil1)).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Sigil2, data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltAquaticSigil2)).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Infusion1, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltAquaticInfusion1)).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Infusion2, data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == GetByte(TemplateBytePosition.AltAquaticInfusion2)).FirstOrDefault().Value);
				template.SetItem(template.PvpAmulet.Slot, TemplateSubSlotType.Item, data.PvpAmulets.Items.Where((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet> e) => e.Value.MappedId == GetByte(TemplateBytePosition.PvpAmulet)).FirstOrDefault().Value);
				template.SetItem(template.PvpAmulet.Slot, TemplateSubSlotType.Rune, data.PvpRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == GetByte(TemplateBytePosition.PvpAmuletRune)).FirstOrDefault().Value);
				template.SetItem(template.Nourishment.Slot, TemplateSubSlotType.Item, data.Nourishments.Items.Where<KeyValuePair<int, Nourishment>>((KeyValuePair<int, Nourishment> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Nourishment)).FirstOrDefault().Value);
				template.SetItem(template.Enhancement.Slot, TemplateSubSlotType.Item, data.Enhancements.Items.Where<KeyValuePair<int, Enhancement>>((KeyValuePair<int, Enhancement> e) => e.Value.MappedId == GetByte(TemplateBytePosition.Enhancement)).FirstOrDefault().Value);
				template.SetItem(template.PowerCore.Slot, TemplateSubSlotType.Item, data.PowerCores.Items.Where<KeyValuePair<int, PowerCore>>((KeyValuePair<int, PowerCore> e) => e.Value.MappedId == GetByte(TemplateBytePosition.PowerCore)).FirstOrDefault().Value);
				template.SetItem(template.PveRelic.Slot, TemplateSubSlotType.Item, data.PveRelics.Items.Where<KeyValuePair<int, Relic>>((KeyValuePair<int, Relic> e) => e.Value.MappedId == GetByte(TemplateBytePosition.PveRelic)).FirstOrDefault().Value);
				template.SetItem(template.PvpRelic.Slot, TemplateSubSlotType.Item, data.PvpRelics.Items.Where<KeyValuePair<int, Relic>>((KeyValuePair<int, Relic> e) => e.Value.MappedId == GetByte(TemplateBytePosition.PvpRelic)).FirstOrDefault().Value);
				byte GetByte(TemplateBytePosition position)
				{
					byte templateByte = (byte)position;
					if (templateByte < array.Length)
					{
						return array[templateByte];
					}
					return 0;
				}
			}
			catch (FormatException)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Info((template?.Name ?? "Unkown Template") + " has a invalid chat code format.");
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn(ex, "Error while loading template from chat code of " + (template?.Name ?? "Unkown Template") + ".");
			}
		}
	}
}
