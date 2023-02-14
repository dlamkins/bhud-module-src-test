using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Characters.Models
{
	public class OldCharacterModel
	{
		public static Version ImportVersion = new Version("1.0.3", false);

		public string Name;

		public string Tags;

		public string Icon;

		public int Map;

		public static void Import(string path, ObservableCollection<Character_Model> characters, string imagePath, string accountName, TagList tags)
		{
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				if (!Directory.Exists(imagePath))
				{
					Directory.CreateDirectory(imagePath);
				}
				List<OldCharacterModel> old_characters = JsonConvert.DeserializeObject<List<OldCharacterModel>>(File.ReadAllText(path));
				string basePath = path.Replace("\\" + accountName + "\\characters.json", "");
				imagePath = imagePath.Replace("\\" + accountName + "\\images", "");
				foreach (Character_Model character in characters)
				{
					OldCharacterModel old = old_characters.Find((OldCharacterModel e) => e.Name == character.Name);
					old?.Tags.Split('|')?.ToList()?.ForEach(delegate(string t)
					{
						if (!string.IsNullOrEmpty(t))
						{
							character.AddTag(t);
							if (!tags.Contains(t))
							{
								tags.Add(t);
							}
						}
					});
					Character_Model character_Model = character;
					int map;
					if (old != null)
					{
						_ = old.Map;
						map = old.Map;
					}
					else
					{
						map = 0;
					}
					character_Model.Map = map;
					character.IconPath = ((!string.IsNullOrEmpty(old.Icon)) ? old.Icon : string.Empty);
					if (string.IsNullOrEmpty(old.Icon))
					{
						continue;
					}
					try
					{
						if (File.Exists(basePath + "\\" + old.Icon) && !File.Exists(imagePath + old.Icon))
						{
							BaseModule<Characters, MainWindow, SettingsModel>.Logger.Info("Copy Icon for " + old.Name + " from old path '" + basePath + "\\" + old.Icon + "' to '" + imagePath + old.Icon + "'.");
							File.Copy(basePath + "\\" + old.Icon, imagePath + old.Icon);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			catch
			{
			}
		}
	}
}
