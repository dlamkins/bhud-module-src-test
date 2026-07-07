using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;

namespace Frtal.LorebookReader
{
	public sealed class TtsService : IDisposable
	{
		private readonly SpeechSynthesizer _synth = new SpeechSynthesizer();

		private CancellationTokenSource _cts;

		private WaveOutEvent _currentOut;

		public static IEnumerable<(string Name, string Lang)> InstalledVoices()
		{
			return from v in SpeechSynthesizer.AllVoices
				select (v.DisplayName, v.Language) into v
				orderby v.Language, v.DisplayName
				select v;
		}

		public async Task<string> SpeakAsync(string text, string voiceName, double rate, string languageTag, Action<string> onChunk = null)
		{
			Stop();
			CancellationToken ct = (_cts = new CancellationTokenSource()).Token;
			string warning = SelectVoice(voiceName, languageTag);
			_synth.Options.SpeakingRate = Math.Max(0.5, Math.Min(3.0, rate));
			List<string> chunks = TextCleaner.SplitChunks(text);
			if (chunks.Count == 0)
			{
				return warning;
			}
			try
			{
				Task<byte[]> nextTask = SynthesizeChunkAsync(chunks[0]);
				for (int i = 0; i < chunks.Count; i++)
				{
					byte[] wav = await nextTask.ConfigureAwait(continueOnCapturedContext: false);
					if (ct.IsCancellationRequested)
					{
						return warning;
					}
					nextTask = ((i + 1 < chunks.Count) ? SynthesizeChunkAsync(chunks[i + 1]) : Task.FromResult<byte[]>(null));
					onChunk?.Invoke(chunks[i]);
					await PlayWavAsync(wav, ct).ConfigureAwait(continueOnCapturedContext: false);
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
			Stop();
			_currentOut?.Dispose();
			_synth.Dispose();
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

		private async Task<byte[]> SynthesizeChunkAsync(string chunk)
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
			MemoryStream ms = new MemoryStream();
			await WindowsRuntimeStreamExtensions.AsStreamForRead((IInputStream)stream).CopyToAsync(ms).ConfigureAwait(continueOnCapturedContext: false);
			return ms.ToArray();
		}

		private async Task PlayWavAsync(byte[] wav, CancellationToken ct)
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
					await done.Task.ConfigureAwait(continueOnCapturedContext: false);
				}
				_currentOut = null;
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
