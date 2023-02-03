using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class GhostsPanel : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<GhostsPanel>();

		private const int Spacing = 10;

		public FullRace? _race;

		private readonly FlowPanel _panel;

		private readonly GhostsMenu _remoteGhostsMenu;

		private readonly GhostsMenu _localGhostsMenu;

		private readonly StandardButton _updateLeaderboardButton;

		private CancellationTokenSource? _updateLeaderboard;

		private CancellationTokenSource? _loadLocalGhosts;

		public FullRace? Race
		{
			get
			{
				return _race;
			}
			set
			{
				_race = value;
				StandardButton updateLeaderboardButton = _updateLeaderboardButton;
				FullRace? race = _race;
				((Control)updateLeaderboardButton).set_Enabled(race != null && !race!.IsLocal);
				PopulateLeaderboard();
				PopulateLocalGhosts();
			}
		}

		public event Action<FullGhost?>? GhostSelected;

		public GhostsPanel()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			_panel = val;
			GhostsMenu ghostsMenu = new GhostsMenu();
			((Control)ghostsMenu).set_Parent((Container)(object)_panel);
			((Panel)ghostsMenu).set_Title(Strings.Leaderboard);
			_remoteGhostsMenu = ghostsMenu;
			GhostsMenu ghostsMenu2 = new GhostsMenu();
			((Control)ghostsMenu2).set_Parent((Container)(object)_panel);
			((Panel)ghostsMenu2).set_Title(Strings.LocalGhosts);
			_localGhostsMenu = ghostsMenu2;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.UpdateLeaderboard);
			_updateLeaderboardButton = val2;
			UpdateLayout();
			_remoteGhostsMenu.ItemSelected += new Action<FullGhost>(OnGhostSelected);
			_localGhostsMenu.ItemSelected += new Action<FullGhost>(OnGhostSelected);
			((Control)_updateLeaderboardButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (Race?.Meta.Id == null)
				{
					ScreenNotification.ShowNotification(Strings.NotifyNoRaceSelected, (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					PopulateLeaderboard(force: true);
				}
			});
			Race = null;
			RacingModule.Server.LeaderboardsChanged += new Action<string>(LeaderboardsChanged);
			RacingModule.Server.UserChanged += new Action<UserData>(UserChanged);
			RacingModule.LocalData.GhostsChanged += new Action<FullRace>(GhostsChanged);
		}

		public void GhostsChanged(FullRace race)
		{
			if (race.Meta?.Id == Race?.Meta?.Id)
			{
				PopulateLocalGhosts();
			}
		}

		private void OnGhostSelected(FullGhost? ghost)
		{
			_remoteGhostsMenu.Selected = ghost;
			_localGhostsMenu.Selected = ghost;
			this.GhostSelected?.Invoke(ghost);
		}

		private void UpdateLeaderboard(string raceId)
		{
			TaskUtils.Cancel(ref _updateLeaderboard);
			if (!RacingModule.Server.IsOnline)
			{
				return;
			}
			((Control)_updateLeaderboardButton).set_Enabled(false);
			RacingModule.Server.UpdateLeaderboard(raceId, TaskUtils.New(out _updateLeaderboard)).Done(Logger, Strings.ErrorLeaderboardLoad, _updateLeaderboard).ContinueWith(delegate(Task<TaskUtils.TaskState> task)
			{
				if (!task.IsCanceled)
				{
					((Control)_updateLeaderboardButton).set_Enabled(true);
				}
			});
		}

		private void PopulateLeaderboard(bool force = false)
		{
			FullRace race = Race;
			if (race != null)
			{
				string raceId = race.Meta?.Id;
				if (raceId != null)
				{
					Leaderboard board;
					if (race.IsLocal)
					{
						_remoteGhostsMenu.Placeholder(Strings.Unavailable, Strings.RaceLocalLeaderboardTooltip);
					}
					else if (!RacingModule.Server.IsOnline)
					{
						_remoteGhostsMenu.Placeholder(Strings.Unavailable, Strings.UnavailableOffline);
					}
					else if (force || !RacingModule.Server.Leaderboards.Boards.TryGetValue(raceId, out board))
					{
						_remoteGhostsMenu.Placeholder(Strings.Loading);
						UpdateLeaderboard(raceId);
					}
					else
					{
						_remoteGhostsMenu.SetLeaderboard(board);
					}
					return;
				}
			}
			_remoteGhostsMenu.Placeholder(Strings.Nothing);
		}

		private void PopulateLocalGhosts()
		{
			TaskUtils.Cancel(ref _loadLocalGhosts);
			FullRace race = Race;
			if (race == null)
			{
				_localGhostsMenu.Placeholder(Strings.Nothing);
				return;
			}
			_localGhostsMenu.Placeholder(Strings.Loading);
			IEnumerable<FullGhost> ghosts = null;
			Task.Run(delegate
			{
				ghosts = LocalData.GetGhosts(race).Values;
			}, TaskUtils.New(out _loadLocalGhosts)).Done(Logger, null, _loadLocalGhosts).ContinueWith(delegate(Task<TaskUtils.TaskState> task)
			{
				if (task.Result.Success && ghosts != null)
				{
					_localGhostsMenu.SetLocalGhosts(ghosts);
				}
			});
		}

		private void UserChanged(UserData _)
		{
			PopulateLeaderboard();
		}

		private void LeaderboardsChanged(string raceId)
		{
			if (raceId == Race?.Meta?.Id)
			{
				PopulateLeaderboard();
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null)
			{
				((Control)_updateLeaderboardButton).set_Bottom(((Container)this).get_ContentRegion().Height);
				((Control)_panel).set_Location(Point.get_Zero());
				((Control)_panel).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)_panel).set_Height(((Control)_updateLeaderboardButton).get_Top() - 10);
				((Control)_updateLeaderboardButton).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_updateLeaderboardButton).get_Width() / 2);
				GhostsMenu remoteGhostsMenu = _remoteGhostsMenu;
				int width;
				((Control)_localGhostsMenu).set_Width(width = ((Container)this).get_ContentRegion().Width - 20);
				((Control)remoteGhostsMenu).set_Width(width);
			}
		}

		protected override void DisposeControl()
		{
			RacingModule.Server.LeaderboardsChanged -= new Action<string>(LeaderboardsChanged);
			RacingModule.Server.UserChanged -= new Action<UserData>(UserChanged);
			RacingModule.LocalData.GhostsChanged -= new Action<FullRace>(GhostsChanged);
			((Container)this).DisposeControl();
		}
	}
}
