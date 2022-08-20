using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RacesPanel : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<RacesPanel>();

		private const int Spacing = 10;

		private FullRace _race;

		private readonly FlowPanel _panel;

		private readonly Scrollbar _scrollbar;

		private readonly Panel _racesPanel;

		private readonly Menu _racesMenu;

		private readonly Panel _localRacesPanel;

		private readonly Menu _localRacesMenu;

		private readonly StandardButton _updateRacesButton;

		private readonly StandardButton _raceEditorButton;

		private readonly Dictionary<int, MenuItem> _remoteRaceMaps = new Dictionary<int, MenuItem>();

		private readonly Dictionary<int, MenuItem> _localRaceMaps = new Dictionary<int, MenuItem>();

		private (int frames, float target) _scrollTarget;

		public FullRace Race
		{
			get
			{
				return _race;
			}
			set
			{
				_race = value;
				this.RaceChanged?.Invoke(_race);
			}
		}

		public event Action<FullRace> RaceChanged;

		public RacesPanel()
			: this()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			_panel = val;
			_scrollbar = ((IEnumerable)((Container)this).get_Children()).OfType<Scrollbar>().First();
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_panel);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_CanCollapse(true);
			val2.set_Title(Strings.Races);
			_racesPanel = val2;
			Menu val3 = new Menu();
			((Control)val3).set_Parent((Container)(object)_racesPanel);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			val3.set_CanSelect(true);
			_racesMenu = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)_panel);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_CanCollapse(true);
			val4.set_Title(Strings.LocalRaces);
			_localRacesPanel = val4;
			Menu val5 = new Menu();
			((Control)val5).set_Parent((Container)(object)_localRacesPanel);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_CanSelect(true);
			_localRacesMenu = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.UpdateRaces);
			_updateRacesButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(Strings.RaceEditor);
			_raceEditorButton = val7;
			((Control)_updateRacesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!RacingModule.Server.NotifyIfOffline())
				{
					((Control)_updateRacesButton).set_Enabled(false);
					RacingModule.Server.UpdateRaces().Done(Logger, Strings.ErrorRacesLoad).ContinueWith(delegate(Task<TaskUtils.TaskState> task)
					{
						if (!task.Result.Success)
						{
							PopulateRemoteRaces(RacingModule.Server.RemoteRaces.Races.Values);
						}
						((Control)_updateRacesButton).set_Enabled(true);
					});
				}
			});
			((Control)_raceEditorButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (RacingModule.Racer.FullRace != Race)
				{
					RacingModule.Racer.FullRace = Race;
				}
				RacingModule.Racer.EditMode = true;
			});
			RacingModule.Server.RemoteRacesChanged += RemoteRacesChanged;
			RacingModule.Racer.LocalRacesChanged += new Action(LocalRacesChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			PopulateRaces();
			UpdateLayout();
		}

		private void PopulateRemoteRaces(IEnumerable<FullRace> races)
		{
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Expected O, but got Unknown
			FullRace race2 = Race;
			string oldId = ((race2 != null && !race2.IsLocal) ? Race.Meta.Id : null);
			if (oldId != null)
			{
				Race = null;
			}
			_remoteRaceMaps.Clear();
			((Container)_racesMenu).ClearChildren();
			_racesMenu.set_CanSelect(races.Any());
			if (!races.Any())
			{
				_racesMenu.AddMenuItem(Strings.Nothing, (Texture2D)null);
				return;
			}
			foreach (var item in from r in (from r in races
					group r by r.Race.MapId).Select(delegate(IGrouping<int, FullRace> r)
				{
					Map? map = RacingModule.MapData.GetMap(r.Key);
					return ((map != null) ? map!.get_Name() : null, r);
				})
				orderby r.@group.Key != GameService.Gw2Mumble.get_CurrentMap().get_Id(), r.name, r.@group.Key
				select r)
			{
				IGrouping<int, FullRace> group = item.Item2;
				MenuItem val2 = (_remoteRaceMaps[group.Key] = _racesMenu.AddMenuItem(RacingModule.MapData.Describe(group.Key), (Texture2D)null));
				MenuItem mapItem = val2;
				((Control)mapItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SaveScroll(2);
				});
				foreach (FullRace race in group.OrderBy((FullRace r) => r.Race.Name))
				{
					MenuItem val3 = new MenuItem("  " + race.Race.Name);
					((Control)val3).set_Parent((Container)(object)mapItem);
					((Control)val3).set_BasicTooltipText(race.Race.Name);
					MenuItem raceItem = val3;
					raceItem.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
					{
						MenuItem selectedMenuItem = _localRacesMenu.get_SelectedMenuItem();
						if (selectedMenuItem != null)
						{
							selectedMenuItem.Deselect();
						}
						Race = race;
					});
					if (race.Meta.Id == oldId)
					{
						_localRacesMenu.Select(raceItem);
					}
				}
			}
		}

		private void PopulateLocalRaces()
		{
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Expected O, but got Unknown
			string oldId = ((Race?.IsLocal ?? false) ? Race.Meta.Id : null);
			if (oldId != null)
			{
				Race = null;
			}
			_localRaceMaps.Clear();
			((Container)_localRacesMenu).ClearChildren();
			Dictionary<string, FullRace>.ValueCollection races = DataInterface.GetLocalRaces().Values;
			_localRacesMenu.set_CanSelect(races.Any());
			if (!races.Any())
			{
				_localRacesMenu.AddMenuItem(Strings.Nothing, (Texture2D)null);
				return;
			}
			foreach (var item in from r in (from r in races
					group r by r.Race.MapId).Select(delegate(IGrouping<int, FullRace> r)
				{
					Map? map = RacingModule.MapData.GetMap(r.Key);
					return ((map != null) ? map!.get_Name() : null, r);
				})
				orderby r.@group.Key != GameService.Gw2Mumble.get_CurrentMap().get_Id(), r.name, r.@group.Key
				select r)
			{
				IGrouping<int, FullRace> group = item.Item2;
				MenuItem val2 = (_localRaceMaps[group.Key] = _localRacesMenu.AddMenuItem(RacingModule.MapData.Describe(group.Key), (Texture2D)null));
				MenuItem mapItem = val2;
				((Control)mapItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SaveScroll(2);
				});
				foreach (FullRace race in group.OrderBy((FullRace r) => r.Race.Name))
				{
					MenuItem val3 = new MenuItem("  " + race.Race.Name);
					((Control)val3).set_Parent((Container)(object)mapItem);
					((Control)val3).set_BasicTooltipText(race.Race.Name);
					MenuItem raceItem = val3;
					raceItem.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
					{
						MenuItem selectedMenuItem = _racesMenu.get_SelectedMenuItem();
						if (selectedMenuItem != null)
						{
							selectedMenuItem.Deselect();
						}
						Race = race;
					});
					if (race.Meta.Id == oldId)
					{
						_localRacesMenu.Select(raceItem);
					}
				}
			}
		}

		private void PopulateRaces()
		{
			PopulateLocalRaces();
			PopulateRemoteRaces(RacingModule.Server.RemoteRaces.Races.Values);
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
				Panel racesPanel = _racesPanel;
				((Control)_localRacesPanel).set_Width(height = ((Container)this).get_ContentRegion().Width - 20);
				((Control)racesPanel).set_Width(height);
			}
		}

		private void RemoteRacesChanged(RemoteRaces races)
		{
			PopulateRemoteRaces(races.Races.Values);
		}

		private void LocalRacesChanged()
		{
			PopulateLocalRaces();
		}

		private void MapChanged(object sender, ValueEventArgs<int> e)
		{
			PopulateRaces();
		}

		private void SaveScroll(int frames)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_scrollTarget = (frames, _scrollbar.get_ScrollDistance() * (float)(((Control)_localRacesPanel).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_scrollTarget.frames > 0)
			{
				_scrollbar.set_ScrollDistance(_scrollTarget.target / (float)(((Control)_localRacesPanel).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
				_scrollTarget.frames--;
			}
		}

		protected override void DisposeControl()
		{
			RacingModule.Server.RemoteRacesChanged -= RemoteRacesChanged;
			RacingModule.Racer.LocalRacesChanged -= new Action(LocalRacesChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			((Container)this).DisposeControl();
		}
	}
}
