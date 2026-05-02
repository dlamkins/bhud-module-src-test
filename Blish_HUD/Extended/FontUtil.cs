using System.Collections.Generic;
using SpriteFontPlus;

namespace Blish_HUD.Extended
{
	internal static class FontUtil
	{
		public static IReadOnlyList<CharacterRange> GetRanges(Gw2FontRanges ranges)
		{
			List<CharacterRange> list = new List<CharacterRange>();
			if (ranges.HasFlag(Gw2FontRanges.BasicLatin))
			{
				list.Add(CharacterRange.BasicLatin);
			}
			if (ranges.HasFlag(Gw2FontRanges.Latin1Supplement))
			{
				list.Add(CharacterRange.Latin1Supplement);
			}
			if (ranges.HasFlag(Gw2FontRanges.LatinExtendedA))
			{
				list.Add(CharacterRange.LatinExtendedA);
			}
			if (ranges.HasFlag(Gw2FontRanges.CurrencySymbols))
			{
				list.Add(new CharacterRange('₣', '₾'));
			}
			if (ranges.HasFlag(Gw2FontRanges.Arrows))
			{
				list.Add(new CharacterRange('←', '⇿'));
			}
			if (ranges.HasFlag(Gw2FontRanges.MathOperators))
			{
				list.Add(new CharacterRange('∀', '⋿'));
			}
			if (ranges.HasFlag(Gw2FontRanges.EnclosedAlphanumerics))
			{
				list.Add(new CharacterRange('①', '⓿'));
			}
			if (ranges.HasFlag(Gw2FontRanges.BoxDrawing))
			{
				list.Add(new CharacterRange('─', '╿'));
			}
			if (ranges.HasFlag(Gw2FontRanges.GeometricShapes))
			{
				list.Add(new CharacterRange('■', '◿'));
			}
			if (ranges.HasFlag(Gw2FontRanges.DigitsOnly))
			{
				list.Add(new CharacterRange('0', '9'));
				list.Add(new CharacterRange(':', ':'));
				list.Add(new CharacterRange('.', '.'));
				list.Add(new CharacterRange(',', ','));
				list.Add(new CharacterRange('-', '-'));
				list.Add(new CharacterRange('+', '+'));
				list.Add(new CharacterRange(' ', ' '));
				list.Add(new CharacterRange('%', '%'));
			}
			return list;
		}
	}
}
