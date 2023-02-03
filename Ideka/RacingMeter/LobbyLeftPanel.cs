using System;
using System.Collections;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class LobbyLeftPanel : Container
	{
		private readonly RacingClient Client;

		private readonly FlowPanel _panel;

		private readonly Scrollbar _scrollbar;

		private readonly LobbyUsersPanel _usersPanel;

		private readonly LobbyRacesPanel _racesPanel;

		private (int frames, float target) _scrollTarget;

		public LobbyLeftPanel(RacingClient client)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			Client = client;
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			_panel = val;
			_scrollbar = ((IEnumerable)((Container)this).get_Children()).OfType<Scrollbar>().First();
			LobbyUsersPanel lobbyUsersPanel = new LobbyUsersPanel(Client);
			((Control)lobbyUsersPanel).set_Parent((Container)(object)_panel);
			_usersPanel = lobbyUsersPanel;
			LobbyRacesPanel lobbyRacesPanel = new LobbyRacesPanel(Client);
			((Control)lobbyRacesPanel).set_Parent((Container)(object)_panel);
			_racesPanel = lobbyRacesPanel;
			_usersPanel.SaveScroll += new Action<int>(saveScroll);
			_racesPanel.SaveScroll += new Action<int>(saveScroll);
			UpdateLayout();
			void saveScroll(int frames)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				_scrollTarget = (frames, _scrollbar.get_ScrollDistance() * (float)(((Control)_racesPanel).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null)
			{
				((Control)_panel).set_Location(Point.get_Zero());
				((Control)_panel).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)(object)_panel).HeightFillDown();
				LobbyUsersPanel usersPanel = _usersPanel;
				int width;
				((Control)_racesPanel).set_Width(width = ((Container)_panel).get_ContentRegion().Width - 20);
				((Control)usersPanel).set_Width(width);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_scrollTarget.frames > 0)
			{
				_scrollbar.set_ScrollDistance(_scrollTarget.target / (float)(((Control)_racesPanel).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
				_scrollTarget.frames--;
			}
		}
	}
}
