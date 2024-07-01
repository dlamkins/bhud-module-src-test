using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class RaceOnline : RaceDrawerWorld
	{
		private FullRace? _fullRace;

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly RacingClient Client;

		private RaceRunFx? _runFx;

		private readonly AnchoredRect _hud;

		private CancellationTokenSource? _countdown;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				_fullRace = value;
				Race race = _fullRace?.Race;
				if (race == null)
				{
					_runFx?.Dispose();
					_runFx = null;
					return;
				}
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
		}

		public override Race? Race => FullRace?.Race;

		public int TotalLaps => Client.Lobby?.Settings.Laps ?? 0;

		public int CompletedLaps
		{
			get
			{
				RaceRunFx runFx = _runFx;
				if (runFx != null)
				{
					User user = Client.User;
					if (user != null)
					{
						Lobby lobby = Client.Lobby;
						if (lobby != null && lobby.IsRunning)
						{
							return runFx.Route.Progress(user.RacerData.Times.Count).lap;
						}
					}
				}
				return 0;
			}
		}

		public int TotalPoints => _runFx?.Route.TotalPoints(TotalLaps) ?? 0;

		public int PassedPoints
		{
			get
			{
				User user = Client.User;
				if (user != null)
				{
					Lobby lobby = Client.Lobby;
					if (lobby != null && lobby.IsRunning)
					{
						return user.RacerData.Times.Count;
					}
				}
				return 0;
			}
		}

		public double Progress
		{
			get
			{
				RaceRunFx runFx = _runFx;
				if (runFx != null)
				{
					User user = Client.User;
					if (user != null)
					{
						Lobby lobby = Client.Lobby;
						if (lobby != null && lobby.IsRunning)
						{
							return runFx.Route.ProgressPercent(user.RacerData, TotalLaps);
						}
					}
				}
				return 0.0;
			}
		}

		public TimeSpan RaceTime
		{
			get
			{
				User user = Client.User;
				if (user != null)
				{
					Lobby lobby = Client.Lobby;
					if (lobby != null && user.RacerData.Times.Any())
					{
						if (!lobby.IsRunning || !user.RacerData.RaceReady)
						{
							RacerTime time = user.RacerData.LastTime;
							if (time != null)
							{
								return time.Time;
							}
						}
						return DateTime.UtcNow - lobby.StartTime;
					}
				}
				return TimeSpan.Zero;
			}
		}

		public int Place
		{
			get
			{
				RaceRunFx runFx = _runFx;
				if (runFx != null)
				{
					User user = Client.User;
					if (user != null)
					{
						Lobby lobby = Client.Lobby;
						if (lobby != null && user.RacerData.Times.Any())
						{
							return (from x in lobby.GetLeaderboard(runFx.Route).Enumerate()
								select (x.index + 1, x.item.racer.Id)).FirstOrDefault<(int, string)>(((int position, string userId) x) => x.userId == user.Id).Item1;
						}
					}
				}
				return 0;
			}
		}

		public RaceOnline(RacingClient client)
		{
			Client = client;
			_hud = _dc.Add(new OnlineHUD(this));
			Client.LobbyRaceUpdated += new Action<FullRace>(RaceUpdated);
			Client.CheckpointReached += new Action<User>(CheckpointReached);
			Client.CountdownStarted += new Action<int>(CountdownStarted);
			Client.RaceStarted += new Action(RaceStarted);
			Client.RaceCanceled += new Action(RaceCanceled);
		}

		private void RaceUpdated(FullRace? fullRace)
		{
			FullRace = fullRace;
		}

		private void CheckpointReached(User user)
		{
			if (Client.Lobby == null || user != Client.User)
			{
				return;
			}
			RaceRunFx runFx = _runFx;
			if (runFx == null)
			{
				return;
			}
			int count = user.RacerData.Times.Count;
			if (count <= 0)
			{
				return;
			}
			if (runFx.Route.RaceFinished(count, TotalLaps))
			{
				_runFx?.FinishSfx(forcePlay: true);
				Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					ScreenNotification.ShowNotification(StringExtensions.Format(Strings.OnlineNoticeRaceFinished, Place.Ordinalize(), RaceTime.Formatted()), (NotificationType)0, (Texture2D)null, 4);
				});
				return;
			}
			_runFx?.CheckpointSfx(forcePlay: true);
			int laps = CompletedLaps;
			if (laps > runFx.Route.Progress(count - 1).lap)
			{
				List<TimeSpan> lapTimes = runFx.Route.LapTimes(user.RacerData.Times.Select((RacerTime x) => x.Time).ToList());
				Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					ScreenNotification.ShowNotification(Strings.OnlineNoticeLaps.Format(laps + 1, TotalLaps, lapTimes.Last().Formatted()), (NotificationType)0, (Texture2D)null, 4);
				});
			}
		}

		private void CountdownStarted(int seconds)
		{
			TaskUtils.Cancel(ref _countdown);
			CancellationToken ct = TaskUtils.New(out _countdown);
			((Func<Task>)async delegate
			{
				int i;
				for (i = 0; i < seconds; i++)
				{
					ct.ThrowIfCancellationRequested();
					Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						ScreenNotification.ShowNotification($"{seconds - i}", (NotificationType)0, (Texture2D)null, 4);
					});
					_runFx?.CountdownTickSfx();
					await Task.Delay(TimeSpan.FromSeconds(1.0));
				}
			})();
		}

		private void RaceStarted()
		{
			Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				ScreenNotification.ShowNotification(Strings.OnlineNoticeGo, (NotificationType)0, (Texture2D)null, 4);
			});
			_runFx?.CountdownGoSfx();
			_runFx?.StartSfx(forcePlay: true);
		}

		private void RaceCanceled()
		{
			TaskUtils.Cancel(ref _countdown);
			Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				ScreenNotification.ShowNotification(Strings.OnlineNoticeRaceCanceled, (NotificationType)0, (Texture2D)null, 4);
			});
			_runFx?.CancelSfx(forcePlay: true);
		}

		private static IEnumerable<User> DrawableCompetitors(RacingClient client)
		{
			RacingClient client2 = client;
			Lobby lobby = client2.Lobby;
			if (lobby == null)
			{
				return Array.Empty<User>();
			}
			return lobby.Racers.Where((User r) => r.IsOnline && r.RacerData.Sent && r.RacerData.MapId == GameService.Gw2Mumble.get_CurrentMap().get_Id() && r.Id != client2.UserId);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			User user = Client.User;
			if (user != null && user.LobbyData.IsRacer)
			{
				_hud.Update(gameTime);
			}
		}

		protected override void DrawRaceToWorld(SpriteBatch spriteBatch)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			Lobby lobby = Client.Lobby;
			if (lobby == null)
			{
				return;
			}
			if (RacingModule.Settings.OnlineMarkRacers.Value)
			{
				foreach (User racer in DrawableCompetitors(Client))
				{
					DrawGhost(spriteBatch, racer.RacerData.Position, racer.RacerData.Front, Color.get_White());
				}
			}
			User user = Client.User;
			if (user == null)
			{
				return;
			}
			RaceRunFx runFx = _runFx;
			if (runFx != null)
			{
				if ((user.LobbyData.IsRacer && !user.RacerData.RaceReady) || (!user.LobbyData.IsRacer && !lobby.IsRunning))
				{
					runFx.DrawStart(spriteBatch);
				}
				if (user.LobbyData.IsRacer && user.RacerData.RaceReady)
				{
					runFx.DrawPoints(spriteBatch, lobby.IsRunning ? user.RacerData.Times.Count : 0, TotalLaps);
				}
			}
			if (user.LobbyData.IsRacer)
			{
				spriteBatch.End();
				SpriteBatchExtensions.Begin(spriteBatch, base.UIParams);
				_hud.Draw(spriteBatch, (Control)(object)this, RectangleF.op_Implicit(new Rectangle(0, 0, ((Control)Control.get_Graphics().get_SpriteScreen()).get_Width(), ((Control)Control.get_Graphics().get_SpriteScreen()).get_Height())));
			}
		}

		protected override void DrawRaceToMap(SpriteBatch spriteBatch, IMapBounds map)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			RaceRunFx runFx = _runFx;
			if (runFx == null)
			{
				return;
			}
			User user = Client.User;
			if (user == null)
			{
				return;
			}
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				DrawMapLine(spriteBatch, map);
				foreach (RacePoint point in runFx.Checkpoints)
				{
					DrawMapRacePoint(spriteBatch, map, point, RaceDrawer.CheckpointColor);
				}
			}
			else if (PassedPoints < runFx.Checkpoints.Count && user.LobbyData.IsRacer)
			{
				DrawMapEdgeRacePoint(spriteBatch, map, runFx.Checkpoints[PassedPoints], RaceDrawer.NextCheckpointColor);
			}
			foreach (User racer in DrawableCompetitors(Client))
			{
				DrawMapGhost(spriteBatch, map, racer.RacerData.Position, racer.RacerData.Front, Color.get_White());
			}
		}

		protected override void DisposeControl()
		{
			Client.LobbyRaceUpdated -= new Action<FullRace>(RaceUpdated);
			Client.CheckpointReached -= new Action<User>(CheckpointReached);
			Client.CountdownStarted -= new Action<int>(CountdownStarted);
			Client.RaceStarted -= new Action(RaceStarted);
			Client.RaceCanceled -= new Action(RaceCanceled);
			_dc.Dispose();
			_runFx?.Dispose();
			_runFx = null;
			TaskUtils.Cancel(ref _countdown);
			base.DisposeControl();
		}
	}
}
