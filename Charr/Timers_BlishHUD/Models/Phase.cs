using System;
using System.Collections.Generic;
using Blish_HUD;
using Charr.Timers_BlishHUD.Models.Triggers;
using Charr.Timers_BlishHUD.Pathing.Content;
using Charr.Timers_BlishHUD.State;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Phase : IDisposable
	{
		private bool _activated;

		private bool _showAlerts = true;

		private bool _showDirections = true;

		private bool _showMarkers = true;

		private static readonly Logger Logger = Logger.GetLogger<Phase>();

		[JsonProperty("name")]
		public string Name { get; set; } = "Unnamed Phase";


		[JsonConverter(typeof(TriggerConverter))]
		[JsonProperty("start")]
		public Trigger StartTrigger { get; set; }

		[JsonConverter(typeof(TriggerConverter))]
		[JsonProperty("finish")]
		public Trigger FinishTrigger { get; set; }

		[JsonProperty("alerts")]
		public List<Alert> Alerts { get; set; }

		[JsonProperty("directions")]
		public List<Direction> Directions { get; set; } = new List<Direction>();


		[JsonProperty("markers")]
		public List<Marker> Markers { get; set; } = new List<Marker>();


		[JsonProperty("sounds")]
		public List<Sound> Sounds { get; set; } = new List<Sound>();


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
				Alerts?.ForEach(delegate(Alert al)
				{
					al.ShowAlert = value;
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
				Directions?.ForEach(delegate(Direction dir)
				{
					dir.ShowDirection = value;
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
				Markers?.ForEach(delegate(Marker mark)
				{
					mark.ShowMarker = value;
				});
				_showMarkers = value;
			}
		}

		public bool Active { get; private set; }

		public string Initialize(PathableResourceManager pathableResourceManager)
		{
			if (StartTrigger == null)
			{
				return "phase missing start trigger";
			}
			string message = StartTrigger.Initialize();
			if (message != null)
			{
				return message;
			}
			if (FinishTrigger != null)
			{
				message = FinishTrigger.Initialize();
				if (message != null)
				{
					return message;
				}
			}
			if (Alerts != null)
			{
				foreach (Alert alert in Alerts)
				{
					message = alert.Initialize(pathableResourceManager);
					alert.ShowAlert = ShowAlerts;
					if (message != null)
					{
						return message;
					}
				}
			}
			if (Directions != null)
			{
				foreach (Direction direction in Directions)
				{
					message = direction.Initialize(pathableResourceManager);
					direction.ShowDirection = ShowDirections;
					if (message != null)
					{
						return message;
					}
				}
			}
			if (Markers != null)
			{
				foreach (Marker marker in Markers)
				{
					message = marker.Initialize(pathableResourceManager);
					marker.ShowMarker = ShowMarkers;
					if (message != null)
					{
						return message;
					}
				}
			}
			if (Sounds != null)
			{
				foreach (Sound sound in Sounds)
				{
					message = sound.Initialize();
					if (message != null)
					{
						return message;
					}
				}
			}
			return null;
		}

		public void Activate()
		{
			if (_activated)
			{
				return;
			}
			Alerts?.ForEach(delegate(Alert al)
			{
				if (!string.IsNullOrEmpty(al.UID))
				{
					if (!TimersModule.ModuleInstance._activeAlertIds.ContainsKey(al.UID))
					{
						al.Activate();
						TimersModule.ModuleInstance._activeAlertIds.Add(al.UID, al);
					}
				}
				else
				{
					al.Activate();
				}
			});
			Directions?.ForEach(delegate(Direction dir)
			{
				if (!string.IsNullOrEmpty(dir.UID))
				{
					if (!TimersModule.ModuleInstance._activeDirectionIds.ContainsKey(dir.UID))
					{
						dir.Activate();
						TimersModule.ModuleInstance._activeDirectionIds.Add(dir.UID, dir);
					}
				}
				else
				{
					dir.Activate();
				}
			});
			Markers?.ForEach(delegate(Marker mark)
			{
				if (!string.IsNullOrEmpty(mark.UID))
				{
					if (!TimersModule.ModuleInstance._activeMarkerIds.ContainsKey(mark.UID))
					{
						mark.Activate();
						TimersModule.ModuleInstance._activeMarkerIds.Add(mark.UID, mark);
					}
				}
				else
				{
					mark.Activate();
				}
			});
			Sounds.ForEach(delegate(Sound voice)
			{
				voice.Activate();
			});
			_activated = true;
		}

		public void Deactivate()
		{
			if (!_activated)
			{
				return;
			}
			Stop();
			Alerts?.ForEach(delegate(Alert al)
			{
				if (!string.IsNullOrEmpty(al.UID) && TimersModule.ModuleInstance._activeAlertIds.TryGetValue(al.UID, out var value3) && value3 == al)
				{
					TimersModule.ModuleInstance._activeAlertIds.Remove(al.UID);
				}
				al.Deactivate();
			});
			Directions?.ForEach(delegate(Direction dir)
			{
				if (!string.IsNullOrEmpty(dir.UID) && TimersModule.ModuleInstance._activeDirectionIds.TryGetValue(dir.UID, out var value2) && value2 == dir)
				{
					TimersModule.ModuleInstance._activeDirectionIds.Remove(dir.UID);
				}
				dir.Deactivate();
			});
			Markers?.ForEach(delegate(Marker mark)
			{
				if (!string.IsNullOrEmpty(mark.UID) && TimersModule.ModuleInstance._activeMarkerIds.TryGetValue(mark.UID, out var value) && value == mark)
				{
					TimersModule.ModuleInstance._activeMarkerIds.Remove(mark.UID);
				}
				mark.Deactivate();
			});
			Sounds?.ForEach(delegate(Sound voice)
			{
				voice.Deactivate();
			});
			_activated = false;
		}

		public void WaitForStart()
		{
			StartTrigger?.Enable();
			Logger.Warn(Name + " phase waiting");
		}

		public void Start()
		{
			if (!Active && _activated)
			{
				StartTrigger?.Reset();
				StartTrigger?.Disable();
				FinishTrigger?.Enable();
				Active = true;
				Logger.Warn(Name + " phase starting");
			}
		}

		public void Stop()
		{
			if (!Active)
			{
				return;
			}
			StartTrigger?.Reset();
			StartTrigger?.Disable();
			FinishTrigger?.Reset();
			FinishTrigger?.Disable();
			Alerts?.ForEach(delegate(Alert al)
			{
				if (!string.IsNullOrEmpty(al.UID) && TimersModule.ModuleInstance._activeAlertIds.TryGetValue(al.UID, out var value3) && value3 == al)
				{
					TimersModule.ModuleInstance._activeAlertIds.Remove(al.UID);
				}
				al.Stop();
			});
			Directions?.ForEach(delegate(Direction dir)
			{
				if (!string.IsNullOrEmpty(dir.UID) && TimersModule.ModuleInstance._activeDirectionIds.TryGetValue(dir.UID, out var value2) && value2 == dir)
				{
					TimersModule.ModuleInstance._activeDirectionIds.Remove(dir.UID);
				}
				dir.Stop();
			});
			Markers?.ForEach(delegate(Marker mark)
			{
				if (!string.IsNullOrEmpty(mark.UID) && TimersModule.ModuleInstance._activeMarkerIds.TryGetValue(mark.UID, out var value) && value == mark)
				{
					TimersModule.ModuleInstance._activeMarkerIds.Remove(mark.UID);
				}
				mark.Stop();
			});
			Sounds?.ForEach(delegate(Sound voice)
			{
				voice.Stop();
			});
			Active = false;
			Logger.Warn(Name + " phase stopping");
		}

		public void Update(float elapsedTime)
		{
			Alerts?.ForEach(delegate(Alert al)
			{
				al.Update(elapsedTime);
			});
			Directions?.ForEach(delegate(Direction dir)
			{
				dir.Update(elapsedTime);
			});
			Markers?.ForEach(delegate(Marker mark)
			{
				mark.Update(elapsedTime);
			});
			Sounds?.ForEach(delegate(Sound voice)
			{
				voice.Update(elapsedTime);
			});
		}

		public void Dispose()
		{
			Deactivate();
			Alerts?.ForEach(delegate(Alert al)
			{
				al?.Dispose();
			});
			Directions?.Clear();
			Markers?.Clear();
			Alerts?.Clear();
			Sounds?.Clear();
		}
	}
}
