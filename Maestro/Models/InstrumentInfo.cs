using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Maestro.Models
{
	public sealed class InstrumentInfo
	{
		public InstrumentType Type { get; }

		public string DisplayName { get; }

		public Color Accent { get; }

		public Color AccentDark { get; }

		public bool SharpsEnabled { get; }

		public int MinOctave { get; }

		public int MaxOctave { get; }

		public bool IsPercussion { get; }

		public IReadOnlyList<string> OctaveLabels { get; }

		public bool ListedInPickers { get; }

		public InstrumentInfo(InstrumentType type, string displayName, Color accent, Color accentDark, bool sharpsEnabled, int minOctave, int maxOctave, string[] octaveLabels, bool listedInPickers = true, bool isPercussion = false)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			Type = type;
			DisplayName = displayName;
			Accent = accent;
			AccentDark = accentDark;
			SharpsEnabled = sharpsEnabled;
			MinOctave = minOctave;
			MaxOctave = maxOctave;
			OctaveLabels = Array.AsReadOnly(octaveLabels);
			ListedInPickers = listedInPickers;
			IsPercussion = isPercussion;
		}
	}
}
