using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using DavidRice.BlishHud.MidiControl.Keymaps.BuiltIn;
using Newtonsoft.Json;

namespace DavidRice.BlishHud.MidiControl.Keymaps
{
	public class KeymapRegistry
	{
		private static readonly Logger Logger = Logger.GetLogger<KeymapRegistry>();

		private readonly List<Keymap> _builtInKeymaps = new List<Keymap>();

		private readonly List<Keymap> _customKeymaps = new List<Keymap>();

		private readonly List<string> _loadErrors = new List<string>();

		public IReadOnlyList<Keymap> AllKeymaps => _builtInKeymaps.Concat<Keymap>(_customKeymaps).ToList().AsReadOnly();

		public int CustomKeymapCount => _customKeymaps.Count;

		public IReadOnlyList<string> LoadErrors => _loadErrors.AsReadOnly();

		public KeymapRegistry()
		{
			RegisterBuiltIn(GeneralKeymap.Instance);
			RegisterBuiltIn(GrandPianoAutoKeymap.Instance);
			RegisterBuiltIn(BassGuitarAutoKeymap.Instance);
			RegisterBuiltIn(FluteCAutoKeymap.Instance);
			RegisterBuiltIn(FluteEAutoKeymap.Instance);
			RegisterBuiltIn(HarpAutoKeymap.Instance);
			RegisterBuiltIn(HornCAutoKeymap.Instance);
			RegisterBuiltIn(HornEAutoKeymap.Instance);
			RegisterBuiltIn(LuteAutoKeymap.Instance);
			RegisterBuiltIn(ChoirBellAutoKeymap.Instance);
			RegisterBuiltIn(MinstrelKeymap.Instance);
			RegisterBuiltIn(MinstrelAutoKeymap.Instance);
			RegisterBuiltIn(VerdarachAutoKeymap.Instance);
		}

		public void Register(Keymap keymap)
		{
			_builtInKeymaps.Add(keymap);
		}

		private void RegisterBuiltIn(Keymap keymap)
		{
			_builtInKeymaps.Add(keymap);
		}

		public Keymap? FindById(string id)
		{
			string id2 = id;
			return _builtInKeymaps.FirstOrDefault((Keymap k) => k.Id == id2) ?? _customKeymaps.FirstOrDefault((Keymap k) => k.Id == id2);
		}

		public Keymap? FindByName(string name)
		{
			string name2 = name;
			return _builtInKeymaps.FirstOrDefault((Keymap k) => k.Name == name2) ?? _customKeymaps.FirstOrDefault((Keymap k) => k.Name == name2);
		}

		public void LoadCustomKeymaps(string directoryPath)
		{
			//IL_0189: Expected O, but got Unknown
			_customKeymaps.Clear();
			_loadErrors.Clear();
			if (!Directory.Exists(directoryPath))
			{
				Logger.Info("Custom keymaps directory not found: " + directoryPath);
				return;
			}
			string[] files;
			try
			{
				files = Directory.GetFiles(directoryPath, "*.json");
			}
			catch (Exception ex3)
			{
				Logger.Warn("Failed to scan custom keymaps directory: " + ex3.Message);
				_loadErrors.Add("Failed to scan directory: " + ex3.Message);
				return;
			}
			int loadedCount = 0;
			string[] array = files;
			foreach (string filePath in array)
			{
				string fileName = Path.GetFileName(filePath);
				try
				{
					Keymap keymap = JsonConvert.DeserializeObject<Keymap>(File.ReadAllText(filePath));
					if (keymap == null)
					{
						_loadErrors.Add(fileName + ": deserialization returned null");
						continue;
					}
					if (string.IsNullOrWhiteSpace(keymap.Id))
					{
						_loadErrors.Add(fileName + ": missing required field 'id'");
						continue;
					}
					if (string.IsNullOrWhiteSpace(keymap.Name))
					{
						_loadErrors.Add(fileName + ": missing required field 'name'");
						continue;
					}
					if (keymap.Notes == null)
					{
						_loadErrors.Add(fileName + ": missing required field 'notes'");
						continue;
					}
					if (FindById(keymap.Id) != null)
					{
						_loadErrors.Add(fileName + ": id '" + keymap.Id + "' conflicts with existing keymap");
						continue;
					}
					_customKeymaps.Add(keymap);
					loadedCount++;
				}
				catch (JsonException val)
				{
					JsonException ex2 = val;
					_loadErrors.Add(fileName + ": JSON parse error — " + ((Exception)(object)ex2).Message);
				}
				catch (Exception ex)
				{
					_loadErrors.Add(fileName + ": read error — " + ex.Message);
				}
			}
			Logger.Info($"Loaded {loadedCount} custom keymap(s) from {directoryPath}.");
			if (_loadErrors.Count <= 0)
			{
				return;
			}
			Logger.Warn($"{_loadErrors.Count} custom keymap file(s) had errors:");
			foreach (string error in _loadErrors)
			{
				Logger.Warn("  " + error);
			}
		}
	}
}
