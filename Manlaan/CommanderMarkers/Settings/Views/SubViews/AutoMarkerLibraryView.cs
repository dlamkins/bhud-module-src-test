using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.CommanderMarkers.Library.Controls;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.Presets.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class AutoMarkerLibraryView : View
	{
		private enum LibraryViewMode
		{
			List,
			Edit,
			Share
		}

		private const int HEADER_HEIGHT = 45;

		private List<MarkerSet> _markers = new List<MarkerSet>();

		private Panel? _listingHeader;

		private Panel? _detailsHeader;

		private Panel? _shareHeader;

		private FlowPanel? _listingPanel;

		private MarkerSetEditor? _detailsPanel;

		private MarkerSetSharePanel? _sharePanel;

		private LibraryViewMode _viewMode;

		private Checkbox? _currentMapFilter;

		private Checkbox? _mineFilter;

		private TextBox? _searchBox;

		private StandardButton? _shareSubmitButton;

		private MarkerSet? _editingMarkerSet;

		private MarkerSet? _sharingMarkerSet;

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
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Expected O, but got Unknown
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Expected O, but got Unknown
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Expected O, but got Unknown
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Expected O, but got Unknown
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Expected O, but got Unknown
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Expected O, but got Unknown
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Expected O, but got Unknown
			//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Expected O, but got Unknown
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
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
			Panel val3 = new Panel();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Size(new Point(((Control)buildPanel).get_Width(), 45));
			((Control)val3).set_Location(new Point(0, ((Control)buildPanel).get_Height() - 45));
			val3.set_ShowBorder(true);
			((Control)val3).set_Visible(false);
			((Control)val3).set_ClipsBounds(false);
			_shareHeader = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Add New");
			((Control)val4).set_Parent((Container)(object)_listingHeader);
			((Control)val4).set_Width(95);
			((Control)val4).set_Location(new Point(20, 3));
			StandardButton newMarkerSet = val4;
			Checkbox val5 = new Checkbox();
			val5.set_Text("Current map");
			((Control)val5).set_Parent((Container)(object)_listingHeader);
			((Control)val5).set_Location(new Point(((Control)newMarkerSet).get_Right() + 5, 10));
			val5.set_Checked(Service.Settings.AutoMarker_LibraryFilterToCurrent.get_Value());
			((Control)val5).set_BasicTooltipText("Only show marker sets for your current map");
			_currentMapFilter = val5;
			Checkbox val6 = new Checkbox();
			val6.set_Text("Mine");
			((Control)val6).set_Parent((Container)(object)_listingHeader);
			((Control)val6).set_Location(new Point(((Control)_currentMapFilter).get_Right() + 8, 10));
			val6.set_Checked(Service.Settings.AutoMarker_LibraryFilterMine.get_Value());
			((Control)val6).set_BasicTooltipText("Hide marker sets imported from the community library");
			_mineFilter = val6;
			TextBox val7 = new TextBox();
			((Control)val7).set_Parent((Container)(object)_listingHeader);
			((Control)val7).set_Width(200);
			((Control)val7).set_Location(new Point(((Control)_listingHeader).get_Width() - 200 - 20, 8));
			((Control)val7).set_BasicTooltipText("Search name, description, or map");
			_searchBox = val7;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
			((Control)newMarkerSet).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MarkerSet markerSet2 = new MarkerSet
				{
					id = Guid.NewGuid().ToString(),
					source = "custom",
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
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)_detailsHeader);
			val8.set_Text("Cancel");
			((Control)val8).set_Width(100);
			((Control)val8).set_Location(new Point(10, 0));
			val8.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconGoBack));
			StandardButton cancelButton = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)_detailsHeader);
			val9.set_Text("Save");
			((Control)val9).set_Width(100);
			((Control)val9).set_Location(new Point(115, 0));
			val9.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconSave));
			StandardButton saveButton = val9;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)_detailsHeader);
			val10.set_Text("Export");
			((Control)val10).set_Width(95);
			val10.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconExport));
			((Control)val10).set_Location(new Point(220, 0));
			((Control)val10).set_BasicTooltipText("Export this marker set to your clipboard to share with others");
			StandardButton export = val10;
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)_detailsHeader);
			val11.set_Text("Import");
			((Control)val11).set_Width(95);
			((Control)val11).set_Location(new Point(320, 0));
			val11.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
			((Control)val11).set_BasicTooltipText("Copy a marker set to your clipboard, then import it by clicking this button");
			StandardButton import = val11;
			StandardButton val12 = new StandardButton();
			((Control)val12).set_Parent((Container)(object)_detailsHeader);
			val12.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconDelete));
			((Control)val12).set_Width(80);
			val12.set_Text("Delete");
			((Control)val12).set_BasicTooltipText("Delete Marker Set");
			((Control)val12).set_Location(new Point(420, 0));
			((Control)cancelButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SwapView(wasUpdated: false);
			});
			((Control)export).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					if (_editingMarkerSet != null)
					{
						Clipboard.SetText(MarkerSetShareCode.Export(_editingMarkerSet));
						ScreenNotification.ShowNotification("Marker set " + _editingMarkerSet!.name + " copied to your clipboard!", (NotificationType)4, Service.Textures!._blishHeart, 4);
					}
				}
				catch (Exception)
				{
				}
			});
			((Control)import).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					MarkerSet markerSet = MarkerSetShareCode.Import(Clipboard.GetText(), (string communitySetId, string name) => Service.CommunityCatalog.FetchSetDetail(communitySetId));
					if (markerSet == null)
					{
						throw new Exception("Invalid share code");
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
					if (string.IsNullOrWhiteSpace(_editingMarkerSet!.id))
					{
						_editingMarkerSet!.id = Guid.NewGuid().ToString();
					}
					if (string.IsNullOrWhiteSpace(_editingMarkerSet!.source))
					{
						_editingMarkerSet!.source = "custom";
					}
					Service.MarkersListing.SaveMarker(_editingMarkerSet);
				}
				SwapView(wasUpdated: true);
			});
			((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_editingMarkerSetIndex >= 0)
				{
					Service.MarkersListing.DeleteMarker(_editingMarkerSet);
				}
				SwapView(wasUpdated: true);
			});
			StandardButton val13 = new StandardButton();
			((Control)val13).set_Parent((Container)(object)_shareHeader);
			val13.set_Text("Cancel");
			((Control)val13).set_Width(100);
			((Control)val13).set_Location(new Point(10, 0));
			val13.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconGoBack));
			StandardButton val14 = new StandardButton();
			((Control)val14).set_Parent((Container)(object)_shareHeader);
			val14.set_Text("Submit");
			((Control)val14).set_Width(110);
			((Control)val14).set_Location(new Point(115, 0));
			val14.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
			((Control)val14).set_BasicTooltipText("Send this marker set for community moderator review");
			_shareSubmitButton = val14;
			((Control)val13).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseShareView();
			});
			((Control)_shareSubmitButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SubmitShare();
			});
			_listingPanel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel, new Point(-10, -45), new Point(0, 45));
			_listingPanel!.set_ControlPadding(new Vector2(0f, 10f));
			_listingPanel!.set_OuterControlPadding(new Vector2(20f, 10f));
			((Panel)_listingPanel).set_CanScroll(true);
			((Control)_listingPanel).set_Visible(_viewMode == LibraryViewMode.List);
			MarkerSetEditor markerSetEditor = new MarkerSetEditor(new Action<bool>(SwapView));
			((FlowPanel)markerSetEditor).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)markerSetEditor).set_OuterControlPadding(new Vector2(20f, 10f));
			((Control)markerSetEditor).set_Parent(buildPanel);
			((Control)markerSetEditor).set_Size(((Control)buildPanel).get_Size() + new Point(-10, -45));
			((Panel)markerSetEditor).set_ShowBorder(true);
			((Control)markerSetEditor).set_Location(new Point(0, 0));
			_detailsPanel = markerSetEditor;
			((Control)_detailsPanel).set_Visible(_viewMode == LibraryViewMode.Edit);
			((Panel)_detailsPanel).set_CanScroll(true);
			MarkerSetSharePanel markerSetSharePanel = new MarkerSetSharePanel();
			((FlowPanel)markerSetSharePanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)markerSetSharePanel).set_OuterControlPadding(new Vector2(20f, 10f));
			((Control)markerSetSharePanel).set_Parent(buildPanel);
			((Control)markerSetSharePanel).set_Size(((Control)buildPanel).get_Size() + new Point(-10, -45));
			((Panel)markerSetSharePanel).set_ShowBorder(true);
			((Control)markerSetSharePanel).set_Location(new Point(0, 0));
			_sharePanel = markerSetSharePanel;
			((Control)_sharePanel).set_Visible(_viewMode == LibraryViewMode.Share);
			((Panel)_sharePanel).set_CanScroll(true);
			ApplyViewMode();
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
			_mineFilter!.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				Service.Settings.AutoMarker_LibraryFilterMine.set_Value(_mineFilter!.get_Checked());
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
		}

		protected void SwapView(bool wasUpdated)
		{
			if (!wasUpdated)
			{
				Service.MarkersListing.ReloadFromFile();
			}
			_viewMode = LibraryViewMode.List;
			_editingMarkerSet = null;
			_sharingMarkerSet = null;
			ApplyViewMode();
			int currentScroll = ((Container)_listingPanel).get_VerticalScrollOffset();
			ReloadMarkerList(_currentMapFilter!.get_Checked());
			((Container)_listingPanel).set_VerticalScrollOffset(currentScroll);
		}

		protected void SwapView(MarkerSet marker, int idx)
		{
			if (MarkerListing.IsCommunityLinked(marker))
			{
				OpenPersonalizedEditor(marker);
				return;
			}
			_editingMarkerSet = marker;
			_editingMarkerSetIndex = idx;
			_sharingMarkerSet = null;
			_viewMode = LibraryViewMode.Edit;
			ApplyViewMode();
			_detailsPanel!.LoadMarkerSet(marker, idx);
		}

		private void OpenPersonalizedEditor(MarkerSet template)
		{
			MarkerSet personalized = (_editingMarkerSet = MarkerListing.DuplicateAsEditableCopy(template));
			_editingMarkerSetIndex = -1;
			_sharingMarkerSet = null;
			_viewMode = LibraryViewMode.Edit;
			ApplyViewMode();
			_detailsPanel!.LoadMarkerSet(personalized, -1);
		}

		private void OpenShareView(MarkerSet marker)
		{
			_sharingMarkerSet = marker;
			_editingMarkerSet = null;
			_editingMarkerSetIndex = -1;
			_viewMode = LibraryViewMode.Share;
			ApplyViewMode();
			_sharePanel!.LoadMarkerSet(marker);
		}

		private void CloseShareView()
		{
			_sharingMarkerSet = null;
			_viewMode = LibraryViewMode.List;
			ApplyViewMode();
			int currentScroll = ((Container)_listingPanel).get_VerticalScrollOffset();
			ReloadMarkerList(_currentMapFilter!.get_Checked());
			((Container)_listingPanel).set_VerticalScrollOffset(currentScroll);
		}

		private void ApplyViewMode()
		{
			bool showList = _viewMode == LibraryViewMode.List;
			bool showEdit = _viewMode == LibraryViewMode.Edit;
			bool showShare = _viewMode == LibraryViewMode.Share;
			((Control)_listingHeader).set_Visible(showList);
			((Control)_listingPanel).set_Visible(showList);
			((Control)_detailsHeader).set_Visible(showEdit);
			((Control)_detailsPanel).set_Visible(showEdit);
			((Control)_shareHeader).set_Visible(showShare);
			((Control)_sharePanel).set_Visible(showShare);
			if (_shareSubmitButton != null)
			{
				bool canSubmit = showShare && CanShareMarkerSets() && !(_sharePanel?.IsSubmitting ?? false);
				((Control)_shareSubmitButton).set_Enabled(canSubmit);
				_shareSubmitButton!.set_Text((_sharePanel?.IsSubmitting ?? false) ? "Submitting..." : "Submit");
			}
		}

		private void SubmitShare()
		{
			if (_sharingMarkerSet == null || _sharePanel == null || _sharePanel!.IsSubmitting)
			{
				return;
			}
			if (!_sharePanel!.TryGetCategory(out var category, out var error))
			{
				_sharePanel!.SetFeedback(error, null);
				return;
			}
			MarkerSet markerSet = _sharingMarkerSet;
			_sharePanel!.SetSubmitting(submitting: true);
			_sharePanel!.SetFeedback(null, null);
			ApplyViewMode();
			Task.Run(async delegate
			{
				CommunityShareResult result = await CommunityShareHelper.SubmitAsync(markerSet, category).ConfigureAwait(continueOnCapturedContext: false);
				GameThreadUtil.Enqueue(delegate
				{
					if (_sharePanel != null && _sharingMarkerSet == markerSet)
					{
						_sharePanel!.SetSubmitting(submitting: false);
						if (result.Success)
						{
							ScreenNotification.ShowNotification(result.Message, (NotificationType)5, Service.Textures!._blishHeart, 4);
							CloseShareView();
						}
						else
						{
							_sharePanel!.SetFeedback(result.Message, null);
							ApplyViewMode();
						}
					}
				});
			});
		}

		private static bool CanShareMarkerSets()
		{
			if (Service.Gw2ApiManager != null)
			{
				return Service.Gw2ApiManager.HasPermission((TokenPermission)1);
			}
			return false;
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
				shouldDoIt &= CommanderPermissionHelper.PassesCommanderGate();
			}
			return shouldDoIt;
		}

		protected void RenderLibraryList(FlowPanel? panel, bool shouldFilter, int currentMapId)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Expected O, but got Unknown
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			if (panel == null)
			{
				return;
			}
			int detailButtonWidth = ((Control)panel).get_Width() - (int)panel!.get_OuterControlPadding().X * 2 - 10;
			bool showPlaceBtn = CanShowPreviewAndPlaceButtons();
			((Container)panel).get_Children().Clear();
			TextBox? searchBox = _searchBox;
			string searchLower = LibrarySearch.ToLowerCopy(((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null) ?? "");
			for (int markerIdx = 0; markerIdx < _markers.Count; markerIdx++)
			{
				MarkerSet marker = _markers[markerIdx];
				int presetIndex = markerIdx;
				if (shouldFilter && marker.MapId != currentMapId)
				{
					continue;
				}
				Checkbox? mineFilter = _mineFilter;
				if (mineFilter != null && mineFilter!.get_Checked() && MarkerListing.IsCommunityLinked(marker))
				{
					continue;
				}
				string mapName = marker.MapName;
				if (!LibrarySearch.MatchesLocal(marker, mapName, searchLower))
				{
					continue;
				}
				bool communityLinked = MarkerListing.IsCommunityLinked(marker);
				bool canShare = !communityLinked && MarkerListing.IsShareableWithCommunity(marker) && CanShareMarkerSets();
				int bottomSectionHeight = ((marker.MapId == currentMapId && showPlaceBtn) ? 40 : 35);
				if (communityLinked || canShare)
				{
					bottomSectionHeight = Math.Max(bottomSectionHeight, 40);
				}
				Texture2D fallbackIcon = (marker.enabled ? ((SquadMarker)(markerIdx % 8 + 1)).GetIcon() : Service.Textures!._imgClear);
				DetailsButton val = new DetailsButton();
				val.set_Text((marker.enabled ? "" : "(Disabled) ") + marker.name + "\n" + marker.description + "\n" + mapName);
				val.set_Icon(AsyncTexture2D.op_Implicit(fallbackIcon));
				val.set_IconDetails(MarkerListing.DisplayAuthor(marker));
				((Control)val).set_Width(detailButtonWidth);
				val.set_BottomSectionHeight(bottomSectionHeight);
				val.set_HighlightType((DetailsHighlightType)2);
				val.set_ShowToggleButton(true);
				((Control)val).set_BackgroundColor((Color)(marker.enabled ? Color.get_Transparent() : new Color(0.4f, 0.1f, 0.1f, 0.1f)));
				DetailsButton btn = val;
				if (!string.IsNullOrWhiteSpace(marker.communitySetId))
				{
					string communitySetId = marker.communitySetId;
					Texture2D thumb = Service.PreviewImageCache.GetThumbTexture(communitySetId, fallbackIcon);
					if (thumb != null)
					{
						btn.set_Icon(AsyncTexture2D.op_Implicit(thumb));
					}
					else
					{
						DetailsButton capturedBtn = btn;
						Service.PreviewImageCache.RequestThumb(communitySetId, "", delegate
						{
							if (((Control)capturedBtn).get_Parent() != null)
							{
								Texture2D thumbTexture = Service.PreviewImageCache.GetThumbTexture(communitySetId, fallbackIcon);
								if (thumbTexture != null)
								{
									capturedBtn.set_Icon(AsyncTexture2D.op_Implicit(thumbTexture));
								}
							}
						});
					}
					MapPreviewTooltip.Apply((Control)(object)btn, MapPreviewTarget.FromLocalMarker(marker));
				}
				else
				{
					((Control)btn).set_BasicTooltipText(marker.name + "\n" + marker.description + "\nMap: " + mapName + "\n\nMarkers in use:\n" + marker.DescribeMarkers());
				}
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)btn);
				val2.set_Text(communityLinked ? "Personalize" : "Edit");
				((Control)val2).set_Width(communityLinked ? 95 : 60);
				((Control)val2).set_Location(new Point(10, 5));
				((Control)val2).set_BasicTooltipText(communityLinked ? "Open the editor with this set as a template. Save to add your personalized copy." : ("Click to edit " + marker.name));
				val2.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconEdit));
				StandardButton edit = val2;
				((Control)edit).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (communityLinked)
					{
						OpenPersonalizedEditor(marker);
					}
					else
					{
						SwapView(marker, presetIndex);
					}
				});
				if (canShare)
				{
					StandardButton val3 = new StandardButton();
					((Control)val3).set_Parent((Container)(object)btn);
					val3.set_Text("Share");
					((Control)val3).set_Width(75);
					((Control)val3).set_Location(new Point(((Control)edit).get_Right() + 5, 5));
					((Control)val3).set_BasicTooltipText("Submit this marker set to the community library");
					val3.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
					((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						OpenShareView(marker);
					});
				}
				if (communityLinked)
				{
					StandardButton val4 = new StandardButton();
					((Control)val4).set_Parent((Container)(object)btn);
					val4.set_Text("Delete");
					((Control)val4).set_Width(75);
					val4.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconDelete));
					((Control)val4).set_BasicTooltipText("Remove this imported set from your library");
					((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.RemovePreviewMarkerSet();
						Service.MarkersListing.DeleteMarker(marker);
						ScreenNotification.ShowNotification("Removed \"" + marker.name + "\" from your library", (NotificationType)0, (Texture2D)null, 4);
					});
				}
				if (marker.MapId == currentMapId && showPlaceBtn && marker.enabled)
				{
					IconButton iconButton = new IconButton();
					((Control)iconButton).set_Parent((Container)(object)btn);
					iconButton.Icon = Service.Textures!.IconEye;
					((Control)iconButton).set_BasicTooltipText("Hover to preview markers on the map");
					((Control)iconButton).set_Size(new Point(30, 30));
					((Control)iconButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.PreviewMarkerSet(marker);
					});
					((Control)iconButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.RemovePreviewMarkerSet();
					});
					StandardButton val5 = new StandardButton();
					((Control)val5).set_Parent((Container)(object)btn);
					val5.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!._blishHeartSmall));
					val5.set_Text("Place");
					((Control)val5).set_Width(100);
					((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.PlaceMarkers(marker);
					});
				}
				EnabledIconButton enabledIconButton = new EnabledIconButton(marker.enabled);
				((Control)enabledIconButton).set_Size(new Point(Service.Textures!._imgArrow.get_Width(), Service.Textures!._imgArrow.get_Height()));
				((Control)enabledIconButton).set_Parent((Container)(object)btn);
				((Control)enabledIconButton).set_Opacity(0.5f);
				enabledIconButton.ValueChanged += delegate(object _, bool enabled)
				{
					Service.MapWatch.RemovePreviewMarkerSet();
					Service.MarkersListing.SetMarkerEnabled(presetIndex, enabled);
				};
				panel.AddFlowControl((Control)(object)btn);
			}
		}

		public AutoMarkerLibraryView()
			: this()
		{
		}
	}
}
