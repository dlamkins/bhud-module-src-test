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
	public class RaceRunFx : IDisposable
	{
		public const double MinCheckpointAlpha = 0.1;

		private readonly RaceDrawer Drawer;

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly SoundEffect _raceStartSfx;

		private readonly SoundEffect _raceFinishSfx;

		private readonly SoundEffect _raceCancelSfx;

		private readonly SoundEffect _checkpointSfx;

		public Race Race { get; private set; }

		public RaceRouteData Route { get; private set; }

		public List<RacePoint> Checkpoints { get; private set; }

		public int LoopStartIndex { get; private set; }

		public List<(int i, RacePoint point)> RoadPoints { get; private set; }

		public bool OfficialPoints
		{
			get
			{
				if (Race?.IsOfficial ?? false)
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

		public RaceRunFx(RaceDrawer drawer, Race race)
		{
			Drawer = drawer;
			_raceStartSfx = _dc.Add<SoundEffect>(RacingModule.ContentsManager.GetSound("SFX/RaceStart.wav"));
			_raceFinishSfx = _dc.Add<SoundEffect>(RacingModule.ContentsManager.GetSound("SFX/RaceFinish.wav"));
			_raceCancelSfx = _dc.Add<SoundEffect>(RacingModule.ContentsManager.GetSound("SFX/RaceCancel.wav"));
			_checkpointSfx = _dc.Add<SoundEffect>(RacingModule.ContentsManager.GetSound("SFX/Checkpoint.wav"));
			_dc.Add(RacingModule.Settings.SfxVolumeMultiplier.OnChangedAndNow(delegate(float v)
			{
				Volume = v;
			}));
			_dc.Add(RacingModule.Settings.ShownCheckpoints.OnChangedAndNow(delegate(int v)
			{
				ShownCheckpoints = v;
			}));
			_dc.Add(RacingModule.Settings.ShowGuides.OnChangedAndNow(delegate(bool v)
			{
				ShowGuides = v;
			}));
			_dc.Add(RacingModule.Settings.NormalizedOfficialCheckpoints.OnChangedAndNow(delegate(bool v)
			{
				NormalizedOfficialCheckpoints = v;
			}));
			Race = race;
			Route = new RaceRouteData(race);
			Checkpoints = new List<RacePoint>();
			RoadPoints = new List<(int, RacePoint)>();
			LoadRace();
		}

		public void LoadRace(Race race)
		{
			Race = race;
			Route = new RaceRouteData(race);
			RoadPoints = new List<(int, RacePoint)>();
			LoadRace();
		}

		private void LoadRace()
		{
			Checkpoints = Race.Checkpoints.ToList();
			LoopStartIndex = Race.LoopStartIndex;
			int i = 0;
			foreach (RacePoint point in Race.RoadPoints)
			{
				RoadPoints.Add((i, point));
				if (point.IsCheckpoint)
				{
					i++;
				}
			}
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

		public void DrawStart(SpriteBatch spriteBatch)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (Race != null)
			{
				Drawer.DrawRacePoint(spriteBatch, Checkpoints[0], RaceDrawer.CheckpointColor, flat: true);
			}
		}

		public void DrawPoints(SpriteBatch spriteBatch, int reachedPoints, int laps = 1)
		{
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			(int, int) tuple = Route.Progress(reachedPoints);
			int currentLap = tuple.Item1;
			int pointIndex = tuple.Item2;
			RacePoint lastCheckpoint = Checkpoints[Checkpoints.Count - 1];
			int shown = 0;
			foreach (var item3 in RoadPoints.Concat<(int, RacePoint)>(RoadPoints.SkipWhile<(int, RacePoint)>(((int i, RacePoint point) p) => p.i < LoopStartIndex)).SkipWhile<(int, RacePoint)>(((int i, RacePoint point) p) => p.i != pointIndex).By2())
			{
				(int, RacePoint) item = item3.Item1;
				(int, RacePoint) item2 = item3.Item2;
				int i = item.Item1;
				RacePoint point = item.Item2;
				RacePoint next = item2.Item2;
				bool num = point == lastCheckpoint;
				bool isFinishLine = num && currentLap + 1 >= laps;
				Color color = (num ? RaceDrawer.FinalCheckpointColor : ((i <= pointIndex) ? RaceDrawer.NextCheckpointColor : RaceDrawer.CheckpointColor));
				if (ShownCheckpoints != 1)
				{
					((Color)(ref color)).set_A((byte)(MathUtils.Scale(shown, 0.0, ShownCheckpoints, 1.0, 0.1) * 255.0));
				}
				if (point.Type == RacePointType.Guide && ShowGuides && i - 1 <= pointIndex)
				{
					Drawer.DrawRaceArrow(spriteBatch, point.Position, point.Radius, next.Position, color);
				}
				else if (point.IsCheckpoint)
				{
					shown++;
					Drawer.DrawRacePoint(spriteBatch, point.Position, (OfficialPoints && i >= LoopStartIndex) ? 7.62f : point.Radius, isFinishLine ? null : new Vector3?(next.Position), color);
				}
				if (isFinishLine || shown >= ShownCheckpoints)
				{
					break;
				}
			}
		}

		public void Dispose()
		{
			_dc.Dispose();
		}
	}
}
