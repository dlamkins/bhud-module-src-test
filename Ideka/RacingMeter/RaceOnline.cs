using System;
using Blish_HUD;
using Ideka.BHUDCommon;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RaceOnline : RaceDrawerWorld
	{
		private FullRace? _fullRace;

		private readonly RacingClient Client;

		private RaceRunFx? _runFx;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				_fullRace = value;
				Race race = _fullRace?.Race;
				if (race == null)
				{
					_runFx?.Dispose();
					_runFx = null;
					return;
				}
				RaceRunFx runFx = _runFx;
				if (runFx != null)
				{
					runFx.LoadRace(race);
				}
				else
				{
					_runFx = new RaceRunFx(this, race);
				}
			}
		}

		public override Race? Race => FullRace?.Race;

		public RaceOnline(RacingClient client)
		{
			Client = client;
			Client.LobbyRaceUpdated += new Action<FullRace>(RaceUpdated);
		}

		private void RaceUpdated(FullRace? race)
		{
			FullRace = race;
		}

		protected override void DrawRaceToWorld(SpriteBatch spriteBatch)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
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
			RaceRunFx runFx = _runFx;
			if (runFx == null)
			{
				return;
			}
			User user = Client.User;
			if (user != null && user.LobbyData.IsRacer)
			{
				if (!user.RacerData.RaceReady)
				{
					runFx.DrawStart(spriteBatch);
				}
				else
				{
					runFx.DrawPoints(spriteBatch, lobby.IsRunning ? user.RacerData.Times.Count : 0, lobby.Settings.Laps);
				}
			}
		}

		protected override void DrawRaceToMap(SpriteBatch spriteBatch, IMapBounds map)
		{
		}

		protected override void DisposeControl()
		{
			Client.LobbyRaceUpdated -= new Action<FullRace>(RaceUpdated);
			_runFx?.Dispose();
			base.DisposeControl();
		}
	}
}
