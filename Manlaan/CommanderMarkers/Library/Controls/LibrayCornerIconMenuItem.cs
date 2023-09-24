using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Presets.Model;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class LibrayCornerIconMenuItem : ContextMenuStripItem
	{
		public LibrayCornerIconMenuItem(SettingEntry<bool> setting, string displayLabel)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			SettingEntry<bool> setting2 = setting;
			((ContextMenuStripItem)this)._002Ector(displayLabel);
			LibrayCornerIconMenuItem librayCornerIconMenuItem = this;
			((Control)this).set_Visible(setting2.get_Value());
			((ContextMenuStripItem)this).set_Submenu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => librayCornerIconMenuItem.GetLibraryForCurrentMap())));
			setting2.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				((Control)librayCornerIconMenuItem).set_Visible(setting2.get_Value());
			});
			((Control)this).add_Click((EventHandler<MouseEventArgs>)LibrayCornerIconMenuItem_Click);
		}

		private void LibrayCornerIconMenuItem_Click(object sender, MouseEventArgs e)
		{
			Service.SettingsWindow.ShowLibrary();
		}

		protected IEnumerable<ContextMenuStripItem> GetLibraryForCurrentMap()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			int currentMap = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			List<MarkerSet> markersForMap = Service.MarkersListing.GetMarkersForMap(currentMap);
			List<ContextMenuStripItem> menuListItems = new List<ContextMenuStripItem>();
			markersForMap.ForEach(delegate(MarkerSet marker)
			{
				if (marker.enabled)
				{
					menuListItems.Add((ContextMenuStripItem)(object)new MarkerPlaceMenuItem(marker));
				}
			});
			if (menuListItems.Count <= 0)
			{
				menuListItems.Add(new ContextMenuStripItem("No marker sets for this map"));
			}
			return menuListItems;
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)LibrayCornerIconMenuItem_Click);
		}
	}
}
