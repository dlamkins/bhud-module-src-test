using System;
using System.Collections.Generic;
using System.IO;
using Maestro.Models;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Maestro.Services
{
	public static class SongSerializer
	{
		[MessagePackObject(false)]
		public class SongBundleDto
		{
			[Key(0)]
			public string Name { get; set; }

			[Key(1)]
			public string Artist { get; set; }

			[Key(2)]
			public int Instrument { get; set; }

			[Key(3)]
			public int Bpm { get; set; }

			[Key(4)]
			public List<CommandBundleDto> Commands { get; set; }
		}

		[MessagePackObject(false)]
		public class CommandBundleDto
		{
			[Key(0)]
			public int Type { get; set; }

			[Key(1)]
			public int Key { get; set; }

			[Key(2)]
			public int Duration { get; set; }
		}

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

		private static readonly JsonSerializerSettings JsonSettings;

		public static List<Song> DeserializeBundle(Stream stream)
		{
			MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
			object[][] array = MessagePackSerializer.Deserialize<object[][]>(stream, options);
			List<Song> songs = new List<Song>();
			object[][] array2 = array;
			foreach (object[] songData in array2)
			{
				Song song = new Song
				{
					Name = (songData[0]?.ToString() ?? ""),
					Artist = (songData[1]?.ToString() ?? ""),
					Instrument = (InstrumentType)Convert.ToInt32(songData[2]),
					Bpm = Convert.ToInt32(songData[3])
				};
				object[] commandsData = songData[4] as object[];
				if (commandsData != null)
				{
					object[] array3 = commandsData;
					for (int j = 0; j < array3.Length; j++)
					{
						object[] cmdArray = array3[j] as object[];
						if (cmdArray != null && cmdArray.Length >= 3)
						{
							song.Commands.Add(new SongCommand
							{
								Type = (CommandType)Convert.ToInt32(cmdArray[0]),
								Key = (Keys)Convert.ToInt32(cmdArray[1]),
								Duration = Convert.ToInt32(cmdArray[2])
							});
						}
					}
				}
				songs.Add(song);
			}
			return songs;
		}

		public static void SerializeBundle(List<Song> songs, string filePath)
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected I4, but got Unknown
			List<SongBundleDto> dtos = new List<SongBundleDto>();
			foreach (Song song in songs)
			{
				SongBundleDto dto = new SongBundleDto
				{
					Name = song.Name,
					Artist = song.Artist,
					Instrument = (int)song.Instrument,
					Bpm = song.Bpm.GetValueOrDefault(),
					Commands = new List<CommandBundleDto>()
				};
				foreach (SongCommand cmd in song.Commands)
				{
					dto.Commands.Add(new CommandBundleDto
					{
						Type = (int)cmd.Type,
						Key = (int)cmd.Key,
						Duration = cmd.Duration
					});
				}
				dtos.Add(dto);
			}
			byte[] bytes = MessagePackSerializer.Serialize(dtos);
			File.WriteAllBytes(filePath, bytes);
		}

		public static Song DeserializeJson(string filePath)
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			SongJsonDto dto = JsonConvert.DeserializeObject<SongJsonDto>(File.ReadAllText(filePath), JsonSettings);
			Enum.TryParse<InstrumentType>(dto.Instrument, out var instrument);
			Song song = new Song
			{
				Name = dto.Name,
				Artist = dto.Artist,
				Instrument = instrument,
				Bpm = dto.Bpm
			};
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
