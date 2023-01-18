using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LobbyBar : Container
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly Label _pingLabel;

		private readonly StandardButton _connectButton;

		private readonly StandardButton _backButton;

		private readonly StandardButton _lobbyCreateButton;

		private readonly TextBox _lobbyIdInput;

		private readonly StandardButton _lobbyJoinButton;

		private readonly StandardButton _lobbyLeaveButton;

		private RacingServer Server => Client.Server;

		public LobbyBar(RacingClient client)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			Client = client;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Ping: ...");
			val.set_AutoSizeWidth(true);
			_pingLabel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.BackToRacing);
			_backButton = val2;
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RacingModule.Racer.CurrentMode = Racer.Mode.Racing;
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Connect");
			_connectButton = val3;
			((Control)_connectButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Client.Connect();
			});
			RacingServer server = Server;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Create Lobby");
			_lobbyCreateButton = server.Register<StandardButton>(val4);
			((Control)_lobbyCreateButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.Server.CreateLobby();
				}
			});
			RacingServer server2 = Server;
			TextBox val5 = new TextBox();
			((Control)val5).set_Parent((Container)(object)this);
			((TextInputBase)val5).set_PlaceholderText("Lobby ID to join");
			_lobbyIdInput = server2.Register<TextBox>(val5);
			RacingServer server3 = Server;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Join Lobby");
			_lobbyJoinButton = server3.Register<StandardButton>(val6);
			((Control)_lobbyJoinButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.Server.JoinLobby(((TextInputBase)_lobbyIdInput).get_Text());
				}
			});
			RacingServer server4 = Server;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Leave Lobby");
			_lobbyLeaveButton = server4.Register<StandardButton>(val7);
			((Control)_lobbyLeaveButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.Server.LeaveLobby();
				}
			});
			Client.PingUpdated += PingUpdated;
			Client.StateChanged += ClientStateChanged;
			Client.LobbyChanged += LobbyChanged;
			UpdateLayout();
		}

		private void PingUpdated(int ping)
		{
			_pingLabel.set_Text($"Ping: {ping}ms");
		}

		private void ClientStateChanged(RacingClient.ClientState state)
		{
			((Control)_connectButton).set_Visible(state != RacingClient.ClientState.Online);
			((Control)_connectButton).set_Enabled(state == RacingClient.ClientState.Offline);
			_pingLabel.set_Text("Ping: ...");
			UpdateLobbyButtons(state, Client.Lobby);
		}

		private void LobbyChanged(Lobby currentLobby)
		{
			UpdateLobbyButtons(Client.State, currentLobby);
		}

		private void UpdateLobbyButtons(RacingClient.ClientState state, Lobby currentLobby)
		{
			StandardButton lobbyCreateButton = _lobbyCreateButton;
			TextBox lobbyIdInput = _lobbyIdInput;
			bool flag;
			((Control)_lobbyJoinButton).set_Visible(flag = state == RacingClient.ClientState.Online && currentLobby == null);
			bool visible;
			((Control)lobbyIdInput).set_Visible(visible = flag);
			((Control)lobbyCreateButton).set_Visible(visible);
			((Control)_lobbyLeaveButton).set_Visible(state == RacingClient.ClientState.Online && currentLobby != null);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			ClientStateChanged(Client.State);
			((Control)_backButton).set_Location(Point.get_Zero());
			((Control)(object)_backButton).AlignMiddle();
			((Control)(object)_backButton).ArrangeLeftRight(10, (Control)_pingLabel);
			StandardButton lobbyCreateButton = _lobbyCreateButton;
			StandardButton lobbyJoinButton = _lobbyJoinButton;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			int right;
			((Control)lobbyJoinButton).set_Right(right = ((Rectangle)(ref contentRegion)).get_Right());
			((Control)lobbyCreateButton).set_Right(right);
			((Control)_lobbyJoinButton).set_Bottom(((Container)this).get_ContentRegion().Height / 2);
			((Control)_lobbyCreateButton).set_Top(((Container)this).get_ContentRegion().Height / 2);
			((Control)(object)_lobbyJoinButton).ArrangeRightLeft(10, (Control)_lobbyIdInput);
			((Control)(object)_lobbyIdInput).MiddleWith((Control)(object)_lobbyJoinButton);
			((Control)_lobbyLeaveButton).set_Location(((Control)_lobbyJoinButton).get_Location());
			((Control)(object)_lobbyLeaveButton).AlignMiddle();
			((Control)_connectButton).set_Location(((Control)_lobbyLeaveButton).get_Location());
		}

		protected override void DisposeControl()
		{
			Client.PingUpdated -= PingUpdated;
			Client.StateChanged -= ClientStateChanged;
			Client.LobbyChanged -= LobbyChanged;
			((Container)this).DisposeControl();
		}
	}
}
