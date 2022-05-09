using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	public class Template
	{
		public enum _TrinketSlots
		{
			Back,
			Amulet,
			Ring1,
			Ring2,
			Accessoire1,
			Accessoire2
		}

		public enum _AmorSlots
		{
			Helmet,
			Shoulders,
			Chest,
			Gloves,
			Leggings,
			Boots
		}

		public enum _WeaponSlots
		{
			Weapon1_MainHand,
			Weapon1_OffHand,
			Weapon2_MainHand,
			Weapon2_OffHand
		}

		private enum _AquaticWeaponSlots
		{
			AquaticWeapon1,
			AquaticWeapon2
		}

		public Template_json Template_json;

		public string Name;

		public API.Profession Profession;

		public API.Specialization Specialization;

		public string Path;

		public GearTemplate Gear = new GearTemplate();

		public BuildTemplate Build;

		public event EventHandler Edit;

		public Template(string path = null)
		{
			if (path != null && File.Exists(path))
			{
				Template_json template = JsonConvert.DeserializeObject<Template_json>(File.ReadAllText(path));
				if (template == null)
				{
					return;
				}
				Template_json = template;
				Name = template.Name;
				Profession = BuildsManager.Data.Professions.Find((API.Profession e) => e.Id == template.Profession);
				Specialization = ((Profession != null) ? Profession.Specializations.Find((API.Specialization e) => e.Id == template.Specialization) : null);
				Path = path.Replace(Name + ".json", "");
				foreach (Armor_TemplateItem_json jItem in Template_json.Gear.Armor)
				{
					int index4 = (int)Enum.Parse(typeof(_AmorSlots), jItem._Slot);
					if (Gear.Armor[index4] == null)
					{
						Gear.Armor[index4] = new Armor_TemplateItem();
					}
					Gear.Armor[index4].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == jItem._Stat);
					Gear.Armor[index4].Rune = BuildsManager.Data.Runes.Find((API.RuneItem e) => e.Id == jItem._Rune);
					Gear.Armor[index4].Slot = (_EquipmentSlots)Enum.Parse(typeof(_EquipmentSlots), jItem._Slot);
				}
				foreach (TemplateItem_json jItem2 in Template_json.Gear.Trinkets)
				{
					int index3 = (int)Enum.Parse(typeof(_TrinketSlots), jItem2._Slot);
					if (Gear.Trinkets[index3] == null)
					{
						Gear.Trinkets[index3] = new TemplateItem();
					}
					Gear.Trinkets[index3].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == jItem2._Stat);
					Gear.Trinkets[index3].Slot = (_EquipmentSlots)Enum.Parse(typeof(_EquipmentSlots), jItem2._Slot);
				}
				foreach (Weapon_TemplateItem_json jItem3 in Template_json.Gear.Weapons)
				{
					int index2 = (int)Enum.Parse(typeof(_WeaponSlots), jItem3._Slot);
					if (Gear.Weapons[index2] == null)
					{
						Gear.Weapons[index2] = new Weapon_TemplateItem();
					}
					Gear.Weapons[index2].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == jItem3._Stat);
					Gear.Weapons[index2].Sigil = BuildsManager.Data.Sigils.Find((API.SigilItem e) => e.Id == jItem3._Sigil);
					Gear.Weapons[index2].Slot = (_EquipmentSlots)Enum.Parse(typeof(_EquipmentSlots), jItem3._Slot);
					Gear.Weapons[index2].WeaponType = (API.weaponType)Enum.Parse(typeof(API.weaponType), jItem3._WeaponType);
				}
				foreach (AquaticWeapon_TemplateItem_json jItem4 in Template_json.Gear.AquaticWeapons)
				{
					int index = (int)Enum.Parse(typeof(_AquaticWeaponSlots), jItem4._Slot);
					if (Gear.AquaticWeapons[index] == null)
					{
						Gear.AquaticWeapons[index] = new AquaticWeapon_TemplateItem();
					}
					Gear.AquaticWeapons[index].Stat = BuildsManager.Data.Stats.Find((API.Stat e) => e.Id == jItem4._Stat);
					Gear.AquaticWeapons[index].Slot = (_EquipmentSlots)Enum.Parse(typeof(_EquipmentSlots), jItem4._Slot);
					Gear.AquaticWeapons[index].WeaponType = (API.weaponType)Enum.Parse(typeof(API.weaponType), jItem4._WeaponType);
					Gear.AquaticWeapons[index].Sigils = new List<API.SigilItem>();
					if (jItem4._Sigils != null)
					{
						foreach (int id in jItem4._Sigils)
						{
							Gear.AquaticWeapons[index].Sigils.Add(BuildsManager.Data.Sigils.Find((API.SigilItem e) => e.Id == id));
						}
					}
					for (int i = Gear.AquaticWeapons[index].Sigils.Count; i < 2; i++)
					{
						Gear.AquaticWeapons[index].Sigils.Add(new API.SigilItem());
					}
				}
				Build = new BuildTemplate(Template_json.BuildCode);
				return;
			}
			Gear = new GearTemplate();
			Build = new BuildTemplate();
			Name = "[No Name Set]";
			Template_json = new Template_json();
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player != null)
			{
				Profession = BuildsManager.Data.Professions.Find(delegate(API.Profession e)
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					string id2 = e.Id;
					ProfessionType profession = player.get_Profession();
					return id2 == ((object)(ProfessionType)(ref profession)).ToString();
				});
				Path = BuildsManager.Paths.builds;
			}
			SetChanged();
		}

		public void Reset()
		{
			Name = "[No Name Set]";
			Template_json = new Template_json
			{
				Name = Name
			};
			Specialization = null;
			foreach (TemplateItem trinket in Gear.Trinkets)
			{
				trinket.Stat = null;
			}
			foreach (Armor_TemplateItem item in Gear.Armor)
			{
				item.Stat = null;
				item.Rune = null;
			}
			foreach (Weapon_TemplateItem weapon in Gear.Weapons)
			{
				weapon.WeaponType = API.weaponType.Unkown;
				weapon.Stat = null;
				weapon.Sigil = null;
			}
			foreach (AquaticWeapon_TemplateItem aquaticWeapon in Gear.AquaticWeapons)
			{
				aquaticWeapon.WeaponType = API.weaponType.Unkown;
				aquaticWeapon.Stat = null;
				aquaticWeapon.Sigils = new List<API.SigilItem>
				{
					new API.SigilItem(),
					new API.SigilItem()
				};
			}
			Build = new BuildTemplate();
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player != null)
			{
				Profession = BuildsManager.Data.Professions.Find(delegate(API.Profession e)
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					string id = e.Id;
					ProfessionType profession = player.get_Profession();
					return id == ((object)(ProfessionType)(ref profession)).ToString();
				});
				Path = BuildsManager.Paths.builds;
			}
			SetChanged();
		}

		public void Delete()
		{
			if (Path != null && !(Name == "[No Name Set]"))
			{
				string path = Path + Name + ".json";
				FileInfo fi = null;
				try
				{
					fi = new FileInfo(path);
				}
				catch (ArgumentException)
				{
				}
				catch (PathTooLongException)
				{
				}
				catch (NotSupportedException)
				{
				}
				if (Name.Contains("/") || Name.Contains("\\") || fi == null)
				{
					ScreenNotification.ShowNotification(Name + " is not a valid Name!", (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				File.Delete(Path + Name + ".json");
				BuildsManager.ModuleInstance.OnTemplate_Deleted();
			}
		}

		public void Save()
		{
			if (Path == null || Name == "[No Name Set]")
			{
				return;
			}
			string path = Path + Name + ".json";
			FileInfo fi = null;
			try
			{
				fi = new FileInfo(path);
			}
			catch (ArgumentException)
			{
			}
			catch (PathTooLongException)
			{
			}
			catch (NotSupportedException)
			{
			}
			if (Name.Contains("/") || Name.Contains("\\") || fi == null)
			{
				ScreenNotification.ShowNotification(Name + " is not a valid Name!", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			bool existsAlready = File.Exists(Path + Name + ".json") || File.Exists(Path + Template_json.Name + ".json");
			if (Template_json.Name != Name)
			{
				File.Delete(Path + Template_json.Name + ".json");
			}
			Template_json.Name = Name;
			Template_json.Profession = ((Profession != null) ? Profession.Id : "Unkown");
			Template_json.Specialization = ((Specialization != null) ? Specialization.Id : 0);
			Template_json.Gear = new GearTemplate_json();
			for (int l = 0; l < Gear.Trinkets.Count; l++)
			{
				TemplateItem item = Gear.Trinkets[l];
				Template_json.Gear.Trinkets[l]._Stat = ((item.Stat != null) ? item.Stat.Id : 0);
			}
			for (int k = 0; k < Gear.Armor.Count; k++)
			{
				Armor_TemplateItem item2 = Gear.Armor[k];
				Template_json.Gear.Armor[k]._Stat = ((item2.Stat != null) ? item2.Stat.Id : 0);
				Template_json.Gear.Armor[k]._Rune = ((item2.Rune != null) ? item2.Rune.Id : 0);
			}
			for (int j = 0; j < Gear.Weapons.Count; j++)
			{
				Weapon_TemplateItem item3 = Gear.Weapons[j];
				Template_json.Gear.Weapons[j]._Stat = ((item3.Stat != null) ? item3.Stat.Id : 0);
				Template_json.Gear.Weapons[j]._Sigil = ((item3.Sigil != null) ? item3.Sigil.Id : 0);
				Template_json.Gear.Weapons[j]._WeaponType = item3.WeaponType.ToString();
			}
			for (int i = 0; i < Gear.AquaticWeapons.Count; i++)
			{
				AquaticWeapon_TemplateItem item4 = Gear.AquaticWeapons[i];
				Template_json.Gear.AquaticWeapons[i]._Stat = ((item4.Stat != null) ? item4.Stat.Id : 0);
				Template_json.Gear.AquaticWeapons[i]._Sigils = ((item4.Sigils != null) ? item4.Sigils.Select((API.SigilItem e) => e?.Id ?? 0).ToList() : new List<int>());
				Template_json.Gear.AquaticWeapons[i]._WeaponType = item4.WeaponType.ToString();
			}
			Template_json.BuildCode = Build.ParseBuildCode();
			Template_json.GearCode = BuildsManager.ModuleInstance.Selected_Template?.Gear.TemplateCode;
			File.WriteAllText(Path + Name + ".json", JsonConvert.SerializeObject((object)Template_json));
			if (!existsAlready)
			{
				BuildsManager.ModuleInstance.OnTemplates_Loaded();
			}
		}

		private void OnEdit(object sender, EventArgs e)
		{
			this.Edit?.Invoke(this, EventArgs.Empty);
			Save();
		}

		public void SetChanged()
		{
			OnEdit(null, EventArgs.Empty);
		}
	}
}
