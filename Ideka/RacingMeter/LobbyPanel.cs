using Blish_HUD.Controls;
using Ideka.BHUDCommon;

namespace Ideka.RacingMeter
{
	public class LobbyPanel : Container
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly LobbyLeftPanel _leftPanel;

		private readonly LobbyDetailsPanel _detailsPanel;

		private readonly LobbyRaceInfoPanel _raceInfoPanel;

		private readonly LobbyRaceActionsPanel _raceActionsPanel;

		private readonly LobbyLeaderboardPanel _leaderboardPanel;

		public LobbyPanel(RacingClient client)
			: this()
		{
			Client = client;
			LobbyLeftPanel lobbyLeftPanel = new LobbyLeftPanel(Client);
			((Control)lobbyLeftPanel).set_Parent((Container)(object)this);
			_leftPanel = lobbyLeftPanel;
			LobbyDetailsPanel lobbyDetailsPanel = new LobbyDetailsPanel(Client);
			((Control)lobbyDetailsPanel).set_Parent((Container)(object)this);
			_detailsPanel = lobbyDetailsPanel;
			LobbyRaceInfoPanel lobbyRaceInfoPanel = new LobbyRaceInfoPanel(Client);
			((Control)lobbyRaceInfoPanel).set_Parent((Container)(object)this);
			_raceInfoPanel = lobbyRaceInfoPanel;
			LobbyRaceActionsPanel lobbyRaceActionsPanel = new LobbyRaceActionsPanel(Client);
			((Control)lobbyRaceActionsPanel).set_Parent((Container)(object)this);
			_raceActionsPanel = lobbyRaceActionsPanel;
			LobbyLeaderboardPanel lobbyLeaderboardPanel = new LobbyLeaderboardPanel(Client);
			((Control)lobbyLeaderboardPanel).set_Parent((Container)(object)this);
			_leaderboardPanel = lobbyLeaderboardPanel;
			UpdateLayout();
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
				((Control)_leftPanel).set_Width(250);
				((Control)(object)_leftPanel).HeightFillDown();
				((Control)(object)_leftPanel).ArrangeLeftRight(10, (Control)_detailsPanel, (Control)_leaderboardPanel);
				((Control)(object)_leaderboardPanel).WidthFillRight();
				((Control)(object)_leaderboardPanel).HeightFillDown();
				LobbyDetailsPanel detailsPanel = _detailsPanel;
				LobbyRaceInfoPanel raceInfoPanel = _raceInfoPanel;
				int num;
				((Control)_raceActionsPanel).set_Width(num = 250);
				int width;
				((Control)raceInfoPanel).set_Width(width = num);
				((Control)detailsPanel).set_Width(width);
				((Control)(object)_detailsPanel).ArrangeTopDown(10, (Control)_raceInfoPanel, (Control)_raceActionsPanel);
			}
		}
	}
}
