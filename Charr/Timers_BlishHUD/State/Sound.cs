using System.Collections.Generic;
using System.Speech.Synthesis;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.State
{
	public class Sound
	{
		private bool _activated;

		private SpeechSynthesizer _synthesizer;

		[JsonProperty("uid")]
		public string UID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; } = "Unnamed Sound";


		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("timestamps")]
		public List<float> Timestamps { get; set; }

		public bool Activated
		{
			get
			{
				return _activated;
			}
			set
			{
				if (value)
				{
					Activate();
				}
				else
				{
					Deactivate();
				}
			}
		}

		public string Initialize()
		{
			if (Text.IsNullOrEmpty())
			{
				return Name + " invalid text property";
			}
			if (Timestamps.IsNullOrEmpty())
			{
				return Name + " invalid timestamps property";
			}
			_synthesizer = new SpeechSynthesizer();
			_synthesizer.Rate = -2;
			_synthesizer.Volume = 100;
			return null;
		}

		public void Activate()
		{
			_activated = true;
		}

		public void Deactivate()
		{
			_synthesizer.SpeakAsyncCancelAll();
			_activated = false;
		}

		public void Stop()
		{
			_synthesizer.SpeakAsyncCancelAll();
		}

		public void Update(float elapsedTime)
		{
			if (!_activated || TimersModule.ModuleInstance._hideSoundsSetting.Value)
			{
				return;
			}
			foreach (float time in Timestamps)
			{
				if (elapsedTime >= time && elapsedTime <= time + TimersModule.ModuleInstance.Resources.TICKINTERVAL)
				{
					_synthesizer.SpeakAsync(Text);
					break;
				}
			}
		}
	}
}
