using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
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

		public event Action? BackRequested;

		public LobbyBar(RacingClient client)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			Client = client;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(StringExtensions.Format(Strings.LabelPing, "..."));
			val.set_AutoSizeWidth(true);
			_pingLabel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.BackToRacing);
			_backButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Strings.Connect);
			_connectButton = val3;
			RacingServer server = Server;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(Strings.LobbyCreate);
			_lobbyCreateButton = server.Register<StandardButton>(val4);
			RacingServer server2 = Server;
			TextBox val5 = new TextBox();
			((Control)val5).set_Parent((Container)(object)this);
			((TextInputBase)val5).set_PlaceholderText(Strings.LobbyIdInput);
			_lobbyIdInput = server2.Register<TextBox>(val5);
			RacingServer server3 = Server;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.LobbyJoin);
			_lobbyJoinButton = server3.Register<StandardButton>(val6);
			RacingServer server4 = Server;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(Strings.LobbyLeave);
			_lobbyLeaveButton = server4.Register<StandardButton>(val7);
			UpdateLayout();
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.BackRequested?.Invoke();
			});
			((Control)_connectButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Client.Connect();
			});
			((Control)_lobbyCreateButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.Server.CreateLobby();
				}
			});
			((Control)_lobbyJoinButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.Server.JoinLobby(((TextInputBase)_lobbyIdInput).get_Text());
				}
			});
			((Control)_lobbyLeaveButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.Server.LeaveLobby();
				}
			});
			Client.PingUpdated += new Action<int>(PingUpdated);
			Client.StateChanged += new Action<RacingClient.ClientState>(ClientStateChanged);
			Client.LobbyChanged += new Action<Lobby>(LobbyChanged);
		}

		private void PingUpdated(int ping)
		{
			_pingLabel.set_Text(Strings.LabelPing.Format(ping));
		}

		private void ClientStateChanged(RacingClient.ClientState state)
		{
			((Control)_connectButton).set_Visible(state != RacingClient.ClientState.Online);
			((Control)_connectButton).set_Enabled(state == RacingClient.ClientState.Offline);
			_pingLabel.set_Text(StringExtensions.Format(Strings.LabelPing, "..."));
			UpdateLobbyButtons(state, Client.Lobby);
		}

		private void LobbyChanged(Lobby? currentLobby)
		{
			UpdateLobbyButtons(Client.State, currentLobby);
		}

		private void UpdateLobbyButtons(RacingClient.ClientState state, Lobby? currentLobby)
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
			Client.PingUpdated -= new Action<int>(PingUpdated);
			Client.StateChanged -= new Action<RacingClient.ClientState>(ClientStateChanged);
			Client.LobbyChanged -= new Action<Lobby>(LobbyChanged);
			((Container)this).DisposeControl();
		}
	}
}
