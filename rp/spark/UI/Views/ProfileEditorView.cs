using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	public class ProfileEditorView : View
	{
		private const int EmptyGlanceAssetId = 0;

		private const int GlanceEditorWidth = 500;

		private const int GlanceEditorHeight = 595;

		private const int TextBoxWidth = 760;

		private const int ShortTextBoxWidth = 400;

		private const int GlanceSlotSize = 50;

		private const int GlancePadding = 8;

		private const int DescriptionHeight = 185;

		private const int KnownForHeight = 58;

		private const int GlanceY = 435;

		private const int IconSearchResultLimit = 50;

		private const int IconResultListWidth = 444;

		private const int IconResultListHeight = 260;

		private const int IconResultRowHeight = 42;

		private static readonly Color PopupBackground = new Color(38, 35, 32);

		private static readonly Color PopupPanelBackground = new Color(28, 27, 25);

		private static readonly Color PopupRowBackground = new Color(42, 40, 37);

		private static readonly Color PopupAlternateRowBackground = new Color(52, 49, 45);

		private static readonly Color PopupStatusText = new Color(210, 210, 210);

		private readonly ProfileEditorSession _session;

		private readonly IconIndexService _iconIndex;

		private Container _buildPanel;

		private Label _status;

		private TextBox _displayName;

		private TextBox _customProfession;

		private TextBox _pronouns;

		private MultilineTextBox _knownFor;

		private MultilineTextBox _description;

		private AssetIcon[] _glanceSlots;

		private Panel _glanceEditorPanel;

		private bool _isRefreshing;

		private Checkbox _matureCheckbox;

		public ProfileEditorView(ProfileEditorSession session, IconIndexService iconIndex = null)
			: this()
		{
			_session = session;
			_iconIndex = iconIndex;
		}

		protected override void Build(Container buildPanel)
		{
			_buildPanel = buildPanel;
			if (!_session.State.CanEditProfile)
			{
				ProfileEditorUI.ShowUnavailableMessage(buildPanel);
				return;
			}
			BuildFields(buildPanel);
			ProfileEditorUI.AddLabel(buildPanel, "At a Glance", 435);
			BuildGlance(buildPanel, 460);
			BuildFooter(buildPanel);
			_session.ProfileChanged += HandleProfileChanged;
			RefreshFromSession();
		}

		private void BuildFields(Container buildPanel)
		{
			FlowPanel form = SparkFormLayout.AddVerticalStack(buildPanel, 0, 0, 760, 425, 4);
			_displayName = SparkFormLayout.AddLabeledTextBox(form, "Character Name", string.Empty, string.Empty, 400, 35, 30);
			((TextInputBase)_displayName).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.DisplayName = ((TextInputBase)_displayName).get_Text()?.Trim() ?? string.Empty;
				}
			});
			FlowPanel professionRow = SparkFormLayout.AddRow((Container)(object)form, 760, 60, 30);
			_customProfession = SparkFormLayout.AddLabeledTextBox(professionRow, "Custom Profession", string.Empty, string.Empty, 400, 35, 40);
			((TextInputBase)_customProfession).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.CustomProfession = ((TextInputBase)_customProfession).get_Text()?.Trim() ?? string.Empty;
				}
			});
			_pronouns = SparkFormLayout.AddLabeledTextBox(professionRow, "Pronouns", string.Empty, string.Empty, 220, 35, 20);
			((TextInputBase)_pronouns).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.Pronouns = ((TextInputBase)_pronouns).get_Text()?.Trim() ?? string.Empty;
				}
			});
			_knownFor = (MultilineTextBox)(object)SparkFormLayout.AddLabeledMultilineTextBox(form, "Known For", string.Empty, string.Empty, 760, 58, 500);
			((TextInputBase)_knownFor).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.KnownFor = ((TextInputBase)_knownFor).get_Text()?.Trim() ?? string.Empty;
				}
			});
			_description = (MultilineTextBox)(object)SparkFormLayout.AddLabeledMultilineTextBox(form, "Description", string.Empty, string.Empty, 760, 185, 8000);
			((TextInputBase)_description).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.Description = ((TextInputBase)_description).get_Text()?.Trim() ?? string.Empty;
				}
			});
		}

		private void BuildFooter(Container buildPanel)
		{
			_status = ProfileEditorUI.AddSaveFooter(buildPanel, _session, 515, 520, 560, 455, 305);
			_matureCheckbox = SparkViewUI.AddCheckbox(buildPanel, "Mark profile as mature/18+", _session.Profile.IsMature, 170, 517, 270);
			_matureCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.IsMature = _matureCheckbox.get_Checked();
				}
			});
			_session.StatusChanged += HandleStatusChanged;
		}

		private void HandleStatusChanged(string statusText)
		{
			SparkUiThread.Queue(delegate
			{
				Label status = _status;
				if (((status != null) ? ((Control)status).get_Parent() : null) != null)
				{
					_status.set_Text(statusText ?? string.Empty);
				}
			});
		}

		private void HandleProfileChanged()
		{
			SparkUiThread.Queue(delegate
			{
				TextBox displayName = _displayName;
				if (((displayName != null) ? ((Control)displayName).get_Parent() : null) != null)
				{
					RefreshFromSession();
				}
			});
		}

		private void RefreshFromSession()
		{
			_isRefreshing = true;
			try
			{
				((TextInputBase)_displayName).set_Text(_session.Profile.DisplayName ?? string.Empty);
				((TextInputBase)_customProfession).set_Text(_session.Profile.CustomProfession ?? string.Empty);
				((TextInputBase)_pronouns).set_Text(_session.Profile.Pronouns ?? string.Empty);
				((TextInputBase)_knownFor).set_Text(_session.Profile.KnownFor ?? string.Empty);
				((TextInputBase)_description).set_Text(_session.Profile.Description ?? string.Empty);
				_matureCheckbox.set_Checked(_session.Profile.IsMature);
				if (_glanceSlots != null)
				{
					for (int i = 0; i < _glanceSlots.Length; i++)
					{
						RefreshGlance(i);
					}
				}
			}
			finally
			{
				_isRefreshing = false;
			}
		}

		private void BuildGlance(Container buildPanel, int y)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			_glanceSlots = new AssetIcon[_session.Glance.Length];
			for (int i = 0; i < _glanceSlots.Length; i++)
			{
				int slotIndex = i;
				AssetIcon[] glanceSlots = _glanceSlots;
				int num = i;
				AssetIcon assetIcon = new AssetIcon();
				((Control)assetIcon).set_Location(new Point(i * 58, y));
				((Control)assetIcon).set_Size(new Point(50, 50));
				((Control)assetIcon).set_Parent(buildPanel);
				((Control)assetIcon).set_BackgroundColor(new Color(20, 20, 20, 180));
				glanceSlots[num] = assetIcon;
				((Control)_glanceSlots[i]).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					EditGlance(slotIndex);
				});
				RefreshGlance(slotIndex);
			}
		}

		private void RefreshGlance(int index)
		{
			AssetIcon obj = _glanceSlots[index];
			AtAGlanceEntry entry = _session.Glance[index];
			int assetId = ((entry.AssetId > 0) ? entry.AssetId : 0);
			obj.SetAssetId(assetId);
			((Control)obj).set_Tooltip(MakeGlanceTooltip(entry));
		}

		private static Tooltip MakeGlanceTooltip(AtAGlanceEntry entry)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			return new Tooltip((ITooltipView)(object)new ProfileTooltipView(entry?.Title, entry?.Description, "At a Glance"));
		}

		private void EditGlance(int index)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Expected O, but got Unknown
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Expected O, but got Unknown
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Expected O, but got Unknown
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Expected O, but got Unknown
			CloseGlanceEditor();
			AtAGlanceEntry entry = _session.Glance[index];
			Container popupParent = (Container)(((object)_buildPanel) ?? ((object)GameService.Graphics.get_SpriteScreen()));
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("At a Glance");
			((Control)val).set_Size(new Point(500, 595));
			((Control)val).set_Location(GetGlanceEditorLocation(popupParent));
			((Control)val).set_Parent(popupParent);
			((Control)val).set_BackgroundColor(PopupBackground);
			((Control)val).set_ClipsBounds(false);
			((Control)val).set_ZIndex(100);
			_glanceEditorPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("X");
			((Control)val2).set_Location(new Point(468, -28));
			((Control)val2).set_Size(new Point(24, 24));
			((Control)val2).set_Parent((Container)(object)_glanceEditorPanel);
			((Control)val2).set_ClipsBounds(false);
			((Control)val2).set_ZIndex(10011);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseGlanceEditor();
			});
			ProfileEditorUI.AddLabel((Container)(object)_glanceEditorPanel, "Icon ID", 10);
			TextBox val3 = new TextBox();
			((TextInputBase)val3).set_Text((entry.AssetId > 0) ? entry.AssetId.ToString() : string.Empty);
			((TextInputBase)val3).set_PlaceholderText("Asset ID");
			((Control)val3).set_Location(new Point(12, 35));
			((Control)val3).set_Size(new Point(110, 30));
			((Control)val3).set_Parent((Container)(object)_glanceEditorPanel);
			TextBox assetIdBox = val3;
			AssetIcon assetIcon = new AssetIcon();
			((Control)assetIcon).set_Location(new Point(132, 34));
			((Control)assetIcon).set_Size(new Point(32, 32));
			((Control)assetIcon).set_Parent((Container)(object)_glanceEditorPanel);
			((Control)assetIcon).set_BackgroundColor(new Color(20, 20, 20, 180));
			AssetIcon selectedIcon = assetIcon;
			UpdateIconPreview(assetIdBox, selectedIcon);
			ProfileEditorUI.AddLabel((Container)(object)_glanceEditorPanel, "Search Icons", 10, 180);
			TextBox val4 = new TextBox();
			((TextInputBase)val4).set_Text(string.Empty);
			((TextInputBase)val4).set_PlaceholderText("Search");
			((Control)val4).set_Location(new Point(180, 35));
			((Control)val4).set_Size(new Point(220, 30));
			((Control)val4).set_Parent((Container)(object)_glanceEditorPanel);
			TextBox iconSearchBox = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Search");
			((Control)val5).set_Location(new Point(410, 35));
			((Control)val5).set_Size(new Point(62, 30));
			((Control)val5).set_Parent((Container)(object)_glanceEditorPanel);
			StandardButton searchButton = val5;
			ProfileScrollList profileScrollList = new ProfileScrollList(444, 260, 42, 2);
			((Control)profileScrollList).set_Location(new Point(12, 75));
			((Control)profileScrollList).set_Parent((Container)(object)_glanceEditorPanel);
			((Control)profileScrollList).set_BackgroundColor(PopupPanelBackground);
			ProfileScrollList iconResultsList = profileScrollList;
			ProfileEditorUI.AddLabel((Container)(object)_glanceEditorPanel, "Title", 345);
			TextBox val6 = new TextBox();
			((TextInputBase)val6).set_Text(entry.Title ?? string.Empty);
			((TextInputBase)val6).set_PlaceholderText("Title");
			((TextInputBase)val6).set_MaxLength(60);
			((Control)val6).set_Location(new Point(12, 370));
			((Control)val6).set_Size(new Point(460, 30));
			((Control)val6).set_Parent((Container)(object)_glanceEditorPanel);
			TextBox titleBox = val6;
			ProfileEditorUI.AddLabel((Container)(object)_glanceEditorPanel, "Description", 405);
			SparkMultiline sparkMultiline = new SparkMultiline();
			((TextInputBase)sparkMultiline).set_Text(entry.Description ?? string.Empty);
			((TextInputBase)sparkMultiline).set_PlaceholderText("Description");
			((TextInputBase)sparkMultiline).set_MaxLength(280);
			((Control)sparkMultiline).set_Location(new Point(12, 430));
			((Control)sparkMultiline).set_Size(new Point(460, 55));
			((Control)sparkMultiline).set_Parent((Container)(object)_glanceEditorPanel);
			SparkMultiline descriptionBox = sparkMultiline;
			descriptionBox.AttachWheelSource((Container)(object)_glanceEditorPanel);
			StandardButton val7 = new StandardButton();
			val7.set_Text("Confirm");
			((Control)val7).set_Location(new Point(12, 505));
			((Control)val7).set_Size(new Point(95, 30));
			((Control)val7).set_Parent((Container)(object)_glanceEditorPanel);
			StandardButton confirmButton = val7;
			StandardButton val8 = new StandardButton();
			val8.set_Text("Clear");
			((Control)val8).set_Location(new Point(117, 505));
			((Control)val8).set_Size(new Point(75, 30));
			((Control)val8).set_Parent((Container)(object)_glanceEditorPanel);
			Label val9 = new Label();
			val9.set_Text(string.Empty);
			((Control)val9).set_Location(new Point(202, 509));
			((Control)val9).set_Size(new Point(270, 25));
			val9.set_TextColor(PopupStatusText);
			((Control)val9).set_Parent((Container)(object)_glanceEditorPanel);
			Label status = val9;
			((TextInputBase)assetIdBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				UpdateIconPreview(assetIdBox, selectedIcon);
			});
			iconSearchBox.add_EnterPressed((EventHandler<EventArgs>)async delegate
			{
				await SearchIconsAsync(((TextInputBase)iconSearchBox).get_Text(), assetIdBox, selectedIcon, iconResultsList, status);
			});
			((Control)searchButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await SearchIconsAsync(((TextInputBase)iconSearchBox).get_Text(), assetIdBox, selectedIcon, iconResultsList, status);
			});
			status.set_Text("Press Enter or Search.");
			((Control)confirmButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				string text = ((TextInputBase)assetIdBox).get_Text()?.Trim();
				if (string.IsNullOrWhiteSpace(text))
				{
					_session.Glance[index].AssetId = 0;
					_session.Glance[index].Title = string.Empty;
					_session.Glance[index].Description = string.Empty;
					_session.Glance[index].Tooltip = string.Empty;
				}
				else
				{
					if (!int.TryParse(text, out var result) || result <= 0)
					{
						status.set_Text("Invalid ID.");
						return;
					}
					_session.Glance[index].AssetId = result;
					_session.Glance[index].Title = ((TextInputBase)titleBox).get_Text()?.Trim() ?? string.Empty;
					_session.Glance[index].Description = ((TextInputBase)descriptionBox).get_Text()?.Trim() ?? string.Empty;
					_session.Glance[index].Tooltip = string.Empty;
				}
				RefreshGlance(index);
				CloseGlanceEditor();
			});
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_session.Glance[index].AssetId = 0;
				_session.Glance[index].Title = string.Empty;
				_session.Glance[index].Description = string.Empty;
				_session.Glance[index].Tooltip = string.Empty;
				RefreshGlance(index);
				CloseGlanceEditor();
			});
		}

		private static bool IsIconSearchUiAlive(ProfileScrollList resultsList, Label status)
		{
			if (((resultsList != null) ? ((Control)resultsList).get_Parent() : null) != null)
			{
				return ((status != null) ? ((Control)status).get_Parent() : null) != null;
			}
			return false;
		}

		private async Task SearchIconsAsync(string queryText, TextBox assetIdBox, AssetIcon selectedIcon, ProfileScrollList resultsList, Label status)
		{
			if (!IsIconSearchUiAlive(resultsList, status))
			{
				return;
			}
			resultsList.ClearRows();
			if (_iconIndex == null)
			{
				status.set_Text("Icon search unavailable.");
				return;
			}
			string query = queryText?.Trim() ?? string.Empty;
			if (query.Length < 2)
			{
				status.set_Text("Enter at least 2 characters.");
				return;
			}
			status.set_Text("Searching...");
			try
			{
				IReadOnlyList<Gw2IconSearchResult> results = await _iconIndex.SearchAsync(query, 50);
				if (IsIconSearchUiAlive(resultsList, status))
				{
					ShowIconResults(results, assetIdBox, selectedIcon, resultsList, status);
				}
			}
			catch
			{
				if (IsIconSearchUiAlive(resultsList, status))
				{
					status.set_Text("Icon search failed.");
				}
			}
		}

		private void ShowIconResults(IReadOnlyList<Gw2IconSearchResult> results, TextBox assetIdBox, AssetIcon selectedIcon, ProfileScrollList resultsList, Label status)
		{
			if (!IsIconSearchUiAlive(resultsList, status))
			{
				return;
			}
			resultsList.ClearRows();
			status.set_Text((results.Count == 0) ? "No matches." : $"{results.Count} matches.");
			if (results.Count == 0)
			{
				resultsList.ShowEmptyMessage("No matching icons.");
				return;
			}
			for (int i = 0; i < results.Count; i++)
			{
				AddIconSearchResult(resultsList, results[i], i, assetIdBox, selectedIcon, status);
			}
		}

		private static void AddIconSearchResult(ProfileScrollList list, Gw2IconSearchResult result, int index, TextBox assetIdBox, AssetIcon selectedIcon, Label status)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			Panel row = list.AddRow(index, string.Empty);
			((Control)row).set_BackgroundColor((index % 2 == 0) ? PopupRowBackground : PopupAlternateRowBackground);
			AssetIcon assetIcon = new AssetIcon();
			((Control)assetIcon).set_Location(new Point(3, 2));
			((Control)assetIcon).set_Size(new Point(38, 38));
			((Control)assetIcon).set_Parent((Container)(object)row);
			((Control)assetIcon).set_BackgroundColor(new Color(20, 20, 20, 180));
			AssetIcon icon = assetIcon;
			icon.SetAssetId(result.AssetId);
			Label val = new Label();
			val.set_Text(Shorten(result.Name, 48));
			((Control)val).set_Location(new Point(50, 9));
			((Control)val).set_Size(new Point(380, 26));
			((Control)val).set_Parent((Container)(object)row);
			val.set_TextColor(Color.get_White());
			((Control)row).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectIconSearchResult(result, assetIdBox, selectedIcon, status);
			});
			((Control)icon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectIconSearchResult(result, assetIdBox, selectedIcon, status);
			});
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectIconSearchResult(result, assetIdBox, selectedIcon, status);
			});
		}

		private static void SelectIconSearchResult(Gw2IconSearchResult result, TextBox assetIdBox, AssetIcon selectedIcon, Label status)
		{
			if (((assetIdBox != null) ? ((Control)assetIdBox).get_Parent() : null) != null && ((selectedIcon != null) ? ((Control)selectedIcon).get_Parent() : null) != null && ((status != null) ? ((Control)status).get_Parent() : null) != null)
			{
				((TextInputBase)assetIdBox).set_Text(result.AssetId.ToString());
				selectedIcon.SetAssetId(result.AssetId);
				status.set_Text("Selected.");
			}
		}

		private static void UpdateIconPreview(TextBox assetIdBox, AssetIcon selectedIcon)
		{
			if (selectedIcon != null)
			{
				if (int.TryParse(((TextInputBase)assetIdBox).get_Text()?.Trim(), out var assetId) && assetId > 0)
				{
					selectedIcon.SetAssetId(assetId);
				}
				else
				{
					selectedIcon.SetAssetId(0);
				}
			}
		}

		private static string Shorten(string value, int maxLength)
		{
			value = value?.Trim() ?? string.Empty;
			if (value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, Math.Max(0, maxLength - 3)) + "...";
		}

		private void CloseGlanceEditor()
		{
			Panel glanceEditorPanel = _glanceEditorPanel;
			if (glanceEditorPanel != null)
			{
				((Control)glanceEditorPanel).Dispose();
			}
			_glanceEditorPanel = null;
		}

		protected override void Unload()
		{
			_session.StatusChanged -= HandleStatusChanged;
			_session.ProfileChanged -= HandleProfileChanged;
			CloseGlanceEditor();
		}

		private static Point GetGlanceEditorLocation(Container parent)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			Point size;
			if (parent == null)
			{
				size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			}
			else
			{
				Rectangle contentRegion = parent.get_ContentRegion();
				size = ((Rectangle)(ref contentRegion)).get_Size();
			}
			Point parentSize = size;
			int x = (parentSize.X - 500) / 2;
			int y = Math.Min(36, Math.Max(8, (parentSize.Y - 595) / 2));
			return new Point(Math.Max(8, x), Math.Max(8, y));
		}
	}
}
