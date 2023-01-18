using Blish_HUD;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RaceOnline : RaceHolder
	{
		public const double MinCheckpointAlpha = 0.1;

		private FullRace _fullRace;

		private readonly RacingClient Client;

		private readonly RaceDrawer Drawer;

		public override FullRace FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				_fullRace = value;
				Drawer.LoadRace(_fullRace?.Race);
			}
		}

		public RaceOnline()
		{
			Client = new RacingClient();
			Drawer = new RaceDrawer(this);
			RacingModule.MetaPanel.PanelOverride = new OnlinePanel(Client);
			Client.LobbyRaceUpdated += RaceUpdated;
			Client.Connect();
		}

		private void RaceUpdated(FullRace race)
		{
			FullRace = race;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			if (!CanDraw())
			{
				return;
			}
			Lobby lobby = Client.Lobby;
			if (lobby == null)
			{
				return;
			}
			foreach (User user2 in lobby.Users.Values)
			{
				if (user2.IsOnline && user2.LobbyData.IsRacer && user2.RacerData.Sent && user2.RacerData.MapId == GameService.Gw2Mumble.get_CurrentMap().get_Id() && !(user2.Id == Client.UserId))
				{
					DrawRaceArrow(spriteBatch, user2.RacerData.Position, 1f, user2.RacerData.Position + user2.RacerData.Front, Color.get_White());
				}
			}
			if (!CanDrawRace())
			{
				return;
			}
			User user = Client.User;
			if (user != null && user.LobbyData.IsRacer)
			{
				if (!user.RacerData.RaceReady)
				{
					Drawer.DrawStart(spriteBatch);
				}
				else
				{
					Drawer.DrawPoints(spriteBatch, lobby.IsRunning ? user.RacerData.Times.Count : 0, lobby.Settings.Laps);
				}
			}
		}

		public override void DrawToMap(SpriteBatch spriteBatch, MapBounds map)
		{
		}

		protected override void DisposeControl()
		{
			if (RacingModule.MetaPanel.PanelOverride is OnlinePanel)
			{
				RacingModule.MetaPanel.PanelOverride = null;
			}
			Client.LobbyRaceUpdated -= RaceUpdated;
			Drawer?.Dispose();
			Client?.Dispose();
			base.DisposeControl();
		}
	}
}
