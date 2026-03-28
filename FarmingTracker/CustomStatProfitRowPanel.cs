using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class CustomStatProfitRowPanel
	{
		public CustomStatProfitRowPanel(Stat stat, HintLabel hintLabel, Model model, Services services, Container parent)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Expected O, but got Unknown
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Expected O, but got Unknown
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			Stat stat2 = stat;
			Services services2 = services;
			HintLabel hintLabel2 = hintLabel;
			Model model2 = model;
			base._002Ector();
			int iconSize = 50;
			int iconMargin = 1;
			int backgroundSize = iconSize + 2 * iconMargin;
			int backgroundMargin = 5;
			int panelHeight = backgroundSize + 2 * backgroundMargin;
			Panel val = new Panel();
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.4f);
			((Control)val).set_Width(450);
			((Control)val).set_Height(panelHeight);
			((Control)val).set_Parent(parent);
			Panel statRowPanel = val;
			Image val2 = new Image(services2.TextureService.InventorySlotBackgroundTexture);
			((Control)val2).set_Location(new Point(backgroundMargin));
			((Control)val2).set_BasicTooltipText(stat2.Details.Name);
			((Control)val2).set_Size(new Point(backgroundSize));
			((Control)val2).set_Parent((Container)(object)statRowPanel);
			Image val3 = new Image(services2.TextureService.GetTextureFromAssetCacheOrFallback(stat2.Details.IconAssetId));
			((Control)val3).set_Location(new Point(backgroundMargin + iconMargin));
			((Control)val3).set_BasicTooltipText(stat2.Details.Name);
			((Control)val3).set_Size(new Point(iconSize));
			((Control)val3).set_Parent((Container)(object)statRowPanel);
			Image statImage = val3;
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(((Control)statImage).get_Right() + 10, backgroundMargin));
			val4.set_Text(stat2.Details.Name);
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Parent((Container)(object)statRowPanel);
			Label statNameLabel = val4;
			if (!stat2.Profit.Unsigned_Custom_ProfitInCopper.HasValue)
			{
				Module.Logger.Error("Cannot create CustomProfit row because CustomProfit is not set");
				return;
			}
			Coin coin = new Coin(stat2.Profit.Unsigned_Custom_ProfitInCopper.Value);
			NumberTextBox numberTextBox = new NumberTextBox(6);
			((Control)numberTextBox).set_Location(new Point(((Control)statImage).get_Right() + 10, ((Control)statNameLabel).get_Bottom() + 5));
			((TextInputBase)numberTextBox).set_Text(coin.Unsigned_Gold.ToString());
			((Control)numberTextBox).set_Width(60);
			((Control)numberTextBox).set_Parent((Container)(object)statRowPanel);
			NumberTextBox goldTextBox = numberTextBox;
			Image val5 = new Image(services2.TextureService.SmallGoldCoinTexture);
			((Control)val5).set_Location(new Point(((Control)goldTextBox).get_Right(), ((Control)statNameLabel).get_Bottom() + 5));
			((Control)val5).set_Size(new Point(((Control)goldTextBox).get_Height()));
			((Control)val5).set_Parent((Container)(object)statRowPanel);
			Image goldCoinImage = val5;
			NumberTextBox numberTextBox2 = new NumberTextBox(2);
			((Control)numberTextBox2).set_Location(new Point(((Control)goldCoinImage).get_Right() + 10, ((Control)statNameLabel).get_Bottom() + 5));
			((TextInputBase)numberTextBox2).set_Text(coin.Unsigned_Silver.ToString());
			((Control)numberTextBox2).set_Width(35);
			((Control)numberTextBox2).set_Parent((Container)(object)statRowPanel);
			NumberTextBox silverTextBox = numberTextBox2;
			Image val6 = new Image(services2.TextureService.SmallSilverCoinTexture);
			((Control)val6).set_Location(new Point(((Control)silverTextBox).get_Right(), ((Control)statNameLabel).get_Bottom() + 5));
			((Control)val6).set_Size(new Point(((Control)silverTextBox).get_Height()));
			((Control)val6).set_Parent((Container)(object)statRowPanel);
			Image silverCoinImage = val6;
			NumberTextBox numberTextBox3 = new NumberTextBox(2);
			((Control)numberTextBox3).set_Location(new Point(((Control)silverCoinImage).get_Right() + 10, ((Control)statNameLabel).get_Bottom() + 5));
			((TextInputBase)numberTextBox3).set_Text(coin.Unsigned_Copper.ToString());
			((Control)numberTextBox3).set_Width(35);
			((Control)numberTextBox3).set_Parent((Container)(object)statRowPanel);
			NumberTextBox copperTextBox = numberTextBox3;
			goldTextBox.NumberTextChanged += delegate
			{
				OnCoinTextChanged(stat2, ((TextInputBase)goldTextBox).get_Text(), ((TextInputBase)silverTextBox).get_Text(), ((TextInputBase)copperTextBox).get_Text(), services2);
			};
			silverTextBox.NumberTextChanged += delegate
			{
				OnCoinTextChanged(stat2, ((TextInputBase)goldTextBox).get_Text(), ((TextInputBase)silverTextBox).get_Text(), ((TextInputBase)copperTextBox).get_Text(), services2);
			};
			copperTextBox.NumberTextChanged += delegate
			{
				OnCoinTextChanged(stat2, ((TextInputBase)goldTextBox).get_Text(), ((TextInputBase)silverTextBox).get_Text(), ((TextInputBase)copperTextBox).get_Text(), services2);
			};
			Image val7 = new Image(services2.TextureService.SmallCopperCoinTexture);
			((Control)val7).set_Location(new Point(((Control)copperTextBox).get_Right(), ((Control)statNameLabel).get_Bottom() + 5));
			((Control)val7).set_Size(new Point(((Control)copperTextBox).get_Height()));
			((Control)val7).set_Parent((Container)(object)statRowPanel);
			StandardButton val8 = new StandardButton();
			val8.set_Text("x");
			((Control)val8).set_Width(28);
			((Control)val8).set_Parent((Container)(object)statRowPanel);
			StandardButton removeButton = val8;
			((Control)removeButton).set_Location(new Point(((Control)statRowPanel).get_Width() - ((Control)removeButton).get_Width() - 5, backgroundMargin));
			((Control)removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RemoveCustomStatProfit(stat2, services2);
				Panel obj = statRowPanel;
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
				CustomStatProfitTabView.ShowNoCustomStatProfitsExistHintIfNecessary(hintLabel2, model2);
			});
		}

		private static void OnCoinTextChanged(Stat stat, string goldText, string silverText, string copperText, Services services)
		{
			goldText = (string.IsNullOrWhiteSpace(goldText) ? "0" : goldText);
			silverText = (string.IsNullOrWhiteSpace(silverText) ? "0" : silverText);
			copperText = (string.IsNullOrWhiteSpace(copperText) ? "0" : copperText);
			int gold = int.Parse(goldText);
			int silver = int.Parse(silverText);
			int copper = int.Parse(copperText);
			stat.Profit.Unsigned_Custom_ProfitInCopper = 10000 * gold + 100 * silver + copper;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void RemoveCustomStatProfit(Stat stat, Services services)
		{
			stat.Profit.Unsigned_Custom_ProfitInCopper = null;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}
	}
}
