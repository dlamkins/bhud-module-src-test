using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class SummaryTabView : View, IDisposable
	{
		private bool _isTaskRunning;

		private bool _isUiUpdateTaskRunning;

		private Label _hintLabel;

		private readonly Stopwatch _timeSinceModuleStartStopwatch = new Stopwatch();

		private ProfitPanels _profitPanels;

		private SearchPanel _searchPanel;

		private readonly FlowPanel _rootFlowPanel;

		private Label _drfErrorLabel;

		private OpenSettingsButton _openSettingsButton;

		private FlowPanel _farmingRootFlowPanel;

		private StandardButton _resetButton;

		private FlowPanel _timeAndHintFlowPanel;

		private ElapsedFarmingTimeLabel _elapsedFarmingTimeLabel;

		private string _oldApiTokenErrorTooltip = string.Empty;

		private bool _apiErrorHintVisible;

		private bool _lastStatsUpdateSuccessfull = true;

		private ResetState _resetState;

		private readonly StatsSetter _statsSetter = new StatsSetter();

		private readonly ProfitWindow _profitWindow;

		private readonly Model _model;

		private readonly Services _services;

		private readonly AutomaticResetService _automaticResetService;

		private readonly StatsPanels _statsPanels = new StatsPanels();

		private CollapsibleHelp _collapsibleHelp;

		public const string GW2_API_ERROR_HINT = "GW2 API error";

		public const string FAVORITE_ITEMS_PANEL_TITLE = "Favorite Items";

		public const string ITEMS_PANEL_TITLE = "Items";

		private const string CURRENCIES_PANEL_TITLE = "Currencies";

		private readonly Interval _profitPerHourUpdateInterval = new Interval(TimeSpan.FromMilliseconds(5000.0));

		private readonly Interval _saveFarmingDurationInterval = new Interval(TimeSpan.FromMinutes(1.0));

		private readonly Interval _automaticResetCheckInterval = new Interval(TimeSpan.FromMinutes(1.0), firstIntervalEnded: true);

		public SummaryTabView(FarmingTrackerWindow farmingTrackerWindow, ProfitWindow profitWindow, Model model, Services services)
			: this()
		{
			_profitWindow = profitWindow;
			_model = model;
			_services = services;
			_rootFlowPanel = CreateUi(farmingTrackerWindow);
			AutomaticResetService automaticResetService = (_automaticResetService = new AutomaticResetService(services));
			_timeSinceModuleStartStopwatch.Restart();
			services.UpdateLoop.TriggerUpdateStats();
		}

		public void Dispose()
		{
			_automaticResetService?.Dispose();
		}

		protected override void Unload()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((Control)_rootFlowPanel).set_Parent(buildPanel);
			int resetAndDrfButtonsOffset = 70;
			int width = buildPanel.get_ContentRegion().Width - 30;
			ResizeToViewWidth(resetAndDrfButtonsOffset, width);
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				int width2 = e.get_CurrentRegion().Width - 30;
				ResizeToViewWidth(resetAndDrfButtonsOffset, width2);
			});
		}

		private void ResizeToViewWidth(int resetAndDrfButtonsOffset, int width)
		{
			((Control)_statsPanels.CurrenciesFlowPanel).set_Width(width);
			((Control)_statsPanels.ItemsFlowPanel).set_Width(width);
			((Control)_statsPanels.FavoriteItemsFlowPanel).set_Width(width);
			_statsPanels.ItemsFilterIcon.SetLeft(width);
			_statsPanels.CurrencyFilterIcon.SetLeft(width);
			_searchPanel.UpdateSize(width);
			_collapsibleHelp.UpdateSize(width - resetAndDrfButtonsOffset);
		}

		public void Update(GameTime gameTime)
		{
			_services.UpdateLoop.AddToRunningTime(gameTime.get_ElapsedGameTime().TotalMilliseconds);
			if (!_isUiUpdateTaskRunning && _services.UpdateLoop.HasToUpdateUi())
			{
				_isUiUpdateTaskRunning = true;
				Task.Run(delegate
				{
					StatsSnapshot statsSnapshot = _model.StatsSnapshot;
					_services.ProfitCalculator.CalculateProfits(statsSnapshot, _model.IgnoredItemApiIds, _services.FarmingDuration.Elapsed);
					_profitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
					_profitWindow.ProfitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
					UiUpdater.UpdateStatPanels(_statsPanels, statsSnapshot, _model, _services);
					_isUiUpdateTaskRunning = false;
				});
			}
			if (_saveFarmingDurationInterval.HasEnded())
			{
				_services.FarmingDuration.SaveFarmingTime();
			}
			if (_automaticResetCheckInterval.HasEnded())
			{
				if (_automaticResetService.HasToResetAutomatically())
				{
					_resetState = ResetState.ResetRequired;
				}
				_automaticResetService.UpdateNextResetDateTimeForMinutesUntilResetAfterModuleShutdown();
			}
			if (_resetState != 0)
			{
				_hintLabel.set_Text("Resetting... (this may take a few seconds)");
				if (!_isTaskRunning)
				{
					_isTaskRunning = true;
					_resetState = ResetState.Resetting;
					Task.Run(delegate
					{
						ResetStats();
						_automaticResetService.UpdateNextResetDateTime();
						_elapsedFarmingTimeLabel.RestartTime();
						_services.UpdateLoop.TriggerUpdateUi();
						_services.UpdateLoop.TriggerSaveModel();
						((Control)_resetButton).set_Enabled(true);
						_resetState = ResetState.NoResetRequired;
						_isTaskRunning = false;
					});
				}
				return;
			}
			if (!_isTaskRunning && _services.UpdateLoop.HasToSaveModel())
			{
				_isTaskRunning = true;
				Task.Run(async delegate
				{
					await _services.FileSaver.SaveModelToFile(_model);
					_isTaskRunning = false;
				});
			}
			if (_profitPerHourUpdateInterval.HasEnded())
			{
				_services.ProfitCalculator.CalculateProfitPerHour(_services.FarmingDuration.Elapsed);
				_profitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
				_profitWindow.ProfitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
			}
			_elapsedFarmingTimeLabel.UpdateTimeEverySecond();
			if (!_services.UpdateLoop.UpdateIntervalEnded())
			{
				return;
			}
			_services.UpdateLoop.ResetRunningTime();
			_services.UpdateLoop.UseFarmingUpdateInterval();
			ShowOrHideDrfErrorLabelAndStatPanels(_services.Drf.DrfConnectionStatus, _drfErrorLabel, _openSettingsButton, _farmingRootFlowPanel);
			ApiToken apiToken = new ApiToken(_services.Gw2ApiManager);
			ShowOrHideApiErrorHint(apiToken, _hintLabel, _timeSinceModuleStartStopwatch.Elapsed.TotalSeconds);
			if (apiToken.CanAccessApi && !_isTaskRunning)
			{
				_isTaskRunning = true;
				Task.Run(async delegate
				{
					await UpdateStats();
					_isTaskRunning = false;
				});
			}
		}

		private void ResetStats()
		{
			try
			{
				StatsService.ResetCounts(_model.ItemById);
				StatsService.ResetCounts(_model.CurrencyById);
				_model.UpdateStatsSnapshot();
				_lastStatsUpdateSuccessfull = true;
				_hintLabel.set_Text(" ");
			}
			catch (Exception exception)
			{
				Module.Logger.Error(exception, "ResetStats failed.");
				_hintLabel.set_Text("Module crash. :-(");
			}
		}

		private async Task UpdateStats()
		{
			try
			{
				List<DrfMessage> drfMessages = _services.Drf.GetDrfMessages();
				if (drfMessages.Any() || !_lastStatsUpdateSuccessfull || _services.UpdateLoop.HasToUpdateStats())
				{
					_hintLabel.set_Text("Updating... (this may take a few seconds)");
					await UpdateStatsInModel(drfMessages, _services);
					_model.UpdateStatsSnapshot();
					_services.UpdateLoop.TriggerUpdateUi();
					_services.UpdateLoop.TriggerSaveModel();
					_lastStatsUpdateSuccessfull = true;
					_hintLabel.set_Text(" ");
				}
			}
			catch (Gw2ApiException exception2)
			{
				Module.Logger.Warn((Exception)exception2, exception2.Message);
				_services.UpdateLoop.UseRetryAfterApiFailureUpdateInterval();
				_lastStatsUpdateSuccessfull = false;
				_hintLabel.set_Text(string.Format("{0}. Retry every {1}s", "GW2 API error", 5));
			}
			catch (Exception exception)
			{
				Module.Logger.Error(exception, "UpdateStats failed.");
				_lastStatsUpdateSuccessfull = false;
				_hintLabel.set_Text("Module crash. :-(");
			}
		}

		private static void ShowOrHideDrfErrorLabelAndStatPanels(DrfConnectionStatus drfConnectionStatus, Label drfErrorLabel, OpenSettingsButton openSettingsButton, FlowPanel farmingRootFlowPanel)
		{
			drfErrorLabel.set_Text((drfConnectionStatus == DrfConnectionStatus.Connected) ? "" : DrfConnectionStatusService.GetSummaryTabDrfConnectionStatusText(drfConnectionStatus));
			if (drfConnectionStatus == DrfConnectionStatus.AuthenticationFailed)
			{
				((Control)openSettingsButton).Show();
				((Control)farmingRootFlowPanel).Hide();
			}
			else
			{
				((Control)openSettingsButton).Hide();
				((Control)farmingRootFlowPanel).Show();
			}
		}

		private void ShowOrHideApiErrorHint(ApiToken apiToken, Label hintLabel, double timeSinceModuleStartInSeconds)
		{
			if (!apiToken.CanAccessApi)
			{
				_apiErrorHintVisible = true;
				string apiTokenErrorMessage = apiToken.CreateApiTokenErrorTooltipText();
				bool isGivingBlishSomeTimeToGiveToken = timeSinceModuleStartInSeconds < 20.0;
				bool loadingHintVisible = apiToken.ApiTokenMissing && isGivingBlishSomeTimeToGiveToken;
				LogApiTokenErrorOnce(apiTokenErrorMessage, loadingHintVisible);
				hintLabel.set_Text(loadingHintVisible ? "Loading... (this may take a few seconds)" : $"{apiToken.CreateApiTokenErrorLabelText()} Retry every {2}s");
				((Control)hintLabel).set_BasicTooltipText(loadingHintVisible ? "" : apiTokenErrorMessage);
			}
			else if (_apiErrorHintVisible)
			{
				_apiErrorHintVisible = false;
				hintLabel.set_Text("");
				((Control)hintLabel).set_BasicTooltipText("");
			}
		}

		private void LogApiTokenErrorOnce(string apiTokenErrorMessage, bool loadingHintVisible)
		{
			if (!loadingHintVisible)
			{
				if (_oldApiTokenErrorTooltip != apiTokenErrorMessage)
				{
					Module.Logger.Info(apiTokenErrorMessage);
				}
				_oldApiTokenErrorTooltip = apiTokenErrorMessage;
			}
		}

		private async Task UpdateStatsInModel(List<DrfMessage> drfMessages, Services services)
		{
			DrfResultAdder.UpdateCountsOrAddNewStats(drfMessages, _model.ItemById, _model.CurrencyById);
			await _statsSetter.SetDetailsAndProfitFromApi(_model.ItemById, _model.CurrencyById, services.Gw2ApiManager);
		}

		private FlowPanel CreateUi(FarmingTrackerWindow farmingTrackerWindow)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			FlowPanel rootFlowPanel = val;
			Label val2 = new Label();
			val2.set_Text("");
			val2.set_Font(_services.FontService.Fonts[(FontSize)18]);
			val2.set_TextColor(Color.get_Yellow());
			val2.set_StrokeText(true);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)rootFlowPanel);
			_drfErrorLabel = val2;
			_openSettingsButton = new OpenSettingsButton("Open settings tab to setup DRF", farmingTrackerWindow, (Container)(object)rootFlowPanel);
			((Control)_openSettingsButton).Hide();
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(0f, 5f));
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Parent((Container)(object)rootFlowPanel);
			_farmingRootFlowPanel = val3;
			CreateHelpResetDrfButtons();
			CreateTimeAndHintLabels();
			_profitPanels = new ProfitPanels(_services, isProfitWindow: false, (Container)(object)_farmingRootFlowPanel);
			_searchPanel = new SearchPanel(_services, (Container)(object)_farmingRootFlowPanel);
			CreateStatsPanels((Container)(object)_farmingRootFlowPanel);
			return rootFlowPanel;
		}

		private void CreateStatsPanels(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel currenciesFilterIconPanel = val;
			StatsPanels statsPanels = _statsPanels;
			FlowPanel val2 = new FlowPanel();
			((Panel)val2).set_Title("Currencies");
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val2).set_CanCollapse(true);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)currenciesFilterIconPanel);
			statsPanels.CurrenciesFlowPanel = val2;
			StatsPanels statsPanels2 = _statsPanels;
			FlowPanel val3 = new FlowPanel();
			((Panel)val3).set_Title("Favorite Items");
			val3.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val3).set_CanCollapse(true);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Parent(parent);
			statsPanels2.FavoriteItemsFlowPanel = val3;
			Panel val4 = new Panel();
			((Container)val4).set_WidthSizingMode((SizingMode)1);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Parent(parent);
			Panel itemsFilterIconPanel = val4;
			StatsPanels statsPanels3 = _statsPanels;
			FlowPanel val5 = new FlowPanel();
			((Panel)val5).set_Title("Items");
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val5).set_CanCollapse(true);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Parent((Container)(object)itemsFilterIconPanel);
			statsPanels3.ItemsFlowPanel = val5;
			_statsPanels.CurrencyFilterIcon = new ClickThroughImage(_services.TextureService.FilterTabIconTexture, new Point(380, 3), (Container)(object)currenciesFilterIconPanel);
			_statsPanels.ItemsFilterIcon = new ClickThroughImage(_services.TextureService.FilterTabIconTexture, new Point(380, 3), (Container)(object)itemsFilterIconPanel);
			new HintLabel((Container)(object)_statsPanels.CurrenciesFlowPanel, "  Loading...");
			new HintLabel((Container)(object)_statsPanels.FavoriteItemsFlowPanel, "  Loading...");
			new HintLabel((Container)(object)_statsPanels.ItemsFlowPanel, "  Loading...");
		}

		private void CreateTimeAndHintLabels()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(20f, 0f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)_farmingRootFlowPanel);
			_timeAndHintFlowPanel = val;
			_elapsedFarmingTimeLabel = new ElapsedFarmingTimeLabel(_services, (Container)(object)_timeAndHintFlowPanel);
			Label val2 = new Label();
			val2.set_Text(" ");
			val2.set_Font(_services.FontService.Fonts[(FontSize)14]);
			((Control)val2).set_Width(250);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)_timeAndHintFlowPanel);
			_hintLabel = val2;
		}

		private void CreateHelpResetDrfButtons()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f, 0f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)_farmingRootFlowPanel);
			FlowPanel buttonFlowPanel = val;
			_collapsibleHelp = new CollapsibleHelp("DRF setup instructions and DRF and module troubleshooting can be found in the settings tab.\n\nPROFIT:\n15% trading post fee is already deducted.\nProfit also includes changes in 'raw gold'. In other words coins spent or gained. 'raw gold' changes are also visible in the 'Currencies' panel.\nLost items reduce the profit accordingly.\nCurrencies are not included in the profit calculation (except 'raw gold').\nrough profit = raw gold + item count * tp sell price * 0.85 + ...for all items.\nWhen tp sell price does not exist, tp buy price will be used. Vendor price will be used when it is higher than tp sell/buy price * 0.85.\nModule and DRF live tracking website profit calculation may differ because different profit formulas are used.\n" + $"Profit per hour is updated every {5} seconds.\n" + "The profit is only a rough estimate because the trading post buy/sell prices can change over time and only the highest tp buy price and tp sell price for an item are considered. The tp buy/sell prices are a snapshot from when the item was tracked for the first time during a blish sesssion.\n\nRESIZE:\nYou can resize the window by dragging the bottom right window corner. Some UI elements might be cut off when the window becomes too small.", 270, (Container)(object)buttonFlowPanel);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)0);
			val2.set_ControlPadding(new Vector2(5f, 5f));
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)buttonFlowPanel);
			FlowPanel subButtonFlowPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Reset");
			((Control)val3).set_BasicTooltipText("Start new farming session by resetting tracked items and currencies.");
			((Control)val3).set_Width(90);
			((Control)val3).set_Parent((Container)(object)subButtonFlowPanel);
			_resetButton = val3;
			((Control)_resetButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_resetButton).set_Enabled(false);
				_resetState = ResetState.ResetRequired;
			});
			((Control)new OpenUrlInBrowserButton("https://drf.rs/dashboard/livetracker/summary", "DRF", "Open DRF live tracking website in your default web browser.\nThe module and the DRF live tracking web page are both DRF clients. But they are independent of each other. They do not synchronize the data they display. So one client may show less or more data dependend on when the client session started.", _services.TextureService.OpenLinkTexture, (Container)(object)subButtonFlowPanel)).set_Width(60);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Export CSV");
			((Control)val4).set_BasicTooltipText("Export tracked items and currencies to '" + _services.CsvFileExporter.ModuleFolderPath + "\\<date-time>.csv'.\nThis feature can be used to import the tracked items/currencies in Microsoft Excel for example.");
			((Control)val4).set_Width(90);
			((Control)val4).set_Parent((Container)(object)subButtonFlowPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_services.CsvFileExporter.ExportSummaryAsCsvFile(_model);
			});
		}
	}
}
