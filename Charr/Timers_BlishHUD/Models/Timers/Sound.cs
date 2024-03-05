using System;
using System.Speech.Synthesis;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Timers
{
	public class Sound : Timer
	{
		private SpeechSynthesizer _synthesizer;

		private int timeIndex;

		[JsonProperty("text")]
		public string Text { get; set; }

		public Sound()
		{
			base.Name = "Unnamed Sound";
		}

		public string Initialize()
		{
			if (Text.IsNullOrEmpty())
			{
				return base.Name + " invalid text property";
			}
			if (base.Timestamps.IsNullOrEmpty())
			{
				return base.Name + " invalid timestamps property";
			}
			base.Timestamps.Sort();
			_synthesizer = new SpeechSynthesizer();
			_synthesizer.Rate = -2;
			_synthesizer.Volume = TimersModule.ModuleInstance._volumeSetting?.Value ?? 100;
			TimersModule.ModuleInstance._volumeSetting.SettingChanged += HandleVolumeSettingChanged;
			return null;
		}

		private void HandleVolumeSettingChanged(object sender = null, EventArgs e = null)
		{
			_synthesizer.Volume = TimersModule.ModuleInstance._volumeSetting.Value;
		}

		public override void Activate()
		{
			_activated = true;
			timeIndex = 0;
		}

		public override void Deactivate()
		{
			_synthesizer.SpeakAsyncCancelAll();
			_activated = false;
			timeIndex = 0;
		}

		public override void Stop()
		{
			_synthesizer.SpeakAsyncCancelAll();
			timeIndex = 0;
		}

		public override void Update(float elapsedTime)
		{
			if (_activated && !TimersModule.ModuleInstance._hideSoundsSetting.Value && timeIndex < base.Timestamps.Count && elapsedTime >= base.Timestamps[timeIndex])
			{
				_synthesizer.SpeakAsync(Text);
				timeIndex++;
			}
		}
	}
}
