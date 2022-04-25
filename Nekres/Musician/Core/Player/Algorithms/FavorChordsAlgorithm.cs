using System;
using System.Diagnostics;
using System.Threading;
using Blish_HUD;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
	public class FavorChordsAlgorithm : PlayAlgorithmBase
	{
		public FavorChordsAlgorithm(InstrumentBase instrument)
			: base(instrument)
		{
		}

		public override void Play(Metronome metronomeMark, ChordOffset[] melody)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int strumIndex = 0;
			while (strumIndex < melody.Length && !_abort && CanContinue())
			{
				ChordOffset strum = melody[strumIndex];
				if ((double)stopwatch.ElapsedMilliseconds > TimeSpanExtension.Multiply(metronomeMark.WholeNoteLength, (decimal)strum.Offset).TotalMilliseconds)
				{
					foreach (RealNote note in strum.Chord.Notes)
					{
						Instrument.GoToOctave(note);
						Instrument.PlayNote(note);
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
	}
}
