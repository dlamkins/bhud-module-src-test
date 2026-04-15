using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using RaidWeekPlanner.Domain;
using RaidWeekPlanner.Ressources;
using RaidWeekPlanner.Services;
using RaidWeekPlanner.UI.Controls;
using RaidWeekPlanner.Utils;

namespace RaidWeekPlanner.UI.Views
{
	public class RaidWeekPlannerWindow : StandardWindow
	{
		private LoadingSpinner _loadingSpinner;

		private FlowPanel _tableContainer;

		private List<(Panel, Label, string)> _tablePanels = new List<(Panel, Label, string)>();

		private List<Label> _labels = new List<Label>();

		private readonly List<StandardButton> _buttons = new List<StandardButton>();

		private StandardButton _toggleTableDrawModeBtn;

		private List<Area> _areas;

		private Dictionary<int, List<string>> _bounties;

		private List<string> _neverOnTheMenu;

		private List<string> _accountClears;

		private TableDrawMode _tableDrawMode;

		private readonly BusinessService _businessService;

		private ResourceManager _stringsResx;

		private ResourceManager _areasResx;

		private ResourceManager _encountersResx;

		public RaidWeekPlannerWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, AsyncTexture2D cornerIconTexture, BusinessService businessService)
			: this(background, windowRegion, contentRegion)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("RaidWeekPlanner");
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(cornerIconTexture));
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_SavesPosition(true);
			_businessService = businessService;
			_stringsResx = strings.ResourceManager;
			_areasResx = areas.ResourceManager;
			_encountersResx = encounters.ResourceManager;
			LoadData();
		}

		public void BuildUi()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			((Panel)flowPanel).set_CanScroll(true);
			FlowPanel mainContainer = flowPanel;
			((Control)mainContainer).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				int width = ((Control)mainContainer).get_Width() - 20;
				((Control)_tableContainer).set_Width(width);
			});
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)mainContainer);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)0);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Panel)flowPanel2).set_ShowBorder(true);
			((FlowPanel)flowPanel2).set_OuterControlPadding(new Vector2(5f));
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(5f));
			_tableContainer = flowPanel2;
			((Container)_tableContainer).add_ContentResized((EventHandler<RegionChangedEventArgs>)TableContainer_ContentResized);
			FlowPanel flowPanel3 = new FlowPanel();
			((Control)flowPanel3).set_Parent((Container)(object)mainContainer);
			((Container)flowPanel3).set_WidthSizingMode((SizingMode)2);
			((Control)flowPanel3).set_Height(35);
			((FlowPanel)flowPanel3).set_OuterControlPadding(new Vector2(5f));
			((FlowPanel)flowPanel3).set_ControlPadding(new Vector2(5f));
			FlowPanel actionContainer = flowPanel3;
			((Container)actionContainer).add_ContentResized((EventHandler<RegionChangedEventArgs>)ActionContainer_ContentResized);
			List<StandardButton> buttons = _buttons;
			StandardButton obj = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_Refresh_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_Refresh_Tooltip
			};
			((Control)obj).set_Parent((Container)(object)actionContainer);
			StandardButton button = (StandardButton)(object)obj;
			buttons.Add((StandardButton)(object)obj);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await RefreshData();
			});
			List<StandardButton> buttons2 = _buttons;
			StandardButton obj2 = new StandardButton
			{
				SetLocalizedText = () => GetToggleTableDrawModeBtnLabel(),
				SetLocalizedTooltip = () => strings.MainWindow_Button_ToggleTableDrawMode_Tooltip
			};
			((Control)obj2).set_Parent((Container)(object)actionContainer);
			StandardButton item = (StandardButton)(object)obj2;
			_toggleTableDrawModeBtn = (StandardButton)(object)obj2;
			buttons2.Add(item);
			((Control)_toggleTableDrawModeBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleTableDrawMode();
			});
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)actionContainer);
			((Control)val).set_Size(new Point(29, 29));
			((Control)val).set_Visible(false);
			_loadingSpinner = val;
			FlowPanel flowPanel4 = new FlowPanel();
			((Control)flowPanel4).set_Parent((Container)(object)mainContainer);
			((Container)flowPanel4).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel4).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel4).set_OuterControlPadding(new Vector2(5f));
			((FlowPanel)flowPanel4).set_ControlPadding(new Vector2(5f));
			FlowPanel legendContainer = flowPanel4;
			FlowPanel flowPanel5 = new FlowPanel();
			((Control)flowPanel5).set_Parent((Container)(object)mainContainer);
			((Container)flowPanel5).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel5).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel5).set_OuterControlPadding(new Vector2(5f));
			((FlowPanel)flowPanel5).set_ControlPadding(new Vector2(5f));
			FlowPanel disclaimerContainer = flowPanel5;
			UiUtils.CreateLabel(() => strings.MainWindow_Label_ClearTrack_Notice, () => "", disclaimerContainer, 1, (HorizontalAlignment)1);
			UiUtils.CreateLabel(() => strings.MainWindow_Label_ClearTrack_Notice2, () => "", disclaimerContainer, 1, (HorizontalAlignment)1);
			DrawLegend(legendContainer);
			DrawTable();
		}

		public void InjectData(List<string> accountClears)
		{
			_accountClears = accountClears;
			UpdateColors();
		}

		private void LoadData()
		{
			_areas = _businessService.GetAreas();
			_bounties = _businessService.GetEventsForCurrentWeek();
			_neverOnTheMenu = _businessService.GetNeverOnTheMenu();
		}

		public string GetToggleTableDrawModeBtnLabel()
		{
			if (_tableDrawMode != 0)
			{
				return strings.MainWindow_Button_ToggleTableDrawMode_Areas;
			}
			return strings.MainWindow_Button_ToggleTableDrawMode_Week;
		}

		private void DrawLegend(FlowPanel container)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			UiUtils.CreateLabel(() => strings.Legend_Title, () => "", container, 12, (HorizontalAlignment)1);
			((Control)UiUtils.CreateLabel(() => strings.Legend_None_Label, () => strings.Legend_None_Tooltip, container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.None);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Todo_Label, () => strings.Legend_Todo_Tooltip, container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Todo);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Planned_Label, () => strings.Legend_Planned_Tooltip, container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Planned);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Done_Label, () => strings.Legend_Done_Tooltip, container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Done);
		}

		private void DrawTable()
		{
			if (_tableDrawMode == TableDrawMode.Week)
			{
				DrawTableAsWeek();
			}
			else if (_tableDrawMode == TableDrawMode.Areas)
			{
				DrawTableAsAreas();
			}
		}

		private void DrawTableAsWeek()
		{
			AddHeadersAsWeek(_tableContainer);
			DrawLinesAsWeek();
		}

		private void AddHeadersAsWeek(FlowPanel container)
		{
			foreach (int day in new List<int>(7) { 0, 1, 2, 3, 4, 5, 6 })
			{
				_labels.Add((Label)(object)UiUtils.CreateLabel(() => _stringsResx.GetString($"day{day}"), () => "", container, 7, (HorizontalAlignment)1).label);
			}
		}

		private void DrawLinesAsWeek()
		{
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			foreach (int row in new List<int>(4) { 0, 1, 2, 3 })
			{
				foreach (KeyValuePair<int, List<string>> bounty in _bounties)
				{
					string encounter = bounty.Value[row];
					(FlowPanel, Label) label = UiUtils.CreateLabel(() => _encountersResx.GetString(encounter + "Label"), () => GetTooltip(encounter), _tableContainer, 7, (HorizontalAlignment)1);
					((Control)label.Item1).set_BackgroundColor(GetBackgroundColor(encounter));
					_tablePanels.Add(((Panel)(object)label.Item1, (Label)(object)label.Item2, encounter));
				}
			}
		}

		private void DrawTableAsAreas()
		{
			AddHeadersAsAreas(_tableContainer);
			DrawLinesAsAreas();
		}

		private void AddHeadersAsAreas(FlowPanel container)
		{
			foreach (Area area in _areas)
			{
				_labels.Add((Label)(object)UiUtils.CreateLabel(() => _areasResx.GetString(area.Key + "Label"), () => _areasResx.GetString(area.Key + "Tooltip"), container, 12, (HorizontalAlignment)1).label);
			}
		}

		private void DrawLinesAsAreas()
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			bool keepDrawing = true;
			int count = 1;
			while (keepDrawing)
			{
				keepDrawing = false;
				foreach (Area area in _areas)
				{
					if (area.Encounters.Count >= count)
					{
						keepDrawing = true;
						Encounter currentEncounter = area.Encounters[count - 1];
						(FlowPanel, Label) label2 = UiUtils.CreateLabel(() => _encountersResx.GetString(currentEncounter.Key + "Label"), () => GetTooltip(currentEncounter.Key), _tableContainer, 12, (HorizontalAlignment)1);
						((Control)label2.Item1).set_BackgroundColor(GetBackgroundColor(currentEncounter.Key));
						_tablePanels.Add(((Panel)(object)label2.Item1, (Label)(object)label2.Item2, currentEncounter.Key));
					}
					else
					{
						var (panel, label) = UiUtils.CreateLabel(() => "", () => "", _tableContainer, 12, (HorizontalAlignment)1);
						_tablePanels.Add(((Panel)(object)panel, (Label)(object)label, string.Empty));
					}
				}
				count++;
			}
		}

		private void UpdateColors()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			foreach (var tablePanel in _tablePanels)
			{
				((Control)tablePanel.Item1).set_BackgroundColor(GetBackgroundColor(tablePanel.Item3));
			}
		}

		private void ClearTable()
		{
			_labels = new List<Label>();
			_tablePanels = new List<(Panel, Label, string)>();
			((Container)_tableContainer).ClearChildren();
		}

		private bool IsCleared(string key)
		{
			return _accountClears?.Contains(key) ?? false;
		}

		private bool IsPlanned(string key)
		{
			return _bounties?.SelectMany((KeyValuePair<int, List<string>> b) => b.Value).Contains(key) ?? false;
		}

		private Color GetBackgroundColor(string encounterKey)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(encounterKey))
			{
				return Colors.Empty;
			}
			switch (_tableDrawMode)
			{
			case TableDrawMode.Week:
				if (IsCleared(encounterKey))
				{
					return Colors.Done;
				}
				return Colors.Planned;
			case TableDrawMode.Areas:
				if (IsCleared(encounterKey))
				{
					return Colors.Done;
				}
				if (IsPlanned(encounterKey))
				{
					return Colors.Planned;
				}
				if (_neverOnTheMenu.Contains(encounterKey))
				{
					return Colors.None;
				}
				return Colors.Todo;
			default:
				return Colors.Todo;
			}
		}

		private string GetTooltip(string currentEncounter)
		{
			string cplTootlip = string.Empty;
			if (_bounties != null && _bounties.SelectMany((KeyValuePair<int, List<string>> b) => b.Value).Contains(currentEncounter))
			{
				IEnumerable<string> allDays = from b in _bounties
					where b.Value.Contains(currentEncounter)
					select _stringsResx.GetString($"day{b.Key}");
				cplTootlip = " - " + string.Join(", ", allDays);
			}
			return _encountersResx.GetString(currentEncounter + "Tooltip") + cplTootlip;
		}

		private async Task RefreshData()
		{
			((Control)_loadingSpinner).set_Visible(true);
			_accountClears = await _businessService.GetAccountClears(forceRefresh: true);
			UpdateColors();
			((Control)_loadingSpinner).set_Visible(false);
		}

		private void ToggleTableDrawMode()
		{
			_tableDrawMode = ((_tableDrawMode == TableDrawMode.Week) ? TableDrawMode.Areas : TableDrawMode.Week);
			ClearTable();
			DrawTable();
			_toggleTableDrawModeBtn.set_Text(GetToggleTableDrawModeBtnLabel());
		}

		private void TableContainer_ContentResized(object sender, RegionChangedEventArgs e)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			List<Label> labels = _labels;
			if (labels == null || labels.Count < 0)
			{
				return;
			}
			int columns = 12;
			Label obj = _labels.FirstOrDefault();
			FlowPanel obj2 = ((obj != null) ? ((Control)obj).get_Parent() : null) as FlowPanel;
			int width = ((((obj2 != null) ? new int?(((Container)obj2).get_ContentRegion().Width) : null) - (int)((obj2 != null) ? ((FlowPanel)obj2).get_OuterControlPadding().X : 0f) - (int)((obj2 != null) ? ((FlowPanel)obj2).get_ControlPadding().X : 0f) * (columns - 1)) / columns).GetValueOrDefault(100);
			foreach (Label label in _labels)
			{
				((Control)label).set_Width(width - 10);
			}
		}

		private void ActionContainer_ContentResized(object sender, RegionChangedEventArgs e)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			List<StandardButton> buttons = _buttons;
			if (buttons == null || buttons.Count < 0)
			{
				return;
			}
			int columns = 9;
			StandardButton obj = _buttons.FirstOrDefault();
			FlowPanel obj2 = ((obj != null) ? ((Control)obj).get_Parent() : null) as FlowPanel;
			int width = ((((obj2 != null) ? new int?(((Container)obj2).get_ContentRegion().Width) : null) - (int)((FlowPanel)obj2).get_OuterControlPadding().X - (int)((FlowPanel)obj2).get_ControlPadding().X * (columns - 1)) / columns).GetValueOrDefault(100);
			foreach (StandardButton button in _buttons)
			{
				((Control)button).set_Width(width);
			}
		}
	}
}
