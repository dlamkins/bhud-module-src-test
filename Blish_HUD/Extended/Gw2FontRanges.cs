using System;

namespace Blish_HUD.Extended
{
	[Flags]
	public enum Gw2FontRanges
	{
		None = 0x0,
		BasicLatin = 0x1,
		Latin1Supplement = 0x2,
		LatinExtendedA = 0x4,
		CurrencySymbols = 0x8,
		Arrows = 0x10,
		MathOperators = 0x20,
		EnclosedAlphanumerics = 0x40,
		BoxDrawing = 0x80,
		GeometricShapes = 0x100,
		DigitsOnly = 0x200,
		Default = 0x7,
		All = 0x1FF
	}
}
