using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class GhostsPanel : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<GhostsPanel>();

		private static readonly Color SelfBoardColor = Color.get_Blue() * 0.05f;

		private const int Spacing = 10;

		private FullRace _race;

		private FullGhost _ghost;

		private readonly FlowPanel _panel;

		private readonly Panel _ghostsPanel;

		private readonly Menu _ghostsMenu;

		private readonly Panel _localGhostsPanel;

		private readonly Menu _localGhostsMenu;

		private readonly StandardButton _updateLeaderboardButton;

		private CancellationTokenSource _updateLeaderboard;

		public FullRace Race
		{
			get
			{
				return _race;
			}
			set
			{
				_race = value;
				TaskUtils.Cancel(ref _updateLeaderboard);
				StandardButton updateLeaderboardButton = _updateLeaderboardButton;
				FullRace race = _race;
				((Control)updateLeaderboardButton).set_Enabled(race != null && !race.IsLocal);
				PopulateGhosts();
			}
		}

		public FullGhost Ghost
		{
			get
			{
				return _ghost;
			}
			private set
			{
				_ghost = value;
				this.GhostChanged?.Invoke(_ghost);
			}
		}

		public event Action<FullGhost> GhostChanged;

		public GhostsPanel()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			_panel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_panel);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_CanCollapse(true);
			val2.set_Title(Strings.Leaderboard);
			_ghostsPanel = val2;
			Menu val3 = new Menu();
			((Control)val3).set_Parent((Container)(object)_ghostsPanel);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			val3.set_CanSelect(true);
			_ghostsMenu = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)_panel);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_CanCollapse(true);
			val4.set_Title(Strings.LocalGhosts);
			_localGhostsPanel = val4;
			Menu val5 = new Menu();
			((Control)val5).set_Parent((Container)(object)_localGhostsPanel);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_CanSelect(true);
			_localGhostsMenu = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.UpdateLeaderboard);
			_updateLeaderboardButton = val6;
			((Control)_updateLeaderboardButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!RacingModule.Server.NotifyIfOffline())
				{
					if (Race?.Meta.Id == null)
					{
						ScreenNotification.ShowNotification(Strings.NotifyNoRaceSelected, (NotificationType)2, (Texture2D)null, 4);
					}
					else
					{
						PopulateLeaderboard(force: true);
					}
				}
			});
			RacingModule.Server.UserChanged += UserChanged;
			RacingModule.Server.LeaderboardsChanged += LeaderboardsChanged;
			RacingModule.Racer.GhostSaved += new Action<Ghost>(GhostSaved);
			UpdateLayout();
		}

		private void UpdateLeaderboard(string raceId)
		{
			if (!RacingModule.Server.IsOnline)
			{
				return;
			}
			TaskUtils.Cancel(ref _updateLeaderboard);
			((Control)_updateLeaderboardButton).set_Enabled(false);
			RacingModule.Server.UpdateLeaderboard(raceId, TaskUtils.New(ref _updateLeaderboard)).Done(Logger, Strings.ErrorLeaderboardLoad, _updateLeaderboard).ContinueWith(delegate(Task<TaskUtils.TaskState> task)
			{
				if (task.Result.Success && raceId == Race?.Meta.Id)
				{
					PopulateLeaderboard();
				}
				StandardButton updateLeaderboardButton = _updateLeaderboardButton;
				FullRace race = Race;
				((Control)updateLeaderboardButton).set_Enabled(race != null && !race.IsLocal);
			});
		}

		public void PopulateLeaderboard(bool force = false)
		{
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			FullGhost ghost = Ghost;
			if (ghost != null && !ghost.IsLocal)
			{
				Ghost = null;
			}
			((Container)_ghostsMenu).ClearChildren();
			_ghostsMenu.set_CanSelect(false);
			string raceId = Race?.Meta.Id;
			if (raceId == null)
			{
				return;
			}
			if (Race?.IsLocal ?? false)
			{
				((Control)_ghostsMenu.AddMenuItem(Strings.Unavailable, (Texture2D)null)).set_BasicTooltipText(Strings.RaceLocalLeaderboardTooltip);
				return;
			}
			if (!RacingModule.Server.IsOnline)
			{
				((Control)_ghostsMenu.AddMenuItem(Strings.Unavailable, (Texture2D)null)).set_BasicTooltipText(Strings.NotifyOfflineMode);
				return;
			}
			if (force || !RacingModule.Server.Leaderboards.Boards.TryGetValue(raceId, out var board))
			{
				_ghostsMenu.AddMenuItem(Strings.Loading, (Texture2D)null);
				UpdateLeaderboard(raceId);
				return;
			}
			_ghostsMenu.set_CanSelect(board.Places.Any());
			if (!board.Places.Any())
			{
				_ghostsMenu.AddMenuItem(Strings.Nothing, (Texture2D)null);
				return;
			}
			foreach (KeyValuePair<int, MetaGhost> place2 in board.Places)
			{
				place2.Deconstruct(out var key, out var value);
				int place = key;
				MetaGhost meta = value;
				MenuItem item = _ghostsMenu.AddMenuItem($"{place + 1}. {meta.Time.Formatted()} - {meta.AccountName}", (Texture2D)null);
				if (RacingModule.Server.User?.AccountId == meta.UserId)
				{
					((Control)item).set_BackgroundColor(SelfBoardColor);
				}
				((Control)item).set_BasicTooltipText(meta.AccountName);
				item.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					MenuItem selectedMenuItem = _localGhostsMenu.get_SelectedMenuItem();
					if (selectedMenuItem != null)
					{
						selectedMenuItem.Deselect();
					}
					Ghost = new FullGhost
					{
						Meta = meta,
						Ghost = null
					};
				});
			}
		}

		private void PopulateLocalGhosts()
		{
			if (Ghost?.IsLocal ?? false)
			{
				Ghost = null;
			}
			((Container)_localGhostsMenu).ClearChildren();
			_localGhostsMenu.set_CanSelect(false);
			if (Race?.Meta.Id == null)
			{
				return;
			}
			Dictionary<string, FullGhost>.ValueCollection ghosts = DataInterface.GetLocalGhosts(Race).Values;
			_localGhostsMenu.set_CanSelect(ghosts.Any());
			if (!ghosts.Any())
			{
				_localGhostsMenu.AddMenuItem(Strings.Nothing, (Texture2D)null);
				return;
			}
			foreach (var item in ghosts.OrderBy((FullGhost g) => g.Meta.Time).Enumerate())
			{
				var (place, ghost) = item;
				_localGhostsMenu.AddMenuItem($"{place + 1}. {ghost.Meta.Time.Formatted()}", (Texture2D)null).add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					MenuItem selectedMenuItem = _ghostsMenu.get_SelectedMenuItem();
					if (selectedMenuItem != null)
					{
						selectedMenuItem.Deselect();
					}
					Ghost = ghost;
				});
			}
		}

		private void PopulateGhosts()
		{
			Ghost = null;
			PopulateLocalGhosts();
			PopulateLeaderboard();
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
				Panel ghostsPanel = _ghostsPanel;
				int width;
				((Control)_localGhostsPanel).set_Width(width = ((Container)this).get_ContentRegion().Width - 20);
				((Control)ghostsPanel).set_Width(width);
			}
		}

		private void UserChanged(UserData _)
		{
			PopulateLeaderboard();
		}

		private void LeaderboardsChanged(string raceId)
		{
			if (Race?.Meta.Id == raceId)
			{
				PopulateLeaderboard();
			}
		}

		private void GhostSaved(Ghost ghost)
		{
			if (Race?.Meta.Id == ghost.RaceId)
			{
				PopulateLocalGhosts();
			}
		}

		protected override void DisposeControl()
		{
			RacingModule.Server.UserChanged -= UserChanged;
			RacingModule.Server.LeaderboardsChanged -= LeaderboardsChanged;
			RacingModule.Racer.GhostSaved -= new Action<Ghost>(GhostSaved);
			((Container)this).DisposeControl();
		}
	}
}
