using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class LobbyRacesPanel : Panel
	{
		private readonly RacingClient Client;

		private readonly Menu _menu;

		private readonly LobbyRaceMenu _raceMenu;

		public event Action<int>? SaveScroll;

		public LobbyRacesPanel(RacingClient client)
			: this()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Client = client;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Panel)this).set_CanCollapse(true);
			((Panel)this).set_Title(Strings.Races);
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_CanSelect(true);
			_menu = val;
			_raceMenu = new LobbyRaceMenu(Client);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			PopulateRaces();
		}

		private void MapChanged(object sender, ValueEventArgs<int> e)
		{
			PopulateRaces();
		}

		private void PopulateRaces()
		{
			List<FullRace> races = new List<FullRace>();
			races.AddRange(RacingModule.Server.RemoteRaces.Races.Values);
			races.AddRange(RacingModule.LocalData.Races.Values);
			((Container)_menu).ClearChildren();
			if (!races.Any())
			{
				_menu.AddMenuItem(Strings.Nothing, (Texture2D)null);
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
				OnelineMenuItem onelineMenuItem = new OnelineMenuItem(RacingModule.MapData.Describe(group.Key));
				((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
				OnelineMenuItem mapItem = onelineMenuItem;
				((Control)mapItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					this.SaveScroll?.Invoke(2);
				});
				foreach (FullRace race in group.OrderBy((FullRace r) => r.Race.Name))
				{
					OnelineMenuItem onelineMenuItem2 = new OnelineMenuItem("  " + race.Race.Name);
					((Control)onelineMenuItem2).set_Parent((Container)(object)mapItem);
					((Control)onelineMenuItem2).set_BasicTooltipText(race.Race.Name);
					OnelineMenuItem raceItem = onelineMenuItem2;
					((Control)raceItem).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						_raceMenu.Show(race, (Control?)(object)raceItem);
					});
				}
			}
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			_raceMenu.Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
