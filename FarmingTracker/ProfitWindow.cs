using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class ProfitWindow : RelativePositionAndMouseDraggableContainer
	{
		private readonly Services _services;

		public ProfitPanels ProfitPanels { get; private set; }

		public ProfitWindow(Services services)
			: base(services.SettingService)
		{
			_services = services;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			ProfitPanels = new ProfitPanels(services, isProfitWindow: true, (Container)(object)this);
			OnProfitWindowBackgroundOpacitySettingChanged();
			OnIsProfitWindowVisibleSettingChanged();
			services.SettingService.ProfitWindowBackgroundOpacitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnProfitWindowBackgroundOpacitySettingChanged);
			services.SettingService.IsProfitWindowVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsProfitWindowVisibleSettingChanged);
		}

		private void OnIsProfitWindowVisibleSettingChanged(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			((Control)this).set_Visible(_services.SettingService.IsProfitWindowVisibleSetting.get_Value());
		}

		private void OnProfitWindowBackgroundOpacitySettingChanged(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_BackgroundColor(Color.get_Black() * ((float)_services.SettingService.ProfitWindowBackgroundOpacitySetting.get_Value() / 255f));
		}

		protected override void DisposeControl()
		{
			_services.SettingService.ProfitWindowBackgroundOpacitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnProfitWindowBackgroundOpacitySettingChanged);
			_services.SettingService.IsProfitWindowVisibleSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsProfitWindowVisibleSettingChanged);
			base.DisposeControl();
		}
	}
}
