using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.HitboxView
{
	public class HitboxEntity : IEntity, IUpdatable, IRenderable3D, IDisposable
	{
		private class TimePos
		{
			public readonly TimeSpan Time;

			public readonly Vector3 Position;

			public readonly Vector3 Forward;

			public TimePos(TimeSpan time, Vector3 position, Vector3 forward)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				Time = time;
				Position = position;
				Forward = forward;
			}

			public TimePos(Vector3 position, Vector3 forward)
				: this(default(TimeSpan), position, forward)
			{
			}//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)


			public TimePos(TimeSpan time)
				: this(time, PlayerPosition, PlayerForward)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)


			public static bool AreEquivalent(TimePos a, TimePos b)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (a.Position == b.Position)
				{
					return a.Forward == b.Forward;
				}
				return false;
			}

			public static TimePos Lerp(TimePos a, TimePos b, TimeSpan delay, TimeSpan time)
			{
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				double start = (a.Time + delay).TotalMilliseconds;
				double totalMilliseconds = time.TotalMilliseconds;
				double end = (b.Time + delay).TotalMilliseconds;
				float p = (float)((totalMilliseconds - start) / (end - start));
				if (!float.IsNaN(p) && !float.IsInfinity(p))
				{
					return new TimePos(Vector3.Lerp(a.Position, b.Position, p), Vector3.Lerp(a.Forward, b.Forward, p));
				}
				return a;
			}
		}

		private static readonly TimeSpan SmoothingCompensation = TimeSpan.FromMilliseconds(16.0);

		private Color _color = Color.get_White();

		private TimeSpan _delay;

		private readonly Quad _quad;

		private readonly Texture2D _texture;

		private BasicEffect _effect;

		private int _lastTick;

		private TimePos _lastPopped;

		private TimePos _lastQueued;

		private readonly Queue<TimePos> _timePosQueue = new Queue<TimePos>();

		public float DrawOrder => 100f;

		public bool Smoothing { get; set; } = true;


		public Color Color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _color;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				_color = value;
				if (_effect != null)
				{
					_effect.set_DiffuseColor(((Color)(ref _color)).ToVector3());
				}
			}
		}

		public TimeSpan Delay
		{
			get
			{
				return _delay + (Smoothing ? SmoothingCompensation : TimeSpan.Zero);
			}
			set
			{
				_delay = value;
			}
		}

		public int Ping
		{
			get
			{
				return (int)Delay.TotalMilliseconds;
			}
			set
			{
				Delay = TimeSpan.FromMilliseconds(value);
			}
		}

		public Vector3 Position { get; private set; }

		public Vector3 Forward { get; private set; }

		public bool IsDisposed { get; private set; }

		private static Vector3 PlayerPosition => Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_AvatarPosition());

		private static Vector3 PlayerForward => Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_AvatarFront());

		public HitboxEntity()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			_quad = new Quad(Vector3.get_Zero(), Vector3.get_Backward(), Vector3.get_Up(), 1f, 1f);
			_texture = HitboxModule.ContentsManager.GetTexture("Hitbox.png");
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Expected O, but got Unknown
				if (_effect == null && !IsDisposed)
				{
					BasicEffect val = new BasicEffect(graphicsDevice);
					val.set_Texture(_texture);
					val.set_TextureEnabled(true);
					Color color = Color;
					val.set_DiffuseColor(((Color)(ref color)).ToVector3());
					_effect = val;
				}
			});
			Reset();
		}

		public void Reset()
		{
			_lastQueued = null;
			_lastPopped = new TimePos(TimeSpan.Zero);
			_timePosQueue.Clear();
		}

		public void Update(GameTime gameTime)
		{
			if (GameService.Gw2Mumble.get_RawClient().get_Tick() > _lastTick)
			{
				_lastTick = GameService.Gw2Mumble.get_RawClient().get_Tick();
				TimePos newTimePos = new TimePos(gameTime.get_TotalGameTime());
				if (_lastQueued == null || !TimePos.AreEquivalent(_lastQueued, newTimePos))
				{
					_lastQueued = new TimePos(gameTime.get_TotalGameTime());
					_timePosQueue.Enqueue(_lastQueued);
				}
			}
			while (_timePosQueue.Any() && _timePosQueue.Peek().Time + Delay <= gameTime.get_TotalGameTime())
			{
				_lastPopped = _timePosQueue.Dequeue();
			}
			if (!Smoothing || !_timePosQueue.Any())
			{
				apply(_lastPopped);
			}
			else
			{
				apply(TimePos.Lerp(_lastPopped, _timePosQueue.Peek(), Delay, gameTime.get_TotalGameTime()));
			}
			void apply(TimePos timePos)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				Position = timePos.Position;
				Forward = timePos.Forward;
			}
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			if (_effect == null)
			{
				return;
			}
			_effect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			_effect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateTranslation(Position);
			Vector3 t = ((Matrix)(ref worldMatrix)).get_Translation();
			worldMatrix *= Matrix.CreateRotationZ(0f - (float)Math.Atan2(Forward.X, Forward.Y));
			((Matrix)(ref worldMatrix)).set_Translation(t);
			_effect.set_World(worldMatrix);
			Enumerator enumerator = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>((PrimitiveType)0, _quad.Vertices, 0, 4, _quad.Indexes, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		public void Dispose()
		{
			IsDisposed = true;
			Texture2D texture = _texture;
			if (texture != null)
			{
				((GraphicsResource)texture).Dispose();
			}
			BasicEffect effect = _effect;
			if (effect != null)
			{
				((GraphicsResource)effect).Dispose();
			}
		}
	}
}
