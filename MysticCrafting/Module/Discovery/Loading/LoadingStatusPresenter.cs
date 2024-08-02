using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusPresenter : IPresenter
	{
		private readonly LoadingStatusView _view;

		private readonly IList<IApiService> _recurringServices;

		private LoadingStatusTooltipView _loadingStatusTooltipView;

		public LoadingStatusPresenter(LoadingStatusView view, IList<IApiService> recurringServices)
		{
			_view = view;
			_recurringServices = recurringServices;
			foreach (IApiService recurringService in recurringServices)
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
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			if (_view._loadingSpinner == null || _view._dateLabel == null)
			{
				return;
			}
			if (_recurringServices.Any((IApiService s) => s.Loading))
			{
				if (string.IsNullOrEmpty(_view._dateLabel.get_Text()))
				{
					((Control)_view._loadingSpinner).set_Location(new Point(90, 7));
				}
				else
				{
					((Control)_view._loadingSpinner).set_Location(new Point(140, 7));
				}
				((Control)_view._loadingSpinner).Show();
			}
			else
			{
				((Control)_view._loadingSpinner).Hide();
				_view._statusImage.set_Tint(ColorHelper.FromServicesLoading(_recurringServices));
				((Control)_view._statusImage).Show();
				DateTime lastLoaded = _recurringServices.Max((IApiService s) => s.LastLoaded);
				_view._dateLabel.set_Text($"{lastLoaded:t}");
				((Control)_view._dateLabel).Show();
			}
			BuildTooltip();
		}

		private void BuildTooltip()
		{
			if (_loadingStatusTooltipView == null)
			{
				_loadingStatusTooltipView = new LoadingStatusTooltipView(_recurringServices);
				DisposableTooltip _tooltip = new DisposableTooltip((ITooltipView)(object)_loadingStatusTooltipView);
				((Control)_view._statusImage).set_Tooltip((Tooltip)(object)_tooltip);
				((Control)_view._nameLabel).set_Tooltip((Tooltip)(object)_tooltip);
				((Control)_view._dateLabel).set_Tooltip((Tooltip)(object)_tooltip);
				((Control)_view._loadingSpinner).set_Tooltip((Tooltip)(object)_tooltip);
			}
		}

		public void DoUnload()
		{
		}
	}
}
