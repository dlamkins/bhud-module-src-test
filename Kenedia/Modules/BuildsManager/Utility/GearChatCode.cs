using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;

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
			return "[&" + Convert.ToBase64String(new System.Span<byte>(new byte[77]
			{
				(byte)(template.MainHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown),
				template.MainHand.Stat?.MappedId ?? 0,
				template.MainHand.Sigil1?.MappedId ?? 0,
				template.MainHand.PvpSigil?.MappedId ?? 0,
				template.MainHand.Infusion1?.MappedId ?? 0,
				(byte)(template.OffHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown),
				template.OffHand.Stat?.MappedId ?? 0,
				template.OffHand.Sigil1?.MappedId ?? 0,
				template.OffHand.PvpSigil?.MappedId ?? 0,
				template.OffHand.Infusion1?.MappedId ?? 0,
				(byte)(template.AltMainHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown),
				template.AltMainHand.Stat?.MappedId ?? 0,
				template.AltMainHand.Sigil1?.MappedId ?? 0,
				template.AltMainHand.PvpSigil?.MappedId ?? 0,
				template.AltMainHand.Infusion1?.MappedId ?? 0,
				(byte)(template.AltOffHand.Weapon?.WeaponType ?? ItemWeaponType.Unknown),
				template.AltOffHand.Stat?.MappedId ?? 0,
				template.AltOffHand.Sigil1?.MappedId ?? 0,
				template.AltOffHand.PvpSigil?.MappedId ?? 0,
				template.AltOffHand.Infusion1?.MappedId ?? 0,
				template.Head.Stat?.MappedId ?? 0,
				template.Head.Rune?.MappedId ?? 0,
				template.Head.Infusion1?.MappedId ?? 0,
				template.Shoulder.Stat?.MappedId ?? 0,
				template.Shoulder.Rune?.MappedId ?? 0,
				template.Shoulder.Infusion1?.MappedId ?? 0,
				template.Chest.Stat?.MappedId ?? 0,
				template.Chest.Rune?.MappedId ?? 0,
				template.Chest.Infusion1?.MappedId ?? 0,
				template.Hand.Stat?.MappedId ?? 0,
				template.Hand.Rune?.MappedId ?? 0,
				template.Hand.Infusion1?.MappedId ?? 0,
				template.Leg.Stat?.MappedId ?? 0,
				template.Leg.Rune?.MappedId ?? 0,
				template.Leg.Infusion1?.MappedId ?? 0,
				template.Foot.Stat?.MappedId ?? 0,
				template.Foot.Rune?.MappedId ?? 0,
				template.Foot.Infusion1?.MappedId ?? 0,
				template.Back.Stat?.MappedId ?? 0,
				template.Back.Infusion1?.MappedId ?? 0,
				template.Back.Infusion2?.MappedId ?? 0,
				template.Amulet.Stat?.MappedId ?? 0,
				template.Amulet.Enrichment?.MappedId ?? 0,
				template.Accessory_1.Stat?.MappedId ?? 0,
				template.Accessory_1.Infusion1?.MappedId ?? 0,
				template.Accessory_2.Stat?.MappedId ?? 0,
				template.Accessory_2.Infusion1?.MappedId ?? 0,
				template.Ring_1.Stat?.MappedId ?? 0,
				template.Ring_1.Infusion1?.MappedId ?? 0,
				template.Ring_1.Infusion2?.MappedId ?? 0,
				template.Ring_1.Infusion3?.MappedId ?? 0,
				template.Ring_2.Stat?.MappedId ?? 0,
				template.Ring_2.Infusion1?.MappedId ?? 0,
				template.Ring_2.Infusion2?.MappedId ?? 0,
				template.Ring_2.Infusion3?.MappedId ?? 0,
				template.AquaBreather.Stat?.MappedId ?? 0,
				template.AquaBreather.Rune?.MappedId ?? 0,
				template.AquaBreather.Infusion1?.MappedId ?? 0,
				(byte)(template.Aquatic.Weapon?.WeaponType ?? ItemWeaponType.Unknown),
				template.Aquatic.Stat?.MappedId ?? 0,
				template.Aquatic.Sigil1?.MappedId ?? 0,
				template.Aquatic.Sigil2?.MappedId ?? 0,
				template.Aquatic.Infusion1?.MappedId ?? 0,
				template.Aquatic.Infusion2?.MappedId ?? 0,
				(byte)(template.AltAquatic.Weapon?.WeaponType ?? ItemWeaponType.Unknown),
				template.AltAquatic.Stat?.MappedId ?? 0,
				template.AltAquatic.Sigil1?.MappedId ?? 0,
				template.AltAquatic.Sigil2?.MappedId ?? 0,
				template.AltAquatic.Infusion1?.MappedId ?? 0,
				template.AltAquatic.Infusion2?.MappedId ?? 0,
				template.PvpAmulet.PvpAmulet?.MappedId ?? 0,
				template.PvpAmulet.Rune?.MappedId ?? 0,
				template.Nourishment.Nourishment?.MappedId ?? 0,
				template.Enhancement.Enhancement?.MappedId ?? 0,
				template.PowerCore.PowerCore?.MappedId ?? 0,
				template.PveRelic.Relic?.MappedId ?? 0,
				template.PvpRelic.Relic?.MappedId ?? 0
			}).ToArray()) + "]";
		}

		public static void LoadTemplateFromChatCode(Template template, string? chatCode)
		{
			if (!string.IsNullOrEmpty(chatCode))
			{
				byte[] array = Convert.FromBase64String(GearTemplateCode.PrepareBase64String(chatCode));
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{array[0]}", out var mainHandWeaponType) ? BuildsManager.Data.Weapons.Values.Where((Weapon e) => e.WeaponType == mainHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[1]).FirstOrDefault().Value);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Sigil1, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[2]).FirstOrDefault().Value);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[4]).FirstOrDefault().Value);
				template.SetItem(template.MainHand.Slot, TemplateSubSlotType.PvpSigil, BuildsManager.Data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[3]).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{array[5]}", out var offHandWeaponType) ? BuildsManager.Data.Weapons.Values.Where((Weapon e) => e.WeaponType == offHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[6]).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Sigil1, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[7]).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[9]).FirstOrDefault().Value);
				template.SetItem(template.OffHand.Slot, TemplateSubSlotType.PvpSigil, BuildsManager.Data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[8]).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{array[10]}", out var altMainHandWeaponType) ? BuildsManager.Data.Weapons.Values.Where((Weapon e) => e.WeaponType == altMainHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[11]).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Sigil1, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[12]).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[14]).FirstOrDefault().Value);
				template.SetItem(template.AltMainHand.Slot, TemplateSubSlotType.PvpSigil, BuildsManager.Data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[13]).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{array[15]}", out var altOffHandWeaponType) ? BuildsManager.Data.Weapons.Values.Where((Weapon e) => e.WeaponType == altOffHandWeaponType).FirstOrDefault() : null);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[16]).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Sigil1, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[17]).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[19]).FirstOrDefault().Value);
				template.SetItem(template.AltOffHand.Slot, TemplateSubSlotType.PvpSigil, BuildsManager.Data.PvpSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[18]).FirstOrDefault().Value);
				template.SetItem(template.Head.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[20]).FirstOrDefault().Value);
				template.SetItem(template.Head.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[21]).FirstOrDefault().Value);
				template.SetItem(template.Head.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[22]).FirstOrDefault().Value);
				template.SetItem(template.Shoulder.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[23]).FirstOrDefault().Value);
				template.SetItem(template.Shoulder.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[24]).FirstOrDefault().Value);
				template.SetItem(template.Shoulder.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[25]).FirstOrDefault().Value);
				template.SetItem(template.Chest.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[26]).FirstOrDefault().Value);
				template.SetItem(template.Chest.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[27]).FirstOrDefault().Value);
				template.SetItem(template.Chest.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[28]).FirstOrDefault().Value);
				template.SetItem(template.Hand.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[29]).FirstOrDefault().Value);
				template.SetItem(template.Hand.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[30]).FirstOrDefault().Value);
				template.SetItem(template.Hand.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[31]).FirstOrDefault().Value);
				template.SetItem(template.Leg.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[32]).FirstOrDefault().Value);
				template.SetItem(template.Leg.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[33]).FirstOrDefault().Value);
				template.SetItem(template.Leg.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[34]).FirstOrDefault().Value);
				template.SetItem(template.Foot.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[35]).FirstOrDefault().Value);
				template.SetItem(template.Foot.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[36]).FirstOrDefault().Value);
				template.SetItem(template.Foot.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[37]).FirstOrDefault().Value);
				template.SetItem(template.Back.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[38]).FirstOrDefault().Value);
				template.SetItem(template.Back.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[39]).FirstOrDefault().Value);
				template.SetItem(template.Back.Slot, TemplateSubSlotType.Infusion2, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[40]).FirstOrDefault().Value);
				template.SetItem(template.Amulet.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[41]).FirstOrDefault().Value);
				template.SetItem(template.Amulet.Slot, TemplateSubSlotType.Enrichment, BuildsManager.Data.Enrichments.Items.Where<KeyValuePair<int, Enrichment>>((KeyValuePair<int, Enrichment> e) => e.Value.MappedId == array[42]).FirstOrDefault().Value);
				template.SetItem(template.Accessory_1.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[43]).FirstOrDefault().Value);
				template.SetItem(template.Accessory_1.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[44]).FirstOrDefault().Value);
				template.SetItem(template.Accessory_2.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[45]).FirstOrDefault().Value);
				template.SetItem(template.Accessory_2.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[46]).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[47]).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[48]).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Infusion2, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[49]).FirstOrDefault().Value);
				template.SetItem(template.Ring_1.Slot, TemplateSubSlotType.Infusion3, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[50]).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[51]).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[52]).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Infusion2, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[53]).FirstOrDefault().Value);
				template.SetItem(template.Ring_2.Slot, TemplateSubSlotType.Infusion3, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[54]).FirstOrDefault().Value);
				template.SetItem(template.AquaBreather.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[55]).FirstOrDefault().Value);
				template.SetItem(template.AquaBreather.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PveRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[56]).FirstOrDefault().Value);
				template.SetItem(template.AquaBreather.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[57]).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{array[58]}", out var aquaticWeaponType) ? BuildsManager.Data.Weapons.Values.Where((Weapon e) => e.WeaponType == aquaticWeaponType).FirstOrDefault() : null);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[59]).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Sigil1, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[60]).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Sigil2, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[61]).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[62]).FirstOrDefault().Value);
				template.SetItem(template.Aquatic.Slot, TemplateSubSlotType.Infusion2, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[63]).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Item, Enum.TryParse<ItemWeaponType>($"{array[64]}", out var altAquaticWeaponType) ? BuildsManager.Data.Weapons.Values.Where((Weapon e) => e.WeaponType == altAquaticWeaponType).FirstOrDefault() : null);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Stat, BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array[65]).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Sigil1, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[66]).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Sigil2, BuildsManager.Data.PveSigils.Items.Where<KeyValuePair<int, Sigil>>((KeyValuePair<int, Sigil> e) => e.Value.MappedId == array[67]).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Infusion1, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[68]).FirstOrDefault().Value);
				template.SetItem(template.AltAquatic.Slot, TemplateSubSlotType.Infusion2, BuildsManager.Data.Infusions.Items.Where<KeyValuePair<int, Infusion>>((KeyValuePair<int, Infusion> e) => e.Value.MappedId == array[69]).FirstOrDefault().Value);
				template.SetItem(template.PvpAmulet.Slot, TemplateSubSlotType.Item, BuildsManager.Data.PvpAmulets.Items.Where((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Items.PvpAmulet> e) => e.Value.MappedId == array[70]).FirstOrDefault().Value);
				template.SetItem(template.PvpAmulet.Slot, TemplateSubSlotType.Rune, BuildsManager.Data.PvpRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array[71]).FirstOrDefault().Value);
				template.SetItem(template.Nourishment.Slot, TemplateSubSlotType.Item, BuildsManager.Data.Nourishments.Items.Where<KeyValuePair<int, Nourishment>>((KeyValuePair<int, Nourishment> e) => e.Value.MappedId == array[72]).FirstOrDefault().Value);
				template.SetItem(template.Enhancement.Slot, TemplateSubSlotType.Item, BuildsManager.Data.Enhancements.Items.Where<KeyValuePair<int, Enhancement>>((KeyValuePair<int, Enhancement> e) => e.Value.MappedId == array[73]).FirstOrDefault().Value);
				template.SetItem(template.PowerCore.Slot, TemplateSubSlotType.Item, BuildsManager.Data.PowerCores.Items.Where<KeyValuePair<int, PowerCore>>((KeyValuePair<int, PowerCore> e) => e.Value.MappedId == array[74]).FirstOrDefault().Value);
				template.SetItem(template.PveRelic.Slot, TemplateSubSlotType.Item, BuildsManager.Data.PveRelics.Items.Where<KeyValuePair<int, Relic>>((KeyValuePair<int, Relic> e) => e.Value.MappedId == array[75]).FirstOrDefault().Value);
				template.SetItem(template.PvpRelic.Slot, TemplateSubSlotType.Item, BuildsManager.Data.PvpRelics.Items.Where<KeyValuePair<int, Relic>>((KeyValuePair<int, Relic> e) => e.Value.MappedId == array[76]).FirstOrDefault().Value);
			}
		}
	}
}
