using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.HitboxView
{
	public class HitboxDraw : Container
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

		private TimeSpan _delay;

		private readonly Primitive _circle;

		private readonly Primitive _slice;

		private int _lastTick;

		private TimePos _lastPopped;

		private TimePos _lastQueued;

		private readonly Queue<TimePos> _timePosQueue = new Queue<TimePos>();

		public float DrawOrder => 100f;

		public bool Smoothing { get; set; } = true;


		public Color Color { get; set; }

		public Color OutlineColor { get; set; }

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

		public HitboxDraw()
			: this()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ClipsBounds(false);
			_circle = Primitive.HorizontalCircle(0.5f, 100);
			_slice = new Primitive(new Vector3(-0.5f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0.5f, 0f)).Transformed(Matrix.CreateRotationZ(MathHelper.ToRadians(-45f)));
			Reset();
		}

		public void Reset()
		{
			_lastQueued = null;
			_lastPopped = new TimePos(TimeSpan.Zero);
			_timePosQueue.Clear();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
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

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			Matrix trs = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f - (float)Math.Atan2(Forward.X, Forward.Y)) * Matrix.CreateTranslation(Position);
			IEnumerable<Vector2> circle = _circle.Transformed(trs).ToScreen();
			spriteBatch.DrawPolygon(Vector2.get_Zero(), circle, Color.get_Black(), 3f);
			spriteBatch.DrawPolygon(Vector2.get_Zero(), circle, Color.get_White(), 2f);
			IEnumerable<Vector2> slice = _slice.Transformed(trs).ToScreen();
			spriteBatch.DrawPolygon(Vector2.get_Zero(), slice, Color.get_Black(), 3f, 0f, open: true);
			spriteBatch.DrawPolygon(Vector2.get_Zero(), slice, Color.get_White(), 2f, 0f, open: true);
		}
	}
}
