using System.IO;
using System.Text.Json;

namespace NpcFinder.Services
{
	public class CacheStore
	{
		private readonly string _root;

		public CacheStore(string rootDirectory)
		{
			if (string.IsNullOrWhiteSpace(rootDirectory))
			{
				rootDirectory = Path.Combine(Path.GetTempPath(), "NpcFinderCache");
			}
			_root = rootDirectory;
			Directory.CreateDirectory(_root);
		}

		private string PathFor(string key)
		{
			string safe = key.Replace(":", "_").Replace("/", "_").Replace("\\", "_");
			return Path.Combine(_root, safe + ".json");
		}

		public bool TryLoad<T>(string key, out T value)
		{
			value = default(T);
			try
			{
				string p = PathFor(key);
				if (!File.Exists(p))
				{
					return false;
				}
				string json = File.ReadAllText(p);
				value = JsonSerializer.Deserialize<T>(json, (JsonSerializerOptions)null);
				return value != null;
			}
			catch
			{
				return false;
			}
		}

		public void Save<T>(string key, T value)
		{
			try
			{
				string path = PathFor(key);
				string json = JsonSerializer.Serialize<T>(value, (JsonSerializerOptions)null);
				File.WriteAllText(path, json);
			}
			catch
			{
			}
		}
	}
}
