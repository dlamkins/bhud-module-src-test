using System.Collections.Generic;
using Blish_HUD;
using Charr.Timers_BlishHUD.Pathing.Content;
using Charr.Timers_BlishHUD.Pathing.Entities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Timers
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Direction : Timer
	{
		private TrailPathable _trail;

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

		public bool ShowDirection
		{
			get
			{
				return _showTimer;
			}
			set
			{
				if (_trail != null)
				{
					_trail.ShouldShow = value;
				}
				_showTimer = value;
			}
		}

		public Direction()
		{
			base.Name = "Unnamed Direction";
			_showTimer = true;
		}

		public string Initialize(PathableResourceManager resourceManager)
		{
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			if (Position == null || Position.Count != 3)
			{
				return base.Name + " invalid position property";
			}
			if (base.Timestamps == null || base.Timestamps.Count == 0)
			{
				return base.Name + " invalid timestamps property";
			}
			if (string.IsNullOrEmpty(TextureString))
			{
				return base.Name + " invalid texture property";
			}
			_trail = new TrailPathable
			{
				Opacity = Opacity,
				AnimationSpeed = AnimSpeed,
				TrailTexture = resourceManager.LoadTexture(TextureString),
				PointA = GameService.Gw2Mumble.PlayerCharacter.Position,
				PointB = new Vector3(Position[0], Position[1], Position[2]),
				ShouldShow = ShowDirection
			};
			_trail.Visible = false;
			return null;
		}

		public override void Activate()
		{
			if (_trail != null && !_activated)
			{
				GameService.Graphics.World.AddEntity(_trail);
				_activated = true;
			}
		}

		public override void Deactivate()
		{
			if (_trail != null && _activated)
			{
				GameService.Graphics.World.RemoveEntity(_trail);
				_activated = false;
			}
		}

		public override void Stop()
		{
			if (_trail != null)
			{
				_trail.Visible = false;
			}
		}

		public override void Update(float elapsedTime)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (_trail == null || !_activated)
			{
				return;
			}
			bool enabled = false;
			foreach (float time in base.Timestamps)
			{
				if (elapsedTime >= time && elapsedTime <= time + Duration)
				{
					enabled = true;
					_trail.Visible = true;
					_trail.PointA = GameService.Gw2Mumble.PlayerCharacter.Position;
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
