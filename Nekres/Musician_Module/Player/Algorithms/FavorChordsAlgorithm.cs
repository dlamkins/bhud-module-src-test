using System;
using System.Diagnostics;
using System.Threading;
using Blish_HUD;
using Nekres.Musician_Module.Controls.Instrument;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Player.Algorithms
{
	public class FavorChordsAlgorithm : IPlayAlgorithm
	{
		private bool Abort;

		public void Dispose()
		{
			Abort = true;
		}

		public void Play(Instrument instrument, MetronomeMark metronomeMark, ChordOffset[] melody)
		{
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
					foreach (Note note in strum.Chord.Notes)
					{
						instrument.GoToOctave(note);
						instrument.PlayNote(note);
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
	}
}
