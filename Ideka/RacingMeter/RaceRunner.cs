using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class RaceRunner : RaceHolder
	{
		public const double ResetFadeStartMetersSq = 2500.0;

		public const double ResetFadeEndMetersSq = 40000.0;

		public const double MinCheckpointAlpha = 0.1;

		private const int GhostSlack = 10;

		private readonly DropOutStack<PosSnapshot> PosStack = new DropOutStack<PosSnapshot>(10);

		private FullRace _fullRace;

		private FullGhost _fullGhost;

		private int _currentStep;

		public readonly RaceDrawer Drawer;

		private Ghost _ready;

		private Ghost _racing;

		private TimeSpan _timeBase;

		private RacePoint _inPoint;

		private readonly Primitive _ghostA;

		private readonly Primitive _ghostB;

		private readonly RectAnchor _ui;

		public override FullRace FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				CancelRace();
				_fullRace = null;
				if (value != null)
				{
					if (!value.Race.Checkpoints.Skip(1).Any())
					{
						ScreenNotification.ShowNotification(Strings.NotifyNotEnoughCheckpoints, (NotificationType)2, (Texture2D)null, 4);
					}
					else
					{
						_fullRace = value;
					}
				}
				Drawer.LoadRace(_fullRace?.Race);
				RaceLoad();
			}
		}

		public FullGhost FullGhost
		{
			get
			{
				return _fullGhost;
			}
			set
			{
				_fullGhost = value;
				if (FullGhost?.Ghost != null)
				{
					FullGhost.Ghost.CalculateStart(base.Race);
					FullGhost.Ghost.Checkpoints(base.Race);
				}
				this.GhostLoaded?.Invoke(_fullGhost);
			}
		}

		public List<RacePoint> Checkpoints => Drawer.Checkpoints;

		public Ghost Ghost
		{
			get
			{
				if (!IsTesting)
				{
					return FullGhost?.Ghost;
				}
				return null;
			}
		}

		public Dictionary<string, FullGhost> LocalGhosts { get; private set; }

		public bool SpecificGhostLoaded { get; set; }

		public int CurrentStep
		{
			get
			{
				return Math.Min(Math.Max(_currentStep, TestCheckpoint), Checkpoints.Count - 1);
			}
			private set
			{
				_currentStep = Math.Max(value, TestCheckpoint);
			}
		}

		public double Progress => Drawer.Route.ProgressPercent(CurrentStep, RacingModule.Measurer.Pos.Meters);

		public int TestCheckpoint { get; }

		public bool IsTesting => TestCheckpoint >= 0;

		public TimeSpan RaceTime
		{
			get
			{
				if (_racing != null)
				{
					if (!(_racing.End != TimeSpan.Zero))
					{
						return RacingModule.Measurer.Pos.LastUpdate - _racing.Start - _timeBase;
					}
					return _racing.Time;
				}
				return TimeSpan.Zero;
			}
		}

		public int MaxGhostData { get; set; }

		public bool AutoLocalGhost { get; set; }

		public event Action<FullRace> RaceLoaded;

		public event Action<Race> RaceStarted;

		public event Action<Race> RaceCancelled;

		public event Action<Race, TimeSpan> RaceFinished;

		public event Action<FullGhost> GhostLoaded;

		public event Action<Ghost> GhostSaved;

		public RaceRunner(int testCheckpoint = -1)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			Drawer = new RaceDrawer(this);
			TestCheckpoint = testCheckpoint;
			CurrentStep = (IsTesting ? TestCheckpoint : 0);
			_ghostA = Primitive.HorizontalCircle(1f, 100);
			_ghostB = new Primitive(Vector3.get_Zero(), new Vector3(0f, 1f, 0f));
			RacingModule.Settings.MaxGhostData.OnChangedAndNow(delegate(int v)
			{
				MaxGhostData = v;
			});
			RacingModule.Settings.AutoLocalGhost.OnChangedAndNow(delegate(bool v)
			{
				AutoLocalGhost = v;
			});
			_ui = RunnerUI.Construct(this);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			RacingModule.Measurer.NewPosition += new Action<PosSnapshot>(NewPosition);
			RacingModule.Measurer.Teleported += new Action(Teleported);
		}

		private void NewPosition(PosSnapshot pos)
		{
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			PosStack.Push(pos);
			if (Drawer.Race == null)
			{
				return;
			}
			_ready?.AddSnapshot(_timeBase, pos);
			_racing?.AddSnapshot(_timeBase, pos);
			if (RacingModule.Measurer.Speed.Speed3D == 0f)
			{
				if (_racing != null && base.Race.ResetPoints.Any((RacePoint r) => r.Collides(pos.Meters)))
				{
					CancelRace();
				}
				else if (_racing == null && Checkpoints[0].Collides(pos.Meters))
				{
					(_timeBase, _ready) = GhostExtensions.Ready(FullRace, PosStack);
				}
			}
			if (_racing == null && _ready != null && !IsTesting && hasLeft(Checkpoints[0]))
			{
				StartRace();
				CurrentStep++;
				_inPoint = null;
			}
			else if (_racing == null && TestCheckpoint == CurrentStep && hasLeft(Checkpoints[CurrentStep]))
			{
				StartTesting(pos);
				CurrentStep++;
				_inPoint = null;
			}
			else if (_racing != null)
			{
				bool entered = hasEntered(Checkpoints[CurrentStep]);
				if (CurrentStep == Checkpoints.Count - 1)
				{
					if (entered)
					{
						Drawer.FinishSfx(IsTesting);
					}
					int after;
					if (IsTesting)
					{
						if (entered)
						{
							FinishTesting();
						}
					}
					else if (_racing.Checkpoints(base.Race, out after).Count == Checkpoints.Count && after >= 10)
					{
						FinishRace();
					}
				}
				else if (entered)
				{
					Drawer.CheckpointSfx(IsTesting);
					CurrentStep++;
				}
			}
			if (_ready != null && !Checkpoints[0].Collides(pos.Meters))
			{
				_ready = null;
			}
			bool hasEntered(RacePoint point)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				bool isIn = point.Collides(pos.Meters);
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
				bool isIn2 = point.Collides(pos.Meters);
				if (_inPoint == point)
				{
					_inPoint = (isIn2 ? _inPoint : null);
					return !isIn2;
				}
				_inPoint = (isIn2 ? point : _inPoint);
				return false;
			}
		}

		private void Teleported()
		{
			CancelRace();
		}

		private void RaceLoad()
		{
			SpecificGhostLoaded = false;
			CurrentStep = 0;
			_inPoint = null;
			LocalGhosts = DataInterface.GetLocalGhosts(FullRace);
			RacingModule.Racer.FullGhost = null;
			LoadBestLocalGhost();
			this.RaceLoaded?.Invoke(FullRace);
		}

		private void StartTesting(PosSnapshot pos)
		{
			_ready = null;
			_racing = new Ghost();
			_timeBase = pos.Time;
			_racing.AddSnapshot(_timeBase, pos);
			Drawer.StartSfx(IsTesting);
			this.RaceStarted?.Invoke(base.Race);
		}

		private void FinishTesting()
		{
			CurrentStep = 0;
			_ready = null;
			_racing = null;
		}

		private void StartRace()
		{
			CurrentStep = 0;
			_racing = ((_ready?.CalculateStart(base.Race) ?? false) ? _ready : null);
			_ready = null;
			if (_racing == null)
			{
				ScreenNotification.ShowNotification(Strings.NotifyRaceStartFailed, (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			Drawer.StartSfx(IsTesting);
			this.RaceStarted?.Invoke(base.Race);
		}

		private void FinishRace()
		{
			CurrentStep = 0;
			TimeSpan time = _racing.Time;
			SaveGhost(_racing);
			_ready = null;
			_racing = null;
			LoadBestLocalGhost();
			this.RaceFinished?.Invoke(base.Race, time);
		}

		private void CancelRace()
		{
			if (_racing != null)
			{
				Drawer.CancelSfx(IsTesting);
				CurrentStep = 0;
				_ready = null;
				_racing = null;
				this.RaceCancelled?.Invoke(base.Race);
			}
		}

		private void SaveGhost(Ghost ghost)
		{
			if (!IsTesting && ghost != null && ghost.RaceId != null)
			{
				Dictionary<string, FullGhost> localGhosts = LocalGhosts;
				bool num = DataInterface.SaveGhost(ref localGhosts, ghost, MaxGhostData);
				LocalGhosts = localGhosts;
				if (num)
				{
					this.GhostSaved?.Invoke(ghost);
				}
			}
		}

		private void LoadBestLocalGhost()
		{
			if (!IsTesting && AutoLocalGhost && !SpecificGhostLoaded)
			{
				RacingModule.Racer.FullGhost = (LocalGhosts.Any() ? LocalGhosts.Values.MinBy((FullGhost g) => g.Meta.Time) : null);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			if (!CanDrawRace())
			{
				return;
			}
			foreach (RacePoint reset in base.Race.ResetPoints)
			{
				Color color = RaceHolder.ResetColor;
				float distanceSq = Vector3.DistanceSquared(GameService.Gw2Mumble.get_PlayerCamera().get_Position(), reset.Position);
				((Color)(ref color)).set_A((byte)(MathUtils.Scale(distanceSq, 2500.0, 40000.0, 1.0, 0.0, clamp: true) * 255.0));
				DrawRacePoint(spriteBatch, reset, color);
			}
			if (_racing == null && _ready == null && !IsTesting)
			{
				Drawer.DrawStart(spriteBatch);
			}
			else
			{
				Drawer.DrawPoints(spriteBatch, CurrentStep);
			}
			if (Ghost != null && _racing != null)
			{
				GhostSnapshot snapshot = Ghost.SnapshotAt(RaceTime);
				Matrix trs = Matrix.CreateScale(1f) * Matrix.CreateRotationZ(0f - (float)Math.Atan2(snapshot.Front.X, snapshot.Front.Y)) * Matrix.CreateTranslation(snapshot.Position);
				_ghostA.Transformed(trs).ToScreen().Draw(spriteBatch, RaceHolder.GhostColor, 2f);
				_ghostB.Transformed(trs).ToScreen().Draw(spriteBatch, RaceHolder.GhostColor, 2f);
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, base.UIParams);
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(0, 0, ((Control)Control.get_Graphics().get_SpriteScreen()).get_Width(), ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height());
			ShapeExtensions.DrawRectangle(spriteBatch, RectangleF.op_Implicit(rect), Color.get_Black(), 1f, 0f);
			_ui.Draw(spriteBatch, (Control)(object)this, RectangleF.op_Implicit(rect));
		}

		public override void DrawToMap(SpriteBatch spriteBatch, MapBounds map)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			if (base.Race == null)
			{
				return;
			}
			foreach (RacePoint reset in base.Race.ResetPoints)
			{
				DrawMapRacePoint(spriteBatch, map, reset, RaceHolder.ResetColor);
			}
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				DrawMapLine(spriteBatch, map);
				foreach (RacePoint point in Checkpoints)
				{
					DrawMapRacePoint(spriteBatch, map, point, RaceHolder.CheckpointColor);
				}
			}
			else
			{
				DrawMapEdgeRacePoint(spriteBatch, map, Checkpoints[CurrentStep], RaceHolder.NextCheckpointColor);
			}
			if (Ghost != null && _racing != null)
			{
				DrawGhost(spriteBatch, map, Ghost.SnapshotAt(RaceTime));
			}
		}

		private void MapChanged(object sender, ValueEventArgs<int> e)
		{
			CancelRace();
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			RacingModule.Measurer.NewPosition -= new Action<PosSnapshot>(NewPosition);
			RacingModule.Measurer.Teleported -= new Action(Teleported);
			Drawer?.Dispose();
			base.DisposeControl();
		}
	}
}
