using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Flurl;
using Flurl.Http;
using TmfLib.Content;

namespace BhModule.Community.Pathing.Content
{
	public class WebReader : IDataReader, IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<WebReader>();

		private const string ENTRIES_ENDPOINT = "entries.json";

		private readonly string _baseUrl;

		private HashSet<string> _entries;

		public WebReader(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public async Task InitWebReader()
		{
			_entries = await GetEntries();
		}

		private async Task<HashSet<string>> GetEntries()
		{
			Url entriesUrl = StringExtensions.AppendPathSegment(_baseUrl, (object)"entries.json", false);
			HashSet<string> entrySet = new HashSet<string>();
			try
			{
				string[] array = await GeneratedExtensions.GetJsonAsync<string[]>(entriesUrl, default(CancellationToken), (HttpCompletionOption)0);
				foreach (string entry in array)
				{
					entrySet.Add(entry.Replace('\\', '/'));
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Failed to load {entriesUrl}.");
				return new HashSet<string>(0);
			}
			return entrySet;
		}

		public void Dispose()
		{
		}

		public IDataReader GetSubPath(string subPath)
		{
			return new WebReader(Url.Combine(new string[2] { _baseUrl, subPath }));
		}

		public string GetPathRepresentation(string relativeFilePath = null)
		{
			return Url.Combine(new string[2]
			{
				_baseUrl,
				relativeFilePath ?? string.Empty
			});
		}

		private void ThrowIfNoInit()
		{
			if (_entries == null)
			{
				throw new InvalidOperationException("InitWebReader() must be called before any calls can be made to a WebReader.");
			}
		}

		private string GetCaseSensitiveEntryUri(string filePath)
		{
			ThrowIfNoInit();
			foreach (string entry in _entries)
			{
				if (string.Equals(filePath, entry, StringComparison.InvariantCultureIgnoreCase))
				{
					return entry;
				}
			}
			return filePath;
		}

		public async Task LoadOnFileTypeAsync(Func<Stream, IDataReader, Task> loadFileFunc, string fileExtension = "", IProgress<string> progress = null)
		{
			ThrowIfNoInit();
			string[] validEntries = _entries.Where((string e) => e.EndsWith(fileExtension.ToLowerInvariant())).ToArray();
			string[] array = validEntries;
			foreach (string entry in array)
			{
				progress?.Report("Loading " + entry + "...");
				await loadFileFunc(await GetFileStreamAsync(entry), this);
			}
		}

		public bool FileExists(string filePath)
		{
			ThrowIfNoInit();
			return true;
		}

		public Stream GetFileStream(string filePath)
		{
			throw new InvalidOperationException();
		}

		public byte[] GetFileBytes(string filePath)
		{
			throw new InvalidOperationException();
		}

		private async Task<Stream> GetFileStreamAsyncWithRetry(string filePath, int attempts = 3)
		{
			try
			{
				return await GeneratedExtensions.GetStreamAsync(Url.Combine(new string[2]
				{
					_baseUrl,
					GetCaseSensitiveEntryUri(filePath)
				}), default(CancellationToken), (HttpCompletionOption)0);
			}
			catch (Exception ex)
			{
				if (attempts > 0)
				{
					Logger.Debug(ex, $"Failed to load {filePath}.  Retrying with {attempts} attempts left.");
					WebReader webReader = this;
					int num = attempts - 1;
					attempts = num;
					return await webReader.GetFileStreamAsyncWithRetry(filePath, num);
				}
				Logger.Warn(ex, "Failed to load " + filePath);
			}
			return Stream.Null;
		}

		public async Task<Stream> GetFileStreamAsync(string filePath)
		{
			return await GetFileStreamAsyncWithRetry(filePath);
		}

		public async Task<byte[]> GetFileBytesAsync(string filePath)
		{
			Stream stream = await GetFileStreamAsync(GetCaseSensitiveEntryUri(filePath));
			using MemoryStream memStream = new MemoryStream();
			await stream.CopyToAsync(memStream);
			return memStream.ToArray();
		}

		public void AttemptReleaseLocks()
		{
		}
	}
}
