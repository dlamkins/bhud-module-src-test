using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class IconIndexService : IDisposable
	{
		private sealed class ReadStream : Stream
		{
			private readonly Stream _inner;

			private readonly long _maxBytes;

			private long _bytesRead;

			public override bool CanRead => true;

			public override bool CanSeek => false;

			public override bool CanWrite => false;

			public override long Length
			{
				get
				{
					throw new NotSupportedException();
				}
			}

			public override long Position
			{
				get
				{
					throw new NotSupportedException();
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			public ReadStream(Stream inner, long maxBytes)
			{
				_inner = inner;
				_maxBytes = maxBytes;
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				int read = _inner.Read(buffer, offset, count);
				if (read > 0)
				{
					_bytesRead += read;
					if (_bytesRead > _maxBytes)
					{
						throw new InvalidDataException("GW2 icon index gzip expands beyond the allowed size.");
					}
				}
				return read;
			}

			public override void Flush()
			{
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotSupportedException();
			}

			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<IconIndexService>();

		private static readonly char[] SearchWordSeparators = new char[12]
		{
			' ', '\t', '\r', '\n', ',', ';', ':', '/', '\\', '|',
			'-', '_'
		};

		private const int MaxDecompressedIndexBytes = 26214400;

		private const int DownloadBufferSize = 81920;

		private const int MaxSearchResults = 50;

		private readonly ContentsManager _contentsManager;

		private volatile bool _isDisposed;

		public IconIndexService(ContentsManager contentsManager)
		{
			_contentsManager = contentsManager;
		}

		public Task<IReadOnlyList<Gw2IconSearchResult>> SearchAsync(string query, int limit, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run(() => SearchCore(query, limit, cancellationToken), cancellationToken);
		}

		private IReadOnlyList<Gw2IconSearchResult> SearchCore(string query, int limit, CancellationToken cancellationToken)
		{
			if (_isDisposed)
			{
				return Array.Empty<Gw2IconSearchResult>();
			}
			limit = Math.Min(limit, 50);
			string[] terms = GetSearchTerms(query);
			if (terms.Length == 0 || limit <= 0)
			{
				return Array.Empty<Gw2IconSearchResult>();
			}
			List<Gw2IconSearchResult> results = new List<Gw2IconSearchResult>(limit);
			try
			{
				StreamSearch(terms, limit, results, cancellationToken);
				return results;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to search bundled GW2 icon index.");
				return results;
			}
		}

		private static bool MoveToEntriesArray(JsonReader reader)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			if (!reader.Read() || (int)reader.get_TokenType() != 1)
			{
				return false;
			}
			while (reader.Read())
			{
				if ((int)reader.get_TokenType() == 4)
				{
					string propertyName = (string)reader.get_Value();
					if (!reader.Read())
					{
						return false;
					}
					if (string.Equals(propertyName, "entries", StringComparison.Ordinal))
					{
						return (int)reader.get_TokenType() == 2;
					}
					reader.Skip();
				}
			}
			return false;
		}

		private void StreamSearch(string[] terms, int limit, List<Gw2IconSearchResult> results, CancellationToken cancellationToken)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Invalid comparison between Unknown and I4
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Invalid comparison between Unknown and I4
			List<Gw2IconSearchResult> nameResults = new List<Gw2IconSearchResult>(limit);
			List<Gw2IconSearchResult> fallbackResults = new List<Gw2IconSearchResult>(limit * 4);
			HashSet<int> nameAssetIds = new HashSet<int>();
			HashSet<int> fallbackAssetIds = new HashSet<int>();
			using (Stream compressed = _contentsManager.GetFileStream("icon_index.json.gz"))
			{
				if (compressed == null)
				{
					return;
				}
				using GZipStream gzip = new GZipStream(compressed, CompressionMode.Decompress);
				using ReadStream limited = new ReadStream(gzip, 26214400L);
				using StreamReader text = new StreamReader(limited, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, 81920);
				JsonTextReader reader = new JsonTextReader((TextReader)text);
				try
				{
					if (!MoveToEntriesArray((JsonReader)(object)reader))
					{
						return;
					}
					JsonSerializer serializer = JsonSerializer.CreateDefault();
					while (((JsonReader)reader).Read())
					{
						cancellationToken.ThrowIfCancellationRequested();
						if ((int)((JsonReader)reader).get_TokenType() == 14)
						{
							break;
						}
						if ((int)((JsonReader)reader).get_TokenType() != 1)
						{
							((JsonReader)reader).Skip();
							continue;
						}
						Gw2IconIndexEntry entry = serializer.Deserialize<Gw2IconIndexEntry>((JsonReader)(object)reader);
						if (entry == null || entry.AssetId <= 0)
						{
							continue;
						}
						string matchingName = FindMatchingName(entry, terms);
						if (matchingName != null)
						{
							if (nameAssetIds.Add(entry.AssetId))
							{
								nameResults.Add(ToResult(entry, matchingName));
								if (nameResults.Count >= limit)
								{
									break;
								}
							}
						}
						else if (fallbackResults.Count < limit * 4 && EntryContainsAllTerms(entry, terms) && fallbackAssetIds.Add(entry.AssetId))
						{
							fallbackResults.Add(ToResult(entry, entry.Name));
						}
					}
				}
				finally
				{
					((IDisposable)reader)?.Dispose();
				}
			}
			results.AddRange(nameResults);
			foreach (Gw2IconSearchResult result in fallbackResults)
			{
				if (results.Count >= limit)
				{
					break;
				}
				if (!nameAssetIds.Contains(result.AssetId))
				{
					results.Add(result);
				}
			}
		}

		private static Gw2IconSearchResult ToResult(Gw2IconIndexEntry entry, string matchingName)
		{
			return new Gw2IconSearchResult
			{
				AssetId = entry.AssetId,
				Name = (matchingName ?? entry.Name ?? string.Empty)
			};
		}

		private static string FindMatchingName(Gw2IconIndexEntry entry, IReadOnlyList<string> terms)
		{
			if (ContainsAllTerms(entry.Name, terms))
			{
				return entry.Name;
			}
			if (entry.Aliases == null)
			{
				return null;
			}
			foreach (string alias in entry.Aliases)
			{
				if (ContainsAllTerms(alias, terms))
				{
					return alias;
				}
			}
			return null;
		}

		private static bool EntryContainsAllTerms(Gw2IconIndexEntry entry, IReadOnlyList<string> terms)
		{
			foreach (string term in terms)
			{
				if (!EntryContainsTerm(entry, term))
				{
					return false;
				}
			}
			return true;
		}

		private static bool EntryContainsTerm(Gw2IconIndexEntry entry, string term)
		{
			if (ContainsTerm(entry.Name, term) || ContainsTerm(entry.Description, term) || ContainsTerm(entry.Source, term))
			{
				return true;
			}
			if (entry.Aliases != null && entry.Aliases.Any((string alias) => ContainsTerm(alias, term)))
			{
				return true;
			}
			if (entry.Keywords != null)
			{
				return entry.Keywords.Any((string keyword) => ContainsTerm(keyword, term));
			}
			return false;
		}

		private static bool ContainsAllTerms(string value, IReadOnlyList<string> terms)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			return terms.All((string term) => ContainsTerm(value, term));
		}

		private static bool ContainsTerm(string value, string term)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
			}
			return false;
		}

		private static string[] GetSearchTerms(string query)
		{
			return (query ?? string.Empty).Trim().ToLowerInvariant().Split(SearchWordSeparators, StringSplitOptions.RemoveEmptyEntries)
				.Take(6)
				.ToArray();
		}

		public void Dispose()
		{
			_isDisposed = true;
		}
	}
}
