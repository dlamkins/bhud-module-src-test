using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.RacingMeter.Lib;
using Ideka.RacingMeter.Lib.RacingServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class LobbyLeaderboardPanel : Container
	{
		private static readonly Color FinishedColor = Color.get_Yellow() * 0.05f;

		private readonly RacingClient Client;

		private readonly List<MenuItem> _itemList = new List<MenuItem>();

		private readonly Panel _panel;

		private readonly Scrollbar _scrollbar;

		private readonly ReMenu _menu;

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
			UpdateLayout();
			Client.RaceStarted += new Action(RaceStarted);
			Client.RaceCanceled += new Action(RaceCanceled);
			Client.UserUpdated += new Action<User, bool>(UserUpdated);
			Client.PositionUpdated += new Action<User>(PositionUpdated);
		}

		private void RaceStarted()
		{
			UpdatePositions();
		}

		private void RaceCanceled()
		{
			UpdatePositions();
		}

		private void UserUpdated(User user, bool leaving)
		{
			UpdatePositions();
		}

		private void PositionUpdated(User user)
		{
			UpdatePositions();
		}

		private void UpdatePositions()
		{
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			Lobby lobby = Client?.Lobby;
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
				foreach (var (racer, percent) in from r in lobby.Racers
					select (r, route.ProgressPercent(r.RacerData, lobby.Settings.Laps)) into r
					orderby r.percent descending, r.racer.RacerData.Times.Last()
					select r)
				{
					if (racer.RacerData.RaceReady || racer.RacerData.Times.Any())
					{
						MenuItem item;
						if (_itemList.Count <= i)
						{
							SaveScroll(2);
							OnelineMenuItem onelineMenuItem = new OnelineMenuItem("");
							((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
							item = (MenuItem)(object)onelineMenuItem;
							_itemList.Add(item);
						}
						else
						{
							item = _itemList[i];
						}
						item.set_Text($"{i + 1}. {racer.Id}");
						((Control)item).set_BasicTooltipText(racer.Id);
						((Control)item).set_BackgroundColor((percent < 1.0) ? Color.get_Transparent() : FinishedColor);
						i++;
					}
				}
				foreach (MenuItem item2 in _itemList.Skip(i).ToList())
				{
					_itemList.RemoveAt(i);
					SaveScroll(2);
					((Control)item2).Dispose();
				}
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
			}
		}

		protected override void DisposeControl()
		{
			Client.RaceStarted -= new Action(RaceStarted);
			Client.RaceCanceled -= new Action(RaceCanceled);
			Client.UserUpdated -= new Action<User, bool>(UserUpdated);
			Client.PositionUpdated -= new Action<User>(PositionUpdated);
			((Container)this).DisposeControl();
		}
	}
}
