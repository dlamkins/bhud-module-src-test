using System.Collections.Generic;
using Blish_HUD;
using Charr.Timers_BlishHUD.Pathing.Content;
using Charr.Timers_BlishHUD.Pathing.Entities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models.Timers
{
	public class Marker : Timer
	{
		private MarkerPathable _markerPathable;

		[JsonProperty("position")]
		public List<float> Position { get; set; }

		[JsonProperty("rotation")]
		public List<float> Rotation { get; set; }

		[JsonProperty("duration")]
		public float Duration { get; set; } = 10f;


		[JsonProperty("opacity")]
		public float Opacity { get; set; } = 0.8f;


		[JsonProperty("size")]
		public float Size { get; set; } = 1f;


		[JsonProperty("texture")]
		public string TextureString { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("fadeCenter")]
		public bool FadeCenter { get; set; } = true;


		public bool ShowMarker
		{
			get
			{
				return _showTimer;
			}
			set
			{
				if (_markerPathable != null)
				{
					_markerPathable.ShouldShow = value;
				}
				_showTimer = value;
			}
		}

		public Marker()
		{
			base.Name = "Unnamed Marker";
			_showTimer = true;
		}

		public string Initialize(PathableResourceManager resourceManager)
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
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
			_markerPathable = new MarkerPathable
			{
				FadeCenter = FadeCenter,
				Opacity = Opacity,
				Rotation = (Vector3)((Rotation != null && Rotation.Count == 3) ? new Vector3(Rotation[0], Rotation[1], Rotation[2]) : Vector3.get_Zero()),
				Texture = resourceManager.LoadTexture(TextureString),
				Position = new Vector3(Position[0], Position[1], Position[2]),
				Size = new Vector2(Size, Size),
				BasicTitleText = Text,
				ShouldShow = ShowMarker
			};
			_markerPathable.Visible = false;
			return null;
		}

		public override void Activate()
		{
			if (_markerPathable != null && !_activated)
			{
				GameService.Graphics.World.AddEntity(_markerPathable);
				_activated = true;
			}
		}

		public override void Deactivate()
		{
			if (_markerPathable != null && _activated)
			{
				GameService.Graphics.World.RemoveEntity(_markerPathable);
				_activated = false;
			}
		}

		public override void Stop()
		{
			if (_markerPathable != null)
			{
				_markerPathable.Visible = false;
			}
		}

		public override void Update(float elapsedTime)
		{
			if (_markerPathable == null || !_activated)
			{
				return;
			}
			bool enabled = false;
			foreach (float time in base.Timestamps)
			{
				if (elapsedTime >= time && elapsedTime <= time + Duration)
				{
					enabled = true;
					_markerPathable.Visible = true;
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
