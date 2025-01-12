using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class MainPanel : Panel, IUIPanel
	{
		private const int Spacing = 10;

		private FullRace? _fullRace;

		private readonly PanelStack _panelStack;

		private readonly RaceRunner _runner;

		private readonly RaceRunnerLoader _runnerLoader;

		private readonly TopBar _topBar;

		private readonly RacesPanel _racesPanel;

		private readonly GhostsPanel _ghostsPanel;

		private readonly RaceInfoPanel _raceInfoPanel;

		private readonly GhostInfoPanel _ghostInfoPanel;

		private readonly RacePreview _racePreview;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			private set
			{
				RaceInfoPanel raceInfoPanel = _raceInfoPanel;
				GhostInfoPanel ghostInfoPanel = _ghostInfoPanel;
				GhostsPanel ghostsPanel = _ghostsPanel;
				FullRace fullRace2 = (_racePreview.FullRace = (_fullRace = value));
				FullRace fullRace4 = (ghostsPanel.FullRace = fullRace2);
				FullRace fullRace7 = (raceInfoPanel.FullRace = (ghostInfoPanel.FullRace = fullRace4));
				_racesPanel.SelectRace(value);
			}
		}

		public Panel Panel => (Panel)(object)this;

		public Texture2D Icon { get; } = RacingModule.ContentsManager.GetTexture("Icon.png");


		public string Caption => Strings.RacingMeter;

		public MainPanel(PanelStack panelStack, IMeasurer measurer)
		{
			IMeasurer measurer2 = measurer;
			((Panel)this)._002Ector();
			MainPanel mainPanel = this;
			_panelStack = panelStack;
			_runner = new RaceRunner(measurer2);
			_runnerLoader = new RaceRunnerLoader(_runner);
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
			UpdateLayout();
			_topBar.OnlineRequested += delegate
			{
				mainPanel._panelStack.Push(new OnlinePanel(mainPanel._panelStack, measurer2));
			};
			_topBar.UnloadRace += delegate
			{
				mainPanel._runnerLoader.LoadRace(null, null);
			};
			_topBar.UnloadGhost += delegate
			{
				mainPanel._runnerLoader.SetGhost(null, lockGhost: true);
			};
			_racesPanel.RaceSelected += delegate(FullRace? race)
			{
				mainPanel.FullRace = race;
			};
			_racesPanel.RaceEditorRequested += delegate
			{
				mainPanel._panelStack.Push(new EditorPanel(mainPanel._panelStack, measurer2, mainPanel.FullRace));
			};
			_ghostsPanel.GhostSelected += delegate(FullGhost? ghost)
			{
				FullGhost fullGhost6 = (mainPanel._ghostInfoPanel.FullGhost = (mainPanel._racePreview.FullGhost = ghost));
			};
			_raceInfoPanel.RaceRequested += delegate(FullRace race)
			{
				mainPanel._runnerLoader.LoadRace(race, null);
			};
			_ghostInfoPanel.GhostChanged += delegate(FullGhost ghost)
			{
				FullGhost fullGhost3 = (mainPanel._ghostInfoPanel.FullGhost = (mainPanel._racePreview.FullGhost = ghost));
			};
			_ghostInfoPanel.GhostRequested += delegate(FullRace race, FullGhost ghost)
			{
				mainPanel._runnerLoader.LoadRace(race, ghost);
			};
			_runnerLoader.RaceLoaded += new Action<FullRace>(_topBar.RaceLoaded);
			_runnerLoader.GhostLoaded += new Action<FullGhost>(_topBar.GhostLoaded);
			RacingModule.LocalData.RacesChanged += new Action<IReadOnlyDictionary<string, FullRace>>(RacesChanged);
			RacingModule.LocalData.RaceCreated += new Action<FullRace>(RaceCreated);
			this.SoftChild((Control)(object)_runner);
		}

		private void RacesChanged(IReadOnlyDictionary<string, FullRace> races)
		{
			string id = FullRace?.Meta.Id;
			if (id != null)
			{
				FullRace = (races.TryGetValue(id, out var fullRace) ? fullRace : null);
			}
		}

		private void RaceCreated(FullRace fullRace)
		{
			FullRace = fullRace;
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

		protected override void DisposeControl()
		{
			_runnerLoader.RaceLoaded -= new Action<FullRace>(_topBar.RaceLoaded);
			_runnerLoader.GhostLoaded -= new Action<FullGhost>(_topBar.GhostLoaded);
			RacingModule.LocalData.RacesChanged -= new Action<IReadOnlyDictionary<string, FullRace>>(RacesChanged);
			RacingModule.LocalData.RaceCreated -= new Action<FullRace>(RaceCreated);
			((GraphicsResource)Icon).Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
