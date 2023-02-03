using System;
using Blish_HUD;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class PosSnapshot : ITimedMeasure
	{
		public static readonly PosSnapshot Empty = new PosSnapshot(TimeSpan.Zero);

		public Vector3 Meters { get; private set; }

		public Vector3 Front { get; private set; }

		public Vector3 CameraMeters { get; private set; }

		public Vector3 CameraFront { get; private set; }

		public MountType Mount { get; private set; }

		public float CameraPitch => (float)(Math.Asin(CameraFront.Z) * (1.0 / Math.PI));

		public float HeightM => Meters.Z;

		public Vector3 Inches => Meters * 39.37008f;

		public float HeightIn => HeightM * 39.37008f;

		public TimeSpan Time { get; private set; }

		public TimeSpan LastUpdate { get; private set; }

		public PosSnapshot(PosSnapshot source)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Meters = source.Meters;
			Front = source.Front;
			CameraMeters = source.CameraMeters;
			CameraFront = source.CameraFront;
			Mount = source.Mount;
			Time = source.Time;
			LastUpdate = source.LastUpdate;
		}

		public PosSnapshot(TimeSpan time)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Meters = Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_AvatarPosition());
			Time = time;
			Update(time);
		}

		public void Update(TimeSpan time)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			Front = Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_AvatarFront());
			CameraMeters = Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_CameraPosition());
			CameraFront = Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_CameraFront());
			Mount = GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount();
			LastUpdate = time;
		}
	}
}
