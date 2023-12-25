using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class LobbyLeaderboardPanel : Container
	{
		private readonly RacingClient Client;

		private readonly List<ProgressMenuItem> _itemList = new List<ProgressMenuItem>();

		private readonly Panel _panel;

		private readonly Scrollbar _scrollbar;

		private readonly ReMenu _menu;

		private readonly GlowButton _configButton;

		private readonly LobbyConfigMenu _configMenu;

		private (int frames, float target) _scrollTarget;

		public LobbyLeaderboardPanel(RacingClient client)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			Client = client;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_CanScroll(true);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_Title(Strings.LobbyLeaderboard);
			_panel = val;
			_scrollbar = ((IEnumerable)((Container)this).get_Children()).OfType<Scrollbar>().First();
			ReMenu reMenu = new ReMenu();
			((Control)reMenu).set_Parent((Container)(object)_panel);
			((Container)reMenu).set_WidthSizingMode((SizingMode)2);
			((Container)reMenu).set_HeightSizingMode((SizingMode)1);
			_menu = reMenu;
			GlowButton val2 = new GlowButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Icon(AsyncTexture2D.FromAssetId(157109));
			val2.set_ActiveIcon(AsyncTexture2D.FromAssetId(157110));
			_configButton = val2;
			_configMenu = new LobbyConfigMenu();
			UpdateLayout();
			((Control)_configButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_configMenu.Show((Control?)(object)_configButton);
			});
			Client.LobbyChanged += new Action<Lobby>(LobbyChanged);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			Lobby lobby = Client.Lobby;
			if (lobby == null)
			{
				return;
			}
			Race race = lobby.FullRace?.Race;
			if (race == null)
			{
				return;
			}
			RaceRouteData route = Client.Route;
			if (route == null || route.Race != race)
			{
				return;
			}
			lock (_itemList)
			{
				int i = 0;
				foreach (var (racer, percent) in lobby.GetLeaderboard(route))
				{
					if (!racer.RacerData.RaceReady && !racer.RacerData.Times.Any())
					{
						continue;
					}
					ProgressMenuItem item;
					if (_itemList.Count <= i)
					{
						SaveScroll(2);
						ProgressMenuItem progressMenuItem = new ProgressMenuItem("");
						((Control)progressMenuItem).set_Parent((Container)(object)_menu);
						item = progressMenuItem;
						_itemList.Add(item);
					}
					else
					{
						item = _itemList[i];
					}
					ProgressMenuItem progressMenuItem2 = item;
					string text = $"{i + 1}. {racer.DisplayName}";
					object obj;
					if (percent >= 1.0 || lobby.IsAfterRace(racer.RacerData))
					{
						RacerTime time = racer.RacerData.LastTime;
						if (time != null)
						{
							obj = " [" + time.Time.Formatted() + "]";
							goto IL_015f;
						}
					}
					obj = "";
					goto IL_015f;
					IL_015f:
					((MenuItem)progressMenuItem2).set_Text(text + (string)obj);
					((Control)item).set_BasicTooltipText(racer.IsGuest ? StringExtensions.Format(Strings.LabelGuest, racer.DisplayName) : racer.Id);
					item.Progress = (lobby.IsAfterRace(racer.RacerData) ? 1.0 : percent);
					i++;
				}
				foreach (ProgressMenuItem item2 in _itemList.Skip(i).ToList())
				{
					_itemList.RemoveAt(i);
					SaveScroll(2);
					((Control)item2).Dispose();
				}
			}
		}

		private void LobbyChanged(Lobby? obj)
		{
			lock (_itemList)
			{
				foreach (ProgressMenuItem item in _itemList)
				{
					((Control)item).Dispose();
				}
				_itemList.Clear();
			}
		}

		private void SaveScroll(int frames)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_scrollTarget = (frames, _scrollbar.get_ScrollDistance() * (float)(((Control)_menu).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_scrollTarget.frames > 0)
			{
				_scrollTarget.frames--;
			}
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
				((Control)(object)_panel).WidthFillRight();
				((Control)(object)_panel).HeightFillDown();
				((Control)_configButton).set_Top(((Control)_panel).get_Top() + 2);
				((Control)_configButton).set_Right(((Control)_panel).get_Right());
			}
		}

		protected override void DisposeControl()
		{
			Client.LobbyChanged -= new Action<Lobby>(LobbyChanged);
			_configMenu.Dispose();
			((Container)this).DisposeControl();
		}
	}
}
