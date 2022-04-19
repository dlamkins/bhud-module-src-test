using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Nekres.Musician_Module.Controls.Instrument;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Player.Algorithms
{
	public class FavorNotesAlgorithm : IPlayAlgorithm
	{
		private bool Abort;

		public void Dispose()
		{
			Abort = true;
		}

		public void Play(Instrument instrument, MetronomeMark metronomeMark, ChordOffset[] melody)
		{
			PrepareChordsOctave(instrument, melody[0].Chord);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int strumIndex = 0;
			while (strumIndex < melody.Length)
			{
				if (Abort)
				{
					return;
				}
				ChordOffset strum = melody[strumIndex];
				if ((double)stopwatch.ElapsedMilliseconds > TimeSpanExtension.Multiply(metronomeMark.WholeNoteLength, (decimal)strum.Offest).TotalMilliseconds)
				{
					Chord chord = strum.Chord;
					PlayChord(instrument, chord);
					if (strumIndex < melody.Length - 1)
					{
						PrepareChordsOctave(instrument, melody[strumIndex + 1].Chord);
					}
					strumIndex++;
				}
				else
				{
					Thread.Sleep(TimeSpan.FromMilliseconds(1.0));
				}
			}
			stopwatch.Stop();
		}

		private static void PrepareChordsOctave(Instrument instrument, Chord chord)
		{
			instrument.GoToOctave(chord.Notes.First());
		}

		private static void PlayChord(Instrument instrument, Chord chord)
		{
			Note[] notes = chord.Notes.ToArray();
			for (int noteIndex = 0; noteIndex < notes.Length; noteIndex++)
			{
				instrument.PlayNote(notes[noteIndex]);
				if (noteIndex < notes.Length - 1)
				{
					PrepareNoteOctave(instrument, notes[noteIndex + 1]);
				}
			}
		}

		private static void PrepareNoteOctave(Instrument instrument, Note note)
		{
			instrument.GoToOctave(note);
		}
	}
}
