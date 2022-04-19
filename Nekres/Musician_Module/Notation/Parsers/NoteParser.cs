using System;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Notation.Parsers
{
	public class NoteParser
	{
		public Note Parse(string text, string instrument)
		{
			Note.Keys key = ParseKey(text);
			Note.Octaves octave = ParseOctave(text, instrument);
			return new Note(key, octave);
		}

		private static Note.Keys ParseKey(string text)
		{
			switch (text)
			{
			case "Z,":
			case "Z,,":
			case "Z,,,":
			case "Z":
			case "z":
			case "z'":
				return Note.Keys.None;
			case "C,":
			case "C,,":
			case "C,,,":
			case "C":
			case "c":
			case "c'":
			case "c''":
				return Note.Keys.C;
			case "D,":
			case "D,,":
			case "D,,,":
			case "D":
			case "d":
			case "d'":
			case "d''":
				return Note.Keys.D;
			case "E,":
			case "E,,":
			case "E,,,":
			case "E":
			case "e":
			case "e'":
				return Note.Keys.E;
			case "F,":
			case "F,,":
			case "F,,,":
			case "F":
			case "f":
			case "f'":
				return Note.Keys.F;
			case "G,":
			case "G,,":
			case "G,,,":
			case "G":
			case "g":
			case "g'":
				return Note.Keys.G;
			case "A,":
			case "A,,":
			case "A,,,":
			case "A":
			case "a":
			case "a'":
				return Note.Keys.A;
			case "B,":
			case "B,,":
			case "B,,,":
			case "B":
			case "b":
			case "b'":
				return Note.Keys.B;
			default:
				throw new NotSupportedException(text);
			}
		}

		private static Note.Octaves ParseOctave(string text, string instrument)
		{
			switch (text)
			{
			case "Z,":
			case "Z":
			case "z":
			case "z'":
				return Note.Octaves.None;
			case "C,":
			case "D,":
			case "E,":
			case "F,":
			case "G,":
			case "A,":
			case "B,":
				return Note.Octaves.Lowest;
			case "C":
			case "D":
			case "E":
			case "F":
			case "G":
			case "A":
			case "B":
			case "C,,":
			case "D,,":
			case "E,,":
			case "F,,":
			case "G,,":
			case "A,,":
			case "B,,":
				return Note.Octaves.Low;
			case "c":
			case "d":
			case "e":
			case "f":
			case "g":
			case "a":
			case "b":
			case "C,,,":
			case "D,,,":
			case "E,,,":
			case "F,,,":
			case "G,,,":
			case "A,,,":
			case "B,,,":
				return Note.Octaves.Middle;
			case "c'":
			case "d'":
			case "e'":
			case "f'":
			case "g'":
			case "a'":
			case "b'":
				return Note.Octaves.High;
			case "c''":
			case "d''":
				return Note.Octaves.Highest;
			default:
				throw new NotSupportedException(text);
			}
		}
	}
}
