using System.Collections.Generic;
using Charr.Timers_BlishHUD.Models.Triggers;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Actions
{
	public abstract class TimerAction
	{
		protected bool _initialized;

		[JsonProperty("name")]
		public string Name { get; set; } = "Unnamed Action";


		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("sets")]
		public List<string> TimerSets { get; set; } = new List<string>();


		[JsonConverter(typeof(TriggerConverter))]
		[JsonProperty("trigger")]
		public Trigger ActionTrigger { get; set; }

		public bool Initialized => _initialized;

		public abstract string Initialize();

		public abstract void Update();

		public abstract void Dispose();

		public abstract void Start();

		public abstract void Stop();

		public abstract void Reset();
	}
}
