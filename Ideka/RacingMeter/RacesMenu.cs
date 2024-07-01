using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class RacesMenu : SelectablesMenu<FullRace>
	{
		public event Action? SizeChanging;

		protected override string? ExtractId(FullRace? item)
		{
			return item?.Meta.Id;
		}

		public void SetRaces(IEnumerable<FullRace> races)
		{
			IEnumerable<FullRace> races2 = races;
			Repopulate(delegate
			{
				if (!races2.Any())
				{
					Placeholder(Strings.Nothing);
				}
				else
				{
					_menu.set_CanSelect(true);
					foreach (var item2 in from r in (from r in races2
							group r by r.Race.MapId).Select(delegate(IGrouping<int, FullRace> r)
						{
							Map? map = RacingModule.MapData.GetMap(r.Key);
							return ((map != null) ? map!.get_Name() : null, r);
						})
						orderby r.@group.Key != GameService.Gw2Mumble.get_CurrentMap().get_Id(), r.name, r.@group.Key
						select r)
					{
						IGrouping<int, FullRace> item = item2.Item2;
						OnelineMenuItem onelineMenuItem = new OnelineMenuItem(RacingModule.MapData.Describe(item.Key));
						((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
						OnelineMenuItem onelineMenuItem2 = onelineMenuItem;
						((Control)onelineMenuItem2).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							this.SizeChanging?.Invoke();
						});
						foreach (FullRace race in item.OrderBy((FullRace r) => r.Race.Name))
						{
							RacesMenu racesMenu = this;
							string id = race.Meta.Id;
							OnelineMenuItem onelineMenuItem3 = new OnelineMenuItem("  " + race.Race.Name);
							((Control)onelineMenuItem3).set_Parent((Container)(object)onelineMenuItem2);
							((Control)onelineMenuItem3).set_BasicTooltipText(race.Race.Name);
							((MenuItem)racesMenu.SetItem(id, onelineMenuItem3)).add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
							{
								Select(race);
							});
						}
					}
				}
			});
		}
	}
}
