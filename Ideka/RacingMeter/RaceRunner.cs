using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

		private Ghost _ready;

		private Ghost _racing;

		private TimeSpan _timeBase;

		private RacePoint _inPoint;

		private readonly Primitive _ghostA;

		private readonly Primitive _ghostB;

		private readonly SoundEffect _raceStartSfx;

		private readonly SoundEffect _raceFinishSfx;

		private readonly SoundEffect _raceCancelSfx;

		private readonly SoundEffect _checkpointSfx;

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
				Checkpoints = null;
				LoopStartIndex = 0;
				RoadPoints = null;
				if (value != null)
				{
					Checkpoints = value.Race?.Checkpoints.ToList();
					if (Checkpoints.Count < 2)
					{
						Checkpoints = null;
						ScreenNotification.ShowNotification(Strings.NotifyNotEnoughCheckpoints, (NotificationType)2, (Texture2D)null, 4);
					}
					else
					{
						LoopStartIndex = value.Race.LoopStartIndex;
						RoadPoints = new List<(int, RacePoint)>();
						int i = 0;
						foreach (RacePoint point in value.Race.RoadPoints)
						{
							RoadPoints.Add((i, point));
							if (point.IsCheckpoint)
							{
								i++;
							}
						}
						_fullRace = value;
					}
				}
				RaceLoad();
			}
		}

		public List<RacePoint> Checkpoints { get; private set; }

		public int LoopStartIndex { get; private set; }

		public List<(int i, RacePoint point)> RoadPoints { get; private set; }

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

		public float Volume { get; set; }

		public int MaxGhostData { get; set; }

		public bool AutoLocalGhost { get; set; }

		public int ShownCheckpoints { get; set; }

		public bool ShowGuides { get; set; }

		public bool NormalizedOfficialCheckpoints { get; set; }

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

		public bool OfficialPoints
		{
			get
			{
				if (base.Race != null && base.Race.IsOfficial)
				{
					return NormalizedOfficialCheckpoints;
				}
				return false;
			}
		}

		public event Action<FullRace> RaceLoaded;

		public event Action<Race> RaceStarted;

		public event Action<Race> RaceCancelled;

		public event Action<Race, TimeSpan> RaceFinished;

		public event Action<FullGhost> GhostLoaded;

		public event Action<Ghost> GhostSaved;

		public RaceRunner(int testCheckpoint = -1)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			TestCheckpoint = testCheckpoint;
			CurrentStep = (IsTesting ? TestCheckpoint : 0);
			_ghostA = Primitive.HorizontalCircle(1f, 100);
			_ghostB = new Primitive(Vector3.get_Zero(), new Vector3(0f, 1f, 0f));
			_raceStartSfx = RacingModule.ContentsManager.GetSound("SFX/RaceStart.wav");
			_raceFinishSfx = RacingModule.ContentsManager.GetSound("SFX/RaceFinish.wav");
			_raceCancelSfx = RacingModule.ContentsManager.GetSound("SFX/RaceCancel.wav");
			_checkpointSfx = RacingModule.ContentsManager.GetSound("SFX/Checkpoint.wav");
			_ui = RunnerUI.Construct(this);
			RacingModule.Settings.SfxVolumeMultiplier.OnChangedAndNow(delegate(float v)
			{
				Volume = v;
			});
			RacingModule.Settings.MaxGhostData.OnChangedAndNow(delegate(int v)
			{
				MaxGhostData = v;
			});
			RacingModule.Settings.AutoLocalGhost.OnChangedAndNow(delegate(bool v)
			{
				AutoLocalGhost = v;
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
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			RacingModule.Measurer.NewPosition += new Action<PosSnapshot>(NewPosition);
			RacingModule.Measurer.Teleported += new Action(Teleported);
		}

		private void Play(SoundEffect sfx)
		{
			if (!OfficialPoints || IsTesting)
			{
				sfx.Play(GameService.GameIntegration.get_Audio().get_Volume() * Volume, 0f, 0f);
			}
		}

		private void NewPosition(PosSnapshot pos)
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			PosStack.Push(pos);
			if (base.Race == null)
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
						Play(_raceFinishSfx);
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
					Play(_checkpointSfx);
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
			Play(_raceStartSfx);
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
			Play(_raceStartSfx);
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
				Play(_raceCancelSfx);
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
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			if (!CanDraw())
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
				DrawRacePoint(spriteBatch, Checkpoints[0], RaceHolder.CheckpointColor, flat: true);
			}
			else
			{
				int shown = 0;
				RacePoint lastCheckpoint = Checkpoints[Checkpoints.Count - 1];
				foreach (var item3 in RoadPoints.Append(RoadPoints.First()).SkipWhile(((int i, RacePoint point) p) => p.i != CurrentStep).By2())
				{
					(int, RacePoint) item = item3.Item1;
					(int i, RacePoint point) item2 = item3.Item2;
					int i = item.Item1;
					RacePoint point = item.Item2;
					RacePoint next = item2.point;
					bool isLast = point == lastCheckpoint;
					Color color2 = (isLast ? RaceHolder.FinalCheckpointColor : ((i <= CurrentStep) ? RaceHolder.NextCheckpointColor : RaceHolder.CheckpointColor));
					if (ShownCheckpoints != 1)
					{
						((Color)(ref color2)).set_A((byte)(MathUtils.Scale(shown, 0.0, ShownCheckpoints, 1.0, 0.1) * 255.0));
					}
					if (point.Type == RacePointType.Guide && ShowGuides && i - 1 <= CurrentStep)
					{
						DrawRaceArrow(spriteBatch, point.Position, point.Radius, next.Position, color2);
					}
					else if (point.IsCheckpoint)
					{
						shown++;
						DrawRacePoint(spriteBatch, point.Position, (OfficialPoints && i >= LoopStartIndex) ? 7.62f : point.Radius, isLast ? null : new Vector3?(next.Position), color2);
					}
					if (isLast || shown >= ShownCheckpoints)
					{
						break;
					}
				}
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
			base.DisposeControl();
		}
	}
}
