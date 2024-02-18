using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusTooltipView : View, ITooltipView, IView
	{
		private List<LoadingStatusTooltipControl> ServiceControls = new List<LoadingStatusTooltipControl>();

		private IEnumerable<IRecurringService> _recurringServices;

		private FlowPanel _panel;

		public LoadingStatusTooltipView(IEnumerable<IRecurringService> services)
		{
			_recurringServices = services;
			foreach (IRecurringService service in services)
			{
				ServiceControls.Add(new LoadingStatusTooltipControl
				{
					Loading = service.Loading,
					Size = new Point(170, 40),
					Name = service.Name,
					LastLoaded = service.LastLoaded,
					LastFailed = service.LastFailed
				});
				service.LoadingStarted += LoadingChanged;
				service.LoadingFinished += LoadingChanged;
			}
		}

		protected override void Build(Container buildPanel)
		{
			_panel = new FlowPanel
			{
				Parent = buildPanel,
				Width = 170,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(0f, 10f)
			};
			foreach (LoadingStatusTooltipControl serviceControl in ServiceControls)
			{
				serviceControl.Parent = _panel;
			}
		}

		private void LoadingChanged(object sender, EventArgs e)
		{
			foreach (IRecurringService service in _recurringServices)
			{
				LoadingStatusTooltipControl control = ServiceControls.FirstOrDefault((LoadingStatusTooltipControl s) => s.Name.Equals(service.Name, StringComparison.OrdinalIgnoreCase));
				if (control != null)
				{
					control.LastLoaded = service.LastLoaded;
					control.LastFailed = service.LastFailed;
					control.Loading = service.Loading;
				}
			}
		}
	}
}
