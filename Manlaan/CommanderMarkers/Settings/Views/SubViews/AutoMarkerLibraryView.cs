using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Library.Controls;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class AutoMarkerLibraryView : View
	{
		private const int HEADER_HEIGHT = 45;

		private List<MarkerSet> _markers = new List<MarkerSet>();

		private Panel? _listingHeader;

		private Panel? _detailsHeader;

		private FlowPanel? _listingPanel;

		private MarkerSetEditor? _detailsPanel;

		private bool _showingDetails;

		private List<(DetailsButton, MarkerSet)> _markerSetButtons = new List<(DetailsButton, MarkerSet)>();

		private Checkbox? _currentMapFilter;

		private MarkerSet? _editingMarkerSet;

		private int _editingMarkerSetIndex = -1;

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
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Expected O, but got Unknown
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), 45));
			((Control)val).set_Location(new Point(0, 0));
			val.set_ShowBorder(true);
			_listingHeader = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width(), 45));
			((Control)val2).set_Location(new Point(0, ((Control)buildPanel).get_Height() - 45));
			val2.set_ShowBorder(true);
			((Control)val2).set_Visible(false);
			((Control)val2).set_ClipsBounds(false);
			_detailsHeader = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Add New");
			((Control)val3).set_Parent((Container)(object)_listingHeader);
			((Control)val3).set_Width(200);
			((Control)val3).set_Location(new Point(20, 3));
			Checkbox val4 = new Checkbox();
			val4.set_Text("Filter to current map");
			((Control)val4).set_Parent((Container)(object)_listingHeader);
			((Control)val4).set_Location(new Point(230, 10));
			val4.set_Checked(Service.Settings.AutoMarker_LibraryFilterToCurrent.get_Value());
			_currentMapFilter = val4;
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MarkerSet markerSet2 = new MarkerSet
				{
					name = "new set name",
					description = "description",
					mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id(),
					trigger = new WorldCoord()
				};
				MarkerCoord item = new MarkerCoord
				{
					name = "marker name"
				};
				markerSet2.marks.Add(item);
				SwapView(markerSet2, -1);
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_detailsHeader);
			val5.set_Text("Cancel");
			((Control)val5).set_Width(100);
			((Control)val5).set_Location(new Point(10, 0));
			val5.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconGoBack));
			StandardButton cancelButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_detailsHeader);
			val6.set_Text("Save");
			((Control)val6).set_Width(100);
			((Control)val6).set_Location(new Point(115, 0));
			val6.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconSave));
			StandardButton saveButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_detailsHeader);
			val7.set_Text("Export");
			((Control)val7).set_Width(95);
			val7.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconExport));
			((Control)val7).set_Location(new Point(220, 0));
			((Control)val7).set_BasicTooltipText("Export this marker set to your clipboard to share with others");
			StandardButton export = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)_detailsHeader);
			val8.set_Text("Import");
			((Control)val8).set_Width(95);
			((Control)val8).set_Location(new Point(320, 0));
			val8.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
			((Control)val8).set_BasicTooltipText("Copy a marker set to your clipboard, then import it by clicking this button");
			StandardButton import = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)_detailsHeader);
			val9.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconDelete));
			((Control)val9).set_Width(80);
			val9.set_Text("Delete");
			((Control)val9).set_BasicTooltipText("Delete Marker Set");
			((Control)val9).set_Location(new Point(420, 0));
			((Control)cancelButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SwapView(wasUpdated: false);
			});
			((Control)export).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					string s2 = JsonConvert.SerializeObject(_editingMarkerSet);
					Clipboard.SetText(Convert.ToBase64String(Encoding.UTF8.GetBytes(s2)));
					ScreenNotification.ShowNotification("Marker set " + _editingMarkerSet?.name + " copied to your clipboard!", (NotificationType)4, Service.Textures!._blishHeart, 4);
				}
				catch (Exception)
				{
				}
			});
			((Control)import).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					byte[] bytes = Convert.FromBase64String(Clipboard.GetText());
					MarkerSet markerSet = JsonConvert.DeserializeObject<MarkerSet>(Encoding.UTF8.GetString(bytes));
					if (markerSet == null)
					{
						throw new Exception("Invalid JSON");
					}
					ScreenNotification.ShowNotification("Imported marker set " + markerSet.name, (NotificationType)5, Service.Textures!._blishHeart, 4);
					_editingMarkerSet?.CloneFromMarkerSet(markerSet);
					_detailsPanel!.LoadMarkerSet(_editingMarkerSet, _editingMarkerSetIndex);
				}
				catch (Exception)
				{
					ScreenNotification.ShowNotification("Unable to import clipboard content\nDid you copy a marker set first?", (NotificationType)6, (Texture2D)null, 5);
				}
			});
			((Control)saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_editingMarkerSetIndex >= 0)
				{
					Service.MarkersListing.EditMarker(_editingMarkerSetIndex, _editingMarkerSet);
				}
				else
				{
					Service.MarkersListing.SaveMarker(_editingMarkerSet);
					Service.MarkersListing.SaveMarker(_editingMarkerSet);
				}
				SwapView(wasUpdated: true);
			});
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_editingMarkerSetIndex > 0)
				{
					Service.MarkersListing.DeleteMarker(_editingMarkerSet);
				}
				SwapView(wasUpdated: true);
			});
			_listingPanel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel, new Point(-10, -45), new Point(0, 45));
			_listingPanel!.set_ControlPadding(new Vector2(0f, 10f));
			_listingPanel!.set_OuterControlPadding(new Vector2(20f, 10f));
			((Panel)_listingPanel).set_CanScroll(true);
			((Control)_listingPanel).set_Visible(!_showingDetails);
			MarkerSetEditor markerSetEditor = new MarkerSetEditor(new Action<bool>(SwapView));
			((FlowPanel)markerSetEditor).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)markerSetEditor).set_OuterControlPadding(new Vector2(20f, 10f));
			((Control)markerSetEditor).set_Parent(buildPanel);
			((Control)markerSetEditor).set_Size(((Control)buildPanel).get_Size() + new Point(-10, -45));
			((Panel)markerSetEditor).set_ShowBorder(true);
			((Control)markerSetEditor).set_Location(new Point(0, 0));
			_detailsPanel = markerSetEditor;
			((Control)_detailsPanel).set_Visible(_showingDetails);
			((Panel)_detailsPanel).set_CanScroll(true);
			ReloadMarkerList(_currentMapFilter!.get_Checked());
			Service.MarkersListing.MarkersChanged += delegate
			{
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			};
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)delegate
			{
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
			_currentMapFilter!.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Service.Settings.AutoMarker_LibraryFilterToCurrent.set_Value(_currentMapFilter!.get_Checked());
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
		}

		protected void SwapView(bool wasUpdated)
		{
			if (!wasUpdated)
			{
				Service.MarkersListing.ReloadFromFile();
			}
			_showingDetails = false;
			((Control)_listingHeader).set_Visible(!_showingDetails);
			((Control)_detailsHeader).set_Visible(_showingDetails);
			((Control)_detailsPanel).set_Visible(_showingDetails);
			int currentScroll = ((Container)_listingPanel).get_VerticalScrollOffset();
			ReloadMarkerList(_currentMapFilter!.get_Checked());
			((Container)_listingPanel).set_VerticalScrollOffset(currentScroll);
			((Control)_listingPanel).set_Visible(!_showingDetails);
		}

		protected void SwapView(MarkerSet marker, int idx)
		{
			_editingMarkerSet = marker;
			_editingMarkerSetIndex = idx;
			_showingDetails = true;
			((Control)_listingHeader).set_Visible(!_showingDetails);
			((Control)_listingPanel).set_Visible(!_showingDetails);
			((Control)_detailsHeader).set_Visible(_showingDetails);
			((Control)_detailsPanel).set_Visible(_showingDetails);
			_detailsPanel!.LoadMarkerSet(marker, idx);
		}

		protected void ReloadMarkerList(bool filterToCurrent)
		{
			int currentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			_markers = Service.MarkersListing.GetAllMarkerSets();
			RenderLibraryList(_listingPanel, filterToCurrent, currentMapId);
		}

		private bool CanShowPreviewAndPlaceButtons()
		{
			bool shouldDoIt = Service.Settings.AutoMarker_FeatureEnabled.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable();
			if (Service.Settings._settingOnlyWhenCommander.get_Value() || Service.LtMode.get_Value())
			{
				shouldDoIt &= GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander() || Service.LtMode.get_Value();
			}
			return shouldDoIt;
		}

		protected void RenderLibraryList(FlowPanel? panel, bool shouldFilter, int currentMapId)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel panel2 = panel;
			if (panel2 == null)
			{
				return;
			}
			Texture2D editIcon = Service.Textures!._imgArrow;
			Point editSize = new Point(editIcon.get_Width(), editIcon.get_Height());
			int DetailButtonWidth = ((Control)panel2).get_Width() - (int)panel2.get_OuterControlPadding().X * 2 - 10;
			int i = 0;
			bool showPlaceBtn = CanShowPreviewAndPlaceButtons();
			((Container)panel2).get_Children().Clear();
			_markers.ForEach(delegate(MarkerSet marker)
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Expected O, but got Unknown
				//IL_0168: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_0201: Unknown result type (might be due to invalid IL or missing references)
				//IL_0206: Unknown result type (might be due to invalid IL or missing references)
				//IL_0285: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_031f: Unknown result type (might be due to invalid IL or missing references)
				MarkerSet marker2 = marker;
				int markerIdx = i++;
				string mapName = marker2.MapName;
				DetailsButton val = new DetailsButton();
				val.set_Text((marker2.enabled ? "" : "(Disabled) ") + marker2.name + "\n" + marker2.description + "\n" + mapName);
				val.set_Icon(AsyncTexture2D.op_Implicit(marker2.enabled ? ((SquadMarker)(i % 8 + 1)).GetIcon() : Service.Textures!._imgClear));
				((Control)val).set_Width(DetailButtonWidth);
				val.set_IconSize((DetailsIconSize)0);
				val.set_ShowToggleButton(true);
				((Control)val).set_BasicTooltipText(marker2.name + "\n" + marker2.description + "\nMap: " + mapName + "\n\nMarkers in use:\n" + marker2.DescribeMarkers());
				((Control)val).set_BackgroundColor((Color)(marker2.enabled ? Color.get_Transparent() : new Color(0.4f, 0.1f, 0.1f, 0.1f)));
				((Control)val).set_Visible(!shouldFilter || marker2.MapId == currentMapId);
				DetailsButton val2 = val;
				StandardButton val3 = new StandardButton();
				((Control)val3).set_Parent((Container)(object)val2);
				val3.set_Text("Edit");
				((Control)val3).set_Width(60);
				((Control)val3).set_BasicTooltipText("Click to edit " + marker2.name);
				val3.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconEdit));
				((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SwapView(marker2, markerIdx);
				});
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)val2);
				((Control)val4).set_Width((marker2.MapId == currentMapId && showPlaceBtn) ? 210 : 340);
				if (marker2.MapId == currentMapId && showPlaceBtn)
				{
					IconButton iconButton = new IconButton();
					((Control)iconButton).set_Parent((Container)(object)val2);
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
					StandardButton val5 = new StandardButton();
					((Control)val5).set_Parent((Container)(object)val2);
					val5.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!._blishHeartSmall));
					val5.set_Text("Place");
					((Control)val5).set_Width(100);
					((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.PlaceMarkers(marker2);
					});
				}
				EnabledIconButton enabledIconButton = new EnabledIconButton(marker2.enabled);
				((Control)enabledIconButton).set_Size(editSize);
				((Control)enabledIconButton).set_Parent((Container)(object)val2);
				((Control)enabledIconButton).set_Opacity(0.5f);
				EnabledIconButton img = enabledIconButton;
				((Control)img).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					marker2.enabled = img.WatchValue;
					Service.MarkersListing.EditMarker(markerIdx, marker2);
				});
				panel2.AddFlowControl((Control)(object)val2);
				_markerSetButtons.Add((val2, marker2));
			});
		}

		public AutoMarkerLibraryView()
			: this()
		{
		}
	}
}
