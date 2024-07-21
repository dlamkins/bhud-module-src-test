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
			: this()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			_recurringServices = services;
			foreach (IRecurringService service in services)
			{
				List<LoadingStatusTooltipControl> serviceControls = ServiceControls;
				LoadingStatusTooltipControl obj = new LoadingStatusTooltipControl
				{
					Loading = service.Loading
				};
				((Control)obj).set_Size(new Point(170, 40));
				obj.Name = service.Name;
				obj.LastLoaded = service.LastLoaded;
				obj.LastFailed = service.LastFailed;
				serviceControls.Add(obj);
				service.LoadingStarted += LoadingChanged;
				service.LoadingFinished += LoadingChanged;
			}
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(170);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(0f, 10f));
			_panel = val;
			foreach (LoadingStatusTooltipControl serviceControl in ServiceControls)
			{
				((Control)serviceControl).set_Parent((Container)(object)_panel);
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
