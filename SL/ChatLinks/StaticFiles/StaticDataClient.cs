using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SL.Common;

namespace SL.ChatLinks.StaticFiles
{
	public sealed class StaticDataClient
	{
		[CompilerGenerated]
		private HttpClient _003ChttpClient_003EP;

		private static readonly Uri SeedIndex = new Uri("seed-index.json", UriKind.Relative);

		public StaticDataClient(HttpClient httpClient)
		{
			_003ChttpClient_003EP = httpClient;
			base._002Ector();
		}

		public async Task<SeedIndex> GetSeedIndex(CancellationToken cancellationToken)
		{
			using HttpResponseMessage response = await _003ChttpClient_003EP.GetAsync(SeedIndex, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			using Stream content = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			response.EnsureSuccessStatusCode();
			return (await JsonSerializer.DeserializeAsync<SeedIndex>(content, (JsonSerializerOptions?)null, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)) ?? throw new InvalidOperationException("Couldn't retrieve seed index.");
		}

		public async Task Download(SeedDatabase database, string destination, CancellationToken cancellationToken)
		{
			ThrowHelper.ThrowIfNull(database, "database");
			using HttpResponseMessage response = await _003ChttpClient_003EP.GetAsync(database.Url, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			using Stream content = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			response.EnsureSuccessStatusCode();
			string tmp = Path.GetTempFileName();
			using (FileStream destination2 = File.OpenWrite(tmp))
			{
				await content.CopyToAsync(destination2, 8192, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			using (SHA256 sha256 = SHA256.Create())
			{
				using FileStream fileStream = File.OpenRead(tmp);
				if (!BitConverter.ToString(sha256.ComputeHash(fileStream)).Replace("-", "").Equals(database.SHA256, StringComparison.OrdinalIgnoreCase))
				{
					File.Delete(tmp);
					throw new InvalidOperationException("SHA256 hash mismatch.");
				}
			}
			File.Delete(destination);
			DecompressGzipFile(tmp, destination);
			File.Delete(tmp);
		}

		private static void DecompressGzipFile(string sourceFile, string destinationFile)
		{
			using FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
			using GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
			using FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
			decompressionStream.CopyTo(destinationStream);
		}
	}
}
