using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Models
{
	public static class DrumMapping
	{
		private static readonly IReadOnlyList<DrumSoundInfo> _all;

		private static readonly Dictionary<DrumSound, DrumSoundInfo> _bySound;

		private static readonly Dictionary<string, DrumSoundInfo> _byCode;

		public static IReadOnlyList<DrumSoundInfo> All => _all;

		static DrumMapping()
		{
			_all = new List<DrumSoundInfo>
			{
				new DrumSoundInfo(DrumSound.Bass, "b", "Bass", needsAlt: false, (Keys)97, (Keys)98),
				new DrumSoundInfo(DrumSound.Snare, "s", "Snare", needsAlt: false, (Keys)99, (Keys)100),
				new DrumSoundInfo(DrumSound.CrossStick, "x", "Cross Stick", needsAlt: false, (Keys)101),
				new DrumSoundInfo(DrumSound.Ghost, "g", "Ghost", needsAlt: false, (Keys)102, (Keys)103),
				new DrumSoundInfo(DrumSound.HighTom, "ht", "High Tom", needsAlt: false, (Keys)104),
				new DrumSoundInfo(DrumSound.MidTom, "mt", "Mid Tom", needsAlt: false, (Keys)105),
				new DrumSoundInfo(DrumSound.FloorTom, "ft", "Floor Tom", needsAlt: false, (Keys)96),
				new DrumSoundInfo(DrumSound.Crash, "cr", "Crash", needsAlt: true, (Keys)97),
				new DrumSoundInfo(DrumSound.Ride, "rd", "Ride", needsAlt: true, (Keys)98),
				new DrumSoundInfo(DrumSound.HatClosed, "hc", "Hat Closed", needsAlt: true, (Keys)99),
				new DrumSoundInfo(DrumSound.HatOpen, "ho", "Hat Open", needsAlt: true, (Keys)100),
				new DrumSoundInfo(DrumSound.HatFoot, "hf", "Hat Foot", needsAlt: true, (Keys)101)
			};
			_bySound = _all.ToDictionary((DrumSoundInfo i) => i.Sound);
			_byCode = _all.ToDictionary((DrumSoundInfo i) => i.Code, StringComparer.Ordinal);
			foreach (DrumSound sound in Enum.GetValues(typeof(DrumSound)))
			{
				if (!_bySound.ContainsKey(sound))
				{
					throw new InvalidOperationException($"DrumMapping is missing a row for DrumSound.{sound}");
				}
			}
		}

		public static DrumSoundInfo Get(DrumSound sound)
		{
			return _bySound[sound];
		}

		public static bool TryFromCode(string code, out DrumSoundInfo info)
		{
			return _byCode.TryGetValue(code, out info);
		}
	}
}
