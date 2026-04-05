using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using NAudio.Midi;
using SongbookOfTyria.Models;
using SongbookOfTyria.Utilities;

namespace SongbookOfTyria.Services
{
	public sealed class MidiFileParser : IDisposable
	{
		private class TempoEntry
		{
			public long Tick;

			public int MicrosecondsPerBeat;
		}

		private static readonly Logger Logger = Logger.GetLogger<MidiFileParser>();

		private readonly HttpClient _httpClient;

		private readonly bool _ownsHttpClient;

		private readonly string _cacheDirectory;

		private bool _disposed;

		public MidiFileParser(string cacheDirectory, HttpClient httpClient = null)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_cacheDirectory = Path.Combine(cacheDirectory, "midi_cache");
			_ownsHttpClient = httpClient == null;
			object obj = httpClient;
			if (obj == null)
			{
				HttpClient val = new HttpClient();
				obj = (object)val;
				val.set_Timeout(TimeSpan.FromSeconds(30.0));
			}
			_httpClient = (HttpClient)obj;
			Directory.CreateDirectory(_cacheDirectory);
		}

		public Task<MidiData> ParseFromUrlAsync(string midiFileUrl)
		{
			return ParseFromUrlAsync(midiFileUrl, CancellationToken.None);
		}

		public async Task<MidiData> ParseFromUrlAsync(string midiFileUrl, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(midiFileUrl))
			{
				return null;
			}
			try
			{
				string filePath = await DownloadMidiFileAsync(midiFileUrl, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (filePath == null)
				{
					return null;
				}
				return ParseMidiFile(filePath);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to parse MIDI file from {Url}", new object[1] { midiFileUrl });
				return null;
			}
		}

		private async Task<string> DownloadMidiFileAsync(string url, CancellationToken cancellationToken)
		{
			string fileName = GenerateCacheFileName(url);
			string cachedPath = Path.Combine(_cacheDirectory, fileName);
			if (File.Exists(cachedPath))
			{
				Logger.Debug("Using cached MIDI file: {Path}", new object[1] { cachedPath });
				return cachedPath;
			}
			Logger.Debug("Downloading MIDI file from {Url}", new object[1] { url });
			HttpResponseMessage response = await _httpClient.GetAsync(url, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				response.EnsureSuccessStatusCode();
				using Stream stream = await response.get_Content().ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
				using FileStream fileStream = new FileStream(cachedPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
				await stream.CopyToAsync(fileStream).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
			return cachedPath;
		}

		private MidiData ParseMidiFile(string filePath)
		{
			MidiFile midi = new MidiFile(filePath, strictChecking: false);
			int ticksPerQuarterNote = midi.DeltaTicksPerQuarterNote;
			List<TempoEntry> tempoMap = BuildTempoMap(midi);
			List<MidiTrack> tracks = new List<MidiTrack>();
			double maxDuration = 0.0;
			for (int trackIndex = 0; trackIndex < midi.Tracks; trackIndex++)
			{
				IList<MidiEvent> events = midi.Events[trackIndex];
				string trackName = GetTrackName(events);
				MidiInstrument instrument = GetTrackInstrument(events);
				List<MidiNote> notes = ExtractNotes(events, tempoMap, ticksPerQuarterNote);
				if (notes.Count != 0)
				{
					double trackDuration = notes.Max((MidiNote n) => n.Time + n.Duration);
					if (trackDuration > maxDuration)
					{
						maxDuration = trackDuration;
					}
					tracks.Add(new MidiTrack
					{
						Index = tracks.Count,
						Name = (trackName ?? $"Track {tracks.Count + 1}"),
						Instrument = instrument,
						NoteCount = notes.Count,
						Notes = notes
					});
				}
			}
			if (tracks.Count == 0)
			{
				Logger.Warn("MIDI file contained no note data: {Path}", new object[1] { filePath });
				return null;
			}
			List<MidiTempo> tempos = tempoMap.Select((TempoEntry t) => new MidiTempo
			{
				Ticks = (int)t.Tick,
				Bpm = 60000000.0 / (double)t.MicrosecondsPerBeat,
				MicrosecondsPerBeat = t.MicrosecondsPerBeat
			}).ToList();
			Logger.Debug("Parsed MIDI: {TrackCount} tracks, {Duration:F1}s duration", new object[2] { tracks.Count, maxDuration });
			return new MidiData
			{
				Tracks = tracks,
				Duration = maxDuration,
				Ppq = ticksPerQuarterNote,
				Tempos = tempos
			};
		}

		private List<TempoEntry> BuildTempoMap(MidiFile midi)
		{
			List<TempoEntry> tempoMap = new List<TempoEntry>();
			for (int trackIndex = 0; trackIndex < midi.Tracks; trackIndex++)
			{
				foreach (MidiEvent midiEvent in midi.Events[trackIndex])
				{
					TempoEvent tempoEvent = midiEvent as TempoEvent;
					if (tempoEvent != null)
					{
						tempoMap.Add(new TempoEntry
						{
							Tick = midiEvent.AbsoluteTime,
							MicrosecondsPerBeat = tempoEvent.MicrosecondsPerQuarterNote
						});
					}
				}
			}
			List<TempoEntry> ordered = tempoMap.OrderBy((TempoEntry t) => t.Tick).ToList();
			if (ordered.Count == 0 || ordered[0].Tick > 0)
			{
				ordered.Insert(0, new TempoEntry
				{
					Tick = 0L,
					MicrosecondsPerBeat = 500000
				});
			}
			return ordered;
		}

		private double TicksToSeconds(long ticks, List<TempoEntry> tempoMap, int ticksPerQuarterNote)
		{
			double seconds = 0.0;
			long lastTick = 0L;
			int currentTempo = tempoMap[0].MicrosecondsPerBeat;
			foreach (TempoEntry entry in tempoMap.Skip(1))
			{
				if (entry.Tick >= ticks)
				{
					break;
				}
				long deltaTicks = entry.Tick - lastTick;
				seconds += (double)(deltaTicks * currentTempo) / ((double)ticksPerQuarterNote * 1000000.0);
				lastTick = entry.Tick;
				currentTempo = entry.MicrosecondsPerBeat;
			}
			long remainingTicks = ticks - lastTick;
			return seconds + (double)(remainingTicks * currentTempo) / ((double)ticksPerQuarterNote * 1000000.0);
		}

		private List<MidiNote> ExtractNotes(IList<MidiEvent> events, List<TempoEntry> tempoMap, int ticksPerQuarterNote)
		{
			List<MidiNote> notes = new List<MidiNote>();
			Dictionary<int, (long tick, int velocity)> activeNotes = new Dictionary<int, (long, int)>();
			foreach (MidiEvent midiEvent in events)
			{
				NoteOnEvent noteOn = midiEvent as NoteOnEvent;
				if (noteOn != null)
				{
					if (noteOn.Velocity > 0)
					{
						activeNotes[noteOn.NoteNumber] = (noteOn.AbsoluteTime, noteOn.Velocity);
					}
					else
					{
						CompleteNote(noteOn.NoteNumber, noteOn.AbsoluteTime);
					}
					continue;
				}
				NoteEvent noteOff = midiEvent as NoteEvent;
				if (noteOff != null && midiEvent.CommandCode == MidiCommandCode.NoteOff)
				{
					CompleteNote(noteOff.NoteNumber, noteOff.AbsoluteTime);
				}
			}
			return notes;
			void CompleteNote(int noteNumber, long endTick)
			{
				if (activeNotes.TryGetValue(noteNumber, out var start))
				{
					activeNotes.Remove(noteNumber);
					double startTime = TicksToSeconds(start.Item1, tempoMap, ticksPerQuarterNote);
					double duration = TicksToSeconds(endTick, tempoMap, ticksPerQuarterNote) - startTime;
					if (duration > 0.0)
					{
						notes.Add(new MidiNote
						{
							Midi = noteNumber,
							Time = startTime,
							Duration = duration,
							Velocity = start.Item2
						});
					}
				}
			}
		}

		private static string GetTrackName(IList<MidiEvent> events)
		{
			return events.OfType<TextEvent>().FirstOrDefault((TextEvent e) => e.MetaEventType == MetaEventType.SequenceTrackName)?.Text;
		}

		private static MidiInstrument GetTrackInstrument(IList<MidiEvent> events)
		{
			PatchChangeEvent patchChange = events.OfType<PatchChangeEvent>().FirstOrDefault();
			if (patchChange == null)
			{
				return null;
			}
			return new MidiInstrument
			{
				Number = patchChange.Patch,
				Name = PatchChangeEvent.GetPatchName(patchChange.Patch)
			};
		}

		private static string GenerateCacheFileName(string url)
		{
			string hash = HashUtility.GetStableHashCode(url).ToString("X8");
			string originalName = Path.GetFileNameWithoutExtension(new Uri(url).LocalPath);
			if (string.IsNullOrEmpty(originalName))
			{
				originalName = "midi";
			}
			return originalName + "_" + hash + ".mid";
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			if (_ownsHttpClient)
			{
				HttpClient httpClient = _httpClient;
				if (httpClient != null)
				{
					((HttpMessageInvoker)httpClient).Dispose();
				}
			}
		}
	}
}
