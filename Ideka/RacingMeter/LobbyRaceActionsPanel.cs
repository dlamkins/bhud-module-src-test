using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LobbyRaceActionsPanel : Panel
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly Label _raceLabel;

		private readonly Label _timeLabel;

		private readonly Label _lapsLabel;

		private readonly Label _racersLabel;

		private readonly StandardButton _startRaceButton;

		private readonly StandardButton _cancelRaceButton;

		private RacingServer Server => Client.Server;

		public LobbyRaceActionsPanel(RacingClient client)
			: this()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			Client = client;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_Title(Strings.Race);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			_raceLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			_timeLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			_lapsLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			_racersLabel = val4;
			RacingServer server = Server;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.LobbyRaceStart);
			_startRaceButton = server.Register<StandardButton>(val5);
			RacingServer server2 = Server;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.LobbyRaceCancel);
			_cancelRaceButton = server2.Register<StandardButton>(val6);
			UpdateLayout();
			((Control)_startRaceButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Server.Lock())
				{
					await Server.StartCountdown();
				}
			});
			((Control)_cancelRaceButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Server.Lock())
				{
					await Server.CancelRace();
				}
			});
			Client.LobbyRaceUpdated += new Action<FullRace>(RaceUpdated);
			Client.LobbySettingsUpdated += new Action<Lobby>(SettingsUpdated);
			Client.UserUpdated += new Action<User, bool>(UserUpdated);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			TimeSpan time = TimeSpan.Zero;
			if (Client.Lobby != null)
			{
				if (Client.Lobby!.IsRunning)
				{
					time = DateTime.UtcNow - Client.Lobby!.StartTime;
				}
				else
				{
					IEnumerable<TimeSpan> times = Client.Lobby!.Racers.Select((User x) => x.RacerData.LastTime?.Time ?? TimeSpan.Zero);
					time = (times.Any() ? times.Max() : TimeSpan.Zero);
				}
			}
			_timeLabel.set_Text("Time: " + time.Formatted());
		}

		private void RaceUpdated(FullRace? _)
		{
			UpdateVisuals();
		}

		private void SettingsUpdated(Lobby? _)
		{
			UpdateVisuals();
		}

		private void UserUpdated(User user, bool leaving)
		{
			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			_raceLabel.set_Text(Client.Lobby?.FullRace?.Race.Name ?? Strings.None);
			int laps = Client.Lobby?.Settings.Laps ?? 0;
			Lobby? lobby = Client.Lobby;
			bool notLooping = lobby != null && lobby!.FullRace?.Race.IsLooping == false;
			_lapsLabel.set_Text(Strings.LabelRaceLaps.Format(notLooping ? 1 : laps));
			((Control)_lapsLabel).set_BasicTooltipText(notLooping ? Strings.TooltipRaceNotLooping : null);
			List<User> obj = Client.Lobby?.Racers.ToList();
			List<User> notReady = obj?.Where((User r) => !r.RacerData.RaceReady).ToList();
			int total = obj?.Count ?? 0;
			int ready = total - (notReady?.Count ?? 0);
			_racersLabel.set_Text(Strings.LabelLobbyRacers.Format(ready, total));
			((Control)_racersLabel).set_BasicTooltipText((notReady == null || !notReady.Any()) ? null : StringExtensions.Format(Strings.LabelLobbyUsersNotReady, string.Join("\n", notReady.Select((User n) => n.DisplayName))));
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			if (Client != null)
			{
				Label raceLabel = _raceLabel;
				Label timeLabel = _timeLabel;
				Label lapsLabel = _lapsLabel;
				int num;
				((Control)_racersLabel).set_Left(num = 10);
				int num2;
				((Control)lapsLabel).set_Left(num2 = num);
				int left;
				((Control)timeLabel).set_Left(left = num2);
				((Control)raceLabel).set_Left(left);
				((Control)_raceLabel).set_Top(10);
				((Control)(object)_raceLabel).ArrangeTopDown(10, (Control)_timeLabel, (Control)_lapsLabel, (Control)_racersLabel);
				((Control)(object)_raceLabel).WidthFillRight(10);
				((Control)(object)_timeLabel).WidthFillRight(10);
				((Control)(object)_lapsLabel).WidthFillRight(10);
				((Control)(object)_racersLabel).WidthFillRight(10);
				StandardButton startRaceButton = _startRaceButton;
				StandardButton cancelRaceButton = _cancelRaceButton;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)cancelRaceButton).set_Right(left = ((Rectangle)(ref contentRegion)).get_Right() - 10);
				((Control)startRaceButton).set_Right(left);
				((Control)(object)_startRaceButton).MiddleWith((Control)(object)_lapsLabel);
				((Control)(object)_cancelRaceButton).MiddleWith((Control)(object)_racersLabel);
				((Container)(object)this).SetContentRegionHeight(((Control)_cancelRaceButton).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			Client.LobbyRaceUpdated -= new Action<FullRace>(RaceUpdated);
			Client.LobbySettingsUpdated -= new Action<Lobby>(SettingsUpdated);
			Client.UserUpdated -= new Action<User, bool>(UserUpdated);
			((Panel)this).DisposeControl();
		}
	}
}
