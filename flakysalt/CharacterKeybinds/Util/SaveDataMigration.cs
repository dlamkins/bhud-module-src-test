using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Util
{
	public class SaveDataMigration
	{
		public static List<Keymap> MigrateToKeymaps(List<CharacterKeybind> characterKeybinds, IEnumerable<Specialization> specializations, out List<string> errors)
		{
			errors = new List<string>();
			try
			{
				List<Keymap> migratedKeymaps = new List<Keymap>();
				foreach (CharacterKeybind keymap in characterKeybinds)
				{
					int id = 0;
					string spezialisation = keymap.spezialisation;
					if (!(spezialisation == "All Specialization"))
					{
						if (spezialisation == "Core")
						{
							id = -1;
						}
						else
						{
							Specialization specialization = specializations.FirstOrDefault((Specialization e) => e.get_Name() == keymap.spezialisation);
							if (specialization == null)
							{
								id = -10;
								Logger.GetLogger<SaveDataMigration>().Warn("Unable to find specialization " + keymap.spezialisation + " and migrate to new data");
								errors.Add(keymap.spezialisation + " for character " + keymap.characterName);
							}
							else
							{
								id = specialization.get_Id();
							}
						}
					}
					else
					{
						id = -2;
					}
					migratedKeymaps.Add(new Keymap
					{
						KeymapName = keymap.keymap,
						CharacterName = keymap.characterName,
						SpecialisationId = id
					});
				}
				return migratedKeymaps;
			}
			catch (Exception e2)
			{
				Logger.GetLogger<SaveDataMigration>().Fatal(e2.Message, new object[1] { e2.StackTrace });
				throw;
			}
		}
	}
}
