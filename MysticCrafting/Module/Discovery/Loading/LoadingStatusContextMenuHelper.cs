using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using MysticCrafting.Module.Services.API;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusContextMenuHelper
	{
		public ContextMenuStrip BuildContextMenu(IList<IApiService> services)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			ContextMenuStrip menuStrip = new ContextMenuStrip();
			menuStrip.AddMenuItem(BuildContextMenuItem(services));
			foreach (IApiService service in services)
			{
				menuStrip.AddMenuItem(BuildContextMenuItem(service));
			}
			return menuStrip;
		}

		public ContextMenuStripItem BuildContextMenuItem(IApiService service)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(string.Format(Common.LoadingReloadService, service.Name));
			((Control)val).set_Enabled(!service.Loading);
			ContextMenuStripItem serviceItem = val;
			service.LoadingStarted += ServiceLoadingChanged;
			service.LoadingFinished += ServiceLoadingChanged;
			((Control)serviceItem).add_Disposed((EventHandler<EventArgs>)delegate
			{
				service.LoadingStarted -= ServiceLoadingChanged;
				service.LoadingFinished -= ServiceLoadingChanged;
			});
			((Control)serviceItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!service.Loading)
				{
					Task.Run(async delegate
					{
						await service.LoadSafeAsync();
					});
				}
			});
			return serviceItem;
			void ServiceLoadingChanged(object sender, EventArgs e)
			{
				((Control)serviceItem).set_Enabled(!service.Loading);
			}
		}

		public ContextMenuStripItem BuildContextMenuItem(IList<IApiService> services)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(Common.LoadingReloadAllServices);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Task.Run(async delegate
				{
					await Task.WhenAll(from s in services
						where !s.Loading
						select s into service
						select service.LoadSafeAsync());
				});
			});
			return val;
		}
	}
}
