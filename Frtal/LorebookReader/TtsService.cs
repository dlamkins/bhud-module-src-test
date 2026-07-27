using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using Windows.Media;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;

namespace Frtal.LorebookReader
{
	public sealed class TtsService : IDisposable
	{
		private readonly SpeechSynthesizer _synth = new SpeechSynthesizer();

		private CancellationTokenSource _cts;

		private WaveOutEvent _currentOut;

		private Stopwatch _currentClock;

		public bool IsPaused { get; private set; }

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

		public static IEnumerable<(string Name, string Lang)> InstalledVoices()
		{
			return from v in SpeechSynthesizer.AllVoices
				select (v.DisplayName, v.Language) into v
				orderby v.Language, v.DisplayName
				select v;
		}

		public async Task<string> SpeakAsync(string text, string voiceName, double rate, string languageTag, Action<string> onChunk = null, Action<int> onWord = null)
		{
			Stop();
			CancellationToken ct = (_cts = new CancellationTokenSource()).Token;
			string warning = SelectVoice(voiceName, languageTag);
			_synth.Options.SpeakingRate = Math.Max(0.5, Math.Min(3.0, rate));
			_synth.Options.IncludeWordBoundaryMetadata = true;
			List<string> chunks = TextCleaner.SplitChunks(text);
			if (chunks.Count == 0)
			{
				return warning;
			}
			try
			{
				Task<(byte[] wav, List<TimeSpan> words)> nextTask = SynthesizeChunkAsync(chunks[0]);
				for (int i = 0; i < chunks.Count; i++)
				{
					var (wav, words) = await nextTask.ConfigureAwait(continueOnCapturedContext: false);
					if (ct.IsCancellationRequested)
					{
						return warning;
					}
					nextTask = (Task<(byte[] wav, List<TimeSpan> words)>)((i + 1 < chunks.Count) ? ((Task)SynthesizeChunkAsync(chunks[i + 1])) : ((Task)Task.FromResult<(byte[], List<TimeSpan>)>((null, null))));
					onChunk?.Invoke(chunks[i]);
					await PlayWavAsync(wav, words, onWord, ct).ConfigureAwait(continueOnCapturedContext: false);
					if (ct.IsCancellationRequested)
					{
						return warning;
					}
				}
			}
			finally
			{
				onChunk?.Invoke(null);
			}
			return warning;
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
			try
			{
				_synth?.Dispose();
			}
			catch
			{
			}
		}

		private string SelectVoice(string voiceName, string languageTag)
		{
			IReadOnlyList<VoiceInformation> all = SpeechSynthesizer.AllVoices;
			VoiceInformation voice = null;
			string warning = null;
			if (!string.IsNullOrWhiteSpace(voiceName))
			{
				voice = all.FirstOrDefault((VoiceInformation v) => v.DisplayName.IndexOf(voiceName.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
				if (voice == null)
				{
					warning = "Voice \"" + voiceName + "\" not found, choosing by language.";
				}
			}
			if (voice == null)
			{
				string prefix = (languageTag ?? "en").Split('-')[0];
				voice = all.FirstOrDefault((VoiceInformation v) => v.Language.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
				if (voice == null)
				{
					warning = "No \"" + prefix + "\" voice is installed in Windows — using default. Install one via Settings > Time & Language > Speech > Add voices.";
				}
			}
			if (voice != null)
			{
				_synth.Voice = voice;
			}
			return warning;
		}

		private async Task<(byte[] wav, List<TimeSpan> words)> SynthesizeChunkAsync(string chunk)
		{
			TaskAwaiter<SpeechSynthesisStream> taskAwaiter = WindowsRuntimeSystemExtensions.GetAwaiter<SpeechSynthesisStream>(_synth.SynthesizeTextToStreamAsync(chunk));
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<SpeechSynthesisStream> taskAwaiter2 = default(TaskAwaiter<SpeechSynthesisStream>);
				taskAwaiter = taskAwaiter2;
			}
			SpeechSynthesisStream result = taskAwaiter.GetResult();
			using SpeechSynthesisStream stream = result;
			List<TimeSpan> words = new List<TimeSpan>();
			try
			{
				foreach (IMediaMarker i in stream.Markers)
				{
					words.Add(i.Time);
				}
			}
			catch
			{
			}
			MemoryStream ms = new MemoryStream();
			await WindowsRuntimeStreamExtensions.AsStreamForRead((IInputStream)stream).CopyToAsync(ms).ConfigureAwait(continueOnCapturedContext: false);
			return (ms.ToArray(), words);
		}

		private async Task PlayWavAsync(byte[] wav, List<TimeSpan> words, Action<int> onWord, CancellationToken ct)
		{
			if (wav == null || wav.Length == 0)
			{
				return;
			}
			using MemoryStream ms = new MemoryStream(wav);
			using WaveFileReader reader = new WaveFileReader(ms);
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
	}
}
