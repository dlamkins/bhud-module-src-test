using System;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
	public class FavorNotesAlgorithm : PlayAlgorithmBase
	{
		public FavorNotesAlgorithm(InstrumentBase instrument)
			: base(instrument)
		{
		}

		public override void Play(Metronome metronomeMark, ChordOffset[] melody)
		{
			PrepareChordsOctave(melody[0].Chord);
			_stopwatch.Start();
			int strumIndex = 0;
			while (strumIndex < melody.Length && !_abort && CanContinue())
			{
				ChordOffset strum = melody[strumIndex];
				if ((double)_stopwatch.ElapsedMilliseconds > TimeSpanExtension.Multiply(metronomeMark.WholeNoteLength, (decimal)strum.Offset).TotalMilliseconds)
				{
					Chord chord = strum.Chord;
					PlayChord(chord);
					if (strumIndex < melody.Length - 1)
					{
						PrepareChordsOctave(melody[strumIndex + 1].Chord);
					}
					strumIndex++;
				}
				else
				{
					Thread.Sleep(TimeSpan.FromMilliseconds(1.0));
				}
			}
			MusicianModule.ModuleInstance.MusicPlayer?.Stop();
		}

		private void PrepareChordsOctave(Chord chord)
		{
			Instrument.GoToOctave(chord.Notes.First());
		}

		private void PlayChord(Chord chord)
		{
			RealNote[] notes = chord.Notes.ToArray();
			for (int noteIndex = 0; noteIndex < notes.Length; noteIndex++)
			{
				Instrument.PlayNote(notes[noteIndex]);
				if (noteIndex < notes.Length - 1)
				{
					PrepareNoteOctave(notes[noteIndex + 1]);
				}
			}
		}

		private void PrepareNoteOctave(RealNote note)
		{
			Instrument.GoToOctave(note);
		}
	}
}
