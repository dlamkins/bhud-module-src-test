using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Ideka.BHUDCommon;
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
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Time = time;
				Position = position;
				Forward = forward;
				base._002Ector();
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

		private static readonly Primitive Circle = Primitive.HorizontalCircle(0.5f, 100);

		private static readonly Primitive Slice = new Primitive(new Vector3(-0.5f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0.5f, 0f)).Transformed(Matrix.CreateRotationZ(MathHelper.ToRadians(-45f)));

		private TimeSpan _delay;

		private int _lastTick;

		private TimePos _lastPopped;

		private TimePos? _lastQueued;

		private readonly Queue<TimePos> _timePosQueue = new Queue<TimePos>();

		private static readonly Dictionary<MountType, Vector2> Sizes = new Dictionary<MountType, Vector2>
		{
			[(MountType)0] = Vector2.get_One() * 1f,
			[(MountType)5] = Vector2.get_One() * 2.8f,
			[(MountType)3] = Vector2.get_One() * 2.2f,
			[(MountType)4] = Vector2.get_One() * 3.1f,
			[(MountType)1] = Vector2.get_One() * 2.2f,
			[(MountType)2] = Vector2.get_One() * 2.7f,
			[(MountType)6] = Vector2.get_One() * 2.8f,
			[(MountType)7] = Vector2.get_One() * 1.7f,
			[(MountType)8] = Vector2.get_One() * 2.7f,
			[(MountType)9] = new Vector2(3.1f, 11.1f),
			[(MountType)10] = Vector2.get_One() * 3.9f
		};

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
			((Control)this).set_ClipsBounds(false);
			_lastPopped = new TimePos(TimeSpan.Zero);
			Reset();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
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
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				Vector2 s;
				Vector2 scale = (Sizes.TryGetValue(GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount(), out s) ? s : Vector2.get_One());
				Matrix trs = Matrix.CreateScale(scale.X, scale.Y, 1f) * Matrix.CreateRotationZ(0f - (float)Math.Atan2(Forward.X, Forward.Y)) * Matrix.CreateTranslation(Position);
				Primitive.ScreenPrimitive screenPrimitive = Circle.Transformed(trs).ToScreen();
				screenPrimitive.Draw(spriteBatch, Color.get_Black(), 3f);
				screenPrimitive.Draw(spriteBatch, Color.get_White(), 2f);
				if (scale.X == scale.Y)
				{
					Primitive.ScreenPrimitive screenPrimitive2 = Slice.Transformed(trs).ToScreen();
					screenPrimitive2.Draw(spriteBatch, Color.get_Black(), 3f);
					screenPrimitive2.Draw(spriteBatch, Color.get_White(), 2f);
				}
			}
		}
	}
}
