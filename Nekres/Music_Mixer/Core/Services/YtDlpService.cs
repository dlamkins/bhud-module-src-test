using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gapotchenko.FX.Diagnostics;
using Nekres.Music_Mixer.Core.Services.YtDlp;

namespace Nekres.Music_Mixer.Core.Services
{
	public class YtDlpService
	{
		public enum AudioFormat
		{
			Best,
			MP3,
			AAC,
			WMA,
			FLAC
		}

		public enum AudioBitrate
		{
			B64,
			B96,
			B128,
			B160,
			B192,
			B256,
			B320
		}

		private readonly Regex _mediaId = new Regex("^([a-zA-Z0-9_-]+)$", RegexOptions.Compiled);

		private readonly Regex _youtubeVideoId = new Regex("youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)(?<id>[a-zA-Z0-9-_]+).*", RegexOptions.Compiled);

		private readonly Regex _progressReport = new Regex("^\\[download\\].*?(?<percentage>(.*?))% of (?<size>(.*?))MiB at (?<speed>(.*?)) ETA (?<eta>(.*?))$", RegexOptions.Compiled);

		private readonly Regex _upToDate = new Regex("^yt-dlp is up to date \\((?<version>.*?)\\)$", RegexOptions.Compiled);

		private readonly Regex _updating = new Regex("^Updating to (?<version>.*?) \\.\\.\\.$", RegexOptions.Compiled);

		private readonly Regex _updated = new Regex("^Updated yt-dlp to (?<version>.*?)$", RegexOptions.Compiled);

		private readonly Regex _version = new Regex("^Available version: (?<available>.*?), Current version: (?<current>.*?)$", RegexOptions.Compiled);

		private readonly Regex _warning = new Regex("^WARNING:[^\\S\\r\\n](.*?)$", RegexOptions.Compiled);

		private readonly Regex _error = new Regex("^ERROR:[^\\S\\r\\n](.*?)$", RegexOptions.Compiled);

		private string _executablePath;

		private Logger _logger = Logger.GetLogger(typeof(YtDlpService));

		private const string _globalYtDlpArgs = "--ignore-config --no-call-home --no-warnings --no-get-comments --extractor-retries 0 --extractor-args \"youtube:max_comments=0;player_client=web,web_music;skip=configs,webpage\"";

		public YtDlpService()
		{
			ExtractFile("bin/yt-dlp.exe");
		}

		private void ExtractFile(string filePath)
		{
			_executablePath = Path.Combine(MusicMixer.Instance.ModuleDirectory, filePath);
			if (!File.Exists(_executablePath))
			{
				using Stream fs = MusicMixer.Instance.ContentsManager.GetFileStream(filePath);
				fs.Position = 0L;
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, (int)fs.Length);
				Directory.CreateDirectory(Path.GetDirectoryName(_executablePath));
				File.WriteAllBytes(_executablePath, buffer);
			}
		}

		public bool GetYouTubeVideoId(string url, out string id)
		{
			Match match = _youtubeVideoId.Match(url);
			if (match.Success && match.Groups["id"].Success)
			{
				id = match.Groups["id"].Value;
				return true;
			}
			id = string.Empty;
			return false;
		}

		public void RemoveCache()
		{
			Process process = new Process();
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.FileName = _executablePath;
			process.StartInfo.Arguments = "--rm-cache-dir --ignore-config --no-call-home --no-warnings --no-get-comments --extractor-retries 0 --extractor-args \"youtube:max_comments=0;player_client=web,web_music;skip=configs,webpage\"";
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.UseShellExecute = false;
			process.ErrorDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(e.Data))
				{
					_logger.Info(e.Data);
				}
			};
			process.Start();
			process.BeginErrorReadLine();
		}

		public async Task Update(IProgress<string> progressHandler)
		{
			Process process = new Process();
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.FileName = _executablePath;
			process.StartInfo.Arguments = "-U --ignore-config --no-call-home --no-warnings --no-get-comments --extractor-retries 0 --extractor-args \"youtube:max_comments=0;player_client=web,web_music;skip=configs,webpage\"";
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.UseShellExecute = false;
			process.OutputDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(e.Data))
				{
					Match match = _version.Match(e.Data);
					if (match.Success)
					{
						string value = match.Groups["current"].Value;
						string value2 = match.Groups["available"].Value;
						_logger.Info("Current version \"" + value + "\"");
						if (!string.Equals(value2, value))
						{
							_logger.Info("Available version \"" + value2 + "\"");
						}
					}
					Match match2 = _updating.Match(e.Data);
					if (match2.Success)
					{
						string value3 = match2.Groups["version"].Value;
						progressHandler.Report("Updating yt-dlp to \"" + value3 + "\"…");
					}
					Match match3 = _updated.Match(e.Data);
					if (match3.Success)
					{
						string value4 = match3.Groups["version"].Value;
						progressHandler.Report(null);
						_logger.Info("Updated to \"" + value4 + "\"");
					}
				}
			};
			process.ErrorDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(e.Data))
				{
					_logger.Info(e.Data);
				}
			};
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			await ProcessExtensions.WaitForExitAsync(process, default(CancellationToken));
		}

		public void GetThumbnail(string link, Action<string> callback)
		{
			using Process p = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					FileName = _executablePath,
					Arguments = "--get-thumbnail " + link + " --ignore-config --no-call-home --no-warnings --no-get-comments --extractor-retries 0 --extractor-args \"youtube:max_comments=0;player_client=web,web_music;skip=configs,webpage\""
				}
			};
			p.OutputDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				callback(e.Data);
			};
			p.Start();
			p.BeginOutputReadLine();
		}

		public async Task<string> GetAudioOnlyUrl(string link)
		{
			using Process p = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					FileName = _executablePath,
					Arguments = string.Format("-g {0} -f \"bestaudio[ext=m4a][abr<={1}]/bestaudio[ext=aac][abr<={1}]/bestaudio[abr<={1}]/bestaudio\" {2}", link, MusicMixer.Instance.AverageBitrate.get_Value().ToString().Substring(1), "--ignore-config --no-call-home --no-warnings --no-get-comments --extractor-retries 0 --extractor-args \"youtube:max_comments=0;player_client=web,web_music;skip=configs,webpage\"")
				}
			};
			string result = string.Empty;
			p.OutputDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(e.Data) && !e.Data.ToLower().StartsWith("error"))
				{
					result = e.Data;
				}
			};
			p.Start();
			p.BeginOutputReadLine();
			await ProcessExtensions.WaitForExitAsync(p, default(CancellationToken));
			return result;
		}

		public async Task<MetaData> GetMetaData(string link)
		{
			using Process p = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					FileName = _executablePath,
					Arguments = "--print id,webpage_url,title,uploader,duration " + link + " --ignore-config --no-call-home --no-warnings --no-get-comments --extractor-retries 0 --extractor-args \"youtube:max_comments=0;player_client=web,web_music;skip=configs,webpage\""
				}
			};
			string externalId = string.Empty;
			string url = string.Empty;
			string title = string.Empty;
			string uploader = string.Empty;
			TimeSpan duration = TimeSpan.Zero;
			int i = 0;
			p.OutputDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(e.Data))
				{
					switch (i)
					{
					case 0:
						if (!_mediaId.Match(e.Data).Success)
						{
							return;
						}
						externalId = e.Data;
						break;
					case 1:
						url = e.Data;
						break;
					case 2:
						title = e.Data;
						break;
					case 3:
						uploader = e.Data;
						break;
					case 4:
					{
						duration = (int.TryParse(Regex.Replace(e.Data, "[^0-9]", string.Empty), out var result) ? TimeSpan.FromSeconds(result) : TimeSpan.Zero);
						break;
					}
					}
					i++;
				}
			};
			p.ErrorDataReceived += delegate(object _, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(e.Data))
				{
					Match match = _error.Match(e.Data);
					if (match.Success)
					{
						_logger.Info($"{match}");
					}
				}
			};
			p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();
			await ProcessExtensions.WaitForExitAsync(p, default(CancellationToken));
			return new MetaData(externalId, title, url, uploader, duration);
		}
	}
}
