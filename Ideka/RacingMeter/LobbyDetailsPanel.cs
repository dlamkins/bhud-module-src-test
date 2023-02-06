using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class LobbyDetailsPanel : FlowPanel
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly StringBox _idBox;

		private readonly IntBox _sizeBox;

		private readonly IntBox _lapsBox;

		private RacingServer Server => Client.Server;

		public LobbyDetailsPanel(RacingClient client)
			: this()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Client = client;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_Title(Strings.LobbySettings);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(10f, 10f);
			((FlowPanel)this).set_OuterControlPadding(val);
			((FlowPanel)this).set_ControlPadding(val);
			StringBox stringBox = new StringBox();
			((Control)stringBox).set_Parent((Container)(object)this);
			stringBox.Label = Strings.LobbyId;
			stringBox.ControlEnabled = false;
			_idBox = stringBox;
			RacingServer server = Server;
			IntBox intBox = new IntBox();
			((Control)intBox).set_Parent((Container)(object)this);
			intBox.Label = Strings.LobbyMaxUsers;
			intBox.MinValue = 2;
			intBox.MaxValue = 50;
			_sizeBox = server.Register<IntBox>(intBox);
			RacingServer server2 = Server;
			IntBox intBox2 = new IntBox();
			((Control)intBox2).set_Parent((Container)(object)this);
			intBox2.Label = Strings.LobbyRaceLaps;
			intBox2.MinValue = 1;
			intBox2.MaxValue = 50;
			_lapsBox = server2.Register<IntBox>(intBox2);
			UpdateLayout();
			((Control)_idBox).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!string.IsNullOrEmpty(_idBox.Value))
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_idBox.Value);
					ScreenNotification.ShowNotification(Strings.NoticeCopied, (NotificationType)0, (Texture2D)null, 4);
				}
			});
			_sizeBox.ValueCommitted += async delegate
			{
				Lobby lobby2 = Client.Lobby;
				if (lobby2 != null)
				{
					int newSize = _sizeBox.Value;
					_sizeBox.Value = lobby2.Settings.Size;
					using (Server.Lock())
					{
						await Server.SetLobbySize(newSize);
					}
				}
			};
			_lapsBox.ValueCommitted += async delegate
			{
				Lobby lobby = Client.Lobby;
				if (lobby != null)
				{
					int newLaps = _lapsBox.Value;
					_lapsBox.Value = lobby.Settings.Laps;
					using (Server.Lock())
					{
						await Server.SetLobbyLaps(newLaps);
					}
				}
			};
			Client.LobbyChanged += new Action<Lobby>(LobbyChanged);
			Client.LobbySettingsUpdated += new Action<Lobby>(LobbySettingsUpdated);
		}

		private void LobbyChanged(Lobby? lobby)
		{
			_idBox.Value = lobby?.Id ?? "";
			_idBox.AllBasicTooltipText = ((lobby == null) ? null : Strings.ClickToCopy);
			_sizeBox.Value = lobby?.Settings.Size ?? 0;
			_lapsBox.Value = lobby?.Settings.Laps ?? 0;
		}

		private void LobbySettingsUpdated(Lobby? lobby)
		{
			_sizeBox.Value = lobby?.Settings.Size ?? 0;
			_lapsBox.Value = lobby?.Settings.Laps ?? 0;
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
				((Control)(object)_idBox).WidthFillRight(10);
				((Control)(object)_sizeBox).WidthFillRight(10);
				((Control)(object)_lapsBox).WidthFillRight(10);
				ValueControl.AlignLabels(_idBox, _sizeBox, _lapsBox);
				((Container)(object)this).SetContentRegionHeight(((Control)_lapsBox).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			Client.LobbyChanged -= new Action<Lobby>(LobbyChanged);
			Client.LobbySettingsUpdated -= new Action<Lobby>(LobbySettingsUpdated);
			((FlowPanel)this).DisposeControl();
		}
	}
}
