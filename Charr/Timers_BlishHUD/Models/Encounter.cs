using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Charr.Timers_BlishHUD.Models.Triggers;
using Charr.Timers_BlishHUD.Pathing.Content;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Encounter : IDisposable
	{
		private bool _activated;

		private bool _awaitingNextPhase;

		private bool _showAlerts = true;

		private bool _showMarkers = true;

		private bool _showDirections = true;

		private bool _tempResetCondition;

		private int _currentPhase;

		private DateTime _startTime;

		private DateTime _lastUpdate;

		private readonly int TICKRATE = 100;

		[JsonProperty("id")]
		public string Id { get; set; } = "Unknown Id";


		[JsonProperty("name")]
		public string Name { get; set; } = "Unknown Timer";


		[JsonProperty("category")]
		public string Category { get; set; } = "Other";


		[JsonProperty("description")]
		public string Description { get; set; } = "Timer description has not been set.";


		[JsonProperty("author")]
		public string Author { get; set; } = "Unknown Author";


		[JsonProperty("icon")]
		public string IconString { get; set; } = "raid";


		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		[JsonProperty("map")]
		public int Map { get; set; }

		[JsonProperty("phases")]
		public List<Phase> Phases { get; set; }

		[JsonConverter(typeof(TriggerConverter))]
		[JsonProperty("reset")]
		public Trigger ResetTrigger { get; set; }

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

		public bool ShowAlerts
		{
			get
			{
				return _showAlerts;
			}
			set
			{
				Phases?.ForEach(delegate(Phase ph)
				{
					ph.ShowAlerts = value;
				});
				_showAlerts = value;
			}
		}

		public bool ShowDirections
		{
			get
			{
				return _showDirections;
			}
			set
			{
				Phases?.ForEach(delegate(Phase ph)
				{
					ph.ShowDirections = value;
				});
				_showDirections = value;
			}
		}

		public bool ShowMarkers
		{
			get
			{
				return _showMarkers;
			}
			set
			{
				Phases?.ForEach(delegate(Phase ph)
				{
					ph.ShowMarkers = value;
				});
				_showMarkers = value;
			}
		}

		public bool Active { get; private set; }

		public bool Invalid { get; private set; } = true;


		public AsyncTexture2D Icon { get; set; }

		public void Initialize(PathableResourceManager resourceManager)
		{
			Icon = TimersModule.ModuleInstance.Resources.GetIcon(IconString);
			if (Icon == null)
			{
				Icon = AsyncTexture2D.op_Implicit(resourceManager.LoadTexture(IconString));
			}
			if (Map <= 0)
			{
				throw new TimerReadException("Map property undefined/invalid");
			}
			if (Phases == null || Phases.Count == 0)
			{
				throw new TimerReadException("Phase property undefined");
			}
			if (ResetTrigger == null)
			{
				throw new TimerReadException("Reset property is undefined");
			}
			string message = ResetTrigger.Initialize();
			if (message != null)
			{
				throw new TimerReadException("Reset trigger invalid - " + message);
			}
			foreach (Phase phase in Phases)
			{
				message = phase.Initialize(resourceManager);
				phase.ShowAlerts = ShowAlerts;
				phase.ShowDirections = ShowDirections;
				phase.ShowMarkers = ShowMarkers;
				if (message != null)
				{
					throw new TimerReadException(Id + ": " + message);
				}
			}
			Invalid = false;
		}

		private void Activate()
		{
			if (Enabled && !_activated)
			{
				ResetTrigger.Enable();
				if (TimersModule.ModuleInstance._debugModeSetting.get_Value() && !ResetTrigger.DepartureRequired && !ResetTrigger.DepartureRequired)
				{
					ResetTrigger.DepartureRequired = true;
					_tempResetCondition = true;
				}
				Phases.ForEach(delegate(Phase ph)
				{
					ph.Activate();
				});
				Phases[0].WaitForStart();
				_activated = true;
			}
		}

		private void Deactivate()
		{
			if (_activated)
			{
				Stop();
				Phases.ForEach(delegate(Phase ph)
				{
					ph.Deactivate();
				});
				_activated = false;
			}
		}

		private bool ShouldStart()
		{
			if (Active || !Enabled || !Activated)
			{
				return false;
			}
			if (Map != GameService.Gw2Mumble.get_CurrentMap().get_Id())
			{
				return false;
			}
			return Phases[0].StartTrigger.Triggered();
		}

		private bool ShouldStop()
		{
			if (!Active)
			{
				return false;
			}
			if (Map != GameService.Gw2Mumble.get_CurrentMap().get_Id())
			{
				return true;
			}
			if (_currentPhase == Phases.Count - 1 && Phases[_currentPhase].FinishTrigger != null && Phases[_currentPhase].FinishTrigger.Triggered())
			{
				return true;
			}
			return ResetTrigger.Triggered();
		}

		private void Start()
		{
			if (!Active && Enabled && Activated)
			{
				_startTime = DateTime.Now;
				Active = true;
				Phases[_currentPhase].Start();
				Phases[_currentPhase].Update(0f);
				_lastUpdate = DateTime.Now;
			}
		}

		private void Stop()
		{
			if (Active)
			{
				Phases.ForEach(delegate(Phase ph)
				{
					ph.Stop();
				});
				Active = false;
				_currentPhase = 0;
				_awaitingNextPhase = false;
				ResetTrigger.Disable();
				ResetTrigger.Reset();
				if (_tempResetCondition)
				{
					ResetTrigger.EntryRequired = false;
					ResetTrigger.DepartureRequired = false;
				}
				if (TimersModule.ModuleInstance._debugModeSetting.get_Value() && !ResetTrigger.DepartureRequired && !ResetTrigger.DepartureRequired)
				{
					ResetTrigger.DepartureRequired = true;
					_tempResetCondition = true;
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			if (ShouldStart())
			{
				Start();
			}
			else if (ShouldStop())
			{
				Stop();
				if (Enabled && Map == GameService.Gw2Mumble.get_CurrentMap().get_Id())
				{
					Phases[0].WaitForStart();
					ResetTrigger.Enable();
				}
			}
			else if (_awaitingNextPhase)
			{
				if (_currentPhase + 1 < Phases.Count && Phases[_currentPhase + 1].StartTrigger != null && Phases[_currentPhase + 1].StartTrigger.Triggered())
				{
					_currentPhase++;
					_awaitingNextPhase = false;
					Start();
				}
			}
			else if (Phases[_currentPhase].FinishTrigger != null && Phases[_currentPhase].FinishTrigger.Triggered())
			{
				_awaitingNextPhase = true;
				Phases[_currentPhase].Stop();
				Active = false;
				if (_currentPhase + 1 < Phases.Count)
				{
					Phases[_currentPhase + 1].WaitForStart();
				}
			}
			else if (Active && (DateTime.Now - _lastUpdate).TotalSeconds >= (double)TimersModule.ModuleInstance.Resources.TICKINTERVAL)
			{
				float elapsedTime = (float)(DateTime.Now - _startTime).TotalSeconds;
				_lastUpdate = DateTime.Now;
				Phases[_currentPhase].Update(elapsedTime);
			}
		}

		public void Dispose()
		{
			Deactivate();
			Phases?.ForEach(delegate(Phase ph)
			{
				ph?.Dispose();
			});
			Phases?.Clear();
		}
	}
}
