using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RacesPanel : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<RacesPanel>();

		private const int Spacing = 10;

		private readonly FlowPanel _panel;

		private readonly Scrollbar _scrollbar;

		private readonly RacesMenu _remoteRacesMenu;

		private readonly RacesMenu _localRacesMenu;

		private readonly StandardButton _updateRacesButton;

		private readonly StandardButton _raceEditorButton;

		private (int frames, float target) _scrollTarget;

		public event Action<FullRace?>? RaceSelected;

		public event Action? RaceEditorRequested;

		public RacesPanel()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			_panel = val;
			_scrollbar = ((IEnumerable)((Container)this).get_Children()).OfType<Scrollbar>().First();
			RacesMenu racesMenu = new RacesMenu();
			((Control)racesMenu).set_Parent((Container)(object)_panel);
			((Panel)racesMenu).set_Title(Strings.Races);
			_remoteRacesMenu = racesMenu;
			RacesMenu racesMenu2 = new RacesMenu();
			((Control)racesMenu2).set_Parent((Container)(object)_panel);
			((Panel)racesMenu2).set_Title(Strings.LocalRaces);
			_localRacesMenu = racesMenu2;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.UpdateRaces);
			_updateRacesButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Strings.RaceEditor);
			_raceEditorButton = val3;
			UpdateLayout();
			_remoteRacesMenu.SizeChanging += delegate
			{
				SaveScroll();
			};
			_remoteRacesMenu.ItemSelected += new Action<FullRace>(OnRaceSelected);
			_localRacesMenu.SizeChanging += delegate
			{
				SaveScroll();
			};
			_localRacesMenu.ItemSelected += new Action<FullRace>(OnRaceSelected);
			((Control)_updateRacesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_updateRacesButton).set_Enabled(false);
				RacingModule.LocalData.ReloadRaces();
				RacingModule.Server.UpdateRaces().Done(Logger, Strings.ErrorRacesLoad).ContinueWith(delegate
				{
					bool result;
					((Control)_updateRacesButton).set_Enabled(result = true);
					return result;
				});
			});
			((Control)_raceEditorButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.RaceEditorRequested?.Invoke();
			});
			_remoteRacesMenu.SetRaces(RacingModule.Server.RemoteRaces.Races.Values);
			RacingModule.Server.RemoteRacesChanged += new Action<RemoteRaces>(RemoteRacesChanged);
			RacingModule.LocalData.RacesChanged += new Action<IReadOnlyDictionary<string, FullRace>>(RacesChanged);
			RacingModule.LocalData.ReloadRaces();
		}

		public void SelectRace(FullRace? race)
		{
			_remoteRacesMenu.Selected = race;
			_localRacesMenu.Selected = race;
		}

		private void RacesChanged(IReadOnlyDictionary<string, FullRace> races)
		{
			_localRacesMenu.SetRaces(races.Values);
		}

		private void RemoteRacesChanged(RemoteRaces remoteRaces)
		{
			_remoteRacesMenu.SetRaces(remoteRaces.Races.Values);
		}

		private void OnRaceSelected(FullRace? race)
		{
			_remoteRacesMenu.Selected = race;
			_localRacesMenu.Selected = race;
			this.RaceSelected?.Invoke(race);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null)
			{
				StandardButton updateRacesButton = _updateRacesButton;
				int height;
				((Control)_raceEditorButton).set_Bottom(height = ((Container)this).get_ContentRegion().Height);
				((Control)updateRacesButton).set_Bottom(height);
				((Control)_panel).set_Location(Point.get_Zero());
				((Control)_panel).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)_panel).set_Height(((Control)_updateRacesButton).get_Top() - 10);
				StandardButton updateRacesButton2 = _updateRacesButton;
				((Control)_raceEditorButton).set_Width(height = (int)((float)(((Container)this).get_ContentRegion().Width - 10) / 2f));
				((Control)updateRacesButton2).set_Width(height);
				((Control)_updateRacesButton).set_Left(0);
				((Control)_raceEditorButton).set_Left(((Control)_updateRacesButton).get_Right() + 10);
				RacesMenu remoteRacesMenu = _remoteRacesMenu;
				((Control)_localRacesMenu).set_Width(height = ((Container)this).get_ContentRegion().Width - 20);
				((Control)remoteRacesMenu).set_Width(height);
			}
		}

		private void SaveScroll(int frames = 2)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_scrollTarget = (frames, _scrollbar.get_ScrollDistance() * (float)(((Control)_localRacesMenu).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_scrollTarget.frames > 0)
			{
				_scrollbar.set_ScrollDistance(_scrollTarget.target / (float)(((Control)_localRacesMenu).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
				_scrollTarget.frames--;
			}
		}

		protected override void DisposeControl()
		{
			RacingModule.Server.RemoteRacesChanged -= new Action<RemoteRaces>(RemoteRacesChanged);
			RacingModule.LocalData.RacesChanged -= new Action<IReadOnlyDictionary<string, FullRace>>(RacesChanged);
			((Container)this).DisposeControl();
		}
	}
}
