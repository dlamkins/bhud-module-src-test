using System;
using Blish_HUD.Controls;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class OnlinePanel : Panel, IUIPanel
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly RaceOnline _online;

		private readonly LobbyBar _lobbyBar;

		private readonly LobbyPanel _lobbyPanel;

		public Panel Panel => (Panel)(object)this;

		public Texture2D Icon { get; } = RacingModule.ContentsManager.GetTexture("OnlineIcon.png");


		public string Caption => Strings.OnlineRacing;

		public OnlinePanel(PanelStack panelStack, MeasurerRealtime measurer)
			: this()
		{
			Client = new RacingClient(measurer);
			_online = new RaceOnline(Client);
			LobbyBar lobbyBar = new LobbyBar(Client);
			((Control)lobbyBar).set_Parent((Container)(object)this);
			_lobbyBar = lobbyBar;
			LobbyPanel lobbyPanel = new LobbyPanel(Client);
			((Control)lobbyPanel).set_Parent((Container)(object)this);
			((Control)lobbyPanel).set_Visible(Client.Lobby != null);
			_lobbyPanel = lobbyPanel;
			UpdateLayout();
			_lobbyBar.BackRequested += new Action(panelStack.GoBack);
			Client.LobbyChanged += new Action<Lobby>(LobbyChanged);
			this.SoftChild((Control)(object)_online);
		}

		private void LobbyChanged(Lobby? lobby)
		{
			((Control)_lobbyPanel).set_Visible(lobby != null);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (_lobbyBar != null)
			{
				LobbyBar lobbyBar = _lobbyBar;
				int width;
				((Control)_lobbyPanel).set_Width(width = ((Container)this).get_ContentRegion().Width);
				((Control)lobbyBar).set_Width(width);
				((Control)_lobbyBar).set_Height(50);
				((Control)(object)_lobbyBar).ArrangeTopDown(10, (Control)_lobbyPanel);
				((Control)(object)_lobbyPanel).HeightFillDown();
			}
		}

		protected override void DisposeControl()
		{
			((GraphicsResource)Icon).Dispose();
			Client.Dispose();
			((Control)_online).Dispose();
			Client.LobbyChanged -= new Action<Lobby>(LobbyChanged);
			((Panel)this).DisposeControl();
		}
	}
}
