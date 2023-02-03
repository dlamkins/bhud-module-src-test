using System;
using System.Collections.Generic;
using System.Linq;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class GhostExtensions
	{
		public static (TimeSpan, Ghost?) Ready(FullRace fullRace, IEnumerable<PosSnapshot> initialSnapshots)
		{
			if (!initialSnapshots.Any())
			{
				return (default(TimeSpan), null);
			}
			Ghost ghost = new Ghost
			{
				RaceId = fullRace.Meta?.Id
			};
			TimeSpan timeBase = initialSnapshots.First().Time;
			foreach (PosSnapshot snapshot in initialSnapshots)
			{
				ghost.AddSnapshot(timeBase, snapshot);
			}
			return (timeBase, ghost);
		}

		public static GhostSnapshot ToGhostSnapshot(this PosSnapshot pos, TimeSpan timeBase)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			GhostSnapshot result = default(GhostSnapshot);
			result.Position = pos.Meters;
			result.Front = pos.Front;
			result.CameraPosition = pos.CameraMeters;
			result.CameraFront = pos.CameraFront;
			result.Mount = pos.Mount;
			result.Time = pos.Time - timeBase;
			return result;
		}

		public static void AddSnapshot(this Ghost ghost, TimeSpan timeBase, PosSnapshot pos)
		{
			ghost.AddSnapshot(pos.ToGhostSnapshot(timeBase));
		}

		public static void AddSnapshot(this Ghost ghost, GhostSnapshot snapshot)
		{
			if (ghost.Snapshots.Any())
			{
				GhostSnapshot last = ghost.Snapshots.Last();
				if (last.Equivalent(snapshot))
				{
					ghost.Snapshots.Remove(last);
					if (!ghost.Snapshots.Any() || !ghost.Snapshots.Last().Equivalent(snapshot))
					{
						ghost.Snapshots.Add(last);
					}
				}
			}
			ghost.Snapshots.Add(snapshot);
		}

		public static GhostSnapshot SnapshotAt(this Ghost ghost, float p)
		{
			return ghost.SnapshotAt(ghost.Time.Multiply(p));
		}

		public static GhostSnapshot SnapshotAt(this Ghost ghost, TimeSpan time, bool clamped = true)
		{
			if (!ghost.Snapshots.Any())
			{
				return default(GhostSnapshot);
			}
			if (clamped && time > ghost.Time)
			{
				time = ghost.Time;
			}
			time += ghost.Start;
			if (time <= ghost.Snapshots.First().Time)
			{
				return ghost.Snapshots.First();
			}
			if (time >= ghost.Snapshots.Last().Time)
			{
				return ghost.Snapshots.Last();
			}
			foreach (var (a, b) in ghost.Snapshots.By2())
			{
				if (!(time < a.Time) && !(time > b.Time))
				{
					GhostSnapshot result = Lerp(a, b, MathUtils.InverseLerp(a.Time.Ticks, b.Time.Ticks, time.Ticks, clamp: true));
					result.Time -= ghost.Start;
					return result;
				}
			}
			return default(GhostSnapshot);
		}

		public static GhostSnapshot Lerp(GhostSnapshot a, GhostSnapshot b, double p)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			GhostSnapshot result = default(GhostSnapshot);
			result.Position = Vector3.Lerp(a.Position, b.Position, (float)p);
			result.Front = Vector3.Lerp(a.Front, b.Front, (float)p);
			result.CameraFront = Vector3.Lerp(a.CameraFront, b.CameraFront, (float)p);
			result.Mount = ((p < 0.5) ? a.Mount : b.Mount);
			result.Time = TimeSpan.FromTicks((long)MathUtils.Lerp(a.Time.Ticks, b.Time.Ticks, p));
			return result;
		}
	}
}
