using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Actions
{
	public class SkipAction : TimerAction
	{
		private float _skippedTime;

		[JsonProperty("time")]
		public float Time { get; set; }

		public float SkippedTime => _skippedTime;

		public SkipAction()
		{
			base.Name = "Skip Action";
			base.Type = "skipTime";
		}

		public override void Update()
		{
			if (base.ActionTrigger != null && base.ActionTrigger.Triggered())
			{
				_skippedTime += Time;
				base.ActionTrigger.Reset();
			}
		}

		public override string Initialize()
		{
			if (base.TimerSets == null)
			{
				return "no TimerSets.";
			}
			if (base.ActionTrigger == null)
			{
				return "no Triggers.";
			}
			base.ActionTrigger.Initialize();
			_initialized = true;
			return null;
		}

		public override void Dispose()
		{
			base.ActionTrigger?.Reset();
			base.ActionTrigger?.Disable();
		}

		public override void Start()
		{
			base.ActionTrigger?.Reset();
			base.ActionTrigger?.Enable();
		}

		public override void Stop()
		{
			base.ActionTrigger?.Reset();
			base.ActionTrigger?.Disable();
		}

		public override void Reset()
		{
			base.ActionTrigger?.Reset();
			_skippedTime = 0f;
		}
	}
}
