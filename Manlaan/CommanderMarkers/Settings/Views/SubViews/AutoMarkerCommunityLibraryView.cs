using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
using Manlaan.CommanderMarkers.Settings.Controls;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class AutoMarkerCommunityLibraryView : View
	{
		private sealed class ShareRowState
		{
			public int CategoryIndex { get; set; }

			public string CustomCategory { get; set; } = "";


			public string Error { get; set; } = "";


			public string Status { get; set; } = "";


			public bool Sharing { get; set; }
		}

		private const int HEADER_HEIGHT = 68;

		private const int HEADER_SIDE_PADDING = 20;

		private const int HEADER_ROW1_Y = 4;

		private const int HEADER_ROW2_Y = 36;

		private const int SHARE_SECTION_HEIGHT = 0;

		private Panel? _listingHeader;

		private FlowPanel? _listingPanel;

		private FlowPanel? _sharePanel;

		private Dropdown? _categorySelection;

		private Checkbox? _currentMapFilter;

		private Checkbox? _hideImportedFilter;

		private TextBox? _searchBox;

		private readonly Dictionary<string, ShareRowState> _shareRows = new Dictionary<string, ShareRowState>();

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
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), 68));
			((Control)val).set_Location(new Point(0, 0));
			val.set_ShowBorder(true);
			_listingHeader = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent((Container)(object)_listingHeader);
			((Control)val2).set_Width(200);
			((Control)val2).set_Location(new Point(20, 4));
			_categorySelection = val2;
			_categorySelection!.get_Items().Add("All categories");
			_categorySelection!.set_SelectedItem(_categorySelection!.get_Items()[0]);
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)_listingHeader);
			((Control)val3).set_Width(200);
			((Control)val3).set_Location(new Point(((Control)_listingHeader).get_Width() - 200 - 20, 6));
			((Control)val3).set_BasicTooltipText("Search name, description, author, or map");
			_searchBox = val3;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ReloadMarkerList(_currentMapFilter!.get_Checked());
			});
			Checkbox val4 = new Checkbox();
			val4.set_Text("Current map");
			((Control)val4).set_Parent((Container)(object)_listingHeader);
			((Control)val4).set_Location(new Point(20, 36));
			val4.set_Checked(Service.Settings.AutoMarker_LibraryFilterToCurrent.get_Value());
			_currentMapFilter = val4;
			Checkbox val5 = new Checkbox();
			val5.set_Text("Available");
			((Control)val5).set_Parent((Container)(object)_listingHeader);
			((Control)val5).set_Location(new Point(150, 36));
			val5.set_Checked(false);
			((Control)val5).set_BasicTooltipText("Only show sets you have not imported yet");
			_hideImportedFilter = val5;
			NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
			((Control)nuclearOptionButton).set_Parent((Container)(object)_listingHeader);
			((Control)nuclearOptionButton).set_Width(100);
			((StandardButton)nuclearOptionButton).set_Text("Redownload");
			((Control)nuclearOptionButton).set_BasicTooltipText("Force a redownload of the community library.\n\nHold Ctrl and Shift to activate the button");
			((Control)nuclearOptionButton).set_Location(new Point(((Control)_listingHeader).get_Width() - 100 - 20, 34));
			((Control)nuclearOptionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Task.Run(delegate
				{
					Service.CommunityCatalog.SyncCatalog();
					GameThreadUtil.Enqueue(delegate
					{
						LoadCategorySelection();
						ReloadMarkerList(_currentMapFilter!.get_Checked());
						RenderShareSection();
						ScreenNotification.ShowNotification("Community Library has been reloaded.", (NotificationType)0, (Texture2D)null, 4);
					});
				});
			});
			_listingPanel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel, new Point(-10, -78), new Point(0, 68));
			_listingPanel!.set_ControlPadding(new Vector2(0f, 10f));
			_listingPanel!.set_OuterControlPadding(new Vector2(20f, 10f));
			((Panel)_listingPanel).set_CanScroll(true);
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent(buildPanel);
			((Control)val6).set_Location(new Point(0, ((Control)buildPanel).get_Height()));
			((Control)val6).set_Size(new Point(0, 0));
			val6.set_FlowDirection((ControlFlowDirection)3);
			val6.set_OuterControlPadding(new Vector2(20f, 8f));
			val6.set_ControlPadding(new Vector2(0f, 6f));
			((Panel)val6).set_CanScroll(true);
			((Panel)val6).set_ShowBorder(true);
			_sharePanel = val6;
			Service.CommunityCatalog.CatalogUpdated += delegate
			{
				GameThreadUtil.Enqueue(delegate
				{
					LoadCategorySelection();
					ReloadMarkerList(_currentMapFilter!.get_Checked());
				});
			};
			LoadCategorySelection();
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
			_hideImportedFilter!.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
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
			RenderLibraryList(_listingPanel, filterToCurrent, currentMapId);
		}

		protected void LoadCategorySelection()
		{
			Dropdown? categorySelection = _categorySelection;
			string selected = ((categorySelection != null) ? categorySelection!.get_SelectedItem() : null);
			Dropdown? categorySelection2 = _categorySelection;
			if (categorySelection2 != null)
			{
				categorySelection2!.get_Items().Clear();
			}
			Dropdown? categorySelection3 = _categorySelection;
			if (categorySelection3 != null)
			{
				categorySelection3!.get_Items().Add("All categories");
			}
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (CommunityCategoryEntry category in Service.CommunityCatalog.Categories)
			{
				if (!string.IsNullOrWhiteSpace(category.Name) && seen.Add(category.Name))
				{
					Dropdown? categorySelection4 = _categorySelection;
					if (categorySelection4 != null)
					{
						categorySelection4!.get_Items().Add(category.Name);
					}
				}
			}
			if (_categorySelection!.get_Items().Count <= 1)
			{
				foreach (CommunitySetSummary summary in Service.CommunityCatalog.Sets)
				{
					if (!string.IsNullOrWhiteSpace(summary.CategoryName) && seen.Add(summary.CategoryName))
					{
						_categorySelection!.get_Items().Add(summary.CategoryName);
					}
				}
			}
			if (selected != null && _categorySelection!.get_Items().Contains(selected))
			{
				_categorySelection!.set_SelectedItem(selected);
			}
			else
			{
				_categorySelection!.set_SelectedItem(_categorySelection!.get_Items()[0]);
			}
		}

		[IteratorStateMachine(typeof(_003CVisibleSets_003Ed__17))]
		private IEnumerable<CommunitySetSummary> VisibleSets(bool filterToCurrent, int currentMapId)
		{
			return new _003CVisibleSets_003Ed__17(-2)
			{
				_003C_003E4__this = this,
				_003C_003E3__filterToCurrent = filterToCurrent,
				_003C_003E3__currentMapId = currentMapId
			};
		}

		protected void RenderLibraryList(FlowPanel? panel, bool shouldFilter, int currentMapId)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			if (panel == null)
			{
				return;
			}
			int detailButtonWidth = ((Control)panel).get_Width() - (int)panel!.get_OuterControlPadding().X * 2 - 10;
			int i = 0;
			((Container)panel).get_Children().Clear();
			if (Service.CommunityCatalog.Sets.Count < 1)
			{
				Label val = new Label();
				val.set_Text("Community library not loaded yet. Use Redownload (Ctrl+Shift) to fetch.");
				val.set_AutoSizeWidth(true);
				panel.AddFlowControl((Control)val);
				return;
			}
			foreach (CommunitySetSummary summary in VisibleSets(shouldFilter, currentMapId))
			{
				int markerIdx = i++;
				string mapName = (string.IsNullOrWhiteSpace(summary.MapName) ? Service.MapDataCache.Describe(summary.MapId) : summary.MapName);
				Texture2D fallbackIcon = ((SquadMarker)(markerIdx % 8 + 1)).GetIcon();
				DetailsButton val2 = new DetailsButton();
				val2.set_Text(summary.Name + "\n" + summary.Description + "\n" + mapName);
				val2.set_Icon(AsyncTexture2D.op_Implicit(fallbackIcon));
				val2.set_IconDetails(summary.Author ?? "");
				((Control)val2).set_Width(detailButtonWidth);
				val2.set_BottomSectionHeight((summary.MapId == currentMapId) ? 40 : 35);
				val2.set_HighlightType((DetailsHighlightType)2);
				val2.set_ShowToggleButton(true);
				((Control)val2).set_BackgroundColor((Color)(summary.Enabled ? Color.get_Transparent() : new Color(0.4f, 0.1f, 0.1f, 0.1f)));
				DetailsButton btn = val2;
				Texture2D thumb = Service.PreviewImageCache.GetThumbTexture(summary.Id, fallbackIcon);
				if (thumb != null)
				{
					btn.set_Icon(AsyncTexture2D.op_Implicit(thumb));
				}
				else
				{
					DetailsButton capturedBtn = btn;
					string capturedId = summary.Id;
					Service.PreviewImageCache.RequestThumb(summary.Id, summary.PreviewThumbUrl, delegate
					{
						if (((Control)capturedBtn).get_Parent() != null)
						{
							Texture2D thumbTexture = Service.PreviewImageCache.GetThumbTexture(capturedId, fallbackIcon);
							if (thumbTexture != null)
							{
								capturedBtn.set_Icon(AsyncTexture2D.op_Implicit(thumbTexture));
							}
						}
					});
				}
				if (!string.IsNullOrEmpty(summary.Id))
				{
					MapPreviewTooltip.Apply((Control)(object)btn, MapPreviewTarget.FromCommunitySummary(summary));
				}
				if (summary.MapId == currentMapId)
				{
					IconButton iconButton = new IconButton();
					((Control)iconButton).set_Parent((Container)(object)btn);
					iconButton.Icon = Service.Textures!.IconEye;
					((Control)iconButton).set_BasicTooltipText("Hover to preview markers on the map");
					((Control)iconButton).set_Size(new Point(30, 30));
					((Control)iconButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
					{
						MarkerSet markerSet3 = Service.CommunityCatalog.FetchSetDetail(summary.Id);
						if (markerSet3 != null)
						{
							Service.MapWatch.PreviewMarkerSet(markerSet3);
						}
					});
					((Control)iconButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
					{
						Service.MapWatch.RemovePreviewMarkerSet();
					});
					StandardButton val3 = new StandardButton();
					((Control)val3).set_Parent((Container)(object)btn);
					val3.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!._blishHeartSmall));
					val3.set_Text("Place");
					((Control)val3).set_Width(100);
					((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						MarkerSet markerSet2 = Service.CommunityCatalog.FetchSetDetail(summary.Id);
						if (markerSet2 != null)
						{
							Service.MapWatch.PlaceMarkers(markerSet2);
						}
					});
				}
				bool alreadyImported = Service.MarkersListing.ContainsCommunitySetId(summary.Id);
				StandardButton val4 = new StandardButton();
				val4.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
				val4.set_Text(alreadyImported ? "Imported" : "Import");
				((Control)val4).set_Enabled(!alreadyImported);
				((Control)val4).set_BasicTooltipText("Import this community marker set into your library");
				((Control)val4).set_Parent((Container)(object)btn);
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MarkerSet markerSet = Service.CommunityCatalog.FetchSetDetail(summary.Id);
					if (markerSet == null)
					{
						ScreenNotification.ShowNotification("Failed to fetch marker set from server.", (NotificationType)2, (Texture2D)null, 4);
					}
					else
					{
						Service.MarkersListing.SaveMarker(markerSet);
						ScreenNotification.ShowNotification("Imported \"" + summary.Name + "\" into your library", (NotificationType)5, (Texture2D)null, 4);
						ReloadMarkerList(shouldFilter);
						RenderShareSection();
					}
				});
				panel.AddFlowControl((Control)(object)btn);
			}
		}

		private void RenderShareSection()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Expected O, but got Unknown
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			if (_sharePanel == null)
			{
				return;
			}
			((Container)_sharePanel).get_Children().Clear();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_sharePanel);
			val.set_Text("Share with community");
			val.set_AutoSizeWidth(true);
			if (Service.Gw2ApiManager == null || !Service.Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)_sharePanel);
				val2.set_Text("Enable the Account API permission for Commander Markers in BlishHUD to share marker sets.");
				((Control)val2).set_Width(((Control)_sharePanel).get_Width() - 40);
				val2.set_WrapText(true);
				return;
			}
			List<string> categoryNames = CommunityShareHelper.CategoryNames();
			List<MarkerSet> shareable = Service.MarkersListing.GetAllMarkerSets().Where(MarkerListing.IsShareableWithCommunity).ToList();
			if (shareable.Count == 0)
			{
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)_sharePanel);
				val3.set_Text("No custom marker sets available to share.");
				val3.set_AutoSizeWidth(true);
				return;
			}
			foreach (MarkerSet markerSet in shareable)
			{
				string setId = markerSet.id ?? Guid.NewGuid().ToString();
				if (!_shareRows.TryGetValue(setId, out var rowState))
				{
					rowState = new ShareRowState();
					_shareRows[setId] = rowState;
				}
				Panel val4 = new Panel();
				((Control)val4).set_Parent((Container)(object)_sharePanel);
				((Control)val4).set_Width(((Control)_sharePanel).get_Width() - 40);
				((Control)val4).set_Height((rowState.CategoryIndex == categoryNames.Count || categoryNames.Count == 0) ? 72 : 48);
				Panel row = val4;
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)row);
				val5.set_Text(markerSet.name);
				((Control)val5).set_Location(new Point(0, 0));
				((Control)val5).set_Width(260);
				val5.set_AutoSizeHeight(true);
				Dropdown val6 = new Dropdown();
				((Control)val6).set_Parent((Container)(object)row);
				((Control)val6).set_Location(new Point(270, 0));
				((Control)val6).set_Width(180);
				Dropdown categoryDropdown = val6;
				foreach (string name in categoryNames)
				{
					categoryDropdown.get_Items().Add(name);
				}
				categoryDropdown.get_Items().Add("Custom...");
				int customIndex = categoryNames.Count;
				if (rowState.CategoryIndex < 0 || rowState.CategoryIndex > customIndex)
				{
					rowState.CategoryIndex = ((categoryNames.Count == 0) ? customIndex : 0);
				}
				if (rowState.CategoryIndex < categoryDropdown.get_Items().Count)
				{
					categoryDropdown.set_SelectedItem(categoryDropdown.get_Items()[rowState.CategoryIndex]);
				}
				categoryDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
				{
					rowState.CategoryIndex = categoryDropdown.get_Items().IndexOf(categoryDropdown.get_SelectedItem());
				});
				TextBox customCategoryBox = null;
				if (rowState.CategoryIndex == customIndex || categoryNames.Count == 0)
				{
					TextBox val7 = new TextBox();
					((Control)val7).set_Parent((Container)(object)row);
					((Control)val7).set_Location(new Point(270, 28));
					((Control)val7).set_Width(180);
					((TextInputBase)val7).set_Text(rowState.CustomCategory);
					((Control)val7).set_BasicTooltipText("Type category name");
					customCategoryBox = val7;
					((TextInputBase)customCategoryBox).add_TextChanged((EventHandler<EventArgs>)delegate
					{
						rowState.CustomCategory = ((TextInputBase)customCategoryBox).get_Text();
					});
				}
				StandardButton val8 = new StandardButton();
				((Control)val8).set_Parent((Container)(object)row);
				((Control)val8).set_Location(new Point(460, 0));
				((Control)val8).set_Width(160);
				val8.set_Text(rowState.Sharing ? "Sharing..." : "Share with community");
				val8.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
				((Control)val8).set_Enabled(!rowState.Sharing);
				if (!string.IsNullOrWhiteSpace(rowState.Error))
				{
					Label val9 = new Label();
					((Control)val9).set_Parent((Container)(object)row);
					val9.set_Text(rowState.Error);
					((Control)val9).set_Location(new Point(460, 28));
					((Control)val9).set_Width(220);
				}
				else if (!string.IsNullOrWhiteSpace(rowState.Status))
				{
					Label val10 = new Label();
					((Control)val10).set_Parent((Container)(object)row);
					val10.set_Text(rowState.Status);
					((Control)val10).set_Location(new Point(460, 28));
					((Control)val10).set_Width(220);
				}
				((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (customCategoryBox != null)
					{
						rowState.CustomCategory = ((TextInputBase)customCategoryBox).get_Text();
					}
					string text = CommunityShareHelper.ResolveCategory(rowState.CategoryIndex, rowState.CustomCategory, categoryNames);
					if (string.IsNullOrWhiteSpace(text))
					{
						rowState.Error = "Enter a category name.";
						rowState.Status = "";
						RenderShareSection();
					}
					else
					{
						rowState.Sharing = true;
						rowState.Error = "";
						rowState.Status = "";
						RenderShareSection();
						MarkerSet capturedSet = markerSet;
						string capturedCategory = text;
						ShareRowState capturedRow = rowState;
						Task.Run(async delegate
						{
							CommunityShareResult result = await CommunityShareHelper.SubmitAsync(capturedSet, capturedCategory).ConfigureAwait(continueOnCapturedContext: false);
							if (result.Success)
							{
								capturedRow.Status = result.Message;
							}
							else
							{
								capturedRow.Error = result.Message;
							}
							capturedRow.Sharing = false;
							GameThreadUtil.Enqueue(new Action(RenderShareSection));
						});
					}
				});
			}
		}

		public AutoMarkerCommunityLibraryView()
			: this()
		{
		}
	}
}
