using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class CustomStatProfitRowPanel
	{
		public CustomStatProfitRowPanel(CustomStatProfit customStatProfit, Stat stat, HintLabel hintLabel, Model model, Services services, Container parent)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Expected O, but got Unknown
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Expected O, but got Unknown
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Expected O, but got Unknown
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			CustomStatProfit customStatProfit2 = customStatProfit;
			Services services2 = services;
			Model model2 = model;
			HintLabel hintLabel2 = hintLabel;
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
			((Control)val2).set_BasicTooltipText(stat.Details.Name);
			((Control)val2).set_Size(new Point(backgroundSize));
			((Control)val2).set_Parent((Container)(object)statRowPanel);
			Image val3 = new Image(services2.TextureService.GetTextureFromAssetCacheOrFallback(stat.Details.IconAssetId));
			((Control)val3).set_Location(new Point(backgroundMargin + iconMargin));
			((Control)val3).set_BasicTooltipText(stat.Details.Name);
			((Control)val3).set_Size(new Point(iconSize));
			((Control)val3).set_Parent((Container)(object)statRowPanel);
			Image statImage = val3;
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(((Control)statImage).get_Right() + 10, backgroundMargin));
			val4.set_Text(stat.Details.Name);
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Parent((Container)(object)statRowPanel);
			Label statNameLabel = val4;
			Coin coin = new Coin(customStatProfit2.CustomProfitInCopper);
			NumberTextBox numberTextBox = new NumberTextBox(6);
			((Control)numberTextBox).set_Location(new Point(((Control)statImage).get_Right() + 10, ((Control)statNameLabel).get_Bottom() + 5));
			((TextInputBase)numberTextBox).set_Text(coin.Gold.ToString());
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
			((TextInputBase)numberTextBox2).set_Text(coin.Silver.ToString());
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
			((TextInputBase)numberTextBox3).set_Text(coin.Copper.ToString());
			((Control)numberTextBox3).set_Width(35);
			((Control)numberTextBox3).set_Parent((Container)(object)statRowPanel);
			NumberTextBox copperTextBox = numberTextBox3;
			goldTextBox.NumberTextChanged += delegate
			{
				OnCoinTextChanged(((TextInputBase)goldTextBox).get_Text(), ((TextInputBase)silverTextBox).get_Text(), ((TextInputBase)copperTextBox).get_Text(), customStatProfit2, services2);
			};
			silverTextBox.NumberTextChanged += delegate
			{
				OnCoinTextChanged(((TextInputBase)goldTextBox).get_Text(), ((TextInputBase)silverTextBox).get_Text(), ((TextInputBase)copperTextBox).get_Text(), customStatProfit2, services2);
			};
			copperTextBox.NumberTextChanged += delegate
			{
				OnCoinTextChanged(((TextInputBase)goldTextBox).get_Text(), ((TextInputBase)silverTextBox).get_Text(), ((TextInputBase)copperTextBox).get_Text(), customStatProfit2, services2);
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
				RemoveCustomStatProfit(customStatProfit2, model2, services2);
				Panel obj = statRowPanel;
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
				CustomStatProfitTabView.ShowNoCustomStatProfitsExistHintIfNecessary(hintLabel2, model2);
			});
		}

		private static void OnCoinTextChanged(string goldText, string silverText, string copperText, CustomStatProfit customStatProfit, Services services)
		{
			goldText = (string.IsNullOrWhiteSpace(goldText) ? "0" : goldText);
			silverText = (string.IsNullOrWhiteSpace(silverText) ? "0" : silverText);
			copperText = (string.IsNullOrWhiteSpace(copperText) ? "0" : copperText);
			int gold = int.Parse(goldText);
			int silver = int.Parse(silverText);
			int copper = int.Parse(copperText);
			customStatProfit.CustomProfitInCopper = 10000 * gold + 100 * silver + copper;
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}

		private static void RemoveCustomStatProfit(CustomStatProfit customStatProfit, Model model, Services services)
		{
			model.CustomStatProfits.RemoveSafe(customStatProfit);
			services.UpdateLoop.TriggerUpdateUi();
			services.UpdateLoop.TriggerSaveModel();
		}
	}
}
