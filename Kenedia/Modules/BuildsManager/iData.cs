using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	public class iData
	{
		public class SkillID_Pair
		{
			public int PaletteID;

			public int ID;
		}

		public static ContentsManager ContentsManager;

		public static DirectoriesManager DirectoriesManager;

		public List<API.Stat> Stats = new List<API.Stat>();

		public List<API.Profession> Professions = new List<API.Profession>();

		public List<API.RuneItem> Runes = new List<API.RuneItem>();

		public List<API.SigilItem> Sigils = new List<API.SigilItem>();

		public List<API.ArmorItem> Armors = new List<API.ArmorItem>();

		public List<API.WeaponItem> Weapons = new List<API.WeaponItem>();

		public List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();

		public List<SkillID_Pair> SkillID_Pairs = new List<SkillID_Pair>();

		private bool fetchAPI;

		private static Texture2D PlaceHolder;

		private Texture2D LoadImage(string path, GraphicsDevice graphicsDevice, List<string> filesToDelete, Rectangle region = default(Rectangle), Rectangle default_Bounds = default(Rectangle))
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
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
				texture = BuildsManager.TextureManager.getIcon(_Icons.Bug);
				fetchAPI = true;
				return texture;
			}
			catch (UnauthorizedAccessException)
			{
				return BuildsManager.TextureManager.getIcon(_Icons.Bug);
			}
			catch (FileNotFoundException)
			{
				texture = BuildsManager.TextureManager.getIcon(_Icons.Bug);
				fetchAPI = true;
				return texture;
			}
			catch (FileLoadException)
			{
				texture = BuildsManager.TextureManager.getIcon(_Icons.Bug);
				fetchAPI = true;
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
				}
				fetchAPI = true;
				return txt;
			}
			catch (UnauthorizedAccessException)
			{
				return txt;
			}
			catch (FileNotFoundException)
			{
				fetchAPI = true;
				return txt;
			}
			catch (FileLoadException)
			{
				fetchAPI = true;
				return txt;
			}
		}

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
			PlaceHolder = BuildsManager.TextureManager.getIcon(_Icons.Bug);
			List<string> filesToDelete = new List<string>();
			string culture = BuildsManager.getCultureString();
			_ = BuildsManager.Paths.BasePath + "\\api\\";
			SkillID_Pairs = JsonConvert.DeserializeObject<List<SkillID_Pair>>(new StreamReader(ContentsManager.GetFileStream("data\\skillpalettes.json")).ReadToEnd());
			string file_path = BuildsManager.Paths.stats + "stats [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Stats = JsonConvert.DeserializeObject<List<API.Stat>>(LoadFile(file_path, filesToDelete));
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
				Professions = JsonConvert.DeserializeObject<List<API.Profession>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.Paths.runes + "runes [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Runes = JsonConvert.DeserializeObject<List<API.RuneItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.Paths.sigils + "sigils [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Sigils = JsonConvert.DeserializeObject<List<API.SigilItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.Paths.armory + "armors [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Armors = JsonConvert.DeserializeObject<List<API.ArmorItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.Paths.armory + "weapons [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Weapons = JsonConvert.DeserializeObject<List<API.WeaponItem>>(LoadFile(file_path, filesToDelete));
			}
			file_path = BuildsManager.Paths.armory + "trinkets [" + culture + "].json";
			if (File.Exists(file_path))
			{
				Trinkets = JsonConvert.DeserializeObject<List<API.TrinketItem>>(LoadFile(file_path, filesToDelete));
			}
			Trinkets = Trinkets.OrderBy((API.TrinketItem e) => e.TrinketType).ToList();
			Weapons = Weapons.OrderBy((API.WeaponItem e) => (int)e.WeaponType).ToList();
			BuildsManager.TextureManager.getIcon(_Icons.Bug);
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_014b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_0154: Unknown result type (might be due to invalid IL or missing references)
				//IL_015a: Unknown result type (might be due to invalid IL or missing references)
				//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_024d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0253: Unknown result type (might be due to invalid IL or missing references)
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				//IL_025c: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_031e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0324: Unknown result type (might be due to invalid IL or missing references)
				//IL_0327: Unknown result type (might be due to invalid IL or missing references)
				//IL_032d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0385: Unknown result type (might be due to invalid IL or missing references)
				//IL_038b: Unknown result type (might be due to invalid IL or missing references)
				//IL_038e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0394: Unknown result type (might be due to invalid IL or missing references)
				//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_03df: Unknown result type (might be due to invalid IL or missing references)
				//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_042a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0430: Unknown result type (might be due to invalid IL or missing references)
				//IL_0433: Unknown result type (might be due to invalid IL or missing references)
				//IL_0439: Unknown result type (might be due to invalid IL or missing references)
				//IL_047e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0484: Unknown result type (might be due to invalid IL or missing references)
				//IL_0487: Unknown result type (might be due to invalid IL or missing references)
				//IL_048d: Unknown result type (might be due to invalid IL or missing references)
				//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_0546: Unknown result type (might be due to invalid IL or missing references)
				//IL_0551: Unknown result type (might be due to invalid IL or missing references)
				//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0664: Unknown result type (might be due to invalid IL or missing references)
				//IL_066b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0671: Unknown result type (might be due to invalid IL or missing references)
				foreach (API.TrinketItem current in iData2.Trinkets)
				{
					current.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current.Icon.Path, graphicsDevice, filesToDelete);
				}
				foreach (API.WeaponItem current2 in iData2.Weapons)
				{
					current2.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current2.Icon.Path, graphicsDevice, filesToDelete);
				}
				foreach (API.ArmorItem current3 in iData2.Armors)
				{
					current3.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current3.Icon.Path, graphicsDevice, filesToDelete);
				}
				foreach (API.SigilItem current4 in iData2.Sigils)
				{
					current4.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current4.Icon.Path, graphicsDevice, filesToDelete);
				}
				foreach (API.RuneItem current5 in iData2.Runes)
				{
					current5.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current5.Icon.Path, graphicsDevice, filesToDelete);
				}
				foreach (API.Profession current6 in iData2.Professions)
				{
					current6.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current6.Icon.Path, graphicsDevice, filesToDelete, new Rectangle(4, 4, 26, 26));
					current6.IconBig.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current6.IconBig.Path, graphicsDevice, filesToDelete);
					foreach (API.Specialization current7 in current6.Specializations)
					{
						current7.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current7.Icon.Path, graphicsDevice, filesToDelete);
						current7.Background.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current7.Background.Path, graphicsDevice, filesToDelete, new Rectangle(0, 123, 643, 123));
						if (current7.ProfessionIcon != null)
						{
							current7.ProfessionIcon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current7.ProfessionIcon.Path, graphicsDevice, filesToDelete);
						}
						if (current7.ProfessionIconBig != null)
						{
							current7.ProfessionIconBig.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current7.ProfessionIconBig.Path, graphicsDevice, filesToDelete);
						}
						if (current7.WeaponTrait != null)
						{
							current7.WeaponTrait.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current7.WeaponTrait.Icon.Path, graphicsDevice, filesToDelete, new Rectangle(3, 3, 58, 58));
						}
						foreach (API.Trait current8 in current7.MajorTraits)
						{
							current8.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current8.Icon.Path, graphicsDevice, filesToDelete, new Rectangle(3, 3, 58, 58), new Rectangle(0, 0, 64, 64));
						}
						foreach (API.Trait current9 in current7.MinorTraits)
						{
							current9.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current9.Icon.Path, graphicsDevice, filesToDelete, new Rectangle(3, 3, 58, 58));
						}
					}
					foreach (API.Skill current10 in current6.Skills)
					{
						current10.Icon.Texture = iData2.LoadImage(BuildsManager.Paths.BasePath + current10.Icon.Path, graphicsDevice, filesToDelete, new Rectangle(12, 12, 104, 104));
					}
				}
				foreach (string current11 in filesToDelete)
				{
					try
					{
						File.Delete(current11);
					}
					catch (IOException)
					{
					}
				}
				if (iData2.fetchAPI)
				{
					iData2.fetchAPI = false;
					BuildsManager.ModuleInstance.Fetch_APIData(force: true);
				}
				else
				{
					BuildsManager.DataLoaded = true;
				}
			});
		}
	}
}
