using System;
using System.Collections.Concurrent;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LobbyUsersPanel : Panel
	{
		private static readonly Color SelfColor = Color.get_Blue() * 0.05f;

		private static readonly Color OfflineColor = Color.get_DarkGray() * 0.05f;

		private readonly ConcurrentDictionary<string, MenuItem> _userItems = new ConcurrentDictionary<string, MenuItem>();

		private readonly RacingClient Client;

		private readonly ReMenu _menu;

		private readonly LobbyUserMenu _userMenu;

		public event Action<int>? SaveScroll;

		public LobbyUsersPanel(RacingClient client)
			: this()
		{
			Client = client;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Panel)this).set_CanCollapse(true);
			((Panel)this).set_Title(Strings.LobbyUsers);
			ReMenu reMenu = new ReMenu();
			((Control)reMenu).set_Parent((Container)(object)this);
			((Container)reMenu).set_WidthSizingMode((SizingMode)2);
			((Container)reMenu).set_HeightSizingMode((SizingMode)1);
			_menu = reMenu;
			_userMenu = new LobbyUserMenu(Client);
			Client.LobbyChanged += new Action<Lobby>(LobbyChanged);
			Client.UserUpdated += new Action<User, bool>(UserUpdated);
		}

		private void LobbyChanged(Lobby? lobby)
		{
			foreach (MenuItem value in _userItems.Values)
			{
				((Control)value).Dispose();
			}
			_userItems.Clear();
			if (lobby == null)
			{
				return;
			}
			foreach (User user in lobby!.Users.Values)
			{
				UserUpdated(user, leaving: false);
			}
		}

		private void UserUpdated(User user, bool leaving)
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			User user2 = user;
			if (!_userItems.TryGetValue(user2.Id, out var item))
			{
				if (leaving)
				{
					return;
				}
				ConcurrentDictionary<string, MenuItem> userItems = _userItems;
				string id = user2.Id;
				OnelineMenuItem onelineMenuItem = new OnelineMenuItem("");
				((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
				MenuItem value = (MenuItem)(object)onelineMenuItem;
				item = (MenuItem)(object)onelineMenuItem;
				userItems[id] = value;
				this.SaveScroll?.Invoke(2);
				((Control)item).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					_userMenu.Show(user2.Id, (Control?)(object)item);
				});
			}
			if (leaving)
			{
				if (_userItems.TryRemove(user2.Id, out item))
				{
					((Control)item).Dispose();
					this.SaveScroll?.Invoke(2);
				}
				return;
			}
			((Control)item).set_BackgroundColor((user2.Id == Client.UserId) ? SelfColor : ((!user2.IsOnline) ? OfflineColor : Color.get_Transparent()));
			item.set_Text("[" + (user2.LobbyData.IsHost ? "H" : "") + (user2.LobbyData.IsRacer ? "R" : "") + "] " + user2.Id);
			((Control)item).set_BasicTooltipText(user2.Id);
		}

		protected override void DisposeControl()
		{
			Client.LobbyChanged -= new Action<Lobby>(LobbyChanged);
			Client.UserUpdated -= new Action<User, bool>(UserUpdated);
			_userMenu.Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
