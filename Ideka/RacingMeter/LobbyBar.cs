using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LobbyBar : Container
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly StandardButton _backButton;

		private readonly Label _pingLabel;

		private readonly TextBox _nicknameInput;

		private readonly StandardButton _updateNicknameButton;

		private readonly StandardButton _connectButton;

		private readonly StandardButton _connectGuestButton;

		private readonly StandardButton _disconnectButton;

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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Expected O, but got Unknown
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			Client = client;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.BackToRacing);
			_backButton = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(StringExtensions.Format(Strings.LabelPing, "..."));
			val2.set_AutoSizeWidth(true);
			_pingLabel = val2;
			TextBoxFix textBoxFix = new TextBoxFix();
			((Control)textBoxFix).set_Parent((Container)(object)this);
			((TextInputBase)textBoxFix).set_Text(RacingModule.Settings.OnlineDefaultNickname.Value);
			((TextInputBase)textBoxFix).set_PlaceholderText(Strings.NicknameInput);
			((Control)textBoxFix).set_BasicTooltipText(Strings.NicknameTooltip);
			_nicknameInput = (TextBox)(object)textBoxFix;
			RacingServer server = Server;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Strings.NicknameUpdate);
			_updateNicknameButton = server.Register<StandardButton>(val3);
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(Strings.Connect);
			_connectButton = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.ConnectGuest);
			_connectGuestButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.Disconnect);
			_disconnectButton = val6;
			RacingServer server2 = Server;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(Strings.LobbyCreate);
			_lobbyCreateButton = server2.Register<StandardButton>(val7);
			RacingServer server3 = Server;
			TextBox val8 = new TextBox();
			((Control)val8).set_Parent((Container)(object)this);
			((TextInputBase)val8).set_PlaceholderText(Strings.LobbyIdInput);
			_lobbyIdInput = server3.Register<TextBox>(val8);
			RacingServer server4 = Server;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text(Strings.LobbyJoin);
			_lobbyJoinButton = server4.Register<StandardButton>(val9);
			RacingServer server5 = Server;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)this);
			val10.set_Text(Strings.LobbyLeave);
			_lobbyLeaveButton = server5.Register<StandardButton>(val10);
			UpdateLayout();
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.BackRequested?.Invoke();
			});
			((Control)_connectButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Client.Connect(asGuest: false, ((TextInputBase)_nicknameInput).get_Text());
			});
			((Control)_connectGuestButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Client.Connect(asGuest: true, ((TextInputBase)_nicknameInput).get_Text());
			});
			((Control)_disconnectButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Client.Disconnect();
			});
			_nicknameInput.add_EnterPressed((EventHandler<EventArgs>)async delegate
			{
				if (Client.State == RacingClient.ClientState.Online)
				{
					using (Client.Server.Lock())
					{
						await Client.UpdateNickname(((TextInputBase)_nicknameInput).get_Text());
					}
				}
			});
			((Control)_updateNicknameButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				using (Client.Server.Lock())
				{
					await Client.UpdateNickname(((TextInputBase)_nicknameInput).get_Text());
				}
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
			StandardButton connectButton = _connectButton;
			bool visible;
			((Control)_connectGuestButton).set_Visible(visible = state != RacingClient.ClientState.Online);
			((Control)connectButton).set_Visible(visible);
			StandardButton disconnectButton = _disconnectButton;
			((Control)_updateNicknameButton).set_Visible(visible = state != RacingClient.ClientState.Offline);
			((Control)disconnectButton).set_Visible(visible);
			StandardButton connectButton2 = _connectButton;
			((Control)_connectGuestButton).set_Enabled(visible = state == RacingClient.ClientState.Offline);
			((Control)connectButton2).set_Enabled(visible);
			((Control)_disconnectButton).set_Enabled(state == RacingClient.ClientState.Online);
			TextBox nicknameInput = _nicknameInput;
			((Control)_updateNicknameButton).set_Enabled(visible = state != RacingClient.ClientState.Connecting);
			((Control)nicknameInput).set_Enabled(visible);
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
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			ClientStateChanged(Client.State);
			((Control)_backButton).set_Location(Point.get_Zero());
			((Control)(object)_backButton).AlignMiddle();
			((Control)(object)_backButton).ArrangeLeftRight(10, (Control)_pingLabel);
			((Control)(object)_pingLabel).ArrangeLeftRight(40, (Control)_nicknameInput);
			((Control)(object)_nicknameInput).ArrangeLeftRight(10, (Control)_connectButton);
			((Control)(object)_nicknameInput).ArrangeLeftRight(10, (Control)_connectGuestButton);
			StandardButton updateNicknameButton = _updateNicknameButton;
			StandardButton connectButton = _connectButton;
			int num;
			((Control)_connectGuestButton).set_Top(num = ((Container)this).get_ContentRegion().Height / 2);
			int top;
			((Control)connectButton).set_Bottom(top = num);
			((Control)updateNicknameButton).set_Top(top);
			((Control)(object)_nicknameInput).MiddleWith((Control)(object)_connectButton);
			((Control)_disconnectButton).set_Location(((Control)_connectButton).get_Location());
			((Control)(object)_updateNicknameButton).CenterWith((Control)(object)_nicknameInput);
			StandardButton lobbyCreateButton = _lobbyCreateButton;
			StandardButton lobbyJoinButton = _lobbyJoinButton;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)lobbyJoinButton).set_Right(top = ((Rectangle)(ref contentRegion)).get_Right());
			((Control)lobbyCreateButton).set_Right(top);
			StandardButton lobbyJoinButton2 = _lobbyJoinButton;
			((Control)_lobbyCreateButton).set_Top(top = ((Container)this).get_ContentRegion().Height / 2);
			((Control)lobbyJoinButton2).set_Bottom(top);
			((Control)(object)_lobbyJoinButton).ArrangeRightLeft(10, (Control)_lobbyIdInput);
			((Control)(object)_lobbyIdInput).MiddleWith((Control)(object)_lobbyJoinButton);
			((Control)_lobbyLeaveButton).set_Location(((Control)_lobbyJoinButton).get_Location());
			((Control)(object)_lobbyLeaveButton).AlignMiddle();
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
