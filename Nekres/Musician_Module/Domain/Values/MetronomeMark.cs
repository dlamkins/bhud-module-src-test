using System;
using Blish_HUD;

namespace Nekres.Musician_Module.Domain.Values
{
	public class MetronomeMark
	{
		public int Metronome { get; }

		public Fraction BeatsPerMeasure { get; }

		public TimeSpan QuaterNoteLength { get; }

		public TimeSpan WholeNoteLength { get; }

		public MetronomeMark(int metronome, Fraction beatsPerMeasure)
		{
			BeatsPerMeasure = beatsPerMeasure;
			Metronome = metronome;
			QuaterNoteLength = TimeSpanExtension.Divide(TimeSpan.FromMinutes(1.0), metronome * 16 / beatsPerMeasure.Denominator);
			WholeNoteLength = TimeSpanExtension.Multiply(TimeSpanExtension.Divide(TimeSpan.FromMinutes(1.0), metronome * 16 / beatsPerMeasure.Denominator), 4);
		}
	}
}
