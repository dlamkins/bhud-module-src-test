using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class StatContainer : Container
	{
		private readonly StatTooltip _statTooltip;

		public StatContainer(Stat stat, PanelType panelType, SafeList<int> ignoredItemApiIds, SafeList<int> favoriteItemApiIds, SafeList<CustomStatProfit> customStatProfits, Services services)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			Stat stat2 = stat;
			SafeList<int> ignoredItemApiIds2 = ignoredItemApiIds;
			SafeList<int> favoriteItemApiIds2 = favoriteItemApiIds;
			SafeList<CustomStatProfit> customStatProfits2 = customStatProfits;
			Services services2 = services;
			((Container)this)._002Ector();
			int iconSize = (int)services2.SettingService.StatIconSizeSetting.get_Value();
			int iconMargin = 1;
			int backgroundSize = iconSize + 2 * iconMargin;
			int backgroundMargin = 1;
			int rarityBorderThickness = 2;
			int rarityBorderLength = backgroundSize;
			int rarityBorderLeftOrTopLocation = backgroundMargin;
			int rarityBorderRightOrBottomLocation = rarityBorderLeftOrTopLocation + rarityBorderLength - rarityBorderThickness;
			((Control)this).set_Size(new Point(backgroundSize + 2 * backgroundMargin));
			long? customStatProfitInCopper = customStatProfits2.ToListSafe().SingleOrDefault((CustomStatProfit c) => c.BelongsToStat(stat2))?.CustomProfitInCopper;
			AsyncTexture2D statIconTexture = GetStatIconTexture(stat2, services2);
			StatTooltip statTooltip = (_statTooltip = new StatTooltip(stat2, customStatProfitInCopper, statIconTexture, panelType, services2));
			Image val = new Image(services2.TextureService.InventorySlotBackgroundTexture);
			((Control)val).set_Tooltip((Tooltip)(object)statTooltip);
			((Control)val).set_Size(new Point(backgroundSize));
			((Control)val).set_Location(new Point(backgroundMargin));
			((Control)val).set_Parent((Container)(object)this);
			Image val2 = new Image(statIconTexture);
			((Control)val2).set_Tooltip((Tooltip)(object)statTooltip);
			((Control)val2).set_Opacity((stat2.Count > 0) ? 1f : ((float)services2.SettingService.NegativeCountIconOpacitySetting.get_Value() / 255f));
			((Control)val2).set_Size(new Point(iconSize));
			((Control)val2).set_Location(new Point(backgroundMargin + iconMargin));
			((Control)val2).set_Parent((Container)(object)this);
			Label val3 = new Label();
			val3.set_Text(stat2.Count.ToString());
			((Control)val3).set_Tooltip((Tooltip)(object)statTooltip);
			val3.set_Font(services2.FontService.Fonts[services2.SettingService.CountFontSizeSetting.get_Value()]);
			val3.set_TextColor((stat2.Count >= 0) ? services2.SettingService.PositiveCountTextColorSetting.get_Value().GetColor() : services2.SettingService.NegativeCountTextColorSetting.get_Value().GetColor());
			val3.set_HorizontalAlignment(services2.SettingService.CountHoritzontalAlignmentSetting.get_Value());
			((Control)val3).set_BackgroundColor(services2.SettingService.CountBackgroundColorSetting.get_Value().GetColor() * ((float)services2.SettingService.CountBackgroundOpacitySetting.get_Value() / 255f));
			val3.set_StrokeText(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Width(iconSize - 5);
			((Control)val3).set_Location(new Point(backgroundMargin + iconMargin, backgroundMargin + iconMargin + 1));
			((Control)val3).set_Parent((Container)(object)this);
			if (services2.SettingService.RarityIconBorderIsVisibleSetting.get_Value())
			{
				AddRarityBorder(stat2.Details.Rarity, rarityBorderLeftOrTopLocation, rarityBorderRightOrBottomLocation, rarityBorderThickness, rarityBorderLength, statTooltip);
			}
			if (panelType == PanelType.IgnoredItems)
			{
				return;
			}
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				StatContextMenuStrip contextMenuStrip = new StatContextMenuStrip(stat2, panelType, ignoredItemApiIds2, favoriteItemApiIds2, customStatProfits2, services2);
				((Control)contextMenuStrip).add_Hidden((EventHandler<EventArgs>)delegate
				{
					((Control)contextMenuStrip).Dispose();
				});
				((ContextMenuStrip)contextMenuStrip).Show(GameService.Input.get_Mouse().get_Position());
			});
		}

		private static AsyncTexture2D GetStatIconTexture(Stat stat, Services services)
		{
			return (AsyncTexture2D)(stat.Details.State switch
			{
				ApiStatDetailsState.GoldCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.GoldCoinTexture), 
				ApiStatDetailsState.SilveCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.SilverCoinTexture), 
				ApiStatDetailsState.CopperCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.CopperCoinTexture), 
				_ => services.TextureService.GetTextureFromAssetCacheOrFallback(stat.Details.IconAssetId), 
			});
		}

		private void AddRarityBorder(ItemRarity rarity, int borderLeftOrTopLocation, int borderRightOrBottomLocation, int borderThickness, int borderLength, StatTooltip tooltip)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			Color borderColor = ColorService.GetRarityBorderColor(rarity);
			new BorderContainer(new Point(borderLeftOrTopLocation), new Point(borderThickness, borderLength), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
			new BorderContainer(new Point(borderRightOrBottomLocation, borderLeftOrTopLocation), new Point(borderThickness, borderLength), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
			new BorderContainer(new Point(borderLeftOrTopLocation), new Point(borderLength, borderThickness), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
			new BorderContainer(new Point(borderLeftOrTopLocation, borderRightOrBottomLocation), new Point(borderLength, borderThickness), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
		}

		protected override void DisposeControl()
		{
			StatTooltip statTooltip = _statTooltip;
			if (statTooltip != null)
			{
				((Control)statTooltip).Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
