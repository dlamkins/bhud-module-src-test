using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Frtal.LorebookReader
{
	public sealed class EdgeTtsService : IDisposable
	{
		private const string TrustedToken = "6A5AA1D4EAFF4E9FB37E23D68491D6F4";

		private const string Host = "speech.platform.bing.com";

		private const string BasePath = "/consumer/speech/synthesize/readaloud/edge/v1";

		private const string ChromiumFull = "143.0.3650.75";

		private const string ChromiumMajor = "143";

		public static readonly string[] CuratedVoices;

		private CancellationTokenSource _cts;

		private WaveOutEvent _currentOut;

		private Stopwatch _currentClock;

		public bool IsPaused { get; private set; }

		public static string VoiceForLanguage(string lang)
		{
			string text = (lang ?? "").ToLowerInvariant();
			if (text != null)
			{
				switch (text.Length)
				{
				case 2:
					switch (text[0])
					{
					case 'c':
						if (!(text == "cs"))
						{
							break;
						}
						return "cs-CZ-AntoninNeural";
					case 'd':
						if (!(text == "de"))
						{
							break;
						}
						return "de-DE-ConradNeural";
					case 'e':
						if (!(text == "es"))
						{
							break;
						}
						return "es-ES-AlvaroNeural";
					case 'f':
						if (!(text == "fr"))
						{
							break;
						}
						return "fr-FR-HenriNeural";
					case 'i':
						if (!(text == "it"))
						{
							break;
						}
						return "it-IT-DiegoNeural";
					case 'p':
						if (!(text == "pl"))
						{
							if (!(text == "pt"))
							{
								break;
							}
							return "pt-PT-DuarteNeural";
						}
						return "pl-PL-MarekNeural";
					case 'r':
						if (!(text == "ru"))
						{
							break;
						}
						return "ru-RU-DmitryNeural";
					case 'j':
						if (!(text == "ja"))
						{
							break;
						}
						return "ja-JP-KeitaNeural";
					case 'k':
						if (!(text == "ko"))
						{
							break;
						}
						return "ko-KR-InJoonNeural";
					}
					break;
				case 5:
					if (!(text == "zh-cn"))
					{
						break;
					}
					return "zh-CN-YunxiNeural";
				}
			}
			return null;
		}

		public void Pause()
		{
			try
			{
				_currentOut?.Pause();
			}
			catch
			{
			}
			try
			{
				_currentClock?.Stop();
			}
			catch
			{
			}
			IsPaused = true;
		}

		public void Resume()
		{
			try
			{
				_currentOut?.Play();
			}
			catch
			{
			}
			try
			{
				_currentClock?.Start();
			}
			catch
			{
			}
			IsPaused = false;
		}

		static EdgeTtsService()
		{
			CuratedVoices = new string[13]
			{
				"en-GB-RyanNeural", "en-GB-SoniaNeural", "en-US-AndrewMultilingualNeural", "en-US-AriaNeural", "en-US-GuyNeural", "en-US-JennyNeural", "en-AU-WilliamNeural", "de-DE-ConradNeural", "de-DE-KatjaNeural", "fr-FR-HenriNeural",
				"fr-FR-DeniseNeural", "es-ES-AlvaroNeural", "es-ES-ElviraNeural"
			};
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
		}

		public async Task SpeakAsync(string text, string voice, double rate, Action<string> onChunk = null, Action<int> onWord = null)
		{
			Stop();
			CancellationToken ct = (_cts = new CancellationTokenSource()).Token;
			List<string> chunks = TextCleaner.SplitChunks(text);
			if (chunks.Count == 0)
			{
				return;
			}
			string prosodyRate = RateToProsody(rate);
			try
			{
				Task<(byte[] mp3, List<TimeSpan> words)> nextTask = SynthesizeChunkAsync(chunks[0], voice, prosodyRate, ct);
				for (int i = 0; i < chunks.Count; i++)
				{
					var (mp3, words) = await nextTask.ConfigureAwait(continueOnCapturedContext: false);
					if (ct.IsCancellationRequested)
					{
						break;
					}
					nextTask = (Task<(byte[] mp3, List<TimeSpan> words)>)((i + 1 < chunks.Count) ? ((Task)SynthesizeChunkAsync(chunks[i + 1], voice, prosodyRate, ct)) : ((Task)Task.FromResult<(byte[], List<TimeSpan>)>((null, null))));
					onChunk?.Invoke(chunks[i]);
					await PlayMp3Async(mp3, words, onWord, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (ct.IsCancellationRequested)
					{
						break;
					}
				}
			}
			finally
			{
				onChunk?.Invoke(null);
			}
		}

		public void Stop()
		{
			IsPaused = false;
			try
			{
				_cts?.Cancel();
			}
			catch
			{
			}
			try
			{
				_currentOut?.Stop();
			}
			catch
			{
			}
		}

		public void Dispose()
		{
			try
			{
				Stop();
			}
			catch
			{
			}
			try
			{
				_currentOut?.Dispose();
			}
			catch
			{
			}
		}

		private static async Task<(byte[] mp3, List<TimeSpan> words)> SynthesizeChunkAsync(string text, string voice, string prosodyRate, CancellationToken outerCt)
		{
			using CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15.0));
			using CancellationTokenSource linked = CancellationTokenSource.CreateLinkedTokenSource(outerCt, timeout.Token);
			using WebSocketLite ws = new WebSocketLite();
			CancellationToken ct = linked.Token;
			string pathAndQuery = "/consumer/speech/synthesize/readaloud/edge/v1?TrustedClientToken=6A5AA1D4EAFF4E9FB37E23D68491D6F4" + $"&ConnectionId={Guid.NewGuid():N}" + "&Sec-MS-GEC=" + GenerateSecMsGec() + "&Sec-MS-GEC-Version=1-143.0.3650.75";
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				["Pragma"] = "no-cache",
				["Cache-Control"] = "no-cache",
				["Origin"] = "chrome-extension://jdiccldimpdaibmpdkjnbmckianbfold",
				["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36 Edg/143.0.0.0",
				["Accept-Encoding"] = "gzip, deflate, br, zstd",
				["Accept-Language"] = "en-US,en;q=0.9",
				["Cookie"] = "muid=" + GenerateMuid() + ";"
			};
			await ws.ConnectAsync("speech.platform.bing.com", pathAndQuery, headers, ct).ConfigureAwait(continueOnCapturedContext: false);
			string ts = DateTime.UtcNow.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT+0000 (Coordinated Universal Time)'");
			string config = "X-Timestamp:" + ts + "\r\nContent-Type:application/json; charset=utf-8\r\nPath:speech.config\r\n\r\n{\"context\":{\"synthesis\":{\"audio\":{\"metadataoptions\":{\"sentenceBoundaryEnabled\":\"false\",\"wordBoundaryEnabled\":\"true\"},\"outputFormat\":\"audio-24khz-48kbitrate-mono-mp3\"}}}}\r\n";
			await ws.SendTextAsync(config, ct).ConfigureAwait(continueOnCapturedContext: false);
			string ssml = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='" + voice + "'><prosody pitch='+0Hz' rate='" + prosodyRate + "' volume='+0%'>" + XmlEscape(text) + "</prosody></voice></speak>";
			string ssmlMsg = $"X-RequestId:{Guid.NewGuid():N}\r\n" + "Content-Type:application/ssml+xml\r\nX-Timestamp:" + ts + "Z\r\nPath:ssml\r\n\r\n" + ssml;
			await ws.SendTextAsync(ssmlMsg, ct).ConfigureAwait(continueOnCapturedContext: false);
			MemoryStream audio = new MemoryStream();
			List<TimeSpan> words = new List<TimeSpan>();
			while (true)
			{
				var (type, data) = await ws.ReceiveAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
				switch (type)
				{
				case WebSocketLite.FrameType.Closed:
					throw new IOException("Edge TTS closed the connection.");
				case WebSocketLite.FrameType.Text:
				{
					string msg = Encoding.UTF8.GetString(data);
					if (!msg.Contains("Path:turn.end"))
					{
						if (msg.Contains("Path:audio.metadata"))
						{
							Match mm = Regex.Match(msg, "\"Offset\"\\s*:\\s*(\\d+)");
							if (mm.Success && long.TryParse(mm.Groups[1].Value, out var off))
							{
								words.Add(TimeSpan.FromTicks(off));
							}
						}
						continue;
					}
					if (audio.Length == 0L)
					{
						throw new IOException("Edge TTS returned no audio.");
					}
					return (audio.ToArray(), words);
				}
				}
				if (data.Length >= 2)
				{
					int headerLen = (data[0] << 8) | data[1];
					int offset = 2 + headerLen;
					if (data.Length > offset)
					{
						audio.Write(data, offset, data.Length - offset);
					}
				}
			}
		}

		private async Task PlayMp3Async(byte[] mp3, List<TimeSpan> words, Action<int> onWord, CancellationToken ct)
		{
			if (mp3 == null || mp3.Length == 0)
			{
				return;
			}
			using MemoryStream ms = new MemoryStream(mp3);
			using Mp3FileReader reader = new Mp3FileReader(ms);
			WaveOutEvent output = new WaveOutEvent();
			try
			{
				_currentOut = output;
				TaskCompletionSource<bool> done = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
				output.PlaybackStopped += delegate
				{
					done.TrySetResult(result: true);
				};
				output.Init(reader);
				output.Play();
				Stopwatch sw = (_currentClock = Stopwatch.StartNew());
				if (IsPaused)
				{
					output.Pause();
					sw.Stop();
				}
				int wi = 0;
				using (ct.Register(delegate
				{
					try
					{
						output.Stop();
					}
					catch
					{
					}
					done.TrySetResult(result: true);
				}))
				{
					while (!done.Task.IsCompleted)
					{
						if (words != null && onWord != null)
						{
							for (; wi < words.Count && sw.Elapsed >= words[wi]; wi++)
							{
								try
								{
									onWord(wi);
								}
								catch
								{
								}
							}
						}
						await Task.WhenAny(done.Task, Task.Delay(25)).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				_currentOut = null;
				_currentClock = null;
			}
			finally
			{
				if (output != null)
				{
					((IDisposable)output).Dispose();
				}
			}
		}

		private static string GenerateSecMsGec()
		{
			long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 11644473600L;
			string input = (ulong)((num - num % 300) * 10000000) + "6A5AA1D4EAFF4E9FB37E23D68491D6F4";
			using SHA256 sha = SHA256.Create();
			byte[] array = sha.ComputeHash(Encoding.ASCII.GetBytes(input));
			StringBuilder sb = new StringBuilder(array.Length * 2);
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}

		private static string GenerateMuid()
		{
			byte[] bytes = new byte[16];
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(bytes);
			}
			StringBuilder sb = new StringBuilder(32);
			byte[] array = bytes;
			foreach (byte b in array)
			{
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}

		private static string RateToProsody(double rate)
		{
			int pct = (int)Math.Round((rate - 1.0) * 100.0);
			pct = Math.Max(-50, Math.Min(100, pct));
			return ((pct >= 0) ? "+" : "") + pct + "%";
		}

		private static string XmlEscape(string s)
		{
			return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
		}
	}
}
