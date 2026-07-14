using System;
using Blish_HUD.Settings;

namespace Maestro.Models
{
	public class PracticeSettings
	{
		public SettingEntry<float> LastUsedSpeed { get; }

		public SettingEntry<float> LookaheadSeconds { get; }

		public SettingEntry<int> CountdownLengthMs { get; }

		public PracticeSettings(SettingCollection settings)
		{
			LastUsedSpeed = settings.DefineSetting<float>("practice.lastUsedSpeed", 1f, (Func<string>)(() => "Last used practice speed"), (Func<string>)(() => "Internal: remembers the last practice speed multiplier."));
			LookaheadSeconds = settings.DefineSetting<float>("practice.lookaheadSeconds", 2.5f, (Func<string>)(() => "Practice: lookahead (seconds)"), (Func<string>)(() => "How far ahead in the song the highway shows. Lower = faster scroll, shorter reaction time."));
			SettingComplianceExtensions.SetRange(LookaheadSeconds, 1f, 5f);
			CountdownLengthMs = settings.DefineSetting<int>("practice.countdownLengthMs", 3000, (Func<string>)(() => "Practice: countdown length (ms)"), (Func<string>)(() => "How long the 3-2-1 countdown lasts before a practice session starts."));
			SettingComplianceExtensions.SetRange(CountdownLengthMs, 1000, 6000);
		}
	}
}
