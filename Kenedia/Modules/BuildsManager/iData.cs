using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	public class iData : IDisposable
	{
		public class _Legend
		{
			public string Name;

			public int Id;

			public int Skill;

			public List<int> Utilities;

			public int Heal;

			public int Elite;

			public int Swap;

			public int Specialization;
		}

		public class SkillID_Pair
		{
			public int PaletteID;

			public int ID;
		}

		private bool disposed;

		public List<API.Stat> Stats = new List<API.Stat>();

		public List<API.Profession> Professions = new List<API.Profession>();

		public List<API.RuneItem> Runes = new List<API.RuneItem>();

		public List<API.SigilItem> Sigils = new List<API.SigilItem>();

		public List<API.ArmorItem> Armors = new List<API.ArmorItem>();

		public List<API.WeaponItem> Weapons = new List<API.WeaponItem>();

		public List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();

		public List<SkillID_Pair> SkillID_Pairs = new List<SkillID_Pair>();

		public List<API.Legend> Legends = new List<API.Legend>();

		public Texture2D PlaceHolder;

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				Professions.DisposeAll();
				Stats.DisposeAll();
				Runes.DisposeAll();
				Sigils.DisposeAll();
				Armors.DisposeAll();
				Weapons.DisposeAll();
				Trinkets.DisposeAll();
				Legends.DisposeAll();
				Stats.Clear();
				Professions.Clear();
				Runes.Clear();
				Sigils.Clear();
				Armors.Clear();
				Weapons.Clear();
				Trinkets.Clear();
				Legends.Clear();
				SkillID_Pairs.Clear();
				Texture2D placeHolder = PlaceHolder;
				if (placeHolder != null)
				{
					((GraphicsResource)placeHolder).Dispose();
				}
			}
		}

		public void UpdateLanguage()
		{
			string culture = BuildsManager.getCultureString();
			List<string> filesToDelete = new List<string>();
			string file_path = BuildsManager.ModuleInstance.Paths.professions + "professions [" + culture + "].json";
			if (!File.Exists(file_path))
			{
				return;
			}
			foreach (API.Profession entry in JsonConvert.DeserializeObject<List<API.Profession>>(LoadFile(file_path, filesToDelete)))
			{
				API.Profession target = Professions.Find((API.Profession e) => e.Id == entry.Id);
				if (target == null)
				{
					continue;
				}
				target.Name = entry.Name;
				foreach (API.Specialization specialization in entry.Specializations)
				{
					API.Specialization targetSpecialization = target.Specializations.Find((API.Specialization e) => e.Id == specialization.Id);
					if (targetSpecialization == null)
					{
						continue;
					}
					targetSpecialization.Name = specialization.Name;
					foreach (API.Trait trait in specialization.MajorTraits)
					{
						API.Trait targetTrait2 = targetSpecialization.MajorTraits.Find((API.Trait e) => e.Id == trait.Id);
						if (targetTrait2 != null)
						{
							targetTrait2.Name = trait.Name;
							targetTrait2.Description = trait.Description;
						}
					}
					foreach (API.Trait trait2 in specialization.MinorTraits)
					{
						API.Trait targetTrait = targetSpecialization.MinorTraits.Find((API.Trait e) => e.Id == trait2.Id);
						if (targetTrait != null)
						{
							targetTrait.Name = trait2.Name;
							targetTrait.Description = trait2.Description;
						}
					}
					if (specialization.WeaponTrait != null)
					{
						targetSpecialization.WeaponTrait.Name = specialization.WeaponTrait.Name;
						targetSpecialization.WeaponTrait.Description = specialization.WeaponTrait.Description;
					}
				}
				foreach (API.Skill skill in entry.Skills)
				{
					API.Skill targetSkill = target.Skills.Find((API.Skill e) => e.Id == skill.Id);
					if (targetSkill != null)
					{
						targetSkill.Name = skill.Name;
						targetSkill.Description = skill.Description;
					}
				}
				foreach (API.Legend legend in entry.Legends)
				{
					API.Legend targetLegend = target.Legends.Find((API.Legend e) => e.Id == legend.Id);
					if (targetLegend != null)
					{
						targetLegend.Name = legend.Name;
					}
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.stats + "stats [" + culture + "].json";
			foreach (API.Stat tStat in JsonConvert.DeserializeObject<List<API.Stat>>(LoadFile(file_path, filesToDelete)))
			{
				API.Stat stat = Stats.Find((API.Stat e) => e.Id == tStat.Id);
				if (stat == null)
				{
					continue;
				}
				stat.Name = tStat.Name;
				foreach (API.StatAttribute attribute in stat.Attributes)
				{
					attribute.Name = attribute.getLocalName();
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.runes + "runes [" + culture + "].json";
			foreach (API.RuneItem tRune in JsonConvert.DeserializeObject<List<API.RuneItem>>(LoadFile(file_path, filesToDelete)))
			{
				API.RuneItem rune = Runes.Find((API.RuneItem e) => e.Id == tRune.Id);
				if (rune != null)
				{
					rune.Name = tRune.Name;
					rune.Bonuses = tRune.Bonuses;
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.sigils + "sigils [" + culture + "].json";
			foreach (API.SigilItem tSigil in JsonConvert.DeserializeObject<List<API.SigilItem>>(LoadFile(file_path, filesToDelete)))
			{
				API.SigilItem sigil = Sigils.Find((API.SigilItem e) => e.Id == tSigil.Id);
				if (sigil != null)
				{
					sigil.Name = tSigil.Name;
					sigil.Description = tSigil.Description;
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.armory + "armors [" + culture + "].json";
			foreach (API.ArmorItem tArmor in JsonConvert.DeserializeObject<List<API.ArmorItem>>(LoadFile(file_path, filesToDelete)))
			{
				API.ArmorItem armor = Armors.Find((API.ArmorItem e) => e.Id == tArmor.Id);
				if (armor != null)
				{
					armor.Name = tArmor.Name;
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.armory + "weapons [" + culture + "].json";
			foreach (API.WeaponItem tWeapon in JsonConvert.DeserializeObject<List<API.WeaponItem>>(LoadFile(file_path, filesToDelete)))
			{
				API.WeaponItem weapon = Weapons.Find((API.WeaponItem e) => e.Id == tWeapon.Id);
				if (weapon != null)
				{
					weapon.Name = tWeapon.Name;
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.armory + "trinkets [" + culture + "].json";
			foreach (API.TrinketItem tTrinket in JsonConvert.DeserializeObject<List<API.TrinketItem>>(LoadFile(file_path, filesToDelete)))
			{
				API.TrinketItem trinket = Trinkets.Find((API.TrinketItem e) => e.Id == tTrinket.Id);
				if (trinket != null)
				{
					trinket.Name = tTrinket.Name;
				}
			}
		}

		private Texture2D LoadImage(string path, GraphicsDevice graphicsDevice, List<string> filesToDelete, Rectangle region = default(Rectangle), Rectangle default_Bounds = default(Rectangle))
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture = PlaceHolder;
			try
			{
				texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				double factor = 1.0;
				Rectangle val = default(Rectangle);
				if (default_Bounds != val)
				{
					factor = (double)texture.get_Width() / (double)default_Bounds.Width;
				}
				Rectangle val2 = region;
				val = default(Rectangle);
				if (val2 != val)
				{
					region = region.Scale(factor);
					val = texture.get_Bounds();
					if (((Rectangle)(ref val)).Contains(region))
					{
						return Texture2DExtension.GetRegion(texture, region);
					}
					return texture;
				}
				return texture;
			}
			catch (InvalidOperationException)
			{
				if (File.Exists(path))
				{
					filesToDelete.Add(path);
				}
				texture = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Bug);
				BuildsManager.Logger.Debug("InvalidOperationException: Failed to load {0}. Fetching the API again.", new object[1] { path });
				return texture;
			}
			catch (UnauthorizedAccessException)
			{
				return BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Bug);
			}
			catch (FileNotFoundException)
			{
				texture = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Bug);
				BuildsManager.Logger.Debug("FileNotFoundException: Failed to load {0}. Fetching the API again.", new object[1] { path });
				return texture;
			}
			catch (FileLoadException)
			{
				texture = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Bug);
				BuildsManager.Logger.Debug("FileLoadException: Failed to load {0}. Fetching the API again.", new object[1] { path });
				return texture;
			}
		}

		private string LoadFile(string path, List<string> filesToDelete)
		{
			string txt = "";
			try
			{
				txt = File.ReadAllText(path);
				return txt;
			}
			catch (InvalidOperationException)
			{
				if (File.Exists(path))
				{
					filesToDelete.Add(path);
					return txt;
				}
				return txt;
			}
			catch (UnauthorizedAccessException)
			{
				return txt;
			}
			catch (FileNotFoundException)
			{
				return txt;
			}
			catch (FileLoadException)
			{
				return txt;
			}
		}

		public iData()
		{
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_060b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_069d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_0719: Unknown result type (might be due to invalid IL or missing references)
			ContentsManager ContentsManager = BuildsManager.ModuleInstance.ContentsManager;
			_ = BuildsManager.ModuleInstance.DirectoriesManager;
			PlaceHolder = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Bug);
			List<string> filesToDelete = new List<string>();
			string culture = BuildsManager.getCultureString();
			_ = BuildsManager.ModuleInstance.Paths.BasePath + "\\api\\";
			SkillID_Pairs = JsonConvert.DeserializeObject<List<SkillID_Pair>>(new StreamReader(ContentsManager.GetFileStream("data\\skillpalettes.json")).ReadToEnd());
			string file_path = BuildsManager.ModuleInstance.Paths.stats + "stats [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Stats = JsonConvert.DeserializeObject<List<API.Stat>>(LoadFile(file_path, filesToDelete));
			}
			foreach (API.Stat stat in Stats)
			{
				stat.Icon._Texture = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(stat.Icon.Path));
				stat.Attributes.Sort((API.StatAttribute a, API.StatAttribute b) => b.Multiplier.CompareTo(a.Multiplier));
				foreach (API.StatAttribute attri in stat.Attributes)
				{
					attri.Name = attri.getLocalName();
					attri.Icon._Texture = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(attri.Icon.Path));
				}
			}
			file_path = BuildsManager.ModuleInstance.Paths.professions + "professions [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Professions = JsonConvert.DeserializeObject<List<API.Profession>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.ModuleInstance.Paths.runes + "runes [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Runes = JsonConvert.DeserializeObject<List<API.RuneItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.ModuleInstance.Paths.sigils + "sigils [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Sigils = JsonConvert.DeserializeObject<List<API.SigilItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.ModuleInstance.Paths.armory + "armors [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Armors = JsonConvert.DeserializeObject<List<API.ArmorItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.ModuleInstance.Paths.armory + "weapons [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Weapons = JsonConvert.DeserializeObject<List<API.WeaponItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.ModuleInstance.Paths.armory + "trinkets [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Trinkets = JsonConvert.DeserializeObject<List<API.TrinketItem>>(LoadFile(file_path, filesToDelete));
			}
			Trinkets = Trinkets.OrderBy((API.TrinketItem e) => e.TrinketType).ToList();
			Weapons = Weapons.OrderBy((API.WeaponItem e) => (int)e.WeaponType).ToList();
			BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Bug);
			foreach (API.Profession profession in Professions)
			{
				profession.Icon.ImageRegion = new Rectangle(4, 4, 26, 26);
				foreach (API.Specialization specialization in profession.Specializations)
				{
					specialization.Background.ImageRegion = new Rectangle(0, 123, 643, 123);
					if (specialization.WeaponTrait != null)
					{
						specialization.WeaponTrait.Icon.ImageRegion = new Rectangle(3, 3, 58, 58);
						specialization.WeaponTrait.Icon.DefaultBounds = new Rectangle(0, 0, 64, 64);
					}
					foreach (API.Trait majorTrait in specialization.MajorTraits)
					{
						majorTrait.Icon.ImageRegion = new Rectangle(3, 3, 58, 58);
						majorTrait.Icon.DefaultBounds = new Rectangle(0, 0, 64, 64);
					}
					foreach (API.Trait minorTrait in specialization.MinorTraits)
					{
						minorTrait.Icon.ImageRegion = new Rectangle(3, 3, 58, 58);
						minorTrait.Icon.DefaultBounds = new Rectangle(0, 0, 64, 64);
					}
				}
				foreach (API.Skill skill in profession.Skills)
				{
					skill.Icon.ImageRegion = new Rectangle(12, 12, 104, 104);
				}
				if (profession.Legends.Count <= 0)
				{
					continue;
				}
				foreach (API.Legend legend in profession.Legends)
				{
					if (legend.Heal.Icon != null && legend.Heal.Icon.Path != "")
					{
						legend.Heal.Icon.ImageRegion = new Rectangle(12, 12, 104, 104);
					}
					if (legend.Elite.Icon != null && legend.Elite.Icon.Path != "")
					{
						legend.Elite.Icon.ImageRegion = new Rectangle(12, 12, 104, 104);
					}
					if (legend.Swap.Icon != null && legend.Swap.Icon.Path != "")
					{
						legend.Swap.Icon.ImageRegion = new Rectangle(12, 12, 104, 104);
					}
					if (legend.Skill.Icon != null && legend.Skill.Icon.Path != "")
					{
						legend.Skill.Icon.ImageRegion = new Rectangle(12, 12, 104, 104);
					}
					foreach (API.Skill utility in legend.Utilities)
					{
						utility.Icon.ImageRegion = new Rectangle(12, 12, 104, 104);
					}
				}
			}
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				BuildsManager.ModuleInstance.DataLoaded = true;
			});
		}
	}
}
