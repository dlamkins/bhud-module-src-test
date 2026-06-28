using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public sealed class TruePerfectionConfettiOverlay : Control, IDisposable
	{
		private struct ConfettiParticle
		{
			public Vector2 Position;

			public Vector2 Velocity;

			public Color Color;

			public float Life;

			public float Size;

			public float Spin;

			public float SpinSpeed;
		}

		private const int ParticleCount = 240;

		private const float DurationSeconds = 4f;

		private const float Gravity = 360f;

		private static TruePerfectionConfettiOverlay? _active;

		private static bool _pendingBurst;

		private static ScoreModel? _pendingScoreBand;

		private static Vector2? _pendingOrigin;

		private readonly List<ConfettiParticle> _particles = new List<ConfettiParticle>();

		private readonly Random _random = new Random();

		private readonly bool _useRainbow;

		private readonly Color _baseColor;

		private readonly Vector2 _burstOrigin;

		private float _elapsed;

		public static void RequestBurst(ScoreModel scoreBand)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (scoreBand.Vfx)
			{
				_pendingScoreBand = scoreBand;
				Point position = GameService.Input.get_Mouse().get_Position();
				_pendingOrigin = ((Point)(ref position)).ToVector2();
				_pendingBurst = true;
			}
		}

		public static void ProcessPendingBurst(GameTime gameTime)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (_pendingBurst && _pendingScoreBand != null)
			{
				ScoreModel? pendingScoreBand = _pendingScoreBand;
				Point position = GameService.Input.get_Mouse().get_Position();
				((Point)(ref position)).ToVector2();
				_pendingBurst = false;
				_pendingScoreBand = null;
				Burst(pendingScoreBand);
			}
		}

		private static void Burst(ScoreModel scoreBand)
		{
			_active?.Dispose();
			_active = new TruePerfectionConfettiOverlay(scoreBand);
		}

		private TruePerfectionConfettiOverlay(ScoreModel scoreBand)
			: this()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			_useRainbow = scoreBand.IsRainbow;
			_baseColor = (_useRainbow ? Color.get_White() : ScoreModel.ParseHexColor(scoreBand.Color));
			Point position = GameService.Input.get_Mouse().get_Position();
			_burstOrigin = ((Point)(ref position)).ToVector2();
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_ZIndex(2147483642);
			((Control)this).set_Visible(true);
			SpawnBurst();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			float dt = (float)gameTime.get_ElapsedGameTime().TotalSeconds;
			_elapsed += dt;
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			for (int i = _particles.Count - 1; i >= 0; i--)
			{
				ConfettiParticle particle = _particles[i];
				particle.Velocity.Y += 360f * dt;
				ref Vector2 position = ref particle.Position;
				position += particle.Velocity * dt;
				particle.Spin += particle.SpinSpeed * dt;
				particle.Life -= dt * 0.55f;
				if (particle.Life <= 0f)
				{
					_particles.RemoveAt(i);
				}
				else
				{
					_particles[i] = particle;
				}
			}
			if (_elapsed >= 4f && _particles.Count == 0)
			{
				Dispose();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			if (_particles.Count == 0)
			{
				return;
			}
			Texture2D pixel = Textures.get_Pixel();
			Rectangle rect = default(Rectangle);
			foreach (ConfettiParticle particle in _particles)
			{
				int size = (int)Math.Max(2f, particle.Size * particle.Life);
				((Rectangle)(ref rect))._002Ector((int)particle.Position.X - size / 2, (int)particle.Position.Y - size / 2, size, size);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, rect, particle.Color * MathHelper.Clamp(particle.Life, 0f, 1f));
			}
		}

		private void SpawnBurst()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			Vector2 origin = _burstOrigin;
			_particles.Clear();
			for (int i = 0; i < 240; i++)
			{
				float angle = (float)(_random.NextDouble() * Math.PI * 2.0);
				float speed = 120f + (float)_random.NextDouble() * 280f;
				_particles.Add(new ConfettiParticle
				{
					Position = origin + new Vector2((float)(_random.NextDouble() * 20.0 - 10.0), (float)(_random.NextDouble() * 20.0 - 10.0)),
					Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed,
					Color = (_useRainbow ? RainbowColors.ColorFromHue((float)_random.NextDouble()) : (_baseColor * (0.75f + (float)_random.NextDouble() * 0.25f))),
					Life = 0.85f + (float)_random.NextDouble() * 0.15f,
					Size = 4f + (float)_random.NextDouble() * 5f,
					Spin = (float)(_random.NextDouble() * Math.PI * 2.0),
					SpinSpeed = (float)(_random.NextDouble() * 8.0 - 4.0)
				});
			}
		}

		public void Dispose()
		{
			if (_active == this)
			{
				_active = null;
			}
			((Control)this).DisposeControl();
		}

		protected override void DisposeControl()
		{
			((Control)this).set_Parent((Container)null);
			((Control)this).DisposeControl();
		}
	}
}
