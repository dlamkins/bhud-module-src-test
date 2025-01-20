using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using DrmTracker.Domain;
using DrmTracker.Ressources;
using DrmTracker.Services;
using DrmTracker.UI.Controls;
using DrmTracker.Utils;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace DrmTracker.UI.Views
{
	public class DrmTrackerWindow : StandardWindow
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly BusinessService _businessService;

		private LoadingSpinner _loadingSpinner;

		private FlowPanel _tableContainer;

		private readonly List<Label> _labels = new List<Label>();

		private readonly List<StandardButton> _buttons = new List<StandardButton>();

		private List<Map> _maps;

		private List<Faction> _factions;

		private List<Drm> _drms;

		private List<DrmProgression> _accountDrms;

		private List<(Panel, Label)> _tablePanels = new List<(Panel, Label)>();

		private ResourceManager _mapsResx;

		private ResourceManager _factionsResx;

		public DrmTrackerWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, AsyncTexture2D cornerIconTexture, ModuleSettings moduleSettings, BusinessService businessService)
			: this(background, windowRegion, contentRegion)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("DrmTracker");
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(cornerIconTexture));
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_SavesPosition(true);
			_moduleSettings = moduleSettings;
			_businessService = businessService;
			_factions = businessService.GetFactions();
			_maps = businessService.GetMaps();
			_drms = businessService.GetDrms();
			_mapsResx = maps.ResourceManager;
			_factionsResx = factions.ResourceManager;
		}

		public void BuildUi()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
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

		public void InjectData(List<DrmProgression> accountDrms)
		{
			_accountDrms = accountDrms;
			DrawData();
		}

		private void AddHeaders(FlowPanel container)
		{
			_labels.Add((Label)(object)UiUtils.CreateLabel(() => "", () => "", container, 12, (HorizontalAlignment)1).label);
			_labels.Add((Label)(object)UiUtils.CreateLabel(() => "Clear", () => "", container, 12, (HorizontalAlignment)1).label);
			_labels.Add((Label)(object)UiUtils.CreateLabel(() => "CM", () => "", container, 12, (HorizontalAlignment)1).label);
			foreach (Faction faction in _factions)
			{
				_labels.Add((Label)(object)UiUtils.CreateLabel(() => _factionsResx.GetString(faction.Key + "Label"), () => _factionsResx.GetString(faction.Key + "Tooltip"), container, 12, (HorizontalAlignment)1).label);
			}
		}

		private void ClearLines()
		{
			for (int i = _tablePanels.Count - 1; i >= 0; i--)
			{
				((Control)_tablePanels.ElementAt(i).Item1).Dispose();
			}
		}

		private void DrawData()
		{
			ClearLines();
			if (_accountDrms == null)
			{
				DrawEmptyTable();
			}
			else
			{
				DrawLines();
			}
		}

		private void DrawEmptyTable()
		{
			(FlowPanel, Label) lineLabel = UiUtils.CreateLabel(() => strings.NoData, () => "", _tableContainer, 1, (HorizontalAlignment)0);
			List<(Panel, Label)> tablePanels = _tablePanels;
			(FlowPanel, Label) tuple = lineLabel;
			tablePanels.Add(((Panel)(object)tuple.Item1, (Label)(object)tuple.Item2));
		}

		private void DrawLines()
		{
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			foreach (Map map in _maps)
			{
				DrmAchievements drmProgression = _accountDrms?.FirstOrDefault((DrmProgression a) => a.Map == map.Id)?.AccountAchievement;
				(FlowPanel, Label) lineLabel = UiUtils.CreateLabel(() => _mapsResx.GetString(map.Key + "Label"), () => _mapsResx.GetString(map.Key + "Tooltip"), _tableContainer, 12, (HorizontalAlignment)0);
				if ((drmProgression?.HasFullSuccess).GetValueOrDefault())
				{
					((Label)lineLabel.Item2).set_TextColor(Colors.Done);
				}
				List<(Panel, Label)> tablePanels = _tablePanels;
				(FlowPanel, Label) tuple = lineLabel;
				tablePanels.Add(((Panel)(object)tuple.Item1, (Label)(object)tuple.Item2));
				(FlowPanel, Label) label = UiUtils.CreateLabel(() => "", () => _mapsResx.GetString(map.Key + "Tooltip") + " - Clear", _tableContainer, 12, (HorizontalAlignment)1);
				((Control)label.Item1).set_BackgroundColor(GetBackgroundColor(drmProgression?.Clear, "Clear"));
				List<(Panel, Label)> tablePanels2 = _tablePanels;
				tuple = label;
				tablePanels2.Add(((Panel)(object)tuple.Item1, (Label)(object)tuple.Item2));
				label = UiUtils.CreateLabel(() => "", () => _mapsResx.GetString(map.Key + "Tooltip") + " - CM", _tableContainer, 12, (HorizontalAlignment)1);
				((Control)label.Item1).set_BackgroundColor(GetBackgroundColor(drmProgression?.FullCM, "CM"));
				if (drmProgression?.FullCM == null || (drmProgression?.FullCM != null && !drmProgression.FullCM.get_Done()))
				{
					label.Item2.SetLocalizedText = delegate
					{
						AccountAchievement fullCM = drmProgression.FullCM;
						object arg = ((fullCM != null) ? fullCM.get_Current() : 0);
						AccountAchievement fullCM2 = drmProgression.FullCM;
						return $"{arg} / {((fullCM2 != null) ? fullCM2.get_Max() : 5)}";
					};
				}
				List<(Panel, Label)> tablePanels3 = _tablePanels;
				tuple = label;
				tablePanels3.Add(((Panel)(object)tuple.Item1, (Label)(object)tuple.Item2));
				foreach (Faction faction in _factions)
				{
					label = UiUtils.CreateLabel(() => "", () => _mapsResx.GetString(map.Key + "Tooltip") + " - " + _factionsResx.GetString(faction.Key + "Tooltip"), _tableContainer, 12, (HorizontalAlignment)1);
					((Control)label.Item1).set_BackgroundColor(GetBackgroundColorFaction(drmProgression?.Factions, map.Id, faction.Id));
					List<(Panel, Label)> tablePanels4 = _tablePanels;
					tuple = label;
					tablePanels4.Add(((Panel)(object)tuple.Item1, (Label)(object)tuple.Item2));
				}
			}
		}

		private void DrawLegend(FlowPanel container)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			UiUtils.CreateLabel(() => strings.Legend_Title, () => "", container, 12, (HorizontalAlignment)1);
			((Control)UiUtils.CreateLabel(() => strings.Legend_None, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.None);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Todo, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Todo);
			((Control)UiUtils.CreateLabel(() => strings.Legend_Done, () => "", container, 11, (HorizontalAlignment)1).panel).set_BackgroundColor(Colors.Done);
		}

		private async Task RefreshData()
		{
			((Control)_loadingSpinner).set_Visible(true);
			_accountDrms = await _businessService.GetAccountDrm(forceRefresh: true);
			DrawData();
			((Control)_loadingSpinner).set_Visible(false);
		}

		private Color GetBackgroundColor(AccountAchievement accountAchievement, string type)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (accountAchievement == null)
			{
				return Colors.Todo;
			}
			if (type == "Clear")
			{
				if (!accountAchievement.get_Done())
				{
					return Colors.Todo;
				}
				return Colors.Done;
			}
			if (accountAchievement.get_Done())
			{
				return Colors.Done;
			}
			return Colors.Todo;
		}

		private Color GetBackgroundColorFaction(AccountAchievement accountAchievement, int mapId, int factionId)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (accountAchievement == null)
			{
				return Colors.Todo;
			}
			if (!_drms.FirstOrDefault((Drm drm) => drm.Map == mapId).FactionsIds.Contains(factionId))
			{
				return Colors.None;
			}
			if (accountAchievement.get_Done() || accountAchievement.get_Bits().Contains(factionId))
			{
				return Colors.Done;
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
			int columns = 11;
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
