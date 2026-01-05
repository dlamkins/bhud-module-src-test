using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Maestro.Models;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services
{
	public static class SongParser
	{
		private static readonly Dictionary<string, Keys> NumpadKeyMap = new Dictionary<string, Keys>
		{
			{
				"Numpad0",
				(Keys)96
			},
			{
				"Numpad1",
				(Keys)97
			},
			{
				"Numpad2",
				(Keys)98
			},
			{
				"Numpad3",
				(Keys)99
			},
			{
				"Numpad4",
				(Keys)100
			},
			{
				"Numpad5",
				(Keys)101
			},
			{
				"Numpad6",
				(Keys)102
			},
			{
				"Numpad7",
				(Keys)103
			},
			{
				"Numpad8",
				(Keys)104
			},
			{
				"Numpad9",
				(Keys)105
			}
		};

		public static Song ParseAhkFile(string filePath, InstrumentType instrument)
		{
			return ParseAhkContent(File.ReadAllText(filePath), Path.GetFileNameWithoutExtension(filePath), instrument);
		}

		public static Song ParseAhkContent(string content, string fileName, InstrumentType instrument)
		{
			Song song = new Song
			{
				Instrument = instrument
			};
			ParseMetadata(song, fileName);
			ParseCommands(content, song);
			return song;
		}

		private static void ParseMetadata(Song song, string fileName)
		{
			string[] parts = fileName.Split(new string[1] { " - " }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length >= 3 && int.TryParse(parts[parts.Length - 1].Trim(), out var bpm))
			{
				song.Bpm = bpm;
				song.Name = parts[0].Trim();
				song.Artist = string.Join(" - ", parts.Skip(1).Take(parts.Length - 2)).Trim();
			}
			else if (parts.Length >= 2)
			{
				song.Name = parts[0].Trim();
				song.Artist = string.Join(" - ", parts.Skip(1)).Trim();
			}
			else
			{
				song.Name = fileName;
				song.Artist = "Unknown";
			}
		}

		private static void ParseCommands(string content, Song song)
		{
			string[] array = content.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string line = array[i].Trim();
				if (string.IsNullOrEmpty(line) || line.StartsWith(";") || line.Contains("PlaySong()") || line == "{" || line == "}")
				{
					continue;
				}
				if (line.StartsWith("SendInput"))
				{
					ParseSendInput(line, song);
				}
				else if (line.StartsWith("Sleep,"))
				{
					Match match = Regex.Match(line, "Sleep,\\s*(\\d+)");
					if (match.Success)
					{
						int duration = int.Parse(match.Groups[1].Value);
						song.Commands.Add(SongCommand.WaitCmd(duration));
					}
				}
			}
		}

		private static void ParseSendInput(string line, Song song)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			if (line.Contains("{Alt down}"))
			{
				Match keyMatch = Regex.Match(line, "\\{(Numpad\\d)\\}");
				if (keyMatch.Success && NumpadKeyMap.TryGetValue(keyMatch.Groups[1].Value, out var altKey))
				{
					song.Commands.Add(SongCommand.KeyDownCmd((Keys)164));
					song.Commands.Add(SongCommand.KeyDownCmd(altKey));
					song.Commands.Add(SongCommand.KeyUpCmd(altKey));
					song.Commands.Add(SongCommand.KeyUpCmd((Keys)164));
				}
				return;
			}
			Match keyMatch2 = Regex.Match(line, "\\{(Numpad\\d)(?:\\s+(down|up))?\\}");
			if (!keyMatch2.Success)
			{
				return;
			}
			string keyName = keyMatch2.Groups[1].Value;
			if (NumpadKeyMap.TryGetValue(keyName, out var key))
			{
				string modifier = keyMatch2.Groups[2].Value;
				if (string.IsNullOrEmpty(modifier))
				{
					song.Commands.Add(SongCommand.KeyDownCmd(key));
					song.Commands.Add(SongCommand.KeyUpCmd(key));
				}
				else if (modifier == "down")
				{
					song.Commands.Add(SongCommand.KeyDownCmd(key));
				}
				else if (modifier == "up")
				{
					song.Commands.Add(SongCommand.KeyUpCmd(key));
				}
			}
		}
	}
}
