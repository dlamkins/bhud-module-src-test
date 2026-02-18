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
		private readonly BusinessService _businessService;

		private LoadingSpinner _loadingSpinner;

		private FlowPanel _tableContainer;

		private readonly List<Label> _labels = new List<Label>();

		private readonly List<StandardButton> _buttons = new List<StandardButton>();

		private List<Area> _areas;

		private List<string> _accountClears;

		private List<string> _bounties;

		private List<string> _neverOnTheMenu;

		private bool _showClears = true;

		private List<(Panel, Label, string)> _tablePanels = new List<(Panel, Label, string)>();

		private ResourceManager _areasResx;

		private ResourceManager _encountersResx;

		public RaidWeekPlannerWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, AsyncTexture2D cornerIconTexture, BusinessService businessService)
			: this(background, windowRegion, contentRegion)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("RaidWeekPlanner");
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(cornerIconTexture));
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_SavesPosition(true);
			_businessService = businessService;
			_areas = businessService.GetAreas();
			_bounties = businessService.GetEventsForCurrentWeek();
			_neverOnTheMenu = businessService.GetNeverOnTheMenu();
			_areasResx = areas.ResourceManager;
			_encountersResx = encounters.ResourceManager;
		}

		public void BuildUi()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Expected O, but got Unknown
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
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
			AddHeaders(_tableContainer);
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
				SetLocalizedText = () => strings.MainWindow_Button_Refresh_Label
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
				SetLocalizedText = () => strings.MainWindow_Button_Toggle_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_Toggle_Tooltip
			};
			((Control)obj2).set_Parent((Container)(object)actionContainer);
			StandardButton toggleButton = (StandardButton)(object)obj2;
			buttons2.Add((StandardButton)(object)obj2);
			((Control)toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleClears();
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
			DrawLegend(legendContainer);
			DrawData();
		}

		public void InjectData(List<string> accountClears)
		{
			_accountClears = accountClears;
			DrawData();
		}

		private void AddHeaders(FlowPanel container)
		{
			foreach (Area area in _areas)
			{
				try
				{
					_labels.Add((Label)(object)UiUtils.CreateLabel(() => _areasResx.GetString(area.Key + "Label"), () => _areasResx.GetString(area.Key + "Tooltip"), container, 12, (HorizontalAlignment)1).label);
				}
				catch (Exception)
				{
				}
			}
		}

		private void DrawData()
		{
			ClearLines();
			DrawLines();
		}

		private void ClearLines()
		{
			for (int i = _tablePanels.Count - 1; i >= 0; i--)
			{
				((Control)_tablePanels.ElementAt(i).Item1).Dispose();
			}
		}

		private void DrawLines()
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
						try
						{
							(FlowPanel, Label) label = UiUtils.CreateLabel(() => _encountersResx.GetString(currentEncounter.Key + "Label"), () => _encountersResx.GetString(currentEncounter.Key + "Tooltip"), _tableContainer, 12, (HorizontalAlignment)1);
							((Control)label.Item1).set_BackgroundColor(GetBackgroundColor(currentEncounter.Key));
							_tablePanels.Add(((Panel)(object)label.Item1, (Label)(object)label.Item2, currentEncounter.Key));
						}
						catch (Exception)
						{
						}
					}
					else
					{
						(FlowPanel, Label) emptyLabel = UiUtils.CreateLabel(() => "", () => "", _tableContainer, 12, (HorizontalAlignment)1);
						_tablePanels.Add(((Panel)(object)emptyLabel.Item1, (Label)(object)emptyLabel.Item2, string.Empty));
					}
				}
				count++;
			}
		}

		private void DrawLegend(FlowPanel container)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			UiUtils.CreateLabel(() => strings.Legend_Title, () => "", container, 12, (HorizontalAlignment)1);
			((Control)UiUtils.CreateLabel(() => strings.Legend_None, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.None);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Todo, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Todo);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Planned, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Planned);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Done, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Done);
		}

		private async Task RefreshData()
		{
			((Control)_loadingSpinner).set_Visible(true);
			_accountClears = await _businessService.GetAccountClears(forceRefresh: true);
			DrawData();
			((Control)_loadingSpinner).set_Visible(false);
		}

		private void ToggleClears()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			_showClears = !_showClears;
			foreach (var tablePanel in _tablePanels)
			{
				((Control)tablePanel.Item1).set_BackgroundColor(GetBackgroundColor(tablePanel.Item3));
			}
		}

		private Color GetBackgroundColor(string encounterKey)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(encounterKey))
			{
				return Colors.Empty;
			}
			if (_showClears && _accountClears != null && _accountClears.Contains(encounterKey))
			{
				return Colors.Done;
			}
			if (_bounties != null && _bounties.Contains(encounterKey))
			{
				return Colors.Planned;
			}
			if (!_showClears && _neverOnTheMenu.Contains(encounterKey))
			{
				return Colors.None;
			}
			return Colors.Todo;
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
