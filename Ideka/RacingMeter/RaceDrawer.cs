using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RaceDrawer : IDisposable
	{
		public const double MinCheckpointAlpha = 0.1;

		private readonly RaceHolder Holder;

		private readonly SoundEffect _raceStartSfx;

		private readonly SoundEffect _raceFinishSfx;

		private readonly SoundEffect _raceCancelSfx;

		private readonly SoundEffect _checkpointSfx;

		public Race Race { get; private set; }

		public RaceRouteData Route { get; private set; }

		public List<RacePoint> Checkpoints { get; private set; }

		public int LoopStartIndex { get; private set; }

		public List<(int i, RacePoint point)> RoadPoints { get; private set; }

		public bool IsReady
		{
			get
			{
				if (Race != null)
				{
					return Checkpoints?.Any() ?? false;
				}
				return false;
			}
		}

		public bool OfficialPoints
		{
			get
			{
				if (Race != null && Race.IsOfficial)
				{
					return NormalizedOfficialCheckpoints;
				}
				return false;
			}
		}

		public float Volume { get; set; }

		public int ShownCheckpoints { get; set; }

		public bool ShowGuides { get; set; }

		public bool NormalizedOfficialCheckpoints { get; set; }

		public RaceDrawer(RaceHolder holder)
		{
			Holder = holder;
			_raceStartSfx = RacingModule.ContentsManager.GetSound("SFX/RaceStart.wav");
			_raceFinishSfx = RacingModule.ContentsManager.GetSound("SFX/RaceFinish.wav");
			_raceCancelSfx = RacingModule.ContentsManager.GetSound("SFX/RaceCancel.wav");
			_checkpointSfx = RacingModule.ContentsManager.GetSound("SFX/Checkpoint.wav");
			RacingModule.Settings.SfxVolumeMultiplier.OnChangedAndNow(delegate(float v)
			{
				Volume = v;
			});
			RacingModule.Settings.ShownCheckpoints.OnChangedAndNow(delegate(int v)
			{
				ShownCheckpoints = v;
			});
			RacingModule.Settings.ShowGuides.OnChangedAndNow(delegate(bool v)
			{
				ShowGuides = v;
			});
			RacingModule.Settings.NormalizedOfficialCheckpoints.OnChangedAndNow(delegate(bool v)
			{
				NormalizedOfficialCheckpoints = v;
			});
		}

		private void Play(SoundEffect sfx, bool forcePlay)
		{
			if (!OfficialPoints || forcePlay)
			{
				sfx.Play(GameService.GameIntegration.get_Audio().get_Volume() * Volume, 0f, 0f);
			}
		}

		public void StartSfx(bool forcePlay)
		{
			Play(_raceStartSfx, forcePlay);
		}

		public void FinishSfx(bool forcePlay)
		{
			Play(_raceFinishSfx, forcePlay);
		}

		public void CancelSfx(bool forcePlay)
		{
			Play(_raceCancelSfx, forcePlay);
		}

		public void CheckpointSfx(bool forcePlay)
		{
			Play(_checkpointSfx, forcePlay);
		}

		public void LoadRace(Race race)
		{
			Checkpoints = null;
			LoopStartIndex = 0;
			RoadPoints = null;
			Race = race;
			Route = ((race == null) ? null : new RaceRouteData(race));
			if (race == null)
			{
				return;
			}
			Checkpoints = race.Checkpoints.ToList();
			LoopStartIndex = race.LoopStartIndex;
			RoadPoints = new List<(int, RacePoint)>();
			int i = 0;
			foreach (RacePoint point in race.RoadPoints)
			{
				RoadPoints.Add((i, point));
				if (point.IsCheckpoint)
				{
					i++;
				}
			}
		}

		public void DrawStart(SpriteBatch spriteBatch)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (Race != null)
			{
				Holder.DrawRacePoint(spriteBatch, Checkpoints[0], RaceHolder.CheckpointColor, flat: true);
			}
		}

		public void DrawPoints(SpriteBatch spriteBatch, int reachedPoints, int laps = 1)
		{
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			if (Race == null)
			{
				return;
			}
			(int, int) tuple = Route.Progress(reachedPoints);
			int currentLap = tuple.Item1;
			int pointIndex = tuple.Item2;
			RacePoint lastCheckpoint = Checkpoints[Checkpoints.Count - 1];
			int shown = 0;
			foreach (var item3 in RoadPoints.Concat(RoadPoints.SkipWhile(((int i, RacePoint point) p) => p.i < LoopStartIndex)).SkipWhile(((int i, RacePoint point) p) => p.i != pointIndex).By2())
			{
				(int, RacePoint) item = item3.Item1;
				(int i, RacePoint point) item2 = item3.Item2;
				int i = item.Item1;
				RacePoint point = item.Item2;
				RacePoint next = item2.point;
				bool num = point == lastCheckpoint;
				bool isFinishLine = num && currentLap + 1 >= laps;
				Color color = (num ? RaceHolder.FinalCheckpointColor : ((i <= pointIndex) ? RaceHolder.NextCheckpointColor : RaceHolder.CheckpointColor));
				if (ShownCheckpoints != 1)
				{
					((Color)(ref color)).set_A((byte)(MathUtils.Scale(shown, 0.0, ShownCheckpoints, 1.0, 0.1) * 255.0));
				}
				if (point.Type == RacePointType.Guide && ShowGuides && i - 1 <= pointIndex)
				{
					Holder.DrawRaceArrow(spriteBatch, point.Position, point.Radius, next.Position, color);
				}
				else if (point.IsCheckpoint)
				{
					shown++;
					Holder.DrawRacePoint(spriteBatch, point.Position, (OfficialPoints && i >= LoopStartIndex) ? 7.62f : point.Radius, isFinishLine ? null : new Vector3?(next.Position), color);
				}
				if (isFinishLine || shown >= ShownCheckpoints)
				{
					break;
				}
			}
		}

		public void Dispose()
		{
			SoundEffect raceStartSfx = _raceStartSfx;
			if (raceStartSfx != null)
			{
				raceStartSfx.Dispose();
			}
			SoundEffect raceFinishSfx = _raceFinishSfx;
			if (raceFinishSfx != null)
			{
				raceFinishSfx.Dispose();
			}
			SoundEffect raceCancelSfx = _raceCancelSfx;
			if (raceCancelSfx != null)
			{
				raceCancelSfx.Dispose();
			}
			SoundEffect checkpointSfx = _checkpointSfx;
			if (checkpointSfx != null)
			{
				checkpointSfx.Dispose();
			}
		}
	}
}
