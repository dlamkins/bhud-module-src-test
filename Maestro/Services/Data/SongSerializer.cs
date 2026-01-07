using System;
using System.Collections.Generic;
using System.IO;
using Maestro.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Maestro.Services.Data
{
	public static class SongSerializer
	{
		private class SongCompactJsonDto
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("artist")]
			public string Artist { get; set; }

			[JsonProperty("instrument")]
			public string Instrument { get; set; }

			[JsonProperty("bpm")]
			public int? Bpm { get; set; }

			[JsonProperty("notes")]
			public List<string> Notes { get; set; }
		}

		private static readonly JsonSerializerSettings JsonSettings;

		public static Song DeserializeJson(string filePath)
		{
			return DeserializeJsonContent(File.ReadAllText(filePath));
		}

		public static Song DeserializeJsonContent(string json)
		{
			SongCompactJsonDto dto = JsonConvert.DeserializeObject<SongCompactJsonDto>(json, JsonSettings);
			Enum.TryParse<InstrumentType>(dto.Instrument, out var instrument);
			Song song = new Song
			{
				Name = dto.Name,
				Artist = dto.Artist,
				Instrument = instrument,
				Bpm = dto.Bpm
			};
			if (dto.Notes != null && dto.Bpm.HasValue)
			{
				List<SongCommand> commands = NoteParser.Parse(dto.Notes, dto.Bpm.Value);
				song.Commands.AddRange(commands);
			}
			return song;
		}

		public static List<Song> DeserializeJsonArray(string json)
		{
			List<Song> songs = new List<Song>();
			List<SongCompactJsonDto> dtos = JsonConvert.DeserializeObject<List<SongCompactJsonDto>>(json, JsonSettings);
			if (dtos == null)
			{
				return songs;
			}
			foreach (SongCompactJsonDto dto in dtos)
			{
				Enum.TryParse<InstrumentType>(dto.Instrument, out var instrument);
				Song song = new Song
				{
					Name = dto.Name,
					Artist = dto.Artist,
					Instrument = instrument,
					Bpm = dto.Bpm
				};
				if (dto.Notes != null && dto.Bpm.HasValue)
				{
					List<SongCommand> commands = NoteParser.Parse(dto.Notes, dto.Bpm.Value);
					song.Commands.AddRange(commands);
				}
				songs.Add(song);
			}
			return songs;
		}

		static SongSerializer()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0021: Expected O, but got Unknown
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_Formatting((Formatting)1);
			val.get_Converters().Add((JsonConverter)new StringEnumConverter());
			JsonSettings = val;
		}
	}
}
