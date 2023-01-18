using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LobbyRaceActionsPanel : FlowPanel
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly Label _raceLabel;

		private readonly Label _lapsLabel;

		private readonly Label _racersLabel;

		private readonly StandardButton _startRaceButton;

		private readonly StandardButton _cancelRaceButton;

		private RacingServer Server => Client.Server;

		public LobbyRaceActionsPanel(RacingClient client)
			: this()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			Client = client;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_Title("Race");
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(10f, 10f);
			((FlowPanel)this).set_OuterControlPadding(val);
			((FlowPanel)this).set_ControlPadding(val);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			_raceLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			_lapsLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			_racersLabel = val4;
			RacingServer server = Server;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Start Race");
			_startRaceButton = server.Register<StandardButton>(val5);
			((Control)_startRaceButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Server.Lock())
				{
					await Server.StartCountdown();
				}
			});
			RacingServer server2 = Server;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Cancel Race");
			_cancelRaceButton = server2.Register<StandardButton>(val6);
			((Control)_cancelRaceButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Server.Lock())
				{
					await Server.CancelRace();
				}
			});
			Client.LobbyRaceUpdated += RaceUpdated;
			Client.LobbySettingsUpdated += SettingsUpdated;
			Client.UserUpdated += UserUpdated;
		}

		private void RaceUpdated(FullRace race)
		{
			UpdateVisuals();
		}

		private void SettingsUpdated(Lobby lobby)
		{
			UpdateVisuals();
		}

		private void UserUpdated(User user, bool leaving)
		{
			UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			_raceLabel.set_Text("Race: " + (Client.Lobby?.Race?.Race.Name ?? Strings.None));
			int laps = Client.Lobby?.Settings.Laps ?? 0;
			Lobby lobby = Client.Lobby;
			bool notLooping = lobby != null && lobby.Race?.Race.IsLooping == false;
			_lapsLabel.set_Text($"Laps: {(notLooping ? 1 : laps)}");
			((Control)_lapsLabel).set_BasicTooltipText(notLooping ? "The current race does not support looping." : null);
			List<User> obj = Client.Lobby?.Racers.ToList();
			List<User> notReady = obj?.Where((User r) => !r.RacerData.RaceReady).ToList();
			int total = obj?.Count ?? 0;
			int ready = total - (notReady?.Count ?? 0);
			_racersLabel.set_Text($"Racers: {ready} / {total}");
			((Control)_racersLabel).set_BasicTooltipText((notReady == null || !notReady.Any()) ? null : ("Not ready:\n" + string.Join("\n", notReady.Select((User n) => n.Id))));
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (Client != null)
			{
				((Control)(object)_raceLabel).WidthFillRight(10);
				((Control)(object)_lapsLabel).WidthFillRight(10);
				((Control)(object)_racersLabel).WidthFillRight(10);
				((Control)(object)_startRaceButton).AlignCenter();
				((Control)(object)_cancelRaceButton).AlignCenter();
				((Container)(object)this).SetContentRegionHeight(((Control)_cancelRaceButton).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			Client.LobbyRaceUpdated -= RaceUpdated;
			Client.LobbySettingsUpdated -= SettingsUpdated;
			Client.UserUpdated -= UserUpdated;
			((Panel)this).DisposeControl();
		}
	}
}
