using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Charr.Timers_BlishHUD.Pathing.Content;
using Charr.Timers_BlishHUD.Pathing.Entities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Direction
	{
		private TrailPathable _trail;

		private bool _activated;

		private bool _showDirection = true;

		[JsonProperty("uid")]
		public string UID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; } = "Unnamed Direction";


		[JsonProperty("position")]
		public List<float> Position { get; set; }

		[JsonProperty("destination")]
		public List<float> Destination
		{
			set
			{
				Position = value;
			}
		}

		[JsonProperty("duration")]
		public float Duration { get; set; } = 10f;


		[JsonProperty("opacity")]
		public float Opacity { get; set; } = 0.8f;


		[JsonProperty("animSpeed")]
		public float AnimSpeed { get; set; } = 1f;


		[JsonProperty("texture")]
		public string TextureString { get; set; }

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

		public bool ShowDirection
		{
			get
			{
				return _showDirection;
			}
			set
			{
				if (_trail != null)
				{
					_trail.ShouldShow = value;
				}
				_showDirection = value;
			}
		}

		public string Initialize(PathableResourceManager resourceManager)
		{
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			if (Position == null || Position.Count != 3)
			{
				return Name + " invalid position property";
			}
			if (Timestamps == null || Timestamps.Count == 0)
			{
				return Name + " invalid timestamps property";
			}
			if (string.IsNullOrEmpty(TextureString))
			{
				return Name + " invalid texture property";
			}
			_trail = new TrailPathable
			{
				Opacity = Opacity,
				AnimationSpeed = AnimSpeed,
				TrailTexture = AsyncTexture2D.op_Implicit(resourceManager.LoadTexture(TextureString)),
				PointA = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				PointB = new Vector3(Position[0], Position[1], Position[2]),
				ShouldShow = ShowDirection
			};
			_trail.Visible = false;
			return null;
		}

		public void Activate()
		{
			if (_trail != null && !_activated)
			{
				GameService.Graphics.get_World().AddEntity((IEntity)(object)_trail);
				_activated = true;
			}
		}

		public void Deactivate()
		{
			if (_trail != null && _activated)
			{
				GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_trail);
				_activated = false;
			}
		}

		public void Stop()
		{
			if (_trail != null)
			{
				_trail.Visible = false;
			}
		}

		public void Update(float elapsedTime)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (_trail == null || !_activated)
			{
				return;
			}
			bool enabled = false;
			foreach (float time in Timestamps)
			{
				if (elapsedTime >= time && elapsedTime <= time + Duration)
				{
					enabled = true;
					_trail.Visible = true;
					_trail.PointA = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
					break;
				}
			}
			if (!enabled)
			{
				Stop();
			}
		}
	}
}
