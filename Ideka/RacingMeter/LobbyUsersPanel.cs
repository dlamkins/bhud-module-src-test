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

		public event Action<int> SaveScroll;

		public LobbyUsersPanel(RacingClient client)
			: this()
		{
			Client = client;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Panel)this).set_CanCollapse(true);
			((Panel)this).set_Title("Users");
			ReMenu reMenu = new ReMenu();
			((Control)reMenu).set_Parent((Container)(object)this);
			((Container)reMenu).set_WidthSizingMode((SizingMode)2);
			((Container)reMenu).set_HeightSizingMode((SizingMode)1);
			_menu = reMenu;
			_userMenu = new LobbyUserMenu(Client);
			Client.LobbyChanged += LobbyChanged;
			Client.UserUpdated += UserUpdated;
		}

		private void LobbyChanged(Lobby lobby)
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
			foreach (User user in lobby.Users.Values)
			{
				UserUpdated(user, leaving: false);
			}
		}

		private void UserUpdated(User user, bool leaving)
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			if (!_userItems.TryGetValue(user.Id, out var item))
			{
				if (leaving)
				{
					return;
				}
				ConcurrentDictionary<string, MenuItem> userItems = _userItems;
				string id = user.Id;
				OnelineMenuItem onelineMenuItem = new OnelineMenuItem("");
				((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
				MenuItem value = (MenuItem)(object)onelineMenuItem;
				item = (MenuItem)(object)onelineMenuItem;
				userItems[id] = value;
				this.SaveScroll?.Invoke(2);
				((Control)item).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					_userMenu.Show(user.Id, (Control)(object)item);
				});
			}
			if (leaving)
			{
				if (_userItems.TryRemove(user.Id, out item))
				{
					((Control)item).Dispose();
					this.SaveScroll?.Invoke(2);
				}
				return;
			}
			((Control)item).set_BackgroundColor((user.Id == Client.UserId) ? SelfColor : ((!user.IsOnline) ? OfflineColor : Color.get_Transparent()));
			item.set_Text("[" + (user.LobbyData.IsHost ? "H" : "") + (user.LobbyData.IsRacer ? "R" : "") + "] " + user.Id);
			((Control)item).set_BasicTooltipText(user.Id);
		}

		protected override void DisposeControl()
		{
			Client.LobbyChanged -= LobbyChanged;
			Client.UserUpdated -= UserUpdated;
			_userMenu.Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
