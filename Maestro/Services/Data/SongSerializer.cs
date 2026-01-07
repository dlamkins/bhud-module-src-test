using System;
using System.Collections.Generic;
using System.IO;
using Maestro.Models;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Maestro.Services.Data
{
	public static class SongSerializer
	{
		private class SongJsonDto
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("artist")]
			public string Artist { get; set; }

			[JsonProperty("instrument")]
			public string Instrument { get; set; }

			[JsonProperty("bpm")]
			public int? Bpm { get; set; }

			[JsonProperty("commands")]
			public List<CommandJsonDto> Commands { get; set; }
		}

		private class CommandJsonDto
		{
			[JsonProperty("type")]
			[JsonConverter(typeof(StringEnumConverter))]
			public CommandType Type { get; set; }

			[JsonProperty("key")]
			[JsonConverter(typeof(StringEnumConverter))]
			public Keys Key { get; set; }

			[JsonProperty("duration")]
			public int Duration { get; set; }
		}

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
			string json = File.ReadAllText(filePath);
			Dictionary<string, object> rawObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (rawObject != null && rawObject.ContainsKey("notes"))
			{
				return DeserializeCompactFormat(json);
			}
			return DeserializeLegacyFormat(json);
		}

		private static Song DeserializeLegacyFormat(string json)
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			SongJsonDto dto = JsonConvert.DeserializeObject<SongJsonDto>(json, JsonSettings);
			Enum.TryParse<InstrumentType>(dto.Instrument, out var instrument);
			Song song = new Song
			{
				Name = dto.Name,
				Artist = dto.Artist,
				Instrument = instrument,
				Bpm = dto.Bpm
			};
			if (dto.Commands != null)
			{
				foreach (CommandJsonDto cmd in dto.Commands)
				{
					song.Commands.Add(new SongCommand
					{
						Type = cmd.Type,
						Key = cmd.Key,
						Duration = cmd.Duration
					});
				}
				return song;
			}
			return song;
		}

		private static Song DeserializeCompactFormat(string json)
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

		public static void SerializeJson(Song song, string filePath)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			SongJsonDto dto = new SongJsonDto
			{
				Name = song.Name,
				Artist = song.Artist,
				Instrument = song.Instrument.ToString(),
				Bpm = song.Bpm,
				Commands = new List<CommandJsonDto>()
			};
			foreach (SongCommand cmd in song.Commands)
			{
				dto.Commands.Add(new CommandJsonDto
				{
					Type = cmd.Type,
					Key = cmd.Key,
					Duration = cmd.Duration
				});
			}
			string json = JsonConvert.SerializeObject((object)dto, JsonSettings);
			File.WriteAllText(filePath, json);
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
