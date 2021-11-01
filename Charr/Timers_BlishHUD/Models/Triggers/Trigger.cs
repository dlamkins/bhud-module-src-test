using System.Collections.Generic;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Triggers
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class Trigger
	{
		protected bool _initialized;

		protected bool _enabled;

		[JsonProperty("type")]
		public string Type { get; }

		[JsonProperty("position")]
		public List<float> Position { get; set; }

		[JsonProperty("antipode")]
		public List<float> Antipode { get; set; }

		[JsonProperty("radius")]
		public float Radius { get; set; }

		[JsonProperty("requireCombat")]
		public bool CombatRequired { get; set; }

		[JsonProperty("requireOutOfCombat")]
		public bool OutOfCombatRequired { get; set; }

		[JsonProperty("requireEntry")]
		public bool EntryRequired { get; set; }

		[JsonProperty("requireDeparture")]
		public bool DepartureRequired { get; set; }

		public bool Initialized => _initialized;

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (value)
				{
					Enable();
				}
				else
				{
					Disable();
				}
			}
		}

		public abstract string Initialize();

		public abstract void Enable();

		public abstract void Disable();

		public abstract void Reset();

		public abstract bool Triggered();
	}
}
