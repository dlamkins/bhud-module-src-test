using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	public class GearTemplate
	{
		public List<TemplateItem> Trinkets = new List<TemplateItem>
		{
			new TemplateItem
			{
				_Slot = "Back"
			},
			new TemplateItem
			{
				_Slot = "Amulet"
			},
			new TemplateItem
			{
				_Slot = "Ring1"
			},
			new TemplateItem
			{
				_Slot = "Ring2"
			},
			new TemplateItem
			{
				_Slot = "Accessoire1"
			},
			new TemplateItem
			{
				_Slot = "Accessoire2"
			}
		};

		public List<Armor_TemplateItem> Armor = new List<Armor_TemplateItem>
		{
			new Armor_TemplateItem
			{
				_Slot = "Helmet"
			},
			new Armor_TemplateItem
			{
				_Slot = "Shoulders"
			},
			new Armor_TemplateItem
			{
				_Slot = "Chest"
			},
			new Armor_TemplateItem
			{
				_Slot = "Gloves"
			},
			new Armor_TemplateItem
			{
				_Slot = "Leggings"
			},
			new Armor_TemplateItem
			{
				_Slot = "Boots"
			}
		};

		public List<Weapon_TemplateItem> Weapons = new List<Weapon_TemplateItem>
		{
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon1_MainHand
			},
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon1_OffHand
			},
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon2_MainHand
			},
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon2_OffHand
			}
		};

		public List<AquaticWeapon_TemplateItem> AquaticWeapons = new List<AquaticWeapon_TemplateItem>
		{
			new AquaticWeapon_TemplateItem
			{
				Slot = _EquipmentSlots.AquaticWeapon1
			},
			new AquaticWeapon_TemplateItem
			{
				Slot = _EquipmentSlots.AquaticWeapon2
			}
		};

		public string TemplateCode
		{
			get
			{
				string code = "";
				foreach (TemplateItem item4 in Trinkets)
				{
					code = code + "[" + ((item4.Stat != null) ? item4.Stat.Id : 0) + "]";
				}
				foreach (Armor_TemplateItem item3 in Armor)
				{
					code = code + "[" + ((item3.Stat != null) ? item3.Stat.Id : 0) + "|" + ((item3.Rune != null) ? item3.Rune.Id : 0) + "]";
				}
				foreach (Weapon_TemplateItem item2 in Weapons)
				{
					string[] obj = new string[8]
					{
						code,
						"[",
						((item2.Stat != null) ? item2.Stat.Id : 0).ToString(),
						"|",
						null,
						null,
						null,
						null
					};
					int weaponType = (int)item2.WeaponType;
					obj[4] = weaponType.ToString();
					obj[5] = "|";
					obj[6] = ((item2.Sigil != null) ? item2.Sigil.Id : 0).ToString();
					obj[7] = "]";
					code = string.Concat(obj);
				}
				foreach (AquaticWeapon_TemplateItem item in AquaticWeapons)
				{
					string[] obj2 = new string[10]
					{
						code,
						"[",
						((item.Stat != null) ? item.Stat.Id : 0).ToString(),
						"|",
						null,
						null,
						null,
						null,
						null,
						null
					};
					int weaponType = (int)item.WeaponType;
					obj2[4] = weaponType.ToString();
					obj2[5] = "|";
					obj2[6] = ((item.Sigils[0] != null) ? item.Sigils[0].Id : 0).ToString();
					obj2[7] = "|";
					obj2[8] = ((item.Sigils[1] != null) ? item.Sigils[1].Id : 0).ToString();
					obj2[9] = "]";
					code = string.Concat(obj2);
				}
				return code;
			}
		}

		public GearTemplate(string code = null)
		{
			if (code == null)
			{
				return;
			}
			int m = 0;
			string[] ItemStrings = code.Split(']');
			if (ItemStrings.Length != 19)
			{
				return;
			}
			m = 0;
			for (int l = 0; l < Trinkets.Count; l++)
			{
				ItemStrings[l] = ItemStrings[l].Replace("[", "").Replace("]", "");
				int.TryParse(ItemStrings[l], out var id);
				if (id > 0)
				{
					Trinkets[m].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == id);
				}
				BuildsManager.Logger.Debug("Trinkets[" + m + "].Stat: " + Trinkets[m].Stat?.Name);
				m++;
			}
			m = 0;
			for (int k = Trinkets.Count; k < Trinkets.Count + Armor.Count; k++)
			{
				ItemStrings[k] = ItemStrings[k].Replace("[", "").Replace("]", "");
				string[] array = ItemStrings[k].Split('|');
				int.TryParse(array[0], out var stat_id3);
				if (stat_id3 > 0)
				{
					Armor[m].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == stat_id3);
				}
				BuildsManager.Logger.Debug("Armor[" + m + "].Stat: " + Armor[m].Stat?.Name);
				int.TryParse(array[1], out var rune_id);
				if (stat_id3 > 0)
				{
					Armor[m].Rune = BuildsManager.Data.Runes.Find((API.RuneItem e) => e.Id == rune_id);
				}
				BuildsManager.Logger.Debug("Armor[" + m + "].Rune: " + Armor[m].Rune?.Name);
				m++;
			}
			m = 0;
			for (int j = Trinkets.Count + Armor.Count; j < Trinkets.Count + Armor.Count + Weapons.Count; j++)
			{
				ItemStrings[j] = ItemStrings[j].Replace("[", "").Replace("]", "");
				string[] array2 = ItemStrings[j].Split('|');
				int.TryParse(array2[0], out var stat_id2);
				if (stat_id2 > 0)
				{
					Weapons[m].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == stat_id2);
				}
				BuildsManager.Logger.Debug("Weapons[" + m + "].Stat: " + Weapons[m].Stat?.Name);
				int weaponType = -1;
				int.TryParse(array2[1], out weaponType);
				Weapons[m].WeaponType = (API.weaponType)weaponType;
				BuildsManager.Logger.Debug("Weapons[" + m + "].WeaponType: " + Weapons[m].WeaponType);
				int.TryParse(array2[2], out var sigil_id);
				if (stat_id2 > 0)
				{
					Weapons[m].Sigil = BuildsManager.Data.Sigils.Find((API.SigilItem e) => e.Id == sigil_id);
				}
				BuildsManager.Logger.Debug("Weapons[" + m + "].Sigil: " + Weapons[m].Sigil?.Name);
				m++;
			}
			m = 0;
			for (int i = Trinkets.Count + Armor.Count + Weapons.Count; i < Trinkets.Count + Armor.Count + Weapons.Count + AquaticWeapons.Count; i++)
			{
				ItemStrings[i] = ItemStrings[i].Replace("[", "").Replace("]", "");
				string[] array3 = ItemStrings[i].Split('|');
				int.TryParse(array3[0], out var stat_id);
				if (stat_id > 0)
				{
					AquaticWeapons[m].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == stat_id);
				}
				BuildsManager.Logger.Debug("AquaticWeapons[" + m + "].Stat: " + AquaticWeapons[m].Stat?.Name);
				int weaponType2 = -1;
				int.TryParse(array3[1], out weaponType2);
				AquaticWeapons[m].WeaponType = (API.weaponType)weaponType2;
				BuildsManager.Logger.Debug("AquaticWeapons[" + m + "].WeaponType: " + AquaticWeapons[m].WeaponType);
				int.TryParse(array3[2], out var sigil1_id);
				if (sigil1_id > 0)
				{
					AquaticWeapons[m].Sigils[0] = BuildsManager.Data.Sigils.Find((API.SigilItem e) => e.Id == sigil1_id);
				}
				BuildsManager.Logger.Debug("AquaticWeapons[" + m + "].Sigil: " + AquaticWeapons[m].Sigils[0]?.Name);
				int.TryParse(array3[3], out var sigil2_id);
				if (sigil2_id > 0)
				{
					AquaticWeapons[m].Sigils[1] = BuildsManager.Data.Sigils.Find((API.SigilItem e) => e.Id == sigil2_id);
				}
				BuildsManager.Logger.Debug("AquaticWeapons[" + m + "].Sigil: " + AquaticWeapons[m].Sigils[1]?.Name);
				m++;
			}
		}
	}
}
