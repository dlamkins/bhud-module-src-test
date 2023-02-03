using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib.RacingServer;

namespace Ideka.RacingMeter
{
	public class LobbyUserMenu : IDisposable
	{
		private readonly RacingClient Client;

		private readonly ContextMenuStrip _menu;

		private readonly ContextMenuStripItem _userName;

		private readonly ContextMenuStripItem _isRacer;

		private readonly ContextMenuStripItem _isHost;

		private readonly ContextMenuStripItem _kick;

		private User? _selected;

		private Control? _control;

		private RacingServer Server => Client.Server;

		public LobbyUserMenu(RacingClient client)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			Client = client;
			ContextMenuStripItem val = new ContextMenuStripItem();
			((Control)val).set_Enabled(false);
			_userName = val;
			RacingServer server = Server;
			ContextMenuStripItem val2 = new ContextMenuStripItem("Is Racer");
			val2.set_CanCheck(true);
			_isRacer = server.Register<ContextMenuStripItem>(val2);
			_isRacer.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate
			{
				bool value2 = _isRacer.get_Checked();
				User selected2 = _selected;
				if (selected2 != null && value2 != selected2.LobbyData.IsRacer)
				{
					_isRacer.set_Checked(selected2.LobbyData.IsRacer);
					using (Client.Server.Lock())
					{
						await Server.SetRacer(selected2.Id, value2);
					}
				}
			});
			RacingServer server2 = Server;
			ContextMenuStripItem val3 = new ContextMenuStripItem("Is Host");
			val3.set_CanCheck(true);
			_isHost = server2.Register<ContextMenuStripItem>(val3);
			_isHost.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate
			{
				bool value = _isHost.get_Checked();
				User selected = _selected;
				if (selected != null && value != selected.LobbyData.IsHost)
				{
					_isHost.set_Checked(selected.LobbyData.IsHost);
					using (Client.Server.Lock())
					{
						await Server.SetHost(selected.Id, value);
					}
				}
			});
			_kick = Server.Register<ContextMenuStripItem>(new ContextMenuStripItem("Kick"));
			((Control)_kick).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				string userId = _selected?.Id;
				if (userId != null)
				{
					using (Client.Server.Lock())
					{
						await Server.KickUser(userId);
					}
				}
			});
			_menu = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)menu);
			Client.LobbyChanged += new Action<Lobby>(LobbyChanged);
			Client.UserUpdated += new Action<User, bool>(UserUpdated);
			IEnumerable<ContextMenuStripItem> menu()
			{
				yield return _userName;
				yield return _isRacer;
				yield return _isHost;
				yield return _kick;
			}
		}

		private void LobbyChanged(Lobby? lobby)
		{
			if (_selected != null && (lobby == null || !lobby!.Users.TryGetValue(_selected!.Id, out var _)))
			{
				Hide();
			}
		}

		private void UserUpdated(User user, bool leaving)
		{
			if (!(user.Id != _selected?.Id))
			{
				if (leaving)
				{
					Hide();
				}
				else if (((Control)_menu).get_Visible())
				{
					Show(user.Id, _control);
				}
			}
		}

		public void Show(string userId, Control? control)
		{
			Lobby? lobby = Client.Lobby;
			if (lobby != null && lobby!.Users.TryGetValue(userId, out var user))
			{
				User? user2 = Client.User;
				if (user2 != null && user2!.LobbyData.IsHost)
				{
					_selected = user;
					_control = control;
					_userName.set_Text(user.Id);
					_isRacer.set_Checked(user.LobbyData.IsRacer);
					_isHost.set_Checked(user.LobbyData.IsHost);
					((Control)_kick).set_Visible(userId != Client.User?.Id);
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
			((Control)_isRacer).Dispose();
			((Control)_isHost).Dispose();
			((Control)_kick).Dispose();
			Client.LobbyChanged -= new Action<Lobby>(LobbyChanged);
			Client.UserUpdated -= new Action<User, bool>(UserUpdated);
		}
	}
}
