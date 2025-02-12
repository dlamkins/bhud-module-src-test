using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SL.ChatLinks.StaticFiles
{
	public sealed class StaticDataClient
	{
		[CompilerGenerated]
		private HttpClient _003ChttpClient_003EP;

		public StaticDataClient(HttpClient httpClient)
		{
			_003ChttpClient_003EP = httpClient;
			base._002Ector();
		}

		public async Task<SeedIndex> GetSeedIndex(CancellationToken cancellationToken)
		{
			using HttpResponseMessage response = await _003ChttpClient_003EP.GetAsync("seed-index.json");
			using Stream content = await response.Content.ReadAsStreamAsync();
			response.EnsureSuccessStatusCode();
			return (await JsonSerializer.DeserializeAsync<SeedIndex>(content, (JsonSerializerOptions?)null, cancellationToken)) ?? throw new InvalidOperationException("Couldn't retrieve seed index.");
		}

		public async Task Download(SeedDatabase database, string destination, CancellationToken cancellationToken)
		{
			using HttpResponseMessage response = await _003ChttpClient_003EP.GetAsync(database.Url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			using Stream content = await response.Content.ReadAsStreamAsync();
			response.EnsureSuccessStatusCode();
			string tmp = Path.GetTempFileName();
			using (FileStream destination2 = File.OpenWrite(tmp))
			{
				await content.CopyToAsync(destination2, 8192, cancellationToken);
			}
			using (SHA256 sha256 = SHA256.Create())
			{
				using FileStream fileStream = File.OpenRead(tmp);
				if (BitConverter.ToString(sha256.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant() != database.SHA256.ToLowerInvariant())
				{
					File.Delete(tmp);
					throw new InvalidOperationException("SHA256 hash mismatch.");
				}
			}
			File.Delete(destination);
			DecompressGzipFile(tmp, destination);
			File.Delete(tmp);
		}

		private void DecompressGzipFile(string sourceFile, string destinationFile)
		{
			using FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
			using GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
			using FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
			decompressionStream.CopyTo(destinationStream);
		}
	}
}
