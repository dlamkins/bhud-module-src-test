using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class SummaryTabView : View, IDisposable
	{
		private bool _statsAccessLocked;

		private bool _isUiUpdateTaskRunning;

		private readonly Stopwatch _timeSinceModuleStartStopwatch = new Stopwatch();

		private string _oldApiTokenErrorTooltip = string.Empty;

		private bool _apiErrorHintVisible;

		private bool _lastStatsUpdateSuccessfull = true;

		private ResetState _resetState;

		private readonly StatsSetter _statsSetter = new StatsSetter();

		private readonly ProfitWindow _profitWindow;

		private readonly Model _model;

		private readonly Services _services;

		private readonly SummaryTabViewControls _controls;

		private readonly AutomaticResetService _automaticResetService;

		private readonly Interval _profitPerHourUpdateInterval = new Interval(TimeSpan.FromMilliseconds(5000.0));

		private readonly Interval _saveFarmingDurationInterval = new Interval(TimeSpan.FromMinutes(1.0));

		private readonly Interval _automaticResetCheckInterval = new Interval(TimeSpan.FromMinutes(1.0), firstIntervalEnded: true);

		public SummaryTabView(ProfitWindow profitWindow, Model model, Services services)
			: this()
		{
			_profitWindow = profitWindow;
			_model = model;
			_services = services;
			_controls = new SummaryTabViewControls(model, services);
			((Control)_controls.ResetButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_controls.ResetButton).set_Enabled(false);
				_resetState = ResetState.ResetRequired;
			});
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
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((Control)_controls.RootFlowPanel).set_Parent(buildPanel);
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
			((Control)_controls.StatsPanels.CurrenciesFlowPanel).set_Width(width);
			((Control)_controls.StatsPanels.ItemsFlowPanel).set_Width(width);
			((Control)_controls.StatsPanels.FavoriteItemsFlowPanel).set_Width(width);
			_controls.StatsPanels.ItemsFilterIcon.SetLeft(width);
			_controls.StatsPanels.CurrencyFilterIcon.SetLeft(width);
			_controls.SearchPanel.UpdateSize(width);
			_controls.CollapsibleHelp.UpdateSize(width - resetAndDrfButtonsOffset);
		}

		public void Update(GameTime gameTime)
		{
			_services.UpdateLoop.AddToRunningTime(gameTime.get_ElapsedGameTime().TotalMilliseconds);
			if (!_isUiUpdateTaskRunning && _services.UpdateLoop.HasToUpdateUi())
			{
				_isUiUpdateTaskRunning = true;
				Task.Run(delegate
				{
					StatsSnapshot statsSnapshot = _model.Stats.StatsSnapshot;
					_services.ProfitCalculator.CalculateProfits(statsSnapshot, _model.CustomStatProfits, _model.IgnoredItemApiIds, _services.FarmingDuration.Elapsed);
					_controls.ProfitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
					_profitWindow.ProfitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
					UiUpdater.UpdateStatPanels(_controls.StatsPanels, statsSnapshot, _model, _services);
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
				_controls.HintLabel.set_Text("Resetting... (this may take a few seconds)");
				if (!_statsAccessLocked)
				{
					_statsAccessLocked = true;
					_resetState = ResetState.Resetting;
					Task.Run(delegate
					{
						ResetStats();
						_automaticResetService.UpdateNextResetDateTime();
						_controls.ElapsedFarmingTimeLabel.RestartTime();
						_services.UpdateLoop.TriggerUpdateUi();
						_services.UpdateLoop.TriggerSaveModel();
						((Control)_controls.ResetButton).set_Enabled(true);
						_resetState = ResetState.NoResetRequired;
						_statsAccessLocked = false;
					});
				}
				return;
			}
			if (!_statsAccessLocked && _services.UpdateLoop.HasToSaveModel())
			{
				_statsAccessLocked = true;
				Task.Run(async delegate
				{
					await _services.FileSaver.SaveModelToFile(_model);
					_statsAccessLocked = false;
				});
			}
			if (_profitPerHourUpdateInterval.HasEnded())
			{
				_services.ProfitCalculator.CalculateProfitPerHour(_services.FarmingDuration.Elapsed);
				_controls.ProfitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
				_profitWindow.ProfitPanels.ShowProfits(_services.ProfitCalculator.ProfitInCopper, _services.ProfitCalculator.ProfitPerHourInCopper);
			}
			_controls.ElapsedFarmingTimeLabel.UpdateTimeEverySecond();
			if (!_services.UpdateLoop.UpdateIntervalEnded())
			{
				return;
			}
			_services.UpdateLoop.ResetRunningTime();
			_services.UpdateLoop.UseFarmingUpdateInterval();
			ShowOrHideDrfErrorLabelAndStatPanels(_services.Drf.DrfConnectionStatus, _controls.DrfErrorLabel, _controls.OpenSettingsButton, _controls.FarmingRootFlowPanel);
			ApiToken apiToken = new ApiToken(_services.Gw2ApiManager);
			ShowOrHideApiErrorHint(apiToken, _controls.HintLabel, _timeSinceModuleStartStopwatch.Elapsed.TotalSeconds);
			if (apiToken.CanAccessApi && !_statsAccessLocked)
			{
				_statsAccessLocked = true;
				Task.Run(async delegate
				{
					await UpdateStats();
					_statsAccessLocked = false;
				});
			}
		}

		private void ResetStats()
		{
			try
			{
				StatsService.ResetCounts(_model.Stats.ItemById);
				StatsService.ResetCounts(_model.Stats.CurrencyById);
				_model.Stats.UpdateStatsSnapshot();
				_lastStatsUpdateSuccessfull = true;
				_controls.HintLabel.set_Text(" ");
			}
			catch (Exception exception)
			{
				Module.Logger.Error(exception, "ResetStats failed.");
				_controls.HintLabel.set_Text("Module crash. :-(");
			}
		}

		private async Task UpdateStats()
		{
			try
			{
				List<DrfMessage> drfMessages = _services.Drf.GetDrfMessages();
				if (drfMessages.Any() || !_lastStatsUpdateSuccessfull || _services.UpdateLoop.HasToUpdateStats())
				{
					_controls.HintLabel.set_Text("Updating... (this may take a few seconds)");
					await UpdateStatsInModel(drfMessages, _services);
					_model.Stats.UpdateStatsSnapshot();
					_services.UpdateLoop.TriggerUpdateUi();
					_services.UpdateLoop.TriggerSaveModel();
					_lastStatsUpdateSuccessfull = true;
					_controls.HintLabel.set_Text(" ");
				}
			}
			catch (Gw2ApiException exception2)
			{
				Module.Logger.Warn((Exception)exception2, exception2.Message);
				_services.UpdateLoop.UseRetryAfterApiFailureUpdateInterval();
				_lastStatsUpdateSuccessfull = false;
				_controls.HintLabel.set_Text(string.Format("{0}. Retry every {1}s", "GW2 API error", 5));
			}
			catch (Exception exception)
			{
				Module.Logger.Error(exception, "UpdateStats failed.");
				_lastStatsUpdateSuccessfull = false;
				_controls.HintLabel.set_Text("Module crash. :-(");
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
			DrfResultAdder.UpdateCountsOrAddNewStats(drfMessages, _model.Stats.ItemById, _model.Stats.CurrencyById);
			await _statsSetter.SetDetailsAndProfitFromApi(_model.Stats.ItemById, _model.Stats.CurrencyById, services.Gw2ApiManager);
		}
	}
}
