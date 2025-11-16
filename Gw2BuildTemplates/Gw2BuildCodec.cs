using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Gw2BuildTemplates
{
	public static class Gw2BuildCodec
	{
		private static readonly byte BuildHeader = 13;

		public static ItemWeaponType GetFromTemplateWeapon(TemplateWeaponType weaponType)
		{
			return weaponType switch
			{
				TemplateWeaponType.Axe => ItemWeaponType.Axe, 
				TemplateWeaponType.LongBow => ItemWeaponType.LongBow, 
				TemplateWeaponType.Dagger => ItemWeaponType.Dagger, 
				TemplateWeaponType.Focus => ItemWeaponType.Focus, 
				TemplateWeaponType.Greatsword => ItemWeaponType.Greatsword, 
				TemplateWeaponType.Hammer => ItemWeaponType.Hammer, 
				TemplateWeaponType.Mace => ItemWeaponType.Mace, 
				TemplateWeaponType.Pistol => ItemWeaponType.Pistol, 
				TemplateWeaponType.Rifle => ItemWeaponType.Rifle, 
				TemplateWeaponType.Scepter => ItemWeaponType.Scepter, 
				TemplateWeaponType.Shield => ItemWeaponType.Shield, 
				TemplateWeaponType.Staff => ItemWeaponType.Staff, 
				TemplateWeaponType.Sword => ItemWeaponType.Sword, 
				TemplateWeaponType.Torch => ItemWeaponType.Torch, 
				TemplateWeaponType.Warhorn => ItemWeaponType.Warhorn, 
				TemplateWeaponType.ShortBow => ItemWeaponType.ShortBow, 
				TemplateWeaponType.Harpoon => ItemWeaponType.Harpoon, 
				_ => ItemWeaponType.Unknown, 
			};
		}

		public static TemplateWeaponType GetToTemplateWeapon(ItemWeaponType weaponType)
		{
			return weaponType switch
			{
				ItemWeaponType.Axe => TemplateWeaponType.Axe, 
				ItemWeaponType.LongBow => TemplateWeaponType.LongBow, 
				ItemWeaponType.Dagger => TemplateWeaponType.Dagger, 
				ItemWeaponType.Focus => TemplateWeaponType.Focus, 
				ItemWeaponType.Greatsword => TemplateWeaponType.Greatsword, 
				ItemWeaponType.Hammer => TemplateWeaponType.Hammer, 
				ItemWeaponType.Mace => TemplateWeaponType.Mace, 
				ItemWeaponType.Pistol => TemplateWeaponType.Pistol, 
				ItemWeaponType.Rifle => TemplateWeaponType.Rifle, 
				ItemWeaponType.Scepter => TemplateWeaponType.Scepter, 
				ItemWeaponType.Shield => TemplateWeaponType.Shield, 
				ItemWeaponType.Staff => TemplateWeaponType.Staff, 
				ItemWeaponType.Sword => TemplateWeaponType.Sword, 
				ItemWeaponType.Torch => TemplateWeaponType.Torch, 
				ItemWeaponType.Warhorn => TemplateWeaponType.Warhorn, 
				ItemWeaponType.ShortBow => TemplateWeaponType.ShortBow, 
				ItemWeaponType.Harpoon => TemplateWeaponType.Harpoon, 
				_ => TemplateWeaponType.None, 
			};
		}

		public static bool TryDecode(string chatCode, out BuildTemplate build)
		{
			try
			{
				build = Decode(chatCode);
				return true;
			}
			catch
			{
				build = null;
				return false;
			}
		}

		public static bool TryEncode(BuildTemplate build, out string chatCode)
		{
			try
			{
				chatCode = Encode(build);
				return true;
			}
			catch
			{
				chatCode = null;
				return false;
			}
		}

		public static BuildTemplate Decode(string chatCode)
		{
			if (chatCode.StartsWith("[&"))
			{
				chatCode = chatCode.Substring(2, chatCode.Length - 3);
			}
			byte[] raw = Convert.FromBase64String(chatCode);
			BuildTemplate build = new BuildTemplate();
			if (raw.Length < 1)
			{
				return build;
			}
			if (raw[0] != BuildHeader)
			{
				throw new Exception("Not a valid build template chat code.");
			}
			int p = 1;
			build.Profession = (ProfessionType)raw[p++];
			build.Specializations = new SpecializationEntry[3];
			for (int k = 0; k < 3; k++)
			{
				byte spec = raw[p++];
				byte num = raw[p++];
				byte t1 = (byte)(num & 3u);
				byte t2 = (byte)((uint)(num >> 2) & 3u);
				byte t3 = (byte)((uint)(num >> 4) & 3u);
				build.Specializations[k] = new SpecializationEntry
				{
					SpecializationId = spec,
					Trait1 = t1,
					Trait2 = t2,
					Trait3 = t3
				};
			}
			build.TerrestrialHeal = ReadU16(raw, ref p);
			build.AquaticHeal = ReadU16(raw, ref p);
			build.TerrestrialUtility1 = ReadU16(raw, ref p);
			build.AquaticUtility1 = ReadU16(raw, ref p);
			build.TerrestrialUtility2 = ReadU16(raw, ref p);
			build.AquaticUtility2 = ReadU16(raw, ref p);
			build.TerrestrialUtility3 = ReadU16(raw, ref p);
			build.AquaticUtility3 = ReadU16(raw, ref p);
			build.TerrestrialElite = ReadU16(raw, ref p);
			build.AquaticElite = ReadU16(raw, ref p);
			byte[] professionBlock = raw.Skip(p).Take(16).ToArray();
			p += 16;
			if (build.Profession == ProfessionType.Ranger)
			{
				build.RangerPets = new RangerPetData
				{
					Terrestrial1 = professionBlock[0],
					Terrestrial2 = professionBlock[1],
					Aquatic1 = professionBlock[2],
					Aquatic2 = professionBlock[3]
				};
			}
			else if (build.Profession == ProfessionType.Revenant)
			{
				build.RevenantLegends = new RevenantLegendData
				{
					TerrestrialLegend1 = professionBlock[0],
					TerrestrialLegend2 = professionBlock[1],
					AquaticLegend1 = professionBlock[2],
					AquaticLegend2 = professionBlock[3],
					InactiveTerrestrial1 = (ushort)(professionBlock[4] | (professionBlock[5] << 8)),
					InactiveTerrestrial2 = (ushort)(professionBlock[6] | (professionBlock[7] << 8)),
					InactiveTerrestrial3 = (ushort)(professionBlock[8] | (professionBlock[9] << 8)),
					InactiveAquatic1 = (ushort)(professionBlock[10] | (professionBlock[11] << 8)),
					InactiveAquatic2 = (ushort)(professionBlock[12] | (professionBlock[13] << 8)),
					InactiveAquatic3 = (ushort)(professionBlock[14] | (professionBlock[15] << 8))
				};
			}
			if (p >= raw.Length)
			{
				return build;
			}
			byte weaponCount = raw[p++];
			for (int j = 0; j < weaponCount; j++)
			{
				build.SelectedWeapons.Add((TemplateWeaponType)ReadU16(raw, ref p));
			}
			byte overrideCount = raw[p++];
			for (int i = 0; i < overrideCount; i++)
			{
				uint id = (uint)(raw[p] | (raw[p + 1] << 8) | (raw[p + 2] << 16) | (raw[p + 3] << 24));
				p += 4;
				build.SkillOverrides.Add(id);
			}
			return build;
		}

		public static string Encode(BuildTemplate t)
		{
			List<byte> buf = new List<byte>();
			buf.Add(BuildHeader);
			buf.Add((byte)t.Profession);
			SpecializationEntry[] specializations = t.Specializations;
			foreach (SpecializationEntry s in specializations)
			{
				buf.Add(s.SpecializationId);
				byte traitByte = (byte)((s.Trait1 & 3u) | (uint)((s.Trait2 & 3) << 2) | (uint)((s.Trait3 & 3) << 4));
				buf.Add(traitByte);
			}
			WriteU16(buf, t.TerrestrialHeal);
			WriteU16(buf, t.AquaticHeal);
			WriteU16(buf, t.TerrestrialUtility1);
			WriteU16(buf, t.AquaticUtility1);
			WriteU16(buf, t.TerrestrialUtility2);
			WriteU16(buf, t.AquaticUtility2);
			WriteU16(buf, t.TerrestrialUtility3);
			WriteU16(buf, t.AquaticUtility3);
			WriteU16(buf, t.TerrestrialElite);
			WriteU16(buf, t.AquaticElite);
			byte[] profData = new byte[16];
			if (t.Profession == ProfessionType.Ranger && t.RangerPets != null)
			{
				profData[0] = t.RangerPets.Terrestrial1;
				profData[1] = t.RangerPets.Terrestrial2;
				profData[2] = t.RangerPets.Aquatic1;
				profData[3] = t.RangerPets.Aquatic2;
			}
			else if (t.Profession == ProfessionType.Revenant && t.RevenantLegends != null)
			{
				profData[0] = t.RevenantLegends.TerrestrialLegend1;
				profData[1] = t.RevenantLegends.TerrestrialLegend2;
				profData[2] = t.RevenantLegends.AquaticLegend1;
				profData[3] = t.RevenantLegends.AquaticLegend2;
				WriteU16Into(profData, 4, t.RevenantLegends.InactiveTerrestrial1);
				WriteU16Into(profData, 6, t.RevenantLegends.InactiveTerrestrial2);
				WriteU16Into(profData, 8, t.RevenantLegends.InactiveTerrestrial3);
				WriteU16Into(profData, 10, t.RevenantLegends.InactiveAquatic1);
				WriteU16Into(profData, 12, t.RevenantLegends.InactiveAquatic2);
				WriteU16Into(profData, 14, t.RevenantLegends.InactiveAquatic3);
			}
			buf.AddRange(profData);
			buf.Add((byte)t.SelectedWeapons.Count);
			foreach (TemplateWeaponType w in t.SelectedWeapons)
			{
				WriteU16(buf, (ushort)w);
			}
			buf.Add((byte)t.SkillOverrides.Count);
			foreach (uint o in t.SkillOverrides)
			{
				buf.Add((byte)(o & 0xFFu));
				buf.Add((byte)((o >> 8) & 0xFFu));
				buf.Add((byte)((o >> 16) & 0xFFu));
				buf.Add((byte)((o >> 24) & 0xFFu));
			}
			string base64 = Convert.ToBase64String(buf.ToArray());
			return "[&" + base64 + "]";
		}

		private static ushort ReadU16(byte[] raw, ref int p)
		{
			ushort result = (ushort)(raw[p] | (raw[p + 1] << 8));
			p += 2;
			return result;
		}

		private static void WriteU16(List<byte> buf, ushort v)
		{
			buf.Add((byte)(v & 0xFFu));
			buf.Add((byte)((uint)(v >> 8) & 0xFFu));
		}

		private static void WriteU16Into(byte[] arr, int offset, ushort v)
		{
			arr[offset] = (byte)(v & 0xFFu);
			arr[offset + 1] = (byte)((uint)(v >> 8) & 0xFFu);
		}
	}
}
