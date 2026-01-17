using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NpcFinder.Util
{
	internal sealed class SimpleDiskCache
	{
		private readonly string _rootDir;

		public SimpleDiskCache(string rootDir)
		{
			_rootDir = rootDir ?? throw new ArgumentNullException("rootDir");
			Directory.CreateDirectory(_rootDir);
		}

		private static string SafeFileName(string key)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char c in invalidFileNameChars)
			{
				key = key.Replace(c, '_');
			}
			return key;
		}

		private string PathFor(string prefix, string key)
		{
			return Path.Combine(_rootDir, prefix + "_" + SafeFileName(key) + ".json");
		}

		public async Task<T> TryGetAsync<T>(string prefix, string key, TimeSpan maxAge, CancellationToken ct) where T : class
		{
			try
			{
				string path = PathFor(prefix, key);
				if (!File.Exists(path))
				{
					return null;
				}
				if (DateTime.UtcNow - File.GetLastWriteTimeUtc(path) > maxAge)
				{
					return null;
				}
				using FileStream fs = File.OpenRead(path);
				return await JsonSerializer.DeserializeAsync<T>((Stream)fs, (JsonSerializerOptions)null, ct).ConfigureAwait(false);
			}
			catch
			{
				return null;
			}
		}

		public async Task PutAsync<T>(string prefix, string key, T value, CancellationToken ct)
		{
			try
			{
				string path = PathFor(prefix, key);
				string tmp = path + ".tmp";
				using (FileStream fs = File.Create(tmp))
				{
					await JsonSerializer.SerializeAsync<T>((Stream)fs, value, (JsonSerializerOptions)null, ct).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				File.Move(tmp, path);
			}
			catch
			{
			}
		}
	}
}
