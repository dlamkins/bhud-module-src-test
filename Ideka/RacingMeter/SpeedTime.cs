using System;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class SpeedTime : ValueTime<PosSnapshot>
	{
		public float Speed2D { get; }

		public float Speed3D { get; }

		public float UpSpeed { get; }

		public float DownSpeed { get; }

		public float CamMovementYaw { get; private set; }

		public float FwdMovementYaw { get; private set; }

		public float SlopeAngle { get; }

		public SpeedTime()
		{
		}

		public SpeedTime(PosSnapshot first, PosSnapshot second, TimeSpan deltaTime, bool isDouble = false)
			: base(first, second, deltaTime, isDouble)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			float t = ValueTime.TimeMagnitude(base.DeltaTime);
			float distance2D = Vector2.Distance(base.First.Inches.ToVector2(), base.Second.Inches.ToVector2());
			Speed2D = distance2D / t;
			Speed3D = Vector3.Distance(base.First.Inches, second.Inches) / t;
			float height = base.Second.HeightIn - base.First.HeightIn;
			UpSpeed = height / t;
			DownSpeed = UpSpeed * -1f;
			if (distance2D != 0f)
			{
				SlopeAngle = (float)(Math.Atan(height / distance2D) * (1.0 / Math.PI));
			}
			Update();
		}

		public void Update()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 movement = base.Second.Inches - base.First.Inches;
			if (movement != Vector3.get_Zero())
			{
				CamMovementYaw = YawBetween(base.Second.CameraFront, movement);
				FwdMovementYaw = YawBetween(base.Second.Front, movement);
			}
		}

		public static float YawBetween(Vector3 a, Vector3 b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			return (float)(Math.Atan2(a.X * b.Y - b.X * a.Y, a.X * b.X + a.Y * b.Y) * (1.0 / Math.PI));
		}
	}
}
