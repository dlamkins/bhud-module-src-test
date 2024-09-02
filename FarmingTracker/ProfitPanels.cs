using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class ProfitPanels : FlowPanel
	{
		private readonly ProfitPanel _totalProfitPanel;

		private readonly ProfitPanel _profitPerHourPanel;

		private readonly Label _totalProfitLabel;

		private readonly Label _profitPerHourLabel;

		private readonly Stopwatch _stopwatch = new Stopwatch();

		private readonly SettingService _settingService;

		private TimeSpan _oldTime = TimeSpan.Zero;

		private long _totalProfitInCopper;

		public ProfitPanels(Services services, Container parent)
			: this()
		{
			_settingService = services.SettingService;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			string profitTooltip = "Rough profit when selling everything to vendor and on trading post. Click help button for more info.";
			BitmapFont font = services.FontService.Fonts[(FontSize)16];
			_totalProfitPanel = new ProfitPanel(profitTooltip, font, services.TextureService, (Container)(object)this);
			_profitPerHourPanel = new ProfitPanel(profitTooltip, font, services.TextureService, (Container)(object)this);
			_totalProfitLabel = CreateProfitLabel(profitTooltip, font, _totalProfitPanel);
			_profitPerHourLabel = CreateProfitLabel(profitTooltip, font, _profitPerHourPanel);
			_stopwatch.Restart();
			SetTotalAndPerHourProfit(0L, 0L);
			services.SettingService.ProfitPerHourLabelTextSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			services.SettingService.TotalProfitLabelTextSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			OnProfitLabelTextSettingChanged();
		}

		protected override void DisposeControl()
		{
			_settingService.ProfitPerHourLabelTextSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			_settingService.TotalProfitLabelTextSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnProfitLabelTextSettingChanged);
			((FlowPanel)this).DisposeControl();
		}

		public void UpdateProfitLabels(StatsSnapshot snapshot, SafeList<int> ignoredItemApiIds, TimeSpan elapsedFarmingTime)
		{
			long totalProfitInCopper = CalculateTotalProfitInCopper(snapshot, ignoredItemApiIds);
			long profitPerHourInCopper = CalculateProfitPerHourInCopper(totalProfitInCopper, elapsedFarmingTime);
			SetTotalAndPerHourProfit(totalProfitInCopper, profitPerHourInCopper);
		}

		public void UpdateProfitPerHourEveryFiveSeconds(TimeSpan elapsedFarmingTime)
		{
			TimeSpan time = _stopwatch.Elapsed;
			if (time >= _oldTime + TimeSpan.FromSeconds(5.0))
			{
				long profitPerHourInCopper = CalculateProfitPerHourInCopper(_totalProfitInCopper, elapsedFarmingTime);
				_profitPerHourPanel.SetProfit(profitPerHourInCopper);
				_oldTime = time;
			}
		}

		private void SetTotalAndPerHourProfit(long totalProfitInCopper, long profitPerHourInCopper)
		{
			_totalProfitPanel.SetProfit(totalProfitInCopper);
			_profitPerHourPanel.SetProfit(profitPerHourInCopper);
			_totalProfitInCopper = totalProfitInCopper;
		}

		private static long CalculateTotalProfitInCopper(StatsSnapshot snapshot, SafeList<int> ignoredItemApiIds)
		{
			long coinsInCopper = snapshot.CurrencyById.Values.SingleOrDefault((Stat s) => s.IsCoin)?.Count ?? 0;
			List<int> ignoredItemApiIdsCopy = ignoredItemApiIds.ToListSafe();
			long itemsSellProfitInCopper = snapshot.ItemById.Values.Where((Stat s) => !ignoredItemApiIdsCopy.Contains(s.ApiId)).Sum((Stat s) => s.CountSign * s.Profits.All.MaxProfitInCopper);
			long totalProfit = coinsInCopper + itemsSellProfitInCopper;
			if (Module.DebugEnabled)
			{
				Module.Logger.Debug($"totalProfit {totalProfit} = coinsInCopper {coinsInCopper} + itemsSellProfitInCopper {itemsSellProfitInCopper} | " + "maxProfitsPerItem " + string.Join(" ", snapshot.ItemById.Values.Select((Stat s) => s.CountSign * s.Profits.All.MaxProfitInCopper)));
			}
			return totalProfit;
		}

		private static long CalculateProfitPerHourInCopper(long totalProfitInCopper, TimeSpan elapsedFarmingTime)
		{
			if (totalProfitInCopper == 0L)
			{
				return 0L;
			}
			if (elapsedFarmingTime.TotalSeconds < 1.0)
			{
				return 0L;
			}
			double profitPerHourInCopper = (double)totalProfitInCopper / elapsedFarmingTime.TotalHours;
			if (profitPerHourInCopper > 9.223372036854776E+18)
			{
				return long.MaxValue;
			}
			if (profitPerHourInCopper <= -9.223372036854776E+18)
			{
				return -9223372036854775807L;
			}
			return (long)profitPerHourInCopper;
		}

		private Label CreateProfitLabel(string profitTooltip, BitmapFont font, ProfitPanel parent)
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
			((Control)val).set_BasicTooltipText(profitTooltip);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Parent((Container)(object)parent);
			return val;
		}

		private void OnProfitLabelTextSettingChanged(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			_totalProfitLabel.set_Text(" " + _settingService.TotalProfitLabelTextSetting.get_Value());
			_profitPerHourLabel.set_Text(" " + _settingService.ProfitPerHourLabelTextSetting.get_Value());
		}
	}
}
