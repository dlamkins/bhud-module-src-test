using System;
using Blish_HUD.Controls;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class MainPanel : Panel, IPanelOverride
	{
		private const int Spacing = 10;

		private FullRace _race;

		private FullGhost _ghost;

		private readonly TopBar _topBar;

		private readonly RacesPanel _racesPanel;

		private readonly GhostsPanel _ghostsPanel;

		private readonly RaceInfoPanel _raceInfoPanel;

		private readonly GhostInfoPanel _ghostInfoPanel;

		private readonly RacePreview _racePreview;

		public FullRace Race
		{
			get
			{
				return _race;
			}
			private set
			{
				GhostsPanel ghostsPanel = _ghostsPanel;
				GhostInfoPanel ghostInfoPanel = _ghostInfoPanel;
				RaceInfoPanel raceInfoPanel = _raceInfoPanel;
				FullRace fullRace2 = (_racePreview.FullRace = value);
				FullRace fullRace4 = (raceInfoPanel.Race = fullRace2);
				FullRace fullRace6 = (ghostInfoPanel.Race = fullRace4);
				FullRace fullRace8 = (_race = (ghostsPanel.Race = fullRace6));
			}
		}

		public FullGhost Ghost
		{
			get
			{
				return _ghost;
			}
			private set
			{
				GhostInfoPanel ghostInfoPanel = _ghostInfoPanel;
				FullGhost fullGhost2 = (_racePreview.FullGhost = value);
				FullGhost fullGhost4 = (_ghost = (ghostInfoPanel.Ghost = fullGhost2));
			}
		}

		public Panel Panel => (Panel)(object)this;

		public Texture2D Icon { get; } = RacingModule.ContentsManager.GetTexture("Icon.png");


		public string Caption => Strings.RacingMeter;

		public MainPanel()
			: this()
		{
			TopBar topBar = new TopBar();
			((Control)topBar).set_Parent((Container)(object)this);
			_topBar = topBar;
			RacesPanel racesPanel = new RacesPanel();
			((Control)racesPanel).set_Parent((Container)(object)this);
			_racesPanel = racesPanel;
			GhostsPanel ghostsPanel = new GhostsPanel();
			((Control)ghostsPanel).set_Parent((Container)(object)this);
			_ghostsPanel = ghostsPanel;
			RacePreview racePreview = new RacePreview();
			((Control)racePreview).set_Parent((Container)(object)this);
			_racePreview = racePreview;
			RaceInfoPanel raceInfoPanel = new RaceInfoPanel();
			((Control)raceInfoPanel).set_Parent((Container)(object)this);
			_raceInfoPanel = raceInfoPanel;
			GhostInfoPanel ghostInfoPanel = new GhostInfoPanel();
			((Control)ghostInfoPanel).set_Parent((Container)(object)this);
			_ghostInfoPanel = ghostInfoPanel;
			_racesPanel.RaceChanged += delegate(FullRace race)
			{
				Race = race;
			};
			_ghostsPanel.GhostChanged += delegate(FullGhost ghost)
			{
				Ghost = ghost;
			};
			_ghostInfoPanel.GhostChanged += delegate(FullGhost ghost)
			{
				Ghost = ghost;
			};
			RacingModule.Racer.RaceLoaded += new Action<FullRace>(RaceLoaded);
			RacingModule.Racer.GhostLoaded += new Action<FullGhost>(GhostLoaded);
			UpdateLayout();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			if (_topBar != null)
			{
				((Control)_topBar).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)_topBar).set_Height(50);
				((Control)_racesPanel).set_Left(0);
				((Control)_racesPanel).set_Top(((Control)_topBar).get_Bottom() + 10);
				((Control)_racesPanel).set_Width(240);
				((Control)_racesPanel).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_racesPanel).get_Top());
				((Control)_ghostsPanel).set_Right(((Container)this).get_ContentRegion().Width);
				((Control)_ghostsPanel).set_Top(((Control)_topBar).get_Bottom() + 10);
				((Control)_ghostsPanel).set_Width(240);
				((Control)_ghostsPanel).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_ghostsPanel).get_Top());
				((Control)_racePreview).set_Left(((Control)_racesPanel).get_Right() + 10);
				((Control)_racePreview).set_Top(((Control)_topBar).get_Bottom() + 10);
				((Control)_racePreview).set_Width(((Control)_ghostsPanel).get_Left() - ((Control)_racesPanel).get_Right() - 20);
				((Control)_racePreview).set_Height(400);
				((Control)_raceInfoPanel).set_Left(((Control)_racePreview).get_Left());
				((Control)_raceInfoPanel).set_Top(((Control)_racePreview).get_Bottom() + 10);
				((Control)_raceInfoPanel).set_Width(((Control)_racePreview).get_Width() / 2 - 5);
				((Control)_raceInfoPanel).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_racePreview).get_Bottom() - 10);
				((Control)_ghostInfoPanel).set_Left(((Control)_raceInfoPanel).get_Right() + 10);
				((Control)_ghostInfoPanel).set_Top(((Control)_racePreview).get_Bottom() + 10);
				((Control)_ghostInfoPanel).set_Width(((Control)_racePreview).get_Width() / 2 - 5);
				((Control)_ghostInfoPanel).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_racePreview).get_Bottom() - 10);
			}
		}

		private void RaceLoaded(FullRace race)
		{
			if (Race != race)
			{
				Race = race;
			}
		}

		private void GhostLoaded(FullGhost ghost)
		{
			if (Ghost != ghost)
			{
				Ghost = ghost;
			}
		}

		protected override void DisposeControl()
		{
			RacingModule.Racer.RaceLoaded -= new Action<FullRace>(RaceLoaded);
			RacingModule.Racer.GhostLoaded -= new Action<FullGhost>(GhostLoaded);
			Texture2D icon = Icon;
			if (icon != null)
			{
				((GraphicsResource)icon).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
