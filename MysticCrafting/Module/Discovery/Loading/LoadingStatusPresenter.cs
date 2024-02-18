using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusPresenter : IPresenter
	{
		private readonly LoadingStatusView _view;

		private readonly IList<IRecurringService> _recurringServices;

		private LoadingStatusTooltipView _loadingStatusTooltipView;

		public LoadingStatusPresenter(LoadingStatusView view, IList<IRecurringService> recurringServices)
		{
			_view = view;
			_recurringServices = recurringServices;
			foreach (IRecurringService recurringService in recurringServices)
			{
				recurringService.LoadingStarted += LoadingChanged;
				recurringService.LoadingFinished += LoadingChanged;
			}
		}

		public Task<bool> DoLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		public void DoUpdateView()
		{
			UpdateLoadingControls();
		}

		private void LoadingChanged(object sender, EventArgs e)
		{
			UpdateLoadingControls();
		}

		private void UpdateLoadingControls()
		{
			if (_view._loadingSpinner == null || _view._dateLabel == null)
			{
				return;
			}
			if (_recurringServices.Any((IRecurringService s) => s.Loading))
			{
				if (string.IsNullOrEmpty(_view._dateLabel.Text))
				{
					_view._loadingSpinner.Location = new Point(90, 7);
				}
				else
				{
					_view._loadingSpinner.Location = new Point(140, 7);
				}
				_view._loadingSpinner.Show();
			}
			else
			{
				_view._loadingSpinner.Hide();
				_view._statusImage.Tint = ColorHelper.FromServicesLoading(_recurringServices);
				_view._statusImage.Show();
				DateTime lastLoaded = _recurringServices.Max((IRecurringService s) => s.LastLoaded);
				_view._dateLabel.Text = $"{lastLoaded:t}";
				_view._dateLabel.Show();
			}
			BuildTooltip();
		}

		private void BuildTooltip()
		{
			if (_loadingStatusTooltipView == null)
			{
				_loadingStatusTooltipView = new LoadingStatusTooltipView(_recurringServices);
				DisposableTooltip _tooltip = new DisposableTooltip(_loadingStatusTooltipView);
				_view._statusImage.Tooltip = _tooltip;
				_view._nameLabel.Tooltip = _tooltip;
				_view._dateLabel.Tooltip = _tooltip;
				_view._loadingSpinner.Tooltip = _tooltip;
			}
		}

		public void DoUnload()
		{
		}
	}
}
