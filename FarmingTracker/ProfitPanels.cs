using System;
using Blish_HUD;
using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class ProfitPanels : FlowPanel
	{
		private readonly ProfitTooltip _profitTooltip;

		private readonly CoinsPanel _profitPanel;

		private readonly CoinsPanel _profitPerHourPanel;

		private readonly Label _profitLabel;

		private readonly Label _profitPerHourLabel;

		private readonly SettingService _settingService;

		private readonly bool _isProfitWindow;

		public ProfitPanels(Services services, bool isProfitWindow, Container parent)
			: this()
		{
			_settingService = services.SettingService;
			_isProfitWindow = isProfitWindow;
			_profitTooltip = new ProfitTooltip(services);
			((Control)this).set_Tooltip((Tooltip)(object)_profitTooltip);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			BitmapFont font = services.FontService.Fonts[(FontSize)16];
			_profitPanel = new CoinsPanel((Tooltip)(object)_profitTooltip, font, services.TextureService, (Container)(object)this);
			_profitPerHourPanel = new CoinsPanel((Tooltip)(object)_profitTooltip, font, services.TextureService, (Container)(object)this);
			_profitLabel = CreateProfitLabel(_profitTooltip, font, _profitPanel);
			_profitPerHourLabel = CreateProfitLabel(_profitTooltip, font, _profitPerHourPanel);
			ShowProfits(0L, 0L);
			services.SettingService.ProfitPerHourLabelTextSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			services.SettingService.ProfitLabelTextSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			services.SettingService.ProfitWindowDisplayModeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<ProfitWindowDisplayMode>>)OnProfitWindowDisplayModeSettingChanged);
			OnProfitLabelTextSettingChanged();
			OnProfitWindowDisplayModeSettingChanged();
		}

		protected override void DisposeControl()
		{
			_settingService.ProfitPerHourLabelTextSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			_settingService.ProfitLabelTextSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			_settingService.ProfitWindowDisplayModeSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ProfitWindowDisplayMode>>)OnProfitWindowDisplayModeSettingChanged);
			ProfitTooltip profitTooltip = _profitTooltip;
			if (profitTooltip != null)
			{
				((Control)profitTooltip).Dispose();
			}
			((FlowPanel)this).DisposeControl();
		}

		public void ShowProfits(long profitInCopper, long profitPerHourInCopper)
		{
			_profitPanel.SetCoins(profitInCopper);
			_profitPerHourPanel.SetCoins(profitPerHourInCopper);
			_profitTooltip.ProfitPerHourPanel.SetCoins(profitPerHourInCopper);
		}

		private Label CreateProfitLabel(ProfitTooltip profitTooltip, BitmapFont font, CoinsPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			Label val = new Label();
			val.set_Font(font);
			((Control)val).set_Tooltip((Tooltip)(object)profitTooltip);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Parent((Container)(object)parent);
			return val;
		}

		private void OnProfitLabelTextSettingChanged(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			if (_isProfitWindow)
			{
				_profitLabel.set_Text(" " + _settingService.ProfitLabelTextSetting.get_Value());
				_profitPerHourLabel.set_Text(" " + _settingService.ProfitPerHourLabelTextSetting.get_Value());
			}
			else
			{
				_profitLabel.set_Text(" Profit");
				_profitPerHourLabel.set_Text(" Profit per hour");
			}
		}

		private void OnProfitWindowDisplayModeSettingChanged(object sender = null, ValueChangedEventArgs<ProfitWindowDisplayMode> e = null)
		{
			((Control)_profitPanel).set_Parent((Container)null);
			((Control)_profitPerHourPanel).set_Parent((Container)null);
			switch (_settingService.ProfitWindowDisplayModeSetting.get_Value())
			{
			case ProfitWindowDisplayMode.ProfitAndProfitPerHour:
				((Control)_profitPanel).set_Parent((Container)(object)this);
				((Control)_profitPerHourPanel).set_Parent((Container)(object)this);
				break;
			case ProfitWindowDisplayMode.Profit:
				((Control)_profitPanel).set_Parent((Container)(object)this);
				break;
			case ProfitWindowDisplayMode.ProfitPerHour:
				((Control)_profitPerHourPanel).set_Parent((Container)(object)this);
				break;
			}
		}
	}
}
