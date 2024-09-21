using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services.API;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusTooltipView : View, ITooltipView, IView
	{
		private List<LoadingStatusTooltipControl> ServiceControls = new List<LoadingStatusTooltipControl>();

		private IEnumerable<IApiService> _apiServices;

		private FlowPanel _panel;

		private Label _introLabel;

		private Label _rightClickLabel;

		public LoadingStatusTooltipView(IEnumerable<IApiService> services)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			_apiServices = services;
			Label val = new Label();
			val.set_Text(Common.LoadingIntroMessage);
			val.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val).set_Width(220);
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			val.set_TextColor(Color.get_LightGray());
			val.set_ShowShadow(true);
			_introLabel = val;
			foreach (IApiService service in services)
			{
				List<LoadingStatusTooltipControl> serviceControls = ServiceControls;
				LoadingStatusTooltipControl obj = new LoadingStatusTooltipControl
				{
					Loading = service.Loading
				};
				((Control)obj).set_Size(new Point(220, 60));
				obj.Name = service.Name;
				obj.LoadingIntervalMin = service.ExecutionIntervalMinutes;
				obj.LastLoaded = service.LastLoaded;
				obj.LastFailed = service.LastFailed;
				serviceControls.Add(obj);
				service.LoadingStarted += LoadingChanged;
				service.LoadingFinished += LoadingChanged;
			}
			Label val2 = new Label();
			val2.set_Text(Common.LoadingRightClickLabel);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val2).set_Width(220);
			val2.set_WrapText(true);
			val2.set_AutoSizeHeight(true);
			val2.set_TextColor(Color.get_LightGray());
			val2.set_ShowShadow(true);
			_rightClickLabel = val2;
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
			((Control)val).set_Width(220);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(0f, 10f));
			_panel = val;
			((Control)_introLabel).set_Parent((Container)(object)_panel);
			foreach (LoadingStatusTooltipControl serviceControl in ServiceControls)
			{
				((Control)serviceControl).set_Parent((Container)(object)_panel);
			}
			((Control)_rightClickLabel).set_Parent((Container)(object)_panel);
		}

		private void LoadingChanged(object sender, EventArgs e)
		{
			foreach (IApiService service in _apiServices)
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
