using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Library.Controls;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Library.Services;
using Manlaan.CommanderMarkers.Settings.Controls;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class AutoMarkerCommunityLibraryView : View
	{
		private const int HEADER_HEIGHT = 45;

		private CommunityMarkerService _service = new CommunityMarkerService();

		private CommunitySets? _sets;

		private Panel? _listingHeader;

		private FlowPanel? _listingPanel;

		private Dropdown? _categorySelection;

		private Checkbox? _currentMapFilter;

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), 45));
			((Control)val).set_Location(new Point(0, 0));
			val.set_ShowBorder(true);
			_listingHeader = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent((Container)(object)_listingHeader);
			((Control)val2).set_Width(200);
			((Control)val2).set_Location(new Point(20, 3));
			_categorySelection = val2;
			_categorySelection!.get_Items().Add("Select a category to begin");
			_categorySelection!.set_SelectedItem(_categorySelection!.get_Items()[0]);
			Checkbox val3 = new Checkbox();
			val3.set_Text("Filter to current map");
			((Control)val3).set_Parent((Container)(object)_listingHeader);
			((Control)val3).set_Location(new Point(230, 10));
			val3.set_Checked(Service.Settings.AutoMarker_LibraryFilterToCurrent.get_Value());
			_currentMapFilter = val3;
			NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
			((Control)nuclearOptionButton).set_Parent((Container)(object)_listingHeader);
			((Control)nuclearOptionButton).set_Width(100);
			((StandardButton)nuclearOptionButton).set_Text("Redownload");
			((Control)nuclearOptionButton).set_BasicTooltipText("Force a redownload of the community library.\n\nHold Ctrl and Shift to activate the button");
			((Control)nuclearOptionButton).set_Location(new Point(((Control)_listingHeader).get_Width() - 100, 0));
			((Control)nuclearOptionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_sets = _service.FetchListing();
				ReloadMarkerList(_currentMapFilter!.get_Checked());
				ScreenNotification.ShowNotification("Community Library has been reloaded.", (NotificationType)0, (Texture2D)null, 4);
			});
			_listingPanel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel, new Point(-10, -75), new Point(0, 45));
			_listingPanel!.set_ControlPadding(new Vector2(0f, 10f));
			_listingPanel!.set_OuterControlPadding(new Vector2(20f, 10f));
			((Panel)_listingPanel).set_CanScroll(true);
			Label val4 = new Label();
			((Control)val4).set_Parent(buildPanel);
			val4.set_Text("Click here to learn how to contribute your own custom marker set");
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Location(new Point(10, ((Control)buildPanel).get_Height() - 28));
			val4.set_TextColor(new Color(8, 105, 190));
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://github.com/manlaan/BlishHud-CommanderMarkers/blob/bhud-static/Manlaan.CommanderMarkers/README.md",
					UseShellExecute = true
				});
			});
			ReloadMarkerList(_currentMapFilter!.get_Checked());
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)delegate
			{
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
			_currentMapFilter!.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Service.Settings.AutoMarker_LibraryFilterToCurrent.set_Value(_currentMapFilter!.get_Checked());
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
			_categorySelection!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
		}

		protected void ReloadMarkerList(bool filterToCurrent)
		{
			int currentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (_sets == null)
			{
				_sets = _service.CommunitySets;
				LoadCatetorySelection();
			}
			RenderLibraryList(_listingPanel, filterToCurrent, currentMapId);
		}

		protected void LoadCatetorySelection()
		{
			Dropdown? categorySelection = _categorySelection;
			if (categorySelection != null)
			{
				categorySelection!.get_Items().Clear();
			}
			_sets?.Categories.ForEach(delegate(CommunityCategory category)
			{
				Dropdown? categorySelection2 = _categorySelection;
				if (categorySelection2 != null)
				{
					categorySelection2!.get_Items().Add(category?.CategoryName);
				}
			});
		}

		protected void RenderLibraryList(FlowPanel? panel, bool shouldFilter, int currentMapId)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel panel2 = panel;
			if (panel2 == null)
			{
				return;
			}
			Texture2D editIcon = Service.Textures!.IconImport;
			new Point(editIcon.get_Width(), editIcon.get_Height());
			int DetailButtonWidth = ((Control)panel2).get_Width() - (int)panel2.get_OuterControlPadding().X * 2 - 10;
			int i = 0;
			((Container)panel2).get_Children().Clear();
			if (_sets == null || _sets!.Categories.Count < 1)
			{
				return;
			}
			_sets!.Categories.Find(delegate(CommunityCategory cat)
			{
				string categoryName = cat.CategoryName;
				Dropdown? categorySelection = _categorySelection;
				return categoryName == ((categorySelection != null) ? categorySelection!.get_SelectedItem() : null);
			})?.MarkerSets.ForEach(delegate(CommunityMarkerSet marker)
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Expected O, but got Unknown
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0139: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0177: Unknown result type (might be due to invalid IL or missing references)
				//IL_0192: Unknown result type (might be due to invalid IL or missing references)
				//IL_0200: Unknown result type (might be due to invalid IL or missing references)
				//IL_0240: Unknown result type (might be due to invalid IL or missing references)
				//IL_0245: Unknown result type (might be due to invalid IL or missing references)
				//IL_024c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0261: Unknown result type (might be due to invalid IL or missing references)
				//IL_026c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0285: Unknown result type (might be due to invalid IL or missing references)
				//IL_028a: Unknown result type (might be due to invalid IL or missing references)
				//IL_029f: Unknown result type (might be due to invalid IL or missing references)
				//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
				CommunityMarkerSet marker2 = marker;
				i++;
				string mapName = marker2.MapName;
				DetailsButton val = new DetailsButton();
				((Control)val).set_Parent((Container)(object)panel2);
				val.set_Text(marker2.name + "\n" + marker2.description + "\n" + mapName);
				val.set_Icon(AsyncTexture2D.op_Implicit(((SquadMarker)(i % 8 + 1)).GetIcon()));
				((Control)val).set_Width(DetailButtonWidth);
				val.set_IconSize((DetailsIconSize)0);
				val.set_ShowToggleButton(true);
				((Control)val).set_BasicTooltipText(marker2.name + "\n" + marker2.description + "\nMap: " + mapName + "\n\nMarkers in use:\n" + marker2.DescribeMarkers());
				((Control)val).set_BackgroundColor((Color)(marker2.enabled ? Color.get_Transparent() : new Color(0.4f, 0.1f, 0.1f, 0.1f)));
				((Control)val).set_Visible(!shouldFilter || marker2.MapId == currentMapId);
				DetailsButton parent = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)parent);
				val2.set_Text("Author: " + marker2.Author);
				((Control)val2).set_Width((marker2.MapId == currentMapId) ? 180 : 300);
				((Control)val2).set_Height(30);
				if (marker2.MapId == currentMapId)
				{
					IconButton iconButton = new IconButton();
					((Control)iconButton).set_Parent((Container)(object)parent);
					iconButton.Icon = Service.Textures!.IconEye;
					((Control)iconButton).set_BasicTooltipText("Preview");
					((Control)iconButton).set_Size(new Point(30, 30));
					((Control)iconButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.PreviewMarkerSet(marker2);
					});
					((Control)iconButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.RemovePreviewMarkerSet();
					});
					StandardButton val3 = new StandardButton();
					((Control)val3).set_Parent((Container)(object)parent);
					val3.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!._blishHeartSmall));
					val3.set_Text("Place");
					((Control)val3).set_Width(100);
					((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.PlaceMarkers(marker2);
					});
				}
				StandardButton val4 = new StandardButton();
				val4.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
				val4.set_Text("Import");
				((Control)val4).set_BasicTooltipText("Import this communty marker set into your library");
				((Control)val4).set_Parent((Container)(object)parent);
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Service.MarkersListing.SaveMarker(marker2);
					ScreenNotification.ShowNotification("Imported community marker set into your library", (NotificationType)5, (Texture2D)null, 4);
				});
			});
		}

		public AutoMarkerCommunityLibraryView()
			: this()
		{
		}
	}
}
