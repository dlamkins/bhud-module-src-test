using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	public class iData
	{
		public static ContentsManager ContentsManager;

		public static DirectoriesManager DirectoriesManager;

		public List<API.Stat> Stats = new List<API.Stat>();

		public List<API.Profession> Professions = new List<API.Profession>();

		public List<API.RuneItem> Runes = new List<API.RuneItem>();

		public List<API.SigilItem> Sigils = new List<API.SigilItem>();

		public List<API.ArmorItem> Armors = new List<API.ArmorItem>();

		public List<API.WeaponItem> Weapons = new List<API.WeaponItem>();

		public List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();

		public iData(ContentsManager contentsManager = null, DirectoriesManager directoriesManager = null)
		{
			iData iData2 = this;
			if (contentsManager != null)
			{
				ContentsManager = contentsManager;
			}
			if (directoriesManager != null)
			{
				DirectoriesManager = directoriesManager;
			}
			string culture = BuildsManager.getCultureString();
			_ = BuildsManager.Paths.BasePath + "\\api\\";
			string file_path = BuildsManager.Paths.stats + "stats [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Stats = JsonConvert.DeserializeObject<List<API.Stat>>(File.ReadAllText(file_path));
			}
			foreach (API.Stat stat in Stats)
			{
				stat.Icon.Texture = ContentsManager.GetTexture(stat.Icon.Path);
				stat.Attributes.Sort((API.StatAttribute a, API.StatAttribute b) => b.Multiplier.CompareTo(a.Multiplier));
				foreach (API.StatAttribute attri in stat.Attributes)
				{
					attri.Icon.Texture = ContentsManager.GetTexture(attri.Icon.Path);
				}
			}
			file_path = BuildsManager.Paths.professions + "professions [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Professions = JsonConvert.DeserializeObject<List<API.Profession>>(File.ReadAllText(file_path));
			}
			file_path = BuildsManager.Paths.runes + "runes [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Runes = JsonConvert.DeserializeObject<List<API.RuneItem>>(File.ReadAllText(file_path));
			}
			file_path = BuildsManager.Paths.sigils + "sigils [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Sigils = JsonConvert.DeserializeObject<List<API.SigilItem>>(File.ReadAllText(file_path));
			}
			file_path = BuildsManager.Paths.armory + "armors [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Armors = JsonConvert.DeserializeObject<List<API.ArmorItem>>(File.ReadAllText(file_path));
			}
			file_path = BuildsManager.Paths.armory + "weapons [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Weapons = JsonConvert.DeserializeObject<List<API.WeaponItem>>(File.ReadAllText(file_path));
			}
			file_path = BuildsManager.Paths.armory + "trinkets [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Trinkets = JsonConvert.DeserializeObject<List<API.TrinketItem>>(File.ReadAllText(file_path));
			}
			Trinkets = Trinkets.OrderBy((API.TrinketItem e) => e.TrinketType).ToList();
			Weapons = Weapons.OrderBy((API.WeaponItem e) => (int)e.WeaponType).ToList();
			Texture2D texture;
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				foreach (API.TrinketItem current in iData2.Trinkets)
				{
					current.Icon.Texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}
				foreach (API.WeaponItem current2 in iData2.Weapons)
				{
					current2.Icon.Texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current2.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}
				foreach (API.ArmorItem current3 in iData2.Armors)
				{
					current3.Icon.Texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current3.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}
				foreach (API.SigilItem current4 in iData2.Sigils)
				{
					current4.Icon.Texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current4.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}
				foreach (API.RuneItem current5 in iData2.Runes)
				{
					current5.Icon.Texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current5.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}
				foreach (API.Profession current6 in iData2.Professions)
				{
					texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current6.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
					current6.Icon.Texture = Texture2DExtension.GetRegion(texture, 4, 4, 26, 26);
					texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current6.IconBig.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
					current6.IconBig.Texture = texture;
					foreach (API.Specialization current7 in current6.Specializations)
					{
						texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current7.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
						current7.Icon.Texture = texture;
						texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current7.Background.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
						current7.Background.Texture = Texture2DExtension.GetRegion(texture, 0, texture.get_Height() - 133, texture.get_Width() - (texture.get_Width() - 643), texture.get_Height() - (texture.get_Height() - 133));
						if (current7.ProfessionIcon != null)
						{
							texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current7.ProfessionIcon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
							current7.ProfessionIcon.Texture = texture;
						}
						if (current7.ProfessionIconBig != null)
						{
							texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current7.ProfessionIconBig.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
							current7.ProfessionIconBig.Texture = texture;
						}
						if (current7.WeaponTrait != null)
						{
							texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current7.WeaponTrait.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
							current7.WeaponTrait.Icon.Texture = Texture2DExtension.GetRegion(texture, 3, 3, texture.get_Width() - 6, texture.get_Height() - 6);
						}
						foreach (API.Trait current8 in current7.MajorTraits)
						{
							texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current8.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
							current8.Icon.Texture = Texture2DExtension.GetRegion(texture, 3, 3, texture.get_Width() - 6, texture.get_Height() - 6);
						}
						foreach (API.Trait current9 in current7.MinorTraits)
						{
							texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current9.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
							current9.Icon.Texture = Texture2DExtension.GetRegion(texture, 3, 3, texture.get_Width() - 6, texture.get_Height() - 6);
						}
					}
					foreach (API.Skill current10 in current6.Skills)
					{
						texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(current10.Icon.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
						current10.Icon.Texture = Texture2DExtension.GetRegion(texture, 12, 12, texture.get_Width() - 24, texture.get_Height() - 24);
					}
				}
				BuildsManager.DataLoaded = true;
			});
		}
	}
}
