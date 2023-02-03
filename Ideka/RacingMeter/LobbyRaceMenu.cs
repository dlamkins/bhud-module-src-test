using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;

namespace Ideka.RacingMeter
{
	public class LobbyRaceMenu : IDisposable
	{
		private readonly RacingClient Client;

		private readonly ContextMenuStrip _menu;

		private readonly ContextMenuStripItem _raceName;

		private readonly ContextMenuStripItem _setRace;

		private FullRace? _selected;

		private Control? _control;

		private RacingServer Server => Client.Server;

		public LobbyRaceMenu(RacingClient client)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			Client = client;
			ContextMenuStripItem val = new ContextMenuStripItem();
			((Control)val).set_Enabled(false);
			_raceName = val;
			_setRace = Server.Register<ContextMenuStripItem>(new ContextMenuStripItem("Set this Race"));
			((Control)_setRace).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				FullRace race = _selected;
				if (race != null)
				{
					using (Client.Server.Lock())
					{
						await Server.SetLobbyRace(race);
					}
				}
			});
			_menu = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)menu);
			IEnumerable<ContextMenuStripItem> menu()
			{
				yield return _raceName;
				yield return _setRace;
			}
		}

		public void Show(FullRace race, Control? control)
		{
			if (race != null)
			{
				User? user = Client.User;
				if (user != null && user!.LobbyData.IsHost)
				{
					_selected = race;
					_control = control;
					_raceName.set_Text(race.Race.Name);
					_menu.Show(_control);
					return;
				}
			}
			Hide();
		}

		public void Hide()
		{
			_selected = null;
			((Control)_menu).Hide();
		}

		public void Dispose()
		{
			((Control)_menu).Dispose();
			((Control)_setRace).Dispose();
		}
	}
}
