using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace Frtal.LorebookReader
{
	public sealed class LorebookCatalog
	{
		private readonly List<LorebookEntry> _entries = new List<LorebookEntry>();

		private readonly string _filePath;

		private int _capacity;

		private readonly object _lock = new object();

		private bool _saveBlocked;

		public string LoadWarning { get; private set; }

		public IReadOnlyList<LorebookEntry> All
		{
			get
			{
				lock (_lock)
				{
					return _entries.ToList();
				}
			}
		}

		public event Action Changed;

		public LorebookCatalog(string directory, int capacity)
		{
			_capacity = Math.Max(1, capacity);
			_filePath = Path.Combine(directory, "catalog.json");
			Load();
		}

		public void SetCapacity(int capacity)
		{
			lock (_lock)
			{
				_capacity = Math.Max(1, capacity);
				TrimToCapacity();
				Save();
			}
			this.Changed?.Invoke();
		}

		public LorebookEntry AddCaptured(string title, string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			LorebookEntry entry;
			lock (_lock)
			{
				LorebookEntry existing = _entries.FirstOrDefault((LorebookEntry e) => e.Text == text);
				if (existing != null)
				{
					existing.TimestampUtc = DateTime.UtcNow.ToString("o");
					Save();
					entry = existing;
				}
				else
				{
					entry = new LorebookEntry
					{
						Title = (string.IsNullOrWhiteSpace(title) ? MakeFallbackTitle(text) : title),
						Text = text,
						TimestampUtc = DateTime.UtcNow.ToString("o"),
						ColorTag = "None"
					};
					_entries.Insert(0, entry);
					TrimToCapacity();
					Save();
				}
			}
			this.Changed?.Invoke();
			return entry;
		}

		public void Update(LorebookEntry entry)
		{
			lock (_lock)
			{
				int idx = _entries.FindIndex((LorebookEntry e) => e.Id == entry.Id);
				if (idx >= 0)
				{
					_entries[idx] = entry;
				}
				Save();
			}
			this.Changed?.Invoke();
		}

		public LorebookEntry AppendToLatest(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			LorebookEntry latest;
			lock (_lock)
			{
				latest = _entries.OrderByDescending((LorebookEntry e) => e.TimestampUtc).FirstOrDefault();
				if (latest == null)
				{
					return null;
				}
				if (!string.IsNullOrEmpty(latest.Text) && latest.Text.TrimEnd().EndsWith(text.TrimEnd(), StringComparison.Ordinal))
				{
					return latest;
				}
				latest.Text = (string.IsNullOrEmpty(latest.Text) ? text : (latest.Text.TrimEnd() + "\n\n" + text.TrimStart()));
				latest.TimestampUtc = DateTime.UtcNow.ToString("o");
				latest.TranslatedText = null;
				latest.TranslatedLang = null;
				Save();
			}
			this.Changed?.Invoke();
			return latest;
		}

		public void Remove(string id)
		{
			lock (_lock)
			{
				_entries.RemoveAll((LorebookEntry e) => e.Id == id);
				Save();
			}
			this.Changed?.Invoke();
		}

		public void Clear()
		{
			lock (_lock)
			{
				_entries.Clear();
				Save();
			}
			this.Changed?.Invoke();
		}

		public List<LorebookEntry> Query(string search, SortMode sort, string colorFilter = null, string expansionFilter = null)
		{
			lock (_lock)
			{
				IEnumerable<LorebookEntry> q = _entries;
				if (!string.IsNullOrWhiteSpace(search))
				{
					string s = search.Trim();
					q = q.Where((LorebookEntry e) => Contains(e.DisplayTitle, s) || Contains(e.Text, s) || Contains(e.Expansion, s) || Contains(e.Theme, s) || Contains(e.Location, s) || Contains(e.Notes, s));
				}
				if (!string.IsNullOrWhiteSpace(colorFilter) && colorFilter != "All")
				{
					q = q.Where((LorebookEntry e) => string.Equals(e.ColorTag, colorFilter, StringComparison.OrdinalIgnoreCase));
				}
				if (!string.IsNullOrWhiteSpace(expansionFilter) && expansionFilter != "All")
				{
					q = q.Where((LorebookEntry e) => string.Equals(e.Expansion, expansionFilter, StringComparison.OrdinalIgnoreCase));
				}
				return (sort switch
				{
					SortMode.OldestFirst => q.OrderBy((LorebookEntry e) => e.TimestampUtc), 
					SortMode.TitleAZ => q.OrderBy((LorebookEntry e) => e.DisplayTitle, StringComparer.OrdinalIgnoreCase), 
					SortMode.TitleZA => q.OrderByDescending((LorebookEntry e) => e.DisplayTitle, StringComparer.OrdinalIgnoreCase), 
					SortMode.ColorTag => from e in q
						orderby e.ColorTag ?? "", e.TimestampUtc descending
						select e, 
					_ => q.OrderByDescending((LorebookEntry e) => e.TimestampUtc), 
				}).ToList();
			}
		}

		public List<string> DistinctExpansions()
		{
			lock (_lock)
			{
				return (from x in (from e in _entries
						select e.Expansion into x
						where !string.IsNullOrWhiteSpace(x)
						select x).Distinct(StringComparer.OrdinalIgnoreCase)
					orderby x
					select x).ToList();
			}
		}

		public void ExportToFile(string path)
		{
			lock (_lock)
			{
				string json = new JavaScriptSerializer
				{
					MaxJsonLength = 67108864
				}.Serialize(_entries);
				File.WriteAllText(path, json);
			}
		}

		public int ImportFromFile(string path, bool merge = true)
		{
			string json = File.ReadAllText(path);
			List<LorebookEntry> incoming = new JavaScriptSerializer
			{
				MaxJsonLength = 67108864
			}.Deserialize<List<LorebookEntry>>(json);
			if (incoming == null)
			{
				return 0;
			}
			int count = 0;
			lock (_lock)
			{
				if (!merge)
				{
					_entries.Clear();
				}
				foreach (LorebookEntry e in incoming)
				{
					if (string.IsNullOrWhiteSpace(e.Id))
					{
						e.Id = Guid.NewGuid().ToString("N");
					}
					int idx = _entries.FindIndex((LorebookEntry x) => x.Id == e.Id);
					if (idx >= 0)
					{
						_entries[idx] = e;
					}
					else
					{
						_entries.Add(e);
					}
					count++;
				}
				_entries.Sort((LorebookEntry a, LorebookEntry b) => string.CompareOrdinal(b.TimestampUtc ?? "", a.TimestampUtc ?? ""));
				Save();
			}
			this.Changed?.Invoke();
			return count;
		}

		public static string MakeFallbackTitle(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "(untitled)";
			}
			string[] words = text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string title = string.Join(" ", words.Take(6));
			if (words.Length <= 6)
			{
				return title;
			}
			return title + "…";
		}

		private static bool Contains(string haystack, string needle)
		{
			if (!string.IsNullOrEmpty(haystack))
			{
				return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
			}
			return false;
		}

		private static bool HasUserMetadata(LorebookEntry e)
		{
			if ((string.IsNullOrWhiteSpace(e.ColorTag) || string.Equals(e.ColorTag, "None", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrWhiteSpace(e.Notes) && string.IsNullOrWhiteSpace(e.Theme) && string.IsNullOrWhiteSpace(e.Expansion) && string.IsNullOrWhiteSpace(e.Location))
			{
				return !string.IsNullOrWhiteSpace(e.IconKey);
			}
			return true;
		}

		private void TrimToCapacity()
		{
			int i = _entries.Count - 1;
			while (i >= 0 && _entries.Count > _capacity)
			{
				if (!HasUserMetadata(_entries[i]))
				{
					_entries.RemoveAt(i);
				}
				i--;
			}
		}

		private static bool TryLoadFile(string path, out List<LorebookEntry> entries)
		{
			entries = null;
			try
			{
				List<LorebookEntry> loaded = new JavaScriptSerializer
				{
					MaxJsonLength = 67108864
				}.Deserialize<List<LorebookEntry>>(File.ReadAllText(path));
				if (loaded == null)
				{
					return false;
				}
				entries = loaded;
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void AdoptEntries(List<LorebookEntry> loaded)
		{
			_entries.Clear();
			_entries.AddRange(loaded);
			foreach (LorebookEntry e in _entries)
			{
				if (string.IsNullOrWhiteSpace(e.Id))
				{
					e.Id = Guid.NewGuid().ToString("N");
				}
			}
		}

		private void Load()
		{
			if (!File.Exists(_filePath))
			{
				return;
			}
			if (TryLoadFile(_filePath, out var loaded))
			{
				AdoptEntries(loaded);
				return;
			}
			string quarantine = Path.Combine(Path.GetDirectoryName(_filePath) ?? "", "catalog.corrupt-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json");
			try
			{
				File.Move(_filePath, quarantine);
			}
			catch
			{
				_saveBlocked = true;
				LoadWarning = "catalog.json is corrupt and could not be quarantined — saving is disabled to protect it. Check the lorebook_reader folder manually.";
				return;
			}
			string bak = _filePath + ".bak";
			if (File.Exists(bak) && TryLoadFile(bak, out var fromBak))
			{
				AdoptEntries(fromBak);
				LoadWarning = "catalog.json was corrupt — restored from backup. Corrupt file kept as " + Path.GetFileName(quarantine) + ".";
				Save();
			}
			else
			{
				LoadWarning = "catalog.json was corrupt and no usable backup was found — starting empty. Corrupt file kept as " + Path.GetFileName(quarantine) + ".";
			}
		}

		private void Save()
		{
			if (_saveBlocked)
			{
				return;
			}
			try
			{
				string json = new JavaScriptSerializer
				{
					MaxJsonLength = 67108864
				}.Serialize(_entries);
				string tmp = _filePath + ".tmp";
				File.WriteAllText(tmp, json);
				if (File.Exists(_filePath))
				{
					File.Replace(tmp, _filePath, _filePath + ".bak");
				}
				else
				{
					File.Move(tmp, _filePath);
				}
			}
			catch
			{
			}
		}
	}
}
