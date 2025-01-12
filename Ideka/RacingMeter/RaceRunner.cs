using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class RaceRunner : RaceDrawerWorld
	{
		private const double ResetFadeStartMetersSq = 2500.0;

		private const double ResetFadeEndMetersSq = 40000.0;

		private const int GhostSlack = 10;

		private readonly DropOutStack<PosSnapshot> PosStack = new DropOutStack<PosSnapshot>(10);

		private static readonly List<RacePoint> EmptyCheckpoints = new List<RacePoint>();

		private int _currentStep;

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly IMeasurer _measurer;

		private RaceRunFx? _runFx;

		private Ghost? _ready;

		private Ghost? _racing;

		private TimeSpan _timeBase;

		private RacePoint? _inPoint;

		private readonly AnchoredRect _hud;

		public FullRace? FullRace { get; private set; }

		public override Race? Race => FullRace?.Race;

		public FullGhost? FullGhost { get; private set; }

		public Ghost? Ghost => FullGhost?.Ghost;

		public IReadOnlyList<RacePoint> Checkpoints => _runFx?.Checkpoints ?? EmptyCheckpoints;

		public int PassedPoints
		{
			get
			{
				if (_runFx != null)
				{
					return Math.Min(Math.Max(_currentStep, TestCheckpoint), Checkpoints.Count - 1);
				}
				return 0;
			}
			private set
			{
				_currentStep = Math.Max(value, TestCheckpoint);
			}
		}

		public double Progress
		{
			get
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				if (_runFx == null)
				{
					return 0.0;
				}
				return _runFx!.Route.ProgressPercent(PassedPoints, _measurer.Pos.Meters);
			}
		}

		public int TestCheckpoint { get; private set; }

		public bool IsTesting => TestCheckpoint >= 0;

		public TimeSpan RaceTime
		{
			get
			{
				if (_racing != null)
				{
					if (!(_racing!.End != TimeSpan.Zero))
					{
						return _measurer.Pos.LastUpdate - _racing!.Start - _timeBase;
					}
					return _racing!.Time;
				}
				return TimeSpan.Zero;
			}
		}

		public event Action<FullRace?>? RaceLoaded;

		public event Action<FullGhost?>? GhostLoaded;

		public event Action<Race>? RaceStarted;

		public event Action<Race>? RaceCancelled;

		public event Action<Race, Ghost>? RaceFinished;

		public RaceRunner(IMeasurer measurer, int testCheckpoint = -1)
		{
			_measurer = measurer;
			TestCheckpoint = testCheckpoint;
			_hud = _dc.Add(new RunnerHUD(this));
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			_measurer.NewPosition += new Action<PosSnapshot>(NewPosition);
			_measurer.Teleported += new Action(Teleported);
		}

		public void SetRace(FullRace? fullRace)
		{
			CancelRace();
			if (fullRace != null && !fullRace!.Race.Checkpoints.Skip(1).Any())
			{
				ScreenNotification.ShowNotification(Strings.NotifyNotEnoughCheckpoints, (NotificationType)2, (Texture2D)null, 4);
				fullRace = null;
			}
			Race race = fullRace?.Race;
			if (race == null)
			{
				_runFx?.Dispose();
				_runFx = null;
			}
			else
			{
				RaceRunFx runFx = _runFx;
				if (runFx != null)
				{
					runFx.LoadRace(race);
				}
				else
				{
					_runFx = new RaceRunFx(this, race);
				}
			}
			PassedPoints = 0;
			_inPoint = null;
			FullRace = fullRace;
			this.RaceLoaded?.Invoke(fullRace);
		}

		public void SetGhost(FullGhost? fullGhost)
		{
			if (IsTesting)
			{
				fullGhost = null;
			}
			if (fullGhost != null && (FullRace == null || FullRace!.Meta.Id != fullGhost!.Ghost.RaceId))
			{
				return;
			}
			Ghost ghost = fullGhost?.Ghost;
			if (ghost != null)
			{
				Race race = FullRace?.Race;
				if (race != null)
				{
					ghost.CalculateStart(race);
					ghost.Checkpoints(race);
				}
			}
			FullGhost = fullGhost;
			this.GhostLoaded?.Invoke(fullGhost);
		}

		private void NewPosition(PosSnapshot pos)
		{
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			PosSnapshot pos2 = pos;
			if (!((Control)(object)this).IsVisible())
			{
				CancelRace();
				return;
			}
			PosStack.Push(pos2);
			RaceRunFx runFx = _runFx;
			if (runFx == null)
			{
				return;
			}
			Race race = Race;
			if (race == null)
			{
				return;
			}
			FullRace fullRace = FullRace;
			if (fullRace == null)
			{
				return;
			}
			_ready?.AddSnapshot(_timeBase, pos2);
			_racing?.AddSnapshot(_timeBase, pos2);
			if (_measurer.Speed.Speed3D == 0f)
			{
				if (_racing != null && race.ResetPoints.Any((RacePoint r) => r.Collides(pos2.Meters)))
				{
					CancelRace();
				}
				else if (_racing == null && Checkpoints[0].Collides(pos2.Meters))
				{
					(_timeBase, _ready) = GhostExtensions.Ready(fullRace, PosStack);
				}
			}
			if (_racing == null && _ready != null && !IsTesting && hasLeft(Checkpoints[0]))
			{
				StartRace();
				PassedPoints++;
				_inPoint = null;
			}
			else if (_racing == null && TestCheckpoint == PassedPoints && hasLeft(Checkpoints[PassedPoints]))
			{
				StartTesting(pos2);
				PassedPoints++;
				_inPoint = null;
			}
			else if (_racing != null)
			{
				bool entered = hasEntered(Checkpoints[PassedPoints]);
				if (PassedPoints == Checkpoints.Count - 1)
				{
					if (entered)
					{
						runFx.FinishSfx(IsTesting);
					}
					int after;
					if (IsTesting)
					{
						if (entered)
						{
							FinishTesting();
						}
					}
					else if (_racing!.Checkpoints(race, out after).Count == Checkpoints.Count && after >= 10)
					{
						FinishRace();
					}
				}
				else if (entered)
				{
					runFx.CheckpointSfx(IsTesting);
					PassedPoints++;
				}
			}
			if (_ready != null && !Checkpoints[0].Collides(pos2.Meters))
			{
				_ready = null;
			}
			bool hasEntered(RacePoint point)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				bool isIn = point.Collides(pos2.Meters);
				if (_inPoint == point)
				{
					_inPoint = (isIn ? _inPoint : null);
					return false;
				}
				_inPoint = (isIn ? point : _inPoint);
				return isIn;
			}
			bool hasLeft(RacePoint point)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				bool isIn2 = point.Collides(pos2.Meters);
				if (_inPoint == point)
				{
					_inPoint = (isIn2 ? _inPoint : null);
					return !isIn2;
				}
				_inPoint = (isIn2 ? point : _inPoint);
				return false;
			}
		}

		private void StartTesting(PosSnapshot pos)
		{
			RaceRunFx runFx = _runFx;
			if (runFx != null)
			{
				FullRace fullRace = FullRace;
				if (fullRace != null)
				{
					_ready = null;
					_racing = new Ghost(fullRace.Meta.Id);
					_timeBase = pos.Time;
					_racing.AddSnapshot(_timeBase, pos);
					runFx.StartSfx(IsTesting);
					this.RaceStarted?.Invoke(fullRace.Race);
				}
			}
		}

		private void FinishTesting()
		{
			PassedPoints = 0;
			_ready = null;
			_racing = null;
		}

		private void StartRace()
		{
			RaceRunFx runFx = _runFx;
			if (runFx == null)
			{
				return;
			}
			Race race = Race;
			if (race != null)
			{
				PassedPoints = 0;
				_racing = ((_ready?.CalculateStart(race) ?? false) ? _ready : null);
				_ready = null;
				if (_racing == null)
				{
					ScreenNotification.ShowNotification(Strings.NotifyRaceStartFailed, (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				runFx.StartSfx(IsTesting);
				this.RaceStarted?.Invoke(race);
			}
		}

		private void FinishRace()
		{
			Race race = Race;
			if (race != null)
			{
				Ghost racing = _racing;
				if (racing != null)
				{
					PassedPoints = 0;
					_ready = null;
					_racing = null;
					this.RaceFinished?.Invoke(race, racing);
				}
			}
		}

		public void CancelRace()
		{
			if (_racing == null)
			{
				return;
			}
			RaceRunFx runFx = _runFx;
			if (runFx != null)
			{
				Race race = Race;
				if (race != null)
				{
					runFx.CancelSfx(IsTesting);
					PassedPoints = 0;
					_ready = null;
					_racing = null;
					this.RaceCancelled?.Invoke(race);
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			_hud.Update(gameTime);
		}

		protected override void DrawRaceToWorld(SpriteBatch spriteBatch)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			RaceRunFx runFx = _runFx;
			if (runFx == null)
			{
				return;
			}
			Race race = Race;
			if (race == null)
			{
				return;
			}
			foreach (RacePoint reset in race.ResetPoints)
			{
				Color color = RaceDrawer.ResetColor;
				float distanceSq = Vector3.DistanceSquared(GameService.Gw2Mumble.get_PlayerCamera().get_Position(), reset.Position);
				((Color)(ref color)).set_A((byte)(MathUtils.Scale(distanceSq, 2500.0, 40000.0, 1.0, 0.0, clamp: true) * 255.0));
				DrawRacePoint(spriteBatch, reset, color);
			}
			if (_racing == null && _ready == null && !IsTesting)
			{
				runFx.DrawStart(spriteBatch);
			}
			else
			{
				runFx.DrawPoints(spriteBatch, PassedPoints);
			}
			if (Ghost != null && _racing != null)
			{
				DrawGhost(spriteBatch, Ghost.SnapshotAt(RaceTime));
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, base.UIParams);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, base.UIParams);
			_hud.Draw(spriteBatch, (Control)(object)this, RectangleF.op_Implicit(new Rectangle(0, 0, ((Control)Control.get_Graphics().get_SpriteScreen()).get_Width(), ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height())));
		}

		protected override void DrawRaceToMap(SpriteBatch spriteBatch, IMapBounds map)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			Race race = Race;
			if (race == null)
			{
				return;
			}
			foreach (RacePoint reset in race.ResetPoints)
			{
				DrawMapRacePoint(spriteBatch, map, reset, RaceDrawer.ResetColor);
			}
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				DrawMapLine(spriteBatch, map);
				foreach (RacePoint point in Checkpoints)
				{
					DrawMapRacePoint(spriteBatch, map, point, RaceDrawer.CheckpointColor);
				}
			}
			else
			{
				DrawMapEdgeRacePoint(spriteBatch, map, Checkpoints[PassedPoints], RaceDrawer.NextCheckpointColor);
			}
			if (Ghost != null && _racing != null)
			{
				DrawMapGhost(spriteBatch, map, Ghost.SnapshotAt(RaceTime));
			}
		}

		private void MapChanged(object sender, ValueEventArgs<int> e)
		{
			CancelRace();
		}

		private void Teleported()
		{
			CancelRace();
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			_measurer.NewPosition -= new Action<PosSnapshot>(NewPosition);
			_measurer.Teleported -= new Action(Teleported);
			_dc.Dispose();
			_runFx?.Dispose();
			_runFx = null;
			base.DisposeControl();
		}
	}
}
