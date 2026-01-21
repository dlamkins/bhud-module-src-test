using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Models
{
	public static class NoteMapping
	{
		private static readonly Dictionary<NoteName, Keys> NaturalNoteKeys = new Dictionary<NoteName, Keys>
		{
			{
				NoteName.C,
				(Keys)97
			},
			{
				NoteName.D,
				(Keys)98
			},
			{
				NoteName.E,
				(Keys)99
			},
			{
				NoteName.F,
				(Keys)100
			},
			{
				NoteName.G,
				(Keys)101
			},
			{
				NoteName.A,
				(Keys)102
			},
			{
				NoteName.B,
				(Keys)103
			}
		};

		private static readonly Dictionary<NoteName, Keys> SharpNoteKeys = new Dictionary<NoteName, Keys>
		{
			{
				NoteName.C,
				(Keys)97
			},
			{
				NoteName.D,
				(Keys)98
			},
			{
				NoteName.F,
				(Keys)99
			},
			{
				NoteName.G,
				(Keys)100
			},
			{
				NoteName.A,
				(Keys)101
			}
		};

		public const Keys HighCKey = 104;

		public const Keys OctaveUpKey = 105;

		public const Keys OctaveDownKey = 96;

		public static Keys? GetNaturalKey(NoteName note)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (!NaturalNoteKeys.TryGetValue(note, out var key))
			{
				return null;
			}
			return key;
		}

		public static Keys? GetSharpKey(NoteName note)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (!SharpNoteKeys.TryGetValue(note, out var key))
			{
				return null;
			}
			return key;
		}

		public static bool TryParse(string note, out NoteName result)
		{
			result = NoteName.C;
			if (string.IsNullOrEmpty(note))
			{
				return false;
			}
			return Enum.TryParse<NoteName>(note.Substring(0, 1), ignoreCase: true, out result);
		}
	}
}
