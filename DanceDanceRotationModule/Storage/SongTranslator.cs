using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using DanceDanceRotationModule.Model;
using Newtonsoft.Json;

namespace DanceDanceRotationModule.Storage
{
	public class SongTranslator
	{
		protected struct SongJson
		{
			public struct BuildTemplate
			{
				public struct Specialization
				{
					public int id { get; set; }
				}

				public struct Skills
				{
					public struct UtilitySkills
					{
						public int heal { get; set; }

						public List<int> utilities { get; set; }

						public int elite { get; set; }
					}

					public UtilitySkills terrestrial { get; set; }

					public UtilitySkills aquatic { get; set; }
				}

				public int profession { get; set; }

				public List<Specialization> specializations { get; set; }

				public Skills skills { get; set; }
			}

			public struct Note
			{
				public double time { get; set; }

				public double duration { get; set; }

				public string noteType { get; set; }

				public int abilityId { get; set; }

				public bool overrideAuto { get; set; }
			}

			public string name { get; set; }

			public string description { get; set; }

			public List<Note> notes { get; set; }

			public string logUrl { get; set; }

			public string buildUrl { get; set; }

			public string buildChatCode { get; set; }

			public BuildTemplate decodedBuildTemplate { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<SongTranslator>();

		public static Song FromJson(string json)
		{
			SongJson songJson = JsonConvert.DeserializeObject<SongJson>(json);
			List<int> utilities = songJson.decodedBuildTemplate.skills.terrestrial.utilities;
			if (utilities == null)
			{
				utilities = new List<int>(3);
			}
			while (utilities.Count < 3)
			{
				utilities.Add(0);
			}
			Profession profession = ProfessionExtensions.ProfessionFromBuildTemplate(songJson.decodedBuildTemplate.profession);
			string eliteName = EliteNameFromBuildTemplate(profession, songJson.decodedBuildTemplate.specializations.Last().id);
			return new Song
			{
				Id = new Song.ID(songJson.name),
				Description = songJson.description,
				BuildUrl = songJson.buildUrl,
				BuildTemplateCode = songJson.buildChatCode,
				Utility1 = new PaletteId(utilities[0]),
				Utility2 = new PaletteId(utilities[1]),
				Utility3 = new PaletteId(utilities[2]),
				Profession = profession,
				EliteName = eliteName,
				Notes = songJson.notes.Select(delegate(SongJson.Note noteJson)
				{
					if (!Enum.TryParse<NoteType>(noteJson.noteType, out var result))
					{
						Logger.Warn("Unknown Note Type: '" + noteJson.noteType + "'");
						result = NoteType.Unknown;
					}
					return new Note(result, TimeSpan.FromMilliseconds(Math.Round(noteJson.time)), TimeSpan.FromMilliseconds(Math.Round(noteJson.duration)), new AbilityId(noteJson.abilityId), noteJson.overrideAuto);
				}).ToList()
			};
		}

		private static string EliteNameFromBuildTemplate(Profession profession, int buildTemplateCode)
		{
			return buildTemplateCode switch
			{
				5 => "Druid", 
				7 => "Daredevil", 
				18 => "Berserker", 
				27 => "Dragonhunter", 
				34 => "Reaper", 
				40 => "Chronomancer", 
				43 => "Scrapper", 
				48 => "Tempest", 
				52 => "Herald", 
				55 => "Soulbeast", 
				56 => "Weaver", 
				57 => "Holosmith", 
				58 => "Deadeye", 
				59 => "Mirage", 
				60 => "Scourge", 
				61 => "Spellbreaker", 
				62 => "Firebrand", 
				63 => "Renegade", 
				64 => "Harbinger", 
				65 => "Willbender", 
				66 => "Virtuoso", 
				67 => "Catalyst", 
				68 => "Bladesworn", 
				69 => "Vindicator", 
				70 => "Mechanist", 
				71 => "Specter", 
				72 => "Untamed", 
				_ => ProfessionExtensions.GetProfessionDisplayText(profession), 
			};
		}
	}
}
