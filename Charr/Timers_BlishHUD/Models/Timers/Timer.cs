using System.Collections.Generic;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Timers
{
	public abstract class Timer
	{
		protected bool _activated;

		protected bool _showTimer;

		[JsonProperty("uid")]
		public string UID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; } = "Unnamed Timer";


		[JsonProperty("set")]
		public string TimerSet { get; set; } = "default";


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

		public void Update(Dictionary<string, float> elapsedTimes)
		{
			if (elapsedTimes.ContainsKey(TimerSet))
			{
				Update(elapsedTimes[TimerSet]);
			}
			else
			{
				Update(elapsedTimes["default"]);
			}
		}

		public abstract void Update(float elapsedTime);

		public abstract void Activate();

		public abstract void Deactivate();

		public abstract void Stop();
	}
}
